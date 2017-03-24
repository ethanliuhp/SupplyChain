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
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery.WeekScheduleConfirm
{
    public partial class VScheduleConfirmResult : TBasicToolBarByMobile
    {
        private AutomaticSize automaticSize = new AutomaticSize();
        private MProjectTaskQuery model = new MProjectTaskQuery();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        IList list = new ArrayList();
        private int pageIndex = 0;

        public VScheduleConfirmResult(IList alists, int pageIndex)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            this.list = alists;
            this.pageIndex = pageIndex;
            InitData();
            Contents();
            showButton();
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

        void InitData()
        {
            if (list.Count == 0 || list == null) return;
            weekdetail = list[pageIndex - 1] as WeekScheduleDetail;

            txtCompletionAnalysis.Text = ClientUtil.ToString(weekdetail.CompletionAnalysis);

            lblRecord.Text = "第【" + 1 + "】条";
            lblRecordTotal.Text = "共【" + 1 + "】条";
        }

        public override void 功能菜单3Item_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = false;
                if (string.IsNullOrEmpty(weekdetail.Id))
                {
                    flag = true;
                }
                weekdetail.CompletionAnalysis = ClientUtil.ToString(txtCompletionAnalysis.Text);
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
                bool flag = false;
                if (string.IsNullOrEmpty(weekdetail.Id))
                {
                    flag = true;
                }
                weekdetail.CompletionAnalysis = ClientUtil.ToString(txtCompletionAnalysis.Text);
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
            this.txtCompletionAnalysis.Text = "";
        }

        private void VScheduleConfirmResult_Load(object sender, EventArgs e)
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
