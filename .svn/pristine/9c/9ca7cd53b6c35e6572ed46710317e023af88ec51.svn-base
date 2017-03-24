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
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.ResourceManager.Client.Main;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonForm;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VSelectResourceDemandPlan : TBasicDataView
    {
        public bool isOK = false;

        private ResourceRequirePlan _SelectPlan = null;
        /// <summary>
        /// 选择的需求计划
        /// </summary>
        public ResourceRequirePlan SelectPlan
        {
            get { return _SelectPlan; }
            set { _SelectPlan = value; }
        }

        private ResourceRequirePlan optPlan = null;

        private CurrentProjectInfo projectInfo = null;

        public ResourceRequirePlanType planType = ResourceRequirePlanType.总体需求计划;

        public MRollingDemandPlan model = new MRollingDemandPlan();

        public VSelectResourceDemandPlan()
        {
            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();

            InitEvents();
        }

        private void InitEvents()
        {
            gridPlanMaster.CellClick += new DataGridViewCellEventHandler(gridPlanMaster_CellClick);

            gridPlanMaster.CellDoubleClick += new DataGridViewCellEventHandler(gridPlanMaster_CellDoubleClick);

            this.Load += new EventHandler(VSelectResourceDemandPlan_Load);

            btnEnter.Click += new EventHandler(btnEnter_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void VSelectResourceDemandPlan_Load(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
            oq.AddCriterion(Expression.Eq("PlanType", planType.ToString()));

            IList list = model.ObjectQuery(typeof(ResourceRequirePlan), oq);

            if (list.Count > 0)
            {
                foreach (ResourceRequirePlan plan in list)
                {
                    AddResourceRequirePlanInGrid(plan);
                }
                gridPlanMaster.ClearSelection();
                //gridPlanMaster_CellClick(gridPlanMaster, new DataGridViewCellEventArgs(0, 0));
            }
        }

        void gridPlanMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnEnter_Click(btnEnter, new EventArgs());
        }

        void btnEnter_Click(object sender, EventArgs e)
        {
            if (gridPlanMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一条主资源计划！");
                gridPlanMaster.Focus();
                return;
            }

            SelectPlan = gridPlanMaster.SelectedRows[0].Tag as ResourceRequirePlan;

            isOK = true;
            this.Close();
        }
        void btnCancel_Click(object sender, EventArgs e)
        {
            isOK = false;
            this.Close();
        }

        void gridPlanMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1)
                return;

            ResourceRequirePlan plan = gridPlanMaster.Rows[e.RowIndex].Tag as ResourceRequirePlan;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", plan.Id));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

            IList listPlan = model.ObjectQuery(typeof(ResourceRequirePlan), oq);

            //ISet<ResourceRequirePlanDetail> listDtl = new HashedSet<ResourceRequirePlanDetail>();
            //listDtl.AddAll(listPlanDtl.OfType<ResourceRequirePlanDetail>().ToArray());
            //plan.Details = listDtl;

            plan = listPlan[0] as ResourceRequirePlan;
            gridPlanMaster.Rows[e.RowIndex].Tag = plan;

            gridResourceRequireDetail.Rows.Clear();
            foreach (ResourceRequirePlanDetail dtl in plan.Details)
            {
                AddResourceRequireDetailInGrid(dtl);
            }
        }
        private void AddResourceRequirePlanInGrid(ResourceRequirePlan plan)
        {
            int index = gridPlanMaster.Rows.Add();
            DataGridViewRow row = gridPlanMaster.Rows[index];

            row.Cells[colMasterPlanName.Name].Value = plan.RequirePlanVersion;
            row.Cells[colMasterState.Name].Value = plan.State.ToString();
            row.Cells[colMasterResponsiblePerson.Name].Value = plan.HandlePersonName;
            row.Cells[colMasterCreateTime.Name].Value = plan.CreateDate.ToLongDateString();

            row.Tag = plan;
        }
        private void AddResourceRequireDetailInGrid(ResourceRequirePlanDetail dtl)
        {
            int index = gridResourceRequireDetail.Rows.Add();
            DataGridViewRow row = gridResourceRequireDetail.Rows[index];

            row.Cells[TaskName.Name].Value = dtl.TheGWBSTaskName;
            row.Cells[TaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);

            row.Cells[CreateTime.Name].Value = dtl.CreateTime.ToString();
            row.Cells[State.Name].Value = dtl.State.ToString();

            row.Cells[ResourceType.Name].Value = dtl.BuildResourceTypeName;
            row.Cells[ServiceType.Name].Value = dtl.ServiceType;

            row.Cells[ResourceName.Name].Value = dtl.MaterialName;
            row.Cells[ResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[ResourceQuality.Name].Value = dtl.MaterialStuff;
            row.Cells[Spec.Name].Value = dtl.MaterialSpec;

            row.Cells[PlanQuantity.Name].Value = StaticMethod.DecimelTrimEnd0(dtl.PlanRequireQuantity);
            row.Cells[QuantityUnit.Name].Value = dtl.QuantityUnitName;

        }
    }
}
