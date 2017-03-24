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
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.DailyInspectionRecord
{
    public partial class VDailyInspectionResult : TBasicToolBarByMobile
    {
        private MProjectTaskQuery model = new MProjectTaskQuery();
        private AutomaticSize automaticSize = new AutomaticSize();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        InspectionRecord record = new InspectionRecord();
        IList list = new ArrayList();
        IList lists = new ArrayList();
        private int pageIndex = 0;
        public int i = 0;

        public VDailyInspectionResult(IList list, int pageIndex, IList lists, int i)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            Contents();
            this.list = list;
            this.pageIndex = pageIndex;
            this.lists = lists;
            this.i = i;
            InitData();
            InitEvent();
        }

        public void InitEvent()
        {
            btnTheFirst.Click += new EventHandler(btnTheFirst_Click);
            btnBack.Click += new EventHandler(btnBack_Click);
            btnNext.Click += new EventHandler(btnNext_Click);
            btnTheLast.Click += new EventHandler(btnTheLast_Click);
        }

        public override void TBtnPageUp_Click(object sender, EventArgs e)
        {
            base.TBtnPageUp_Click(sender, e);

            this.Close();
        }

        public void Contents()
        {
            this.功能菜单1Item.Visible = true;
            this.功能菜单1Item.Text = "放弃编辑";
            this.功能菜单2Item.Visible = false;
            this.功能菜单2Item.Text = "提交记录";
            this.功能菜单3Item.Visible = false;
            this.功能菜单3Item.Text = "删除记录";
            this.功能菜单4Item.Visible = true;
            this.功能菜单4Item.Text = "保存记录";
            this.功能菜单5Item.Visible = true;
            this.功能菜单5Item.Text = "新增记录";
        }

        public override void 功能菜单1Item_Click(object sender, EventArgs e)
        {
            this.txtInspectionContent.Text = "";
        }

        public override void 功能菜单4Item_Click(object sender, EventArgs e)
        {
            try
            {
                InspectionRecord master = new InspectionRecord();
                if (record.Id != null)
                {
                    master = record;
                }
                else
                {
                    master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
                    master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                    master.HandlePerson = ConstObject.LoginPersonInfo;
                    master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                    CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                    master.ProjectId = projectInfo.Id;
                    master.ProjectName = projectInfo.Name;

                    //工程任务
                    if (txtProjectTask.Tag != null)
                    {
                        master.GWBSTree = weekdetail.GWBSTree;
                        master.GWBSTreeName = weekdetail.GWBSTreeName;
                    }
                    master.WeekScheduleDetail = weekdetail;
                }
                master.InspectionContent = ClientUtil.ToString(txtInspectionContent.Text);
                master = model.SaveOrUpdateByDao(master) as InspectionRecord;
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "保存成功！";
                v_show.ShowDialog();
            }
            catch (Exception ex)
            {
                VMessageBox v_show = new VMessageBox();
                v_show.txtInformation.Text = "数据保存错误！";
                v_show.ShowDialog();
            }
        }

        void btnTheFirst_Click(object sender, EventArgs e)
        {
            InitData();
            btnBack.Enabled = false;
            btnTheFirst.Enabled = false;
            btnNext.Enabled = true;
            btnTheLast.Enabled = true;
            i = 1;
            lblRecord.Text = "第【" + i + "】条";
        }

        void btnBack_Click(object sender, EventArgs e)
        {
            if (i - 1 == 0) return;
            if (i - 1 == 1)
            {
                btnBack.Enabled = false;
                btnTheFirst.Enabled = false;

            }
            btnNext.Enabled = true;
            btnTheLast.Enabled = true;
            if (lists == null || lists.Count == 0) return;
            record = lists[i - 2] as InspectionRecord;

            txtProjectTask.Text = ClientUtil.ToString(record.WeekScheduleDetail.GWBSTreeName);
            txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.WeekScheduleDetail.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            txtInspectionContent.Text = ClientUtil.ToString(record.InspectionContent);
            i--;

            lblRecord.Text = "第【" + i + "】条";
        }

        void btnNext_Click(object sender, EventArgs e)
        {

            if (weekdetail != null)
            {
                if (i + 1 > lists.Count) return;
                if (i + 1 == lists.Count)
                {
                    btnNext.Enabled = false;
                    btnTheLast.Enabled = false;

                }
                btnBack.Enabled = true;
                btnTheFirst.Enabled = true;
                if (lists == null || lists.Count == 0) return;
                record = lists[i] as InspectionRecord; ;

                txtProjectTask.Text = ClientUtil.ToString(record.WeekScheduleDetail.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.WeekScheduleDetail.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            }
            if (weekdetail == null)
            {
                if (i + 1 > list.Count) return;
                if (i + 1 == list.Count)
                {
                    btnNext.Enabled = false;
                    btnTheLast.Enabled = false;

                }
                btnBack.Enabled = true;
                btnTheFirst.Enabled = true;
                if (list == null || list.Count == 0) return;
                record = list[i] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            }
            txtInspectionContent.Text = ClientUtil.ToString(record.InspectionContent);
            i++;

            lblRecord.Text = "第【" + i + "】条";
        }

        void btnTheLast_Click(object sender, EventArgs e)
        {
            btnBack.Enabled = true;
            btnTheFirst.Enabled = true;
            btnNext.Enabled = false;
            btnTheLast.Enabled = false;

            if (weekdetail != null)
            {
                if (lists == null || lists.Count == 0) return;
                record = lists[lists.Count - 1] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.WeekScheduleDetail.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.WeekScheduleDetail.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            }
            if (weekdetail == null)
            {
                if (list == null || list.Count == 0) return;
                record = list[list.Count - 1] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            }


            txtProjectTask.Text = ClientUtil.ToString(record.WeekScheduleDetail.GWBSTreeName);
            txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.WeekScheduleDetail.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);
            txtInspectionContent.Text = ClientUtil.ToString(record.InspectionContent);

            lblRecord.Text = "第【" + lists.Count + "】条";
        }

        void InitData()
        {
            weekdetail = list[0] as WeekScheduleDetail;
            if (weekdetail != null)
            {
                if (lists == null || lists.Count == 0) return;
                record = lists[i - 1] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.WeekScheduleDetail.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.WeekScheduleDetail.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);

                lblRecordTotal.Text = "共【" + lists.Count + "】条";
            }
            if (weekdetail == null)
            {
                if (list == null || list.Count == 0) return;
                record = list[i - 1] as InspectionRecord;

                txtProjectTask.Text = ClientUtil.ToString(record.GWBSTreeName);
                txtTaskFullPath.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), record.GWBSTreeName, (record.GWBSTree as GWBSTree).SysCode);

                lblRecordTotal.Text = "共【" + list.Count + "】条";
            }
            txtInspectionContent.Text = ClientUtil.ToString(record.InspectionContent);

            lblRecord.Text = "第【" + i + "】条";
            
        }

        private void VDailyInspectionResult_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
    }
}
