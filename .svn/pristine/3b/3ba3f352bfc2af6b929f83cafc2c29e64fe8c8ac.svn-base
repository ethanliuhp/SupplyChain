using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.Basic.Template;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using System.Collections;
using VirtualMachine.Component.Util;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Iesi.Collections;
using Iesi.Collections.Generic;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VShowGWBSDetailCostSubject : TBasicDataView
    {
        MRollingDemandPlan model = new MRollingDemandPlan();
        private ResourceRequirePlanDetail opRRPDtl = null;
        private ResourceRequirePlan opRRP = null;

        public VShowGWBSDetailCostSubject(ResourceRequirePlanDetail dtl,ResourceRequirePlan rrp)
        {
            InitializeComponent();
            opRRPDtl = dtl;
            opRRP = rrp;
            gridGWBDetailUsage.ReadOnly = true;
            InitData();
        }

        void InitData()
        {
            LoadData();
        }

        void LoadData()
        {
            ObjectQuery oq = new ObjectQuery();

            oq.AddCriterion(Expression.Eq("TheProjectGUID", Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo().Id));

            if (opRRPDtl.MaterialResource == null)
            {
                oq.AddCriterion(Expression.IsNull("ResourceTypeGUID"));
            }
            else
            {
                oq.AddCriterion(Expression.Eq("ResourceTypeGUID", opRRPDtl.MaterialResource.Id));
            }
            if (string.IsNullOrEmpty(opRRPDtl.DiagramNumber))
            {
                oq.AddCriterion(Expression.IsNull("DiagramNumber"));
            }
            else
            {
                oq.AddCriterion(Expression.Eq("DiagramNumber", opRRPDtl.DiagramNumber));
            }
            oq.AddCriterion(Expression.Like("TheGWBSTreeSyscode", opRRP.TheGWBSTreeSyscode, MatchMode.Start));
            oq.AddCriterion(Expression.Eq("TheGWBSDetail.State", DocumentState.InExecute));
            oq.AddCriterion(Expression.Eq("TheGWBSDetail.CostingFlag", 1));
            IList list = model.ObjectQuery(typeof(GWBSDetailCostSubject), oq);
            gridGWBDetailUsage.Rows.Clear();
            foreach (GWBSDetailCostSubject dtl in list)
            {
                AddUsageDetailInfoInGrid(dtl);
            }
        }


        private void AddUsageDetailInfoInGrid(GWBSDetailCostSubject dtl)
        {
            int index = gridGWBDetailUsage.Rows.Add();
            DataGridViewRow row = gridGWBDetailUsage.Rows[index];
            row.Cells[DataFullPath.Name].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTreeName, dtl.TheGWBSTreeSyscode);
            row.Cells[DtlUsageName.Name].Value = dtl.Name;
            row.Cells[DtlAccountSubject.Name].Value = dtl.CostAccountSubjectName;

            row.Cells[DtlResourceTypeName.Name].Value = dtl.ResourceTypeName;
            row.Cells[DtlResourceTypeSpec.Name].Value = dtl.ResourceTypeSpec;
            row.Cells[DtlResourceTypeQuality.Name].Value = dtl.ResourceTypeQuality;

            row.Cells[DtlMainResourceFlag.Name].Value = dtl.MainResTypeFlag ? "是" : "否";
            row.Cells[DtlQuantityUnit.Name].Value = dtl.ProjectAmountUnitName;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[DtlContractQuotaQuantity.Name].Value = dtl.ContractQuotaQuantity;
            row.Cells[DtlContractBasePrice.Name].Value = dtl.ContractBasePrice;
            row.Cells[DtlContractPricePercent.Name].Value = dtl.ContractPricePercent;
            row.Cells[DtlContractQuantityPrice.Name].Value = dtl.ContractQuantityPrice;
            row.Cells[DtlContractWorkAmountPrice.Name].Value = dtl.ContractPrice;
            row.Cells[DtlContractUsageQuantity.Name].Value = dtl.ContractProjectAmount;
            row.Cells[DtlContractUsageTotal.Name].Value = dtl.ContractTotalPrice;

            row.Cells[DtlResponsibleQuotaQuantity.Name].Value = dtl.ResponsibleQuotaNum;
            row.Cells[DtlResponsibleBasePrice.Name].Value = dtl.ResponsibleBasePrice;
            row.Cells[DtlResponsiblePricePercent.Name].Value = dtl.ResponsiblePricePercent;
            row.Cells[DtlResponsibleQuantityPrice.Name].Value = dtl.ResponsibilitilyPrice;
            row.Cells[DtlResponsibleWorkAmountPrice.Name].Value = dtl.ResponsibleWorkPrice;
            row.Cells[DtlResponsibleUsageQuantity.Name].Value = dtl.ResponsibilitilyWorkAmount;
            row.Cells[DtlResponsibleUsageTotal.Name].Value = dtl.ResponsibilitilyTotalPrice;

            row.Cells[DtlPlanQuotaQuantity.Name].Value = dtl.PlanQuotaNum;
            row.Cells[DtlPlanBasePrice.Name].Value = dtl.PlanBasePrice;
            row.Cells[DtlPlanPricePercent.Name].Value = dtl.PlanPricePercent;
            row.Cells[DtlPlanQuantityPrice.Name].Value = dtl.PlanPrice;
            row.Cells[DtlPlanWorkAmountPrice.Name].Value = dtl.PlanWorkPrice;
            row.Cells[DtlPlanUsageQuantity.Name].Value = dtl.PlanWorkAmount;
            row.Cells[DtlPlanUsageTotal.Name].Value = dtl.PlanTotalPrice;

            row.Cells[colTechnologyParam.Name].Value = dtl.TechnicalParam;

            row.Tag = dtl;

            gridGWBDetailUsage.CurrentCell = row.Cells[0];
        }
       
    }
}