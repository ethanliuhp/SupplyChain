using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VAddUnplannedResourceDemand : TBasicDataView
    {
        MRollingDemandPlan model = new MRollingDemandPlan();
        private CurrentProjectInfo projectInfo = null;

        /// <summary>
        /// 操作{滚动资源需求计划}明细
        /// </summary>
        private ResourceRequirePlanDetail opResourceRequirePlanDetail;
        private ResourceRequirePlan operationResourceRequirePlan;
        public PlanRequireType typeIsInOrOut;//计划内或计划外
        public GWBSTree rootNode;//根节点（范围）
        private ResourceRequirePlanDetail result;
        private List<ResourceRequirePlanDetail> ResourceRequirePlanDetailList;

        public ResourceRequirePlanDetail Result
        {
            get { return result; }
            set { result = value; }
        }

        private GWBSTree wbs;
        private Material mat;
        private string diagramNumber = string.Empty;
        //private StandardUnit su;


        public VAddUnplannedResourceDemand(ResourceRequirePlanDetail rrpd, List<ResourceRequirePlanDetail> rrpDtlList)
        {
            InitializeComponent();
            //operationResourceRequirePlan = rrp;
            opResourceRequirePlanDetail = rrpd;
            ResourceRequirePlanDetailList = rrpDtlList;
            projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            InitData();
            InitEvent();
        }
        void InitData()
        {
            txtGWBSTree.ReadOnly = true;
            //txtNumberUnits.ReadOnly = true;
            txtResourcesType.ReadOnly = true;
            txtMaterialSpec.ReadOnly = true;
            txtDiagramNumber.Text = opResourceRequirePlanDetail.DiagramNumber;
            diagramNumber = opResourceRequirePlanDetail.DiagramNumber;
            if (opResourceRequirePlanDetail.TheGWBSTaskGUID != null)
            {
                wbs = new GWBSTree();
                wbs.Id = opResourceRequirePlanDetail.TheGWBSTaskGUID.Id;
                wbs.Name = opResourceRequirePlanDetail.TheGWBSTaskName;
                wbs.SysCode = opResourceRequirePlanDetail.TheGWBSSysCode;
                txtGWBSTree.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), opResourceRequirePlanDetail.TheGWBSTaskName, opResourceRequirePlanDetail.TheGWBSSysCode);
                txtGWBSTree.Tag = wbs;
            }
            if (opResourceRequirePlanDetail.MaterialResource != null)
            {
                if (opResourceRequirePlanDetail.Id != null)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", opResourceRequirePlanDetail.MaterialResource.Id));
                    mat = model.ObjectQuery(typeof(Material), oq)[0] as Material;
                }
                else
                {
                    mat = opResourceRequirePlanDetail.MaterialResource;
                }
                txtResourcesType.Text = opResourceRequirePlanDetail.MaterialName;
                txtMaterialSpec.Text = opResourceRequirePlanDetail.MaterialSpec;
                txtResourcesType.Tag = mat;
            }
        }
        void InitEvent()
        {
            btnSelectGWBSTree.Click += new EventHandler(btnSelectGWBSTree_Click);
            btnResourcesType.Click += new EventHandler(btnResourcesType_Click);
            //btnNumberUnits.Click += new EventHandler(btnNumberUnits_Click);

            btnOK.Click += new EventHandler(btnOK_Click);
            btnQuit.Click += new EventHandler(btnQuit_Click);
        }

        void btnQuit_Click(object sender, EventArgs e)
        {
            result = null;
            this.Close();
        }

        //void btnOK_Click(object sender, EventArgs e)
        //{
        //    //ResourceRequirePlanDetail rrpd = new ResourceRequirePlanDetail();
        //    //rrpd.TheResourceRequirePlans

        //    if (!Verify()) return;
        //    ObjectQuery oq = new ObjectQuery();
        //    Disjunction dis = new Disjunction();

        //    oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", operationResourceRequirePlan.Id));
        //    oq.AddCriterion(Expression.Eq("MaterialResource.Id", mat.Id));
        //    if (txtDiagramNumber.Text.Trim() != "")
        //        oq.AddCriterion(Expression.Eq("DiagramNumber", txtDiagramNumber.Text.Trim()));
        //    oq.AddCriterion(Expression.Eq("TheGWBSTaskGUID.Id", wbs.Id));

        //    IList rrpdList = model.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
        //    if (rrpdList != null && rrpdList.Count > 0)
        //    {
        //        ResourceRequirePlanDetail updateRRPD = rrpdList[0] as ResourceRequirePlanDetail;
        //        updateRRPD.PlanOutRequireQuantity += ClientUtil.ToDecimal(txtPlanOutRequireQuantity.Text);
        //        //result = model.SaveOrUpdateResourcePlanDetail(updateRRPD);
        //        result = updateRRPD;
        //    }
        //    else
        //    {
        //        ResourceRequirePlanDetail rrpd = new ResourceRequirePlanDetail();
        //        rrpd.TheResourceRequirePlan = operationResourceRequirePlan;
        //        rrpd.TheProjectGUID = projectInfo.Id;
        //        rrpd.TheProjectName = projectInfo.Name;
        //        rrpd.TheGWBSTaskGUID = wbs;
        //        rrpd.TheGWBSTaskName = wbs.Name;
        //        rrpd.TheGWBSSysCode = wbs.SysCode;

        //        rrpd.MaterialResource = mat;
        //        rrpd.MaterialCode = mat.Code;
        //        rrpd.MaterialName = mat.Name;
        //        rrpd.MaterialStuff = mat.Quality;
        //        rrpd.MaterialSpec = mat.Specification;
        //        if (txtDiagramNumber.Text.Trim() == "")
        //        {
        //            rrpd.DiagramNumber = null;
        //        }
        //        else
        //        {
        //            rrpd.DiagramNumber = txtDiagramNumber.Text;
        //        }

        //        ObjectQuery oq1 = new ObjectQuery();
        //        oq1.AddCriterion(Expression.Eq("Id", mat.Category.Id));
        //        MaterialCategory mc = model.ObjectQuery(typeof(MaterialCategory), oq1)[0] as MaterialCategory;
        //        rrpd.ResourceCategory = mc;
        //        rrpd.ResourceTypeClassification = mc.Name;
        //        //rrpd.Summary = null;
        //        rrpd.State = ResourceRequirePlanDetailState.编制;
        //        rrpd.StateUpdateTime = DateTime.Now;
        //        //rrpd.FirstOfferRequireQuantity = null;
        //        //rrpd.ResponsibilityCostQuantity = cs.ResponsibilitilyWorkAmount;
        //        //rrpd.PlannedCostQuantity = cs.PlanWorkAmount;
        //        //rrpd.PlanInRequireQuantity = rrpd.PlannedCostQuantity;
        //        rrpd.PlanOutRequireQuantity = ClientUtil.ToDecimal(txtPlanOutRequireQuantity.Text);
        //        rrpd.MonthPlanPublishQuantity = 0;
        //        rrpd.DailyPlanPublishQuantity = 0;
        //        rrpd.ExecutedQuantity = 0;
        //        //if (su != null)
        //        //{
        //        //    rrpd.QuantityUnitGUID = su;
        //        //    rrpd.QuantityUnitName = su.Name;
        //        //}
        //        rrpd.QuantityUnitGUID = mat.BasicUnit;
        //        rrpd.QuantityUnitName = mat.BasicUnit.Name;
        //        rrpd.RequireType = PlanRequireType.计划外需求;
        //        rrpd.CreateTime = DateTime.Now;
        //        //result = model.SaveOrUpdateResourcePlanDetail(rrpd);
        //        result = rrpd;
        //    }
        //    this.Close();
        //}

        void btnOK_Click(object sender, EventArgs e)
        {
            if (!Verify()) return;

            ResourceRequirePlanDetail rrpd = new ResourceRequirePlanDetail();
            //rrpd.TheResourceRequirePlan = operationResourceRequirePlan;
            rrpd.TheProjectGUID = projectInfo.Id;
            rrpd.TheProjectName = projectInfo.Name;
            if (opResourceRequirePlanDetail.TheGWBSTaskGUID != null)
            {
                if (wbs.Id == opResourceRequirePlanDetail.TheGWBSTaskGUID.Id)
                {
                    rrpd.TheGWBSTaskGUID = opResourceRequirePlanDetail.TheGWBSTaskGUID;
                    rrpd.TheGWBSTaskName = opResourceRequirePlanDetail.TheGWBSTaskName;
                    rrpd.TheGWBSSysCode = opResourceRequirePlanDetail.TheGWBSSysCode;
                }
            }
            else
            {
                rrpd.TheGWBSTaskGUID = wbs;
                rrpd.TheGWBSTaskName = wbs.Name;
                rrpd.TheGWBSSysCode = wbs.SysCode;
            }

            rrpd.MaterialResource = mat;
            rrpd.MaterialCode = mat.Code;
            rrpd.MaterialName = mat.Name;
            rrpd.MaterialStuff = mat.Quality;
            rrpd.MaterialSpec = mat.Specification;

            if (txtDiagramNumber.Text.Trim() == "")
            {
                rrpd.DiagramNumber = null;
            }
            else
            {
                rrpd.DiagramNumber = txtDiagramNumber.Text;
            }

            ObjectQuery oq1 = new ObjectQuery();
            oq1.AddCriterion(Expression.Eq("Id", mat.Category.Id));
            MaterialCategory mc = model.ObjectQuery(typeof(MaterialCategory), oq1)[0] as MaterialCategory;
            rrpd.ResourceCategory = mc;
            rrpd.ResourceTypeClassification = mc.Name;
            //rrpd.Summary = null;
            rrpd.State = ResourceRequirePlanDetailState.编制;
            rrpd.StateUpdateTime = DateTime.Now;
            //rrpd.FirstOfferRequireQuantity = 0;
            //rrpd.ResponsibilityCostQuantity = cs.ResponsibilitilyWorkAmount;
            //rrpd.PlannedCostQuantity = cs.PlanWorkAmount;
            //rrpd.PlanInRequireQuantity = rrpd.PlannedCostQuantity;
            if (typeIsInOrOut == PlanRequireType.计划内需求)
            {
                rrpd.PlanInRequireQuantity = ClientUtil.ToDecimal(txtPlanOutRequireQuantity.Text);
            }
            else
            {
                rrpd.PlanOutRequireQuantity = ClientUtil.ToDecimal(txtPlanOutRequireQuantity.Text);
            }
            rrpd.MonthPlanPublishQuantity = 0;
            rrpd.DailyPlanPublishQuantity = 0;
            rrpd.ExecutedQuantity = 0;
            //if (su != null)
            //{
            //    rrpd.QuantityUnitGUID = su;
            //    rrpd.QuantityUnitName = su.Name;
            //}
            rrpd.QuantityUnitGUID = mat.BasicUnit;
            rrpd.QuantityUnitName = mat.BasicUnit.Name;
            rrpd.RequireType = typeIsInOrOut;
            rrpd.CreateTime = DateTime.Now;
            //result = model.SaveOrUpdateResourcePlanDetail(rrpd);
            result = rrpd;
            this.Close();
        }

        void btnSelectGWBSTree_Click(object sender, EventArgs e)
        {
            //VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(rootNode);
            frm.IsSelectSingleNode = true;
            frm.ShowDialog();
            List<TreeNode> list = frm.SelectResult;
            if (list.Count > 0)
            {
                wbs = (list[0] as TreeNode).Tag as GWBSTree;
                txtGWBSTree.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), wbs.Name, wbs.SysCode);
                txtGWBSTree.Tag = wbs;
                txtGWBSTree.Focus();
            }
        }

        void btnResourcesType_Click(object sender, EventArgs e)
        {
            CommonMaterial materialSelector = new CommonMaterial();
            materialSelector.OpenSelect();

            IList list = materialSelector.Result;
            if (list.Count > 0)
            {
                mat = list[0] as Material;
                txtResourcesType.Text = mat.Name;
                txtMaterialSpec.Text = mat.Specification;
                txtResourcesType.Tag = mat;
                txtResourcesType.Focus();
            }
        }

        bool Verify()
        {
            if (txtGWBSTree.Tag == null || txtGWBSTree.Text.Trim() == "")
            {
                MessageBox.Show("请选择工程项目任务！");
                return false;
            }
            if (txtResourcesType.Tag == null || txtResourcesType.Text.Trim() == "")
            {
                MessageBox.Show("请选择资源类型！");
                return false;
            }
            if (txtDiagramNumber.Text.Trim() == "")
            {
                diagramNumber = null;
            }
            else
            {
                diagramNumber = txtDiagramNumber.Text;
            }
            if (ResourceRequirePlanDetailList != null && ResourceRequirePlanDetailList.Count > 0)
            {
                foreach (ResourceRequirePlanDetail d in ResourceRequirePlanDetailList)
                {
                    if (d.MaterialResource != null && d.MaterialResource.Id == mat.Id && d.DiagramNumber == diagramNumber && d.RequireType == typeIsInOrOut)
                    {
                        if (d.TheGWBSSysCode.Contains(wbs.SysCode) || wbs.SysCode.Contains(d.TheGWBSSysCode))
                        {
                            MessageBox.Show("选择节点的父节点或子节点提了相同的资源，请重新选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
            }

            try
            {
                decimal num = Convert.ToDecimal(txtPlanOutRequireQuantity.Text);
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("计划需求量数据输入有误！");
                return false;
            }
            //return true;
        }
    }
}
