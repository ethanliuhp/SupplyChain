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
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VScheduleSelectorBak : TBasicDataView
    {
        MProductionMng model = new MProductionMng();
        /// <summary>
        /// 前驱引用计划类型
        /// </summary>
        public EnumExecScheduleType FrontPlanType = EnumExecScheduleType.周进度计划;

        /// <summary>
        /// 周进度计划的起始时间
        /// </summary>
        public DateTime? DefaultBeginDate = null;
        /// <summary>
        /// 周进度计划结束时间
        /// </summary>
        public DateTime? DefaultEndDate = null;

        CurrentProjectInfo projectInfo = null;

        private int totalRecords = 0;
        private decimal sumQuantity = 0M;

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VScheduleSelectorBak()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
        }

        public VScheduleSelectorBak(EnumExecScheduleType frontPlanType)
        {
            InitializeComponent();

            FrontPlanType = frontPlanType;

            InitEvent();

            InitForm();
        }

        private void InitForm()
        {
            projectInfo = StaticMethod.GetProjectInfo();


            VBasicDataOptr.InitScheduleTypeRolling(cboSchedulePlanName, false);
            if (cboSchedulePlanName.Items.Count > 0)
            {
                cboSchedulePlanName.SelectedIndex = 0;
            }
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);

            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);

            cboSchedulePlanName.SelectedIndexChanged += new EventHandler(cboScheduleType_SelectedIndexChanged);

            //pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);

            this.Load += new EventHandler(VScheduleSelector_Load);
        }

        void VScheduleSelector_Load(object sender, EventArgs e)
        {
            //在周进度计划前后加减12天刚好一个月范围
            if (DefaultBeginDate != null)
                dtpDateBegin.Value = DefaultBeginDate.Value.AddDays(-7);
            else
                dtpDateBegin.Value = DateTime.Now.Date.AddMonths(-1);

            if (DefaultEndDate != null)
                dtpDateEnd.Value = DefaultEndDate.Value.AddDays(7);
            else
                dtpDateEnd.Value = DateTime.Now.Date;
        }

        void cboScheduleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string planName = cboSchedulePlanName.SelectedItem + "";

                cbPlanVersion.Text = "";
                cbPlanVersion.Items.Clear();

                EnumScheduleType enumScheduleType = EnumScheduleType.总滚动进度计划;

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("ScheduleType", enumScheduleType));
                oq.AddCriterion(Expression.Eq("ScheduleTypeDetail", planName));

                IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleMaster), oq);

                cbPlanVersion.Items.Clear();
                if (listMaster.Count > 0)
                {
                    cbPlanVersion.Items.Add("");
                    for (int i = 0; i < listMaster.Count; i++)
                    {
                        ProductionScheduleMaster item = listMaster[i] as ProductionScheduleMaster;

                        if (!string.IsNullOrEmpty(item.ScheduleName))
                            cbPlanVersion.Items.Add(item.ScheduleName);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnSearch.Focus();
        }

        void dgDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (dgDetail[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colSelect.Name)
            {
                dgDetail.EndEdit();
            }
        }

        void dgDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell cell = dgDetail[e.ColumnIndex, e.RowIndex];
            if (cell.OwningColumn.Name == colSelect.Name)
            {
                bool selected = (bool)cell.Value;
                if (selected)
                {
                    totalRecords += 1;
                }
                else
                {
                    totalRecords -= 1;
                }
                lblRecordTotal.Text = string.Format("共选择【{0}】条记录", totalRecords);
            }
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkNone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                bool isSelected = (bool)var.Cells[colSelect.Name].Value;
                var.Cells[colSelect.Name].Value = !isSelected;
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                var.Cells[colSelect.Name].Value = true;
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnOK.FindForm().Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            this.dgDetail.EndEdit();

            int selectCountRecord = 0;
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow)
                    break;
                object objColSelect = var.Cells[colSelect.Name].Value;
                if (objColSelect != null && (bool)objColSelect)
                {
                    ProductionScheduleDetail dtl = var.Tag as ProductionScheduleDetail;
                    if (DefaultBeginDate == null || DefaultEndDate == null)
                        result.Add(dtl);
                    else if ((dtl.PlannedBeginDate <= DefaultBeginDate && DefaultBeginDate <= dtl.PlannedEndDate)
                        || (dtl.PlannedBeginDate <= DefaultEndDate && DefaultEndDate <= dtl.PlannedEndDate))
                        result.Add(dtl);

                    selectCountRecord += 1;
                }
            }

            if (selectCountRecord == 0)
            {
                MessageBox.Show("没有选中记录！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (result.Count == 0)
            {
                MessageBox.Show("当前编制的周进度计划时间范围和选择的滚动计划时间范围不存在任何交集，请检查！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.btnOK.FindForm().Close();
        }

        private void Clear()
        {
            lblRecordTotal.Text = string.Format("共选择【{0}】条记录", 0);
            totalRecords = 0;
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();

            oq.AddCriterion(Expression.Eq("Master.ProjectId", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("Master.ScheduleType", EnumScheduleType.总滚动进度计划));
            oq.AddCriterion(Expression.Eq("Master.ScheduleTypeDetail", cboSchedulePlanName.SelectedItem + ""));

            if (cbPlanVersion.Text.Trim() != "")
                oq.AddCriterion(Expression.Eq("Master.ScheduleName", cbPlanVersion.Text.Trim()));


            DateTime beginTime = dtpDateBegin.Value.Date;
            DateTime endTime = dtpDateEnd.Value.AddDays(1).Date;
            oq.AddCriterion(Expression.Ge("PlannedBeginDate", beginTime));
            oq.AddCriterion(Expression.Lt("PlannedBeginDate", endTime));


            oq.AddCriterion(Expression.Eq("State", EnumScheduleDetailState.有效));

            oq.AddCriterion(Expression.Not(Expression.Eq("Level", 1)));

            oq.AddCriterion(Expression.Lt("AddupFigureProgress", (decimal)1));

            if (FrontPlanType == EnumExecScheduleType.周进度计划)
                oq.AddCriterion(Expression.Eq("GWBSTree.CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.LeafNode));

            if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("HandlePerson", txtHandlePerson.Result[0] as PersonInfo));
            }
            if (txtGWBSName.Text != "")
            {
                oq.AddCriterion(Expression.Like("GWBSTreeName", txtGWBSName.Text, MatchMode.Anywhere));
            }

            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);

            try
            {
                IList list = model.ProductionManagementSrv.ObjectQuery(typeof(ProductionScheduleDetail), oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList list)
        {
            dgDetail.Rows.Clear();
            if (list == null || list.Count == 0)
                return;

            foreach (ProductionScheduleDetail obj in list)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = obj;

                dgDetail[colPlanVersion.Name, rowIndex].Value = obj.Master.ScheduleName;//计划版本

                dgDetail[colGWBSTreeName.Name, rowIndex].Value = obj.GWBSTreeName;
                dgDetail[colGWBSTreeName.Name, rowIndex].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), obj.GWBSTree);

                dgDetail[colFigureprogress.Name, rowIndex].Value = obj.AddupFigureProgress;

                dgDetail[colPlannedBeginDate.Name, rowIndex].Value = obj.PlannedBeginDate.ToShortDateString();
                dgDetail[colPlannedEndDate.Name, rowIndex].Value = obj.PlannedEndDate.ToShortDateString();
                dgDetail[colPlannedDuration.Name, rowIndex].Value = obj.PlannedDuration;

                dgDetail[colActualBeginDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(obj.ActualBeginDate, false);
                dgDetail[colActualEndDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(obj.ActualEndDate, false);
                dgDetail[colActualDuration.Name, rowIndex].Value = obj.ActualDuration;

                dgDetail[colUnit.Name, rowIndex].Value = obj.ScheduleUnit;
                dgDetail[colPlanDesc.Name, rowIndex].Value = obj.Master.Descript;
                dgDetail[colDtlRemark.Name, rowIndex].Value = obj.TaskDescript;

            }
        }

    }
}
