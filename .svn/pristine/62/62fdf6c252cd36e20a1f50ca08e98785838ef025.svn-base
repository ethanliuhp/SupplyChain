using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialCollectionMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialBalanceMng.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange.MaterialCollection
{
    public partial class VMaterialMonthlyBalance : TBasicDataView
    {
        MMatRentalMng model = new MMatRentalMng();
        MaterialBalanceMaster matBalanceMaster;
        //上期结存
        private MaterialBalanceMaster prophaseMatUnusedBal;
        private CurrentProjectInfo ProjectInfo;

        public VMaterialMonthlyBalance()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            dtpEndDate.Value = ConstObject.TheLogin.LoginDate.AddDays(1);
            txtYear.Text = ConstObject.TheLogin.TheComponentPeriod.NowYear.ToString();
            txtMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode1;
            ProjectInfo = StaticMethod.GetProjectInfo();
        }

        private void InitEvent()
        {
            this.btnMatReckoning.Click += new EventHandler(btnMatReckoning_Click);
            btnMatUnReckoning.Click += new EventHandler(btnMatUnReckoning_Click);
            txtSupply.TextChanged += new EventHandler(txtSupply_TextChanged);
            this.btnSelectGWBS.Click += new EventHandler(btSelectGWBS_Click);
        }

        void btSelectGWBS_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            if (txtAccountRootNode.Tag != null)
            {
                frm.DefaultSelectedGWBS = txtAccountRootNode.Tag as GWBSTree;
            }
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtAccountRootNode.Text = task.Name;
                    txtAccountRootNode.Tag = task;
                }
            }
        }
        void txtSupply_TextChanged(object sender, EventArgs e)
        {
            if (txtSupply.Result.Count > 0)
            {
                ////判断该料具商是否第一次结账
                //if (model.MatMngSrv.CheckIsNotFirstReckoning(txtSupply.Result[0] as SupplierRelationInfo) == false)
                //{
                //    this.dtpBeginDate.Enabled = true;
                //    this.dtpEndDate.Enabled = true;
                //}
                ////查询该料具商上次结算的结束日期，然后设置此次结算的开始日期
                //int fiscalYear = Convert.ToInt32(txtYear.Text);
                //int fiscalMonth = Convert.ToInt32(txtMonth.Text);
                //SupplierRelationInfo theSupplier = txtSupply.Result[0] as SupplierRelationInfo;
                //MaterialBalanceMaster master = model.MatMngSrv.GetPrrviousMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier);
                //if (master != null)
                //{
                //    dtpBeginDate.Value = master.EndDate.AddDays(1);
                //}
            }
        }

        //料具月结
        void btnMatReckoning_Click(object sender, EventArgs e)
        {
            GWBSTree task = this.txtAccountRootNode.Tag as GWBSTree;
            if (txtSupply.Text == "")
            {
                MessageBox.Show("请选择料具商！");
                return;
            }
            if (ClientUtil.ToDecimal(this.txtExtSumMoney.Text) != 0 && task == null)
            {
                MessageBox.Show("请选择料具调整费用对应的部位！");
                return;
            }
            this.btnMatReckoning.Enabled = false;
            int fiscalYear = Convert.ToInt32(txtYear.Text);
            int fiscalMonth = Convert.ToInt32(txtMonth.Text);
            SupplierRelationInfo theSupplier = txtSupply.Result[0] as SupplierRelationInfo;

            //生成商务数据
            //model.MatMngSrv.CreateMatSetBalInfoByYearAndMonth(2012, 7, theSupplier, ProjectInfo);
            //return;

            if (model.MatMngSrv.CheckIsNotFirstReckoning(theSupplier, ProjectInfo) == true)
            {
                //业务结束日期必须大于上次月结的结束日期
                MaterialBalanceMaster master = model.MatMngSrv.GetPrrviousMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);
                if (master != null)
                {
                    DateTime LastBalEndData = master.EndDate.Date;
                    DateTime BalEndData = Convert.ToDateTime(dtpEndDate.Value).Date;
                    if (LastBalEndData >= BalEndData)
                    {
                        if (LastBalEndData > Convert.ToDateTime(dtpEndDate.Text))
                        {
                            MessageBox.Show("当前结算结束日期必须大于上次结算的的结束日期" + LastBalEndData.ToString() + "");
                            return;
                        }
                    }

                }
                //判断当前料具商当月是否已经结账：如果已经结账则不能再次结账
                if (model.MatMngSrv.CheckReckoningCurrentMonth(fiscalYear, fiscalMonth, theSupplier, ProjectInfo))
                {
                    MessageBox.Show("当前月已经结账，不能重复结账！");
                    return;
                }

                //判断当前会计年月是否能结账
                if (model.MatMngSrv.CheckReckoning(fiscalYear, fiscalMonth, theSupplier, ProjectInfo) == false)
                {
                    MessageBox.Show("上一会计期未结账，当前会计期不能结账");
                    return;
                }
            }
            else
            {
                //第一次结账
            }
            FlashScreen.Show("正在进行料具租赁月结算...");
            try
            {
                
                model.MatMngSrv.MaterialReckoning(dtpEndDate.Value.Date, fiscalYear, fiscalMonth, theSupplier, ProjectInfo, ClientUtil.ToDecimal(this.txtExtSumMoney.Text), task);

            }
            catch (Exception e1)
            {
                throw new Exception("料具租赁月结算异常！");
            }
            finally
            {
                FlashScreen.Close();
            }
            
            MessageBox.Show("结账完成！");

        }
        //反结账
        void btnMatUnReckoning_Click(object sender, EventArgs e)
        {
            if (txtSupply.Text == "")
            {
                MessageBox.Show("请选择料具商！");
                return;
            }
            DialogResult dr = MessageBox.Show("是否反结本月数据？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.No) return;

            this.btnMatUnReckoning.Enabled = false;
            int fiscalYear = Convert.ToInt32(txtYear.Text);
            int fiscalMonth = Convert.ToInt32(txtMonth.Text);
            SupplierRelationInfo theSupplier = txtSupply.Result[0] as SupplierRelationInfo;
            //判断当前会计年月是否能反结账
            if (model.MatMngSrv.CheckUnReckoning(fiscalYear, fiscalMonth, theSupplier, ProjectInfo) == true)
            {
                MessageBox.Show("下一会计期已结账，当前会计期不能反结账");
                return;
            }
            //判断当前料具商当月是否结账：如果未结账则不能反结账
            if (model.MatMngSrv.CheckReckoningCurrentMonth(fiscalYear, fiscalMonth, theSupplier, ProjectInfo) == false)
            {
                MessageBox.Show("当前月未结账，不能反结账！");
                return;
            }
            string sMsg = string.Empty;
            if (model.MatMngSrv.IsAccount(fiscalYear, fiscalMonth, theSupplier, ProjectInfo, ref sMsg))
            {

                if (!string.IsNullOrEmpty(sMsg))
                {
                    MessageBox.Show(sMsg);
                }
                return;
            }
           
            FlashScreen.Show("正在进行料具租赁反结算...");
            try
            {
                model.MatMngSrv.MaterialUnReckoning(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);
                FlashScreen.Close();
                MessageBox.Show("反结完成！");
            }
            catch (Exception e1)
            {
                FlashScreen.Close();
                throw new Exception("料具租赁反结算异常！");
            }
            finally
            {
                FlashScreen.Close();
            }
            
            txtSupply.Result.Clear();
            txtSupply.Tag = null;
            txtSupply.Text = "";
        }
    }
}

