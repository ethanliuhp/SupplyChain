using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using NHibernate.Criterion;
using System.Collections;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    public partial class VWeekScheduleSearch : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();  
        private MProjectTaskQuery model = new MProjectTaskQuery();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();

        CurrentProjectInfo proInfo = null ;
        GWBSTree w; 
        IList selectedList;
        SubContractType type;
        int flag;
        string btnName = "";

        public VWeekScheduleSearch(string btn)
        {
            btnName = btn;
            InitializeComponent();
            automaticSize.SetTag(this);
            InitEvent();
        }

        public void InitEvent()
        {
            btnTeamSelect.Click += new EventHandler(btnTeamSelect_Click);
            btnPersonSearch.Click += new EventHandler(btnPersonSearch_Click);
            txtPlanBDateBegin.Click += new EventHandler(txtPlanBDateBegin_Click);
        }

        void txtPlanBDateBegin_Click(object sender, EventArgs e)
        {
            VJscCalendarPicker v_time = new VJscCalendarPicker();
            v_time.ShowDialog();
            IList list = v_time.Result;
            if (list == null || list.Count == 0) return;
            this.txtPlanBDateBegin.Text = list[0] as string;
        }

        void btnPersonSearch_Click(object sender, EventArgs e)
        {
            VChosePersonByRole v_person = new VChosePersonByRole(proInfo, w, selectedList, flag);
            v_person.ShowDialog();
            IList alist = v_person.Result;
            if (alist == null || alist.Count == 0) return;
            StandardPerson person = alist[0] as StandardPerson;
            txtCreatePerson.Text = person.Name;
            txtCreatePerson.Tag = person;
        }

        void btnTeamSelect_Click(object sender, EventArgs e)
        {
            VChoiseTeam v_team = new VChoiseTeam(proInfo, w, selectedList, type, flag);
            v_team.ShowDialog();
            IList list = v_team.Result;
            if (list == null || list.Count == 0) return;
            SubContractProject engineerMaster = list[0] as SubContractProject;
            txtConstructionTeam.Text = engineerMaster.BearerOrgName;
            txtConstructionTeam.Tag = engineerMaster;
        }
        
        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
            txtBuildingProgram.Focus();
            ObjectQuery objectQuery = new ObjectQuery();
            if (this.txtWeekSchedule.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Master.PlanName", this.txtWeekSchedule.Text, MatchMode.Anywhere));
            }
            if (this.txtScheduleTask.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("GWBSTreeName", this.txtScheduleTask.Text, MatchMode.Anywhere));
            }
            //objectQuery.AddCriterion(Expression.Ge("PlannedBeginDate", this.dtpPlanBDateBegin.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("PlannedBeginDate", this.dtpPlanBDateEnd.Value.AddDays(1).Date));

            objectQuery.AddCriterion(Expression.Ge("PlannedEndDate", this.dtpPlanEDateBegin.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("PlannedEndDate", this.dtpPlanEDateEnd.Value.AddDays(1).Date));
            if (txtCreatePerson.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("Master.HandlePersonName", txtCreatePerson.Text));
            }
            if (this.txtConstructionTeam.Text != "")
            {
                objectQuery.AddCriterion(Expression.Eq("SupplierName", this.txtConstructionTeam.Text));
            }

            IList list = model.GetWeekScheduleDetail(objectQuery);
            if (list.Count == 0 || list == null)
            {
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据为空！";
                v_show.ShowDialog();
                return;
            }
            VWeekScheduleResult v_result = new VWeekScheduleResult(list, btnName);
            v_result.Show();
        }

        private void VWeekScheduleSearch_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
