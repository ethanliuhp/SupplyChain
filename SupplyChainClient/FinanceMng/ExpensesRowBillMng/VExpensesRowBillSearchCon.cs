﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesRowBillMng
{
    public partial class VExpensesRowBillSearchCon : BasicUserControl
    {
        private MExpensesRowBillMng model = new MExpensesRowBillMng();
        private VExpensesRowBillSearchList searchList;

        public VExpensesRowBillSearchCon(VExpensesRowBillSearchList searchList)
        {
            this.searchList = searchList;
            InitializeComponent();            
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitForm();
            this.InitEvent();
            btnOK.Focus();
        }

        private void InitForm()
        {
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
            this.dtpDateEnd.Value = ConstObject.LoginDate;
        }

        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
            this.txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", this.txtCodeBegin.Text, this.txtCodeEnd.Text));
            }

            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            if (this.txtBorrowPerson.Text != "")
            {
                oq.AddCriterion(Expression.Eq("HandlePerson", this.txtBorrowPerson.Text));
            }
            oq.AddCriterion(Expression.Ge("IssueDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("IssueDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddOrder(new Order("Code", false));
            IList list = model.ExpensesRowBillSrv.GetExpensesRowBill(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
