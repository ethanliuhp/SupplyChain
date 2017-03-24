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
using Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostMonthAccount
{
    public partial class VContractDisclosureSearchCon : BasicUserControl
    {
        private MCostMonthAccount model = new MCostMonthAccount();
        private VContractDisclosureSearchList searchlist;


        public VContractDisclosureSearchCon(VContractDisclosureSearchList searchlist)
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
            string Name =  this.txtProName.Text;
            if (!ClientUtil.isEmpty(Name))
            {
               oq.AddCriterion(Expression.Like("ProjectName", "%" + Name + "%"));
            }
            if (projectInfo != null && projectInfo.Code != CommonUtil.CompanyProjectCode)
            {
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            }
            oq.AddCriterion(Expression.Eq("CreatePersonName", ConstObject.LoginPersonInfo.Name));
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtDisclosureStart.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtDisclosureEnd.Value.AddDays(1).Date));

            IList list = model.CostMonthAccSrv.GetContractDisclosure(oq);
            searchlist.RefreshData(list);
            this.btnselect.FindForm().Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnselect.FindForm().Close();
        }

    
    }
}
