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
using Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut
{
    public partial class VLossOutSthCon : BasicUserControl
    {
        MLossOut theMLossOut = new MLossOut();
        private LossOut theLossOut;
        public VLossOutList theVLossOutList;
        private EnumStockExecType execType;

        virtual public LossOut TheLossOut
        {
            get { return theLossOut; }
            set { theLossOut = value; }
        }

        public VLossOutSthCon(EnumStockExecType execType)
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
            theLossOut = null;
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
            IList list = theMLossOut.GetObject(oq);
            theVLossOutList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void txtCodeBegin_tbTextChanged(Object sender, EventArgs e)
        {
            txtCodeEnd.Text = txtCodeBegin.Text;
        }
    }
}

