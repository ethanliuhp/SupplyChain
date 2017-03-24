using System;
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
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VPaymentInvoiceSearchCon : BasicUserControl
    {
        private MFinanceMultData model = new MFinanceMultData();
        private VPaymentInvoiceSearchList searchList;

        public VPaymentInvoiceSearchCon(VPaymentInvoiceSearchList searchList)
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
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddMonths(-1);
            this.dtpDateEnd.Value = ConstObject.LoginDate;
        }

        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
        }
        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Like("Code", "%" + this.txtCodeBegin.Text + "%"));
            }
            oq.AddCriterion(Expression.Eq("OperOrgInfo", ConstObject.TheLogin.TheOperationOrgInfo));

            oq.AddCriterion(Expression.Eq("CreatePerson", ConstObject.LoginPersonInfo));
            //区分专业
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddOrder(new Order("Code", false));
            IList list = model.FinanceMultDataSrv.GetPaymentInvoice(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
