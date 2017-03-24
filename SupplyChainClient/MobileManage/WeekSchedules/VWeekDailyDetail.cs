using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules
{
    public partial class VWeekDailyDetail : TBasicDataViewByMobile
    {
        MWeekPlanMng model = new MWeekPlanMng();
        private AutomaticSize automaticSize = new AutomaticSize();
        InspectionRecord record = new InspectionRecord();
        WeekScheduleDetail weekdetail = new WeekScheduleDetail();

        public VWeekDailyDetail(WeekScheduleDetail detail, InspectionRecord record1)
        {
            InitializeComponent();
            weekdetail = detail;
            record = record1;
            automaticSize.SetTag(this);
            InitData();
            InitEvent();
        }

        public void InitData()
        {
            dgMaster.RowTemplate.Height = 33;
            dgMaster.ColumnHeadersHeight = 43;
        }

        private void InitEvent()
        {
            this.btnAddDailyDetail.Click += new EventHandler(btnAddDailyDetail_Click);
            ShowMasterList();
            dgMaster.CellDoubleClick += new DataGridViewCellEventHandler(dgMaster_CellDoubleClick);
        }

        void btnAddDailyDetail_Click(object sender, EventArgs e)
        {
            record.GWBSTree = weekdetail.GWBSTree;
            record.GWBSTreeName = weekdetail.GWBSTreeName;
            record.WeekScheduleDetail = weekdetail;
            record.ProjectId = weekdetail.Master.ProjectId;

            VWeekAddDailyDetail vwdd = new VWeekAddDailyDetail(weekdetail, record);
            vwdd.ShowDialog();
            ShowMasterList();
        }

        //显示dgMaster中的数据
        private void ShowMasterList()
        {
            ObjectQuery objectquery = new ObjectQuery();
            dgMaster.Rows.Clear();
            objectquery.AddCriterion(Expression.Eq("GWBSTreeName", weekdetail.GWBSTreeName));
            IList masterlist = model.ProductionManagementSrv.GetInspectionRecord(objectquery);
            if (masterlist != null && masterlist.Count > 0)
            {
                foreach (InspectionRecord master in masterlist)
                {
                    int rowIndex = dgMaster.Rows.Add();

                    dgMaster[colInspectionDate.Name, rowIndex].Value = master.CreateDate.ToShortDateString();//检查时间
                    dgMaster[colInspectionPerson.Name, rowIndex].Value = master.CreatePersonName;//检查负责人
                    dgMaster[colInspectionSpecail.Name, rowIndex].Value = master.InspectionSpecial;//检查专业
                    dgMaster[colInspectionConclusion.Name, rowIndex].Value = master.InspectionConclusion;//检查结论
                    //整改情况
                    string strCorrectiveSign = ClientUtil.ToString(master.CorrectiveSign);
                    if (strCorrectiveSign.Equals("0"))
                    {
                        dgMaster[colCorrectionCase.Name, rowIndex].Value = "无需整改";
                    }
                    if (strCorrectiveSign.Equals("1"))
                    {
                        dgMaster[colCorrectionCase.Name, rowIndex].Value = "未整改";
                    }
                    dgMaster.Rows[rowIndex].Tag = master;
                }
            }
        }

        void dgMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgMaster[e.ColumnIndex, e.RowIndex];
            InspectionRecord record = dgMaster.CurrentRow.Tag as InspectionRecord;
            VWeekAddDailyDetail detail = new VWeekAddDailyDetail(weekdetail,record);
            detail.ShowDialog();
            ShowMasterList();
        }

        private void VWeekDailyDetail_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

    }
}
