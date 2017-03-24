using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.SupplyMng;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VSelectRollingDemandPlan : TBasicDataView
    {
        /// <summary>
        /// 要查询的计划类型（0表示所有，否则只显示指定计划类型）
        /// </summary>
        public ResourceRequirePlanType FilterPlanType = 0;
        /// <summary>
        /// 要查询的状态（0表示所有，否则只显示指定状态）
        /// </summary>
        public ResourceRequirePlanDetailState FilterPlanState = 0;

        public RemandPlanType PlanType = RemandPlanType.节点需求计划;

        public bool isOK = false;
        public List<ResourceRequirePlanDetail> SelectResult = new List<ResourceRequirePlanDetail>();

        public MRollingDemandPlan model;
        public MDailyPlanMng MDailyPlan = new MDailyPlanMng();
        public MLaborDemandPlanMng MLaborDemandPlan = new MLaborDemandPlanMng();
        public MMonthlyPlanMng MMonthlyPlan = new MMonthlyPlanMng();
        public MDemandMasterPlanMng MDemandMasterPlan = new MDemandMasterPlanMng();

        private CurrentProjectInfo projectInfo = null;

        public VSelectRollingDemandPlan(MRollingDemandPlan mot)
        {
            model = mot;

            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            projectInfo = StaticMethod.GetProjectInfo();

            InitEvents();

            DateTime now = model.GetServerTime();
            dtMadeStartDateQuery.Value = now.Date.AddMonths(-1);
            dtMadeEndDateQuery.Value = now.Date;

        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);

            cbAllSelect.Click += new EventHandler(cbAllSelect_Click);
            gridPlanDetail.CellClick += new DataGridViewCellEventHandler(gridPlanDetail_CellClick);

            cbPlanTypeQuery.SelectedIndexChanged += new EventHandler(cbPlanTypeQuery_SelectedIndexChanged);

            btnSelectTaskNode.Click += new EventHandler(btnSelectTaskNode_Click);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);

            this.Load += new EventHandler(VSelectRollingDemandPlan_Load);
        }

        void gridPlanDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colSelect.Index && e.RowIndex > -1)
            {
                gridPlanDetail.RefreshEdit();
                UpdateRowRecordCount();
            }
        }

        void VSelectRollingDemandPlan_Load(object sender, EventArgs e)
        {
            if (FilterPlanType == 0)
            {
                foreach (string type in Enum.GetNames(typeof(ResourceRequirePlanType)))
                {
                    cbPlanTypeQuery.Items.Add(type);
                }
            }
            else
            {
                cbPlanTypeQuery.Items.Add(FilterPlanType.ToString());
            }
            cbPlanTypeQuery.SelectedIndex = 0;


            if (FilterPlanState == 0)
            {
                cbStateQuery.Items.Add("");
                foreach (string state in Enum.GetNames(typeof(ResourceRequirePlanDetailState)))
                {
                    cbStateQuery.Items.Add(state);
                }
            }
            else
            {
                cbStateQuery.Items.Add(FilterPlanState.ToString());
            }
            cbStateQuery.SelectedIndex = 0;


            if (PlanType == RemandPlanType.需求总计划 || PlanType == RemandPlanType.日常需求计划 || PlanType == RemandPlanType.月度需求计划 || PlanType == RemandPlanType.节点需求计划)
            {
                cbResourceType.Items.Add(ResourceRequirePlanDetailResourceType.物资.ToString());
            }
            else
            {
                cbResourceType.Items.Add(ResourceRequirePlanDetailResourceType.劳务.ToString());
            }
            cbResourceType.SelectedIndex = 0;
        }

        void cbAllSelect_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in gridPlanDetail.Rows)
            {
                row.Cells[colSelect.Name].Value = cbAllSelect.Checked;
            }

            int recordCount = 0;
            if (cbAllSelect.Checked)
                recordCount = gridPlanDetail.Rows.Count;

            UpdateRowRecordCount(recordCount);
        }

        //确定
        void btnEnter_Click(object sender, EventArgs e)
        {
            SelectResult.Clear();

            foreach (DataGridViewRow row in gridPlanDetail.Rows)
            {
                if (row.Cells[colSelect.Name].Value != null && Convert.ToBoolean(row.Cells[colSelect.Name].Value))
                {
                    SelectResult.Add(row.Tag as ResourceRequirePlanDetail);
                }
            }
            if (SelectResult.Count == 0)
            {
                MessageBox.Show("请选择计划明细！");
                gridPlanDetail.Focus();
                return;
            }

            //if (ValidMaterialType() == false)
            //    return;

            isOK = true;
            this.Close();
        }

        private bool ValidMaterialType()
        {
            if (PlanType == RemandPlanType.需求总计划 || PlanType == RemandPlanType.日常需求计划 || PlanType == RemandPlanType.月度需求计划 || PlanType == RemandPlanType.节点需求计划)
            {
                foreach (ResourceRequirePlanDetail theResourceRequirePlanDetail in SelectResult)
                {
                    if (theResourceRequirePlanDetail.BuildResourceTypeName == ResourceRequirePlanDetailResourceType.劳务.ToString())
                    {
                        MessageBox.Show("“" + PlanType + "”的资源类型必须是物资类型,请移除非物资类型的需求计划！");
                        return false;
                    }
                }
            }
            else if (PlanType == RemandPlanType.劳务需求计划)
            {
                foreach (ResourceRequirePlanDetail theResourceRequirePlanDetail in SelectResult)
                {
                    if (theResourceRequirePlanDetail.BuildResourceTypeName == ResourceRequirePlanDetailResourceType.物资.ToString())
                    {
                        MessageBox.Show("“" + PlanType + "”的资源类型必须是劳务类型,请移除非劳务类型的需求计划！");
                        return false;
                    }
                }
            }
            return true;
        }
        //取消
        void btnCancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }

        void cbPlanTypeQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            string planType = cbPlanTypeQuery.SelectedItem.ToString();

            cbPlanNameQuery.Text = "";
            cbPlanNameQuery.Items.Clear();

            //资源需求计划名称
            VBasicDataOptr.InitBasicDataByCurrProjectInfo(planType, cbPlanNameQuery, false);
            if (cbPlanNameQuery.Items.Count > 0)
                cbPlanNameQuery.SelectedIndex = 0;
        }

        void btnSelectTaskNode_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree frm = new VSelectGWBSTree(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;

            if (list != null && list.Count > 0)
            {
                GWBSTree selectTaskNode = list[0].Tag as GWBSTree;
                txtTaskNameQuery.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), selectTaskNode);
                txtTaskNameQuery.Tag = selectTaskNode;
            }
        }

        //查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            string planType = cbPlanTypeQuery.Text.Trim();
            string planName = cbPlanNameQuery.Text.Trim();
            string resourceType = cbResourceType.Text.Trim();

            DateTime madeStartDate = dtMadeStartDateQuery.Value;
            DateTime madeEndDate = dtMadeEndDateQuery.Value;
            madeEndDate = madeEndDate.AddDays(1).AddSeconds(-1);

            if (madeStartDate > madeEndDate)
            {
                MessageBox.Show("制单起始日期不能大于结束日期！");
                dtMadeStartDateQuery.Focus();
                return;
            }
            else if ((madeEndDate - madeStartDate).Days > 31)
            {
                MessageBox.Show("制单日期范围不能超过一个月！");
                dtMadeEndDateQuery.Focus();
                return;
            }

            string resourceName = txtResourceName.Text.Trim();
            string spec = txtSpecification.Text.Trim();

            GWBSTree selectTaskNode = null;
            if (txtTaskNameQuery.Text.Trim() != "" && txtTaskNameQuery.Tag != null)
                selectTaskNode = txtTaskNameQuery.Tag as GWBSTree;

            ResourceRequirePlanDetailState state = 0;
            if (cbStateQuery.Text.Trim() != "")
                state = VirtualMachine.Component.Util.EnumUtil<ResourceRequirePlanDetailState>.FromDescription(cbStateQuery.Text.Trim());


            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));

            oq.AddCriterion(Expression.Ge("TheResourceRequirePlan.CreateTime", madeStartDate));
            oq.AddCriterion(Expression.Le("TheResourceRequirePlan.CreateTime", madeEndDate));

            if (selectTaskNode != null)
                oq.AddCriterion(Expression.Eq("TheGWBSTaskGUID.Id", selectTaskNode.Id));

            if (state != 0)
                oq.AddCriterion(Expression.Eq("State", state));

            if (!string.IsNullOrEmpty(resourceType))
                oq.AddCriterion(Expression.Eq("BuildResourceTypeName", resourceType));


            if (!string.IsNullOrEmpty(resourceName))
                oq.AddCriterion(Expression.Like("MaterialName", resourceName, MatchMode.Anywhere));

            if (!string.IsNullOrEmpty(spec))
                oq.AddCriterion(Expression.Like("MaterialSpec", spec, MatchMode.Anywhere));


            oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.PlanType", planType));
            if (!string.IsNullOrEmpty(planName))
                oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.RequirePlanVersion", planName));


            //oq.AddOrder(NHibernate.Criterion.Order.Asc("TheGWBSTaskName"));
            //oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateTime"));
            //oq.AddOrder(NHibernate.Criterion.Order.Asc("BuildResourceTypeName"));
            //oq.AddOrder(NHibernate.Criterion.Order.Asc("ServiceType"));
            //oq.AddOrder(NHibernate.Criterion.Order.Asc("MaterialName"));
            //oq.AddOrder(NHibernate.Criterion.Order.Asc("MaterialQuality"));
            //oq.AddOrder(NHibernate.Criterion.Order.Asc("MaterialSpec"));

            IEnumerable<ResourceRequirePlanDetail> list = model.ObjectQuery(typeof(ResourceRequirePlanDetail), oq).OfType<ResourceRequirePlanDetail>();

            list = from d in list
                   orderby d.MaterialSpec ascending
                   orderby d.MaterialStuff ascending
                   orderby d.MaterialName ascending
                   orderby d.ServiceType ascending
                   orderby d.BuildResourceTypeName ascending
                   orderby d.CreateTime ascending
                   orderby d.TheGWBSTaskName ascending
                   select d;

            gridPlanDetail.Rows.Clear();
            if (list != null)
            {
                foreach (ResourceRequirePlanDetail tempDtl in list)
                {
                    AddResourceRequireDetailInGrid(tempDtl, false, false);
                }

                gridPlanDetail.ClearSelection();
            }
        }

        private void UpdateRowRecordCount(int recordCount)
        {
            lblSelectCount.Text = "共选择" + recordCount + "条记录";
        }
        private void UpdateRowRecordCount()
        {
            int recordCount = 0;
            foreach (DataGridViewRow row in gridPlanDetail.Rows)
            {
                if (row.Cells[colSelect.Name].Value != null && (bool)row.Cells[colSelect.Name].Value)
                {
                    recordCount += 1;
                }
            }
            lblSelectCount.Text = "共选择" + recordCount + "条记录";
        }

        private bool isExistsDtl(ResourceRequirePlanDetail dtl)
        {
            foreach (DataGridViewRow row in gridPlanDetail.Rows)
            {
                ResourceRequirePlanDetail dtlTemp = row.Tag as ResourceRequirePlanDetail;
                if (dtlTemp.Id == dtl.Id)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddResourceRequireDetailInGrid(ResourceRequirePlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridPlanDetail.Rows.Add();
            DataGridViewRow row = gridPlanDetail.Rows[index];

            row.Cells[TaskName.Name].Value = dtl.TheGWBSTaskName;
            row.Cells[TaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);

            row.Cells[State.Name].Value = dtl.State;

            row.Cells[ResourceType.Name].Value = dtl.BuildResourceTypeName;
            row.Cells[ServiceType.Name].Value = dtl.ServiceType;

            row.Cells[ResourceName.Name].Value = dtl.MaterialName;
            row.Cells[ResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[ResourceQuality.Name].Value = dtl.MaterialStuff;
            row.Cells[Spec.Name].Value = dtl.MaterialSpec;

            row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;

            if (dtl.FirstOfferRequireQuantity != 0)
                row.Cells[FirstOfferQuantity.Name].Value = dtl.FirstOfferRequireQuantity.ToString();
            if (dtl.ResponsibilityRequireQuantity != 0)
                row.Cells[ResponsibilityQuantity.Name].Value = dtl.ResponsibilityRequireQuantity.ToString();
            if (dtl.PlanRequireQuantity != 0)
                row.Cells[PlanQuantity.Name].Value = dtl.PlanRequireQuantity.ToString();
            if (dtl.MonthPlanPublishQuantity != 0)
                row.Cells[MonthlyPlanQuantity.Name].Value = dtl.MonthPlanPublishQuantity.ToString();
            if (dtl.ApproachPlanPublishQuantity != 0)
                row.Cells[ApproachPlanQuantity.Name].Value = dtl.ApproachPlanPublishQuantity.ToString();
            if (dtl.ApproachPlanExecuteQuantity != 0)
                row.Cells[ApproachExecQuantity.Name].Value = dtl.ApproachPlanExecuteQuantity.ToString();

            if (dtl.PlanBeginApproachDate != null)
                row.Cells[PlanStartTime.Name].Value = dtl.PlanBeginApproachDate.Value;
            else
                row.Cells[PlanStartTime.Name].Value = model.GetServerTime().Date;

            if (dtl.PlanEndApproachDate != null)
                row.Cells[PlanEndTime.Name].Value = dtl.PlanEndApproachDate.Value;
            else
                row.Cells[PlanEndTime.Name].Value = model.GetServerTime().Date;

            row.Cells[CreateTime.Name].Value = dtl.CreateTime.ToString();

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridPlanDetail.CurrentCell = row.Cells[0];
        }
        private void AddResourceRequireDetailInSelectGrid(ResourceRequirePlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            //int index = gridSelectPlanDetail.Rows.Add();
            //DataGridViewRow row = gridSelectPlanDetail.Rows[index];

            //row.Cells[colSelectBySelect.Name].Value = true;

            //row.Cells[SelectTaskName.Name].Value = dtl.TheGWBSTaskName;
            //row.Cells[SelectTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);

            //row.Cells[SelectState.Name].Value = dtl.State;

            //row.Cells[SelectResourceType.Name].Value = dtl.BuildResourceTypeName;
            //row.Cells[SelectServiceType.Name].Value = dtl.ServiceType;

            //row.Cells[SelectResourceName.Name].Value = dtl.MaterialName;
            //row.Cells[SelectResourceCode.Name].Value = dtl.MaterialCode;
            //row.Cells[SelectResourceQuality.Name].Value = dtl.MaterialQuality;
            //row.Cells[SelectSpec.Name].Value = dtl.MaterialSpec;

            //if (dtl.FirstOfferRequireQuantity != 0)
            //    row.Cells[SelectFirstOfferQuantity.Name].Value = dtl.FirstOfferRequireQuantity.ToString();
            //if (dtl.ResponsibilityRequireQuantity != 0)
            //    row.Cells[SelectResponsibilityQuantity.Name].Value = dtl.ResponsibilityRequireQuantity.ToString();
            //if (dtl.PlanRequireQuantity != 0)
            //    row.Cells[SelectPlanQuantity.Name].Value = dtl.PlanRequireQuantity.ToString();
            //if (dtl.MonthPlanPublishQuantity != 0)
            //    row.Cells[SelectMonthlyPlanQuantity.Name].Value = dtl.MonthPlanPublishQuantity.ToString();
            //if (dtl.ApproachPlanPublishQuantity != 0)
            //    row.Cells[SelectApproachPlanQuantity.Name].Value = dtl.ApproachPlanPublishQuantity.ToString();
            //if (dtl.ApproachPlanExecuteQuantity != 0)
            //    row.Cells[SelectApproachExecQuantity.Name].Value = dtl.ApproachPlanExecuteQuantity.ToString();

            //if (dtl.PlanBeginApproachDate != null)
            //    row.Cells[SelectPlanStartTime.Name].Value = dtl.PlanBeginApproachDate.Value;
            //else
            //    row.Cells[SelectPlanStartTime.Name].Value = model.GetServerTime().Date;

            //if (dtl.PlanEndApproachDate != null)
            //    row.Cells[SelectPlanEndTime.Name].Value = dtl.PlanEndApproachDate.Value;
            //else
            //    row.Cells[SelectPlanEndTime.Name].Value = model.GetServerTime().Date;

            //row.Cells[SelectCreateTime.Name].Value = dtl.CreateTime.ToString();

            //row.Tag = dtl;

            //row.ReadOnly = isReadOnly;

            //if (isSetCurrentCell)
            //    gridSelectPlanDetail.CurrentCell = row.Cells[0];
        }

        public void Start()
        {
            RefreshState(MainViewState.Browser);
        }

        public override void RefreshControls(MainViewState state)
        {
            switch (state)
            {
                case MainViewState.Modify:

                    break;

                case MainViewState.Browser:

                    break;
            }
        }
    }
}
