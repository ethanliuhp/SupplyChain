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
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.BusinessProposalManage
{
    public partial class VBusinessProposalSearchCon : BasicUserControl
    {
        private MProjectPlanningMng model = new MProjectPlanningMng();
        private VBusinessProposalSearchList searchlist;

        public VBusinessProposalSearchCon(VBusinessProposalSearchList searchlist)
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
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Ge("SubmitDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("SubmitDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.ProjectPlanningSrv.GetBusinessProposal(oq);
            searchlist.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
