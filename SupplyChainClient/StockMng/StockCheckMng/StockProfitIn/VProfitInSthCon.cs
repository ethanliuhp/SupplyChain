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
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.StockManage.StockProfitIn.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn
{
    public partial class VProfitInSthCon : BasicUserControl
    {
        //private static IMProfitIn theMProfitIn = StaticMethod.GetRefModule(typeof(MProfitIn)) as IMProfitIn;
        MProfitIn theMProfitIn = new MProfitIn();
        private ProfitIn theProfitIn;
        public VProfitInList theVProfitInList;
        private EnumStockExecType execType;

        virtual public ProfitIn TheProfitIn
        {
            get { return theProfitIn; }
            set { theProfitIn = value; }
        }

        public VProfitInSthCon(EnumStockExecType execType)
        {
            InitializeComponent();
            this.execType = execType;

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

        void btnCancel_Click(object sender, EventArgs e)
        {
            theProfitIn = null;
            btnOK.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            if (this.txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", this.txtCodeBegin.Text, this.txtCodeEnd.Text));
            }

            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));

            
            if (this.optNoTally.Checked)
            {
                oq.AddCriterion(Expression.Eq("IsTally", 0));
            }
            if (this.optTally.Checked)
            {
                oq.AddCriterion(Expression.Eq("IsTally", 1));
            }
            oq.AddCriterion(Expression.Eq("Special", Enum.GetName(typeof(EnumStockExecType), execType)));
            //区分项目
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            oq.AddOrder(new Order("Code", false));
            IList list = theMProfitIn.GetObject(oq);
            theVProfitInList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void txtCodeBegin_tbTextChanged(Object sender, EventArgs e)
        {
            txtCodeEnd.Text = txtCodeBegin.Text;
        }
    }
}

