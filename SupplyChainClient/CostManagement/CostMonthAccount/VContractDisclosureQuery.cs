using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using System.Collections;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate.Criterion;
using NHibernate;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VContractDisclosureQuery  : TBasicDataView
    {
        private MCostMonthAccount model = new MCostMonthAccount();
        private CurrentProjectInfo projectInfo = null;
        public VContractDisclosureQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }
        private void InitData()
        {
            //this.dtpDateBegin.Value = ConstObject.TheLogin.TheComponentPeriod.BeginDate;
            this.dtpDateBegin.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.TheLogin.LoginDate;
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            dgDetailBill.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                btnSelectProjectBill.Visible=  this.btnSelectProject.Visible  = true;
            }
            else
            {
                txtProName.Tag =txtProNameBill.Tag = projectInfo;
                txtProName.Text = txtProNameBill.Text = projectInfo.Name;
                btnSelectProjectBill.Visible =this.btnSelectProject.Visible = false;
            }
 
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }       

        private void InitEvent()
        {
           
            //this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSelectProjectBill.Click += new EventHandler(btnSelectProject_Click);
            this.btnSelectProject.Click += new EventHandler(btnSelectProject_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            this.dgMaster.CellContentClick+=new DataGridViewCellEventHandler(dgMaster_CellContentClick);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
           
        }
        public void dgMaster_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            if (dgvCell.OwningColumn.Name ==this.colCodeBill.Name   )
            {
                if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString()) || dgMaster.Rows[e.RowIndex].Tag ==null) return;
                DisclosureMaster oMaster = dgMaster.Rows[e.RowIndex].Tag as DisclosureMaster;
                VContractDisclosurePrint oPrint = new VContractDisclosurePrint(oMaster);
                oPrint.StartPosition = FormStartPosition.CenterParent;
                oPrint.ShowDialog();
            }
        }
        void btnSelectProject_Click(object sender, EventArgs e)
        {
            VSelectProjectInfo project = new VSelectProjectInfo();
            CurrentProjectInfo extProject = new CurrentProjectInfo();
            project.ListExtendProject.Add(extProject);
            project.ShowDialog();

            if (project.Result != null && project.Result.Count > 0)
            {
                CurrentProjectInfo selectProject = project.Result[0] as CurrentProjectInfo;
                if (selectProject != null)
                {
                    if (sender == btnSelectProjectBill)
                    {
                        txtProNameBill.Text = selectProject.Name;
                        txtProNameBill.Tag = selectProject;
                    }
                    else if (sender == btnSelectProject)
                    {
                        txtProName.Text = selectProject.Name;
                        txtProName.Tag = selectProject;
                    }
                }
            }
        }
      
        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }

       

        void btnSearch_Click(object sender, EventArgs e)
        {
            
            ObjectQuery oQuery = new ObjectQuery();
            if (!string.IsNullOrEmpty(this.txtCodeBegin.Text))
            {
                oQuery.AddCriterion(Expression.Like("Master.Code", txtCodeBegin.Text));
            }
            if (txtProNameBill.Tag != null)
            {
                CurrentProjectInfo oProject = txtProName.Tag as CurrentProjectInfo;
                oQuery.AddCriterion(Expression.Eq("Master.ProjectId", oProject.Id));
            }
            if (dtpDateBeginBill.Value <= dtpDateEndBill.Value)
            {
                oQuery.AddCriterion(Expression.Between("Master.CreateDate", dtpDateBegin.Value, dtpDateEnd.Value));
            }
            oQuery.AddFetchMode("Master", FetchMode.Eager);
            IList lst = model.CostMonthAccSrv.Query(typeof(DisclosureDetail), oQuery);
            ShowDetail(lst);
          
        }
        public void ShowDetail(IList lst)
        {
            int iRowIndex = 0;
            dgDetail.Rows.Clear();
            DisclosureMaster oMaster = null;
            foreach (DisclosureDetail oDetail in lst)
            {
               // colCodeBill colProjectNameBill colContractNameBill 
//colBearerOrgNameBill colCreateDateBill colStateBill colCreatePersonBill colRealOperationDateBill 
                //colDescriptBill colContractTypeBill colContractInterimMoneyBill合同价格 
                //colQualityBreachDutyBill质量责任及违约责任 colDurationBreachDutyBill 工程目标及违约责任
                //colSafetyBreachDutyBill安全目标及违约责任 colCivilizedBreachDutyBill文明施工及违约责任 
                //colLaborDemandBill劳动力需求 colMaterialDemandBill主要材料需求 colPaymentTypeBill 付款方式
                //colWarrantyDateMoneyBill保修期及保修金 colOtherDescBill其他说明事项
                oMaster =oDetail.Master as DisclosureMaster;
                iRowIndex = dgDetail.Rows.Add();
                dgDetail[colCode.Name, iRowIndex].Value =oMaster.Code;
                dgDetail[colProjectName.Name, iRowIndex].Value = oMaster.ProjectName;
                dgDetail[colContractName.Name, iRowIndex].Value = oMaster.ContractName;
                dgDetail[colBearerOrgName.Name, iRowIndex].Value = oMaster.BearerOrgName;
                dgDetail[colCreateDate.Name, iRowIndex].Value = oMaster.CreateDate.ToString("yyyy-MM-dd");
                dgDetail[colState.Name, iRowIndex].Value =ClientUtil.GetDocStateName((int) oMaster.DocState);
                dgDetail[colCreatePerson.Name, iRowIndex].Value = oMaster.CreatePersonName;
                dgDetail[colRealOperationDate.Name, iRowIndex].Value = oMaster.RealOperationDate.ToString("yyyy-MM-dd");
                dgDetail[colDescript.Name, iRowIndex].Value = oMaster.Descript;
                dgDetail[colContractType.Name, iRowIndex].Value = oDetail.ContractType;
                dgDetail[colContractInterimMoney.Name, iRowIndex].Value = oDetail.ContractInterimMoney;
                dgDetail[colQualityBreachDuty.Name, iRowIndex].Value = oDetail.QualityBreachDuty;
                dgDetail[colDurationBreachDuty.Name, iRowIndex].Value = oDetail.DurationBreachDuty;
                dgDetail[colSafetyBreachDuty.Name, iRowIndex].Value = oDetail.SafetyBreachDuty;
                dgDetail[colCivilizedBreachDuty.Name, iRowIndex].Value = oDetail.CivilizedBreachDuty;
                dgDetail[colLaborDemand.Name, iRowIndex].Value = oDetail.LaborDemand;
                dgDetail[colMaterialDemand.Name, iRowIndex].Value = oDetail.MaterialDemand;
                dgDetail[colPaymentType.Name, iRowIndex].Value = oDetail.PaymentType;
                dgDetail[colWarrantyDateMoney.Name, iRowIndex].Value = oDetail.WarrantyDateMoney;
                dgDetail[colOtherDesc.Name, iRowIndex].Value = oDetail.OtherDesc;
            }
        }
        /// <summary>
        /// 主从表显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearchBill_Click(object sender, EventArgs e)
        {
            
            ObjectQuery oQuery = new ObjectQuery();
            if (!string.IsNullOrEmpty(txtCodeBill.Text))
            {
                oQuery.AddCriterion(Expression.Like("Code", txtCodeBill.Text));
            }
            if (txtProNameBill.Tag != null)
            {
                CurrentProjectInfo oProject=txtProNameBill.Tag as CurrentProjectInfo;
                oQuery.AddCriterion(Expression.Eq("ProjectId", oProject.Id));
            }
            if (dtpDateBeginBill.Value <= dtpDateEndBill.Value)
            {
                oQuery.AddCriterion(Expression.Between("CreateDate", dtpDateBeginBill.Value, dtpDateEndBill.Value));
            }
            oQuery.AddFetchMode("Details", FetchMode.Eager);
            IList lst = model.CostMonthAccSrv.Query(typeof(DisclosureMaster), oQuery);
            ShowMasterList(lst);
        }
        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            int iRowIndex=0;
            dgMaster.Rows.Clear();
            if (masterList != null)
            {
                foreach (DisclosureMaster oMaster in masterList)
                {
                    //colCodeBill colProjectNameBill colContractNameBill 
                    //colBearerOrgNameBill colCreateDateBill 
                    //colStateBill colCreatePersonBill colRealOperationDateBill colDescriptBill
                    iRowIndex=dgMaster.Rows.Add();
                    dgMaster.Rows[iRowIndex].Tag = oMaster;
                    dgMaster[this.colCodeBill.Name, iRowIndex].Value = oMaster.Code;
                    dgMaster[this.colProjectNameBill.Name, iRowIndex].Value = oMaster.ProjectName;
                    dgMaster[this.colContractNameBill.Name, iRowIndex].Value = oMaster.ContractName;
                    dgMaster[this.colBearerOrgNameBill.Name, iRowIndex].Value = oMaster.BearerOrgName;
                    dgMaster[this.colCreateDateBill.Name, iRowIndex].Value = oMaster.CreateDate.ToString("yyyy-MM-dd");
                    dgMaster[this.colStateBill.Name, iRowIndex].Value =ClientUtil.GetDocStateName((int) oMaster.DocState);
                    dgMaster[this.colCreatePersonBill.Name, iRowIndex].Value = oMaster.CreatePersonName;
                    dgMaster[this.colRealOperationDateBill.Name, iRowIndex].Value = oMaster.RealOperationDate.ToString("yyyy-MM-dd");
                    dgMaster[this.colDescriptBill.Name, iRowIndex].Value = oMaster.Descript;
                }
            }

            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.Rows[0].Selected = true;
                dgMaster_SelectionChanged(dgMaster,null);
            }
           
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            int iRowIndex=0;
            dgDetailBill.Rows.Clear();
            if (dgMaster.CurrentRow != null )
            {
                DataGridViewRow oRow = dgMaster.CurrentRow;
                if (oRow.Tag != null)
                {
                    DisclosureMaster oMaster = oRow.Tag as DisclosureMaster;
                    foreach (DisclosureDetail oDetail in oMaster.Details)
                    {
                        //colContractTypeBill colContractInterimMoneyBill colQualityBreachDutyBill
                        //colDurationBreachDutyBill colSafetyBreachDutyBill colCivilizedBreachDutyBill 
                        //colLaborDemandBill
                        //colMaterialDemandBill colPaymentTypeBill colWarrantyDateMoneyBill colOtherDescBill
                        iRowIndex = dgDetailBill.Rows.Add();
                       // dgDetailBill[colContractTypeBill.Name, iRowIndex].Value = oDetail.ContractType;
                        dgDetailBill[colContractInterimMoneyBill.Name, iRowIndex].Value = oDetail.ContractInterimMoney.ToString("N2");
                        dgDetailBill[colQualityBreachDutyBill.Name, iRowIndex].Value = oDetail.QualityBreachDuty;
                        dgDetailBill[colDurationBreachDutyBill.Name, iRowIndex].Value = oDetail.DurationBreachDuty;
                        dgDetailBill[colSafetyBreachDutyBill.Name, iRowIndex].Value = oDetail.SafetyBreachDuty;
                        dgDetailBill[colCivilizedBreachDutyBill.Name, iRowIndex].Value = oDetail.CivilizedBreachDuty;
                        dgDetailBill[colLaborDemandBill.Name, iRowIndex].Value = oDetail.LaborDemand;
                        dgDetailBill[colMaterialDemandBill.Name, iRowIndex].Value = oDetail.MaterialDemand;
                        dgDetailBill[colPaymentTypeBill.Name, iRowIndex].Value = oDetail.PaymentType;
                        dgDetailBill[colWarrantyDateMoneyBill.Name, iRowIndex].Value = oDetail.WarrantyDateMoney;
                        dgDetailBill[colOtherDescBill.Name, iRowIndex].Value = oDetail.OtherDesc;
                    }
                }
            }
           
        }
    }
}

