﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalContractMng.Domain;
using Application.Business.Erp.SupplyChain.Client.MaterialRentalMange;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalManage.MaterialRentalContractMng
{
    public partial class VMaterialRentalContractQuery : TBasicDataView
    {
        private MMatRentalMng model = new MMatRentalMng();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        public VMaterialRentalContractQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
            InitUsedRankType();
        }

        private IList list = new ArrayList();
        /// <summary>
        /// 保存结果
        /// </summary>
        virtual public IList List
        {
            get { return list; }
            set { list = value; }
        }

        private void InitData()
        {
          
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            txtSupply.SupplierCatCode = CommonUtil.SupplierCatCode3 + "-" + CommonUtil.SupplierCatCode4;
            projectInfo = StaticMethod.GetProjectInfo();
            btnAll.Checked = true;
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        private void InitUsedRankType()
        {
            //添加设备来源下拉框
            txtMaterial.DataSource = (Enum.GetNames(typeof(MaterialResource)));
            txtMaterialBill.DataSource = (Enum.GetNames(typeof(MaterialResource)));
            //VBasicDataOptr.InitUsedRankType(txtMaterial, false);
            //comSpecailType.ContextMenuStrip = cmsDg;
        }

        private void InitEvent()
        {
           // this.btnSearch.Click += new EventHandler(btnSearch_Click);  need fix
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetailBill.SelectionChanged+=new EventHandler(dgDetailBill_SelectionChanged);
            dgMaster.CellContentClick += new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            this.btnSelectCostSubject.Click += new EventHandler(btnSelectSubject_Click);
             btnBalanceSelect.Click += new EventHandler(btnBalanceSelect_Click);
        }

        //科目
        void btnSelectSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                this.txtCostSubject.Text = cost.Name;
                this.txtCostSubject.Tag = cost;
            }
        }
        //工程任务
        void btnBalanceSelect_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];

                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    txtBalance.Text = task.Name;
                    txtBalance.Tag = task;
                }
            }
        }
        

        void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name ==colCodeBill.Name)
            {
                MaterialRentalContractMaster master = dgMaster.Rows[e.RowIndex].Tag as MaterialRentalContractMaster;
                if (master.DocState != DocumentState.InExecute)
                {
                    MessageBox.Show("租赁结算单[" + master.Code + "]未审核完，不能打印！");
                    return;
                }
                DateTime lasttime=ClientUtil.ToDateTime(this.dtpDateEndBill.Value);
               // VMaterialRentalReport vmrr = new VMaterialRentalReport(master,lasttime);  need fix
               // vmrr.ShowDialog(); need fix
            }
        }

        /// <summary>
        /// 分科目费用列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colSubject.Name))
            {
                string strId = ClientUtil.ToString(dgDetail.Rows[e.RowIndex].Cells[colDetailId.Name].Value);
                list = model.MatMngSrv.GetMaterialSubjectByParentId(strId);
              //  VMaterialSubjectSelector MaterialSubject = new VMaterialSubjectSelector();  need fix
                //  MaterialSubject.Result = list;    need fix
                //  MaterialSubject.ShowDialog();  need fix
            }
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

        /// <summary>
        /// 主从表显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchBill_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            //单据
            if (txtCodeBeginBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, NHibernate.Criterion.MatchMode.Anywhere));
            }
            //业务时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //创建人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            ////设备
            //if (this.txtMaterialBill.SelectedItem != null)
            //{
            //    objectQuery.AddCriterion(Expression.Like("MaterialSource", txtMaterialBill.Text, MatchMode.Anywhere));
            //}

            //供货商
            if (!txtSupplyBill.Text.Trim().Equals("") && txtSupplyBill.Result != null)
            {
                objectQuery.AddCriterion(Expression.Like("SupplierName", txtSupplyBill.Text, MatchMode.Anywhere)); ;
            }
            ////状态
            //if (comMngType.Text != "")
            //{
            //    System.Web.UI.WebControls.ListItem li = comMngType.SelectedItem as System.Web.UI.WebControls.ListItem;
            //    int values = ClientUtil.ToInt(li.Value);
            //objectQuery.AddCriterion(Expression.Eq("DocState", (DocumentState)values));
            //}
         
            try
            {
              //  list = model.MatMngSrv.GetMaterialRentalContract(objectQuery);  need fix
                dgDetail.Rows.Clear();
                dgMaster.Rows.Clear();
                dgDetailBill.Rows.Clear();
                dgDetailBills.Rows.Clear();
                ShowMasterList(list);
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            decimal sumQuantitys = 0;
            foreach (MaterialRentalContractMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code;
                dgMaster[colSupplierBill.Name, rowIndex].Value = master.SupplierName;   //供应商名称
                dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDate.ToShortDateString();   //业务日期
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;//制单人
                dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate;//制单日期
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);

                sumQuantitys += master.SumMoney;
                dgMaster[colSunTotleMoney.Name, rowIndex].Value = master.SumMoney;

            }
            lblRecordTotalBill.Text = "共【" + dgMaster.Rows.Count + "】条记录";
            this.txtSumQuantityBill.Text = sumQuantitys.ToString("#,###.####");
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetailBill.Rows.Clear();
            MaterialRentalContractMaster master = dgMaster.CurrentRow.Tag as MaterialRentalContractMaster;
            if (master == null) return;
            foreach (MaterialRentalContractDetail dtl in master.Details)
            {
                int rowIndex = dgDetailBill.Rows.Add();
                dgDetailBill[colMaterialCodeBill.Name, rowIndex].Value = dtl.MaterialCode;                            //设备编码
                dgDetailBill[colMaterialNameBill.Name, rowIndex].Value = dtl.MaterialName;                          //设备名称
                dgDetailBill[colMaterialSepBill.Name, rowIndex].Value = dtl.MaterialSpec;                               //规格型号
                dgDetailBill[colSettleQuantityBill.Name, rowIndex].Value = dtl.Quantity;                                        //结算数量
                dgDetailBill[colSettleMoneyBill.Name, rowIndex].Value = dtl.SettleMoney;                                     //结算金额
                //string strId = ClientUtil.ToString(dtl["Id"]);
                //list = model.MatMngSrv.GetMaterialSubjectByParentId(strId);
                //if (list.Count == 0)
                //{
                //    dgDetailBill[colSubjectBill.Name, rowIndex].Value = "无费用信息";
                //}
                //else
                //{
                //    dgDetailBill[colSubjectBill.Name, rowIndex].Value = "有费用信息";
                //}
                ////dgDetailBill[colSubjectBill.Name, rowIndex].Value = dtl.Money;  
                //dgDetailBill[colSubjectBill.Name,rowIndex].Value=dtl.//分科目费用
                dgDetailBill[colAuditPersonBill.Name, rowIndex].Value = master.HandlePersonName;                          //审核人
                dgDetailBill[colAuditDateBill.Name, rowIndex].Value = master.RealOperationDate.ToShortDateString();                                      //审核时间
                dgDetailBill[colDescriptionBill.Name, rowIndex].Value = dtl.Descript;                            //备注      
                dgDetailBill.Rows[rowIndex].Tag = dtl;
            }
            dgDetailBill.CurrentCell = dgDetailBill[1, 0];
            dgDetailBill_SelectionChanged(dgDetailBill, new EventArgs());
        }
        /// <summary>
        /// 明细变化，明细的明细跟随变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetailBill_SelectionChanged(object sender, EventArgs e)
        {
            dgDetailBills.Rows.Clear();
            MaterialRentalContractDetail detail = dgDetailBill.CurrentRow.Tag as MaterialRentalContractDetail;
            if (detail == null) return;
            //foreach (MaterialSubjectDetail dtl in detail.MaterialSubjectDetails)
            //{
            //    int rowIndex = dgDetailBills.Rows.Add();
            //    dgDetailBills[colCostName.Name, rowIndex].Value = dtl.CostName;
            //    dgDetailBills[colSettleSubject.Name, rowIndex].Tag = dtl.SettleSubject;
            //    dgDetailBills[colSettleSubject.Name, rowIndex].Value = dtl.SettleSubjectName;
            //    dgDetailBills[colMaterialCodeDg.Name, rowIndex].Value = dtl.MaterialTypeName;
            //    dgDetailBills[colMaterialCodeDg.Name, rowIndex].Tag = dtl.MaterialResource;
            //    dgDetailBills[colQuantity.Name, rowIndex].Value = dtl.SettleQuantity;
            //    dgDetailBills[colQuantityUnit.Name, rowIndex].Value = dtl.QuantityUnitName;
            //    dgDetailBills[colRentalDate.Name, rowIndex].Value = dtl.SettleDate;
            //    dgDetailBills[colDateUnit.Name, rowIndex].Value = dtl.DateUnitName;
            //    dgDetailBills[colDateUnit.Name, rowIndex].Tag = dtl.DateUnit;
            //    dgDetailBills[colRentalPrice.Name, rowIndex].Value = dtl.SettlePrice;
            //    dgDetailBills[colSettleMoneyDg.Name, rowIndex].Value = dtl.SettleMoney;
            //}
        }
    }
}