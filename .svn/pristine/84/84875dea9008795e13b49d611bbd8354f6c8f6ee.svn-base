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

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.ImplementationPlan
{
    public partial class VImplementationSearchCon : BasicUserControl
    {
        private MImplementationPlan model = new MImplementationPlan();
        private VImplementationSearchList searchlist;
       

        public VImplementationSearchCon(VImplementationSearchList searchlist)
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

            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpMadeBillDate.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpCreateDateEnd.Value.AddDays(1).Date));

            IList list = model.ImplementSrv.GetImp(oq);
            searchlist.RefreshData(list);
            this.btnselect.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnselect.FindForm().Close();
        }
    }
}
