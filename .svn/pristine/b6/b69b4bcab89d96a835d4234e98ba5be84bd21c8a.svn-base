using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireBalance.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.CommonControl;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireOrder;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireCollection
{
    public partial class VMaterialHireMonthlyBalance : TBasicDataView
    {
        MMaterialHireMng model = new MMaterialHireMng();
        MatHireBalanceMaster matBalanceMaster;
        MatHireOrderMaster OrderMaster = null;
        //上期结存
        private MatHireBalanceMaster prophaseMatUnusedBal;
        //private CurrentProjectInfo ProjectInfo;

        public VMaterialHireMonthlyBalance()
        {
            InitializeComponent();
            InitData();
            InitEvent();
        }

        private void InitData()
        {
            for (int iYear = 2010; iYear <= DateTime.Now.Year+1; iYear++)
            {
                txtYear.Items.Insert(txtYear.Items.Count,iYear);
            }
            for (int iMonth = 1; iMonth < 13; iMonth++)
            {
                txtMonth.Items.Insert(txtMonth.Items.Count,iMonth);
            }
                
            txtYear.Text = ConstObject.TheLogin.TheComponentPeriod.NowYear.ToString();
            txtMonth.Text = ConstObject.TheLogin.TheComponentPeriod.NowMonth.ToString();
            dtpEndDate.Value =new DateTime (ConstObject.TheLogin.TheComponentPeriod.NowYear,ConstObject.TheLogin.TheComponentPeriod.NowMonth,1);
            //txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode1;
            this.btnForward.Click+=new EventHandler(btnForward_Click);
            SetControlState();
        }
        private void InitEvent()
        {
            this.btnMatReckoning.Click += new EventHandler(btnMatReckoning_Click);
            btnMatUnReckoning.Click += new EventHandler(btnMatUnReckoning_Click);
            this.txtChangeMoney.tbTextChanged += new EventHandler(txtChangeMoney_tbTextChanged);
          txtYear.SelectedIndexChanged+=new EventHandler(txtYear_SelectedIndexChanged);
            txtMonth.SelectedIndexChanged+=new EventHandler(txtMonth_SelectedIndexChanged);
            //txtSupply.TextChanged += new EventHandler(txtSupply_TextChanged);

            //this.TenantSelector.TenantSelectorAfterEvent += new UcTenantSelector.TenantSelectorAfterEventHandler(TenantSelectorAfter);

        }
        void txtYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime date = dtpEndDate.Value;
            date = new DateTime(ClientUtil.ToInt(txtYear.Text),date.Month,date.Day);
            dtpEndDate.Value = date;
            SetControlState();
        }
        void txtMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime date = dtpEndDate.Value;
            date = new DateTime(date.Year, ClientUtil.ToInt(txtMonth.Text), date.Day);
            dtpEndDate.Value = date;
            SetControlState();
        }
        private void btnForward_Click( object sender ,EventArgs e)
        {
            VMaterialHireOrderSelector oVMaterialHireOrderSelector = new VMaterialHireOrderSelector(EnumMatHireType.其他);
            oVMaterialHireOrderSelector.ShowDialog();
            if (oVMaterialHireOrderSelector.Result != null && oVMaterialHireOrderSelector.Result.Count > 0)
            {
                OrderMaster = oVMaterialHireOrderSelector.Result[0] as MatHireOrderMaster;
                #region 数据填充
                txtSupply.Text = OrderMaster.SupplierName;
                txtSupply.Tag = OrderMaster.TheSupplierRelationInfo;
                txtContract.Tag = OrderMaster;
                txtContract.Text = OrderMaster.Code;
                txtProjectName.Text = OrderMaster.ProjectName;
                txtProjectName.Tag = OrderMaster.ProjectId;
                //this.ProjectInfo = new CurrentProjectInfo() { Id = OrderMaster.ProjectId, Name = OrderMaster.ProjectName };
                #endregion
            }
            SetControlState();
        }
        
        void txtChangeMoney_tbTextChanged(object sender, EventArgs e)
        {
            bool validity = true;
            string changeMoney = this.txtChangeMoney.Text;
            validity = CommonMethod.VeryValid(changeMoney);
            if (validity == false)
            {
                MessageBox.Show("请输入数字！");
                txtChangeMoney.Focus();
                return;
            }
        }
       


        //料具月结
        void btnMatReckoning_Click(object sender, EventArgs e)
        {

            if (OrderMaster==null|| string.IsNullOrEmpty(txtContract.Text) || txtContract.Tag==null)
            {
                ShowMessage("请选择租赁合同");
                return;
            }
            int fiscalYear = Convert.ToInt32(txtYear.Text);
            int fiscalMonth = Convert.ToInt32(txtMonth.Text);
            SupplierRelationInfo theSupplier = txtSupply.Tag as SupplierRelationInfo;

            //生成商务数据
            //model.MatMngSrv.CreateMatSetBalInfoByYearAndMonth(2012, 7, theSupplier, ProjectInfo);
            //return;
            CurrentProjectInfo ProjectInfo = new CurrentProjectInfo() { Name = OrderMaster.ProjectName, Id = OrderMaster.ProjectId };
            if (model.MaterialHireMngSvr.CheckIsNotFirstReckoning(theSupplier, ProjectInfo) == true)
            {
                //业务结束日期必须大于上次月结的结束日期
                MatHireBalanceMaster master = model.MaterialHireMngSvr.GetPrrviousMatBalanceMaster(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);
                if (master != null)
                {
                    DateTime LastBalEndData = master.EndDate.Date;
                    DateTime BalEndData = Convert.ToDateTime(dtpEndDate.Value).Date;
                    if (LastBalEndData >= BalEndData)
                    {
                        if (LastBalEndData > Convert.ToDateTime(dtpEndDate.Text))
                        {
                            this.ShowMessage("当前结算结束日期必须大于上次结算的的结束日期" + LastBalEndData.ToString() + "");
                            return;
                        }
                    }

                }
                //判断当前料具商当月是否已经结账：如果已经结账则不能再次结账
                if (model.MaterialHireMngSvr.CheckReckoningCurrentMonth(fiscalYear, fiscalMonth, theSupplier, ProjectInfo))
                {
                    this.ShowMessage("当前月已经结账，不能重复结账！");
                    return;
                }

                //判断当前会计年月是否能结账
                if (model.MaterialHireMngSvr.CheckReckoning(fiscalYear, fiscalMonth, theSupplier, ProjectInfo) == false)
                {
                    this.ShowMessage("上一会计期未结账，当前会计期不能结账");
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
                
                model.MaterialHireMngSvr.MaterialReckoning(dtpEndDate.Value.Date, fiscalYear, fiscalMonth, theSupplier, ProjectInfo,ClientUtil.ToDecimal(txtChangeMoney.Text));

            }
            catch (Exception e1)
            {
                throw new Exception("料具租赁月结算异常！");
            }
            finally
            {
                SetControlState();
                FlashScreen.Close();
                System.Threading.Thread.Sleep(1);
            }

            this.ShowMessage("结账完成！");

        }
        //反结账
        void btnMatUnReckoning_Click(object sender, EventArgs e)
        {
            if (OrderMaster == null || string.IsNullOrEmpty(txtContract.Text) || txtContract.Tag == null)
            {
                ShowMessage("请选择租赁合同");
            }
            DialogResult dr = MessageBox.Show("是否反结本月数据？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.No) return;
            int fiscalYear = Convert.ToInt32(txtYear.Text);
            int fiscalMonth = Convert.ToInt32(txtMonth.Text);
            SupplierRelationInfo theSupplier = txtSupply.Tag as SupplierRelationInfo;
            //判断当前会计年月是否能反结账
            CurrentProjectInfo ProjectInfo = new CurrentProjectInfo() { Name = OrderMaster.ProjectName, Id = OrderMaster.ProjectId };
            if (model.MaterialHireMngSvr.CheckUnReckoning(fiscalYear, fiscalMonth, theSupplier, ProjectInfo) == true)
            {
                MessageBox.Show("下一会计期已结账，当前会计期不能反结账");
                return;
            }
            //判断当前料具商当月是否结账：如果未结账则不能反结账
            if (model.MaterialHireMngSvr.CheckReckoningCurrentMonth(fiscalYear, fiscalMonth, theSupplier, ProjectInfo) == false)
            {
                MessageBox.Show("当前月未结账，不能反结账！");
                return;
            }
            string sMsg = string.Empty;
            //if (model.MaterialHireMngSvr.IsAccount(fiscalYear, fiscalMonth, theSupplier, ProjectInfo, ref sMsg))
            //{

            //    if (!string.IsNullOrEmpty(sMsg))
            //    {
            //        MessageBox.Show(sMsg);
            //    }
            //    return;
            //}
           
            FlashScreen.Show("正在进行料具租赁反结算...");
            try
            {
                model.MaterialHireMngSvr.MaterialUnReckoning(fiscalYear, fiscalMonth, theSupplier, ProjectInfo);
                FlashScreen.Close();
                System.Threading.Thread.Sleep(1);
                MessageBox.Show("反结完成！");
            }
            catch (Exception e1)
            {
                FlashScreen.Close();
                System.Threading.Thread.Sleep(1);
                throw new Exception("料具租赁反结算异常！");
            }
            finally
            {
                SetControlState();
            }
        }
        //public MatHireOrderMaster GetOrderMaster()
        //{
        //    ObjectQuery oQuery = new ObjectQuery();
        //    oQuery.AddCriterion(Expression.Eq("TheSupplierRelationInfo", this.txtSupply.Tag as SupplierRelationInfo));
        //    oQuery.AddCriterion(Expression.Eq("ProjectId", this.ProjectInfo.Id));
        //    IList list = model.MaterialHireMngSvr.GetMaterialHireOrder(oQuery) as IList;
        //    return list == null || list.Count == 0 ? null : list[0] as MatHireOrderMaster;
        //}
        public void ShowMessage(string sMessage)
        {
            MessageBox.Show(sMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public void SetControlState()
        {
            if (OrderMaster != null)
            {
                int fiscalYear = Convert.ToInt32(txtYear.Text);
                int fiscalMonth = Convert.ToInt32(txtMonth.Text);
                SupplierRelationInfo theSupplier = txtSupply.Tag as SupplierRelationInfo;
                CurrentProjectInfo ProjectInfo = new CurrentProjectInfo() { Name = OrderMaster.ProjectName, Id = OrderMaster.ProjectId };
                if (model.MaterialHireMngSvr.CheckReckoningCurrentMonth(fiscalYear, fiscalMonth, theSupplier, ProjectInfo))
                {
                    btnMatReckoning.Enabled = false;
                    btnMatUnReckoning.Enabled = true;
                }
                else
                {
                    btnMatReckoning.Enabled = true;
                    btnMatUnReckoning.Enabled = false;
                }

            }
            else
            {
                btnMatReckoning.Enabled = btnMatUnReckoning.Enabled = false;
            }
        }
         
    }
}

