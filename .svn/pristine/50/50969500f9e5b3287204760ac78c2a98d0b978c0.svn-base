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

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekScheduleBak : TMasterDetailView
    {
        private MProductionMng model = new MProductionMng();
        private WeekScheduleMaster curBillMaster;
        //private string execScheduleTypeName="周进度计划";
        private EnumExecScheduleType execScheduleType;

        /// <summary>
        /// 当前单据
        /// </summary>
        public WeekScheduleMaster CurBillMaster
        {
            get { return curBillMaster; }
            set { curBillMaster = value; }
        }

        public VWeekScheduleBak(EnumExecScheduleType execScheduleType)
        {
            InitializeComponent();
            this.execScheduleType = execScheduleType;
            InitEvent();
            InitData();
        }

        public void InitData()
        {
            colPlanConformity.DataSource = Enum.GetNames(typeof(EnumWeekSchedulePlanConformity));
            colOPPlanConformity.DataSource = Enum.GetNames(typeof(EnumWeekSchedulePlanConformity));

            dgDetail.ContextMenuStrip = cmsDg;
            dgOtherPlan.ContextMenuStrip = cmsDgOther;
            dgDetail.SelectionMode = DataGridViewSelectionMode.CellSelect;
            //dtpDateEnd.Value = dtpDateBegin.Value.AddDays(7);

            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                SetDatetime(DateTime.Now);
                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tpWeekPlan);
            }
            else
            {
                tabControl1.TabPages.Clear();
                tabControl1.TabPages.Add(tpOtherPlan);
            }
        }

        private void InitEvent()
        {
            this.dgDetail.CellValidating += new DataGridViewCellValidatingEventHandler(dgDetail_CellValidating);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.UserDeletingRow += new DataGridViewRowCancelEventHandler(dgDetail_UserDeletingRow);
            dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            btnProductSchedule.Click += new EventHandler(btnProductSchedule_Click);

            dtpDateBegin.ValueChanged += new EventHandler(dtpDateBegin_ValueChanged);
            dtpDateEnd.ValueChanged += new EventHandler(dtpDateEnd_ValueChanged);
            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);
            dgOtherPlan.CellEndEdit += new DataGridViewCellEventHandler(dgOtherPlan_CellEndEdit);

            //右键删除菜单
            tsmiDel.Click += new EventHandler(tsmiDel_Click);
            //右键复制菜单
            tsmiCopy.Click += new EventHandler(tsmiCopy_Click);
            tsmiDgOtherDel.Click += new EventHandler(tsmiDgOtherDel_Click);
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colOBSService.Name)
            {
                VContractExcuteSelector vces = new VContractExcuteSelector();
                vces.StartPosition = FormStartPosition.CenterScreen;
                vces.ShowDialog();
                if (vces.Result != null && vces.Result.Count > 0)
                {
                    SubContractProject scp = vces.Result[0] as SubContractProject;
                    dgDetail[e.ColumnIndex, e.RowIndex].Tag = scp;
                    WeekScheduleDetail detail = dgDetail.Rows[e.RowIndex].Tag as WeekScheduleDetail;
                    if (detail == null)
                    {
                        detail = new WeekScheduleDetail();

                    }
                    detail.SupplierRelationInfo = scp.BearerOrg;
                    dgDetail.Rows[e.RowIndex].Tag = detail;
                    dgDetail[e.ColumnIndex, e.RowIndex].Value = scp.BearerOrgName;
                    dgDetail.CurrentCell = dgDetail[colPlannedWrokload.Name, e.RowIndex];
                    dgDetail.CurrentCell = dgDetail[e.ColumnIndex, e.RowIndex];
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
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colOPPlannedWorkload.Name || colName == colOPActualWorkload.Name)
            {
                object pWorkload = dgOtherPlan[e.ColumnIndex, e.RowIndex].Value;
                if (pWorkload == null) pWorkload = "";
                if (!CommonMethod.VeryValid(pWorkload.ToString()))
                {
                    MessageBox.Show("请输入数字！");
                    return;
                }

                object aWorkLoad = dgOtherPlan[e.ColumnIndex, e.RowIndex].Value;
                if (aWorkLoad == null) aWorkLoad = "";
                if (!CommonMethod.VeryValid(aWorkLoad.ToString()))
                {
                    MessageBox.Show("请输入数字！");
                    return;
                }

                decimal sumpWorkload = 0M, sumaWorkload = 0M;
                foreach (DataGridViewRow dr in dgOtherPlan.Rows)
                {
                    sumpWorkload += ClientUtil.ToDecimal(dr.Cells[colOPPlannedWorkload.Name].Value);
                    sumaWorkload += ClientUtil.ToDecimal(dr.Cells[colOPActualWorkload.Name].Value);
                }
                txtSumPlanQuantity.Text = sumpWorkload.ToString("###########.####");
                txtSumActualQuantity.Text = sumaWorkload.ToString("###########.####");
            }
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

        void tsmiDgOtherDel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要删除当前选中的记录吗？", "删除记录", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                DataGridViewRow dr = dgOtherPlan.CurrentRow;
                if (dr == null || dr.IsNewRow) return;
                dgOtherPlan.Rows.Remove(dr);
                if (dr.Tag != null)
                {
                    curBillMaster.Details.Remove(dr.Tag as WeekScheduleDetail);
                }
            }
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
                        cellStyle.BackColor = Color.Red;
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
            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                SetDatetime(dtpDateBegin.Value);
            }
            else
            {
                if (dtpDateEnd.Value.Date < dtpDateBegin.Value.Date)
                {
                    dtpDateEnd.Value = dtpDateBegin.Value.AddDays(7);
                }
            }
        }

        void dtpDateBegin_ValueChanged(object sender, EventArgs e)
        {
            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                SetDatetime(dtpDateBegin.Value);
            }
            else
            {
                if (dtpDateEnd.Value.Date < dtpDateBegin.Value.Date)
                {
                    dtpDateEnd.Value = dtpDateBegin.Value.AddDays(7);
                }
            }
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

            if (execScheduleType == EnumExecScheduleType.周进度计划)
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
        }

        void btnProductSchedule_Click(object sender, EventArgs e)
        {
            VScheduleSelector vss = new VScheduleSelector();
            vss.ShowDialog();
            IList list = vss.Result;
            if (list == null || list.Count == 0) return;
            ProductionScheduleMaster master = (list[0] as ProductionScheduleDetail).Master;

            txtProductSchedule.Text = master.ScheduleTypeDetail + "." + master.ScheduleName;
            curBillMaster.ForwardBillId = master.Id;

            //dgDetail.Rows.Clear();
            //dgOtherPlan.Rows.Clear();

            if (this.execScheduleType == EnumExecScheduleType.周进度计划)
            {
                DateTime dt = dtpDateBegin.Value.Date;
                foreach (ProductionScheduleDetail detail in list)
                {
                    //if (HasAdded(detail, execScheduleType)) continue;
                    int rowIndex = dgDetail.Rows.Add();
                    dgDetail[colGWBSTree.Name, rowIndex].Tag = detail.GWBSTree;
                    dgDetail[colGWBSTree.Name, rowIndex].Value = detail.GWBSTreeName;
                    dgDetail[colMonday.Name, rowIndex].Value = false;
                    dgDetail[colMonday.Name, rowIndex].Tag = dt;

                    dgDetail[colTuesday.Name, rowIndex].Value = false;
                    dgDetail[colTuesday.Name, rowIndex].Tag = dt.AddDays(1);

                    dgDetail[colWednesday.Name, rowIndex].Value = false;
                    dgDetail[colWednesday.Name, rowIndex].Tag = dt.AddDays(2);

                    dgDetail[colThursday.Name, rowIndex].Value = false;
                    dgDetail[colThursday.Name, rowIndex].Tag = dt.AddDays(3);

                    dgDetail[colFriday.Name, rowIndex].Value = false;
                    dgDetail[colFriday.Name, rowIndex].Tag = dt.AddDays(4);

                    dgDetail[colSaturday.Name, rowIndex].Value = false;
                    dgDetail[colSaturday.Name, rowIndex].Tag = dt.AddDays(5);

                    dgDetail[colSunday.Name, rowIndex].Value = false;
                    dgDetail[colSunday.Name, rowIndex].Tag = dt.AddDays(6);
                    dgDetail[colPlannedDuration.Name, rowIndex].Value = 7;
                    dgDetail[colForwardBillDtlIdOfWeek.Name, rowIndex].Value = detail.Id;
                }
            }
            else if (this.execScheduleType == EnumExecScheduleType.月度进度计划)
            {
                foreach (ProductionScheduleDetail detail in list)
                {
                    if (HasAdded(detail, execScheduleType)) continue;

                    int dgOpRowIndex = dgOtherPlan.Rows.Add();
                    dgOtherPlan[colOPGWBSTree.Name, dgOpRowIndex].Tag = detail.GWBSTree;
                    dgOtherPlan[colOPGWBSTree.Name, dgOpRowIndex].Value = detail.GWBSTreeName;
                    dgOtherPlan[colOPPlannedDateBegin.Name, dgOpRowIndex].Value = dtpDateBegin.Value.Date;
                    dgOtherPlan[colOPPlannedDateEnd.Name, dgOpRowIndex].Value = dtpDateEnd.Value.Date;
                    dgOtherPlan[colOPPlannedDuration.Name, dgOpRowIndex].Value = (dtpDateEnd.Value.Date - dtpDateBegin.Value.Date).Days + 1;
                    dgOtherPlan[colForwardBillDtlIdOfMonth.Name, dgOpRowIndex].Value = detail.Id;
                }
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
                foreach (DataGridViewRow dr in dgDetail.Rows)
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
                foreach (DataGridViewRow dr in dgOtherPlan.Rows)
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
                    dgOtherPlan.EditMode = DataGridViewEditMode.EditOnEnter;
                    cmsDg.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    foreach (DataGridViewRow dr in dgDetail.Rows)
                    {
                        SetBackColor(dr);
                    }
                    dgOtherPlan.EditMode = DataGridViewEditMode.EditProgrammatically;
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
            object[] os = new object[] { txtCode, txtCreatePerson, txtCreateDate, txtHandlePerson, txtSumPlanQuantity, txtProject, txtProductSchedule, txtSumActualQuantity };
            ObjectLock.Lock(os);

            string[] lockCols = new string[] { colGWBSTree.Name, colActualBeginDate.Name, colActualEndDate.Name, colActualDuration.Name, colActualWorkload.Name, colPlanConformity.Name, colTaskCompletedPercent.Name, colOBSService.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
            string[] otherLockCols = new string[] { colOPGWBSTree.Name, colOPActualDateBegin.Name, colOPActualDateEnd.Name, colOPActualDuration.Name, colOPActualWorkload.Name, colOPCompletedPercent.Name, colOPPlanConformity.Name };
            dgOtherPlan.SetColumnsReadOnly(otherLockCols);
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
                curBillMaster.SummaryStatus = EnumSummaryStatus.未汇总;
                curBillMaster.ExecScheduleType = this.execScheduleType;

                //制单人
                txtCreatePerson.Text = ConstObject.LoginPersonInfo.Name;
                //制单日期
                txtCreateDate.Text = ConstObject.LoginDate.ToShortDateString();
                //责任人
                txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
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
                        StaticMethod.InsertLogData(curBillMaster.Id, "新增", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "月计划", "", curBillMaster.ProjectName);
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
                        StaticMethod.InsertLogData(curBillMaster.Id, "修改", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "月计划", "", curBillMaster.ProjectName);
                    }
                }

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
                    curBillMaster = model.ProductionManagementSrv.SaveWeekScheduleMaster(curBillMaster);

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
                    if (!model.ProductionManagementSrv.DeleteByDao(curBillMaster)) return false;
                    if (execScheduleType == EnumExecScheduleType.周进度计划)
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "周进度计划", "", curBillMaster.ProjectName);
                    }
                    else
                    {
                        //插入日志
                        StaticMethod.InsertLogData(curBillMaster.Id, "删除", curBillMaster.Code, ConstObject.LoginPersonInfo.Name, "月计划", "", curBillMaster.ProjectName);
                    }

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
            if (execScheduleType == EnumExecScheduleType.周进度计划)
            {
                if (this.dgDetail.Rows.Count == 0)
                {
                    MessageBox.Show("明细不能为空！");
                    return false;
                }
                dgDetail.EndEdit();
                //周进度计划数据校验
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
            }
            else if (execScheduleType == EnumExecScheduleType.月度进度计划)
            {
                if (this.dgOtherPlan.Rows.Count == 0)
                {
                    MessageBox.Show("明细不能为空！");
                    return false;
                }
                dgOtherPlan.EndEdit();
                //其它计划数据校验
                foreach (DataGridViewRow var in dgOtherPlan.Rows)
                {
                    if (var.IsNewRow) break;
                    DateTime otherPlanBeginDate = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateBegin.Name].Value);
                    DateTime otherPlanEndDate = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateEnd.Name].Value);
                    if (otherPlanBeginDate.Date > otherPlanEndDate.Date)
                    {
                        MessageBox.Show("明细计划开始时间大于计划结束时间。");
                        dgOtherPlan.CurrentCell = var.Cells[colOPPlannedDateBegin.Name];
                        return false;
                    }
                    var.Cells[colOPPlannedDuration.Name].Value = (otherPlanEndDate.Date - otherPlanBeginDate.Date).Days + 1;

                    object otherDuration = var.Cells[colOPPlannedDuration.Name].Value;
                    if (otherDuration == null || otherDuration.ToString() == "" || ClientUtil.ToInt(otherDuration) <= 0)
                    {
                        MessageBox.Show("计划工期不能为空或小于等于0！");
                        dgOtherPlan.CurrentCell = var.Cells[colOPPlannedDuration.Name];
                        return false;
                    }

                    object otherWorkload = var.Cells[colOPPlannedWorkload.Name].Value;
                    if (otherWorkload == null || otherWorkload.ToString() == "" || ClientUtil.TransToDecimal(otherWorkload) <= 0)
                    {
                        MessageBox.Show("计划工程量不能为空或小于等于0！！");
                        dgOtherPlan.CurrentCell = var.Cells[colOPPlannedWorkload.Name];
                        return false;
                    }
                }
                dgOtherPlan.Update();
            }

            return true;
        }

        //保存数据
        private bool ViewToModel()
        {
            if (!ValidView()) return false;
            try
            {
                curBillMaster.ForwardBillCode = txtProductSchedule.Text;
                curBillMaster.PlannedBeginDate = dtpDateBegin.Value.Date;
                curBillMaster.PlannedEndDate = dtpDateEnd.Value.Date;
                curBillMaster.Descript = txtRemark.Text;
                curBillMaster.CompletionAnalysis = this.txtCompletionAnalysis.Text;

                //curBillMaster.SumPlannedWorkload = ClientUtil.ToDecimal(txtSumPlanQuantity.Text);
                //curBillMaster.SumActualWorkload = ClientUtil.ToDecimal(txtSumActualQuantity.Text);
                if (execScheduleType == EnumExecScheduleType.周进度计划)
                {
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
                        curBillDtl.SummaryStatus = EnumSummaryStatus.未汇总;
                        //周进度计划
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
                }
                else
                {
                    foreach (DataGridViewRow var in this.dgOtherPlan.Rows)
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
                        curBillDtl.SummaryStatus = EnumSummaryStatus.未汇总;
                        //其它计划
                        curBillDtl.Descript = ClientUtil.ToString(var.Cells[colOPDescript.Name].Value);
                        if (var.Cells[colOPGWBSTree.Name].Tag != null)
                        {
                            curBillDtl.GWBSTree = var.Cells[colOPGWBSTree.Name].Tag as GWBSTree;
                            curBillDtl.GWBSTreeName = ClientUtil.ToString(var.Cells[colOPGWBSTree.Name].Value);
                        }
                        curBillDtl.MainTaskContent = ClientUtil.ToString(var.Cells[colOPMainTaskContent.Name].Value);
                        curBillDtl.PlannedBeginDate = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateBegin.Name].Value);
                        curBillDtl.PlannedEndDate = ClientUtil.ToDateTime(var.Cells[colOPPlannedDateEnd.Name].Value);
                        curBillDtl.PlannedDuration = ClientUtil.ToInt(var.Cells[colOPPlannedDuration.Name].Value);
                        curBillDtl.PlannedWrokload = ClientUtil.ToDecimal(var.Cells[colOPPlannedWorkload.Name].Value);
                        curBillDtl.ActualBeginDate = ClientUtil.ToDateTime(var.Cells[colOPActualDateBegin.Name].Value);
                        curBillDtl.ActualEndDate = ClientUtil.ToDateTime(var.Cells[colOPActualDateEnd.Name].Value);
                        curBillDtl.ActualDuration = ClientUtil.ToInt(var.Cells[colOPActualDuration.Name].Value);
                        curBillDtl.ActualWorklaod = ClientUtil.ToDecimal(var.Cells[colOPActualWorkload.Name].Value);
                        curBillDtl.TaskCompletedPercent = ClientUtil.ToDecimal(var.Cells[colOPCompletedPercent.Name].Value);
                        curBillDtl.PlanConformity = ClientUtil.ToString(var.Cells[colOPPlanConformity.Name].Value);
                        curBillDtl.Descript = ClientUtil.ToString(var.Cells[colOPDescript.Name].Value);
                        curBillDtl.ForwardBillDtlId = var.Cells[colForwardBillDtlIdOfMonth.Name].Value + "";

                        if (var.Cells[colOPPBSTree.Name].Tag != null)
                        {
                            curBillDtl.PBSTree = var.Cells[colOPPBSTree.Name].Tag as PBSTree;
                            curBillDtl.PBSTreeName = ClientUtil.ToString(var.Cells[colOPPBSTree.Name].Value);
                        }
                        curBillMaster.AddDetail(curBillDtl);
                    }

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

                txtHandlePerson.Text = curBillMaster.HandlePersonName;
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
                dgOtherPlan.Rows.Clear();
                foreach (WeekScheduleDetail var in curBillMaster.Details)
                {
                    //周进度计划
                    if (execScheduleType == EnumExecScheduleType.周进度计划)
                    {
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
                    else
                    {
                        //其它计划
                        int i = this.dgOtherPlan.Rows.Add();
                        dgOtherPlan.Rows[i].Cells[colOPGWBSTree.Name].Tag = var.GWBSTree;
                        dgOtherPlan.Rows[i].Cells[colOPGWBSTree.Name].Value = var.GWBSTreeName;
                        dgOtherPlan.Rows[i].Cells[colOPMainTaskContent.Name].Value = var.MainTaskContent;
                        dgOtherPlan.Rows[i].Cells[colOPPlannedDateBegin.Name].Value = var.PlannedBeginDate;
                        dgOtherPlan.Rows[i].Cells[colOPPlannedDateEnd.Name].Value = var.PlannedEndDate;
                        dgOtherPlan.Rows[i].Cells[colOPPlannedDuration.Name].Value = var.PlannedDuration;
                        dgOtherPlan.Rows[i].Cells[colOPPlannedWorkload.Name].Value = var.PlannedWrokload;
                        dgOtherPlan.Rows[i].Cells[colOPActualDateBegin.Name].Value = var.ActualBeginDate;
                        dgOtherPlan.Rows[i].Cells[colOPActualDateEnd.Name].Value = var.ActualEndDate;
                        dgOtherPlan.Rows[i].Cells[colOPActualDuration.Name].Value = var.ActualDuration;
                        dgOtherPlan.Rows[i].Cells[colOPActualWorkload.Name].Value = var.ActualWorklaod;
                        dgOtherPlan.Rows[i].Cells[colOPCompletedPercent.Name].Value = var.TaskCompletedPercent;
                        dgOtherPlan.Rows[i].Cells[colOPPlanConformity.Name].Value = var.PlanConformity;
                        dgOtherPlan.Rows[i].Cells[colOPDescript.Name].Value = var.Descript;

                        dgOtherPlan.Rows[i].Cells[colOPPBSTree.Name].Tag = var.PBSTree;
                        dgOtherPlan.Rows[i].Cells[colOPPBSTree.Name].Value = var.PBSTreeName;
                        dgOtherPlan.Rows[i].Cells[colForwardBillDtlIdOfMonth.Name].Value = var.ForwardBillDtlId;

                        this.dgOtherPlan.Rows[i].Tag = var;
                    }
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
