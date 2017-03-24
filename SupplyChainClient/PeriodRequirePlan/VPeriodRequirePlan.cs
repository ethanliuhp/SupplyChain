using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VirtualMachine.Component.Util;
using Application.Business.Erp.Secure.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.ResourceManager.Client.Basic.Controls;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.Client.EngineerManage.TargetRespBookMng;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.ClientManagement;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan
{
    public partial class VPeriodRequirePlan : TBasicDataView
    {
        private MPeriodRequirePlanMng model = new MPeriodRequirePlanMng();
        private ResourceRequirePlan ResPlan = new ResourceRequirePlan();
        MAppPlatMng appModel = new MAppPlatMng();

        int OprCode = 0;
        private ResourceRequireReceipt resReceipt;

        /// <summary>
        /// 期间需求计划单明细
        /// </summary>
        public ResourceRequireReceipt ResReceipt
        {
            get { return resReceipt; }
            set { resReceipt = value; }
        }

        public VPeriodRequirePlan(int OprCode, ResourceRequirePlan ResourcePlan, ResourceRequireReceipt ResRec)
        {
            InitializeComponent();
            this.OprCode = OprCode;
            this.ResPlan = ResourcePlan;
            GetResReceipt(ResRec);
            InitData();
            InitEvent();
        }

        public void GetResReceipt(ResourceRequireReceipt resReceipt)
        {
            if (resReceipt.Id != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", resReceipt.Id));
                oq.AddFetchMode("ResourceCategory", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("OperOrgInfo", NHibernate.FetchMode.Eager);
                oq.AddFetchMode("SchedulingProduction", NHibernate.FetchMode.Eager);

                resReceipt = model.ResourceRequirePlanSrv.GetResourceRequireReceipt(oq);
                ResReceipt = resReceipt;
            }
        }

        public void InitData()
        {
            this.txtOperatioOrgName.Tag = ConstObject.TheLogin.TheAccountOrgInfo;
            if (this.txtOperatioOrgName.Tag != null)
            {
                this.txtOperatioOrgName.Text = ConstObject.TheLogin.TheAccountOrgInfo.Name;
            }
            string[] lockCols = new string[] { colProjectTaskPath.Name, colRequireType.Name, colMaterialType.Name, colMaterialSpec.Name, colArmourDemand.Name, colSumDemand.Name, colQuantityUnit.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
            this.txtRollingRequirePlan.ReadOnly = true;

            IList lists = StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ResourceRequirePlanTypeCate);
            foreach (BasicDataOptr b in lists)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = b.BasicName;
                li.Value = b.BasicCode;
                if (li.Value.Substring(0, 1) == "2" && li.Value.Substring(2, 1) != "0")
                {
                    cbPlanType.Items.Add(li);
                }
            }

            foreach (string state in Enum.GetNames(typeof(ResourceRequirePlanState)))
            {
                cbDocState.Items.Add(state);
            }
            if (ResPlan != null)
            {
                this.txtRollingRequirePlan.Text = ResPlan.RequirePlanVersion;
                this.txtRollingRequirePlan.Tag = ResPlan;
            }
            //新增
            if (OprCode == 1)
            {
                this.txtOperatioOrgName.ReadOnly = true;
                this.cbPlanInner.Checked = true;
                this.cbPlanOuter.Checked = true;
                this.rbAllMaterial.Enabled = false;
                this.txtOwnerName.ReadOnly = true;
                this.cbDocState.Enabled = false;
                this.dtpCreateDate.Enabled = false;
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                this.ResReceipt = new ResourceRequireReceipt();
                ResReceipt.ProjectId = projectInfo.Id;
                ResReceipt.ProjectName = projectInfo.Name;
                ResReceipt.HandlePerson = ConstObject.LoginPersonInfo;
                ResReceipt.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                ResReceipt.State = ResourceRequirePlanState.制定;
                ResReceipt.CreateDate = ConstObject.LoginDate;
                if (this.txtOperatioOrgName.Tag != null)
                {
                    ResReceipt.OwnerOrgSysCode = ConstObject.TheLogin.TheAccountOrgInfo.SysCode;
                }
            }
            //修改
            if (OprCode == 2)
            {
                this.txtOperatioOrgName.ReadOnly = false;
                this.txtOperatioOrgName.Tag = ResReceipt.OpgOrgInfo;
                this.txtOperatioOrgName.Text = ResReceipt.OpgOrgInfoName;
                this.txtScheduleMaster.Text = ResReceipt.SchedulingProductionName;
                this.txtScheduleMaster.Tag = ResReceipt.SchedulingProduction;
                this.txtPlanName.Text = ResReceipt.ReceiptName;
                this.cbPlanType.Text = ResReceipt.ResourceRequirePlanTypeWord;
                if (ResReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "1")
                {
                    this.colEnterDate.Visible = true;
                    this.colUsedRank.Visible = true;
                    this.colDailyPlanPublishQuantity.Visible = false;
                }
                if (ResReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "2")
                {
                    this.colCostQuantity.Visible = true;
                    this.colUsedRank.Visible = true;
                }
                if (ResReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "3")
                {
                    this.colPrice.Visible = true;
                }
                this.dtpPlanDateBegin.Value = ResReceipt.PlanRequireDateBegin;
                this.dtpPlanDateEnd.Value = ResReceipt.PlanRequireDateEnd;
                txtMaterialCategory.Result.Clear();
                txtMaterialCategory.Value = ResReceipt.ResourceCategory.Name;
                txtMaterialCategory.Result.Add(ResReceipt.ResourceCategory);

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheResReceipt", ResReceipt));
                IList alists = model.ResourceRequirePlanSrv.ObjectQuery(oq);
                dgDetail.Rows.Clear();
                if (alists != null && alists.Count > 0)
                {
                    this.dgDetail.Rows.Clear();
                    foreach (ResourceRequireReceiptDetail rd in alists)
                    {
                        int index = this.dgDetail.Rows.Add();
                        if (rd.RequireType == PlanRequireType.计划内需求)
                        {
                            this.cbPlanInner.Checked = true;
                        }
                        if (rd.RequireType == PlanRequireType.计划外需求)
                        {
                            this.cbPlanOuter.Checked = true;
                        }
                        this.dgDetail[colSelect.Name, index].Value = true;
                        this.dgDetail[colProjectTaskPath.Name, index].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), rd.TheGWBSTaskName, rd.TheGWBSSysCode);
                        //this.dgDetail[colProjectTaskPath.Name, index].Value = rd.TheGWBSTaskGUID.FullPath;
                        this.dgDetail[colProjectTaskPath.Name, index].Tag = rd.TheGWBSTaskGUID;
                        this.dgDetail[colMaterialType.Name, index].Tag = rd.MaterialResource;
                        this.dgDetail[colMaterialType.Name, index].Value = rd.MaterialName;
                        this.dgDetail[colMaterialSpec.Name, index].Value = rd.MaterialSpec;
                        this.dgDetail[colArmourDemand.Name, index].Value = rd.DiagramNumber;
                        this.dgDetail[colDocState.Name, index].Value = EnumUtil<ResourceRequireReceiptDetailState>.GetDescription(rd.State);
                        this.dgDetail[colQuantityUnit.Name, index].Tag = rd.QuantityUnitGUID;
                        this.dgDetail[colQuantityUnit.Name, index].Value = rd.QuantityUnitName;
                        this.dgDetail[colPeriodQuantity.Name, index].Value = rd.PeriodQuantity;
                        this.dgDetail[colDailyPlanPublishQuantity.Name, index].Value = rd.DailyPlanPublishQuantity;
                        this.dgDetail[colCostQuantity.Name, index].Value = rd.CostQuantity;
                        this.dgDetail[colInnerDemand.Name, index].Value = rd.PlanInRequireQuantity;
                        this.dgDetail[colOuterDemand.Name, index].Value = rd.PlanOutRequireQuantity;
                        this.dgDetail[colSumDemand.Name, index].Value = rd.PlanInRequireQuantity + rd.PlanOutRequireQuantity;
                        this.dgDetail[colApproachRequestDesc.Name, index].Value = rd.ApproachRequestDesc;
                        this.dgDetail[colRequireType.Name, index].Value = EnumUtil<PlanRequireType>.GetDescription(rd.RequireType);
                        this.dgDetail[colEnterDate.Name, index].Value = rd.ApproachDate;
                        this.dgDetail[colUsedRank.Name, index].Tag = rd.UsedRank;
                        this.dgDetail[colUsedRank.Name, index].Value = rd.UsedRankName;
                        this.dgDetail[colPrice.Name, index].Value = rd.Price;
                        this.dgDetail.Rows[index].Tag = rd;
                    }
                }
            }
            //显示
            if (OprCode == 3)
            {
                Enable();
                this.txtOperatioOrgName.Tag = ResReceipt.OpgOrgInfo;
                this.txtOperatioOrgName.Text = ResReceipt.OpgOrgInfoName;
                this.txtScheduleMaster.Text = ResReceipt.SchedulingProductionName;
                this.txtScheduleMaster.Tag = ResReceipt.SchedulingProduction;
                this.txtPlanName.Text = ResReceipt.ReceiptName;
                this.cbPlanType.Text = ResReceipt.ResourceRequirePlanTypeWord;
                if (ResReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "1")
                {
                    this.colEnterDate.Visible = true;
                    this.colUsedRank.Visible = true;
                    this.colDailyPlanPublishQuantity.Visible = false;
                }
                if (ResReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "2")
                {
                    this.colCostQuantity.Visible = true;
                    this.colUsedRank.Visible = true;

                }
                if (ResReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "3")
                {
                    this.colPrice.Visible = true;
                }
                this.dtpPlanDateBegin.Value = ResReceipt.PlanRequireDateBegin;
                this.dtpPlanDateEnd.Value = ResReceipt.PlanRequireDateEnd;
                txtMaterialCategory.Result.Clear();
                txtMaterialCategory.Value = ResReceipt.ResourceCategory.Name;
                txtMaterialCategory.Result.Add(ResReceipt.ResourceCategory);

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheResReceipt", ResReceipt));
                IList alists = model.ResourceRequirePlanSrv.ObjectQuery(oq);
                if (alists != null && alists.Count > 0)
                {
                    this.dgDetail.Rows.Clear();
                    foreach (ResourceRequireReceiptDetail rd in alists)
                    {
                        int index = this.dgDetail.Rows.Add();
                        if (rd.RequireType == PlanRequireType.计划内需求)
                        {
                            this.cbPlanInner.Checked = true;
                        }
                        if (rd.RequireType == PlanRequireType.计划外需求)
                        {
                            this.cbPlanOuter.Checked = true;
                        }
                        this.dgDetail[colSelect.Name, index].Value = true;
                        this.dgDetail[colProjectTaskPath.Name, index].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), rd.TheGWBSTaskName, rd.TheGWBSSysCode);
                        //this.dgDetail[colProjectTaskPath.Name, index].Value = rd.TheGWBSTaskGUID.FullPath;
                        this.dgDetail[colProjectTaskPath.Name, index].Tag = rd.TheGWBSTaskGUID;
                        this.dgDetail[colMaterialType.Name, index].Tag = rd.MaterialResource;
                        this.dgDetail[colMaterialType.Name, index].Value = rd.MaterialName;
                        this.dgDetail[colMaterialSpec.Name, index].Value = rd.MaterialSpec;
                        this.dgDetail[colArmourDemand.Name, index].Value = rd.DiagramNumber;
                        this.dgDetail[colDocState.Name, index].Value = EnumUtil<ResourceRequireReceiptDetailState>.GetDescription(rd.State);
                        this.dgDetail[colQuantityUnit.Name, index].Value = rd.QuantityUnitName;
                        this.dgDetail[colPeriodQuantity.Name, index].Value = rd.PeriodQuantity;
                        this.dgDetail[colDailyPlanPublishQuantity.Name, index].Value = rd.DailyPlanPublishQuantity;
                        this.dgDetail[colCostQuantity.Name, index].Value = rd.CostQuantity;
                        this.dgDetail[colInnerDemand.Name, index].Value = rd.PlanInRequireQuantity;
                        this.dgDetail[colOuterDemand.Name, index].Value = rd.PlanOutRequireQuantity;
                        this.dgDetail[colSumDemand.Name, index].Value = rd.PlanInRequireQuantity + rd.PlanOutRequireQuantity;
                        this.dgDetail[colApproachRequestDesc.Name, index].Value = rd.ApproachRequestDesc;
                        this.dgDetail[colRequireType.Name, index].Value = EnumUtil<PlanRequireType>.GetDescription(rd.RequireType);
                        this.dgDetail[colEnterDate.Name, index].Value = rd.ApproachDate;
                        this.dgDetail[colUsedRank.Name, index].Tag = rd.UsedRank;
                        this.dgDetail[colUsedRank.Name, index].Value = rd.UsedRankName;
                        this.dgDetail.Rows[index].Tag = rd;
                        this.dgDetail[colPrice.Name, index].Value = rd.Price;
                    }
                }

            }
            this.cbDocState.Text = EnumUtil<ResourceRequirePlanState>.GetDescription(ResReceipt.State);
            this.dtpCreateDate.Value = ResReceipt.CreateDate;
            this.txtOwnerName.Value = ResReceipt.HandlePersonName;
            this.txtOwnerName.Tag = ResReceipt.HandlePerson;
        }

        public void Enable()
        {
            this.btnOperatioOrg.Enabled = false;
            this.btnSelect.Enabled = false;
            this.btnSelectProjectTask.Enabled = false;
            this.btnResourceFilter.Enabled = false;
            this.btnDelete.Enabled = false;
            this.btnSave.Enabled = false;
            this.btnSubmit.Enabled = false;
            this.dgDetail.ReadOnly = true;
            this.cbPlanType.Enabled = false;
            this.txtPlanName.ReadOnly = true;
            this.dtpCreateDate.Enabled = false;
            this.dtpPlanDateBegin.Enabled = false;
            this.dtpPlanDateEnd.Enabled = false;
            this.txtScheduleMaster.ReadOnly = true;
            this.txtMaterialCategory.ReadOnly = true;
            this.txtOperatioOrgName.ReadOnly = true;
            this.txtOwnerName.ReadOnly = true;
            this.cbDocState.Enabled = false;
            this.cbPlanInner.Enabled = false;
            this.cbPlanOuter.Enabled = false;
            this.btnSupplyResourcePlan.Enabled = false;
        }

        public void InitEvent()
        {
            this.btnSelectProjectTask.Click += new EventHandler(btnSelectProjectTask_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnGiveUp.Click += new EventHandler(btnGiveUp_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
            this.btnResourceFilter.Click += new EventHandler(btnResourceFilter_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.btnSelect.Click += new EventHandler(btnSelect_Click);
            this.btnOperatioOrg.Click += new EventHandler(btnOperatioOrg_Click);
            this.cbPlanType.SelectedIndexChanged += new EventHandler(cbPlanType_SelectedIndexChanged);
            this.lnkAll.Click += new EventHandler(lnkAll_Click);
            this.lnkNone.Click += new EventHandler(lnkNone_Click);
            this.dgDetail.MouseClick += new MouseEventHandler(dgDetail_MouseClick);
            this.dgDetail.CellMouseClick += new DataGridViewCellMouseEventHandler(dgDetail_CellMouseClick);
            this.mmuPeriodResoucePlan.ItemClicked += new ToolStripItemClickedEventHandler(mmuPeriodResoucePlan_ItemClicked);
            this.tabControl1.SelectedIndexChanged += new EventHandler(tabControl1_SelectedIndexChanged);
            this.rbAllMaterial.Click += new EventHandler(rbAllMaterial_Click);
            this.btnSupplyResourcePlan.Click += new EventHandler(btnSupplyResourcePlan_Click);
            this.dtpPlanDateBegin.CloseUp += new EventHandler(dtpPlanDateBegin_CloseUp);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
        }

        void dtpPlanDateBegin_CloseUp(object sender, EventArgs e)
        {
            System.Web.UI.WebControls.ListItem li = cbPlanType.SelectedItem as System.Web.UI.WebControls.ListItem;
            string strCode = li.Value;
            if (strCode.Substring(2, 1) == "1")
            {
                DateTime startTime = this.dtpPlanDateBegin.Value;
                if (this.dtpPlanDateEnd.Value < startTime)
                {
                    this.dtpPlanDateEnd.Value = startTime;
                }
            }
            if (strCode.Substring(2, 1) == "2")
            {
                DateTime startTime = this.dtpPlanDateBegin.Value;
                this.dtpPlanDateEnd.Value = dtpPlanDateBegin.Value.AddMonths(1);
            }
        }

        void btnSupplyResourcePlan_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (ResReceipt.Id != null)
            //    {
            //        if ((bool)model.GenerateSupplyResourcePlan(ResReceipt.Id))
            //        {
            //            MessageBox.Show("物资需求计划生成成功！");
            //        }
            //        else
            //        {
            //            if (ResReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "3")
            //            {
            //                MessageBox.Show("所提的工程任务或该任务的父节点上专业分类为空，无法生成！");
            //            }
            //            else
            //            {
            //                MessageBox.Show("生成失败！");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("期间需求计划没有保存，没有要生成的信息！");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("" + ex);
            //}
        }

        void rbAllMaterial_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow var in dgDetail.Rows)
            {
                var.Visible = true;
            }
        }

        void btnResourceFilter_Click(object sender, EventArgs e)
        {
            VMaterialType material = new VMaterialType();
            material.ShowDialog();
            IList lists = material.Result;
            if (lists == null || lists.Count == 0) return;
            Material thematerial = lists[0] as Material;
            foreach (DataGridViewRow var in dgDetail.Rows)
            {
                Material MaterialResource = var.Cells[colMaterialType.Name].Tag as Material;
                if (thematerial.Id != MaterialResource.Id && thematerial.Specification != MaterialResource.Specification)
                {
                    var.Visible = false;
                }
            }
        }

        void dgDetail_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mmuPeriodResoucePlan.Items[HBToolStripMenuItem.Name].Enabled = false;
                mmuPeriodResoucePlan.Items[CopyToolStripMenuItem.Name].Enabled = false;
                mmuPeriodResoucePlan.Items[StickToolStripMenuItem.Name].Enabled = false;
                mmuPeriodResoucePlan.Show(MousePosition.X, MousePosition.Y);
            }
        }

        void dgDetail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    if (dgDetail.Rows[e.RowIndex].Selected == false)
                    {
                        dgDetail.ClearSelection();
                        dgDetail.Rows[e.RowIndex].Selected = true;
                    }
                    mmuPeriodResoucePlan.Items[HBToolStripMenuItem.Name].Enabled = true;
                    mmuPeriodResoucePlan.Items[CopyToolStripMenuItem.Name].Enabled = true;
                    mmuPeriodResoucePlan.Items[StickToolStripMenuItem.Name].Enabled = true;
                    mmuPeriodResoucePlan.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }
        List<ResourceRequireReceiptDetail> dtlList = new List<ResourceRequireReceiptDetail>();
        void mmuPeriodResoucePlan_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim().Equals(HBToolStripMenuItem.Text))
            {
                mmuPeriodResoucePlan.Hide();
                MergerResourceRequireRDtl();
            }
            if (e.ClickedItem.Text.Trim().Equals(CopyToolStripMenuItem.Text))
            {
                dtlList.Clear();
                mmuPeriodResoucePlan.Hide();
                foreach (DataGridViewRow var in dgDetail.SelectedRows)
                {
                    ResourceRequireReceiptDetail dtl = var.Tag as ResourceRequireReceiptDetail;
                    dtlList.Add(dtl);
                }
            }
            if (e.ClickedItem.Text.Trim().Equals(StickToolStripMenuItem.Text))
            {
                mmuPeriodResoucePlan.Hide();
                foreach (ResourceRequireReceiptDetail dtl in dtlList)
                {
                    int i = dgDetail.Rows.Add();
                    this.dgDetail[colSelect.Name, i].Value = true;
                    this.dgDetail[colProjectTaskPath.Name, i].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);
                    //this.dgDetail[colProjectTaskPath.Name, i].Value = dtl.TheGWBSTaskGUID.FullPath;
                    this.dgDetail[colProjectTaskPath.Name, i].Tag = dtl.TheGWBSTaskGUID;
                    this.dgDetail[colMaterialType.Name, i].Tag = dtl.MaterialResource;
                    this.dgDetail[colMaterialType.Name, i].Value = dtl.MaterialName;
                    this.dgDetail[colMaterialSpec.Name, i].Value = dtl.MaterialSpec;
                    this.dgDetail[colArmourDemand.Name, i].Value = dtl.DiagramNumber;
                    this.dgDetail[colDocState.Name, i].Value = EnumUtil<ResourceRequireReceiptDetailState>.GetDescription(dtl.State);
                    this.dgDetail[colQuantityUnit.Name, i].Value = dtl.QuantityUnitName;
                    this.dgDetail[colPeriodQuantity.Name, i].Value = dtl.PeriodQuantity;
                    this.dgDetail[colDailyPlanPublishQuantity.Name, i].Value = dtl.DailyPlanPublishQuantity;
                    this.dgDetail[colCostQuantity.Name, i].Value = dtl.CostQuantity;
                    this.dgDetail[colInnerDemand.Name, i].Value = dtl.PlanInRequireQuantity;
                    this.dgDetail[colOuterDemand.Name, i].Value = dtl.PlanOutRequireQuantity;
                    this.dgDetail[colSumDemand.Name, i].Value = dtl.PlanInRequireQuantity + dtl.PlanOutRequireQuantity;
                    this.dgDetail[colApproachRequestDesc.Name, i].Value = dtl.ApproachRequestDesc;
                    this.dgDetail[colRequireType.Name, i].Value = EnumUtil<PlanRequireType>.GetDescription(dtl.RequireType);
                    this.dgDetail[colEnterDate.Name, i].Value = dtl.ApproachDate;
                    this.dgDetail[colUsedRank.Name, i].Tag = dtl.UsedRank;
                    this.dgDetail[colUsedRank.Name, i].Value = dtl.UsedRankName;
                    this.dgDetail[colPrice.Name, i].Value = dtl.Price;
                    //dtl.Id = null;
                    //this.dgDetail.Rows[i].Tag = dtl;
                }
            }
        }

        /// <summary>
        /// 复制信息
        /// </summary>
        public void CopyData()
        {

        }

        /// <summary>
        /// 合并需求量
        /// </summary>
        /// <returns></returns>
        public void MergerResourceRequireRDtl()
        {
            try
            {
                List<ResourceRequireReceiptDetail> addDtlList = new List<ResourceRequireReceiptDetail>();
                List<int> cfDtlList = new List<int>();
                List<ResourceRequireReceiptDetail> mergerList = new List<ResourceRequireReceiptDetail>();
                foreach (DataGridViewRow var in dgDetail.SelectedRows)
                {
                    bool flag = true;
                    ResourceRequireReceiptDetail rrdtl = var.Tag as ResourceRequireReceiptDetail;
                    if (mergerList != null && mergerList.Count > 0)
                    {
                        for (int i = 0; i < mergerList.Count; i++)
                        {
                            if (rrdtl.MaterialResource.Id == mergerList[i].MaterialResource.Id && rrdtl.DiagramNumber == mergerList[i].DiagramNumber)
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (flag)
                    {
                        mergerList.Add(rrdtl);
                    }
                }

                foreach (ResourceRequireReceiptDetail rrrDtl in mergerList)
                {
                    ResourceRequireReceiptDetail dtl = new ResourceRequireReceiptDetail();
                    dtl.TheResReceipt = rrrDtl.TheResReceipt;
                    dtl.MaterialResource = rrrDtl.MaterialResource;
                    dtl.MaterialName = rrrDtl.MaterialName;
                    dtl.MaterialCode = rrrDtl.MaterialCode;
                    dtl.MaterialSpec = rrrDtl.MaterialSpec;
                    dtl.MaterialStuff = rrrDtl.MaterialStuff;
                    dtl.QuantityUnitGUID = rrrDtl.QuantityUnitGUID;
                    dtl.QuantityUnitName = rrrDtl.QuantityUnitName;
                    dtl.DiagramNumber = rrrDtl.DiagramNumber;
                    dtl.TheGWBSSysCode = rrrDtl.TheGWBSSysCode;
                    dtl.TheGWBSTaskGUID = rrrDtl.TheGWBSTaskGUID;
                    dtl.TheGWBSTaskName = rrrDtl.TheGWBSTaskName;

                    dtl.PlanInRequireQuantity = 0;
                    dtl.PlanOutRequireQuantity = 0;
                    dtl.PeriodQuantity = 0;
                    dtl.PlannedCostQuantity = 0;
                    dtl.DailyPlanPublishQuantity = 0;
                    dtl.FirstOfferRequireQuantity = 0;
                    dtl.CostQuantity = 0;
                    List<ResourceRequireReceiptDetail> rrrList = new List<ResourceRequireReceiptDetail>();
                    foreach (DataGridViewRow row in dgDetail.Rows)
                    {
                        ResourceRequireReceiptDetail rdtl = row.Tag as ResourceRequireReceiptDetail;

                        if (rdtl.MaterialResource.Id == rrrDtl.MaterialResource.Id && rdtl.DiagramNumber == rrrDtl.DiagramNumber)
                        {
                            dtl.PlanInRequireQuantity += rdtl.PlanInRequireQuantity;
                            dtl.PlanOutRequireQuantity += rdtl.PlanOutRequireQuantity;
                            dtl.PeriodQuantity += rdtl.PeriodQuantity;
                            dtl.PlannedCostQuantity += rdtl.PlannedCostQuantity;
                            dtl.DailyPlanPublishQuantity += rdtl.DailyPlanPublishQuantity;
                            dtl.FirstOfferRequireQuantity += rdtl.FirstOfferRequireQuantity;
                            dtl.CostQuantity += rdtl.CostQuantity;
                            cfDtlList.Add(row.Index);
                            rrrList.Add(rdtl);
                        }
                    }
                    var query = rrrList.OrderBy(p => p.TheGWBSSysCode.Length).ToList<ResourceRequireReceiptDetail>();
                    bool qflag = true;
                    string sysCodeMax = "";
                    string[] sysCodes = query[0].TheGWBSSysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = sysCodes.Length - 1; i >= 0; i--)
                    {
                        qflag = true;
                        string sysCode = "";
                        for (int j = 0; j <= i; j++)
                        {
                            sysCode += sysCodes[j] + ".";
                        }

                        for (int g = 1; g < query.Count; g++)
                        {
                            ResourceRequireReceiptDetail r = query[g];
                            if (!r.TheGWBSSysCode.Contains(sysCode))
                            {
                                qflag = false;
                                break;
                            }
                        }

                        if (qflag)
                        {
                            sysCodeMax = sysCode;
                            break;
                        }
                    }
                    #region
                    //根据资源分类层次码查询资源分类
                    ObjectQuery oqCate = new ObjectQuery();
                    oqCate.AddCriterion(Expression.Eq("SysCode", sysCodeMax));
                    GWBSTree cate = model.ResourceRequirePlanSrv.ObjectQuery(typeof(GWBSTree), oqCate)[0] as GWBSTree;
                    dtl.TheGWBSSysCode = cate.SysCode;
                    dtl.TheGWBSTaskGUID = cate;
                    dtl.TheGWBSTaskName = cate.Name;
                    addDtlList.Add(dtl);
                    #endregion
                }

                cfDtlList.Sort();
                int num = 0;
                foreach (int index in cfDtlList)
                {
                    dgDetail.Rows.RemoveAt(index - num);
                    num++;
                }
                foreach (ResourceRequireReceiptDetail curBillDtl in addDtlList)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colSelect.Name, i].Value = true;
                    this.dgDetail[colProjectTaskPath.Name, i].Tag = curBillDtl.TheGWBSTaskGUID;
                    this.dgDetail[colProjectTaskPath.Name, i].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), curBillDtl.TheGWBSTaskName, curBillDtl.TheGWBSSysCode);
                    //this.dgDetail[colProjectTaskPath.Name, i].Value = curBillDtl.TheGWBSTaskGUID.FullPath;
                    this.dgDetail[colMaterialType.Name, i].Value = curBillDtl.MaterialName;
                    this.dgDetail[colMaterialType.Name, i].Tag = curBillDtl.MaterialResource;
                    this.dgDetail[colMaterialSpec.Name, i].Value = curBillDtl.MaterialSpec;
                    this.dgDetail[colArmourDemand.Name, i].Value = curBillDtl.DiagramNumber;
                    this.dgDetail[colQuantityUnit.Name, i].Value = curBillDtl.QuantityUnitName;
                    this.dgDetail[colQuantityUnit.Name, i].Tag = curBillDtl.QuantityUnitGUID;
                    this.dgDetail[colInnerDemand.Name, i].Value = curBillDtl.PlanInRequireQuantity;
                    this.dgDetail[colOuterDemand.Name, i].Value = curBillDtl.PlanOutRequireQuantity;
                    this.dgDetail[colDailyPlanPublishQuantity.Name, i].Value = curBillDtl.DailyPlanPublishQuantity;
                    this.dgDetail[colCostQuantity.Name, i].Value = curBillDtl.CostQuantity;
                    this.dgDetail[colSumDemand.Name, i].Value = curBillDtl.PlanInRequireQuantity + curBillDtl.PlanOutRequireQuantity;
                    this.dgDetail[colDocState.Name, i].Value = ResourceRequireReceiptDetailState.有效;
                    this.dgDetail[colPeriodQuantity.Name, i].Value = 0;
                    this.dgDetail[colRequireType.Name, i].Value = EnumUtil<PlanRequireType>.GetDescription(curBillDtl.RequireType);
                    this.dgDetail.Rows[i].Tag = curBillDtl;

                }
            }
            catch (Exception)
            {
                MessageBox.Show("合并失败");
            }

        }

        /// <summary>
        /// 合并前验证
        /// </summary>
        public bool Verify()
        {
            if (dgDetail.SelectedRows.Count < 2)
            {
                MessageBox.Show("合并的数据至少要有两条！");
                return false;
            }
            ResourceRequireReceiptDetail rrrDtl = dgDetail.SelectedRows[0].Tag as ResourceRequireReceiptDetail;
            return true;
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkAll_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count > 0)
            {
                for (int i = 0; i < dgDetail.Rows.Count; i++)
                {
                    dgDetail.Rows[i].Cells[colSelect.Name].Value = true;
                }
            }
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void lnkNone_Click(object sender, EventArgs e)
        {
            if (dgDetail.Rows.Count > 0)
            {
                for (int i = 0; i < dgDetail.Rows.Count; i++)
                {
                    if ((bool)dgDetail.Rows[i].Cells[colSelect.Name].EditedFormattedValue)
                    {
                        dgDetail.Rows[i].Cells[colSelect.Name].Value = false;
                    }
                    else
                    {
                        dgDetail.Rows[i].Cells[colSelect.Name].Value = true;
                    }
                }
            }

        }

        /// <summary>
        /// 计划类型和物资资源分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cbPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            System.Web.UI.WebControls.ListItem li = cbPlanType.SelectedItem as System.Web.UI.WebControls.ListItem;
            string strCode = li.Value;
            txtMaterialCategory.Result.Clear();
            txtMaterialCategory.Value = "";
            Application.Resource.MaterialResource.Domain.MaterialCategory Unit = null;
            ObjectQuery oq = new ObjectQuery();
            if (strCode.Substring(1, 1) == "0")
            {
                oq.AddCriterion(Expression.Eq("CategoryNodeType", NodeType.RootNode));
                this.txtMaterialCategory.rootCatCode = "";
            }
            if (strCode.Substring(1, 1) == "1")
            {
                //劳务
                oq.AddCriterion(Expression.Eq("Code", "03"));
                this.txtMaterialCategory.rootCatCode = "03";
                //this.txtMaterialCategory.rootLevel = "3";
            }
            if (strCode.Substring(1, 1) == "1")
            {
                //劳务
                this.txtMaterialCategory.rootCatCode = "03";
                //this.txtMaterialCategory.rootLevel = "3";
            }
            if (strCode.Substring(1, 1) == "2")
            {
                //资源
                oq.AddCriterion(Expression.Eq("Code", "01"));
                this.txtMaterialCategory.rootCatCode = "01";
                //this.txtMaterialCategory.rootLevel = "3";
            }
            if (strCode.Substring(1, 1) == "3")
            {
                //机械
                oq.AddCriterion(Expression.Eq("Code", "02"));
                this.txtMaterialCategory.rootCatCode = "02";
                //this.txtMaterialCategory.rootLevel = "3";
            }
            IList lists = model.ResourceRequirePlanSrv.ObjectQuery(typeof(Application.Resource.MaterialResource.Domain.MaterialCategory), oq);
            if (lists != null && lists.Count > 0)
            {
                Unit = lists[0] as Application.Resource.MaterialResource.Domain.MaterialCategory;
                ResReceipt.ResourceCategory = Unit;
                ResReceipt.ResourceCategorySysCode = Unit.SysCode;
                txtMaterialCategory.Result.Add(Unit);
                txtMaterialCategory.Value = Unit.Name;
            }

            if (strCode.Substring(2, 1) == "1")
            {
                this.colCostQuantity.Visible = true;
                this.colDailyPlanPublishQuantity.Visible = false;
                this.colEnterDate.Visible = true;
                this.colUsedRank.Visible = true;
                this.colPrice.Visible = false;
                this.dtpPlanDateBegin.Value = DateTime.Now;
                this.dtpPlanDateEnd.Value = this.dtpPlanDateBegin.Value.AddDays(0);

            }
            if (strCode.Substring(2, 1) == "2")
            {
                this.colDailyPlanPublishQuantity.Visible = true;
                this.colCostQuantity.Visible = true;
                this.colEnterDate.Visible = false;
                this.colUsedRank.Visible = true;
                this.colPrice.Visible = false;
                this.dtpPlanDateBegin.Value = DateTime.Now;
                this.dtpPlanDateEnd.Value = this.dtpPlanDateBegin.Value.AddMonths(1);
            }
            if (strCode.Substring(2, 1) == "3")
            {
                this.colCostQuantity.Visible = false;
                this.colDailyPlanPublishQuantity.Visible = false;
                this.colEnterDate.Visible = false;
                this.colUsedRank.Visible = false;
                this.colPrice.Visible = true;
            }

        }

        /// <summary>
        /// 查询参照的进度计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelect_Click(object sender, EventArgs e)
        {
            VScheduleSelector vs = new VScheduleSelector();
            vs.ShowDialog();
            IList li = vs.Result;
            if (li == null || li.Count == 0) return;
            WeekScheduleMaster sm = li[0] as WeekScheduleMaster;
            this.txtScheduleMaster.Text = sm.PlanName;
            this.txtScheduleMaster.Tag = sm;
            dgDetail.Rows.Clear();
        }

        /// <summary>
        /// 查询核算组织
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOperatioOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect();
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", info.Id));
                OperationOrgInfo org = model.ResourceRequirePlanSrv.ObjectQuery(typeof(OperationOrgInfo), oq)[0] as OperationOrgInfo;
                txtOperatioOrgName.Tag = org;
                txtOperatioOrgName.Text = org.Name;

                ResReceipt.OpgOrgInfo = org;
                ResReceipt.OpgOrgInfoName = org.Name;
                ResReceipt.OwnerOrgSysCode = org.SysCode;
            }
            dgDetail.Rows.Clear();
        }

        /// <summary>
        /// 保存前验证
        /// </summary>
        /// <returns></returns>
        public bool ValidView()
        {
            string validMessage = "";
            if (ClientUtil.ToString(this.txtPlanName.Text) == "")
            {
                MessageBox.Show("计划名称不能为空！");
                return false;
            }
            dgDetail.EndEdit();
            dgDetail_CellEndEdit(this.dgDetail, new DataGridViewCellEventArgs(this.dgDetail.CurrentCell.ColumnIndex, this.dgDetail.CurrentRow.Index));
            if (validMessage != "")
            {
                MessageBox.Show(validMessage);
                return false;
            }
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                if ((bool)dr.Cells[colSelect.Name].Value)
                {
                    if (dr.IsNewRow) break;
                    if (ClientUtil.ToDecimal(dr.Cells[colPeriodQuantity.Name].EditedFormattedValue) <= 0)
                    {
                        MessageBox.Show("期间需求量应大于0！");
                        dgDetail.CurrentCell = dr.Cells[colPeriodQuantity.Name];
                        return false;
                    }
                    else if (ClientUtil.ToDecimal(dr.Cells[colPeriodQuantity.Name].EditedFormattedValue) > ClientUtil.ToDecimal(dr.Cells[colSumDemand.Name].EditedFormattedValue))
                    {
                        MessageBox.Show("期间需求量须小于计划需求总量！");
                        dgDetail.CurrentCell = dr.Cells[colPeriodQuantity.Name];
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 导出Excel表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnExcel_Click(object sender, EventArgs e)
        {
            //if (dgDetail.Rows.Count == 0)
            //{
            //    MessageBox.Show("表格中没有要导出的信息！");
            //}
            //else
            //{
            //    bool flag = false;
            //    grid.Rows.Clear();
            //    for (int i = dgDetail.Rows.Count - 1; i >= 0; i--)
            //    {
            //        if ((bool)dgDetail[colSelect.Name, i].Value)
            //        {
            //            string selectValue = dgDetail.Rows[i].Cells["colSelect"].EditedFormattedValue.ToString();
            //            if (selectValue == "True")
            //            {
            //                int j = grid.Rows.Add();
            //                grid[colPath.Name, j].Value = dgDetail[colProjectTaskPath.Name, j].Value;
            //                grid[colMaterial.Name, j].Value = dgDetail[colMaterialType.Name, j].Value;
            //                grid[colSuffer.Name, j].Value = dgDetail[colMaterialSpec.Name, j].Value;
            //                grid[colDiaNum.Name, j].Value = dgDetail[colArmourDemand.Name, j].Value;
            //                grid[colPer.Name, j].Value = dgDetail[colPeriodQuantity.Name, j].Value;
            //                grid[colDW.Name, j].Value = dgDetail[colQuantityUnit.Name, j].Value;
            //                grid[colDaily.Name, j].Value = dgDetail[colDailyPlanPublishQuantity.Name, j].Value;
            //                grid[colCostQ.Name, j].Value = dgDetail[colCostQuantity.Name, j].Value;
            //                grid[colLJ.Name, j].Value = dgDetail[colAccumulative.Name, j].Value;
            //                grid[colNow.Name, j].Value = dgDetail[colPlanProgress.Name, j].Value;
            //                grid[colTotle.Name, j].Value = dgDetail[colSumDemand.Name, j].Value;
            //                grid[colSM.Name, j].Value = dgDetail[colApproachRequestDesc.Name, j].Value;
            //                grid[colZT.Name, j].Value = dgDetail[colDocState.Name, j].Value;
            //                flag = true;
            //            }
            //        }
            //    }
            //    if (!flag)
            //    {
            //        MessageBox.Show("没有选中的信息！");
            //        return;
            //    }
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
            //}
        }

        /// <summary>
        /// 选择工程任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelectProjectTask_Click(object sender, EventArgs e)
        {
            try
            {
                #region 验证
                if (this.txtOperatioOrgName.Text == "")
                {
                    MessageBox.Show("请选择核算组织！");
                    return;
                }
                if (this.cbPlanType.Text == "")
                {
                    MessageBox.Show("请选择计划类型！");
                    return;
                }
                if (txtScheduleMaster.Text == "")
                {
                    txtScheduleMaster.Tag = null;
                }
                ResReceipt.Details.Clear();

                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    ResReceipt.ResourceCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    ResReceipt.ResourceCategorySysCode = (txtMaterialCategory.Result[0] as MaterialCategory).SysCode;
                }
                if (cbPlanType.Text != "")
                {
                    System.Web.UI.WebControls.ListItem li = cbPlanType.SelectedItem as System.Web.UI.WebControls.ListItem;
                    ResReceipt.ResourceRequirePlanTypeCode = li.Value;
                    ResReceipt.ResourceRequirePlanTypeWord = li.Text;
                }
                #endregion

                #region 生成期间需求计划单明细
                if (txtOperatioOrgName.Tag != null)
                {
                    VProjectTaskOption voption = new VProjectTaskOption(txtOperatioOrgName.Tag as OperationOrgInfo, txtScheduleMaster.Tag as WeekScheduleMaster, ResPlan);
                    voption.ShowDialog();

                    List<GWBSTree> listGWBS = new List<GWBSTree>();
                    Hashtable ht = voption.Ht;
                    foreach (TreeNode node in ht.Keys)
                    {
                        listGWBS.Add(node.Tag as GWBSTree);
                    }

                    if (ht.Count == 0) return;
                    int a = 0;
                    if (cbPlanInner.Checked == true) //
                    {
                        a = 1;
                    }
                    if (cbPlanOuter.Checked == true)
                    {
                        a = 2;
                    }
                    if (cbPlanInner.Checked == true && cbPlanOuter.Checked == true)
                    {
                        a = 3;
                    }
                    IList listPeriod = model.GetPeriodResPlanDetail(ResPlan, ResReceipt, listGWBS, a);

                    if (listPeriod.Count > 0)
                    {
                        #region 判断是否有重复数据
                        for (int j = 0; j < listPeriod.Count; j++)
                        {
                            ResourceRequireReceiptDetail dtl = listPeriod[j] as ResourceRequireReceiptDetail;
                            bool deflag = false;
                            foreach (DataGridViewRow var in dgDetail.Rows)
                            {
                                GWBSTree projectTask = var.Cells[colProjectTaskPath.Name].Tag as GWBSTree;
                                Material material = var.Cells[colMaterialType.Name].Tag as Material;
                                string diagramnumber = ClientUtil.ToString(var.Cells[colArmourDemand.Name].Value);
                                string requireType = ClientUtil.ToString(var.Cells[colRequireType.Name].Value);
                                if (diagramnumber == "")
                                {
                                    diagramnumber = null;
                                }
                                if (dtl.TheGWBSTaskGUID.SysCode.Contains(projectTask.SysCode) && material.Id == dtl.MaterialResource.Id && diagramnumber == dtl.DiagramNumber && EnumUtil<PlanRequireType>.GetDescription(dtl.RequireType) == requireType)
                                {
                                    deflag = true;
                                    break;
                                }
                            }
                            if (!deflag)
                            {
                                int i = dgDetail.Rows.Add();
                                this.dgDetail[colSelect.Name, i].Value = true;
                                this.dgDetail[colProjectTaskPath.Name, i].Tag = dtl.TheGWBSTaskGUID;
                                this.dgDetail[colProjectTaskPath.Name, i].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);
                                //this.dgDetail[colProjectTaskPath.Name, i].Value = dtl.TheGWBSTaskGUID.FullPath;
                                this.dgDetail[colMaterialType.Name, i].Value = dtl.MaterialName;
                                this.dgDetail[colMaterialType.Name, i].Tag = dtl.MaterialResource;
                                this.dgDetail[colMaterialSpec.Name, i].Value = dtl.MaterialSpec;
                                this.dgDetail[colArmourDemand.Name, i].Value = dtl.DiagramNumber;
                                this.dgDetail[colQuantityUnit.Name, i].Value = dtl.QuantityUnitName;
                                this.dgDetail[colQuantityUnit.Name, i].Tag = dtl.QuantityUnitGUID;
                                this.dgDetail[colInnerDemand.Name, i].Value = ToDecimailString(dtl.PlanInRequireQuantity);
                                this.dgDetail[colOuterDemand.Name, i].Value = ToDecimailString(dtl.PlanOutRequireQuantity);
                                this.dgDetail[colDailyPlanPublishQuantity.Name, i].Value = ToDecimailString(dtl.DailyPlanPublishQuantity);
                                this.dgDetail[colCostQuantity.Name, i].Value = ToDecimailString(dtl.CostQuantity);
                                this.dgDetail[colSumDemand.Name, i].Value = ToDecimailString(dtl.PlanInRequireQuantity + dtl.PlanOutRequireQuantity);
                                this.dgDetail[colDocState.Name, i].Value = dtl.State;
                                this.dgDetail[colPeriodQuantity.Name, i].Value = 0;
                                this.dgDetail[colRequireType.Name, i].Value = dtl.RequireType;
                                this.dgDetail[colEnterDate.Name, i].Value = DateTime.Now;
                                this.dgDetail[colPrice.Name, i].Value = dtl.Price;
                                this.dgDetail[colUsedRank.Name, i].Tag = dtl.UsedRank;
                                this.dgDetail[colUsedRank.Name, i].Value = dtl.UsedRankName;
                                this.dgDetail.Rows[i].Tag = dtl;
                            }
                        }
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成信息出错。\n" + ExceptionUtil.ExceptionMessage(ex));
                return;
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private string ToDecimailString(decimal value)
        {
            return decimal.Round(value, 5).ToString();
        }

        /// <summary>
        /// 按资源类型查看数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name.Trim().Equals(tabPage2.Name))
            {
                dgCollect.Rows.Clear();
                Hashtable ht = new Hashtable();
                //循环dgDetail
                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if (var.IsNewRow) break;
                    ResourceRequireReceiptDetail rdtl = new ResourceRequireReceiptDetail();
                    rdtl.PeriodQuantity = ClientUtil.ToDecimal(var.Cells[colPeriodQuantity.Name].Value);
                    rdtl.MaterialName = ClientUtil.ToString(var.Cells[colMaterialType.Name].Value);
                    rdtl.DailyPlanPublishQuantity = ClientUtil.ToDecimal(var.Cells[colDailyPlanPublishQuantity.Name].Value);
                    rdtl.DiagramNumber = ClientUtil.ToString(var.Cells[colArmourDemand.Name].Value);
                    rdtl.QuantityUnitName = ClientUtil.ToString(var.Cells[colQuantityUnit.Name].Value);
                    rdtl.PlanInRequireQuantity = ClientUtil.ToDecimal(var.Cells[colInnerDemand.Name].Value);
                    rdtl.PlanOutRequireQuantity = ClientUtil.ToDecimal(var.Cells[colOuterDemand.Name].Value);
                    decimal query = ClientUtil.ToDecimal(var.Cells[colSumDemand.Name].Value);
                    rdtl.MaterialSpec = ClientUtil.ToString(var.Cells[colMaterialSpec.Name].Value);
                    rdtl.MaterialResource = var.Cells[colMaterialType.Name].Tag as Material;
                    if (ht.Count == 0)
                    {
                        ht.Add(rdtl, rdtl.MaterialResource);
                    }
                    else
                    {
                        if (ht.ContainsValue(rdtl.MaterialResource))
                        {
                            foreach (System.Collections.DictionaryEntry objDE in ht)
                            {
                                if (objDE.Value.ToString().Equals(rdtl.MaterialResource))
                                {
                                    ResourceRequireReceiptDetail dt = objDE.Key as ResourceRequireReceiptDetail;
                                    if (dt.DiagramNumber == rdtl.DiagramNumber)
                                    {
                                        dt.PeriodQuantity += rdtl.PeriodQuantity;
                                        dt.PlanOutRequireQuantity += rdtl.PlanOutRequireQuantity;
                                        dt.PlanInRequireQuantity += rdtl.PlanInRequireQuantity;
                                        ht.Remove(dt);
                                        ht.Add(dt, rdtl.MaterialResource);
                                    }
                                    else
                                    {
                                        ht.Add(rdtl, rdtl.MaterialResource);
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ht.Add(rdtl, rdtl.MaterialResource);
                        }
                    }
                }

                foreach (System.Collections.DictionaryEntry objDE in ht)
                {
                    ResourceRequireReceiptDetail curBillDtl = objDE.Key as ResourceRequireReceiptDetail;
                    int i = this.dgCollect.Rows.Add();
                    this.dgCollect[colResourceType.Name, i].Value = curBillDtl.MaterialName;
                    this.dgCollect[colMaterialSpecl.Name, i].Value = curBillDtl.MaterialSpec;
                    this.dgCollect[colDiagramNumber.Name, i].Value = curBillDtl.DiagramNumber;
                    this.dgCollect[colQuantityName.Name, i].Value = curBillDtl.QuantityUnitName;
                    this.dgCollect[colPeriod.Name, i].Value = curBillDtl.PeriodQuantity;
                    this.dgCollect[colDailyQuantity.Name, i].Value = curBillDtl.DailyPlanPublishQuantity;
                    this.dgCollect[colCost.Name, i].Value = curBillDtl.CostQuantity;
                    this.dgCollect[colPlanQuantity.Name, i].Value = ClientUtil.ToString(curBillDtl.PlanInRequireQuantity + curBillDtl.PlanOutRequireQuantity);
                }
            }
        }

        /// <summary>
        /// 放弃或退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnGiveUp_Click(object sender, EventArgs e)
        {
            this.btnGiveUp.FindForm().Close();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="ResReceipt"></param>
        void reSet(ResourceRequireReceipt ResReceipt)
        {

            cbPlanType.Text = ResReceipt.ResourceRequirePlanTypeWord;
            txtPlanName.Text = ResReceipt.ReceiptName;
            txtRollingRequirePlan.Text = ResReceipt.ResourceRequirePlanName;
            txtRollingRequirePlan.Tag = ResReceipt.ResourceRequirePlan;
            cbDocState.Text = EnumUtil<ResourceRequirePlanState>.GetDescription(ResReceipt.State);
            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
            li.Text = ResReceipt.ResourceRequirePlanTypeWord;
            li.Value = ResReceipt.ResourceRequirePlanTypeCode;
            cbPlanType.Items.Add(li);
            this.txtOperatioOrgName.Tag = ResReceipt.OpgOrgInfo;
            this.txtOperatioOrgName.Text = ResReceipt.OpgOrgInfoName;
            txtMaterialCategory.Result.Clear();
            txtMaterialCategory.Value = ResReceipt.ResourceCategory.Name;
            txtMaterialCategory.Result.Add(ResReceipt.ResourceCategory);

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheResReceipt", ResReceipt));
            oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
            IList list = model.ResourceRequirePlanSrv.ObjectQuery(typeof(ResourceRequireReceiptDetail), oq);
            dgDetail.Rows.Clear();
            foreach (ResourceRequireReceiptDetail var in list)
            {
                int index = dgDetail.Rows.Add();
                this.dgDetail[colSelect.Name, index].Value = true;
                this.dgDetail[colProjectTaskPath.Name, index].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), var.TheGWBSTaskName, var.TheGWBSSysCode);
                this.dgDetail[colProjectTaskPath.Name, index].Tag = var.TheGWBSTaskGUID;
                this.dgDetail[colMaterialType.Name, index].Tag = var.MaterialResource;
                this.dgDetail[colMaterialType.Name, index].Value = var.MaterialName;
                this.dgDetail[colMaterialSpec.Name, index].Value = var.MaterialSpec;
                this.dgDetail[colArmourDemand.Name, index].Value = var.DiagramNumber;
                this.dgDetail[colDocState.Name, index].Value = var.State;
                this.dgDetail[colQuantityUnit.Name, index].Tag = var.QuantityUnitGUID;
                this.dgDetail[colQuantityUnit.Name, index].Value = var.QuantityUnitName;
                this.dgDetail[colPeriodQuantity.Name, index].Value = var.PeriodQuantity;
                this.dgDetail[colDailyPlanPublishQuantity.Name, index].Value = var.DailyPlanPublishQuantity;
                this.dgDetail[colCostQuantity.Name, index].Value = var.CostQuantity;
                this.dgDetail[colInnerDemand.Name, index].Value = var.PlanInRequireQuantity;
                this.dgDetail[colOuterDemand.Name, index].Value = var.PlanOutRequireQuantity;
                this.dgDetail[colSumDemand.Name, index].Value = var.PlanInRequireQuantity + var.PlanOutRequireQuantity;
                this.dgDetail[colApproachRequestDesc.Name, index].Value = var.ApproachRequestDesc;
                this.dgDetail[colRequireType.Name, index].Value = var.RequireType;
                this.dgDetail[colEnterDate.Name, index].Value = var.ApproachDate;
                this.dgDetail[colUsedRank.Name, index].Tag = var.UsedRank;
                this.dgDetail[colUsedRank.Name, index].Value = var.UsedRankName;
                this.dgDetail[colPrice.Name, index].Value = var.Price;
                this.dgDetail.Rows[index].Tag = var;
            }
        }


        void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidView()) return;
            if (dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("没有要保存的信息！");
            }
            else
            {
                SaveView("保存");
                ResReceipt = model.SaveResourceReceipt(ResReceipt);
                reSet(ResReceipt);
                MessageBox.Show("保存成功！");
            }
        }



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SaveView(string strState)
        {
            try
            {
                if (strState == "提交")
                {
                    ObjectQuery oq1 = new ObjectQuery();
                    oq1.AddCriterion(Expression.Eq("Id", ResReceipt.Id));
                    oq1.AddFetchMode("ResourceCategory", NHibernate.FetchMode.Eager);
                    IList lst = model.ResourceRequirePlanSrv.ObjectQuery(typeof(ResourceRequireReceipt), oq1);
                    ResReceipt = lst[0] as ResourceRequireReceipt;

                    ResReceipt.State = ResourceRequirePlanState.提交;
                    return;
                }
                if (txtScheduleMaster.Tag != null)
                {
                    ResReceipt.SchedulingProductionName = this.txtScheduleMaster.Text;
                    ResReceipt.SchedulingProduction = this.txtScheduleMaster.Tag as WeekScheduleMaster;
                }

                ResReceipt.ReceiptName = this.txtPlanName.Text;
                System.Web.UI.WebControls.ListItem li = cbPlanType.SelectedItem as System.Web.UI.WebControls.ListItem;
                ResReceipt.ResourceRequirePlanTypeCode = li.Value;
                ResReceipt.ResourceRequirePlanTypeWord = li.Text;
                if (txtOperatioOrgName.Tag != null)
                {
                    ResReceipt.OpgOrgInfo = this.txtOperatioOrgName.Tag as OperationOrgInfo;
                    ResReceipt.OpgOrgInfoName = this.txtOperatioOrgName.Text;
                }

                ResReceipt.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                ResReceipt.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
                ResReceipt.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;

                ResReceipt.ResourceRequirePlan = ResPlan;
                ResReceipt.ResourceRequirePlanName = ResPlan.RequirePlanVersion;
                ResReceipt.PlanRequireDateBegin = this.dtpPlanDateBegin.Value;
                ResReceipt.PlanRequireDateEnd = this.dtpPlanDateEnd.Value;
                ResReceipt.Details.Clear();

                foreach (DataGridViewRow var in this.dgDetail.Rows)
                {
                    if ((bool)var.Cells[colSelect.Name].Value)
                    {
                        string _selectValue = var.Cells["colSelect"].EditedFormattedValue.ToString();
                        if (_selectValue == "True")
                        {
                            ResourceRequireReceiptDetail rrDtl = new ResourceRequireReceiptDetail();
                            if (var.Tag != null)
                            {
                                rrDtl = var.Tag as ResourceRequireReceiptDetail;
                                ResReceipt.Details.Add(rrDtl);
                            }
                            if (ResReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "1" || ResReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "2")
                            {
                                rrDtl.ApproachDate = ClientUtil.ToDateTime(var.Cells[colEnterDate.Name].Value);
                                rrDtl.UsedRank = var.Cells[colUsedRank.Name].Tag as SubContractProject;
                                rrDtl.UsedRankName = ClientUtil.ToString(var.Cells[colUsedRank.Name].Value);
                            }
                            if (ResReceipt.ResourceRequirePlanTypeCode.Substring(2, 1) == "3")
                            {
                                rrDtl.Price = ClientUtil.ToDecimal(var.Cells[colPrice.Name].Value);
                            }
                            rrDtl.ApproachRequestDesc = ClientUtil.ToString(var.Cells[colApproachRequestDesc.Name].Value);
                            rrDtl.DiagramNumber = ClientUtil.ToString(var.Cells[colArmourDemand.Name].Value);
                            rrDtl.CostQuantity = ClientUtil.ToDecimal(var.Cells[colCostQuantity.Name].Value);
                            rrDtl.DailyPlanPublishQuantity = ClientUtil.ToDecimal(var.Cells[colDailyPlanPublishQuantity.Name].Value);
                            rrDtl.PeriodQuantity = ClientUtil.ToDecimal(var.Cells[colPeriodQuantity.Name].Value);
                            rrDtl.QuantityUnitGUID = var.Cells[colQuantityUnit.Name].Tag as StandardUnit;
                            rrDtl.QuantityUnitName = ClientUtil.ToString(var.Cells[colQuantityUnit.Name].Value);
                            rrDtl.RequireType = EnumUtil<PlanRequireType>.FromDescription(var.Cells[colRequireType.Name].Value);
                            if (var.Cells[colMaterialType.Name].Tag != null)
                            {
                                rrDtl.MaterialCode = (var.Cells[colMaterialType.Name].Tag as Material).Code;
                                rrDtl.MaterialResource = var.Cells[colMaterialType.Name].Tag as Material;
                                rrDtl.MaterialName = (var.Cells[colMaterialType.Name].Tag as Material).Name;
                                rrDtl.MaterialStuff = (var.Cells[colMaterialType.Name].Tag as Material).Stuff;
                                rrDtl.MaterialSpec = (var.Cells[colMaterialType.Name].Tag as Material).Specification;
                            }
                            if (var.Cells[colProjectTaskPath.Name].Tag != null)
                            {
                                rrDtl.TheGWBSTaskGUID = var.Cells[colProjectTaskPath.Name].Tag as GWBSTree;
                                rrDtl.TheGWBSSysCode = (var.Cells[colProjectTaskPath.Name].Tag as GWBSTree).SysCode;
                                rrDtl.TheGWBSTaskName = (var.Cells[colProjectTaskPath.Name].Tag as GWBSTree).Name;
                            }
                            rrDtl.TheResReceipt = ResReceipt;
                            var.Tag = rrDtl;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ResReceipt.State == ResourceRequirePlanState.制定 && OprCode != (int)OperationCode.显示)
            {
                bool flag = false;
                IList list = new ArrayList();
                for (int i = dgDetail.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)this.dgDetail[colSelect.Name, i].Value)
                    {
                        string _selectValue = dgDetail.Rows[i].Cells["colSelect"].EditedFormattedValue.ToString();
                        if (_selectValue == "True")
                        {
                            //物理删除表格中的数据信息
                            dgDetail.Rows.Remove(dgDetail.Rows[i]);
                            flag = true;
                        }
                    }
                    else
                    {
                        this.dgDetail[colSelect.Name, i].Value = true;
                    }
                }
                if (flag)
                {
                    MessageBox.Show("删除成功！");

                }
                else
                {
                    MessageBox.Show("请选择要删除的信息！");
                }
            }
            else
            {
                string message = "此单已提交，不能删除！";
                MessageBox.Show(message);
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = false;
                string errMsg = "";
                if (ResReceipt.State != ResourceRequirePlanState.冻结 && ResReceipt.State != ResourceRequirePlanState.发布 && ResReceipt.State != ResourceRequirePlanState.提交)
                {
                    if (!ValidView()) return;
                    if (ResReceipt.Id != null)
                    {

                        IList list = model.GenerateSupplyResourcePlan(ResReceipt.Id);

                        errMsg = list[0] as string;

                        if (!string.IsNullOrEmpty(errMsg))
                        {
                            MessageBox.Show(errMsg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("物资需求计划生成成功！");
                            flag = true; ;
                        }

                        if (flag)
                        {

                            SaveView("提交");

                            ResReceipt = model.SaveResourceReceipt(ResReceipt);
                            reSet(ResReceipt);
                            Enable();

                            //string code = ResReceipt.ResourceRequirePlanTypeCode;
                            //if (code.Substring(2, 1) == "1")
                            //{
                            //    IList listDaily = list[1] as IList;
                            //    foreach (DailyPlanMaster mat in listDaily)
                            //    {
                            //        appModel.SendMessage(mat.Id, "DailyPlanMaster");
                            //    }
                            //}
                            //if (code.Substring(2, 1) == "2")
                            //{
                            //    IList listMonthly = list[1] as IList;
                            //    foreach (MonthlyPlanMaster mat in listMonthly)
                            //    {
                            //        appModel.SendMessage(mat.Id, "MonthlyPlanMaster");
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        MessageBox.Show("请先保存信息！");
                    }
                }
                else
                {
                    MessageBox.Show("信息已经提交！");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(err));
                return;
            }
        }

        /// <summary>
        /// 刷新状态(按钮状态)
        /// </summary>
        /// <param name="state"></param>
        private void RefreshState(string state)
        {
            //控制表格
            switch (state)
            {
                case "Browser":
                    txtOperatioOrgName.Text = "";
                    txtPlanName.Text = "";
                    cbPlanType.Text = "";
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    break;
                default:
                    break;
            }
        }

        private void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            if (colName == colPeriodQuantity.Name)
            {
                string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colPeriodQuantity.Name].Value.ToString();
                validity = CommonMethod.VeryValid(temp_quantity);
                if (validity == false)
                {
                    MessageBox.Show("请输入数字！");
                    dgDetail.Rows[e.RowIndex].Cells[colPeriodQuantity.Name].Value = "";
                }
            }
            //    System.Web.UI.WebControls.ListItem li = cbPlanType.SelectedItem as System.Web.UI.WebControls.ListItem;
            //    if (li == null)
            //    {
            //        MessageBox.Show("没有选中的计划类型！");
            //        return;
            //    }
            //    if (dgDetail.Rows[e.RowIndex].Cells[colPeriodQuantity.Name].Value != null)
            //    {
            //        GWBSTree TheGWBSTaskGUID = dgDetail.CurrentRow.Cells[colProjectTaskPath.Name].Tag as GWBSTree;
            //        Material MaterialGUID = dgDetail.CurrentRow.Cells[colMaterialType.Name].Tag as Material;
            //        string DiagramNumber = ClientUtil.ToString(dgDetail.CurrentRow.Cells[colArmourDemand.Name].Value);
            //        decimal PeriodQuantity = ClientUtil.ToDecimal(dgDetail.CurrentRow.Cells[colPeriodQuantity.Name].Value);

            //        ResourceRequirePlanDetail dtl = model.ResourceRequirePlanSrv.CalculateGWBSRollingResourceDemandData(ResPlan, TheGWBSTaskGUID, MaterialGUID, DiagramNumber);
            //        if (dtl != null)
            //        {
            //            if (li.Value.Substring(2, 1) == "2")
            //            {
            //                if (PeriodQuantity > dtl.PlanInRequireQuantity + dtl.PlanOutRequireQuantity - dtl.MonthPlanPublishQuantity)
            //                {
            //                    MessageBox.Show("所提月度需求量累计超过需求计划总量！");
            //                    dgDetail.Rows[e.RowIndex].Cells[colPeriodQuantity.Name].Value = "";
            //                }
            //            }
            //            if (li.Value.Substring(2, 1) == "1")
            //            {
            //                if (dtl.MonthPlanPublishQuantity != 0)
            //                {
            //                    if (PeriodQuantity > dtl.MonthPlanPublishQuantity - dtl.DailyPlanPublishQuantity)
            //                    {
            //                        MessageBox.Show("所提日常需求量累计超过月度发布需求计划量！");
            //                        dgDetail.Rows[e.RowIndex].Cells[colPeriodQuantity.Name].Value = "";
            //                    }
            //                }
            //                else
            //                {
            //                    if (PeriodQuantity > dtl.PlanInRequireQuantity + dtl.PlanOutRequireQuantity - dtl.DailyPlanPublishQuantity)
            //                    {
            //                        MessageBox.Show("所提日常需求量累计超过需求计划总量！");
            //                        dgDetail.Rows[e.RowIndex].Cells[colPeriodQuantity.Name].Value = "";
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //string colName = this.dgDetail.Columns[e.ColumnIndex].Name;
            //if (colName == colPeriodQuantity.Name)
            //{
            //    if (dgDetail.Rows[e.RowIndex].Cells[colPeriodQuantity.Name].Value != null)
            //    {

            //    }
            //}
        }

        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colUsedRank.Name))
            {
                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                VCommonSupplierRelationSelect comSelect = new VCommonSupplierRelationSelect();
                comSelect.OpenSelectView("", CommonUtil.SupplierCatCode3);
                IList list = comSelect.Result;
                if (list == null || list.Count < 0) return;
                SupplierRelationInfo relInfo = list[0] as SupplierRelationInfo;
                string id = relInfo.Id;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("BearerOrg.Id", id));
                oq.AddFetchMode("BearerOrg", NHibernate.FetchMode.Eager);
                IList listsub = model.ResourceRequirePlanSrv.ObjectQuery(typeof(SubContractProject), oq);
                if (listsub != null && listsub.Count > 0)
                {
                    SubContractProject s = listsub[0] as SubContractProject;
                    this.dgDetail.CurrentRow.Cells[colUsedRank.Name].Tag = s;
                    this.dgDetail.CurrentRow.Cells[colUsedRank.Name].Value = s.BearerOrgName;
                }
                this.txtPlanName.Focus();
            }
        }
    }
}
