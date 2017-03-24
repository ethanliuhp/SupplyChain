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
using Application.Business.Erp.SupplyChain.Client.EngineerManage.CompleteSettlementBook;

namespace Application.Business.Erp.SupplyChain.Client.CompleteSettlementBook
{
    public partial class VCompleteSearchCon : BasicUserControl
    {
        private MCompleteMng model = new MCompleteMng();
        
        private VCompleteSearchList searchlist;
        public VCompleteSearchCon(VCompleteSearchList searchlist)
        {
            InitializeComponent();
            this.searchlist = searchlist;
            this.ViewType = VirtualMachine.Component.WinMVC.core.ViewType.Search;
            this.InitEvent();
            btnOK.Focus();
        }
        private void InitForm()
        {
            this.dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-7);
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
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Ge("PlanTime", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("PlanTime", this.dtpDateEnd.Value.AddMonths(1).Date));
            //oq.AddCriterion(Expression.Eq("HandlePerson", ConstObject.LoginPersonInfo));
            //oq.AddCriterion(Expression.Eq("HandlePersonName", ConstObject.LoginPersonInfo.Name));
            //oq.AddCriterion(Expression.Eq("AccountName", projectInfo.Name));
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            IList list = model.CompleteSrv.GetComplete(oq);
            searchlist.RefreshData(list);
            this.btnOK.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }
    }
}
