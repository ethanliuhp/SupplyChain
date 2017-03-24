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
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekScheduleSummaryBak : TMasterDetailView
    {
        private MProductionMng model = new MProductionMng();
        private WeekScheduleMaster curBillMaster;

        /// <summary>
        /// 当前单据
        /// </summary>
        public WeekScheduleMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VWeekScheduleSummaryBak()
        {
            InitializeComponent();
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            colPlanConformity.DataSource = Enum.GetNames(typeof(EnumWeekSchedulePlanConformity));

            dgDetail.ContextMenuStrip = cmsDg;
            dgDetail.SelectionMode = DataGridViewSelectionMode.CellSelect;

            DateTime dt = model.ProductionManagementSrv.GetServerTime();
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

        }

        private void InitEvent()
        {
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            btnWeekSchedule.Click += new EventHandler(btnWeekSchedule_Click);

            dtpDateBegin.ValueChanged += new EventHandler(dtpDateBegin_ValueChanged);
            dtpDateEnd.ValueChanged += new EventHandler(dtpDateEnd_ValueChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);

            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            //右键复制菜单
            //tsmiCopy.Click += new EventHandler(tsmiCopy_Click);
        }

        void tsmiCopy_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = dgDetail.CurrentRow;
            if (dr == null || dr.IsNewRow) return;

            int i = dgDetail.Rows.Add();

            dgDetail[colGWBSTree.Name, i].Value = dr.Cells[colGWBSTree.Name].Value;
            dgDetail[colGWBSTree.Name, i].Tag = dr.Cells[colGWBSTree.Name].Tag;
            dgDetail[colForwardBillDtlIdOfWeek.Name, i].Value = dr.Cells[colForwardBillDtlIdOfWeek.Name].Value;

            dgDetail[colMonday.Name, i].Value = dr.Cells[colMonday.Name].Value;
            dgDetail[colMonday.Name, i].Tag = dr.Cells[colMonday.Name].Tag;

            dgDetail[colTuesday.Name, i].Value = dr.Cells[colTuesday.Name].Value;
            dgDetail[colTuesday.Name, i].Tag = dr.Cells[colTuesday.Name].Tag;
            dgDetail[colWednesday.Name, i].Value = dr.Cells[colWednesday.Name].Value;
            dgDetail[colWednesday.Name, i].Tag = dr.Cells[colWednesday.Name].Tag;

            dgDetail[colThursday.Name, i].Value = dr.Cells[colThursday.Name].Value;
            dgDetail[colThursday.Name, i].Tag = dr.Cells[colThursday.Name].Tag;

            dgDetail[colFriday.Name, i].Value = dr.Cells[colFriday.Name].Value;
            dgDetail[colFriday.Name, i].Tag = dr.Cells[colFriday.Name].Tag;

            dgDetail[colSaturday.Name, i].Value = dr.Cells[colSaturday.Name].Value;
            dgDetail[colSaturday.Name, i].Tag = dr.Cells[colSaturday.Name].Tag;

            dgDetail[colSunday.Name, i].Value = dr.Cells[colSunday.Name].Value;
            dgDetail[colSunday.Name, i].Tag = dr.Cells[colSunday.Name].Tag;
            dgDetail[colPlannedDuration.Name, i].Value = dr.Cells[colPlannedDuration.Name].Value;

        }


        void dgDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //自动计算计划工期
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            DataGridViewCell cell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (cell.OwningColumn.Name == colMonday.Name
                || cell.OwningColumn.Name == colTuesday.Name
                || cell.OwningColumn.Name == colWednesday.Name
                || cell.OwningColumn.Name == colThursday.Name
                || cell.OwningColumn.Name == colFriday.Name
                || cell.OwningColumn.Name == colSaturday.Name
                || cell.OwningColumn.Name == colSunday.Name
                )
            {
                int startColumnIndex = dgDetail.Rows[e.RowIndex].Cells[colMonday.Name].ColumnIndex;
                int endColumnIndex = dgDetail.Rows[e.RowIndex].Cells[colSunday.Name].ColumnIndex;
                int a = 0, b = 0;
                for (int i = startColumnIndex; i <= endColumnIndex; i++)
                {
                    object planStartValue = dgDetail.Rows[e.RowIndex].Cells[i].Value;
                    if (planStartValue != null && (bool)planStartValue)
                    {
                        a = i;
                        break;
                    }
                }

                for (int j = endColumnIndex; j >= startColumnIndex; j--)
                {
                    object planEndValue = dgDetail.Rows[e.RowIndex].Cells[j].Value;
                    if (planEndValue != null && (bool)planEndValue)
                    {
                        b = j;
                        break;
                    }
                }

                if (a == 0 && b == 0)
                {
                    dgDetail.Rows[e.RowIndex].Cells[colPlannedDuration.Name].Value = 0;
                }
                else
                {
                    dgDetail.Rows[e.RowIndex].Cells[colPlannedDuration.Name].Value = b - a + 1;
                }

                //设置选中单元格背景色
                for (int k = startColumnIndex; k <= endColumnIndex; k++)
                {
                    DataGridViewCellStyle cellStyle = dgDetail.Rows[e.RowIndex].Cells[k].Style;
                    if (k >= a && k <= b)
                    {
                        cellStyle.BackColor = ColorTranslator.FromHtml("#D7E8FE");// Color.Red;
                    }
                    else
                    {
                        cellStyle.BackColor = Color.White;
                    }
                    dgDetail.Rows[e.RowIndex].Cells[k].Style = cellStyle;
                }
            }
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colMonday.Name
                || dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colTuesday.Name
                || dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colWednesday.Name
                || dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colThursday.Name
                || dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colFriday.Name
                || dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSaturday.Name
                || dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSunday.Name
                )
            {
                dgDetail.EndEdit();
            }
        }

        void dtpDateEnd_ValueChanged(object sender, EventArgs e)
        {
            //SetDatetime(dtpDateBegin.Value);
            SetWeekPlanDatetime(dtpDateBegin.Value.AddDays(-6));
        }

        void dtpDateBegin_ValueChanged(object sender, EventArgs e)
        {
            //SetDatetime(dtpDateBegin.Value);
            SetWeekPlanDatetime(dtpDateBegin.Value);
        }

        /// <summary>
        /// 根据输入日期设置计划日期的起止时间为星期一到日期日
        /// </summary>
        /// <param name="dt"></param>
        private void SetDatetime(DateTime dt)
        {
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

            DateTime planDateBegin = dtpDateBegin.Value.Date;
            foreach (DataGridViewRow var in dgDetail.Rows)
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
            foreach (DataGridViewRow var in dgDetail.Rows)
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

        void btnWeekSchedule_Click(object sender, EventArgs e)
        {
            VWeekScheduleSelector vss = new VWeekScheduleSelector();
            vss.DefaultBeginDate = dtpDateBegin.Value;
            vss.DefaultEndDate = dtpDateEnd.Value;
            vss.ShowDialog();
            IList list = vss.Result;
            if (list == null || list.Count == 0)
                return;


            DateTime dt = dtpDateBegin.Value.Date;
            foreach (WeekScheduleDetail detail in list)
            {
                if (HasAdded(detail))
                    continue;
                int rowIndex = dgDetail.Rows.Add();
                dgDetail[colGWBSTree.Name, rowIndex].Tag = detail.GWBSTree;
                dgDetail[colGWBSTree.Name, rowIndex].Value = detail.GWBSTreeName;
                dgDetail[colGWBSTree.Name, rowIndex].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), detail.GWBSTree);

                dgDetail[colMainTaskContent.Name, rowIndex].Value = detail.MainTaskContent;
                dgDetail[colPlannedDuration.Name, rowIndex].Value = detail.PlannedDuration;
                dgDetail[colPlannedWrokload.Name, rowIndex].Value = detail.PlannedWrokload;

                dgDetail[colOBSService.Name, rowIndex].Value = detail.SupplierName;
                dgDetail[colOBSService.Name, rowIndex].Tag = detail.SubContractProject;

                dgDetail[colPBSTree.Name, rowIndex].Tag = detail.PBSTree;
                dgDetail[colPBSTree.Name, rowIndex].Value = detail.PBSTreeName;

                dgDetail[colForwardBillDtlIdOfWeek.Name, rowIndex].Value = detail.Id;

                //显示周一到周日
                SetPlanView(dgDetail.Rows[rowIndex], detail);

                SetPlanTag(dgDetail.Rows[rowIndex]);
            }

        }

        /// <summary>
        /// 判断滚动进度计划明细是否已经添加
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="enumExecScheduleType"></param>
        /// <returns></returns>
        private bool HasAdded(WeekScheduleDetail detail)
        {
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                string forwardBillDtlId = dr.Cells[colForwardBillDtlIdOfWeek.Name].Value + "";
                if (detail.Id == forwardBillDtlId)
                {
                    return true;
                }
            }
            return false;
        }

        void tsmiDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgDetail.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgDetail.Rows.Remove(dr);
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
                    this.dgDetail.EditMode = DataGridViewEditMode.EditOnEnter;
                    foreach (DataGridViewRow dr in dgDetail.Rows)
                    {
                        SetBackColor(dr);
                    }
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    foreach (DataGridViewRow dr in dgDetail.Rows)
                    {
                        SetBackColor(dr);
                    }
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
            }

            //永久锁定
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtSumPlanQuantity, txtProject, txtSumActualQuantity };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colGWBSTree.Name, colActualBeginDate.Name, colActualEndDate.Name, colActualDuration.Name, colActualWorkload.Name, colPlanConformity.Name, colTaskCompletedPercent.Name, colOBSService.Name };
            dgDetail.SetColumnsReadOnly(lockCols);

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

                this.curBillMaster = new WeekScheduleMaster();
                //curBillMaster.CreatePerson = ConstObject.LoginPersonInfo;
                //curBillMaster.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.CreateDate = ConstObject.LoginDate;
                //curBillMaster.CreateYear = ConstObject.LoginDate.Year;
                //curBillMaster.CreateMonth = ConstObject.LoginDate.Month;
                //curBillMaster.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                //curBillMaster.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                //curBillMaster.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                curBillMaster.HandlePerson = ConstObject.LoginPersonInfo;
                curBillMaster.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                curBillMaster.SummaryStatus = EnumSummaryStatus.汇总生成;
                curBillMaster.ExecScheduleType = EnumExecScheduleType.周进度计划;

                //制单人
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo != null)
                {
                    txtProject.Tag = projectInfo;
                    txtProject.Text = projectInfo.Name;
                    curBillMaster.ProjectId = projectInfo.Id;
                    curBillMaster.ProjectName = projectInfo.Name;
                }
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
                if (!ViewToModel()) return false;
                curBillMaster = model.ProductionManagementSrv.SaveSummaryWeekScheduleMaster(curBillMaster);

                //插入日志
                //MStockIn.InsertLogData(curBillMaster.Id, "保存", curBillMaster.Code, curBillMaster.CreatePerson.Name, "料具租赁合同","");
                txtCode.Text = curBillMaster.Code;
                //更新Caption
                this.ViewCaption = ViewName + "-" + txtCode.Text;
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
            if (MessageBox.Show("确定要提交当前单据吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                try
                {
                    if (!ViewToModel()) return false;
                    curBillMaster.DocState = DocumentState.InExecute;
                    curBillMaster = model.ProductionManagementSrv.SaveSummaryWeekScheduleMaster(curBillMaster);

                    //插入日志
                    //MStockIn.InsertLogData(curBillMaster.Id, "保存", curBillMaster.Code, curBillMaster.CreatePerson.Name, "料具租赁合同","");
                    txtCode.Text = curBillMaster.Code;
                    //更新Caption
                    this.ViewCaption = ViewName + "-" + txtCode.Text;
                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("数据保存错误：" + ExceptionUtil.ExceptionMessage(e));
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
                    if (!model.ProductionManagementSrv.DeleteSummaryWeekScheduleMaster(curBillMaster)) return false;
                    ClearView();
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
            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            dgDetail.EndEdit();
            //周计划数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;
                if ((dr.Cells[colMonday.Name].Value == null || !(bool)dr.Cells[colMonday.Name].Value) &&
                    (dr.Cells[colTuesday.Name].Value == null || !(bool)dr.Cells[colTuesday.Name].Value) &&
                    (dr.Cells[colWednesday.Name].Value == null || !(bool)dr.Cells[colWednesday.Name].Value) &&
                    (dr.Cells[colThursday.Name].Value == null || !(bool)dr.Cells[colThursday.Name].Value) &&
                    (dr.Cells[colFriday.Name].Value == null || !(bool)dr.Cells[colFriday.Name].Value) &&
                    (dr.Cells[colSaturday.Name].Value == null || !(bool)dr.Cells[colSaturday.Name].Value) &&
                    (dr.Cells[colSunday.Name].Value == null || !(bool)dr.Cells[colSunday.Name].Value))
                {
                    MessageBox.Show("请设置明细计划日期。");
                    dgDetail.CurrentCell = dr.Cells[colMonday.Name];
                    return false;
                }
                object plannedDuration = dr.Cells[colPlannedDuration.Name].Value;
                if (plannedDuration == null || plannedDuration.ToString() == "" || ClientUtil.ToInt(plannedDuration) <= 0)
                {
                    MessageBox.Show("计划工期不能为空或小于等于0！");
                    dgDetail.CurrentCell = dr.Cells[colPlannedDuration.Name];
                    return false;
                }

                object plannedWorkload = dr.Cells[colPlannedWrokload.Name].Value;
                if (plannedWorkload == null || plannedWorkload.ToString() == "" || ClientUtil.TransToDecimal(plannedWorkload) <= 0)
                {
                    MessageBox.Show("计划工程量不能为空或小于等于0！！");
                    dgDetail.CurrentCell = dr.Cells[colPlannedWrokload.Name];
                    return false;
                }
                dgDetail.Update();
            }
            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.PlannedBeginDate = dtpDateBegin.Value.Date;
                curBillMaster.PlannedEndDate = dtpDateEnd.Value.Date;
                curBillMaster.Descript = txtRemark.Text;
                curBillMaster.CompletionAnalysis = this.txtCompletionAnalysis.Text;

                //curBillMaster.SumPlannedWorkload = ClientUtil.ToDecimal(txtSumPlanQuantity.Text);
                //curBillMaster.SumActualWorkload = ClientUtil.ToDecimal(txtSumActualQuantity.Text);
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    WeekScheduleDetail curBillDtl = new WeekScheduleDetail();
                    if (var.Tag != null)
                    {
                        curBillDtl = var.Tag as WeekScheduleDetail;
                        if (curBillDtl.Id == null)
                        {
                            curBillMaster.Details.Remove(curBillDtl);
                        }
                    }
                    curBillDtl.SummaryStatus = EnumSummaryStatus.汇总生成;
                    //周计划
                    curBillDtl.Descript = ClientUtil.ToString(var.Cells[colDescript.Name].Value);
                    if (var.Cells[colGWBSTree.Name].Tag != null)
                    {
                        curBillDtl.GWBSTree = var.Cells[colGWBSTree.Name].Tag as GWBSTree;
                        curBillDtl.GWBSTreeName = ClientUtil.ToString(var.Cells[colGWBSTree.Name].Value);
                    }
                    curBillDtl.MainTaskContent = ClientUtil.ToString(var.Cells[colMainTaskContent.Name].Value);
                    curBillDtl.PlannedBeginDate = CalWeekPlanBeginDate(var);
                    curBillDtl.PlannedEndDate = CalWeekPlanEndDate(var);
                    curBillDtl.PlannedDuration = ClientUtil.ToInt(var.Cells[colPlannedDuration.Name].Value);
                    curBillDtl.PlannedWrokload = ClientUtil.ToDecimal(var.Cells[colPlannedWrokload.Name].Value);
                    curBillDtl.ActualBeginDate = ClientUtil.ToDateTime(var.Cells[colActualBeginDate.Name].Value);
                    curBillDtl.ActualEndDate = ClientUtil.ToDateTime(var.Cells[colActualEndDate.Name].Value);
                    curBillDtl.ActualDuration = ClientUtil.ToInt(var.Cells[colActualDuration.Name].Value);
                    curBillDtl.ActualWorklaod = ClientUtil.ToDecimal(var.Cells[colActualWorkload.Name].Value);
                    curBillDtl.TaskCompletedPercent = ClientUtil.ToDecimal(var.Cells[colTaskCompletedPercent.Name].Value);
                    curBillDtl.PlanConformity = ClientUtil.ToString(var.Cells[colPlanConformity.Name].Value);
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
                dtpDateBegin.Value = curBillMaster.PlannedBeginDate;
                dtpDateEnd.Value = curBillMaster.PlannedEndDate;

                txtCode.Text = curBillMaster.Code;
                txtProject.Text = curBillMaster.ProjectName;
                txtRemark.Text = curBillMaster.Descript;

                txtCompletionAnalysis.Text = curBillMaster.CompletionAnalysis;

                txtCreatePerson.Text = curBillMaster.CreatePersonName;
                txtCreateDate.Text = curBillMaster.CreateDate.ToShortDateString();
                txtProject.Text = curBillMaster.ProjectName;
                txtSumPlanQuantity.Text = curBillMaster.SumPlannedWorkload.ToString("######.####");
                txtSumActualQuantity.Text = curBillMaster.SumActualWorkload.ToString("######.####");

                this.dgDetail.Rows.Clear();
                foreach (WeekScheduleDetail var in curBillMaster.Details)
                {
                    //周计划
                    int i = this.dgDetail.Rows.Add();
                    dgDetail.Rows[i].Cells[colGWBSTree.Name].Tag = var.GWBSTree;
                    dgDetail.Rows[i].Cells[colGWBSTree.Name].Value = var.GWBSTreeName;
                    dgDetail.Rows[i].Cells[colMainTaskContent.Name].Value = var.MainTaskContent;
                    //显示周一到周日
                    SetPlanView(dgDetail.Rows[i], var);
                    SetPlanTag(dgDetail.Rows[i]);
                    //SetBackColor(dgDetail.Rows[i]);
                    //dgDetail.Rows[i].Cells[colPlannedBeginDate.Name].Value=var.PlannedBeginDate;
                    //dgDetail.Rows[i].Cells[colPlannedEndDate.Name].Value=var.PlannedEndDate;
                    dgDetail.Rows[i].Cells[colPlannedDuration.Name].Value = var.PlannedDuration;
                    dgDetail.Rows[i].Cells[colPlannedWrokload.Name].Value = var.PlannedWrokload;
                    dgDetail.Rows[i].Cells[colActualBeginDate.Name].Value = var.ActualBeginDate;
                    dgDetail.Rows[i].Cells[colActualEndDate.Name].Value = var.ActualEndDate;
                    dgDetail.Rows[i].Cells[colActualDuration.Name].Value = var.ActualDuration;
                    dgDetail.Rows[i].Cells[colActualWorkload.Name].Value = var.ActualWorklaod;
                    dgDetail.Rows[i].Cells[colTaskCompletedPercent.Name].Value = var.TaskCompletedPercent;
                    dgDetail.Rows[i].Cells[colPlanConformity.Name].Value = var.PlanConformity;
                    dgDetail.Rows[i].Cells[colDescript.Name].Value = var.Descript;
                    dgDetail.Rows[i].Cells[colOBSService.Name].Value = var.SupplierName;
                    dgDetail.Rows[i].Cells[colOBSService.Name].Tag = var.SubContractProject;

                    dgDetail.Rows[i].Cells[colPBSTree.Name].Tag = var.PBSTree;
                    dgDetail.Rows[i].Cells[colPBSTree.Name].Value = var.PBSTreeName;
                    dgDetail.Rows[i].Cells[colForwardBillDtlIdOfWeek.Name].Value = var.ForwardBillDtlId;

                    this.dgDetail.Rows[i].Tag = var;
                }
                return true;
            }
            catch (Exception e)
            {
                throw e;
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
                    cellStyle.BackColor = ColorTranslator.FromHtml("#D7E8FE"); //Color.Red;
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
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colPlannedWrokload.Name || colName == colActualWorkload.Name)
            {
                object pWorkload = dgDetail[e.ColumnIndex, e.RowIndex].Value;
                if (pWorkload == null) pWorkload = "";
                if (!CommonMethod.VeryValid(pWorkload.ToString()))
                {
                    MessageBox.Show("请输入数字！");
                    return;
                }

                object aWorkLoad = dgDetail[e.ColumnIndex, e.RowIndex].Value;
                if (aWorkLoad == null) aWorkLoad = "";
                if (!CommonMethod.VeryValid(aWorkLoad.ToString()))
                {
                    MessageBox.Show("请输入数字！");
                    return;
                }

                decimal sumpWorkload = 0M, sumaWorkload = 0M;
                foreach (DataGridViewRow dr in dgDetail.Rows)
                {
                    sumpWorkload += ClientUtil.ToDecimal(dr.Cells[colPlannedWrokload.Name].Value);
                    sumaWorkload += ClientUtil.ToDecimal(dr.Cells[colActualWorkload.Name].Value);
                }
                txtSumPlanQuantity.Text = sumpWorkload.ToString("###########.####");
                txtSumActualQuantity.Text = sumaWorkload.ToString("###########.####");
            }
        }

        #region 打印处理
        /// <summary>
        /// 打印预览
        /// </summary>
        /// <returns></returns>
        public override bool Preview()
        {
            //if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
            //FillFlex(curBillMaster);
            //flexGrid1.PrintPreview(true,true,true,0,0,0,0,0);
            return true;
        }

        public override bool Print()
        {
            //if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
            //FillFlex(curBillMaster);
            //flexGrid1.Print();
            return true;
        }

        public override bool Export()
        {
            //if (LoadTempleteFile(@"料具租赁管理\料具租赁合同打印.flx") == false) return false;
            //FillFlex(curBillMaster);
            //flexGrid1.ExportToExcel("料具租赁合同【" + curBillMaster.Code + "】", false, false, true);
            return true;
        }

        private bool LoadTempleteFile(string modelName)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(modelName))
            {
                eFile.CreateTempleteFileFromServer(modelName);
                //载入格式和数据
                flexGrid1.OpenFile(path + "\\" + modelName);//载入格式
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + modelName + "】");
                return false;
            }
            return true;
        }

        private void FillFlex(WeekScheduleMaster billMaster)
        {
            //int detailStartRowNumber = 7;//7为模板中的行号
            //int detailCount = billMaster.Details.Count;

            ////插入明细行
            //flexGrid1.InsertRow(detailStartRowNumber, detailCount);

            ////设置单元格的边框，对齐方式
            //FlexCell.Range range = flexGrid1.Range(detailStartRowNumber, 1, detailStartRowNumber + detailCount - 1, flexGrid1.Cols - 1);
            //range.Alignment = FlexCell.AlignmentEnum.RightCenter;
            //range.set_Borders(FlexCell.EdgeEnum.Inside, FlexCell.LineStyleEnum.Thin);
            //range.set_Borders(FlexCell.EdgeEnum.Outside, FlexCell.LineStyleEnum.Thin);
            //range.Mask = FlexCell.MaskEnum.Digital;

            ////主表数据

            //flexGrid1.Cell(2, 1).Text = "使用单位：";
            //flexGrid1.Cell(2, 4).Text = "登记时间：" + DateTime.Now.ToShortDateString();
            //flexGrid1.Cell(2, 7).Text = "制单编号：" + billMaster.Code;
            //flexGrid1.Cell(4, 2).Text = billMaster.OriginalContractNo;
            //flexGrid1.Cell(4, 5).Text = "";//合同名称
            //flexGrid1.Cell(4, 7).Text = "";//材料分类
            //flexGrid1.Cell(5, 2).Text = "";//租赁单位
            //flexGrid1.Cell(5, 2).WrapText = true;
            //flexGrid1.Cell(5, 5).Text = "";//承租单位
            //flexGrid1.Row(5).AutoFit();
            //flexGrid1.Cell(5, 7).Text = billMaster.RealOperationDate.ToShortDateString();//签订日期

            //FlexCell.PageSetup pageSetup = flexGrid1.PageSetup;
            //pageSetup.LeftFooter = "   制单人：" + billMaster.CreatePersonName;
            //pageSetup.RightFooter = "第 &P 页/共 &N 页      ";

            //System.Drawing.Printing.PaperSize paperSize = new System.Drawing.Printing.PaperSize("自定义纸张", 910, 470);
            //pageSetup.PaperSize = paperSize;

            ////填写明细数据
            //for (int i = 0; i < detailCount; i++)
            //{
            //    MaterialRentalOrderDetail detail = (MaterialRentalOrderDetail)billMaster.Details.ElementAt(i);
            //    //物资名称
            //    flexGrid1.Cell(detailStartRowNumber + i, 1).Text = detail.MaterialResource.Name;
            //    flexGrid1.Cell(detailStartRowNumber + i, 1).Alignment = FlexCell.AlignmentEnum.LeftCenter;

            //    //规格型号
            //    flexGrid1.Cell(detailStartRowNumber + i, 2).Text = detail.MaterialResource.Specification;
            //    flexGrid1.Cell(detailStartRowNumber + i, 2).Alignment = FlexCell.AlignmentEnum.LeftCenter;

            //    //数量
            //    flexGrid1.Cell(detailStartRowNumber + i, 4).Text = detail.Quantity.ToString();

            //    //日租金
            //    flexGrid1.Cell(detailStartRowNumber + i, 5).Text = "";

            //    //金额
            //    flexGrid1.Cell(detailStartRowNumber + i, 6).Text = "";

            //    //备注
            //    flexGrid1.Cell(detailStartRowNumber + i, 7).Text = detail.Descript;
            //    flexGrid1.Cell(detailStartRowNumber + i, 7).Alignment = FlexCell.AlignmentEnum.LeftCenter;
            //}
        }
        #endregion
    }
}
