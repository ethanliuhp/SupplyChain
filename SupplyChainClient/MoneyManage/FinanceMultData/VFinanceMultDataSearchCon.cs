using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VFinanceMultDataSearchCon : BasicUserControl
    {
        private MFinanceMultData model = new MFinanceMultData();
        private VFinanceMultDataSearchList searchList;
        private AccountType accountType;

        public VFinanceMultDataSearchCon(VFinanceMultDataSearchList searchList, AccountType accountType, FinanceMultDataExecType _execType)
        {
            this.searchList = searchList;
            this.searchList._execType = _execType;
            this.accountType = accountType;
            InitializeComponent();            
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitForm();
            this.InitEvent();
            btnOK.Focus();

        }

        private void InitForm()
        {
            //this.dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            //this.dtpDateEnd.Value = ConstObject.LoginDate;
            //txtSupplier.SupplierCatCode = CommonUtil.SupplierCatCode;
            cmbYear.Items.Clear();
            for (int iYear = 2012; iYear <= DateTime.Now.Year; iYear++)
            {
                cmbYear.Items.Add(iYear);
            }
            cmbYear.SelectedItem = DateTime.Now.Year;

            cmbMonth.Items.Clear();
            for (int iMonth = 1; iMonth <= 12; iMonth++)
            {
                cmbMonth.Items.Add(iMonth);
            }
            cmbMonth.Items.Insert(0, "");
            cmbMonth.SelectedItem = 0;
        }

        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
           // this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            //this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Like("Code", this.txtCodeBegin.Text,MatchMode.Anywhere));
            }

            //查询自己的单据
            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            oq.AddCriterion(Expression.Eq("Year",ClientUtil.ToInt( this.cmbYear.SelectedItem)));
            if (this.cmbMonth.SelectedItem != null && !string.IsNullOrEmpty(this.cmbMonth.SelectedItem.ToString()))
            {
                oq.AddCriterion(Expression.Eq("Month", ClientUtil.ToInt(this.cmbMonth.SelectedItem)));
            }
            //oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            //oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            //AccountType
            oq.AddCriterion(Expression.Eq("AccountType", this.accountType));
            //区分项目
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            }
            else
            {
                oq.AddCriterion(Expression.Eq("OperOrgInfo.Id", ConstObject.TheLogin.TheOperationOrgInfo.Id));
            }
            oq.AddOrder(new Order("Code", false));
            IList list = model.FinanceMultDataSrv.Query(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}