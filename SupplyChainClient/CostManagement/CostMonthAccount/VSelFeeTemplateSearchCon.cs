using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VSelFeeTemplateSearchCon : BasicUserControl
    {
        private MCostMonthAccount model = new MCostMonthAccount();
        private VSelFeeTemplateSearchList searchlist;

        public VSelFeeTemplateSearchCon(VSelFeeTemplateSearchList searchlist)
        {
            InitializeComponent();
            this.searchlist = searchlist;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            InitEvent();
            btnselect.Focus();
        }
        public void InitEvent()
        {
            btnselect.Click += new EventHandler(btnselect_Click);
            btncancle.Click += new EventHandler(btnCancel_Click);
        }

        void btnselect_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            string Name = this.txtProName.Text;
            if (!ClientUtil.isEmpty(Name))
            {
                oq.AddCriterion(Expression.Like("Name", "%" + Name + "%"));
            }

            oq.AddCriterion(Expression.Ge("CreateDate", this.dtDisclosureStart.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtDisclosureEnd.Value.AddDays(1).Date));

            IList list = model.CostMonthAccSrv.GetSelFeeTemplateMasterList(oq);
            searchlist.RefreshData(list);
            this.btnselect.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnselect.FindForm().Close();
        }
    }
}
