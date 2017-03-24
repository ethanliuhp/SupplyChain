using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VFinanceMultDataQuery : TBasicDataView
    {
        private string sIndirectCostAccountTitleCode = "6602";
        private MFinanceMultData model = new MFinanceMultData();
        public VFinanceMultDataQuery()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            this.txtCreatePersonBill.isAllLoad = false;
            cmbYearBill.Items.Clear();
            cmbYear.Items.Clear();
            for (int iYear = 2012; iYear <= DateTime.Now.Year; iYear++)
            {
                cmbYearBill.Items.Add(iYear);
                cmbYear.Items.Add(iYear);
            }
            cmbYear.Items.Insert(0, "");
            cmbYearBill.Items.Insert(0, "");
            cmbYear.SelectedIndex = 0;
            cmbYearBill.SelectedIndex = 0;
            //cmbYear.SelectedItem = DateTime.Now.Year;

            cmbMonthBill.Items.Clear();
            cmbMonth.Items.Clear();
            for (int iMonth = 1; iMonth <= 12; iMonth++)
            {
                cmbMonthBill.Items.Add(iMonth);
                cmbMonth.Items.Add(iMonth);
            }
            cmbMonthBill.Items.Insert(0, "");
            cmbMonth.Items.Insert(0, "");
            cmbMonthBill.SelectedIndex = 0;
            cmbMonth.SelectedIndex = 0;
            //cmbMonth.SelectedItem = DateTime.Now.Month;

        }
        private void InitEvent()
        {
            pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
            this.btnSearchBill.Click += new EventHandler(btnSearchBill_Click);
            this.btnExcelBill.Click += new EventHandler(btnExcelBill_Click);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
          //  btnSelectAccountTitle.Click += new EventHandler(btnSelectAccountTitle_Click);
          // this.btnSelectOrgName.Click+=new EventHandler(btnSelectOrgName_Click);
        }
        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }
      
        



        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
        }
        

        void btnSearch_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                objectQuery.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
            }
            else
            {
                objectQuery.AddCriterion(Expression.IsNull("Master.ProjectId"));
            }
            if (ConstObject.TheLogin.TheOperationOrgInfo != null)
            {
                objectQuery.AddCriterion(Expression.Like("Master.OpgSysCode", ConstObject.TheLogin.TheOperationOrgInfo.SysCode, MatchMode.Start)); 
            }
            //单据
            if (txtCodeBegin.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Master.Code", txtCodeBegin.Text, MatchMode.Anywhere));
            }
            if (cmbYear.SelectedIndex > 0)
            {
                objectQuery.AddCriterion(Expression.Eq("Master.Year", (int)cmbYear.SelectedItem));
            }
            if (cmbMonth.SelectedIndex > 0)
            {
                objectQuery.AddCriterion(Expression.Eq("Master.Month", (int)cmbMonth.SelectedItem));
            }

            //业务日期
            objectQuery.AddCriterion(Expression.Ge("Master.RealOperationDate", this.dtpDateBegin.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("Master.RealOperationDate", this.dtpDateEnd.Value.AddDays(1).Date));
            //制单人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Master.CreatePersonName", txtCreatePerson.Text, MatchMode.Anywhere));
            }
            try
            {
                objectQuery.AddFetchMode("Master", FetchMode.Eager);
                list = model.FinanceMultDataSrv.Query(typeof(FinanceMultDataDetail), objectQuery);
                dgDetail.Rows.Clear();
                ShowDetailList(list);
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
        void btnExcelBill_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgMaster, true);
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
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                objectQuery.AddCriterion(Expression.Eq("ProjectId",projectInfo.Id));

            }
            else
            {
                //临时
                objectQuery.AddCriterion(Expression.IsNull("ProjectId"));
            }
            if (ConstObject.TheLogin.TheOperationOrgInfo != null)
            {
                objectQuery.AddCriterion(Expression.Like("OpgSysCode", ConstObject.TheLogin.TheOperationOrgInfo.SysCode, MatchMode.Start));
            }
            //单据
            if (txtCodeBeginBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, MatchMode.Anywhere));
            }
            if (cmbYearBill.SelectedIndex > 0)
            {
                objectQuery.AddCriterion(Expression.Eq("Year", (int)cmbYearBill.SelectedItem ));
            }
            if (cmbMonthBill.SelectedIndex > 0)
            {
                objectQuery.AddCriterion(Expression.Eq("Month", (int)cmbMonthBill.SelectedItem ));
            }
            
            //业务日期
            objectQuery.AddCriterion(Expression.Ge("RealOperationDate", this.dtpDateBeginBill.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("RealOperationDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            //制单人
            if (txtCreatePersonBill.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("CreatePersonName", txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            try
            {
                
                objectQuery.AddFetchMode("Details", FetchMode.Eager);
                list = model.FinanceMultDataSrv.Query(typeof(FinanceMultDataMaster), objectQuery);
                dgMaster.Rows.Clear();
                //dgDetailBill.Rows.Clear();
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
            if (masterList == null || masterList.Count == 0) return;
            foreach (FinanceMultDataMaster master in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCodeBill.Name, rowIndex].Value = master.Code; //单据号
                dgMaster[colCreatePersonBill.Name, rowIndex].Value = master.CreatePersonName;//制单人
                dgMaster[colDescriptBill.Name, rowIndex].Value = master.Descript;//备注
                dgMaster[colYearBill.Name, rowIndex].Value = master.Year;
                dgMaster[colMonthBill.Name, rowIndex].Value = master.Month;
                foreach (FinanceMultDataDetail oDetail in master.Details)
                {
                    dgMaster[this.colSummoneyBill.Name, rowIndex].Value = oDetail.Money.ToString("#0,000.00");
                }
                //dgMaster[colCreateDateBill.Name, rowIndex].Value = master.CreateDateToString("yyyy-MM-dd");//业务日期
                dgMaster[colStateBill.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);//状态
                if (master.RealOperationDate > ClientUtil.ToDateTime("1900-1-1"))
                {
                    dgMaster[colRealOperationDateBill.Name, rowIndex].Value = master.RealOperationDate.ToString("yyyy-MM-dd");//制单时间;
                }
            }
            if (dgMaster.Rows.Count > 0)
            {
                this.dgMaster.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
                dgMaster.CurrentCell = dgMaster[1, 0];
                dgMaster_SelectionChanged(dgMaster, new EventArgs());
            }
        }
        public void ShowDetailList(IList lstDetail)
        {
            int iRowIndex = -1;
            this.dgDetail.Rows.Clear();
            if (lstDetail == null || lstDetail.Count == 0) return;
            foreach (FinanceMultDataDetail oDetail in lstDetail)
            {
                iRowIndex = this.dgDetail.Rows.Add();
               
                this.dgDetail[this.colCivilAndSetUpBalance.Name, iRowIndex].Value = oDetail.CivilAndSetUpBalance.ToString("N2");
                this.dgDetail[this.colCivilAndSetUpPayout.Name, iRowIndex].Value = oDetail.CivilAndSetUpPayout.ToString("N2");
                this.dgDetail[this.colCivilProjectBalance.Name, iRowIndex].Value = oDetail.CivilProjectBalance.ToString("N2");

                this.dgDetail[this.colContractGrossProfit.Name, iRowIndex].Value = oDetail.ContractGrossProfit.ToString("N2");

                this.dgDetail[this.colExchangeMaterialRemain.Name, iRowIndex].Value = oDetail.ExchangeMaterialRemain.ToString("N2");
                this.dgDetail[this.colLowValueConsumableRemain.Name, iRowIndex].Value = oDetail.LowValueConsumableRemain.ToString("N2");
                this.dgDetail[this.colMainBusinessTax.Name, iRowIndex].Value = oDetail.MainBusinessTax.ToString("N2");
                this.dgDetail[this.colMaterialCost.Name, iRowIndex].Value = oDetail.MaterialCost.ToString("N2");
                this.dgDetail[this.colMaterialRemain.Name, iRowIndex].Value = oDetail.CivilAndSetUpBalance.ToString("N2");
                this.dgDetail[this.colMechanicalCost.Name, iRowIndex].Value = oDetail.MechanicalCost.ToString("N2");
                this.dgDetail[this.colOtherDirectCost.Name, iRowIndex].Value = oDetail.OtherDirectCost.ToString("N2");
                this.dgDetail[this.colPersonCost.Name, iRowIndex].Value = oDetail.PersonCost.ToString("N2");
                this.dgDetail[this.colSetUpPayout.Name, iRowIndex].Value = oDetail.SetUpPayout.ToString("N2");
                this.dgDetail[this.colSetUpProjectBuild.Name, iRowIndex].Value = oDetail.SetUpProjectBuild.ToString("N2");
                this.dgDetail[this.colSubProjectPayout.Name, iRowIndex].Value = oDetail.SubProjectPayout.ToString("N2");
                this.dgDetail[this.colTempDeviceRemain.Name, iRowIndex].Value = oDetail.TempDeviceRemain.ToString("N2");
                this.dgDetail[this.colMoney.Name, iRowIndex].Value = oDetail.SumMoney.ToString("N2");

                this.dgDetail[this.colCode.Name, iRowIndex].Value = oDetail.Master.Code;
                this.dgDetail[this.colRealOperateDate.Name, iRowIndex].Value = oDetail.Master.RealOperationDate.ToString("yyyy-MM-dd");

                this.dgDetail[this.colMonth.Name, iRowIndex].Value =( oDetail.Master as FinanceMultDataMaster).Month;
                this.dgDetail[this.colYear.Name, iRowIndex].Value = (oDetail.Master as FinanceMultDataMaster).Year;
                this.dgDetail[this.colCreatePersonName.Name, iRowIndex].Value = oDetail.Master.CreatePersonName;
            }
            this.dgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }
        /// <summary>
        /// 主表变化，明细同步变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            int i = 0;
            FinanceMultDataDetail oDetail = null;
            if (this.dgMaster.CurrentRow == null || this.dgMaster.CurrentRow.Tag == null || !(this.dgMaster.CurrentRow.Tag is FinanceMultDataMaster))
            {
                oDetail = new FinanceMultDataDetail();
            }
            else
            {
                FinanceMultDataMaster oMaster = this.dgMaster.CurrentRow.Tag as FinanceMultDataMaster;
                oDetail = oMaster.Details.Count > 0 ? oMaster.Details.ElementAtOrDefault(0) as FinanceMultDataDetail : new FinanceMultDataDetail();
            }
            this.txtCivilAndSetUpBalance.Text = oDetail.CivilAndSetUpBalance.ToString("N2");
            this.txtCivilAndSetUpPayout.Text = oDetail.CivilAndSetUpPayout.ToString("N2");
            this.txtCivilProjectBalance.Text = oDetail.CivilProjectBalance.ToString("N2");
            this.txtContractGrossProfit.Text = oDetail.ContractGrossProfit.ToString("N2");
            this.txtExchangeMaterialRemain.Text = oDetail.ExchangeMaterialRemain.ToString("N2");
            this.txtLowValueConsumableRemain.Text = oDetail.LowValueConsumableRemain.ToString("N2");
            this.txtMainBusinessTax.Text = oDetail.MainBusinessTax.ToString("N2");
            this.txtMaterialCost.Text = oDetail.MaterialCost.ToString("N2");
            this.txtMaterialRemain.Text = oDetail.MaterialRemain.ToString("N2");
            this.txtMechanicalCost.Text = oDetail.MechanicalCost.ToString("N2");
            this.txtOtherDirectCost.Text = oDetail.OtherDirectCost.ToString("N2");
            this.txtPersonCost.Text = oDetail.PersonCost.ToString("N2");
            this.txtSetUpPayout.Text = oDetail.SetUpPayout.ToString("N2");
            this.txtSetUpProjectBuild.Text = oDetail.SetUpProjectBuild.ToString("N2");

            this.txtSubProjectPayout.Text = oDetail.SubProjectPayout.ToString("N2");
            this.txtTempDeviceRemain.Text = oDetail.TempDeviceRemain.ToString("N2");
            this.txtMoney.Text = oDetail.SumMoney.ToString("N2");
        }
    }
}
