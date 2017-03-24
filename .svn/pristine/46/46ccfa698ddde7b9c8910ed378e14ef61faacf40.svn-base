using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.ConstructionDesignManage
{
    public partial class VConstructionDesignSearchCon : BasicUserControl
    {
        private MProjectPlanningMng model = new MProjectPlanningMng();
        private VConstructionDesignSearchList searchList;

        public VConstructionDesignSearchCon(VConstructionDesignSearchList searchList)
        {
            this.searchList = searchList;
            InitializeComponent();            
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitEvent();
            btnOK.Focus();
        }

        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnOK.Click += new EventHandler(btnOK_Click);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            string planninglevel = "施工组织设计";
            oq.AddCriterion(Expression.Ge("SubmitDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("SubmitDate", this.dtpDateEnd.Value.AddDays(1).Date));
            oq.AddCriterion(Expression.Eq("PlanningLevel", planninglevel));
            IList list = model.ProjectPlanningSrv.GetSpecialityProposal(oq);
            searchList.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
