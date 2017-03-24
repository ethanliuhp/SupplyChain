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
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI
{
    public partial class VGWBSConfirmQueryByBill : TBasicDataView
    {
        private MProductionMng model = new MProductionMng();

        public VGWBSConfirmQueryByBill()
        {
            InitializeComponent();
            InitEvent();
        }

        public void InitEvent()
        {
            dtpDateBegin.Value = DateTime.Now.AddMonths(-1);

            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnExcel.Click += new EventHandler(btnExcel_Click);
            txtCodeBegin.tbTextChanged += new EventHandler(txtCodeBegin_tbTextChanged);
            dgMaster.SelectionChanged += new EventHandler(dgMaster_SelectionChanged);
        }

        void dgMaster_SelectionChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            GWBSTaskConfirmMaster master = dgMaster.CurrentRow.Tag as GWBSTaskConfirmMaster;
            if (master == null) return;

            try { int count = master.Details.Count; }
            catch
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
                oq.AddFetchMode("GWBSDetail.TheCostItem", NHibernate.FetchMode.Eager);
                IList listDetail = model.ProductionManagementSrv.ObjectQuery(typeof(GWBSTaskConfirm), oq);

                master.Details = new HashedSet<BaseDetail>();
                master.Details.AddAll(listDetail.OfType<BaseDetail>().ToList());

                dgMaster.CurrentRow.Tag = master;
            }

            foreach (GWBSTaskConfirm dtl in master.Details)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = dtl;
                dgDetail[colState.Name, rowIndex].Value = dtl.AccountingState;
                dgDetail[colGWBSName.Name, rowIndex].Value = dtl.GWBSTreeName;
                dgDetail[colGWBSName.Name, rowIndex].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.GWBSTreeName, dtl.GwbsSysCode);

                dgDetail[colGWBSDetail.Name, rowIndex].Value = dtl.GWBSDetailName;
                dgDetail[colActualCompletedQuantity.Name, rowIndex].Value = dtl.ActualCompletedQuantity;
                dgDetail[colQuantityBeforeConfirm.Name, rowIndex].Value = dtl.QuantityBeforeConfirm;
                dgDetail[colProjectTaskType.Name, rowIndex].Value = dtl.ProjectTaskType;
                dgDetail[colTaskCompletedPercent.Name, rowIndex].Value = dtl.ProgressBeforeConfirm;
                dgDetail[colSumCompletedPercent.Name, rowIndex].Value = dtl.ProgressAfterConfirm;
                dgDetail[colTaskHandle.Name, rowIndex].Value = dtl.TaskHandlerName;
                dgDetail[colUnit.Name, rowIndex].Value = dtl.WorkAmountUnitName;
                dgDetail[colMaterialFeeSettlement.Name, rowIndex].Value = dtl.MaterialFeeSettlementFlag.ToString();
                dgDetail[colDescript.Name, rowIndex].Value = dtl.Descript;
                //dgDetail[colConfirmState.Name, rowIndex].Value = dtl.ConfirmState;
                //dgDetail[colConfirmDescript.Name, rowIndex].Value = dtl.ConfirmDescript;
                dgDetail[colRealOperationDate.Name, rowIndex].Value = dtl.RealOperationDate.ToShortDateString();
                dgDetail[colPlannedQuantity.Name, rowIndex].Value = dtl.PlannedQuantity;

                dgDetail[colCheckState.Name, rowIndex].Value = dtl.DailyCheckState;
                dgDetail[colCheckState.Name, rowIndex].ToolTipText = StaticMethod.GetCheckStateShowText(dtl.DailyCheckState);
                if (dtl.GWBSDetail != null)
                {
                    dgDetail[colMainResourceName.Name, rowIndex].Value = dtl.GWBSDetail.MainResourceTypeName;
                    dgDetail[colMainResourceSpec.Name, rowIndex].Value = dtl.GWBSDetail.MainResourceTypeSpec;
                    dgDetail[colMainResourceDigramNumber.Name, rowIndex].Value = dtl.GWBSDetail.DiagramNumber;

                    if (dtl.GWBSDetail.TheCostItem != null)
                        dgDetail[colCostItemQuotaCode.Name, rowIndex].Value = dtl.GWBSDetail.TheCostItem.QuotaCode;
                }

            }
        }

        void txtCodeBegin_tbTextChanged(object sender, EventArgs e)
        {
            this.txtCodeEnd.Text = this.txtCodeBegin.Text;
        }

        void btnExcel_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgMaster, true);
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            dgMaster.Rows.Clear();
            dgDetail.Rows.Clear();

            ObjectQuery oq = new ObjectQuery();
            IList list = new ArrayList();
            CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            if (txtCodeBegin.Text != "" && txtCodeEnd.Text != "")
            {
                oq.AddCriterion(Expression.Between("Code", txtCodeBegin.Text, txtCodeEnd.Text));
            }
            oq.AddCriterion(Expression.Ge("CreateDate", this.dtpDateBegin.Value.Date));
            oq.AddCriterion(Expression.Lt("CreateDate", this.dtpDateEnd.Value.AddDays(1).Date));
            if (txtCreatePerson.Text != "")
            {
                oq.AddCriterion(Expression.Like("ConfirmHandlePersonName", txtCreatePerson.Text, MatchMode.Anywhere));
            }

            //oq.AddCriterion(Expression.Not(Expression.Eq("DocState", DocumentState.Edit)));

            try
            {
                list = model.ProductionManagementSrv.ObjectQuery(typeof(GWBSTaskConfirmMaster), oq);
                //list = model.ProductionManagementSrv.GetGWBSTaskConfirmMaster(oq);
                ShowMasterList(list);
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
        }

        /// <summary>
        /// 显示主表
        /// </summary>
        /// <param name="masterList"></param>
        private void ShowMasterList(IList masterList)
        {
            dgMaster.Rows.Clear();
            if (masterList == null || masterList.Count == 0) return;
            foreach (GWBSTaskConfirmMaster master in masterList)
            {
                //if (!HasRefQuantity(master)) continue;

                int rowIndex = dgMaster.Rows.Add();
                dgMaster.Rows[rowIndex].Tag = master;
                dgMaster[colCode.Name, rowIndex].Value = master.Code;
                string[] sArray = master.CreateDate.ToString().Split(' ');
                string stra = sArray[0];
                dgMaster[colCreateDate.Name, rowIndex].Value = stra;
                dgMaster[colConfirmHandlePersonName.Name, rowIndex].Value = master.ConfirmHandlePersonName;
                dgMaster[colProjectName.Name, rowIndex].Value = master.ProjectName;
                //dgMaster[colTaskHandler.Name, rowIndex].Value = master.TaskHandleName;

                dgMaster[colDocState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);

            }
            dgMaster.CurrentCell = dgMaster[1, 0];
            dgMaster_SelectionChanged(dgMaster, new EventArgs());
        }

        ///// <summary>
        ///// 判断主表是否有可引用数量
        ///// </summary>
        ///// <param name="master"></param>
        ///// <returns></returns>
        //private bool HasRefQuantity(GWBSTaskConfirmMaster master)
        //{
        //    if (master == null || master.Details.Count == 0) return false;
        //    foreach (GWBSTaskConfirmMaster dtl in master.Details)
        //    {
        //        if (decimal.Subtract(dtl.Quantity, dtl.RefQuantity) > 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}
