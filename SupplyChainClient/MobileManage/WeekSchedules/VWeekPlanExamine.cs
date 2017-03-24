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
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules
{
    public partial class VWeekPlanExamine : TBasicDataViewByMobile
    {
        MWeekPlanMng model = new MWeekPlanMng();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        private AutomaticSize automaticSize = new AutomaticSize(); 

        public VWeekPlanExamine(WeekScheduleDetail weekdetail1)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            weekdetail = weekdetail1;
            InitEvent();
            showData();
           
        }

        void btnGiveUp_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void InitEvent()
        {
            btnSave.Click +=new EventHandler(btnSave_Click);
            btnGiveUp.Click += new EventHandler(btnGiveUp_Click);
            dtpActualBeginDate.CloseUp +=new EventHandler(dtpActualBeginDate_CloseUp);
            dtpActualEndDate.CloseUp += new EventHandler(dtpActualBeginDate_CloseUp);
        }

        private void dtpActualBeginDate_CloseUp(object sender, EventArgs e)
        {
            showData();
        }

        void showData()
        {
            DateTime actubegindate = dtpActualBeginDate.Value.Date;
            DateTime actuenddate = dtpActualEndDate.Value.Date;
            if (actuenddate == actubegindate)
            {
                txtActualWorkTime.Text = "1";
            }
            else
            {
                TimeSpan date = actuenddate - actubegindate;
                string[] date1 = date.ToString().Split('.');
                int date2 = Convert.ToInt32(date1[0]) + 1;
                string date3 = date2.ToString();
                txtActualWorkTime.Text = date3;
            }
        }

        private bool ViewToModel()
        {
            try
            {
                weekdetail.ActualBeginDate = ClientUtil.ToDateTime(this.dtpActualBeginDate.Text);
                weekdetail.ActualEndDate = ClientUtil.ToDateTime(this.dtpActualEndDate.Text);
                weekdetail.ActualDuration = ClientUtil.ToInt(this.txtActualWorkTime.Text);
                weekdetail.TaskCompletedPercent = ClientUtil.ToDecimal(this.txtCumulative.Text);
                weekdetail.CompletionAnalysis = ClientUtil.ToString(this.txtPerfomance.Text);

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }

        }

        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ViewToModel()) return;
                bool flag = false;
                if (string.IsNullOrEmpty(weekdetail.Id))
                {
                    flag = true;
                }
                weekdetail = model.ProductionManagementSrv.SaveOrUpdateByDao(weekdetail) as WeekScheduleDetail;
                MessageBox.Show("保存成功！");
                this.Close();
                ClearView();
            }
            catch (Exception err)
            {
                MessageBox.Show("数据保存错误！");
            }
        }

        public void ClearView()
        {
            this.dtpActualBeginDate.Text = "";
            this.dtpActualEndDate.Text="";
            this.txtActualWorkTime.Text ="";
            this.txtCumulative.Text="";
            this.txtPerfomance.Text = "";
        }

        private void VWeekPlanExamine_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }
        
    }
}
