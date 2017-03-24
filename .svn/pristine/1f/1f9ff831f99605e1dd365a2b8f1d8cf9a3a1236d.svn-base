using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI;
using System.Collections;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using VirtualMachine.Core;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.TotalDemandPlan
{
    public partial class VDetailInformation : TBasicDataView
    {
        private MTotalDemandPlanMng model = new MTotalDemandPlanMng();
        ResourceRequirePlan resPlan = new ResourceRequirePlan();
        MaterialCategory material = new MaterialCategory();
        IList lists = new ArrayList();
        public VDetailInformation(IList list)
        {
            InitializeComponent();
            lists = list;
            InitData(list);
            InnerEvent();
        }

        private void InnerEvent()
        {
            this.btnGaveup.Click +=new EventHandler(btnGaveup_Click);
        }

        void btnGaveup_Click(object sender,EventArgs e)
        {
            this.btnGaveup.FindForm().Close();
        }

        private void InitData(IList lists)
        {
            string strId = lists[0].ToString();
            material = lists[1] as MaterialCategory;
            string strTH = lists[2].ToString();
            string strUnit = lists[3].ToString();
            string strGDDemand = lists[4].ToString();
            string strAppDemand = lists[5].ToString();
            string strMonthDemand = lists[6].ToString();
            string strDailyDemand = lists[7].ToString();
            string strTotalDemand = lists[8].ToString();
            ObjectQuery oby = new ObjectQuery();
            oby.AddCriterion(Expression.Eq("Id", strId));
            oby.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList listPlans = model.ResourceRequirePlanSrv.ObjectQuery(typeof(ResourceRequirePlan), oby);
            if (listPlans.Count > 0)
            {
                resPlan = listPlans[0] as ResourceRequirePlan;
            }

            txtMaterialType.Text = ClientUtil.ToString(material.Name);
            txtMonthDemand.Text = strMonthDemand;
            txtDailyDemand.Text = strDailyDemand;
            txtQuantityUnit.Text = strUnit;
            txtSHPDemand.Text = strAppDemand;
            txtTotal.Text = strTotalDemand;
            txtTH.Text = strTH;
            txtGDDemand.Text = strGDDemand;

            //通过滚动需求计划查询滚动明细信息
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheResourceRequirePlan", resPlan));
            oq.AddCriterion(Expression.Eq("State", ResourceRequirePlanDetailState.发布));
            //根据图号资源类型查询条件
            oq.AddCriterion(Expression.Eq("ResourceCategory", material));
            if (strTH != "")
            {
                oq.AddCriterion(Expression.Eq("DiagramNumber", strTH));
            }
            oq.AddFetchMode("TheResourceRequirePlan", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ResourceCategory", NHibernate.FetchMode.Eager);
            IList listPlan = model.ResourceRequirePlanSrv.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
            if (listPlan.Count > 0)
            {
                foreach (ResourceRequirePlanDetail dtlPlan in listPlan)
                {
                    int i = dgMaster.Rows.Add();
                    dgMaster[colGDDaily.Name, i].Value = ClientUtil.ToString(dtlPlan.DailyPlanPublishQuantity);
                    dgMaster[colGDDemand.Name, i].Value = ClientUtil.ToString(dtlPlan.ExecutedQuantity);
                    dgMaster[colGDZHX.Name, i].Value = ClientUtil.ToString(dtlPlan.PlanInRequireQuantity + dtlPlan.PlanOutRequireQuantity);
                    dgMaster[colGDMonth.Name, i].Value = ClientUtil.ToString(dtlPlan.MonthPlanPublishQuantity);
                    dgMaster[colGDGWBS.Name, i].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtlPlan.TheGWBSTaskName, dtlPlan.TheGWBSSysCode);

                }
            }
            //通过滚动需求计划查询总量需求计划明细信息
            ObjectQuery objectquery = new ObjectQuery();
            objectquery.AddCriterion(Expression.Eq("TheResReceipt.ResourceRequirePlan", resPlan));
            objectquery.AddCriterion(Expression.Eq("State", ResourceRequireReceiptDetailState.有效));
            //根据图号资源类型查询条件
            objectquery.AddCriterion(Expression.Eq("ResourceCategory", material));
            if (strTH != "")
            {
                objectquery.AddCriterion(Expression.Eq("DiagramNumber", strTH));
            }
            objectquery.AddFetchMode("TheResReceipt", NHibernate.FetchMode.Eager);
            objectquery.AddFetchMode("TheResReceipt.ResourceRequirePlan", NHibernate.FetchMode.Eager);
            IList listReceipt = model.ResourceRequirePlanSrv.ObjectQuery(typeof(ResourceRequireReceiptDetail), objectquery);
            if (listReceipt.Count > 0)
            {
                foreach (ResourceRequireReceiptDetail dtlReceipt in listReceipt)
                {
                    int i = dgDetail.Rows.Add();
                    dgDetail[colGWBS.Name, i].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtlReceipt.TheGWBSTaskName, dtlReceipt.TheGWBSSysCode);
                    dgDetail[colTotalQuantity.Name, i].Value = ClientUtil.ToString(dtlReceipt.PlanInRequireQuantity + dtlReceipt.PlanOutRequireQuantity);
                    dgDetail[colTotalMaterial.Name, i].Value = ClientUtil.ToString(dtlReceipt.MaterialName);
                }
            }
        }
    }
}
