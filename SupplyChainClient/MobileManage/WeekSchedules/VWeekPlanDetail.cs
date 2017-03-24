using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules
{
    public partial class VWeekPlanDetail : TBasicDataViewByMobile
    {
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();
        MProductionMng model = new MProductionMng();
        private AutomaticSize automaticSize = new AutomaticSize();
        InspectionRecord record = new InspectionRecord();

        public VWeekPlanDetail(WeekScheduleDetail detail)
        {
            InitializeComponent();
            automaticSize.SetTag(this);
            weekdetail = detail;
            InntData();
            InntEvent();
            //TBtnBack();
        }

        private void InntEvent()
        {
            this.btnAsscss.Click += new EventHandler(btnAsscss_Click);
            btnDailyRescore.Click += new EventHandler(btnDailyRescore_Click);
        }

        //public void TBtnBack()
        //{
        //    this.Close();
        //}

        private void InntData()
        {
            txtTask.Text = weekdetail.GWBSTreeName.ToString();
            txtTaskAccount.Text = ClientUtil.ToString(weekdetail.MainTaskContent);
            if (weekdetail.PlannedBeginDate.ToShortDateString().Equals("1900-1-1"))
            {
                txtDatePlanB.Text = "";
            }
            else
            {
                txtDatePlanB.Text = weekdetail.PlannedBeginDate.ToShortDateString();
            }
            if (weekdetail.PlannedEndDate.ToShortDateString().Equals("1900-1-1"))
            {
                txtDatePlanE.Text = "";
            }
            else
            {
                txtDatePlanE.Text = weekdetail.PlannedEndDate.ToShortDateString();
            }
            txtPlanWorkTime.Text = ClientUtil.ToString(weekdetail.PlannedDuration);
            if (weekdetail.ActualBeginDate.ToShortDateString().Equals("1900-1-1"))
            {
                txtDateActualB.Text = "";
            }
            else
            {
                txtDateActualB.Text = weekdetail.ActualBeginDate.ToShortDateString();
            }
            if (weekdetail.ActualEndDate.ToShortDateString().Equals("1900-1-1"))
            {
                txtDateActualE.Text = "";
            }
            else
            {
                txtDateActualE.Text = weekdetail.ActualEndDate.ToShortDateString();
            }
            txtActualWorkTime.Text = ClientUtil.ToString(weekdetail.ActualDuration);
            txtCumulative.Text = ClientUtil.ToString(weekdetail.TaskCompletedPercent);
            txtPerfomance.Text = ClientUtil.ToString(weekdetail.CompletionAnalysis);
            if (weekdetail.GWBSConfirmDate.ToShortDateString().Equals("1900-1-1"))
            {
                txtDateWorkConfirm.Text = "";
            }
            txtGWBConfirm.Text = ClientUtil.ToString(weekdetail.ScheduleConfirmFlag);
            //if (ClientUtil.ToString(weekdetail.TaskCheckState).Equals("1111111"))
            string taskCheckState = ClientUtil.ToString(weekdetail.TaskCheckState);
            if (taskCheckState == null) return;
            if (taskCheckState.Equals("2222222"))
            {
                txtCheckState.Text = "检查全部通过";
            }
            else
            {
                txtCheckState.Text = "检查未能全部通过";
            }
            
        }

        void btnAsscss_Click(object sender, EventArgs e)
        {
            if (weekdetail.GWBSConfirmFlag.Equals(0))
            {
                VWeekPlanExamine vwpe = new VWeekPlanExamine(weekdetail);
                vwpe.ShowDialog();
                InntData();
            }
        }

        void btnDailyRescore_Click(object sender, EventArgs e)
        {
            VWeekDailyDetail vwdd = new VWeekDailyDetail(weekdetail, record);
            vwdd.ShowDialog();
            InntData();
        }

        private void VWeekPlanDetail_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

    }
}
