using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn
{
    public partial class VAcctLoseAndProfitCon : BasicUserControl
    {
        private IMProfitIn theMProfitIn = StaticMethod.GetRefModule(typeof(MProfitIn)) as IMProfitIn;
        private AcctLoseAndProfit theAcctLostAndProfit;
        public VAcctLoseAndProfitList theVList;

        virtual public AcctLoseAndProfit TheAcctLostAndProfit
        {
            get { return theAcctLostAndProfit; }
            set { theAcctLostAndProfit = value; }
        }

        public VAcctLoseAndProfitCon()
        {
            InitializeComponent();

            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            InitForm();
            InitEvent();
        }

        public void InitForm()
        {
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddMonths(-1);
            this.dtpDateEnd.Value = ConstObject.LoginDate;
        }

        public void InitEvent()
        {
            btnCancel.Click += new EventHandler(btnCancel_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
        }

        void btnCancel_Click(object sender,EventArgs e)
        {
            theAcctLostAndProfit = null;
            btnOK.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", this.txtCodeBegin.Text, this.txtCodeEnd.Text));
            }

            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value));
            oq.AddCriterion(Expression.Le("CreateDate", this.dtpDateEnd.Value));

            oq.AddOrder(new Order("Id", false));
            IList list = theMProfitIn.GetObjectByAcct(oq);
            theVList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void txtCodeBegin_tbTextChanged(Object sender, EventArgs e)
        {
            txtCodeEnd.Text = txtCodeBegin.Text;
        }
    }
}

