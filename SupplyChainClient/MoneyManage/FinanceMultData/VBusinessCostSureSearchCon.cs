using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public partial class VBusinessCostSureSearchCon : BasicUserControl
    {
        private MFinanceMultData service = new MFinanceMultData();
        private VFinanceMultDataSearchList searchList;
        AccountType accountType;
        FinanceMultDataExecType  _execType;

        public VBusinessCostSureSearchCon(VFinanceMultDataSearchList searchList, AccountType accountType, FinanceMultDataExecType _execType)
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
            IList list = null;
            IList lst = new ArrayList();
            ObjectQuery oq = new ObjectQuery();
            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Like("Master.Code", "%" + this.txtCodeBegin.Text + "%"));
            }
            oq.AddCriterion(Expression.Eq("Master.CreatePerson", ConstObject.LoginPersonInfo));
            oq.AddCriterion(Expression.Ge("Master.CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("Master.CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Gt("BusCostSure", 0M));
            oq.AddOrder(new Order("Master.Code", false));
            oq.AddFetchMode("Master", FetchMode.Eager);
            //oq.AddFetchMode("Master.Details", FetchMode.Eager);
             list = service.FinanceMultDataSrv.Query(typeof(FinanceMultDataDetail), oq);
            foreach (FinanceMultDataDetail oDetail in list)
            {
                lst.Add(oDetail.Master);
            }
            searchList.RefreshData(lst);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
