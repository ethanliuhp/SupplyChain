using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Basic.Domain;

using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.OBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Properties;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml.Serialization;
using System.Threading;
using FlexCell;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekScheduleBak2 : TMasterDetailView
    {
        private MProductionMng model = new MProductionMng();
        MStockMng stockModel = new MStockMng();
        private WeekScheduleMaster curBillMaster;
        //private string execScheduleTypeName="周进度计划";
        private EnumExecScheduleType execScheduleType;
        CurrentProjectInfo projectInfo = null;

        private int baseYear = 1990;
        private int baseStartMonth = 1;


        #region 树方式
        private Hashtable detailHashtable = new Hashtable();
        private ProductionScheduleMaster master;
        private string imageExpand = "imageExpand";
        private string imageCollapse = "imageCollapse";
        private List<string> listDtlIds = new List<string>();
        private int startImageCol = 1, endImageCol = 19;
        #endregion

        /// <summary>
        /// 当前单据
        /// </summary>
        public WeekScheduleMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VWeekScheduleBak2(EnumExecScheduleType execScheduleType)
        {
            InitializeComponent();
            this.execScheduleType = execScheduleType;
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            InitFlexGrid(1);

            List<int> year = new List<int>();
            for (int i = baseYear; i < baseYear + 100; i++)
            {
                year.Add(i);
            }
            this.cmoYear.DataSource = year;
            this.cmoYear.Text = ConstObject.LoginDate.Year.ToString();
            List<int> startMonth = new List<int>();
            for (int i = baseStartMonth; i < baseStartMonth + 12; i++)
            {
                startMonth.Add(i);
            }
            this.cmoStartMonth.DataSource = startMonth;
            //this.cmoStartMonth.Text = (ClientUtil.ToInt(cmoEndMonth.Text) - 1).ToString();
            this.cmoStartMonth.Text = ConstObject.LoginDate.Month.ToString();

            foreach (string planType in Enum.GetNames(typeof(EnumExecScheduleType)))
            {
                if (planType != "周进度计划")
                {
                    cbPlanType.Items.Add(planType);
                }
            }

            gridWeekPlanDetail.ContextMenuStrip = cmsDg;
            gridMonthPlan.ContextMenuStrip = cmsDgOther;
            gridWeekPlanDetail.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //dtpDateEnd.Value = dtpDateBegin.Value.AddDays(7);

            DateTime dt = model.ProductionManagementSrv.GetServerTime();
            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                //缺省下周一
                switch (dt.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        dtpDateBegin.Value = dt;
                        dtpDateEnd.Value = dt.AddDays(6);
                        break;
                    case DayOfWeek.Tuesday:
                        dtpDateBegin.Value = dt.AddDays(6);
                        dtpDateEnd.Value = dt.AddDays(12);
                        break;
                    case DayOfWeek.Wednesday:
                        dtpDateBegin.Value = dt.AddDays(5);
                        dtpDateEnd.Value = dt.AddDays(11);
                        break;
                    case DayOfWeek.Thursday:
                        dtpDateBegin.Value = dt.AddDays(4);
                        dtpDateEnd.Value = dt.AddDays(10);
                        break;
                    case DayOfWeek.Friday:
                        dtpDateBegin.Value = dt.AddDays(3);
                        dtpDateEnd.Value = dt.AddDays(9);
                        break;
                    case DayOfWeek.Saturday:
                        dtpDateBegin.Value = dt.AddDays(2);
                        dtpDateEnd.Value = dt.AddDays(8);
                        break;
                    case DayOfWeek.Sunday:
                        dtpDateBegin.Value = dt.AddDays(1);
                        dtpDateEnd.Value = dt.AddDays(7);
                        break;
                }

                dt = dtpDateBegin.Value;

                SetWeekPlanDatetime(dt);

                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tpWeekPlan);
                customLabel6.Visible = false;
                label3.Visible = false;
                label2.Visible = false;
                cmoYear.Visible = false;
                cmoStartMonth.Visible = false;
                lblSupplier.Visible = false;
                cbPlanType.Visible = false;
                customLabel3.Visible = true;
                dtpBusinessDate.Visible = true;
                btnDelete.Visible = false;
                btnExportToMPP.Visible = false;
            }
            else
            {
                //缺省计划时间段从基础数据中取

                int startDate = 21;
                int endDate = 20;

                IList listDateArea = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_MonthScheduleDefaultDateArea);

                if (listDateArea != null && listDateArea.Count > 0)
                {
                    BasicDataOptr basicData = listDateArea[0] as BasicDataOptr;
                    if (basicData != null)
                    {
                        try
                        {
                            startDate = Convert.ToInt32(basicData.BasicCode);
                            endDate = Convert.ToInt32(basicData.BasicName);
                        }
                        catch { }
                    }
                }

                dtpDateBegin.Value = dt.Date.AddDays(-dt.Day).AddDays(startDate).AddMonths(-1);
                dtpDateEnd.Value = dt.Date.AddDays(-dt.Day).AddDays(endDate);

                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tpTreePlan);
                //tabControl1.TabPages.Add(tpOtherPlan);
            }
        }

        private void InitEvent()
        {
            this.gridWeekPlanDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.gridWeekPlanDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.gridWeekPlanDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            gridWeekPlanDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            btnProductSchedule.Click += new EventHandler(btnProductSchedule_Click);

            dtpDateBegin.ValueChanged += new EventHandler(dtpDateBegin_ValueChanged);
            dtpDateEnd.ValueChanged += new EventHandler(dtpDateEnd_ValueChanged);
            gridWeekPlanDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            gridWeekPlanDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);
            gridMonthPlan.CellEndEdit += new DataGridViewCellEventHandler(dgOtherPlan_CellEndEdit);
            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            //右键复制菜单
            tsmiCopy.Click += new EventHandler(tsmiCopy_Click);
            tsmiDgOtherDel.Click += new EventHandler(tsmiDgOtherDel_Click);

            cbPlanType.SelectedIndexChanged += new EventHandler(cbPlanType_SelectedIndexChanged);

            btnExportToMPP.Click += new EventHandler(btnExportToMPP_Click);
            flexGrid.Click += new FlexCell.Grid.ClickEventHandler(flexGrid_Click);
            flexGrid.LeaveCell += new Grid.LeaveCellEventHandler(flexGrid_LeaveCell);
            this.cmoStartMonth.SelectedIndexChanged += new EventHandler(cmoStartMonth_SelectedIndexChanged);
            btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        void cmoStartMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshAccountList();
        }

        private void RefreshAccountList()
        {
            //获取当前会计期的最后一天
            string startDate = stockModel.StockInOutSrv.GetStartDateByFiscalPeriod(ClientUtil.ToInt(this.cmoYear.Text), ClientUtil.ToInt(this.cmoStartMonth.Text));
            string endDate = stockModel.StockInOutSrv.GetEndDateByFiscalPeriod(ClientUtil.ToInt(this.cmoYear.Text), ClientUtil.ToInt(this.cmoStartMonth.Text));
            this.dtpDateBegin.Value = ClientUtil.ToDateTime(startDate);
            this.dtpDateEnd.Value = ClientUtil.ToDateTime(endDate);
        }

        void cbPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPlanType.Text == "季进度计划")
            {
                this.tpOtherPlan.Text = "季度计划明细";
            }
            if (cbPlanType.Text == "月进度计划")
            {
                this.tpOtherPlan.Text = "月进度计划明细";
            }
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colOBSService.Name)
            {
                VContractExcuteSelector vces = new VContractExcuteSelector();
                vces.StartPosition = FormStartPosition.CenterScreen;
                vces.ShowDialog();
                if (vces.Result != null && vces.Result.Count > 0)
                {
                    SubContractProject scp = vces.Result[0] as SubContractProject;
                    gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].Tag = scp;
                    gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].Value = scp.BearerOrgName;

                    WeekScheduleDetail detail = gridWeekPlanDetail.Rows[e.RowIndex].Tag as WeekScheduleDetail;
                    if (detail == null)
                    {
                        detail = new WeekScheduleDetail();
                    }
                    detail.SupplierRelationInfo = scp.BearerOrg;

                    gridWeekPlanDetail.Rows[e.RowIndex].Tag = detail;

                    gridWeekPlanDetail.CurrentCell = gridWeekPlanDetail[e.ColumnIndex - 1, e.RowIndex];
                    gridWeekPlanDetail.CurrentCell = gridWeekPlanDetail[e.ColumnIndex, e.RowIndex];
                }

                //VSelectFWOBS vobs = new VSelectFWOBS(new MGWBSTree());
                //vobs.ShowDialog();
                //OBSService _OBSService = vobs.CurBillService;
                //if (_OBSService != null)
                //{
                //    dgDetail[e.ColumnIndex, e.RowIndex].Tag = _OBSService;
                //    dgDetail[e.ColumnIndex, e.RowIndex].Value = _OBSService.SupplierName;
                //    dgDetail.CurrentCell = dgDetail[colPlannedWrokload.Name, e.RowIndex];
                //    dgDetail.CurrentCell = dgDetail[e.ColumnIndex, e.RowIndex];
                //}
            }
        }

        void dgOtherPlan_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //bool validity = true;
            //string colName = gridWeekPlanDetail.Columns[e.ColumnIndex].Name;
            //if (colName == colOPPlannedWorkload.Name || colName == colOPActualWorkload.Name)
            //{
            //    object pWorkload = dgOtherPlan[e.ColumnIndex, e.RowIndex].Value;
            //    if (pWorkload == null) pWorkload = "";
            //    if (!CommonMethod.VeryValid(pWorkload.ToString()))
            //    {
            //        MessageBox.Show("请输入数字！");
            //        return;
            //    }

            //    object aWorkLoad = dgOtherPlan[e.ColumnIndex, e.RowIndex].Value;
            //    if (aWorkLoad == null) aWorkLoad = "";
            //    if (!CommonMethod.VeryValid(aWorkLoad.ToString()))
            //    {
            //        MessageBox.Show("请输入数字！");
            //        return;
            //    }

            //    decimal sumpWorkload = 0M, sumaWorkload = 0M;
            //    foreach (DataGridViewRow dr in dgOtherPlan.Rows)
            //    {
            //        sumpWorkload += ClientUtil.ToDecimal(dr.Cells[colOPPlannedWorkload.Name].Value);
            //        sumaWorkload += ClientUtil.ToDecimal(dr.Cells[colOPActualWorkload.Name].Value);
            //    }
            //    txtSumPlanQuantity.Text = sumpWorkload.ToString("###########.####");
            //    txtSumActualQuantity.Text = sumaWorkload.ToString("###########.####");
            //}

            //foreach (DataGridViewRow var in gridMonthPlan.Rows)
            //{
            //    DateTime beginTime = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateBegin.Name].Value);
            //    DateTime endTime = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateEnd.Name].Value);
            //    var.Cells[colOPPlannedDuration.Name].Value = ClientUtil.ToString((endTime - beginTime).Days + 1);
            //}

            string colName = gridMonthPlan.Columns[e.ColumnIndex].Name;
            if (colName == colOPPlannedDateEnd.Name)
            {
                foreach (DataGridViewRow var in gridMonthPlan.Rows)
                {
                    DateTime beginTime = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateBegin.Name].Value);
                    DateTime endTime = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateEnd.Name].Value);
                    var.Cells[colOPPlannedDuration.Name].Value = ClientUtil.ToString((endTime - beginTime).Days + 1);
                }
            }
        }

        void tsmiCopy_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = gridWeekPlanDetail.CurrentRow;
            if (dr == null || dr.IsNewRow) return;

            int i = gridWeekPlanDetail.Rows.Add();

            gridWeekPlanDetail[colGWBSTree.Name, i].Value = dr.Cells[colGWBSTree.Name].Value;
            gridWeekPlanDetail[colGWBSTree.Name, i].Tag = dr.Cells[colGWBSTree.Name].Tag;
            gridWeekPlanDetail[colForwardBillDtlIdOfWeek.Name, i].Value = dr.Cells[colForwardBillDtlIdOfWeek.Name].Value;

            gridWeekPlanDetail[colMonday.Name, i].Value = dr.Cells[colMonday.Name].Value;
            gridWeekPlanDetail[colMonday.Name, i].Tag = dr.Cells[colMonday.Name].Tag;

            gridWeekPlanDetail[colTuesday.Name, i].Value = dr.Cells[colTuesday.Name].Value;
            gridWeekPlanDetail[colTuesday.Name, i].Tag = dr.Cells[colTuesday.Name].Tag;
            gridWeekPlanDetail[colWednesday.Name, i].Value = dr.Cells[colWednesday.Name].Value;
            gridWeekPlanDetail[colWednesday.Name, i].Tag = dr.Cells[colWednesday.Name].Tag;

            gridWeekPlanDetail[colThursday.Name, i].Value = dr.Cells[colThursday.Name].Value;
            gridWeekPlanDetail[colThursday.Name, i].Tag = dr.Cells[colThursday.Name].Tag;

            gridWeekPlanDetail[colFriday.Name, i].Value = dr.Cells[colFriday.Name].Value;
            gridWeekPlanDetail[colFriday.Name, i].Tag = dr.Cells[colFriday.Name].Tag;

            gridWeekPlanDetail[colSaturday.Name, i].Value = dr.Cells[colSaturday.Name].Value;
            gridWeekPlanDetail[colSaturday.Name, i].Tag = dr.Cells[colSaturday.Name].Tag;

            gridWeekPlanDetail[colSunday.Name, i].Value = dr.Cells[colSunday.Name].Value;
            gridWeekPlanDetail[colSunday.Name, i].Tag = dr.Cells[colSunday.Name].Tag;
            gridWeekPlanDetail[colPlannedDuration.Name, i].Value = dr.Cells[colPlannedDuration.Name].Value;

        }

        void tsmiDgOtherDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = gridMonthPlan.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                gridMonthPlan.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as WeekScheduleDetail);
                }
            }
        }

        void dgDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //自动计算计划工期
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            DataGridViewCell cell = gridWeekPlanDetail[e.ColumnIndex, e.RowIndex];

            if (cell.OwningColumn.Name == colMonday.Name
                || cell.OwningColumn.Name == colTuesday.Name
                || cell.OwningColumn.Name == colWednesday.Name
                || cell.OwningColumn.Name == colThursday.Name
                || cell.OwningColumn.Name == colFriday.Name
                || cell.OwningColumn.Name == colSaturday.Name
                || cell.OwningColumn.Name == colSunday.Name
                )
            {
                int startColumnIndex = gridWeekPlanDetail.Rows[e.RowIndex].Cells[colMonday.Name].ColumnIndex;
                int endColumnIndex = gridWeekPlanDetail.Rows[e.RowIndex].Cells[colSunday.Name].ColumnIndex;

                int a = 0, b = 0;
                for (int i = startColumnIndex; i <= endColumnIndex; i++)
                {
                    object planStartValue = gridWeekPlanDetail.Rows[e.RowIndex].Cells[i].Value;
                    if (planStartValue != null && (bool)planStartValue)
                    {
                        a = i;
                        break;
                    }
                }

                for (int j = endColumnIndex; j >= startColumnIndex; j--)
                {
                    object planEndValue = gridWeekPlanDetail.Rows[e.RowIndex].Cells[j].Value;
                    if (planEndValue != null && (bool)planEndValue)
                    {
                        b = j;
                        break;
                    }
                }

                if (a == 0 && b == 0)
                {
                    gridWeekPlanDetail.Rows[e.RowIndex].Cells[colPlannedDuration.Name].Value = 0;
                }
                else
                {
                    gridWeekPlanDetail.Rows[e.RowIndex].Cells[colPlannedDuration.Name].Value = b - a + 1;
                }

                //设置选中单元格背景色
                for (int k = startColumnIndex; k <= endColumnIndex; k++)
                {
                    DataGridViewCellStyle cellStyle = gridWeekPlanDetail.Rows[e.RowIndex].Cells[k].Style;
                    if (k >= a && k <= b)
                    {
                        gridWeekPlanDetail.Rows[e.RowIndex].Cells[k].Value = true;
                        cellStyle.BackColor = ColorTranslator.FromHtml("#D7E8FE");// Color.Red;
                    }
                    else
                    {
                        cellStyle.BackColor = Color.White;
                    }
                    gridWeekPlanDetail.Rows[e.RowIndex].Cells[k].Style = cellStyle;
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colMonday.Name
                || gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colTuesday.Name
                || gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colWednesday.Name
                || gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colThursday.Name
                || gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colFriday.Name
                || gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSaturday.Name
                || gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSunday.Name
                )
            {
                gridWeekPlanDetail.EndEdit();
            }
        }

        void dtpDateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                SetWeekPlanDatetime(dtpDateEnd.Value.AddDays(-6));
            }
            else
            {
                if (dtpDateEnd.Value.Date < dtpDateBegin.Value.Date)
                {
                    dtpDateBegin.Value = dtpDateEnd.Value.AddMonths(-1).AddDays(1);
                }
            }
        }

        void dtpDateBegin_ValueChanged(object sender, EventArgs e)
        {
            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                SetWeekPlanDatetime(dtpDateBegin.Value);
            }
            else
            {
                if (dtpDateEnd.Value.Date < dtpDateBegin.Value.Date)
                {
                    dtpDateEnd.Value = dtpDateBegin.Value.AddMonths(1).AddDays(-1);
                }
            }
        }

        /// <summary>
        /// 根据输入日期设置计划日期的起止时间为星期一到日期日
        /// </summary>
        /// <param name="dt"></param>
        private void SetWeekPlanDatetime(DateTime dt)
        {
            dtpDateBegin.ValueChanged -= new EventHandler(dtpDateBegin_ValueChanged);
            dtpDateEnd.ValueChanged -= new EventHandler(dtpDateEnd_ValueChanged);

            dtpDateBegin.Value = dt.Date;
            dtpDateEnd.Value = dt.Date.AddDays(6);

            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    colMonday.HeaderText = "星期一";
                    colTuesday.HeaderText = "星期二";
                    colWednesday.HeaderText = "星期三";
                    colThursday.HeaderText = "星期四";
                    colFriday.HeaderText = "星期五";
                    colSaturday.HeaderText = "星期六";
                    colSunday.HeaderText = "星期日";
                    break;
                case DayOfWeek.Tuesday:
                    colMonday.HeaderText = "星期二";
                    colTuesday.HeaderText = "星期三";
                    colWednesday.HeaderText = "星期四";
                    colThursday.HeaderText = "星期五";
                    colFriday.HeaderText = "星期六";
                    colSaturday.HeaderText = "星期日";
                    colSunday.HeaderText = "星期一";
                    break;
                case DayOfWeek.Wednesday:
                    colMonday.HeaderText = "星期三";
                    colTuesday.HeaderText = "星期四";
                    colWednesday.HeaderText = "星期五";
                    colThursday.HeaderText = "星期六";
                    colFriday.HeaderText = "星期日";
                    colSaturday.HeaderText = "星期一";
                    colSunday.HeaderText = "星期二";
                    break;
                case DayOfWeek.Thursday:
                    colMonday.HeaderText = "星期四";
                    colTuesday.HeaderText = "星期五";
                    colWednesday.HeaderText = "星期六";
                    colThursday.HeaderText = "星期日";
                    colFriday.HeaderText = "星期一";
                    colSaturday.HeaderText = "星期二";
                    colSunday.HeaderText = "星期三";
                    break;
                case DayOfWeek.Friday:
                    colMonday.HeaderText = "星期五";
                    colTuesday.HeaderText = "星期六";
                    colWednesday.HeaderText = "星期日";
                    colThursday.HeaderText = "星期一";
                    colFriday.HeaderText = "星期二";
                    colSaturday.HeaderText = "星期三";
                    colSunday.HeaderText = "星期四";
                    break;
                case DayOfWeek.Saturday:
                    colMonday.HeaderText = "星期六";
                    colTuesday.HeaderText = "星期日";
                    colWednesday.HeaderText = "星期一";
                    colThursday.HeaderText = "星期二";
                    colFriday.HeaderText = "星期三";
                    colSaturday.HeaderText = "星期四";
                    colSunday.HeaderText = "星期五";
                    break;
                case DayOfWeek.Sunday:
                    colMonday.HeaderText = "星期日";
                    colTuesday.HeaderText = "星期一";
                    colWednesday.HeaderText = "星期二";
                    colThursday.HeaderText = "星期三";
                    colFriday.HeaderText = "星期四";
                    colSaturday.HeaderText = "星期五";
                    colSunday.HeaderText = "星期六";
                    break;
            }

            //colMonday.HeaderText += "[" + dt.Date.ToShortDateString() + "]";

            colMonday.ToolTipText = dt.Date.ToShortDateString();
            colTuesday.ToolTipText = dt.Date.AddDays(1).ToShortDateString();
            colWednesday.ToolTipText = dt.Date.AddDays(2).ToShortDateString();
            colThursday.ToolTipText = dt.Date.AddDays(3).ToShortDateString();
            colFriday.ToolTipText = dt.Date.AddDays(4).ToShortDateString();
            colSaturday.ToolTipText = dt.Date.AddDays(5).ToShortDateString();
            colSunday.ToolTipText = dt.Date.AddDays(6).ToShortDateString();

            //SetGridRowWeekDate();

            dtpDateBegin.ValueChanged += new EventHandler(dtpDateBegin_ValueChanged);
            dtpDateEnd.ValueChanged += new EventHandler(dtpDateEnd_ValueChanged);
        }

        private void SetGridRowWeekDate()
        {
            DateTime planDateBegin = dtpDateBegin.Value.Date;
            foreach (DataGridViewRow var in gridWeekPlanDetail.Rows)
            {
                var.Cells[colMonday.Name].Tag = planDateBegin;
                var.Cells[colTuesday.Name].Tag = planDateBegin.AddDays(1);
                var.Cells[colWednesday.Name].Tag = planDateBegin.AddDays(2);
                var.Cells[colThursday.Name].Tag = planDateBegin.AddDays(3);
                var.Cells[colFriday.Name].Tag = planDateBegin.AddDays(4);
                var.Cells[colSaturday.Name].Tag = planDateBegin.AddDays(5);
                var.Cells[colSunday.Name].Tag = planDateBegin.AddDays(6);
            }
        }

        void btnProductSchedule_Click(object sender, EventArgs e)
        {
            //InitFlexGrid(1);
            //VScheduleSelector vss = new VScheduleSelector();
            //vss.FrontPlanType = this.execScheduleType;
            //vss.DefaultBeginDate = dtpDateBegin.Value;
            //vss.DefaultEndDate = dtpDateEnd.Value;
            //vss.ShowDialog();
            //IList list = vss.Result;
            //if (list == null || list.Count == 0) return;
            //master = (list[0] as ProductionScheduleDetail).Master;

            //txtProductSchedule.Text = master.ScheduleTypeDetail + "." + master.ScheduleName;
            //curBillMaster.ForwardBillId = master.Id;

            ////dgDetail.Rows.Clear();
            ////dgOtherPlan.Rows.Clear();

            //if (this.execScheduleType == EnumExecScheduleType.周进度计划)
            //{
            //    DateTime dt = dtpDateBegin.Value.Date;
            //    foreach (ProductionScheduleDetail detail in list)
            //    {
            //        if (HasAdded(detail, execScheduleType))
            //            continue;

            //        int rowIndex = gridWeekPlanDetail.Rows.Add();

            //        gridWeekPlanDetail[colGWBSTree.Name, rowIndex].Tag = detail.GWBSTree;
            //        gridWeekPlanDetail[colGWBSTree.Name, rowIndex].Value = detail.GWBSTreeName;

            //        string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.GWBSTreeName, detail.GWBSTreeSysCode);
            //        gridWeekPlanDetail[colGWBSTree.Name, rowIndex].ToolTipText = fullPath;
            //        gridWeekPlanDetail[colMainTaskContent.Name, rowIndex].Value = fullPath;

            //        gridWeekPlanDetail[colMonday.Name, rowIndex].Value = false;
            //        gridWeekPlanDetail[colTuesday.Name, rowIndex].Value = false;
            //        gridWeekPlanDetail[colWednesday.Name, rowIndex].Value = false;
            //        gridWeekPlanDetail[colThursday.Name, rowIndex].Value = false;
            //        gridWeekPlanDetail[colFriday.Name, rowIndex].Value = false;
            //        gridWeekPlanDetail[colSaturday.Name, rowIndex].Value = false;
            //        gridWeekPlanDetail[colSunday.Name, rowIndex].Value = false;

            //        gridWeekPlanDetail[colPlannedDuration.Name, rowIndex].Value = 7;

            //        gridWeekPlanDetail[colForwardBillDtlIdOfWeek.Name, rowIndex].Value = detail.Id;

            //        //显示周一到周日
            //        SetPlanViewBySelect(gridWeekPlanDetail.Rows[rowIndex], detail);

            //        SetPlanTag(gridWeekPlanDetail.Rows[rowIndex]);
            //    }
            //}
            //else if (this.execScheduleType == EnumExecScheduleType.月度进度计划)
            //{
            //    IList listProDtl = new ArrayList();

            //    ObjectQuery oq = new ObjectQuery();
            //    oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
            //    oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            //    IList listGWBSTree = model.ObjectQuery(typeof(ProductionScheduleDetail), oq);

            //    foreach (ProductionScheduleDetail detail in list)
            //    {
            //        if (HasAdded(detail, execScheduleType)) continue;

            //        #region 表方式
            //        //int dgOpRowIndex = gridMonthPlan.Rows.Add();
            //        //gridMonthPlan[colOPGWBSTree.Name, dgOpRowIndex].Tag = detail.GWBSTree;
            //        //gridMonthPlan[colOPGWBSTree.Name, dgOpRowIndex].Value = detail.GWBSTreeName;

            //        //string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.GWBSTree);
            //        //gridMonthPlan[colOPGWBSTree.Name, dgOpRowIndex].ToolTipText = fullPath;
            //        //gridMonthPlan[colOPMainTaskContent.Name, dgOpRowIndex].Value = fullPath;
            //        ////gridMonthPlan[colOPPlannedDateBegin.Name, dgOpRowIndex].Value = dtpDateBegin.Value.Date;
            //        ////gridMonthPlan[colOPPlannedDateEnd.Name, dgOpRowIndex].Value = dtpDateEnd.Value.Date;
            //        ////gridMonthPlan[colOPPlannedDuration.Name, dgOpRowIndex].Value = (dtpDateEnd.Value.Date - dtpDateBegin.Value.Date).Days + 1;

            //        //SetMonthPlanDateView(gridMonthPlan.Rows[dgOpRowIndex], detail);

            //        //gridMonthPlan[colForwardBillDtlIdOfMonth.Name, dgOpRowIndex].Value = detail.Id;
            //        #endregion

            //        #region 树方式

            //        string[] sysCodes = detail.GWBSTreeSysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            //        for (int i = 0; i < sysCodes.Length; i++)
            //        {
            //            string sysCode = sysCodes[i];
            //            foreach (ProductionScheduleDetail ddtl in listGWBSTree)
            //            {
            //                if (ddtl.GWBSTree == null && ddtl.Level == 1)
            //                {
            //                    listProDtl.Add(ddtl);
            //                }
            //                if (ddtl.GWBSTree != null)
            //                {
            //                    if (sysCode == ddtl.GWBSTree.Id)
            //                    {
            //                        listProDtl.Add(ddtl);
            //                    }
            //                }
            //            }
            //        }
            //        #endregion
            //    }

            //    IList newlistProDtl = new ArrayList();
            //    for (int i = 0; i < listProDtl.Count; i++)
            //    {
            //        ProductionScheduleDetail dtl = listProDtl[i] as ProductionScheduleDetail;
            //        if (newlistProDtl.Count == 0)
            //        {
            //            newlistProDtl.Add(dtl);
            //        }
            //        else
            //        {
            //            bool flag = false;
            //            foreach (ProductionScheduleDetail ddtl in newlistProDtl)
            //            {
            //                if (dtl.Id == ddtl.Id)
            //                {
            //                    flag = true;
            //                }
            //            }
            //            if (!flag)
            //            {
            //                newlistProDtl.Add(dtl);
            //            }
            //        }
            //    }

            //    IList listProChilds = model.ProductionManagementSrv.GetProChilds(newlistProDtl);
            //    curBillMaster = SavePlanDtl(listProChilds);
            //    GridFillFlex();
            //}
            NewWeekPlan();
        }

        private WeekScheduleMaster SavePlanDtl(IList list)
        {
            SaveScheduleDetail(list);
            return model.ProductionManagementSrv.SaveWeekScheduleMaster(curBillMaster) as WeekScheduleMaster;
        }

        private void AddPlanDtl(IList list)
        {
            //curBillMaster.Details.Clear();
            //model.ProductionManagementSrv.DeleteWeekScheduleDetail(curBillMaster);

            if (curBillMaster.Details.Count == 0)
            {

                DateTime beginDate = ClientUtil.ToDateTime(this.dtpDateBegin.Text);
                DateTime endDate = ClientUtil.ToDateTime(this.dtpDateEnd.Text);

                #region
                foreach (ProductionScheduleDetail ddtl in list)
                {
                    WeekScheduleDetail dtl = new WeekScheduleDetail();
                    if (ddtl.GWBSTree != null)
                    {
                        dtl.GWBSTree = ddtl.GWBSTree;
                        dtl.GWBSTreeName = ddtl.GWBSTreeName;
                        dtl.GWBSTreeSysCode = ddtl.GWBSTreeSysCode;

                        //dtl.PlannedBeginDate = ddtl.PlannedBeginDate;
                        //dtl.PlannedEndDate = ddtl.PlannedEndDate;
                        //dtl.PlannedDuration = ddtl.PlannedDuration;

                        DateTime plannedBeginDate = ddtl.PlannedBeginDate;
                        DateTime plannedEndDate = ddtl.PlannedEndDate;


                        if (plannedBeginDate == ClientUtil.ToDateTime("1900-1-1") && plannedEndDate == ClientUtil.ToDateTime("1900-1-1"))
                        {
                            dtl.PlannedBeginDate = ddtl.PlannedBeginDate;
                            dtl.PlannedEndDate = ddtl.PlannedEndDate;
                        }
                        else if (beginDate >= plannedBeginDate && endDate <= plannedEndDate)
                        {
                            dtl.PlannedBeginDate = beginDate;
                            dtl.PlannedEndDate = endDate;
                            dtl.PlannedDuration = (endDate - beginDate).Days + 1;
                        }
                        else if (beginDate >= plannedEndDate)
                        {
                            dtl.PlannedBeginDate = beginDate;
                            dtl.PlannedEndDate = endDate;
                            dtl.PlannedDuration = (endDate - beginDate).Days + 1;
                        }
                        else if (beginDate <= plannedBeginDate && endDate >= plannedEndDate)
                        {
                            dtl.PlannedBeginDate = plannedBeginDate;
                            dtl.PlannedEndDate = plannedEndDate;
                            dtl.PlannedDuration = (plannedEndDate - plannedBeginDate).Days + 1;
                        }
                        else if (endDate <= plannedBeginDate)
                        {
                            dtl.PlannedBeginDate = beginDate;
                            dtl.PlannedEndDate = endDate;
                            dtl.PlannedDuration = (endDate - beginDate).Days + 1;
                        }
                        else if (plannedBeginDate >= beginDate && plannedBeginDate < endDate && plannedEndDate > beginDate && plannedEndDate <= endDate)
                        {
                            dtl.PlannedBeginDate = plannedBeginDate;
                            dtl.PlannedEndDate = plannedEndDate;
                            dtl.PlannedDuration = (plannedEndDate - plannedBeginDate).Days + 1;
                        }
                        else if (beginDate >= plannedBeginDate && endDate >= plannedEndDate)
                        {
                            dtl.PlannedBeginDate = beginDate;
                            dtl.PlannedEndDate = plannedEndDate;
                            dtl.PlannedDuration = (plannedEndDate - beginDate).Days + 1;
                        }
                        else if (beginDate <= plannedBeginDate && endDate <= plannedEndDate)
                        {
                            dtl.PlannedBeginDate = plannedBeginDate;
                            dtl.PlannedEndDate = endDate;
                            dtl.PlannedDuration = (endDate - plannedBeginDate).Days + 1;
                        }



                        dtl.Master = curBillMaster;

                        curBillMaster.AddDetail(dtl);
                    }
                }
                #endregion
            }
            else
            {
                foreach (WeekScheduleDetail ddtl in curBillMaster.Details)
                {
                    foreach (ProductionScheduleDetail pdtl in list)
                    {

                    }
                }
            }
        }


        private void NewWeekPlan()
        {
            VScheduleSelector vss = new VScheduleSelector();
            vss.FrontPlanType = this.execScheduleType;
            vss.DefaultBeginDate = dtpDateBegin.Value;
            vss.DefaultEndDate = dtpDateEnd.Value;
            vss.ShowDialog();
            IList list = vss.Result;
            if (list == null || list.Count == 0) return;
            master = (list[0] as ProductionScheduleDetail).Master;

            txtProductSchedule.Text = master.ScheduleTypeDetail + "." + master.ScheduleName;
            curBillMaster.ForwardBillId = master.Id;

            if (this.execScheduleType == EnumExecScheduleType.周进度计划)
            {
                DateTime dt = dtpDateBegin.Value.Date;
                foreach (ProductionScheduleDetail detail in list)
                {
                    if (HasAdded(detail, execScheduleType))
                        continue;

                    int rowIndex = gridWeekPlanDetail.Rows.Add();

                    gridWeekPlanDetail[colGWBSTree.Name, rowIndex].Tag = detail.GWBSTree;
                    gridWeekPlanDetail[colGWBSTree.Name, rowIndex].Value = detail.GWBSTreeName;

                    string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.GWBSTreeName, detail.GWBSTreeSysCode);
                    gridWeekPlanDetail[colGWBSTree.Name, rowIndex].ToolTipText = fullPath;
                    gridWeekPlanDetail[colMainTaskContent.Name, rowIndex].Value = fullPath;

                    gridWeekPlanDetail[colMonday.Name, rowIndex].Value = false;
                    gridWeekPlanDetail[colTuesday.Name, rowIndex].Value = false;
                    gridWeekPlanDetail[colWednesday.Name, rowIndex].Value = false;
                    gridWeekPlanDetail[colThursday.Name, rowIndex].Value = false;
                    gridWeekPlanDetail[colFriday.Name, rowIndex].Value = false;
                    gridWeekPlanDetail[colSaturday.Name, rowIndex].Value = false;
                    gridWeekPlanDetail[colSunday.Name, rowIndex].Value = false;

                    gridWeekPlanDetail[colPlannedDuration.Name, rowIndex].Value = 7;

                    gridWeekPlanDetail[colForwardBillDtlIdOfWeek.Name, rowIndex].Value = detail.Id;

                    //显示周一到周日
                    SetPlanViewBySelect(gridWeekPlanDetail.Rows[rowIndex], detail);

                    SetPlanTag(gridWeekPlanDetail.Rows[rowIndex]);
                }
            }
            else if (this.execScheduleType == EnumExecScheduleType.月度进度计划)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
                oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
                IList listProductionScheduleDetail = model.ObjectQuery(typeof(ProductionScheduleDetail), oq);

                IList listSysCodes = new ArrayList();
                foreach (ProductionScheduleDetail dtl in list)
                {
                    #region 表方式
                    //if (HasAdded(dtl, execScheduleType)) continue;

                    //int dgOpRowIndex = gridMonthPlan.Rows.Add();
                    //gridMonthPlan[colOPGWBSTree.Name, dgOpRowIndex].Tag = dtl.GWBSTree;
                    //gridMonthPlan[colOPGWBSTree.Name, dgOpRowIndex].Value = dtl.GWBSTreeName;

                    //string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.GWBSTree);
                    //gridMonthPlan[colOPGWBSTree.Name, dgOpRowIndex].ToolTipText = fullPath;
                    //gridMonthPlan[colOPMainTaskContent.Name, dgOpRowIndex].Value = fullPath;
                    ////gridMonthPlan[colOPPlannedDateBegin.Name, dgOpRowIndex].Value = dtpDateBegin.Value.Date;
                    ////gridMonthPlan[colOPPlannedDateEnd.Name, dgOpRowIndex].Value = dtpDateEnd.Value.Date;
                    ////gridMonthPlan[colOPPlannedDuration.Name, dgOpRowIndex].Value = (dtpDateEnd.Value.Date - dtpDateBegin.Value.Date).Days + 1;

                    //SetMonthPlanDateView(gridMonthPlan.Rows[dgOpRowIndex], dtl);

                    //gridMonthPlan[colForwardBillDtlIdOfMonth.Name, dgOpRowIndex].Value = dtl.Id;
                    #endregion

                    #region 树方式
                    string[] sysCodes = dtl.GWBSTreeSysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < sysCodes.Length; i++)
                    {
                        string sysCode = sysCodes[i];
                        foreach (ProductionScheduleDetail ddtl in listProductionScheduleDetail)
                        {
                            if (ddtl.GWBSTree == null && ddtl.Level == 1)
                            {
                                listSysCodes.Add(ddtl);
                            }
                            else if (ddtl.GWBSTree != null && sysCode == ddtl.GWBSTree.Id)
                            {
                                listSysCodes.Add(ddtl);
                            }
                        }
                    }
                    #endregion
                }

                #region 树方式
                IList newlistProDtl = new ArrayList();
                for (int i = 0; i < listSysCodes.Count; i++)
                {
                    ProductionScheduleDetail dtl = listSysCodes[i] as ProductionScheduleDetail;
                    if (newlistProDtl.Count == 0)
                    {
                        newlistProDtl.Add(dtl);
                    }
                    else
                    {
                        bool flag = false;
                        foreach (ProductionScheduleDetail ddtl in newlistProDtl)
                        {
                            if (dtl.Id == ddtl.Id)
                            {
                                flag = true;
                            }
                        }
                        if (!flag)
                        {
                            newlistProDtl.Add(dtl);
                        }
                    }
                }

                IList listProChilds = model.ProductionManagementSrv.GetProChilds(newlistProDtl);
                curBillMaster = SavePlanDtl(listProChilds);
                GridFillFlex();
                #endregion
            }
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (curBillMaster == null)
            {
                MessageBox.Show("当前没有要操作的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (curBillMaster.DocState != DocumentState.Edit)
            {
                MessageBox.Show("当前计划状态为“" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "”,只能删除“编辑”状态的计划！");
                return;
            }

            int activeRowIndex = flexGrid.ActiveCell.Row;
            string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;

            if (detailId != null && !detailId.Equals(""))
            {
                WeekScheduleDetail detail = (WeekScheduleDetail)detailHashtable[detailId];

                if (detail == null)
                {
                    MessageBox.Show("选择计划明细不存在(或已被其他操作员删除),请重载该计划！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("确定要删除【" + detail.GWBSTreeName + "】及其下属进度计划吗?", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        FlashScreen.Show("正在执行删除操作,请稍候......");

                        int childs = 0;
                        string errMsg = "";

                        IList list = model.ProductionManagementSrv.DeleteWeekScheduleDetail(detail, childs, errMsg);

                        errMsg = list[0] as string;
                        childs = Convert.ToInt32(list[1]);

                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            FlashScreen.Close();
                            MessageBox.Show(errMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        //flexGrid.AutoRedraw = false;

                        int rowIndexs = activeRowIndex + childs;
                        for (int i = rowIndexs; i >= activeRowIndex; i--)
                        {
                            flexGrid.Row(i).Delete();
                        }
                        //flexGrid.AutoRedraw = true;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除失败.\n" + ex.Message);
                        return;
                    }
                    finally
                    {
                        FlashScreen.Close();
                    }

                    MessageBox.Show("删除成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", curBillMaster.Id));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    IList list1 = model.ObjectQuery(typeof(WeekScheduleMaster), oq);
                    curBillMaster = list1[0] as WeekScheduleMaster;
                }
            }
        }


        private void SaveScheduleDetail(IList listProdtl)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", curBillMaster.Id));
            oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            IList listWeekScheduleDtl = model.ObjectQuery(typeof(WeekScheduleDetail), oq);

            if (listWeekScheduleDtl == null || listWeekScheduleDtl.Count == 0)
            {
                foreach (ProductionScheduleDetail pdtl in listProdtl)
                {
                    DateTime beginDate = ClientUtil.ToDateTime(this.dtpDateBegin.Text);
                    DateTime endDate = ClientUtil.ToDateTime(this.dtpDateEnd.Text);

                    WeekScheduleDetail wdtl = new WeekScheduleDetail();

                    if (pdtl.GWBSTree != null)
                    {
                        wdtl.GWBSTree = pdtl.GWBSTree;
                        wdtl.GWBSTreeName = pdtl.GWBSTreeName;
                        wdtl.GWBSTreeSysCode = pdtl.GWBSTreeSysCode;

                        DateTime plannedBeginDate = pdtl.PlannedBeginDate;
                        DateTime plannedEndDate = pdtl.PlannedEndDate;


                        if (plannedBeginDate == ClientUtil.ToDateTime("1900-1-1") && plannedEndDate == ClientUtil.ToDateTime("1900-1-1"))
                        {
                            wdtl.PlannedBeginDate = pdtl.PlannedBeginDate;
                            wdtl.PlannedEndDate = pdtl.PlannedEndDate;
                        }
                        else if (beginDate >= plannedBeginDate && endDate <= plannedEndDate)
                        {
                            wdtl.PlannedBeginDate = beginDate;
                            wdtl.PlannedEndDate = endDate;
                            wdtl.PlannedDuration = (endDate - beginDate).Days + 1;
                        }
                        else if (beginDate >= plannedEndDate)
                        {
                            wdtl.PlannedBeginDate = beginDate;
                            wdtl.PlannedEndDate = endDate;
                            wdtl.PlannedDuration = (endDate - beginDate).Days + 1;
                        }
                        else if (beginDate <= plannedBeginDate && endDate >= plannedEndDate)
                        {
                            wdtl.PlannedBeginDate = plannedBeginDate;
                            wdtl.PlannedEndDate = plannedEndDate;
                            wdtl.PlannedDuration = (plannedEndDate - plannedBeginDate).Days + 1;
                        }
                        else if (endDate <= plannedBeginDate)
                        {
                            wdtl.PlannedBeginDate = beginDate;
                            wdtl.PlannedEndDate = endDate;
                            wdtl.PlannedDuration = (endDate - beginDate).Days + 1;
                        }
                        else if (plannedBeginDate >= beginDate && plannedBeginDate < endDate && plannedEndDate > beginDate && plannedEndDate <= endDate)
                        {
                            wdtl.PlannedBeginDate = plannedBeginDate;
                            wdtl.PlannedEndDate = plannedEndDate;
                            wdtl.PlannedDuration = (plannedEndDate - plannedBeginDate).Days + 1;
                        }
                        else if (beginDate >= plannedBeginDate && endDate >= plannedEndDate)
                        {
                            wdtl.PlannedBeginDate = beginDate;
                            wdtl.PlannedEndDate = plannedEndDate;
                            wdtl.PlannedDuration = (plannedEndDate - beginDate).Days + 1;
                        }
                        else if (beginDate <= plannedBeginDate && endDate <= plannedEndDate)
                        {
                            wdtl.PlannedBeginDate = plannedBeginDate;
                            wdtl.PlannedEndDate = endDate;
                            wdtl.PlannedDuration = (endDate - plannedBeginDate).Days + 1;
                        }


                        wdtl.Master = curBillMaster;
                        curBillMaster.AddDetail(wdtl);
                    }
                }
            }
            else
            {
                for (int i = 0; i < listProdtl.Count; i++)
                {
                    bool flag = false;
                    ProductionScheduleDetail dtl = listProdtl[i] as ProductionScheduleDetail;
                    foreach (WeekScheduleDetail wdtl in listWeekScheduleDtl)
                    {
                        if (dtl.GWBSTree != null)
                        {
                            if (dtl.GWBSTree.Id == wdtl.GWBSTree.Id)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (!flag)
                    {
                        WeekScheduleDetail weekDtl = new WeekScheduleDetail();

                        if (dtl.GWBSTree != null)
                        {
                            weekDtl.GWBSTree = dtl.GWBSTree;
                            weekDtl.GWBSTreeName = dtl.GWBSTreeName;
                            weekDtl.GWBSTreeSysCode = dtl.GWBSTreeSysCode;

                            weekDtl.Master = curBillMaster;
                            curBillMaster.AddDetail(weekDtl);
                        }
                    }
                }
            }
        }


        private void SetMonthPlanDateView(DataGridViewRow row, ProductionScheduleDetail detail)
        {
            DateTime startTime = detail.PlannedBeginDate >= dtpDateBegin.Value.Date ? detail.PlannedBeginDate : dtpDateBegin.Value.Date;
            DateTime endTime = detail.PlannedEndDate >= dtpDateEnd.Value.Date ? dtpDateEnd.Value.Date : detail.PlannedEndDate;

            row.Cells[colOPPlannedDateBegin.Name].Value = startTime;
            row.Cells[colOPPlannedDateEnd.Name].Value = endTime;
            row.Cells[colOPPlannedDuration.Name].Value = (endTime - startTime).Days + 1;
        }

        private void SetPlanViewBySelect(DataGridViewRow dr, ProductionScheduleDetail detail)
        {
            DateTime beginDate = detail.PlannedBeginDate.Date;
            DateTime endDate = detail.PlannedEndDate.Date;

            int day = (beginDate - dtpDateBegin.Value.Date).Days;

            int startColumnIndex = day + dr.Cells[colMonday.Name].ColumnIndex;

            int endColumnIndex = startColumnIndex + (endDate - beginDate).Days;

            endColumnIndex = endColumnIndex > dr.Cells[colSunday.Name].ColumnIndex ? dr.Cells[colSunday.Name].ColumnIndex : endColumnIndex;

            SetPlanViewBySelect(dr, startColumnIndex, endColumnIndex);
        }
        private void SetPlanViewBySelect(DataGridViewRow dr, WeekScheduleDetail detail)
        {
            DateTime beginDate = detail.PlannedBeginDate.Date;
            DateTime endDate = detail.PlannedEndDate.Date;

            int day = (beginDate - dtpDateBegin.Value.Date).Days;

            int startColumnIndex = day + dr.Cells[colMonday.Name].ColumnIndex;

            int endColumnIndex = startColumnIndex + (endDate - beginDate).Days;

            endColumnIndex = endColumnIndex > dr.Cells[colSunday.Name].ColumnIndex ? dr.Cells[colSunday.Name].ColumnIndex : endColumnIndex;

            SetPlanViewBySelect(dr, startColumnIndex, endColumnIndex);
        }

        private void SetPlanViewBySelect(DataGridViewRow dr, int columnIndex, int endColumnIndex)
        {
            for (int i = columnIndex; i <= endColumnIndex; i++)
            {
                if (gridWeekPlanDetail.Columns[colMonday.Name].Index <= i && i <= gridWeekPlanDetail.Columns[colSunday.Name].Index)
                    dr.Cells[i].Value = true;
            }
        }

        /// <summary>
        /// 判断滚动进度计划明细是否已经添加
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="enumExecScheduleType"></param>
        /// <returns></returns>
        private bool HasAdded(ProductionScheduleDetail detail, EnumExecScheduleType enumExecScheduleType)
        {
            if (enumExecScheduleType == EnumExecScheduleType.周进度计划)
            {
                foreach (DataGridViewRow dr in gridWeekPlanDetail.Rows)
                {
                    string forwardBillDtlId = dr.Cells[colForwardBillDtlIdOfWeek.Name].Value + "";
                    if (detail.Id == forwardBillDtlId)
                    {
                        return true;
                    }
                }
            }
            else if (enumExecScheduleType == EnumExecScheduleType.月度进度计划)
            {
                foreach (DataGridViewRow dr in gridMonthPlan.Rows)
                {
                    string forwardBillDtlId = dr.Cells[colForwardBillDtlIdOfMonth.Name].Value + "";
                    if (detail.Id == forwardBillDtlId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = gridWeekPlanDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                gridWeekPlanDetail.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as WeekScheduleDetail);
                }
            }
        }

        void dgDetail_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (e.Row.Tag != null)
            {
                curBillMaster.Details.Remove(e.Row.Tag as WeekScheduleDetail);
            }
        }

        #region 固定代码
        /// <summary>
        /// 启动本窗体,(设置状态或重新加载已有的数据)
        /// </summary>
        /// <param name="code">窗体Caption</param>
        public void Start(string code)
        {
            try
            {
                if (code == "空")
                    RefreshState(MainViewState.Initialize);
                else
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Code", code));
                    oq.AddCriterion(Expression.Eq("ProjectId", StaticMethod.GetProjectInfo().Id));

                    curBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterByCode(oq);
                    ModelToView();
                    RefreshState(MainViewState.Browser);

                    //判断是否为制单人
                    PersonInfo pi = this.txtCreatePerson.Tag as PersonInfo;
                    string perid = ConstObject.LoginPersonInfo.Id;
                    if (pi != null && !pi.Id.Equals(perid))
                    {
                        RefreshStateByQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 刷新状态(按钮状态)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //控制表格
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    this.gridWeekPlanDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    foreach (DataGridViewRow dr in gridWeekPlanDetail.Rows)
                    {
                        SetBackColor(dr);
                    }
                    gridMonthPlan.EditMode = DataGridViewEditMode.EditOnEnter;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.gridWeekPlanDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    foreach (DataGridViewRow dr in gridWeekPlanDetail.Rows)
                    {
                        SetBackColor(dr);
                    }
                    gridMonthPlan.EditMode = DataGridViewEditMode.EditProgrammatically;
                    cmsDg.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        #endregion


        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //控制自身控件
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
                ObjectLock.Unlock(btnExportToMPP, true);
            }

            //永久锁定
            object[] os = new object[] { txtCreatePerson, txtSumPlanQuantity, txtProject, txtProductSchedule, txtSumActualQuantity };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colGWBSTree.Name, colOBSService.Name };
            gridWeekPlanDetail.SetColumnsReadOnly(lockCols);
            string[] otherLockCols = new string[] { colOPGWBSTree.Name };
            gridMonthPlan.SetColumnsReadOnly(otherLockCols);
        }

        //清空数据
        private void ClearView()
        {
            ClearControl(pnlFloor);
        }

        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }
            //自定义控件清空
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public override bool NewView()
        {
            try
            {
                base.NewView();
                ClearView();
                InitFlexGrid(1);

                this.curBillMaster = new WeekScheduleMaster();
                curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                //curBillMaster.CreateDate = ConstObject.LoginDate;
                //curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                //curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                //curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                //curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                //curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.HandlePersonSyscode = ConstObject.TheOperationOrg.SysCode;
                curBillMaster.HandleOrg = ConstObject.TheOperationOrg;
                curBillMaster.HandOrgLevel = ConstObject.TheOperationOrg.Level;

                curBillMaster.SummaryStatus = EnumSummaryStatus.未汇总;
                //curBillMaster.ExecScheduleType = this.execScheduleType;

                //制单人
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                //txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //归属项目
                projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }

                cbPlanType.Text = "月度进度计划";

                curBillMaster = model.ProductionManagementSrv.SaveWeekScheduleMaster(curBillMaster);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public override bool ModifyView()
        {
            curBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterById(curBillMaster.Id);
            if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
            {
                base.ModifyView();
                ModelToView();
                return true;
            }
            MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能修改！");
            return false;

        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        public override bool SaveView()
        {
            try
            {
                if (!ViewToModel())
                    return false;

                if (curBillMaster.Id == null)
                {
                    curBillMaster = model.ProductionManagementSrv.SaveWeekScheduleMaster(curBillMaster);
                    if (execScheduleType == EnumExecScheduleType.周进度计划)
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "周进度计划", "", curBillMaster.ProjectName);
                    }
                    else
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "月进度计划", "", curBillMaster.ProjectName);
                    }
                }
                else
                {
                    curBillMaster = model.ProductionManagementSrv.SaveWeekScheduleMaster(curBillMaster);
                    if (execScheduleType == EnumExecScheduleType.周进度计划)
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "周进度计划", "", curBillMaster.ProjectName);
                    }
                    else
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "月进度计划", "", curBillMaster.ProjectName);
                    }
                }

                //更新Caption
                this.ViewCaption = ViewName + "-" + curBillMaster.Code;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
            }
            return false;
        }

        public override bool SubmitView()
        {
            if (!ViewToModel())
                return false;

            if (MessageBox.Show("确定要提交当前单据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    FlashScreen.Show("正在执行提交，请稍候........");

                    curBillMaster.DocState = DocumentState.InExecute;
                    curBillMaster.SubmitDate = DateTime.Now;
                    LogData log = new LogData();
                    log.BillType = execScheduleType.ToString() + "单";
                    if (string.IsNullOrEmpty(curBillMaster.Id))
                        log.OperType = "新增提交";
                    else
                        log.OperType = "修改提交";

                    curBillMaster = model.ProductionManagementSrv.SaveWeekScheduleMaster(curBillMaster);

                    log.BillId = curBillMaster.Id;
                    log.Code = curBillMaster.Code;
                    log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);

                    //更新Caption
                    this.ViewCaption = ViewName + "-" + curBillMaster.Code;

                    return true;
                }
                catch (Exception e)
                {
                    FlashScreen.Close();
                    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
                }
                finally
                {
                    FlashScreen.Close();
                }
            }
            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                curBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterById(curBillMaster.Id);
                if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
                {
                    if (!model.ProductionManagementSrv.DeleteByDao(curBillMaster))
                        return false;

                    if (execScheduleType == EnumExecScheduleType.周进度计划)
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "周进度计划", "", curBillMaster.ProjectName);
                    }
                    else
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "月进度计划", "", curBillMaster.ProjectName);
                    }

                    ClearView();
                    flexGrid.Rows = 1;
                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 撤销
        /// </summary>
        /// <returns></returns>
        public override bool CancelView()
        {
            try
            {
                switch (ViewState)
                {
                    case MainViewState.Modify:
                        //重新查询数据
                        curBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterById(curBillMaster.Id);
                        ModelToView();
                        break;
                    default:
                        ClearView();
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据撤销错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                //重新获得当前对象的值
                curBillMaster = model.ProductionManagementSrv.GetWeekScheduleMasterById(curBillMaster.Id);
                //给界面赋值
                ModelToView();
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            if (txtPlanName.Text.Trim() == "")
            {
                MessageBox.Show("计划名称不能为空！");
                txtPlanName.Focus();
                return false;
            }

            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                if (this.gridWeekPlanDetail.Rows.Count == 0)
                {
                    MessageBox.Show("明细不能为空！");
                    return false;
                }
                gridWeekPlanDetail.EndEdit();
                //周进度计划数据校验
                foreach (DataGridViewRow dr in gridWeekPlanDetail.Rows)
                {
                    //最后一行不进行校验
                    if (dr.IsNewRow)
                        break;
                    if ((dr.Cells[colMonday.Name].Value == null || !(bool)dr.Cells[colMonday.Name].Value) &&
                        (dr.Cells[colTuesday.Name].Value == null || !(bool)dr.Cells[colTuesday.Name].Value) &&
                        (dr.Cells[colWednesday.Name].Value == null || !(bool)dr.Cells[colWednesday.Name].Value) &&
                        (dr.Cells[colThursday.Name].Value == null || !(bool)dr.Cells[colThursday.Name].Value) &&
                        (dr.Cells[colFriday.Name].Value == null || !(bool)dr.Cells[colFriday.Name].Value) &&
                        (dr.Cells[colSaturday.Name].Value == null || !(bool)dr.Cells[colSaturday.Name].Value) &&
                        (dr.Cells[colSunday.Name].Value == null || !(bool)dr.Cells[colSunday.Name].Value))
                    {
                        MessageBox.Show("请设置明细【计划日期】。");
                        gridWeekPlanDetail.CurrentCell = dr.Cells[colMonday.Name];
                        return false;
                    }
                    object plannedDuration = dr.Cells[colPlannedDuration.Name].Value;
                    if (plannedDuration == null || plannedDuration.ToString() == "" || ClientUtil.ToInt(plannedDuration) <= 0)
                    {
                        MessageBox.Show("【计划工期】不能为空或小于等于0！");
                        gridWeekPlanDetail.CurrentCell = dr.Cells[colPlannedDuration.Name];
                        return false;
                    }

                    //if (dr.Cells[colOBSService.Name].Tag == null || ClientUtil.ToString(dr.Cells[colOBSService.Name].Value) == "")
                    //{
                    //    MessageBox.Show("【承担者】不能为空，请双击选择！");
                    //    gridWeekPlanDetail.CurrentCell = dr.Cells[colOBSService.Name];
                    //    return false;
                    //}

                    gridWeekPlanDetail.Update();
                }
            }
            else if (execScheduleType == EnumExecScheduleType.月度进度计划)
            {
                //if (this.gridMonthPlan.Rows.Count == 0)
                //{
                //    MessageBox.Show("明细不能为空！");
                //    return false;
                //}
                //gridMonthPlan.EndEdit();
                ////其它计划数据校验
                //foreach (DataGridViewRow var in gridMonthPlan.Rows)
                //{
                //    if (var.IsNewRow) break;
                //    DateTime otherPlanBeginDate = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateBegin.Name].Value);
                //    DateTime otherPlanEndDate = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateEnd.Name].Value);
                //    if (otherPlanBeginDate.Date > otherPlanEndDate.Date)
                //    {
                //        MessageBox.Show("明细【计划开始时间】不能大于【计划结束时间】。");
                //        gridMonthPlan.CurrentCell = var.Cells[colOPPlannedDateBegin.Name];
                //        return false;
                //    }

                //    object otherDuration = var.Cells[colOPPlannedDuration.Name].EditedFormattedValue;
                //    if (otherDuration == null)
                //    {
                //        var.Cells[colOPPlannedDuration.Name].Value = (otherPlanEndDate.Date - otherPlanBeginDate.Date).Days + 1;
                //        otherDuration = (otherPlanEndDate.Date - otherPlanBeginDate.Date).Days + 1;
                //    }

                //    if (otherDuration == null || otherDuration.ToString() == "" || ClientUtil.ToDecimal(otherDuration) <= 0)
                //    {
                //        MessageBox.Show("【计划工期】不能为空或小于等于0！");
                //        gridMonthPlan.CurrentCell = var.Cells[colOPPlannedDuration.Name];
                //        return false;
                //    }

                //object otherWorkload = var.Cells[colOPPlannedWorkload.Name].Value;
                //if (otherWorkload == null || otherWorkload.ToString() == "" || ClientUtil.TransToDecimal(otherWorkload) <= 0)
                //{
                //    MessageBox.Show("计划工程量不能为空或小于等于0！！");
                //    dgOtherPlan.CurrentCell = var.Cells[colOPPlannedWorkload.Name];
                //    return false;
                //}
                //}
                //gridMonthPlan.Update();
            }

            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView())
                return false;
            try
            {
                curBillMaster.ForwardBillCode = txtProductSchedule.Text;

                curBillMaster.PlanName = txtPlanName.Text.Trim();
                curBillMaster.PlannedBeginDate = dtpDateBegin.Value.Date;
                curBillMaster.PlannedEndDate = dtpDateEnd.Value.Date;
                curBillMaster.Descript = txtRemark.Text;
                curBillMaster.CreateDate = dtpBusinessDate.Value.Date;
                curBillMaster.AccountYear = ClientUtil.ToInt(cmoYear.Text);
                curBillMaster.AccountMonth = ClientUtil.ToInt(cmoStartMonth.Text);

                if (this.execScheduleType == EnumExecScheduleType.周进度计划)
                {
                    curBillMaster.ExecScheduleType = this.execScheduleType;
                }
                else
                {
                    if (cbPlanType.Text == "月度进度计划")
                    {
                        execScheduleType = EnumExecScheduleType.月度进度计划;
                        curBillMaster.ExecScheduleType = this.execScheduleType;
                    }
                    else if (cbPlanType.Text == "季度进度计划")
                    {
                        execScheduleType = EnumExecScheduleType.季度进度计划;
                        curBillMaster.ExecScheduleType = this.execScheduleType;
                    }
                    else
                    {
                        execScheduleType = EnumExecScheduleType.总体进度计划;
                        curBillMaster.ExecScheduleType = this.execScheduleType;
                    }
                }

                //curBillMaster.SumPlannedWorkload = ClientUtil.ToDecimal(txtSumPlanQuantity.Text);
                //curBillMaster.SumActualWorkload = ClientUtil.ToDecimal(txtSumActualQuantity.Text);
                if (execScheduleType == EnumExecScheduleType.周进度计划)
                {
                    SetGridRowWeekDate();

                    foreach (DataGridViewRow var in this.gridWeekPlanDetail.Rows)
                    {
                        if (var.IsNewRow)
                            break;
                        WeekScheduleDetail curBillDtl = new WeekScheduleDetail();
                        if (var.Tag != null)
                        {
                            curBillDtl = var.Tag as WeekScheduleDetail;
                            if (curBillDtl.Id == null)
                            {
                                curBillMaster.Details.Remove(curBillDtl);
                            }
                        }

                        curBillDtl.ProjectId = curBillMaster.ProjectId;
                        curBillDtl.ProjectName = curBillMaster.ProjectName;

                        curBillDtl.SummaryStatus = EnumSummaryStatus.未汇总;
                        //周进度计划
                        curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                        if ((curBillDtl.GWBSTree == null || string.IsNullOrEmpty(curBillDtl.GWBSTreeName))
                            && var.Cells[colGWBSTree.Name].Tag != null)
                        {
                            curBillDtl.GWBSTree = var.Cells[colGWBSTree.Name].Tag as GWBSTree;
                            curBillDtl.GWBSTreeName = ClientUtil.ToString(var.Cells[colGWBSTree.Name].Value);
                            curBillDtl.GWBSTreeSysCode = curBillDtl.GWBSTree.SysCode;
                            curBillDtl.NodeType = curBillDtl.GWBSTree.CategoryNodeType;

                            curBillDtl.TaskCheckState = curBillDtl.GWBSTree.CheckRequire;
                        }

                        curBillDtl.MainTaskContent = ClientUtil.ToString(var.Cells[colMainTaskContent.Name].Value);
                        curBillDtl.PlannedBeginDate = CalWeekPlanBeginDate(var);
                        curBillDtl.PlannedEndDate = CalWeekPlanEndDate(var);
                        curBillDtl.PlannedDuration = ClientUtil.ToDecimal(var.Cells[colPlannedDuration.Name].Value);

                        curBillDtl.ActualBeginDate = curBillDtl.PlannedBeginDate;

                        curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                        curBillDtl.ForwardBillDtlId = var.Cells[colForwardBillDtlIdOfWeek.Name].Value + "";

                        curBillDtl.SubContractProject = var.Cells[colOBSService.Name].Tag as SubContractProject;
                        curBillDtl.SupplierName = var.Cells[colOBSService.Name].Value + "";

                        if (var.Cells[colPBSTree.Name].Tag != null)
                        {
                            curBillDtl.PBSTree = var.Cells[colPBSTree.Name].Tag as PBSTree;
                            curBillDtl.PBSTreeName = ClientUtil.ToString(var.Cells[colPBSTree.Name].Value);
                        }

                        curBillMaster.AddDetail(curBillDtl);

                        var.Tag = curBillDtl;
                    }
                }
                else
                {
                    //foreach (DataGridViewRow var in this.gridMonthPlan.Rows)
                    //{
                    //    if (var.IsNewRow) break;
                    //    WeekScheduleDetail curBillDtl = new WeekScheduleDetail();
                    //    if (var.Tag != null)
                    //    {
                    //        curBillDtl = var.Tag as WeekScheduleDetail;
                    //        if (curBillDtl.Id == null)
                    //        {
                    //            curBillMaster.Details.Remove(curBillDtl);
                    //        }
                    //    }

                    //    curBillDtl.ProjectId = curBillMaster.ProjectId;
                    //    curBillDtl.ProjectName = curBillMaster.ProjectName;

                    //    curBillDtl.SummaryStatus = EnumSummaryStatus.未汇总;

                    //    //其它计划
                    //    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colOPDescript.Name].Value);
                    //    if ((curBillDtl.GWBSTree == null || string.IsNullOrEmpty(curBillDtl.GWBSTreeName))
                    //        && var.Cells[colOPGWBSTree.Name].Tag != null)
                    //    {
                    //        curBillDtl.GWBSTree = var.Cells[colOPGWBSTree.Name].Tag as GWBSTree;
                    //        curBillDtl.GWBSTreeName = ClientUtil.ToString(var.Cells[colOPGWBSTree.Name].Value);
                    //        curBillDtl.GWBSTreeSysCode = curBillDtl.GWBSTree.SysCode;
                    //        curBillDtl.NodeType = curBillDtl.GWBSTree.CategoryNodeType;

                    //        curBillDtl.TaskCheckState = curBillDtl.GWBSTree.CheckRequire;
                    //    }
                    //    curBillDtl.MainTaskContent = ClientUtil.ToString(var.Cells[colOPMainTaskContent.Name].Value);
                    //    curBillDtl.PlannedBeginDate = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateBegin.Name].Value);
                    //    curBillDtl.PlannedEndDate = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateEnd.Name].Value);
                    //    curBillDtl.PlannedDuration = ClientUtil.ToDecimal(var.Cells[colOPPlannedDuration.Name].Value);

                    //    curBillDtl.ActualBeginDate = curBillDtl.PlannedBeginDate;

                    //    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colOPDescript.Name].Value);
                    //    curBillDtl.ForwardBillDtlId = var.Cells[colForwardBillDtlIdOfMonth.Name].Value + "";

                    //    if (var.Cells[colOPPBSTree.Name].Tag != null)
                    //    {
                    //        curBillDtl.PBSTree = var.Cells[colOPPBSTree.Name].Tag as PBSTree;
                    //        curBillDtl.PBSTreeName = ClientUtil.ToString(var.Cells[colOPPBSTree.Name].Value);
                    //    }
                    //    curBillMaster.AddDetail(curBillDtl);
                    //}

                    ViewToDetails();
                    curBillMaster = model.ProductionManagementSrv.SaveOrUpdateByDao(curBillMaster) as WeekScheduleMaster;

                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }

        private DateTime CalWeekPlanBeginDate(DataGridViewRow var)
        {
            int startColumnIndex = var.Cells[colMonday.Name].ColumnIndex;
            int endColumnIndex = var.Cells[colSunday.Name].ColumnIndex;
            for (int i = startColumnIndex; i <= endColumnIndex; i++)
            {
                object planValue = var.Cells[i].Value;
                if (planValue != null && (bool)planValue)
                {
                    return (DateTime)var.Cells[i].Tag;
                }
            }
            return new DateTime(1900, 1, 1);
        }

        private DateTime CalWeekPlanEndDate(DataGridViewRow var)
        {
            int startColumnIndex = var.Cells[colMonday.Name].ColumnIndex;
            int endColumnIndex = var.Cells[colSunday.Name].ColumnIndex;
            for (int i = endColumnIndex; i >= startColumnIndex; i--)
            {
                object planValue = var.Cells[i].Value;
                if (planValue != null && (bool)planValue)
                {
                    return (DateTime)var.Cells[i].Tag;
                }
            }
            return new DateTime(1900, 1, 1);
        }

        //显示数据
        private bool ModelToView()
        {
            try
            {
                txtProductSchedule.Text = curBillMaster.ForwardBillCode;

                dtpDateBegin.Value = curBillMaster.PlannedBeginDate;
                dtpDateEnd.Value = curBillMaster.PlannedEndDate;

                txtPlanName.Text = curBillMaster.PlanName;
                txtProject.Text = curBillMaster.ProjectName;
                txtRemark.Text = curBillMaster.Descript;

                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                //txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                dtpBusinessDate.Value = curBillMaster.CreateDate;
                txtProject.Text = curBillMaster.ProjectName;
                txtSumPlanQuantity.Text = curBillMaster.SumPlannedWorkload.ToString("######.####");
                txtSumActualQuantity.Text = curBillMaster.SumActualWorkload.ToString("######.####");

                cmoYear.Text = curBillMaster.AccountYear.ToString();
                cmoStartMonth.Text = curBillMaster.AccountMonth.ToString();

                this.cbPlanType.SelectedItem = ClientUtil.ToString(curBillMaster.ExecScheduleType);

                this.gridWeekPlanDetail.Rows.Clear();
                gridMonthPlan.Rows.Clear();
                foreach (WeekScheduleDetail var in curBillMaster.Details)
                {
                    //周进度计划
                    if (execScheduleType == EnumExecScheduleType.周进度计划)
                    {
                        int i = this.gridWeekPlanDetail.Rows.Add();
                        gridWeekPlanDetail.Rows[i].Cells[colGWBSTree.Name].Tag = var.GWBSTree;
                        gridWeekPlanDetail.Rows[i].Cells[colGWBSTree.Name].Value = var.GWBSTreeName;

                        string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), var.GWBSTreeName, var.GWBSTreeSysCode);
                        gridWeekPlanDetail.Rows[i].Cells[colGWBSTree.Name].ToolTipText = fullPath;

                        gridWeekPlanDetail.Rows[i].Cells[colMainTaskContent.Name].Value = var.MainTaskContent;
                        //显示周一到周日
                        //SetPlanView(gridWeekPlanDetail.Rows[i], var);
                        //SetPlanTag(gridWeekPlanDetail.Rows[i]);

                        SetPlanViewBySelect(gridWeekPlanDetail.Rows[i], var);
                        SetPlanTag(gridWeekPlanDetail.Rows[i]);

                        //SetBackColor(dgDetail.Rows[i]);
                        //dgDetail.Rows[i].Cells[colPlannedBeginDate.Name].Value=var.PlannedBeginDate;
                        //dgDetail.Rows[i].Cells[colPlannedEndDate.Name].Value=var.PlannedEndDate;
                        gridWeekPlanDetail.Rows[i].Cells[colPlannedDuration.Name].Value = var.PlannedDuration;
                        gridWeekPlanDetail.Rows[i].Cells[colDescript.Name].Value = var.Descript;
                        gridWeekPlanDetail.Rows[i].Cells[colOBSService.Name].Value = var.SupplierName;
                        gridWeekPlanDetail.Rows[i].Cells[colOBSService.Name].Tag = var.SubContractProject;

                        gridWeekPlanDetail.Rows[i].Cells[colPBSTree.Name].Tag = var.PBSTree;
                        gridWeekPlanDetail.Rows[i].Cells[colPBSTree.Name].Value = var.PBSTreeName;
                        gridWeekPlanDetail.Rows[i].Cells[colForwardBillDtlIdOfWeek.Name].Value = var.ForwardBillDtlId;

                        this.gridWeekPlanDetail.Rows[i].Tag = var;
                    }
                    else
                    {
                        //其它计划

                        if (var.GWBSTree != null)
                        {
                            int i = this.gridMonthPlan.Rows.Add();
                            gridMonthPlan.Rows[i].Cells[colOPGWBSTree.Name].Tag = var.GWBSTree;
                            gridMonthPlan.Rows[i].Cells[colOPGWBSTree.Name].Value = var.GWBSTreeName;
                            string fullPath = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), var.GWBSTreeName, var.GWBSTreeSysCode);
                            gridMonthPlan.Rows[i].Cells[colOPGWBSTree.Name].ToolTipText = fullPath;


                            gridMonthPlan.Rows[i].Cells[colOPMainTaskContent.Name].Value = var.MainTaskContent;
                            gridMonthPlan.Rows[i].Cells[colOPPlannedDateBegin.Name].Value = var.PlannedBeginDate;
                            gridMonthPlan.Rows[i].Cells[colOPPlannedDateEnd.Name].Value = var.PlannedEndDate;
                            gridMonthPlan.Rows[i].Cells[colOPPlannedDuration.Name].Value = var.PlannedDuration;
                            gridMonthPlan.Rows[i].Cells[colOPDescript.Name].Value = var.Descript;

                            gridMonthPlan.Rows[i].Cells[colOPPBSTree.Name].Tag = var.PBSTree;
                            gridMonthPlan.Rows[i].Cells[colOPPBSTree.Name].Value = var.PBSTreeName;
                            gridMonthPlan.Rows[i].Cells[colForwardBillDtlIdOfMonth.Name].Value = var.ForwardBillDtlId;

                            this.gridMonthPlan.Rows[i].Tag = var;
                        }

                    }
                }

                GridFillFlex();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void ClearVersionData()
        {
            flexGrid.Rows = 1;

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
                    cellStyle.BackColor = ColorTranslator.FromHtml("#D7E8FE");// Color.Red;
                }
                else
                {
                    if (gridWeekPlanDetail.EditMode == DataGridViewEditMode.EditProgrammatically)
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

        private void SetPlanTag(DataGridViewRow dr)
        {
            DateTime dt = curBillMaster.PlannedBeginDate.Date;
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
                    SetPlanView(dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Tuesday:
                    columnIndex = dr.Cells[colTuesday.Name].ColumnIndex;
                    SetPlanView(dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Wednesday:
                    columnIndex = dr.Cells[colWednesday.Name].ColumnIndex;
                    SetPlanView(dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Thursday:
                    columnIndex = dr.Cells[colThursday.Name].ColumnIndex;
                    SetPlanView(dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Friday:
                    columnIndex = dr.Cells[colFriday.Name].ColumnIndex;
                    SetPlanView(dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Saturday:
                    columnIndex = dr.Cells[colSaturday.Name].ColumnIndex;
                    SetPlanView(dr, columnIndex, planDays);
                    break;
                case DayOfWeek.Sunday:
                    columnIndex = dr.Cells[colSunday.Name].ColumnIndex;
                    SetPlanView(dr, columnIndex, planDays);
                    break;
            }
        }

        private void SetPlanView(DataGridViewRow dr, int columnIndex, int planDays)
        {
            DateTime dt = curBillMaster.PlannedBeginDate.Date;
            dr.Cells[columnIndex].Value = true;
            for (int i = 0; i < planDays; i++)
            {
                dr.Cells[columnIndex + i].Value = true;
            }
        }


        /// <summary>
        /// 在光标跳自动转到下一列时，首先执行校验，确定是否跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //bool validity = true;
            //string colName = gridWeekPlanDetail.Columns[e.ColumnIndex].Name;
            //if (colName == colPlannedWrokload.Name || colName == colActualWorkload.Name)
            //{
            //    object pWorkload = gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].Value;
            //    if (pWorkload == null) pWorkload = "";
            //    if (!CommonMethod.VeryValid(pWorkload.ToString()))
            //    {
            //        MessageBox.Show("请输入数字！");
            //        return;
            //    }

            //    object aWorkLoad = gridWeekPlanDetail[e.ColumnIndex, e.RowIndex].Value;
            //    if (aWorkLoad == null) aWorkLoad = "";
            //    if (!CommonMethod.VeryValid(aWorkLoad.ToString()))
            //    {
            //        MessageBox.Show("请输入数字！");
            //        return;
            //    }

            //    decimal sumpWorkload = 0M, sumaWorkload = 0M;
            //    foreach (DataGridViewRow dr in gridWeekPlanDetail.Rows)
            //    {
            //        sumpWorkload += ClientUtil.ToDecimal(dr.Cells[colPlannedWrokload.Name].Value);
            //        sumaWorkload += ClientUtil.ToDecimal(dr.Cells[colActualWorkload.Name].Value);
            //    }
            //    txtSumPlanQuantity.Text = sumpWorkload.ToString("###########.####");
            //    txtSumActualQuantity.Text = sumaWorkload.ToString("###########.####");
            //}
        }

        #region 打印处理
        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <returns></returns>
        //public override bool Preview()
        //{
        //    if (LoadTempleteFile(@"施工生产周进度计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.PrintPreview(true, true, true, 0, 0, 0, 0, 0);
        //    return true;
        //}

        //public override bool Print()
        //{
        //    if (LoadTempleteFile(@"施工生产周进度计划.flx") == false) return false;
        //FillFlex(curBillMaster);
        //    flexGrid1.Print();
        //    return true;
        //}

        //public override bool Export()
        //{
        //    if (LoadTempleteFile(@"施工生产周进度计划.flx") == false) return false;
        //    FillFlex(curBillMaster);
        //    flexGrid1.ExportToExcel("施工生产周进度计划【" + curBillMaster.Code + "】", false, false, true);
        //    return true;
        //}

        //private bool LoadTempleteFile(string modelName)
        //{
        //    ExploreFile eFile = new ExploreFile();
        //    string path = eFile.Path;
        //    if (eFile.IfExistFileInServer(modelName))
        //    {
        //        eFile.CreateTempleteFileFromServer(modelName);
        //        //载入格式和数据
        //        flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
        //    }
        //    else
        //    {
        //        MessageBox.Show("未找到模板格式文件【" + modelName + "】");
        //        return false;
        //    }
        //    return true;
        //}

        //private void FillFlex(WeekScheduleMaster billMaster)
        //{
        //    int detailStartRowNumber = 4;//7为模板中的行号
        //    int detailCount = billMaster.Details.Count;

        //    //插入明细行
        //    flexGrid1.InsertRow(detailStartRowNumber, detailCount);

        //    //设置单元格的边框，对齐方式
        //    FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
        //    range.Alignment = FlexCell.AlignmentEnum.CenterCenter;
        //    range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
        //    range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
        //    range.Mask = FlexCell.MaskEnum.Digital;
        //    CommonUtil.SetFlexGridPrintFace(this.flexGrid1);
        //    //主表数据
        //    flexGrid1.Cell(3, 2).Text = billMaster.ProjectName;
        //    flexGrid1.Cell(3, 3).Text = billMaster.CreateDate.Year.ToString() +"年"+ billMaster.CreateDate.Month.ToString() +"月";
        //    //flexGrid1.Cell(3, 4).Text = billMaster.Code;
        //    flexGrid1.Cell(3, 6).Text = billMaster.CreateDate.ToShortDateString();

        //    //填写明细数据
        //    for (int i = 0; i < detailCount; i++)
        //    {
        //        WeekScheduleDetail detail = (WeekScheduleDetail)billMaster.Details.ElementAt(i);
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Text = ClientUtil.ToString(i + 1);
        //        flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MainTaskContent;
        //        flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Text = detail.PlannedBeginDate.ToShortDateString();
        //        flexGrid1.Cell(detailStartRowNumber + i, 3).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.PlannedEndDate.ToShortDateString();
        //        flexGrid1.Cell(detailStartRowNumber + i, 4).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Text = detail.ForwardBillMasterOwner;
        //        flexGrid1.Cell(detailStartRowNumber + i, 5).Alignment = FlexCell.AlignmentEnum.LeftCenter;
        //        flexGrid1.Row(detailStartRowNumber + i).AutoFit();
        //    }
        //}
        #endregion


        private void InitFlexGrid(int rows)
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Column(0).Visible = false;

            flexGrid.Rows = rows;
            flexGrid.Cols = 24;//其中0列隐藏 1-19 为放置图片列 20-24为数据列

            flexGrid.SelectionMode = FlexCell.SelectionModeEnum.ByCell;
            flexGrid.ExtendLastCol = true;
            flexGrid.DisplayFocusRect = false;
            flexGrid.LockButton = true;
            flexGrid.ReadonlyFocusRect = FlexCell.FocusRectEnum.Solid;
            flexGrid.BorderStyle = FlexCell.BorderStyleEnum.FixedSingle;
            flexGrid.ScrollBars = FlexCell.ScrollBarsEnum.Both;
            flexGrid.BackColorBkg = SystemColors.Control;
            flexGrid.DefaultFont = new Font("Tahoma", 8);

            FlexCell.Range range;

            for (int i = startImageCol; i <= endImageCol; i++)
            {
                flexGrid.Column(i).TabStop = false;
                flexGrid.Column(i).Width = 20;
            }

            range = flexGrid.Range(0, startImageCol, 0, endImageCol);
            range.Merge();
            flexGrid.Cell(0, 1).Text = "任务名称";
            flexGrid.Cell(0, 1).Alignment = FlexCell.AlignmentEnum.CenterCenter;

            // 加载图片
            flexGrid.Images.Add(Resources.ImageExpend, imageExpand);
            flexGrid.Images.Add(Resources.ImageFold, imageCollapse);

            flexGrid.Cell(0, endImageCol + 1).Text = "计划开始时间";//20
            flexGrid.Cell(0, endImageCol + 2).Text = "计划结束时间";//21
            flexGrid.Cell(0, endImageCol + 3).Text = "计划工期";//23
            flexGrid.Cell(0, endImageCol + 4).Text = "备注";//27


            flexGrid.Column(endImageCol + 1).CellType = FlexCell.CellTypeEnum.Calendar;
            flexGrid.Column(endImageCol + 2).CellType = FlexCell.CellTypeEnum.Calendar;

            flexGrid.Column(endImageCol + 3).Mask = FlexCell.MaskEnum.Digital;

            // Refresh
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        private IList ViewToDetails()
        {
            IList list = new ArrayList();

            if (!string.IsNullOrEmpty(curBillMaster.Id))
            {
                for (int i = 1; i < flexGrid.Rows; i++)
                {
                    string detailId = flexGrid.Cell(i, 0).Tag;

                    if (detailId == null || detailId.Equals(""))
                        continue;

                    WeekScheduleDetail detail = null;
                    foreach (WeekScheduleDetail tempDetail in curBillMaster.Details)
                    {
                        if (detailId == tempDetail.Id)
                        {
                            detail = tempDetail;
                            break;
                        }

                    }
                    //计划开始时间
                    string PlannedBeginDateStr = flexGrid.Cell(i, endImageCol + 1).Text;
                    if (PlannedBeginDateStr != null && !PlannedBeginDateStr.Equals(""))
                    {
                        detail.PlannedBeginDate = DateTime.Parse(PlannedBeginDateStr);
                    }
                    else
                    {
                        detail.PlannedBeginDate = new DateTime(1900, 1, 1);
                    }
                    //计划结束时间
                    string PlannedEndDateStr = flexGrid.Cell(i, endImageCol + 2).Text;
                    if (PlannedEndDateStr != null && !PlannedEndDateStr.Equals(""))
                    {
                        detail.PlannedEndDate = DateTime.Parse(PlannedEndDateStr);
                    }
                    else
                    {
                        detail.PlannedEndDate = new DateTime(1900, 1, 1);
                    }
                    //计划工期
                    detail.PlannedDuration = flexGrid.Cell(i, endImageCol + 3).IntegerValue;

                    //计划说明
                    detail.Descript = flexGrid.Cell(i, endImageCol + 4).Text; ;

                    CurBillMaster.AddDetail(detail);

                }
            }
            return list;
        }

        void btnExportToMPP_Click(object sender, EventArgs e)
        {

            if (CurBillMaster == null)
            {
                MessageBox.Show("当前没有要导出的计划!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //OpenFileDialog ofg = new OpenFileDialog();
            SaveFileDialog sfd = new SaveFileDialog();
            //openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sfd.Filter = "项目 (*.MPP)|*.MPP";
            sfd.RestoreDirectory = true;
            sfd.FileName = CurBillMaster.PlanName + "_" + string.Format("{0:yyyy年MM月dd日HH点mm分}", DateTime.Now) + ".mpp";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                if (System.IO.File.Exists(fileName))
                {
                    IList list = model.ProductionManagementSrv.GetWeekChilds(CurBillMaster);
                    MSProjectUtil.UpdateProject(fileName, list, listDtlIds, this.Handle);
                }
                else
                {

                    IList list = model.ProductionManagementSrv.GetWeekChilds(CurBillMaster);//这是全部树
                    CreateMPP(fileName, list, listDtlIds, this.Handle);

                }
            }
        }

        private void GridFillFlex()
        {
            flexGrid.AutoRedraw = false;
            flexGrid.Rows = 1;
            flexGrid.Column(endImageCol + 3).Locked = true;

            detailHashtable.Clear();

            IList list = null;
            if (!string.IsNullOrEmpty(curBillMaster.Id))
            {
                list = model.ProductionManagementSrv.GetWeekChilds(curBillMaster);
            }

            if (list != null && list.Count > 0)
            {
                flexGrid.Rows = list.Count + 1;
                int rowIndex = 1;
                listDtlIds.Clear();
                Hashtable hs = new Hashtable();
                foreach (WeekScheduleDetail detail in list)
                {
                    hs.Add(detail.Id, detail);
                }
                InsertRow(list[0] as WeekScheduleDetail, list,  hs, ref rowIndex);
                //foreach (WeekScheduleDetail detail in list)
                //{
                //    listDtlIds.Add(detail.Id);
                //    flexGrid.Cell(rowIndex, 0).Tag = detail.Id;
                //    detailHashtable.Add(detail.Id, detail);

                //    flexGrid.Cell(rowIndex, detail.GWBSTree.Level).SetImage(imageCollapse);
                //    FlexCell.Range rangeTemp = flexGrid.Range(rowIndex, detail.GWBSTree.Level + 1, rowIndex, endImageCol);
                //    rangeTemp.Merge();
                //    flexGrid.Cell(rowIndex, detail.GWBSTree.Level + 1).Text = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName;

                //    //计划开始时间
                //    if (detail.PlannedBeginDate == (new DateTime(1900, 1, 1)))
                //    {
                //        flexGrid.Cell(rowIndex, endImageCol + 1).Text = ""; //"计划开始时间";//20
                //    }
                //    else
                //    {
                //        flexGrid.Cell(rowIndex, endImageCol + 1).Text = detail.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                //    }
                //    //计划结束时间
                //    if (detail.PlannedEndDate == (new DateTime(1900, 1, 1)))
                //    {
                //        flexGrid.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划结束时间";//21
                //    }
                //    else
                //    {
                //        flexGrid.Cell(rowIndex, endImageCol + 2).Text = detail.PlannedEndDate.ToShortDateString();
                //    }

                //    //计划工期
                //    if (detail.PlannedBeginDate != (new DateTime(1900, 1, 1)) && detail.PlannedEndDate != (new DateTime(1900, 1, 1)) && detail.PlannedBeginDate <= detail.PlannedEndDate)
                //    {
                //        flexGrid.Cell(rowIndex, endImageCol + 3).Text = ((detail.PlannedEndDate - detail.PlannedBeginDate).Days + 1).ToString();
                //    }
                //    else
                //    {
                //        flexGrid.Cell(rowIndex, endImageCol + 3).Text = "";
                //    }

                //    //备注
                //    flexGrid.Cell(rowIndex, endImageCol + 4).Text = detail.Descript; //"备注"

                //    rowIndex = rowIndex + 1;
                //}
            }
            //1-19列的背景色
            FlexCell.Range range = flexGrid.Range(1, startImageCol, flexGrid.Rows - 1, endImageCol);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }
            //设置计划实际信息列的背景色
            range = flexGrid.Range(1, endImageCol + 6, flexGrid.Rows - 1, endImageCol + 8);
            if (range != null)
            {
                range.Alignment = FlexCell.AlignmentEnum.LeftCenter;
                range.BackColor = SystemColors.Control;
            }

            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }
        public void InsertRow(WeekScheduleDetail oNode, IList list, Hashtable hs, ref int rowIndex)
        {
            if (oNode != null)
            {
                listDtlIds.Add(oNode.Id);
                flexGrid.Cell(rowIndex, 0).Tag = oNode.Id;
                detailHashtable.Add(oNode.Id, oNode);

                flexGrid.Cell(rowIndex, oNode.GWBSTree.Level).SetImage(imageCollapse);
                FlexCell.Range rangeTemp = flexGrid.Range(rowIndex, oNode.GWBSTree.Level + 1, rowIndex, endImageCol);
                rangeTemp.Merge();
                flexGrid.Cell(rowIndex, oNode.GWBSTree.Level + 1).Text = (oNode.GWBSTreeName == null) ? "进度计划" : oNode.GWBSTreeName;

                //计划开始时间
                if (oNode.PlannedBeginDate == (new DateTime(1900, 1, 1)))
                {
                    flexGrid.Cell(rowIndex, endImageCol + 1).Text = ""; //"计划开始时间";//20
                }
                else
                {
                    flexGrid.Cell(rowIndex, endImageCol + 1).Text = oNode.PlannedBeginDate.ToShortDateString(); //"计划开始时间";//20
                }
                //计划结束时间
                if (oNode.PlannedEndDate == (new DateTime(1900, 1, 1)))
                {
                    flexGrid.Cell(rowIndex, endImageCol + 2).Text = ""; //"计划结束时间";//21
                }
                else
                {
                    flexGrid.Cell(rowIndex, endImageCol + 2).Text = oNode.PlannedEndDate.ToShortDateString();
                }

                //计划工期
                if (oNode.PlannedBeginDate != (new DateTime(1900, 1, 1)) && oNode.PlannedEndDate != (new DateTime(1900, 1, 1)) && oNode.PlannedBeginDate <= oNode.PlannedEndDate)
                {
                    flexGrid.Cell(rowIndex, endImageCol + 3).Text = ((oNode.PlannedEndDate - oNode.PlannedBeginDate).Days + 1).ToString();
                }
                else
                {
                    flexGrid.Cell(rowIndex, endImageCol + 3).Text = "";
                }

                //备注
                flexGrid.Cell(rowIndex, endImageCol + 4).Text = oNode.Descript; //"备注"

                rowIndex = rowIndex + 1;


                var queryChilds1 = from d in curBillMaster.Details
                                  where d.Id != oNode.Id &&  d.GWBSTreeSysCode.IndexOf(oNode .GWBSTreeSysCode) >-1
                                  select d;
                if (queryChilds1.Count() > 0)
                {
                    int i = 2;
                     
                    while (true)
                    {
                        var  queryChilds = from d in curBillMaster.Details
                                      where d.Id != oNode.Id && IsChild(d.GWBSTreeSysCode, oNode.GWBSTreeSysCode, i)
                                      select d;
                        if (queryChilds.Count() > 0)
                        {
                            foreach (WeekScheduleDetail oChild in queryChilds)
                            {
                                if (hs.ContainsKey(oChild.Id))
                                {
                                    InsertRow(hs[oChild.Id] as WeekScheduleDetail, list, hs, ref rowIndex);
                                }
                            }
                            break;
                        }
                        else
                        {
                            i++;
                        }
                    }
                   
                }
            }
        }
        public bool IsChild(string sChildSysCode, string sParentSysCode,int i)
        {
            bool bFlag = false;
            if (sChildSysCode.IndexOf(sParentSysCode) > -1)
            {
                string s = sChildSysCode.Replace(sParentSysCode, "");
                if (s.Split('.').Length == i)
                {
                    bFlag = true;
                }
            }
            return bFlag;
        }
        public static void CreateMPP(string fileName, IList scheduleList, List<string> lstID, IntPtr Handle)
        {

            ExportTask(scheduleList, lstID, fileName, Handle);

        }

        [DllImport("shell32.dll")]
        public extern static IntPtr ShellExecute(IntPtr hwnd,
                                                 string lpOperation,
                                                 string lpFile,
                                                 string lpParameters,
                                                 string lpDirectory,
                                                 int nShowCmd
                                                );

        public static void ExportTask(IList scheduleList, List<string> lstID, string sFilePath, IntPtr Handle)
        {
            object missing = Type.Missing;
            string sXmlFilePath = @"d:\\temp\\1.xml";
            string sID = string.Empty;
            string Level = string.Empty;
            int iCount = 0;
            string sNewTag = "(最新)";
            try
            {
                //获取xml路径
                if (sFilePath.LastIndexOf(".") > 0)
                {
                    sXmlFilePath = sFilePath.Substring(0, sFilePath.LastIndexOf(".")) + ".xml";
                }
                else
                {
                    sXmlFilePath = sFilePath + ".xml";
                }
                if (File.Exists(sXmlFilePath))//删除原有的xml文件
                {
                    File.Delete(sXmlFilePath);
                }
                if (scheduleList != null && scheduleList.Count > 0)
                {
                    #region 创建task集合
                    //string sMsg = string.Empty;
                    //for (int i = 0; i < 5; i++)
                    //{
                    //    sMsg += "1111111111111111111111111111111111111111";
                    //}
                    Project2007.ProjectTask[] arrTask = new Project2007.ProjectTask[scheduleList.Count];
                    string sParentID = string.Empty;
                    foreach (WeekScheduleDetail detail in scheduleList)
                    {
                        if (lstID.Contains(detail.Id))
                        {
                            Project2007.ProjectTask task = new Project2007.ProjectTask();
                            task.Name = (detail.GWBSTreeName == null) ? "进度计划" : detail.GWBSTreeName + sNewTag;
                            task.Start111 = detail.PlannedBeginDate;
                            task.Finish111 = detail.PlannedEndDate;
                            task.ID = detail.Id;
                            task.OutlineLevel = detail.GWBSTree.Level.ToString();
                            sParentID = detail.GWBSTree.ParentNode == null ? "" : detail.GWBSTree.ParentNode.Id;
                            task.Notes = sParentID + "|" + string.Format(task.Start111.ToString("G")) + "|" + string.Format(task.Finish111.ToString("G")); ;
                            // task.Notes = sParentID + "|" + "" + "|" + ""; 
                            task.Contact = detail.GWBSTreeName == null ? "root" : detail.Id;
                            task.WBS = detail.GWBSTreeName == null ? "1" : "2";
                            //task.UID = detail.Id;
                            arrTask[iCount++] = task;

                        }
                    }
                    #endregion
                    #region 创建一个project2007的类
                    Project2007.Project p = new Project2007.Project();
                    p.Tasks = arrTask;
                    XmlSerializer xs = new XmlSerializer(typeof(Project2007.Project));

                    Stream stream = new FileStream(sXmlFilePath, FileMode.OpenOrCreate);
                    xs.Serialize(stream, p);//序列化
                    stream.Close();
                    #endregion
                    #region 创建project
                    Microsoft.Office.Interop.MSProject.Application appProject = null;
                    //try
                    //{
                    //    appProject = new Microsoft.Office.Interop.MSProject.Application();
                    //}
                    //catch {
                    //    ShellExecute(Handle, "open", @"C:\Program Files\Microsoft Office\Office12\WINPROJ.EXE", null, null, (int)ShowWindowCommands.SW_SHOW);

                    //    Thread.Sleep(3000);
                    //    appProject = new Microsoft.Office.Interop.MSProject.Application();
                    //}
                    ShellExecute(Handle, "open", ProjectPath.GetPath(), null, null, (int)Application.Business.Erp.SupplyChain.Client.Util.MSProjectUtil.ShowWindowCommands.SW_SHOW);

                    Thread.Sleep(3000);
                    appProject = new Microsoft.Office.Interop.MSProject.Application();
                    appProject.Visible = true;
                    appProject.FileNew(missing, missing, missing, false);//新建一个project
                    appProject.FileOpen(sXmlFilePath, true, Microsoft.Office.Interop.MSProject.PjMergeType.pjAppend, missing, missing, missing, missing, missing, missing, "MSProject.mpp", missing, Microsoft.Office.Interop.MSProject.PjPoolOpen.pjPoolReadWrite, missing, missing, false, missing);//建数据加载到文件中

                    ///
                    ///  appProject.FileSaveAs(@"C:\Documents and Settings\Administrator\桌面\db\xml1.xml", Microsoft.Office.Interop.MSProject.PjFileFormat.pjXLS, missing, missing, missing, missing, missing, missing, missing, "MSProject.xml", missing, missing, missing, missing, missing, missing, missing, missing, missing);
                    // appProject.FileSaveAs(@"C:\Documents and Settings\Administrator\桌面\db\xml2.xml", Microsoft.Office.Interop.MSProject.PjFileFormat.pjTXT , missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
                    ///
                    appProject.FileSaveAs(sFilePath, Microsoft.Office.Interop.MSProject.PjFileFormat.pjMPP, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);//保存
                    #endregion
                    if (File.Exists(sXmlFilePath))//删除原有的xml文件
                    {
                        File.Delete(sXmlFilePath);
                    }
                }

            }
            catch (System.Exception ex)
            {
            }
        }

        void flexGrid_Click(object Sender, EventArgs e)
        {
            flexGrid.AutoRedraw = false;

            if (flexGrid.ActiveCell.ImageKey == imageExpand)
            {
                flexGrid.ActiveCell.SetImage(imageCollapse);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, true);
                }
            }
            else if (flexGrid.ActiveCell.ImageKey == imageCollapse)
            {
                flexGrid.ActiveCell.SetImage(imageExpand);
                int activeRowIndex = flexGrid.ActiveCell.Row;

                string detailId = flexGrid.Cell(activeRowIndex, 0).Tag;
                int childs = CountAllChildNodes(detailId);
                if (childs > 0)
                {
                    SetRowVisible(activeRowIndex + 1, activeRowIndex + childs, false);
                }
            }

            flexGrid.AutoRedraw = true;
        }

        private int CountAllChildNodes(string dtlId)
        {
            var query = from d in curBillMaster.Details
                        where d.Id == dtlId
                        select d;
            WeekScheduleDetail thePlanDtl = query.ElementAt(0);

            var queryChilds = from d in curBillMaster.Details
                              where d.Id != dtlId && d.GWBSTreeSysCode.IndexOf(thePlanDtl.GWBSTreeSysCode) > -1
                              select d;

            return queryChilds.Count();
        }

        private void SetRowVisible(int row1, int row2, bool value)
        {
            flexGrid.AutoRedraw = false;
            for (int i = row1; i <= row2; i++)
            {
                flexGrid.Row(i).Visible = value;
                if (value)
                {
                    for (int j = startImageCol; j <= endImageCol; j++)
                    {
                        if (flexGrid.Cell(i, j).ImageKey != null && !flexGrid.Cell(i, j).ImageKey.Equals(""))
                        {
                            flexGrid.Cell(i, j).SetImage(imageCollapse);
                            break;
                        }
                    }
                }
            }
            flexGrid.AutoRedraw = true;
            flexGrid.Refresh();
        }

        public void flexGrid_LeaveCell(object sender, Grid.LeaveCellEventArgs e)
        {
            if (e.Row > 0 && e.Col > 19 && e.Col < 24)
            {
                string sStartTime = flexGrid.Cell(e.Row, endImageCol + 1).Text;
                string sFinishTime = flexGrid.Cell(e.Row, endImageCol + 2).Text;
                if (!string.IsNullOrEmpty(sStartTime) && !string.IsNullOrEmpty(sFinishTime))
                {
                    try
                    {
                        DateTime StartTime = DateTime.Parse(sStartTime);
                        DateTime FinnishTime = DateTime.Parse(sFinishTime);
                        if (StartTime <= FinnishTime)
                        {
                            flexGrid.Cell(e.Row, endImageCol + 3).Text = ((FinnishTime - StartTime).Days + 1).ToString();
                        }
                        else
                        {
                            //flexGrid.Cell(e.Row, endImageCol + 3).Text = string.Empty;
                            MessageBox.Show("计划开始时间大于计划结束时间！");
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
