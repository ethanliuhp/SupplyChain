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
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.WeekScheduleConfirm;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    public partial class VWeekScheduleResult : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        IList alists = new ArrayList();
        private int pageIndex = 0;

        string btnName = "";

        public VWeekScheduleResult(IList list, string btn)
        {
            this.btnName = btn;
            InitializeComponent();
            automaticSize.SetTag(this);
            this.alists = list;
            ShowWeekDetail();
            InitEvent();
        }

        public void InitEvent()
        {
            btnTheFirst.Click += new EventHandler(btnTheFirst_Click);
            btnBack.Click += new EventHandler(btnBack_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
            btnTheLast.Click += new EventHandler(btnTheLast_Click);

        }

        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
            if (btnName == "btnWeekSchedule")
            {
                VMobileDailyWorkMenu v_menu = new VMobileDailyWorkMenu(alists, pageIndex);
                v_menu.ShowDialog();
                pageIndex = v_menu.pageIndex;
            }
            if (btnName == "btnWeekPlan")
            {
                VScheduleConfirm v_confirm = new VScheduleConfirm(alists, pageIndex);
                v_confirm.ShowDialog();
                pageIndex = v_confirm.pageIndex;
            }

            
            WeekScheduleDetail weekdetail = alists[pageIndex - 1] as WeekScheduleDetail;

            txtWeekSchedule.Text = ClientUtil.ToString(weekdetail.GWBSTreeName);
            txtMainTaskContent.Text = ClientUtil.ToString(weekdetail.MainTaskContent);
            txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode);
            txtPlanDateBegin.Text = weekdetail.PlannedBeginDate.ToShortDateString();
            txtPlanDateEnd.Text = weekdetail.PlannedEndDate.ToShortDateString();
            txtSupplierName.Text = ClientUtil.ToString(weekdetail.SupplierName);
            txtTaskCheckState.Text = ClientUtil.ToString(weekdetail.TaskCheckState);
            string gwbsConfirmFlag = ClientUtil.ToString(weekdetail.GWBSConfirmFlag);
            if (gwbsConfirmFlag.Equals("0"))
            {
                txtGWBSConfirmFlag.Text = "未确认";
            }
            if (gwbsConfirmFlag.Equals("1"))
            {
                txtGWBSConfirmFlag.Text = "已确认";
            }
            string scheduleConfirmFlag = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag);
            if (scheduleConfirmFlag.Equals("0"))
            {
                txtScheduleConfirmFlag.Text = "未确认";
            }
            if (scheduleConfirmFlag.Equals("1"))
            {
                txtScheduleConfirmFlag.Text = "已确认";
            }
            lblRecord.Text = "第【" + pageIndex + "】条";
        }

        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void btnTheFirst_Click(object sender, EventArgs e)
        {
            ShowWeekDetail();
            btnBack.Enabled = false;
            btnTheFirst.Enabled = false;
            btnNext.Enabled = true;
            btnTheLast.Enabled = true;
            pageIndex = 1;
        }

        void btnBack_Click(object sender, EventArgs e)
        {
            if (pageIndex - 1 == 0) return;
            if (pageIndex - 1 == 1)
            {
                btnBack.Enabled = false;
                btnTheFirst.Enabled = false;

            }
            btnNext.Enabled = true;
            btnTheLast.Enabled = true;
            WeekScheduleDetail weekdetail = alists[pageIndex - 2] as WeekScheduleDetail;

            txtWeekSchedule.Text = ClientUtil.ToString(weekdetail.GWBSTreeName);
            txtMainTaskContent.Text = ClientUtil.ToString(weekdetail.MainTaskContent);
            txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode);
            txtPlanDateBegin.Text = weekdetail.PlannedBeginDate.ToShortDateString();
            txtPlanDateEnd.Text = weekdetail.PlannedEndDate.ToShortDateString();
            txtSupplierName.Text = ClientUtil.ToString(weekdetail.SupplierName);
            txtTaskCheckState.Text = ClientUtil.ToString(weekdetail.TaskCheckState);
            string gwbsConfirmFlag = ClientUtil.ToString(weekdetail.GWBSConfirmFlag);
            if (gwbsConfirmFlag.Equals("0"))
            {
                txtGWBSConfirmFlag.Text = "未确认";
            }
            if (gwbsConfirmFlag.Equals("1"))
            {
                txtGWBSConfirmFlag.Text = "已确认";
            }
            string scheduleConfirmFlag = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag);
            if (gwbsConfirmFlag.Equals("0"))
            {
                txtScheduleConfirmFlag.Text = "未确认";
            }
            if (gwbsConfirmFlag.Equals("1"))
            {
                txtScheduleConfirmFlag.Text = "已确认";
            }
            pageIndex--;

            lblRecord.Text = "第【" + pageIndex + "】条";
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            if (pageIndex + 1 > alists.Count) return;
            if (pageIndex + 1 == alists.Count)
            {
                btnNext.Enabled = false;
                btnTheLast.Enabled = false;

            }
            btnBack.Enabled = true;
            btnTheFirst.Enabled = true;
            WeekScheduleDetail weekdetail = alists[pageIndex] as WeekScheduleDetail;

            txtWeekSchedule.Text = ClientUtil.ToString(weekdetail.GWBSTreeName);
            txtMainTaskContent.Text = ClientUtil.ToString(weekdetail.MainTaskContent);
            txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode);
            txtPlanDateBegin.Text = weekdetail.PlannedBeginDate.ToShortDateString();
            txtPlanDateEnd.Text = weekdetail.PlannedEndDate.ToShortDateString();
            txtSupplierName.Text = ClientUtil.ToString(weekdetail.SupplierName);
            txtTaskCheckState.Text = ClientUtil.ToString(weekdetail.TaskCheckState);
            string gwbsConfirmFlag = ClientUtil.ToString(weekdetail.GWBSConfirmFlag);
            if (gwbsConfirmFlag.Equals("0"))
            {
                txtGWBSConfirmFlag.Text = "未确认";
            }
            if (gwbsConfirmFlag.Equals("1"))
            {
                txtGWBSConfirmFlag.Text = "已确认";
            }
            string scheduleConfirmFlag = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag);
            if (gwbsConfirmFlag.Equals("0"))
            {
                txtScheduleConfirmFlag.Text = "未确认";
            }
            if (gwbsConfirmFlag.Equals("1"))
            {
                txtScheduleConfirmFlag.Text = "已确认";
            }
            pageIndex++;

            lblRecord.Text = "第【" + pageIndex + "】条";
        }

        void btnTheLast_Click(object sender, EventArgs e)
        {
            btnBack.Enabled = true;
            btnTheFirst.Enabled = true;
            btnNext.Enabled = false;
            btnTheLast.Enabled = false;
            WeekScheduleDetail weekdetail = alists[alists.Count - 1] as WeekScheduleDetail;

            txtWeekSchedule.Text = ClientUtil.ToString(weekdetail.GWBSTreeName);
            txtMainTaskContent.Text = ClientUtil.ToString(weekdetail.MainTaskContent);
            txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode);
            txtPlanDateBegin.Text = weekdetail.PlannedBeginDate.ToShortDateString();
            txtPlanDateEnd.Text = weekdetail.PlannedEndDate.ToShortDateString();
            txtSupplierName.Text = ClientUtil.ToString(weekdetail.SupplierName);
            txtTaskCheckState.Text = ClientUtil.ToString(weekdetail.TaskCheckState);
            string gwbsConfirmFlag = ClientUtil.ToString(weekdetail.GWBSConfirmFlag);
            if (gwbsConfirmFlag.Equals("0"))
            {
                txtGWBSConfirmFlag.Text = "未确认";
            }
            if (gwbsConfirmFlag.Equals("1"))
            {
                txtGWBSConfirmFlag.Text = "已确认";
            }
            string scheduleConfirmFlag = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag);
            if (gwbsConfirmFlag.Equals("0"))
            {
                txtScheduleConfirmFlag.Text = "未确认";
            }
            if (gwbsConfirmFlag.Equals("1"))
            {
                txtScheduleConfirmFlag.Text = "已确认";
            }
            pageIndex = alists.Count;

            lblRecord.Text = "第【" + alists.Count + "】条";
        }

        void ShowWeekDetail()
        {
            pageIndex = 1;
            if (alists.Count == 0 || alists == null) return;
            WeekScheduleDetail weekdetail = alists[0] as WeekScheduleDetail;

            txtWeekSchedule.Text = ClientUtil.ToString(weekdetail.GWBSTreeName);
            txtMainTaskContent.Text = ClientUtil.ToString(weekdetail.MainTaskContent);
            txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode);
            txtPlanDateBegin.Text = weekdetail.PlannedBeginDate.ToShortDateString();
            txtPlanDateEnd.Text = weekdetail.PlannedEndDate.ToShortDateString();
            txtSupplierName.Text = ClientUtil.ToString(weekdetail.SupplierName);
            txtTaskCheckState.Text = ClientUtil.ToString(weekdetail.TaskCheckState);
            string gwbsConfirmFlag = ClientUtil.ToString(weekdetail.GWBSConfirmFlag);
            if (gwbsConfirmFlag.Equals("0"))
            {
                txtGWBSConfirmFlag.Text = "未确认";
            }
            if (gwbsConfirmFlag.Equals("1"))
            {
                txtGWBSConfirmFlag.Text = "已确认";
            }
            string scheduleConfirmFlag = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag);
            if (scheduleConfirmFlag.Equals("0"))
            {
                txtScheduleConfirmFlag.Text = "未确认";
            }
            if (scheduleConfirmFlag.Equals("1"))
            {
                txtScheduleConfirmFlag.Text = "已确认";
            }
            lblRecord.Text = "第【" + 1 + "】条";
            lblRecordTotal.Text = "共【" + alists.Count + "】条";
        }

        private void VWeekScheduleResult_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
