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
using Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.DailyInspectionRecord;
using Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.WeekScheduleConfirm;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm;
using Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrection;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    public partial class VMobileDailyWorkMenu : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        IList list = new ArrayList();
        public int pageIndex = 0;

        public VMobileDailyWorkMenu(IList alists, int pageIndex)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            InitEvent();
            this.skinEngine1.Active = false;
            //this.toolStrip1.Visible = false;
            this.list = alists;
            this.pageIndex = pageIndex;
            InitData();
        }

        public void InitEvent()
        {
            btnDailyInspection.Click += new EventHandler(btnDailyInspection_Click);
            btnWeekSchedule.Click += new EventHandler(btnWeekSchedule_Click);
            btnGWBSConfirm.Click += new EventHandler(btnGWBSConfirm_Click);
            btnDailyCorrection.Click += new EventHandler(btnDailyCorrection_Click);
        }

        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);

            this.Close();
        }

        private void VMobileDailyWorkMenu_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        void btnDailyInspection_Click(object sender, EventArgs e)
        {
            GotoBusiness("日常检查");
        }
        void btnWeekSchedule_Click(object sender, EventArgs e)
        {
            GotoBusiness("周计划确认");
        }
        void btnGWBSConfirm_Click(object sender, EventArgs e)
        {
            GotoBusiness("工程量确认");
        }
        void btnDailyCorrection_Click(object sender, EventArgs e)
        {
            GotoBusiness("整改单确认");
        }

        public void ControlsClear(bool ifDisplay)
        {
            this.btnDailyInspection.Visible = ifDisplay;
            this.btnWeekSchedule.Visible = ifDisplay;
            this.btnGWBSConfirm.Visible = ifDisplay;
            this.btnDailyCorrection.Visible = ifDisplay;
            this.toolStrip1.Visible = ifDisplay;
        }

        private void GotoBusiness(string menuName)
        {
            switch (menuName)
            {
                case "日常检查":
                    //this.ControlsClear(false);
                    VDailyInspectionRecord v_daily = new VDailyInspectionRecord(list, pageIndex);
                    //v_daily.TopLevel = false;
                    //v_daily.FormBorderStyle = FormBorderStyle.None; // 去边框标题栏等
                    //v_daily.Dock = DockStyle.Fill; // 填充
                    //v_daily.Parent = this.pnlFloor;
                    v_daily.ShowDialog();
                    break;
                case "周计划确认":
                    //this.ControlsClear(false);
                    VScheduleConfirm v_firm = new VScheduleConfirm(list, pageIndex);
                    //v_firm.TopLevel = false;
                    //v_firm.FormBorderStyle = FormBorderStyle.None; // 去边框标题栏等
                    //v_firm.Dock = DockStyle.Fill; // 填充
                    //v_firm.Parent = this.pnlFloor;
                    v_firm.ShowDialog();
                    pageIndex = v_firm.pageIndex;
                    break;
                case "工程量确认":
                    //this.ControlsClear(false);
                    IList taskConfrimList = GetTaskConfirmList();
                    if (taskConfrimList != null && taskConfrimList.Count > 0)
                    {
                        VGwbsMng v_GWBSConfirm = new VGwbsMng(taskConfrimList);
                        v_GWBSConfirm.ShowDialog();
                    }
                    else
                    {
                        VMessageBox vmb = new VMessageBox();
                        vmb.txtInformation.Text = "未找到相应的数据！";
                        vmb.ShowDialog();
                    }
                    //VGwbsMng v_GWBSConfirm = new VGwbsMng(list);
                    //v_GWBSConfirm.weekList = list;
                    //v_GWBSConfirm.pageIndex = pageIndex;
                    //v_GWBSConfirm.TopLevel = false;
                    //v_GWBSConfirm.FormBorderStyle = FormBorderStyle.None;
                    //v_GWBSConfirm.Dock = DockStyle.Fill;
                    //v_GWBSConfirm.Parent = this.pnlFloor;
                    break;
                case "整改单确认":
                    //this.ControlsClear(false);
                    VDailyCorrectionMaster v_dcmaster = new VDailyCorrectionMaster(list, pageIndex);
                    //v_dcmaster.TopLevel = false;
                    //v_dcmaster.FormBorderStyle = FormBorderStyle.None; // 去边框标题栏等
                    //v_dcmaster.Dock = DockStyle.Fill; // 填充
                    //v_dcmaster.Parent = this.pnlFloor;
                    v_dcmaster.ShowDialog();
                    break;

            }
        }

        void InitData()
        {
            if (list == null || list.Count == 0) return;
            WeekScheduleDetail weekdetail = list[pageIndex - 1] as WeekScheduleDetail;

            txtWeekSchedule.Text = ClientUtil.ToString(weekdetail.GWBSTreeName);
            txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode);
        }

        private void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Image image = Bitmap.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"Images\选择日常工作.jpg");
            Bitmap b = new Bitmap(image, this.Width, this.Height);

            Graphics dc = Graphics.FromImage((System.Drawing.Image)b);
            //将要绘制的内容绘制到dc上
            g.DrawImage(b, 0, 0);
            dc.Dispose();
        }


        //工程任务确认明细集
        private IList GetTaskConfirmList()
        {
            MConfirmmng model = new MConfirmmng();
            WeekScheduleDetail Week = list[pageIndex] as WeekScheduleDetail;
            GWBSTaskConfirm c = new GWBSTaskConfirm();
            //c.Master.ConfirmHandlePerson.Id
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("GWBSTree.Id", Week.GWBSTree.Id));
            oq.AddCriterion(Expression.Eq("Master.ConfirmHandlePerson.Id", ConstObject.LoginPersonInfo.Id));
            oq.AddCriterion(Expression.Eq("Master.DocState", VirtualMachine.Patterns.BusinessEssence.Domain.DocumentState.Edit));
            oq.AddFetchMode("WeekScheduleDetailGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("Master.Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("WeekScheduleDetailGUID.SubContractProject", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("TaskHandler.TheContractGroup", NHibernate.FetchMode.Eager);
            //oq.AddFetchMode("TaskHandler", NHibernate.FetchMode.Eager);
            IList listConfirm = model.ProductionManagementSrv.ObjectQuery(typeof(GWBSTaskConfirm), oq);
            if (listConfirm.Count <= 0)
            {
                //主表的信息
                GWBSTaskConfirmMaster curBillMaster = new GWBSTaskConfirmMaster();
                curBillMaster.ConfirmHandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.ConfirmHandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.OpgSysCode = ConstObject.TheOperationOrg.SysCode;
                curBillMaster.DocState = DocumentState.Edit;
                //curBillMaster.CreateDate = ConstObject.LoginDate;//缺省为创建实例的服务器时间，不需设置

                curBillMaster = model.ProductionManagementSrv.SaveGWBSTaskConfirmMaster(curBillMaster);
                //新建工程任务明细
                ObjectQuery oqGwbsDtl = new ObjectQuery();
                oqGwbsDtl.AddCriterion(Expression.Eq("TheGWBS.Id", Week.GWBSTree.Id));
                IList listDetail = model.ProductionManagementSrv.ObjectQuery(typeof(GWBSDetail), oqGwbsDtl);

                //针对每一个工程计划明细，添加一个工程任务确认明细
                //IList listConfirm1 = new ArrayList();
                if (listDetail != null && listDetail.Count > 0)
                {
                    IList addConfirmList = new ArrayList();
                    foreach (GWBSDetail detail in listDetail)
                    {
                        GWBSTaskConfirm taskConfirmDetail = new GWBSTaskConfirm();
                        taskConfirmDetail.Master = curBillMaster;

                        taskConfirmDetail.GWBSTree = detail.TheGWBS;
                        taskConfirmDetail.GWBSTreeName = detail.TheGWBS.Name;
                        taskConfirmDetail.GwbsSysCode = detail.TheGWBS.SysCode;

                        taskConfirmDetail.GWBSDetail = detail;
                        taskConfirmDetail.GWBSDetailName = detail.Name;

                        taskConfirmDetail.CostItem = detail.TheCostItem;

                        taskConfirmDetail.WeekScheduleDetailGUID = Week;
                        //taskConfirmDetail.TaskHandler = taskConfirmDetail.WeekScheduleDetailGUID.SubContractProject;

                        if (!string.IsNullOrEmpty(taskConfirmDetail.WeekScheduleDetailGUID.TaskCheckState))
                            taskConfirmDetail.DailyCheckState = "2" + taskConfirmDetail.WeekScheduleDetailGUID.TaskCheckState.Substring(1);

                        if (taskConfirmDetail.WeekScheduleDetailGUID.SubContractProject != null)
                        {
                            taskConfirmDetail.TaskHandler = model.ProductionManagementSrv.GetObjectById(typeof(SubContractProject), taskConfirmDetail.WeekScheduleDetailGUID.SubContractProject.Id) as SubContractProject;
                            if (taskConfirmDetail.TaskHandler != null)
                                taskConfirmDetail.TaskHandlerName = taskConfirmDetail.TaskHandler.BearerOrgName;
                        }

                        taskConfirmDetail.WorkAmountUnitId = detail.WorkAmountUnitGUID;
                        taskConfirmDetail.WorkAmountUnitName = detail.WorkAmountUnitName;

                        taskConfirmDetail.ProjectTaskType = detail.ProjectTaskTypeCode;

                        taskConfirmDetail.AccountingState = EnumGWBSTaskConfirmAccountingState.未核算;
                        taskConfirmDetail.PlannedQuantity = detail.PlanWorkAmount;
                        taskConfirmDetail.QuantityBeforeConfirm = detail.QuantityConfirmed;
                        //taskConfirmDetail.DailyCheckState = "2" + taskConfirmDetail.WeekScheduleDetailGUID.TaskCheckState.Substring(1);
                        taskConfirmDetail.ActualCompletedQuantity = 0;
                        taskConfirmDetail.ProgressBeforeConfirm = detail.ProgressConfirmed;
                        taskConfirmDetail.MaterialFeeSettlementFlag = EnumMaterialFeeSettlementFlag.不结算;

                        
                        addConfirmList.Add(taskConfirmDetail);
                        //IList resultAddConfirmList = new ArrayList();
                        //resultAddConfirmList = model.ProductionManagementSrv.SaveOrUpdate(addConfirmList);
                        //GWBSTaskConfirm resultAddConfirm = resultAddConfirmList[0] as GWBSTaskConfirm;
                        //listConfirm1.Add(resultAddConfirm);
                    }
                    return model.ProductionManagementSrv.SaveOrUpdate(addConfirmList);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return listConfirm;
            }
        }

    }
}
