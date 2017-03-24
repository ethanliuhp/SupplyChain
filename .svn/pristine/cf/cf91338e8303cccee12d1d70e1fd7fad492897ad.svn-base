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
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    /// <summary>
    /// 周计划确认
    /// </summary>
    public partial class VWeekScheduleConfirmBak : TBasicDataView
    {
        MProductionMng model = new MProductionMng();

        public VWeekScheduleConfirmBak()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        private void InitData()
        {
            dtpDateBegin.Value = DateTime.Now.AddDays(-10);
            colPlanConformity.DataSource = Enum.GetNames(typeof(EnumWeekSchedulePlanConformity));
            string[] lockCols = new string[] { colGWBSTree.Name,colMainTaskContent.Name,colMonday.Name,colTuesday.Name,colWednesday.Name,colThursday.Name,colFriday.Name,colSaturday.Name,colSunday.Name,colPlannedDuration.Name,colPlannedWrokload.Name,colActualDuration.Name, colActualWorkload.Name, colOBSService.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
        }

        private void InitEvent()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            btnConfirm.Click += new EventHandler(btnConfirm_Click);
            dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
        }

        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colActualBeginDate.Name)
            {
                DateTime dt = DateTime.Parse(e.FormattedValue + "");
                DateTime endDate = (DateTime)dgDetail[colActualEndDate.Name, e.RowIndex].Value;
                if (dt.Date.CompareTo(endDate.Date) > 0)
                {
                    MessageBox.Show("实际开始时间不能大于实际结束时间。");
                    e.Cancel = true;
                    return;
                }
                dgDetail[colActualDuration.Name,e.RowIndex].Value = (int)((endDate.Date - dt.Date).TotalDays)+1;
            }
            else if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colActualEndDate.Name)
            {
                DateTime beginDate = (DateTime)dgDetail[colActualBeginDate.Name, e.RowIndex].Value;
                DateTime endDate = DateTime.Parse(e.FormattedValue + "");
                if (endDate.Date.CompareTo(beginDate.Date) < 0)
                {
                    MessageBox.Show("实际结束时间不能小于实际开始时间。");
                    e.Cancel = true;
                    return;
                }
                dgDetail[colActualDuration.Name, e.RowIndex].Value = (int)((endDate.Date - beginDate.Date).TotalDays)+1;
            }
        }

        void btnConfirm_Click(object sender, EventArgs e)
        {
            if (dgMaster.CurrentRow == null) return;
            if (!Valid()) return;
            WeekScheduleMaster master = dgMaster.CurrentRow.Tag as WeekScheduleMaster;
            bool needSave = false;
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                WeekScheduleDetail obj = dr.Tag as WeekScheduleDetail;
                if (obj.GWBSConfirmFlag == 1) continue;
                object progressO = dr.Cells[colTaskCompletedPercent.Name].Value;
                if (progressO == null || progressO.ToString().Equals("0")) continue;

                needSave = true;

                obj.ActualBeginDate = ((DateTime)dr.Cells[colActualBeginDate.Name].Value).Date;
                obj.ActualEndDate = ((DateTime)dr.Cells[colActualEndDate.Name].Value).Date;
                obj.ActualDuration = ClientUtil.ToInt(dr.Cells[colActualDuration.Name].Value);
                obj.TaskCompletedPercent = ClientUtil.ToDecimal(dr.Cells[colTaskCompletedPercent.Name].Value);
                obj.PlanConformity = dr.Cells[colPlanConformity.Name].Value + "";
                obj.ScheduleConfirmFlag = 1;
                obj.ScheduleConfirmDate = DateTime.Now;
                master.Details.Add(obj);
            }

            try
            {
                if (needSave)
                {
                    master = model.ProductionManagementSrv.SaveWeekScheduleMaster(master);
                    dgMaster.CurrentRow.Tag = master;
                    dgMaster.CurrentCell = dgMaster.CurrentRow.Cells[colMasterCode.Name];
                    dgMaster_SelectionChanged(dgMaster, new EventArgs());
                    MessageBox.Show("确认成功。");
                }
                else {
                    MessageBox.Show("确认成功。");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("周计划确认出错。\n"+ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private bool Valid()
        {
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                WeekScheduleDetail obj = dr.Tag as WeekScheduleDetail;
                if (obj.GWBSConfirmFlag == 1) continue;

                object progressO = dr.Cells[colTaskCompletedPercent.Name].Value;
                if (progressO == null || progressO.ToString().Equals("0")) continue;
                decimal progressD = 0M;
                if (!decimal.TryParse(progressO.ToString(), out progressD) || progressD<0)
                {
                    MessageBox.Show("请输入正确的形象进度。");
                    dgDetail.CurrentCell = dr.Cells[colTaskCompletedPercent.Name];
                    return false;
                }

                if (dr.Cells[colPlanConformity.Name].Value == null)
                {
                    MessageBox.Show("请输入【与进度符合度】。");
                    dgDetail.CurrentCell = dr.Cells[colPlanConformity.Name];
                    return false;
                }
            }
            return true;
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgMaster.CurrentRow;
            if (dr == null) return;
            WeekScheduleMaster master = dr.Tag as WeekScheduleMaster;
            if (master == null) return;
            FillDgDetail(master);
        }

        private void FillDgDetail(WeekScheduleMaster master)
        {
            dgDetail.Rows.Clear();
            foreach (WeekScheduleDetail var in master.Details)
            {
                int i = this.dgDetail.Rows.Add();
                dgDetail.Rows[i].Cells[colGWBSTree.Name].Tag = var.GWBSTree;
                dgDetail.Rows[i].Cells[colGWBSTree.Name].Value = var.GWBSTreeName;
                dgDetail.Rows[i].Cells[colMainTaskContent.Name].Value = var.MainTaskContent;
                //显示周一到周日
                SetPlanView(dgDetail.Rows[i], var);
                SetPlanTag(master.PlannedBeginDate,dgDetail.Rows[i]);
                SetBackColor(dgDetail.Rows[i]);
                dgDetail.Rows[i].Cells[colPlannedDuration.Name].Value = var.PlannedDuration;
                dgDetail.Rows[i].Cells[colPlannedWrokload.Name].Value = var.PlannedWrokload;
                if (var.ScheduleConfirmFlag == 1)
                {
                    dgDetail.Rows[i].Cells[colActualBeginDate.Name].Value = var.ActualBeginDate;
                    dgDetail.Rows[i].Cells[colActualEndDate.Name].Value = var.ActualEndDate;
                    dgDetail.Rows[i].Cells[colActualDuration.Name].Value = var.ActualDuration;
                }
                else
                {
                    dgDetail.Rows[i].Cells[colActualBeginDate.Name].Value = master.PlannedBeginDate.Date;
                    dgDetail.Rows[i].Cells[colActualEndDate.Name].Value = master.PlannedEndDate.Date;
                    dgDetail.Rows[i].Cells[colActualDuration.Name].Value = 7;
                }
                dgDetail.Rows[i].Cells[colActualWorkload.Name].Value = var.ActualWorklaod;
                dgDetail.Rows[i].Cells[colTaskCompletedPercent.Name].Value = var.TaskCompletedPercent;
                dgDetail.Rows[i].Cells[colPlanConformity.Name].Value = var.PlanConformity;
                dgDetail.Rows[i].Cells[colDescript.Name].Value = var.Descript;
                dgDetail.Rows[i].Cells[colOBSService.Name].Value = var.SupplierName;
                dgDetail.Rows[i].Cells[colOBSService.Name].Tag = var.SubContractProject;

                this.dgDetail.Rows[i].Tag = var;
            }
        }

        /// <summary>
        /// 设置单元格背景色
        /// </summary>
        /// <param name="dr"></param>
        private void SetBackColor(DataGridViewRow dr)
        {
            int startColumnIndex = dr.Cells[colMonday.Name].ColumnIndex;
            int endColumnIndex = dr.Cells[colSunday.Name].ColumnIndex;
            int a = 0, b = 0;
            for (int i = startColumnIndex; i <= endColumnIndex; i++)
            {
                object planStartValue = dr.Cells[i].Value;
                if (planStartValue != null && (bool)planStartValue)
                {
                    a = i;
                    break;
                }
            }

            for (int j = endColumnIndex; j >= startColumnIndex; j--)
            {
                object planEndValue = dr.Cells[j].Value;
                if (planEndValue != null && (bool)planEndValue)
                {
                    b = j;
                    break;
                }
            }

            //设置选中单元格背景色
            for (int k = startColumnIndex; k <= endColumnIndex; k++)
            {
                DataGridViewCellStyle cellStyle = dr.Cells[k].Style;
                if (k >= a && k <= b)
                {
                    cellStyle.BackColor = Color.Red;
                }
                else
                {
                    if (dgDetail.EditMode == DataGridViewEditMode.EditProgrammatically)
                    {
                        cellStyle.BackColor = Color.FromName("Control");
                    }
                    else
                    {
                        cellStyle.BackColor = Color.White;
                    }
                }
                dr.Cells[k].Style = cellStyle;
            }
        }

        private void SetPlanTag(DateTime masterPlannedBeginDate, DataGridViewRow dr)
        {
            DateTime dt = masterPlannedBeginDate.Date;
            dr.Cells[colMonday.Name].Tag = dt;
            dr.Cells[colTuesday.Name].Tag = dt.AddDays(1);
            dr.Cells[colWednesday.Name].Tag = dt.AddDays(2);
            dr.Cells[colThursday.Name].Tag = dt.AddDays(3);
            dr.Cells[colFriday.Name].Tag = dt.AddDays(4);
            dr.Cells[colSaturday.Name].Tag = dt.AddDays(5);
            dr.Cells[colSunday.Name].Tag = dt.AddDays(6);
        }

        private void SetPlanView(DataGridViewRow dr, WeekScheduleDetail detail)
        {
            DayOfWeek dow = detail.PlannedBeginDate.Date.DayOfWeek;
            int planDays = (detail.PlannedEndDate.Date - detail.PlannedBeginDate.Date).Days + 1;
            int columnIndex = 0;
            switch (dow)
            {
                case DayOfWeek.Monday:
                    columnIndex = dr.Cells[colMonday.Name].ColumnIndex;
                    SetPlanView(detail.Master.PlannedBeginDate,dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Tuesday:
                    columnIndex = dr.Cells[colTuesday.Name].ColumnIndex;
                    SetPlanView(detail.Master.PlannedBeginDate, dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Wednesday:
                    columnIndex = dr.Cells[colWednesday.Name].ColumnIndex;
                    SetPlanView(detail.Master.PlannedBeginDate, dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Thursday:
                    columnIndex = dr.Cells[colThursday.Name].ColumnIndex;
                    SetPlanView(detail.Master.PlannedBeginDate, dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Friday:
                    columnIndex = dr.Cells[colFriday.Name].ColumnIndex;
                    SetPlanView(detail.Master.PlannedBeginDate, dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Saturday:
                    columnIndex = dr.Cells[colSaturday.Name].ColumnIndex;
                    SetPlanView(detail.Master.PlannedBeginDate, dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Sunday:
                    columnIndex = dr.Cells[colSunday.Name].ColumnIndex;
                    SetPlanView(detail.Master.PlannedBeginDate, dr, columnIndex, planDays);
                    break;
            }
        }

        private void SetPlanView(DateTime masterPlannedBeginDate,DataGridViewRow dr, int columnIndex, int planDays)
        {
            DateTime dt = masterPlannedBeginDate.Date;
            dr.Cells[columnIndex].Value = true;
            for (int i = 0; i < planDays; i++)
            {
                dr.Cells[columnIndex + i].Value = true;
            }
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            txtCodeEnd.Text = txtCodeBegin.Text;
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ExecScheduleType",EnumExecScheduleType.周进度计划));
            oq.AddCriterion(Expression.Eq("SummaryStatus", EnumSummaryStatus.汇总生成));
            oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));
            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Ge("Code", txtCodeBegin.Text));
                oq.AddCriterion(Expression.Le("Code", txtCodeEnd.Text));
            }

            oq.AddCriterion(Expression.Ge("CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", dtpDateEnd.Value.AddDays(1).Date));

            try
            {
                IList list = model.ProductionManagementSrv.GetWeekScheduleMaster(oq);
                FillMaster(list);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询周计划出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void FillMaster(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (WeekScheduleMaster master in masterList)
            {
                int rowIndex=dgMaster.Rows.Add();
                DataGridViewRow dr = dgMaster.Rows[rowIndex];
                dr.Tag = master;
                dr.Cells[colMasterCode.Name].Value = master.Code;
                dr.Cells[colMasterCreateDate.Name].Value = master.CreateDate.ToShortDateString();
                dr.Cells[colMasterPlannedBeginDate.Name].Value = master.PlannedBeginDate.ToShortDateString();
                dr.Cells[colMasterPlannedDateEnd.Name].Value = master.PlannedEndDate.ToShortDateString();
                dr.Cells[colMasterHandlePerson.Name].Value = master.HandlePersonName;
                dr.Cells[colMasterDescript.Name].Value = master.Descript;
            }
            dgMaster.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            if (dgMaster.Rows.Count > 0)
            {
                dgMaster.CurrentCell = dgMaster.Rows[0].Cells[colMasterCode.Name];
                dgMaster_SelectionChanged(dgMaster, new EventArgs());
            }
        }
    }
}
