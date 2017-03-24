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
using NHibernate.Criterion;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.WebSitePlanManage
{
    public partial class VWebSitePlanSearchCon : BasicUserControl
    {
        private MProjectPlanningMng model = new MProjectPlanningMng();
        private VWebSitePlanSearchList searchlist;

        public VWebSitePlanSearchCon(VWebSitePlanSearchList searchlist)
        {
            InitializeComponent();
            this.searchlist = searchlist;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            InitEvent();
            btnOK.Focus();
        }

        public void InitEvent()
        {
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            string planninglevel = "专业策划";
            oq.AddCriterion(Expression.Ge("SubmitDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("SubmitDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("PlanningLevel", planninglevel));
            IList list = model.ProjectPlanningSrv.GetSpecialityProposal(oq);
            searchlist.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
