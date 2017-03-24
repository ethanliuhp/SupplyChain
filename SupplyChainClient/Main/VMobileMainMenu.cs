using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.MobileManage;
using Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.MobileManage.OftenWords;
using Application.Business.Erp.SupplyChain.Client.MobileManage.FilesUpload;
using Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsManage;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;
using Application.Business.Erp.SupplyChain.Client.MobileManage.GwbsConfirm;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery;
using Application.Business.Erp.SupplyChain.Client.MobileManage.ProInRecord;
using Application.Business.Erp.SupplyChain.Client.MobileManage.DailyInspection;
using Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrectioSearch;


namespace Application.Business.Erp.SupplyChain.Client.Main
{
    public partial class VMobileMainMenu : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        IList list = new ArrayList();
        public VMobileMainMenu()
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            this.toolStrip1.Visible = false;
            this.lblHeaderLine.Visible = false;
            this.skinEngine1.Active = false;
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            this.TtxtTheSysRole.Text = ConstObject.TheSysRole.RoleName;
        }

        private void InitEvent()
        {
            this.btnWeekPlan.Click += new EventHandler(btnWeekPlan_Click);
            this.btnApp.Click += new EventHandler(btnOftenWord_Click);
            this.btnWeekSchedule.Click += new EventHandler(btnWeekSchedule_Click);
            this.btnGwbs.Click += new EventHandler(btnGwbs_Click);
            this.btnConfirmSearch.Click += new EventHandler(btnConfirmSearch_Click);
            this.btnCheck.Click += new EventHandler(btnCheck_Click);
            this.btnDailyInspection.Click += new EventHandler(btnDailyInspection_Click);
            this.btnWeekSchedule.MouseHover +=new EventHandler(btnWeekSchedule_MouseHover);
            this.btnDailyOperation.Click += new EventHandler(btnDailyOperation_Click);
            this.btnCorrection.Click+=new EventHandler(btnCorrection_Click);
        }

        void btnWeekSchedule_MouseHover(object sender,EventArgs e)
        {
            
        }

        private void VMobileMainMenu_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

        }
        void btnCorrection_Click(object sender, EventArgs e)
        {
            GotoBusiness("整改单查询");
        }
        void btnCheck_Click(object sender, EventArgs e)
        {
            GotoBusiness("专业检查");
        }
        void btnWeekPlan_Click(object sender, EventArgs e)
        {
            GotoBusiness("周进度计划");
        }
        void btnOftenWord_Click(object sender, EventArgs e)
        {
            GotoBusiness("表单审批");
        }
        void btnWeekSchedule_Click(object sender, EventArgs e)
        {
            GotoBusiness("工作任务查询");
        }
        void btnGwbs_Click(object sender, EventArgs e)
        {
            GotoBusiness("GWBS查询");
        }
        void btnConfirmSearch_Click(object sender, EventArgs e)
        {
            GotoBusiness("工程量确认查询");
        }
        void btnDailyInspection_Click(object sender, EventArgs e)
        {
            GotoBusiness("日常检查");
        }
        void btnDailyOperation_Click(object sender, EventArgs e)
        {
            GotoBusiness("日常操作快速通道");
        }

        public void ControlsClear(bool ifDisplay)
        {
            this.btnWeekSchedule.Visible = ifDisplay;
            this.btnGwbs.Visible = ifDisplay;
            this.btnApp.Visible = ifDisplay;
            this.btnWeekPlan.Visible = ifDisplay;
            this.btnConfirmSearch.Visible = ifDisplay;
            this.btnCheck.Visible = ifDisplay;
            this.btnDailyInspection.Visible = ifDisplay;
            this.btnCorrection.Visible = ifDisplay;
            this.toolStrip1.Visible = ifDisplay;
        }

        private void GotoBusiness(string menuName)
        {
            switch (menuName)
            {
                case "周进度计划":
                    VWeekScheduleSearch v_week = new VWeekScheduleSearch("btnWeekPlan");
                    v_week.Show();
                    break;
                case "整改单查询":
                    VDailyCorrectionSearch v_dcs = new VDailyCorrectionSearch("btnCorrection");
                    v_dcs.Show();
                    break;
                case "表单审批":
                    //this.ControlsClear(false);
                    VAppPlatMng v_test = new VAppPlatMng();
                    //v_test.TopLevel = false;
                    //v_test.FormBorderStyle = FormBorderStyle.None; // 去边框标题栏等
                    //v_test.Dock = DockStyle.Fill; // 填充
                    //v_test.Parent = this.pnlFloor;

                    v_test.Show();
                    break;
                case "工作任务查询":
                    //this.ControlsClear(false);
                    VWeekScheduleSearch v_search = new VWeekScheduleSearch("btnWeekSchedule");
                    //v_search.TopLevel = false;
                    //v_search.FormBorderStyle = FormBorderStyle.None; // 去边框标题栏等
                    //v_search.Dock = DockStyle.Fill; // 填充
                    //v_search.Parent = this.pnlFloor;

                    v_search.Show();
                    break;
                case "GWBS查询":
                    //this.ControlsClear(false);
                    VGwbsManage v_gwbs = new VGwbsManage();
                    //v_gwbs.TopLevel = false;
                    //v_gwbs.FormBorderStyle = FormBorderStyle.None; // 去边框标题栏等
                    //v_gwbs.Dock = DockStyle.Fill; // 填充
                    //v_gwbs.Parent = this.pnlFloor;

                    v_gwbs.Show();
                    break;
                case "工程量确认查询":
                    //this.ControlsClear(false);
                    VGWBSTreeConfirmSelect v_Confirm = new VGWBSTreeConfirmSelect();
                    //VGwbsMng v_Confirm = new VGwbsMng(list);
                    //v_Confirm.TopLevel = false;
                    //v_Confirm.FormBorderStyle = FormBorderStyle.None; // 去边框标题栏等
                    //v_Confirm.Dock = DockStyle.Fill; // 填充
                    //v_Confirm.Parent = this.pnlFloor;

                    v_Confirm.Show();
                    break;
                case "专业检查":
                    //this.ControlsClear(false);
                    VProInRecordMng vp = new VProInRecordMng();
                    //vp.TopLevel = false;
                    //vp.FormBorderStyle = FormBorderStyle.None; // 去边框标题栏等
                    //vp.Dock = DockStyle.Fill; // 填充
                    //vp.Parent = this.pnlFloor;

                    vp.Show();
                    break;
                case "日常检查":
                    VInspectionRecord v_record = new VInspectionRecord();
                    v_record.Show();
                    break;
                case "日常操作快速通道":
                    VDailyOperation v_tion = new VDailyOperation();
                    v_tion.Show();
                    break;
            }
        }

        private void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Image image = Bitmap.FromFile(AppDomain.CurrentDomain.BaseDirectory + @"Images\移动信息系统.jpg");
            Bitmap b = new Bitmap(image, this.Width, this.Height);

            Graphics dc = Graphics.FromImage((System.Drawing.Image)b);
            //将要绘制的内容绘制到dc上
            g.DrawImage(b, 0, 0);
            dc.Dispose();
        }

    }
}
