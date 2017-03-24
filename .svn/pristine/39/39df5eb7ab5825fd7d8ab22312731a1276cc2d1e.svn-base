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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI
{
    public partial class VWeekScheduleSelector : TBasicDataView
    {
        public DateTime DefaultBeginDate = ConstObject.LoginDate.AddDays(-9);
        public DateTime DefaultEndDate = ConstObject.LoginDate;

        MProductionMng model = new MProductionMng();
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

        public VWeekScheduleSelector()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
        }

        private void InitForm()
        {

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

            this.Load += new EventHandler(VWeekScheduleSelector_Load);

            //pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void VWeekScheduleSelector_Load(object sender, EventArgs e)
        {
            dtpDateBegin.Value = DefaultBeginDate;
            dtpDateEnd.Value = DefaultEndDate;
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
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                object objColSelect = var.Cells[colSelect.Name].Value;
                if (objColSelect != null && (bool)objColSelect)
                {
                    WeekScheduleDetail dtl = var.Tag as WeekScheduleDetail;
                    result.Add(dtl);
                }
            }
            if (result.Count == 0)
            {
                MessageBox.Show("没有选中的记录！");
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
            IList list = new ArrayList();
            oq.AddCriterion(Expression.Eq("Master.ExecScheduleType", EnumExecScheduleType.周进度计划));
            oq.AddCriterion(Expression.Eq("Master.ProjectId", StaticMethod.GetProjectInfo().Id));

            oq.AddCriterion(Expression.Ge("Master.CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("Master.CreateDate", dtpDateEnd.Value.AddDays(1).Date));

            if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
            {
                PersonInfo per = txtHandlePerson.Result[0] as PersonInfo;
                oq.AddCriterion(Expression.Eq("Master.HandlePerson.Id", per.Id));
            }
            if (txtGWBSName.Text != "")
            {
                oq.AddCriterion(Expression.Like("GWBSTreeName", txtGWBSName.Text, MatchMode.Anywhere));
            }

            oq.AddCriterion(Expression.Eq("Master.SummaryStatus", EnumSummaryStatus.未汇总));

            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            try
            {
                list = model.ProductionManagementSrv.GetWeekScheduleDetail(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList list)
        {
            dgDetail.Rows.Clear();
            if (list == null || list.Count == 0) return;
            foreach (WeekScheduleDetail obj in list)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = obj;

                dgDetail[colWeekPlanName.Name, rowIndex].Value = obj.Master.PlanName;
                dgDetail[colOwner.Name, rowIndex].Value = obj.Master.HandlePersonName;

                dgDetail[colGWBSTreeName.Name, rowIndex].Value = obj.GWBSTreeName;
                dgDetail[colGWBSTreeName.Name, rowIndex].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), obj.GWBSTree);

                dgDetail[colPlannedBeginDate.Name, rowIndex].Value = obj.PlannedBeginDate.ToShortDateString();
                dgDetail[colPlannedEndDate.Name, rowIndex].Value = obj.PlannedEndDate.ToShortDateString();
                dgDetail[colPlannedDuration.Name, rowIndex].Value = obj.PlannedDuration;
                dgDetail[colActualBeginDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(obj.ActualBeginDate, false);
                dgDetail[colActualEndDate.Name, rowIndex].Value = StaticMethod.GetShowDateTimeStr(obj.ActualEndDate, false);
                dgDetail[colActualDuration.Name, rowIndex].Value = obj.ActualDuration;
                dgDetail[colDescript.Name, rowIndex].Value = obj.Descript;
            }
        }
    }
}
