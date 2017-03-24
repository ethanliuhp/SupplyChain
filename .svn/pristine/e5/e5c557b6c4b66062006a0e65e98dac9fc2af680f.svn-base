using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Core;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.WeekScheduleConfirm
{
    public partial class VScheduleConfirm : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        private MProjectTaskQuery model = new MProjectTaskQuery();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        IList alists = new ArrayList();
        public int pageIndex = 0;

        public VScheduleConfirm(IList list, int pageIndex)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            this.alists = list;
            this.pageIndex = pageIndex;
            //this.skinEngine1.Active = false;
            InitData();
            InitEvent();
            Contents();
            showButton();
        }

        public void InitEvent()
        {
            //btnTheFirst.Click += new EventHandler(btnTheFirst_Click);
            //btnBack.Click += new EventHandler(btnBack_Click);
            //btnNext.Click += new EventHandler(btnNext_Click);
            //btnTheLast.Click += new EventHandler(btnTheLast_Click);
        }

        public override void TBtnPageDown_Click(object sender, EventArgs e)
        {
            VScheduleConfirmResult v_sresult = new VScheduleConfirmResult(alists, pageIndex);
            v_sresult.ShowDialog();
        }

        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Contents()
        {
            this.功能菜单1Item.Visible = true;
            this.功能菜单1Item.Text = "放弃编辑内容";
            this.功能菜单2Item.Visible = true;
            this.功能菜单2Item.Text = "提交编辑内容";
            this.功能菜单3Item.Visible = true;
            this.功能菜单3Item.Text = "保存编辑内容";
        }

        private bool ViewToModel()
        {
            weekdetail.ActualBeginDate = ClientUtil.ToDateTime(this.dtpActualDateBegin.Text);
            weekdetail.ActualEndDate = ClientUtil.ToDateTime(this.dtpActualDateEnd.Text);
            weekdetail.ScheduleConfirmDate = DateTime.Now;

            return true;
        }

        public override void 功能菜单3Item_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ViewToModel()) return;
                bool flag = false;
                if (string.IsNullOrEmpty(weekdetail.Id))
                {
                    flag = true;
                }
                weekdetail.ScheduleConfirmFlag = ClientUtil.ToInt("0");
                weekdetail = model.SaveOrUpdateByDao(weekdetail) as WeekScheduleDetail;
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据保存成功！";
                v_show.ShowDialog();
            }
            catch (Exception ex)
            {
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据保存错误！";
                v_show.ShowDialog();
            }
        }

        public override void 功能菜单2Item_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ViewToModel()) return;
                bool flag = false;
                if (string.IsNullOrEmpty(weekdetail.Id))
                {
                    flag = true;
                }
                weekdetail.ScheduleConfirmFlag = ClientUtil.ToInt("1");
                weekdetail = model.SaveOrUpdateByDao(weekdetail) as WeekScheduleDetail;
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据已成功提交！";
                v_show.ShowDialog();
            }
            catch (Exception ex)
            {
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据提交错误！";
                v_show.ShowDialog();
            }
        }

        public override void 功能菜单1Item_Click(object sender, EventArgs e)
        {
            txtTaskCompletedPercentNow.Text = "";
        }

        //void btnTheFirst_Click(object sender, EventArgs e)
        //{
        //    InitData();
        //    btnBack.Enabled = false;
        //    btnTheFirst.Enabled = false;
        //    btnNext.Enabled = true;
        //    btnTheLast.Enabled = true;
        //    pageIndex = 1;
        //}

        //void btnBack_Click(object sender, EventArgs e)
        //{
        //    if (pageIndex - 1 == 0) return;
        //    if (pageIndex - 1 == 1)
        //    {
        //        btnBack.Enabled = false;
        //        btnTheFirst.Enabled = false;

        //    }
        //    btnNext.Enabled = true;
        //    btnTheLast.Enabled = true;
        //    WeekScheduleDetail weekdetail = alists[pageIndex - 2] as WeekScheduleDetail;

        //    txtProjectTask.Text = ClientUtil.ToString(weekdetail.GWBSTreeName);
        //    txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode);
        //    dtpActualDateBegin.Value = weekdetail.ActualBeginDate;
        //    dtpActualDateEnd.Value = weekdetail.ActualEndDate;
        //    txtTaskCompletedPercent.Text = ClientUtil.ToString(weekdetail.TaskCompletedPercent);
        //    txtDocState.Text = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag);
        //    dtpRecordDate.Value = weekdetail.ScheduleConfirmDate;
        //    pageIndex--;
        //}

        //void btnNext_Click(object sender, EventArgs e)
        //{
        //    if (pageIndex + 1 > alists.Count) return;
        //    if (pageIndex + 1 == alists.Count)
        //    {
        //        btnNext.Enabled = false;
        //        btnTheLast.Enabled = false;

        //    }
        //    btnBack.Enabled = true;
        //    btnTheFirst.Enabled = true;
        //    WeekScheduleDetail weekdetail = alists[pageIndex] as WeekScheduleDetail;

        //    txtProjectTask.Text = ClientUtil.ToString(weekdetail.GWBSTreeName);
        //    txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode);
        //    dtpActualDateBegin.Value = weekdetail.ActualBeginDate;
        //    dtpActualDateEnd.Value = weekdetail.ActualEndDate;
        //    txtTaskCompletedPercent.Text = ClientUtil.ToString(weekdetail.TaskCompletedPercent);
        //    txtDocState.Text = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag);
        //    dtpRecordDate.Value = weekdetail.ScheduleConfirmDate;
        //    pageIndex++;
        //}

        //void btnTheLast_Click(object sender, EventArgs e)
        //{
        //    btnBack.Enabled = true;
        //    btnTheFirst.Enabled = true;
        //    btnNext.Enabled = false;
        //    btnTheLast.Enabled = false;
        //    WeekScheduleDetail weekdetail = alists[alists.Count - 1] as WeekScheduleDetail;

        //    txtProjectTask.Text = ClientUtil.ToString(weekdetail.GWBSTreeName);
        //    txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode);
        //    dtpActualDateBegin.Value = weekdetail.ActualBeginDate;
        //    dtpActualDateEnd.Value = weekdetail.ActualEndDate;
        //    txtTaskCompletedPercent.Text = ClientUtil.ToString(weekdetail.TaskCompletedPercent);
        //    txtDocState.Text = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag);
        //    dtpRecordDate.Value = weekdetail.ScheduleConfirmDate;
        //    pageIndex = alists.Count;
        //}


        void InitData()
        {
            if (alists.Count == 0 || alists == null) return;
            weekdetail = alists[pageIndex - 1] as WeekScheduleDetail;

            txtProjectTask.Text = ClientUtil.ToString(weekdetail.GWBSTreeName);
            txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), weekdetail.GWBSTreeName, weekdetail.GWBSTreeSysCode);
            dtpActualDateBegin.Value = weekdetail.ActualBeginDate;
            dtpActualDateEnd.Value = weekdetail.ActualEndDate;
            txtTaskCompletedPercent.Text = ClientUtil.ToString(weekdetail.TaskCompletedPercent);
            //txtDocState.Text = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag)  ;
            string scheduleConfirmFlag = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag);
            if (scheduleConfirmFlag.Equals("0"))
            {
                txtDocState.Text = "未确认";
            }
            if (scheduleConfirmFlag.Equals("1"))
            {
                txtDocState.Text = "已确认";
            }
            dtpRecordDate.Value = weekdetail.ScheduleConfirmDate;

            lblRecord.Text = "第【" + 1 + "】条";
            lblRecordTotal.Text = "共【" + 1 + "】条";
        }

        private void VScheduleConfirm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        public void showButton()
        {
            btnTheFirst.Visible = false;
            btnBack.Visible = false;
            btnNext.Visible = false;
            btnTheLast.Visible = false;
        }
    }
}
