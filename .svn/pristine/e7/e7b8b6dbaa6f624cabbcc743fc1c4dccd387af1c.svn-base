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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI
{
    public partial class VGWBSDetailSelectorBak : TBasicDataView
    {
        MProductionMng model = new MProductionMng();
        private int totalRecords = 0;

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        public VGWBSDetailSelectorBak()
        {
            InitializeComponent();
            InitEvent();
            InitForm();
        }

        private void InitForm()
        {
            dtpDateBegin.Value = ConstObject.LoginDate.AddDays(-30);
            dtpDateEnd.Value = ConstObject.LoginDate;
        }

        private void InitEvent()
        {
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
            dgDetail.CellValueChanged += new DataGridViewCellEventHandler(dgDetail_CellValueChanged);

            dgDetail.CellContentClick += new DataGridViewCellEventHandler(dgDetail_CellContentClick);
            Load += new EventHandler(VGWBSDetailSelector_Load);
            dgMaster.CellValidating += new DataGridViewCellValidatingEventHandler(dgMaster_CellValidating);
            btnSearchGWBS.Click += new EventHandler(btnSearchGWBS_Click);

            //pnlFloor.Paint += new PaintEventHandler(pnlFloor_Paint);
        }

        void btnSearchGWBS_Click(object sender, EventArgs e)
        {
            bool rowSelected = false;
            foreach (DataGridViewRow dr in dgMaster.Rows)
            {
                if ((bool)dr.Cells[colDgMasterSelect.Name].Value)
                {
                    rowSelected = true;
                    break;
                }
            }
            if (rowSelected == false)
            {
                MessageBox.Show("请先选择周计划。");
                return;
            }
            dgDetail.Rows.Clear();
            try
            {
                //foreach (DataGridViewRow dr in dgMaster.Rows)
                //{
                //    if ((bool)dr.Cells[colDgMasterSelect.Name].Value)
                //    {
                //        WeekScheduleDetail detail = dr.Tag as WeekScheduleDetail;
                //        IList treeNodes = model.ProductionManagementSrv.GetGWBSTree(detail.GWBSTree);
                //        IList GWBSDetailList = GetGwbsDetail(treeNodes);
                //        AddGWBSDetail(GWBSDetailList,detail);
                //    }
                //}

                foreach (DataGridViewRow dr in dgMaster.Rows)
                {
                    if ((bool)dr.Cells[colDgMasterSelect.Name].Value)
                    {
                        WeekScheduleDetail detail = dr.Tag as WeekScheduleDetail;

                        ObjectQuery oq = new ObjectQuery();
                        oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
                        oq.AddCriterion(Expression.Like("TheGWBSSysCode", detail.GWBSTree.SysCode, MatchMode.Start));
                        oq.AddFetchMode("TheGWBS", NHibernate.FetchMode.Eager);

                        IList GWBSDetailList = model.ProductionManagementSrv.GetGWBSDetail(oq);

                        AddGWBSDetail(GWBSDetailList, detail);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询工程任务明细出错。\n" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private IList GetGwbsDetail(IList gwbsTrees)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProduceConfirmFlag", 1));
            Disjunction dis = Expression.Disjunction();
            foreach (GWBSTree tree in gwbsTrees)
            {
                dis.Add(Expression.Eq("TheGWBS.Id", tree.Id));
            }
            oq.AddCriterion(dis);
            oq.AddFetchMode("TheGWBS", NHibernate.FetchMode.Eager);
            return model.ProductionManagementSrv.GetGWBSDetail(oq);
        }

        private void AddGWBSDetail(IList GWBSDetailList, WeekScheduleDetail weekScheduleDetail)
        {
            if (GWBSDetailList == null || GWBSDetailList.Count == 0) return;
            foreach (GWBSDetail detail in GWBSDetailList)
            {
                int rowIndex = dgDetail.Rows.Add();
                detail.WeekScheduleDetail = weekScheduleDetail;
                dgDetail.Rows[rowIndex].Tag = detail;

                dgDetail[colGWBSDetailName.Name, rowIndex].Value = detail.Name;
                dgDetail[colPart.Name, rowIndex].Value = detail.WorkPart;
                dgDetail[colMaterial.Name, rowIndex].Value = detail.WorkUseMaterial;
                dgDetail[colMethod.Name, rowIndex].Value = detail.WorkMethod;
                dgDetail[colDescript.Name, rowIndex].Value = detail.ContentDesc;
                //dgDetail[colCostItemName.Name, rowIndex].Value = detail.CostItemName;
                dgDetail[colOrg.Name, rowIndex].Value = detail.BearOrgName;
                dgDetail[colSelect.Name, rowIndex].Value = true;
            }

        }

        void dgMaster_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgMaster[e.ColumnIndex, e.RowIndex].OwningColumn.Name == colDgMasterSelect.Name)
            {
                if (e.FormattedValue.ToString().Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    string gwbwTreeId = (dgMaster.Rows[e.RowIndex].Tag as WeekScheduleDetail).GWBSTree.Id;
                    //选择时判断是否已经选中了此任务的不同进度明细
                    foreach (DataGridViewRow dr in dgMaster.Rows)
                    {
                        if (dr.Index == e.RowIndex) continue;
                        WeekScheduleDetail detail = dr.Tag as WeekScheduleDetail;

                        if (detail.GWBSTree.Id == gwbwTreeId && (bool)dr.Cells[colDgMasterSelect.Name].Value)
                        {
                            MessageBox.Show("工程任务【" + detail.GWBSTreeName + "】已经选中，请先取消，再进行选择。");
                            e.Cancel = true;
                            //dgMaster[e.ColumnIndex, e.RowIndex].Value = false;
                            return;
                        }
                    }
                }
            }
        }

        void VGWBSDetailSelector_Load(object sender, EventArgs e)
        {
            //txtHandlePerson.Visible = true;
            //txtHandlePerson.ReadOnly = false;
            //txtHandlePerson.Enabled = true;
            txtHandlePerson.Text = ConstObject.LoginPersonInfo.Name;
            txtHandlePerson.Result.Add(ConstObject.LoginPersonInfo);
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
            //throw new NotImplementedException();
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

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            //dgDetail.Rows.Clear();
            //Clear();
            //WeekScheduleDetail weekScheduleDetail = dgMaster.CurrentRow.Tag as WeekScheduleDetail;
            //if (weekScheduleDetail == null || weekScheduleDetail.GWBSTree == null) return;

            //IList list = model.ProductionManagementSrv.GetGWBSDetailByParentId(weekScheduleDetail.GWBSTree.Id);
            //if (list == null || list.Count == 0) return;

            //foreach (GWBSDetail detail in list)
            //{
            //    int rowIndex = dgDetail.Rows.Add();
            //    dgDetail.Rows[rowIndex].Tag = detail;

            //    dgDetail[colGWBSDetailName.Name, rowIndex].Value = detail.Name;
            //    dgDetail[colPart.Name, rowIndex].Value = detail.WorkPart;
            //    dgDetail[colMaterial.Name, rowIndex].Value = detail.WorkUseMaterial;
            //    dgDetail[colMethod.Name, rowIndex].Value = detail.WorkMethod;
            //    dgDetail[colDescript.Name, rowIndex].Value = detail.ContentDesc;
            //    //dgDetail[colCostItemName.Name, rowIndex].Value = detail.CostItemName;
            //    dgDetail[colOrg.Name, rowIndex].Value = detail.BearOrgName;
            //    dgDetail[colSelect.Name, rowIndex].Value = true;
            //}
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
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
            if (this.dgMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有可以引用的工程任务！");
                return;
            }
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                if ((bool)var.Cells[colSelect.Name].Value)
                {
                    GWBSDetail gwbsDetail = var.Tag as GWBSDetail;
                    result.Add(gwbsDetail);
                }
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
            oq.AddCriterion(Expression.Ge("Master.CreateDate", dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("Master.CreateDate", dtpDateEnd.Value.AddDays(1).Date));
            if (txtCodeBegin.Text != "")
            {
                oq.AddCriterion(Expression.Between("Master.Code", txtCodeBegin.Text, txtCodeEnd.Text));
            }

            if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
            {
                oq.AddCriterion(Expression.Eq("Master.HandlePerson.Id", (txtHandlePerson.Result[0] as PersonInfo).Id));
            }
            //if (txtHandlePerson.Text != "" && txtHandlePerson.Result.Count > 0)
            //{
            //    oq.AddCriterion(Expression.Eq("HandlePerson",txtHandlePerson.Result[0] as PersonInfo));
            //}
            if (txtGWBSName.Text != "")
            {
                oq.AddCriterion(Expression.Like("GWBSTreeName", txtGWBSName.Text, MatchMode.Anywhere));
            }
            oq.AddCriterion(Expression.Eq("Master.ProjectId", StaticMethod.GetProjectInfo().Id));
            oq.AddCriterion(Expression.Eq("GWBSConfirmFlag", 0));//工程任务确认标志
            oq.AddCriterion(Expression.Eq("ScheduleConfirmFlag", 1));//形象进度确认标志
            oq.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
            oq.AddOrder(Order.Asc("GWBSTreeName"));
            oq.AddOrder(Order.Asc("TaskCompletedPercent"));
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
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            string lastGwbsId = "";
            decimal lastPercent = 0M;
            foreach (WeekScheduleDetail detail in masterList)
            {
                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = detail;
                if (rowIndex == 0)
                {
                    dgMaster[colDgMasterSelect.Name, rowIndex].Value = true;
                }
                else
                {
                    dgMaster[colDgMasterSelect.Name, rowIndex].Value = true;
                    if (lastGwbsId == detail.GWBSTree.Id && detail.TaskCompletedPercent > lastPercent)
                    {
                        dgMaster[colDgMasterSelect.Name, rowIndex - 1].Value = false;
                    }
                }
                lastPercent = detail.TaskCompletedPercent;
                lastGwbsId = detail.GWBSTree.Id;
                dgMaster[colDgMasterCode.Name, rowIndex].Value = detail.Master.Code;
                dgMaster[colDgMasterGWBSName.Name, rowIndex].Value = detail.GWBSTreeName;
                dgMaster[colDgMasterActualBeginDate.Name, rowIndex].Value = detail.ActualBeginDate.ToShortDateString();
                dgMaster[colDgMasterActualEndDate.Name, rowIndex].Value = detail.ActualEndDate.ToShortDateString();
                dgMaster[colDgMasterTaskContent.Name, rowIndex].Value = detail.MainTaskContent;
                dgMaster[colDgMasterFinishPercent.Name, rowIndex].Value = detail.TaskCompletedPercent;
                dgMaster[colDgMasterAnalysis.Name, rowIndex].Value = detail.CompletionAnalysis;
            }
            if (dgMaster.Rows.Count == 0) return;
            //dgMaster.CurrentCell = dgMaster[1, 0];
            //dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }

        /// <summary>
        /// 判断主表是否有可引用数量
        /// </summary>
        /// <param name="master"></param>
        /// <returns></returns>
        private bool HasRefQuantity(BaseMaster master)
        {
            if (master == null || master.Details.Count == 0) return false;
            foreach (BaseDetail dtl in master.Details)
            {
                if (decimal.Subtract(dtl.Quantity, dtl.RefQuantity) > 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
