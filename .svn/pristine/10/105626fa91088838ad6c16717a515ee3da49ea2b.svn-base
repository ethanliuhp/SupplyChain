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
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm
{
    public partial class VGWBSTreeConfirmSelect : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        MConfirmmng model = new MConfirmmng();
        CurrentProjectInfo projectInfo;

        SubContractProject subSelect;
        DocumentState stateSelect;
        PersonInfo personSelect;

        public VGWBSTreeConfirmSelect()
        {
            InitializeComponent();
            automaticSize.SetTag(this);

            InitEvent();
            InitDate();
        }
        void InitDate()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            personSelect = ConstObject.LoginPersonInfo;


            DateTime time = model.GetPreviousSundayDate();
            //dtpConfirmDateA.Value = dtpPlanStartDateA.Value = time.Date;
            //dtpPlanStartDateB.Value = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
            //dtpPlanEndDateA.Value = DateTime.Now.Date;
            //dtpPlanEndDateB.Value = time.AddDays(8).Date.AddSeconds(-1);
        }
        void InitEvent()
        {
            //txtConfirmState.Click += new EventHandler(txtConfirmState_Click);
            //txtConfirmPerson.Click += new EventHandler(txtConfirmPerson_Click);
            txtTaskHandle.Click += new EventHandler(txtTaskHandle_Click);
        }

        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);//上一页
            this.Close();
        }
        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
            base.TBtnPageDown_Click(sender, e);//下一页
            IList taskConfirmList = Search();
            if (taskConfirmList != null && taskConfirmList.Count > 0)
            {
                VGwbsMng vcon = new VGwbsMng(taskConfirmList);
                vcon.ShowDialog();
            }
            else
            {
                VMessageBox vmb = new VMessageBox();
                vmb.txtInformation.Text = "未找到数据！";
                vmb.ShowDialog();
            }
        }
        //public override void TBtnContents_Click(object sender, EventArgs e)
        //{
        //    base.TBtnContents_Click(sender, e);//功能菜单
        //}

        /// <summary>
        /// 确认状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void txtConfirmState_Click(object sender, EventArgs e)
        //{
        //    List<string> stateListt = new List<string>();
        //    foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
        //    {
        //        string stateName = ClientUtil.GetDocStateName(state);
        //        stateListt.Add(stateName);
        //    }
        //    if (stateListt.Count > 0 && stateListt != null)
        //    {
        //        VCheckConfirmState vccs = new VCheckConfirmState(stateListt);
        //        vccs.ShowDialog();
        //        string resultState = vccs.Result;
        //        if (resultState == "" || resultState == null)
        //            return;
        //        txtConfirmState.Text = resultState;
        //        foreach (DocumentState s in Enum.GetValues(typeof(DocumentState)))
        //        {
        //            string s_name = ClientUtil.GetDocStateName(s);
        //            if (s_name == resultState)
        //            {
        //                stateSelect = s;
        //                txtConfirmState.Text = s_name;
        //                break;
        //            }
        //        }
        //    }
        //}
        /// <summary>
        /// 承担者
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtTaskHandle_Click(object sender, EventArgs e)
        {
            VChoiseTeam vct = new VChoiseTeam(projectInfo, null, null, SubContractType.劳务分包, 0);
            vct.ShowDialog();
            IList result = vct.Result;
            if (result != null && result.Count > 0)
            {
                subSelect = result[0] as SubContractProject;
                txtTaskHandle.Text = subSelect.BearerOrgName;
            }
        }
        ///// <summary>
        ///// 确认人
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void txtConfirmPerson_Click(object sender, EventArgs e)
        //{
        //    VChosePersonByRole vcp = new VChosePersonByRole(projectInfo, null, null, 0);
        //    vcp.ShowDialog();
        //    IList resultPersonList = vcp.Result;
        //    if(resultPersonList!=null && resultPersonList.Count>0)
        //    {
        //        StandardPerson per = resultPersonList[0] as StandardPerson;
        //        ObjectQuery oq = new ObjectQuery();
        //        oq.AddCriterion(Expression.Eq("Id",per.Id));
        //        personSelect = model.ProductionManagementSrv.ObjectQuery(typeof(PersonInfo), oq)[0] as PersonInfo;
        //        txtConfirmPerson.Text = personSelect.Name;
        //    }
        //}

        IList Search()
        {
            ObjectQuery oq = new ObjectQuery();
            Disjunction dis = new Disjunction();

            if (subSelect != null)
                oq.AddCriterion(Expression.Eq("TaskHandler.Id", subSelect.Id));
            //if (personSelect != null)
            //    oq.AddCriterion(Expression.Eq("CreatePerson.Id", personSelect.Id));
            if (txtTaskDetailName.Text.Trim() != "")
                oq.AddCriterion(Expression.Like("GWBSDetailName", txtTaskDetailName.Text, MatchMode.Start));
            if (txtConstructionSite.Text.Trim() != "")
            {
                oq.AddCriterion(Expression.Like("GWBSTreeName", txtConstructionSite.Text, MatchMode.Start));
            }

            //if (txtConstructionSite.Text.Trim() != "")
            //{
            //    ObjectQuery oq1 = new ObjectQuery();
            //    oq1.AddCriterion(Expression.Like("PBSName", txtConstructionSite.Text, MatchMode.Anywhere));
            //    List<GWBSRelaPBS> pbslist = model.ProductionManagementSrv.ObjectQuery(typeof(GWBSRelaPBS), oq1).OfType<GWBSRelaPBS>().ToList();

            //    var listRelaPBS = from pbs in pbslist
            //                      group pbs by pbs.TheGWBSTree
            //                          into g
            //                          select new { g.Key.Id };
            //    foreach (var id in listRelaPBS)
            //    {
            //        dis.Add(Expression.Eq("GWBSTree", id.Id));
            //    }
            //    oq.AddCriterion(dis);
            //}
            oq.AddFetchMode("WeekScheduleDetailGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Master.Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("WeekScheduleDetailGUID.SubContractProject", NHibernate.FetchMode.Eager);


            IList resultList1 = model.ProductionManagementSrv.ObjectQuery(typeof(GWBSTaskConfirm),oq);

            return resultList1;
        }

        private void VGWBSTreeConfirmSelect_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

    }
}
