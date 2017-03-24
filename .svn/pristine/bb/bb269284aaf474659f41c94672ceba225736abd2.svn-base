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
using NHibernate.Criterion;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;


namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
    public partial class VIndirectCostCopy : Form
    {
        private MIndirectCost model = new MIndirectCost();
        private IndirectCostMaster _selectMaster = null;
        private CurrentProjectInfo projectInfo = null;
        public IndirectCostMaster SelectMaster
        {
            get { return this._selectMaster; }
        }
        public VIndirectCostCopy()
        {
            InitializeComponent();
            InitialEvent();
            InitialData();
        }
        public void InitialData()
        {
            this.dtpDateBeginBill.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpDateEndBill.Value = ConstObject.TheLogin.LoginDate;
            projectInfo = StaticMethod.GetProjectInfo();

        }
        public void InitialEvent()
        {
         
            this.btnCancel.Click += new EventHandler(CancelClick);
            this.btnSure.Click += new EventHandler(SureClick);
            this.btnSearchBill.Click += new EventHandler(SearchClick);
            this.chkCheckAll.CheckedChanged += new EventHandler(CheckChange);
            this.chkRerveser.CheckedChanged += new EventHandler(CheckChange);
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
        }
        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            this.btnSearchBill.Focus();
        }
        public void CancelClick(object sender, EventArgs e)
        {

            this.Close();
        }

        public void SureClick(object sender, EventArgs e)
        {
            try
            {
                bool bFlag = false;
                IndirectCostDetail oDetail = null;
                // IndirectCostDetail oDe
                if (this.dgMaster.SelectedRows.Count == 0)
                {
                    throw new Exception("请选择[费用信息列表]行");
                }
                else
                {
                    this._selectMaster = this.dgMaster.SelectedRows[0].Tag as IndirectCostMaster;
                }
                if (this.dgDetail.Rows.Count > 0)
                {
                    foreach (DataGridViewRow oRow in this.dgDetail.Rows)
                    {
                        if (!ClientUtil.ToBool(oRow.Cells[this.colSelect.Name].Value))
                        {
                            oDetail = oRow.Tag as IndirectCostDetail;
                            this.SelectMaster.Details.Remove(oDetail);
                        }
                        else
                        {
                            bFlag = true;
                        }
                    }
                    if (bFlag)
                    {
                        this.Close();
                    }
                    else
                    {
                        throw new Exception("请选择明细");
                    }
                }
                else
                {
                    throw new Exception("没有明细");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SearchClick(object sender, EventArgs e)
        {
            int iRow = 0;
            ObjectQuery oQuery = new ObjectQuery();
            if (txtCodeBeginBill.Text != "")
            {
                oQuery.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, MatchMode.Anywhere));
            }
            if (this.txtCreatePersonBill.Text != "")
            {
                oQuery.AddCriterion(Expression.Like("CreatePersonName", this.txtCreatePersonBill.Text, MatchMode.Anywhere));
            }
            oQuery.AddCriterion(Expression.Eq("IsSubCompany", 2));
            //业务日期
            oQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBeginBill.Value.Date));
            oQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEndBill.Value.AddDays(1).Date));
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                oQuery.AddCriterion(Expression.Eq("ProjectId", this.projectInfo.Id));
            }
            else
            {
                oQuery.AddCriterion(Expression.IsNull("ProjectId"));
            }

            // oQuery.AddCriterion(Expression.Eq("Details.CostType", EnumCostType.间接费用));
            //oQuery.AddCriterion(Expression.In("Details.AccountSymbol", new ArrayList() { EnumAccountSymbol.其他, EnumAccountSymbol.财务费用标志 }));
            oQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oQuery.AddFetchMode("Details.AccountTitle", NHibernate.FetchMode.Eager);
            IList lstMaster = model.IndirectCostSvr.QueryIndirectCostMaster(oQuery);
            this.dgMaster.SelectionChanged -= new EventHandler(dgMaster_SelectionChanged);
            this.dgMaster.Rows.Clear();
            this.dgDetail.Rows.Clear();
            DataGridViewRow oRow = null;
            IndirectCostDetail oFinanceDetail = null;
            foreach (IndirectCostMaster oMaster in lstMaster)
            {
                if (oMaster.IndirectCost.Count > 0)
                {
                    oFinanceDetail = oMaster.FinanceCostSymbol;
                    if (oFinanceDetail != null)
                    {
                        oMaster.Details.Remove(oFinanceDetail);
                    }
                    iRow = dgMaster.Rows.Add();
                    oRow = dgMaster.Rows[iRow];
                    oRow.Tag = oMaster;
                    oRow.Cells[colCodeBill.Name].Value = oMaster.Code;
                    oRow.Cells[colCreateDateBill.Name].Value = oMaster.CreateDate;
                    oRow.Cells[colSummoneyBill.Name].Value = oMaster.SumMoney;
                    oRow.Cells[colStateBill.Name].Value = oMaster.DocState.ToString();
                    oRow.Cells[colCreatePersonBill.Name].Value = oMaster.CreatePersonName;
                    oRow.Cells[colRealOperationDateBill.Name].Value = oMaster.RealOperationDate;
                    oRow.Cells[colDescriptBill.Name].Value = oMaster.Descript;
                    //dgMaster.Rows[iRow].Cells[this.col]
                    //dgMaster.Rows[iRow].Cells[col]
                }
            }
            this.dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.Rows[0].Selected = true;
                // this._selectMaster = dgMaster.Rows[0].Tag as IndirectCostMaster;
                dgMaster_SelectionChanged(this.dgMaster,e);
            }
        }
        public void CheckChange(object sender, EventArgs e)
        {
            if (sender == this.chkCheckAll)
            {
                foreach (DataGridViewRow oRow in this.dgDetail.Rows)
                {
                    oRow.Cells[colSelect.Name].Value = true;
                }
            }
            else if (sender == this.chkRerveser)
            {
                foreach (DataGridViewRow oRow in this.dgDetail.Rows)
                {
                    oRow.Cells[colSelect.Name].Value = !ClientUtil.ToBool(oRow.Cells[colSelect.Name].Value) ;
                }
            }
        }
        public void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            int iRow = 0;
            this.dgDetail.Rows.Clear();
            if (dgMaster.SelectedRows.Count > 0 && dgMaster.SelectedRows[0].Tag != null)
            {
                IndirectCostMaster oMaster = dgMaster.SelectedRows[0].Tag as IndirectCostMaster;
                if (oMaster != null)
                {
                    foreach (IndirectCostDetail oDetail in oMaster.Details)
                    {//colAccountTitle colBudgetMoney colActualMoney colRate colDescript
                        iRow = this.dgDetail.Rows.Add();
                        this.dgDetail.Rows[iRow].Tag = oDetail;
                        this.dgDetail.Rows[iRow].Cells[colAccountTitle.Name].Value = oDetail.AccountTitleName;
                        this.dgDetail.Rows[iRow].Cells[colBudgetMoney.Name].Value = oDetail.Money.ToString("#0.00");
                        this.dgDetail.Rows[iRow].Cells[colActualMoney.Name].Value = oDetail.BudgetMoney.ToString("#0.00");
                        this.dgDetail.Rows[iRow].Cells[colRate.Name].Value = oDetail.Rate.ToString("#0.00");
                        this.dgDetail.Rows[iRow].Cells[colDescript.Name].Value = oDetail.Descript;
                        this.dgDetail.Rows[iRow].Cells[this.colSelect.Name].Value = true;
                    }
                }
            }
        }




    }
}
