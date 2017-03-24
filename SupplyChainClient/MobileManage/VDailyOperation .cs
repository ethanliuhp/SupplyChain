using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrectioSearch;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage
{
    public partial class VDailyOperation : TBasicToolBarByMobile
    {
        private GWBSTree gwbsTree;
        private MProjectTaskQuery model = new MProjectTaskQuery();
        private AutomaticSize automaticSize = new AutomaticSize();
        IList list = new ArrayList();
        string btn = "";

        public VDailyOperation()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitEvent();
        }

        public void InitEvent()
        {
            this.Load += new EventHandler(VDailyOperation_Load);
            lbl0.Click += new EventHandler(lbl0_Click);
            lbl1.Click += new EventHandler(lbl1_Click);
            lbl2.Click += new EventHandler(lbl2_Click);
            lbl3.Click += new EventHandler(lbl3_Click);
            lbl4.Click += new EventHandler(lbl4_Click);
            lbl5.Click += new EventHandler(lbl5_Click);
            lbl6.Click += new EventHandler(lbl6_Click);
        }

        void lbl0_Click(object sender, EventArgs e)
        {
            showDailyOperation(list);
            VWeekScheduleResult v_result = new VWeekScheduleResult(list, btn);
            v_result.ShowDialog();
        }

        void lbl1_Click(object sender, EventArgs e)
        {

        }

        void lbl2_Click(object sender, EventArgs e)
        {

        }

        void lbl3_Click(object sender, EventArgs e)
        {

        }

        void lbl4_Click(object sender, EventArgs e)
        {

        }

        void lbl5_Click(object sender, EventArgs e)
        {

        }

        void lbl6_Click(object sender, EventArgs e)
        {
            VDailyCorrectionSearch v_corrention = new VDailyCorrectionSearch(btn);
            v_corrention.ShowDialog();
        }

        void VDailyOperation_Load(object sender, EventArgs e)
        {
            showDailyOperation(list);
            showScheduleConfirmFlag();
            showTaskCheckState();
            showTaskCheckStateNO();
            showGWBSTaskConfirm();
            showInspectionRecord();
            showRectificationNotice();
            this.WindowState = FormWindowState.Maximized;
        }

        private void showDailyOperation(IList list)
        {
            ObjectQuery objectQuery = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
            decimal taskCompletedPercent = 100;
            objectQuery.AddCriterion(Expression.Eq("TaskCompletedPercent", taskCompletedPercent));
            objectQuery.AddCriterion(Expression.Eq("GWBSConfirmFlag", 0));
            objectQuery.AddCriterion(Expression.Eq("GWBSTree.OwnerGUID", ConstObject.LoginPersonInfo.Id));
            objectQuery.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            list = model.GetWeekScheduleDetail(objectQuery);
            lbl0.Text = "任务完成但未开工单的" + list.Count + "项";

        }

        private void showScheduleConfirmFlag()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("ScheduleConfirmFlag", 0));
            objectQuery.AddCriterion(Expression.Eq("GWBSTree.OwnerGUID", ConstObject.LoginPersonInfo.Id));
            objectQuery.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            IList list = model.GetWeekScheduleDetail(objectQuery);
            lbl1.Text = "到期但未确认形象进度的" + list.Count + "项";
        }

        private void showTaskCheckState()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("GWBSTree.OwnerGUID", ConstObject.LoginPersonInfo.Id));
            objectQuery.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            IList list = model.GetWeekScheduleDetail(objectQuery);
            lbl2.Text = "任务已完成但未通过检查的" + list.Count + "项";
        }

        private void showTaskCheckStateNO()
        {
            ObjectQuery objectQuery = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            objectQuery.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
            objectQuery.AddCriterion(Expression.Eq("GWBSTree.OwnerGUID", ConstObject.LoginPersonInfo.Id));
            objectQuery.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            IList list = model.GetWeekScheduleDetail(objectQuery);
            lbl3.Text = "已开单但未通过日常检查的" + list.Count + "项";
        }

        private void showGWBSTaskConfirm()
        {
            ObjectQuery oq = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("ConfirmHandlePersonName", ConstObject.LoginPersonInfo.Name));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.Edit));
            IList list = model.GetGWBSTaskConfirmMaster(oq);
            lbl4.Text = "未提交的工程量确认单" + list.Count + "项";
        }

        private void showInspectionRecord()
        {
            ObjectQuery oq = new ObjectQuery();
            IList list = model.GetWeekScheduleDetail(oq);
            WeekScheduleDetail detail = list[0] as WeekScheduleDetail;
            oq.AddCriterion(Expression.Eq("GWBSTreeName", detail.GWBSTreeName));
            oq.AddCriterion(Expression.Eq("InspectionPersonName", ConstObject.LoginPersonInfo.Name));
            IList alists = model.GetInspectionRecord(oq);
            lbl5.Text = "未提交的工程量确认单" + alists.Count + "项";
        }

        private void showRectificationNotice()
        {
            ObjectQuery oq = new ObjectQuery();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("DocState", DocumentState.Edit));
            IList alists = model.GetRectificationNotice(oq);
            lbl6.Text = "到期未回复的整改确认单" + alists.Count + "项";
        }
    }
}
