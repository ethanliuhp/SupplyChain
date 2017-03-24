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
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatMng;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;

namespace Application.Business.Erp.SupplyChain.Client.TotalDemandPlan
{
    public partial class VTotalDemandPlanQuery : TBasicDataView
    {
        private MTotalDemandPlanMng model = new MTotalDemandPlanMng();
        MAppPlatMng appModel = new MAppPlatMng();
        int OperaCode = 0;//操作码

        private ResourceRequireReceipt resReceipt;
        /// <summary>
        /// 当前明细单据
        /// </summary>
        public ResourceRequireReceipt ResReceipt
        {
            get { return resReceipt; }
            set { resReceipt = value; }
        }
        ResourceRequirePlan RecPlan = new ResourceRequirePlan();
        public VTotalDemandPlanQuery(int OprCode, ResourceRequirePlan ResourcePlan, ResourceRequireReceipt ResRec)
        {
            InitializeComponent();
            OperaCode = OprCode;
            GetRes(ResRec);
            RecPlan = ResourcePlan;

            InitEvent();
            InitData();
        }
        void GetRes(ResourceRequireReceipt ResRec)
        {
            if (ResRec.Id != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", ResRec.Id));
                oq.AddFetchMode("ResourceCategory", NHibernate.FetchMode.Eager);
                ResRec = model.ResourceRequirePlanSrv.GetResourceRequireReceipt(oq);
                ResReceipt = ResRec;
            }
        }

        private void InitData()
        {
            string[] lockCols = new string[] { colQuantityUnit.Name, colMaterialType.Name, colSpec.Name, colArmourDemand.Name };
            dgDetail.SetColumnsReadOnly(lockCols);
            btnSearch.Enabled = true;
            btnOperationOrg.Enabled = true;
            btnBuild.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Enabled = true;
            btnSubmit.Enabled = true;
            comProjectTask.Enabled = true;
            txtOperationOrg.Enabled = true;
            txtPlanName.Enabled = true;
            comPlanType.Enabled = true;
            dgDetail.ReadOnly = false;
            txtMaterialCategory.Enabled = true;
            txtHandlePerson.Enabled = true;
            //VBasicDataOptr.InitResReqirePlan(comPlanType, false);
            this.txtOperationOrg.Tag = ConstObject.TheLogin.TheAccountOrgInfo;
            if (this.txtOperationOrg.Tag != null)
            {
                this.txtOperationOrg.Text = ConstObject.TheLogin.TheAccountOrgInfo.Name;
            }
            IList planTypeList = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ResourceRequirePlanTypeCate);
            if (planTypeList != null)
            {
                foreach (BasicDataOptr bdo in planTypeList)
                {
                    string strCode = bdo.BasicCode;
                    if (strCode.Substring(2, 1) == "0")
                    {
                        System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                        li.Text = bdo.BasicName;
                        li.Value = bdo.BasicCode;
                        comPlanType.Items.Add(li);
                    }
                }
            }
            if (RecPlan != null)
            {
                txtDemandPlanName.Text = RecPlan.RequirePlanVersion;
                txtDemandPlanName.Tag = RecPlan;

            }
            if (OperaCode == 1)
            {
                //新增  
                //归属项目
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                this.ResReceipt = new ResourceRequireReceipt();
                ResReceipt.CreateDate = ConstObject.LoginDate;//制单时间
                ResReceipt.HandlePerson = ConstObject.LoginPersonInfo;//负责人
                ResReceipt.HandlePersonName = ConstObject.LoginPersonInfo.Name;//负责人名称
                if (txtOperationOrg.Tag != null)
                {
                    ResReceipt.OwnerOrgSysCode = ConstObject.TheLogin.TheAccountOrgInfo.SysCode;//核算组织层次吗
                }
                ResReceipt.PlanRequireDateBegin = projectInfo.BeginDate;
                ResReceipt.PlanRequireDateEnd = projectInfo.EndDate;
                ResReceipt.ResourceRequirePlan = RecPlan;
                ResReceipt.State = ResourceRequirePlanState.制定;
                ResReceipt.ProjectId = projectInfo.Id;
                ResReceipt.ProjectName = projectInfo.Name;
            }
            if (OperaCode == 2)
            {
                //修改
                this.btnOperationOrg.Enabled = false;
                this.btnSearch.Enabled = false;
                this.comPlanType.Enabled = false;
                if (ResReceipt.State == ResourceRequirePlanState.提交)
                {
                    controlsLock();
                }
                comPlanType.Text = ResReceipt.ResourceRequirePlanTypeWord;
                txtPlanName.Text = ResReceipt.ReceiptName;
                txtDemandPlanName.Text = ResReceipt.ResourceRequirePlanName;
                txtDemandPlanName.Tag = ResReceipt.ResourceRequirePlan;
                comState.Text = EnumUtil<ResourceRequirePlanState>.GetDescription(ResReceipt.State);
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = ResReceipt.ResourceRequirePlanTypeWord;
                li.Value = ResReceipt.ResourceRequirePlanTypeCode;
                comPlanType.Items.Add(li);
                txtOperationOrg.Tag = ResReceipt.OpgOrgInfo;
                txtOperationOrg.Text = ResReceipt.OpgOrgInfoName;
                txtMaterialCategory.Result.Clear();
                txtMaterialCategory.Value = ResReceipt.ResourceCategory.Name;
                txtMaterialCategory.Result.Add(ResReceipt.ResourceCategory);
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheResReceipt", resReceipt));
                IList list = model.ResourceRequirePlanSrv.ObjectQuery(oq);
                dgDetail.Rows.Clear();
                foreach (ResourceRequireReceiptDetail var in list)
                {
                    int i = this.dgDetail.Rows.Add();
                    this.dgDetail[colSelete.Name, i].Value = true;
                    this.dgDetail[colArmourDemand.Name, i].Value = var.DiagramNumber;
                    this.dgDetail[colMaterialType.Name, i].Tag = var.MaterialResource;
                    this.dgDetail[colMaterialType.Name, i].Value = var.MaterialName;
                    this.dgDetail[colSpec.Name, i].Value = var.MaterialSpec;
                    this.dgDetail[colState.Name, i].Value = EnumUtil<ResourceRequirePlanDetailState>.GetDescription(var.State);
                    this.dgDetail[colJGDemand.Name, i].Value = var.FirstOfferRequireQuantity;
                    this.dgDetail[colPlanType.Name, i].Value = EnumUtil<PlanRequireType>.GetDescription(var.RequireType);
                    //this.dgDetail[colInnerDemand.Name, i].Value = var.PlanInRequireQuantity;
                    this.dgDetail[colPlanQuantity.Name, i].Value = var.PlannedCostQuantity;
                    //this.dgDetail[colOuterDemand.Name, i].Value = var.PlanOutRequireQuantity;
                    this.dgDetail[colQuantityUnit.Name, i].Value = var.QuantityUnitName;
                    this.dgDetail[colQuantityUnit.Name, i].Tag = var.QuantityUnitGUID;
                    this.dgDetail[colSumDemand.Name, i].Value = var.PlanInRequireQuantity + var.PlanOutRequireQuantity;
                    this.dgDetail[colTechnicalParameters.Name, i].Value = var.TechnicalParameters;
                    comProjectTask.Tag = var.TheGWBSTaskGUID;
                    comProjectTask.Text = var.TheGWBSTaskName;
                    this.dgDetail.Rows[i].Tag = var;
                }
            }
            if (OperaCode == 3)
            {
                controlsLock();
                //显示  
                comPlanType.Text = ResReceipt.ResourceRequirePlanTypeWord;
                txtPlanName.Text = ResReceipt.ReceiptName;
                txtDemandPlanName.Text = ResReceipt.ResourceRequirePlanName;
                txtDemandPlanName.Tag = ResReceipt.ResourceRequirePlan;
                comState.Text = EnumUtil<ResourceRequirePlanState>.GetDescription(ResReceipt.State);
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = ResReceipt.ResourceRequirePlanTypeWord;
                li.Value = ResReceipt.ResourceRequirePlanTypeCode;
                comPlanType.Items.Add(li);
                txtOperationOrg.Tag = ResReceipt.OpgOrgInfo;
                txtOperationOrg.Text = ResReceipt.OpgOrgInfoName;
                txtMaterialCategory.Result.Clear();
                txtMaterialCategory.Value = ResReceipt.ResourceCategory.Name;
                txtMaterialCategory.Result.Add(ResReceipt.ResourceCategory);
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheResReceipt", resReceipt));
                IList list = model.ResourceRequirePlanSrv.ObjectQuery(oq);
                if (list.Count > 0)
                {
                    foreach (ResourceRequireReceiptDetail var in list)
                    {
                        int i = this.dgDetail.Rows.Add();
                        this.dgDetail[colSelete.Name, i].Value = true;
                        this.dgDetail[colArmourDemand.Name, i].Value = var.DiagramNumber;
                        this.dgDetail[colMaterialType.Name, i].Tag = var.MaterialResource;
                        this.dgDetail[colMaterialType.Name, i].Value = var.MaterialName;
                        this.dgDetail[colJGDemand.Name, i].Value = var.FirstOfferRequireQuantity;
                        this.dgDetail[colPlanQuantity.Name, i].Value = var.PlannedCostQuantity;
                        this.dgDetail[colSpec.Name, i].Value = var.MaterialSpec;
                        this.dgDetail[colState.Name, i].Value = EnumUtil<ResourceRequirePlanDetailState>.GetDescription(var.State);
                        this.dgDetail[colPlanType.Name, i].Value = EnumUtil<PlanRequireType>.GetDescription(var.RequireType);
                        this.dgDetail[colSumDemand.Name, i].Value = var.PlanOutRequireQuantity + var.PlanInRequireQuantity;
                        comProjectTask.Tag = var.TheGWBSTaskGUID;
                        comProjectTask.Text = var.TheGWBSTaskName;
                        this.dgDetail[colTechnicalParameters.Name, i].Value = var.TechnicalParameters;
                        this.dgDetail[colQuantityUnit.Name, i].Value = var.QuantityUnitName;
                        this.dgDetail[colQuantityUnit.Name, i].Tag = var.QuantityUnitGUID;
                        this.dgDetail.Rows[i].Tag = var;
                    }
                }
            }
            txtHandlePerson.Value = ResReceipt.HandlePersonName;
            txtHandlePerson.Tag = ResReceipt.HandlePerson;
            comPlanType.Text = ResReceipt.ResourceRequirePlanTypeWord;
            comState.Text = EnumUtil<ResourceRequirePlanState>.GetDescription(ResReceipt.State);
            dtDate.Value = ResReceipt.CreateDate;
        }

        void controlsLock()
        {
            btnSearch.Enabled = false;
            btnOperationOrg.Enabled = false;
            btnBuild.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
            comProjectTask.Enabled = false;
            txtOperationOrg.Enabled = false;
            txtPlanName.Enabled = false;
            comPlanType.Enabled = false;
            dgDetail.ReadOnly = true;
            txtMaterialCategory.Enabled = false;
            txtHandlePerson.Enabled = false;
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
                    comPlanType.Text = "";
                    comProjectTask.Text = "";
                    txtPlanName.Text = "";
                    this.dgDetail.EditMode = DataGridViewEditMode.EditProgrammatically;
                    break;
                default:
                    break;
            }
        }

        void pnlFloor_Paint(object sender, PaintEventArgs e)
        {
            btnBuild.Focus();
        }

        private void InitEvent()
        {
            lnkAll.Click += new EventHandler(lnkAll_Click);
            lnkNone.Click += new EventHandler(lnkNone_Click);
            this.btnBuild.Click += new EventHandler(btnBuild_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnGaveup.Click += new EventHandler(btnGaveup_Click);
            this.btnExcel.Click += new EventHandler(btnExcel_Click);
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.dgDetail.CellDoubleClick += new DataGridViewCellEventHandler(dgDetail_CellDoubleClick);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            this.comPlanType.SelectedIndexChanged += new EventHandler(comPlanType_SelectedIndexChanged);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.dgDetail.MouseClick += new MouseEventHandler(dgDetail_MouseClick);
            this.dgDetail.CellEndEdit += new DataGridViewCellEventHandler(dgDetail_CellEndEdit);
            this.conMenu.ItemClicked += new ToolStripItemClickedEventHandler(conMenu_ItemClicked);
            this.btnSupplyResourcePlan.Click += new EventHandler(btnSupplyResourcePlan_Click);
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
            //            MessageBox.Show("生成失败！");
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

        void dgDetail_MouseClick(object sender, MouseEventArgs e)
        {
            if (!dgDetail.ReadOnly)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (dgDetail.Enabled)
                    {
                        conMenu.Items[ConAdd.Name].Enabled = true;
                        conMenu.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (!dgDetail.ReadOnly)
                {
                    if (e.RowIndex >= 0)
                    {
                        if (dgDetail.Rows[e.RowIndex].Selected == false)
                        {
                            dgDetail.ClearSelection();
                            dgDetail.Rows[e.RowIndex].Selected = true;
                        }
                        conMenu.Items[ConAdd.Name].Enabled = true;
                        conMenu.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }

        void conMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text.Trim() == "按比例调整选中资源需求总量")
            {
                conMenu.Hide();
                VRace race = new VRace();
                race.ShowDialog();
                decimal quantity = race.Result;
                if (quantity != 0)
                {
                    foreach (DataGridViewRow var in this.dgDetail.Rows)
                    {
                        if (var.IsNewRow) break;
                        if (var.Cells[colSelete.Name].Value != null)
                        {
                            if ((bool)var.Cells[colSelete.Name].Value)
                            {
                                string _selectValue = var.Cells["colSelete"].EditedFormattedValue.ToString();
                                if (_selectValue == "True")
                                {
                                    var.Cells[colSumDemand.Name].Value = ClientUtil.ToString(ClientUtil.ToDecimal(var.Cells[colSumDemand.Name].Value) * quantity / 100);
                                }
                            }
                        }
                    }
                }
            }
        }

        void dgDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            bool validity = true;
            string colName = dgDetail.Columns[e.ColumnIndex].Name;
            //if (colName == colInnerDemand.Name)
            //{
            //    if (dgDetail.Rows[e.RowIndex].Cells[colInnerDemand.Name].Value != null)
            //    {
            //        string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colInnerDemand.Name].Value.ToString();
            //        validity = CommonMethod.VeryValid(temp_quantity);
            //        if (validity == false)
            //        {
            //            MessageBox.Show("计划内需求量为数字！");
            //            return;
            //        }
            //    }
            //    decimal OutDemand = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOuterDemand.Name].Value);
            //    decimal InnerDemand = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colInnerDemand.Name].Value);
            //    dgDetail.Rows[e.RowIndex].Cells[colSumDemand.Name].Value =ClientUtil.ToString(OutDemand + InnerDemand);
            //}
            //if (colName == colOuterDemand.Name)
            //{
            //    if (dgDetail.Rows[e.RowIndex].Cells[colOuterDemand.Name].Value != null)
            //    {
            //        string temp_quantity = dgDetail.Rows[e.RowIndex].Cells[colOuterDemand.Name].Value.ToString();
            //        validity = CommonMethod.VeryValid(temp_quantity);
            //        if (validity == false)
            //        {
            //            MessageBox.Show("计划外需求量为数字！");
            //            return;
            //        }
            //    }
            //    decimal OutDemand = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colOuterDemand.Name].Value);
            //    decimal InnerDemand = ClientUtil.ToDecimal(dgDetail.Rows[e.RowIndex].Cells[colInnerDemand.Name].Value);
            //    dgDetail.Rows[e.RowIndex].Cells[colSumDemand.Name].Value = ClientUtil.ToString(OutDemand + InnerDemand);
            //}
        }

        void comPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgDetail.Rows.Clear();
            System.Web.UI.WebControls.ListItem li = comPlanType.SelectedItem as System.Web.UI.WebControls.ListItem;
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
            }
            if (strCode.Substring(1, 1) == "1")
            {
                //劳务
                this.txtMaterialCategory.rootCatCode = "03";
            }
            if (strCode.Substring(1, 1) == "2")
            {
                //资源
                oq.AddCriterion(Expression.Eq("Code", "01"));
                this.txtMaterialCategory.rootCatCode = "01";
            }
            if (strCode.Substring(1, 1) == "3")
            {
                //机械
                oq.AddCriterion(Expression.Eq("Code", "02"));
                this.txtMaterialCategory.rootCatCode = "02";
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
                var.Cells[colSelete.Name].Value = true;
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
                bool isSelected = (bool)var.Cells[colSelete.Name].Value;
                var.Cells[colSelete.Name].Value = !isSelected;
            }
        }

        void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect();
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                OperationOrgInfo info = frm.Result[0] as OperationOrgInfo;
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", info.Id));
                OperationOrgInfo org = model.ResourceRequirePlanSrv.ObjectQuery(typeof(OperationOrgInfo), oq)[0] as OperationOrgInfo;
                txtOperationOrg.Tag = org;
                txtOperationOrg.Text = org.Name;


                ResReceipt.OpgOrgInfo = org;
                ResReceipt.OpgOrgInfoName = org.Name;
                ResReceipt.OwnerOrgSysCode = org.SysCode;
            }
            dgDetail.Rows.Clear();
        }
        //查询
        void btnSearch_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                TreeNode root = frm.SelectResult[0];
                GWBSTree task = root.Tag as GWBSTree;
                if (task != null)
                {
                    this.comProjectTask.Text = task.Name;
                    this.comProjectTask.Tag = task;

                }

            }
            dgDetail.Rows.Clear();
        }
        //创建明细信息
        void btnBuild_Click(object sender, EventArgs e)
        {
            try
            {
                //计划类型不为空
                if (comProjectTask.Text == "")
                {
                    MessageBox.Show("工程任务不能为空！");
                    return;
                }
                if (txtOperationOrg.Text == "")
                {
                    MessageBox.Show("核算组织不能为空！");
                    return;
                }
                if (comPlanType.Text == "")
                {
                    MessageBox.Show("计划类型不能为空！");
                    return;
                }
                else
                {
                    if (txtMaterialCategory.Text == "")
                    {
                        MessageBox.Show("物资资源不能为空！");
                        return;
                    }
                }
                FlashScreen.Show("生成总量需求计划明细......");
                ResReceipt.Details.Clear();
                if (txtMaterialCategory.Result != null && txtMaterialCategory.Result.Count > 0)
                {
                    ResReceipt.ResourceCategory = txtMaterialCategory.Result[0] as MaterialCategory;
                    ResReceipt.ResourceCategorySysCode = (txtMaterialCategory.Result[0] as MaterialCategory).SysCode;
                }
                if (comProjectTask.Tag != null)
                {
                    //查询滚动需求计划明细
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheResourceRequirePlan", RecPlan));
                    oq.AddCriterion(Expression.Eq("State", ResourceRequirePlanDetailState.发布));
                    oq.AddFetchMode("MaterialCategory", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
                    oq.AddCriterion(Expression.Like("ResourceCategory.SysCode", "%" + ResReceipt.ResourceCategorySysCode + "%"));
                    oq.AddCriterion(Expression.Like("TheGWBSTaskGUID.SysCode", "%" + (comProjectTask.Tag as GWBSTree).SysCode + "%"));
                    Hashtable ht = model.ResourceRequirePlanSrv.GetResPlanDetail(oq);
                    if (ht.Count > 0)
                    {
                        dgDetail.Rows.Clear();
                        //循环hashtable
                        foreach (System.Collections.DictionaryEntry objDE in ht)
                        {
                            ResourceRequirePlanDetail detail = objDE.Key as ResourceRequirePlanDetail;
                            int i = this.dgDetail.Rows.Add();
                            ResourceRequireReceiptDetail resdtl = new ResourceRequireReceiptDetail();
                            resdtl.MaterialResource = detail.MaterialResource;
                            resdtl.MaterialCode = detail.MaterialCode;
                            resdtl.MaterialName = detail.MaterialName;
                            resdtl.MaterialStuff = detail.MaterialStuff;
                            resdtl.MaterialSpec = detail.MaterialSpec;
                            resdtl.MaterialCategory = detail.ResourceCategory;
                            resdtl.MaterialCategoryName = detail.ResourceTypeClassification;
                            resdtl.PlanInRequireQuantity = detail.PlanInRequireQuantity;
                            resdtl.PlannedCostQuantity = detail.PlannedCostQuantity;//计划成本量
                            resdtl.PlanOutRequireQuantity = detail.PlanOutRequireQuantity;
                            resdtl.QuantityUnitGUID = detail.QuantityUnitGUID;
                            resdtl.RequireType = detail.RequireType;
                            resdtl.QuantityUnitName = detail.QuantityUnitName;
                            resdtl.ResponsibilityCostQuantity = detail.ResponsibilityCostQuantity;//责任成本量
                            resdtl.TheGWBSSysCode = detail.TheGWBSSysCode;
                            resdtl.TheGWBSTaskGUID = detail.TheGWBSTaskGUID;
                            resdtl.FirstOfferRequireQuantity = detail.FirstOfferRequireQuantity;
                            resdtl.SupplyPlanPublishQuantity = detail.SupplyPlanPublishQuantity;
                            resdtl.State = ResourceRequireReceiptDetailState.有效;
                            resdtl.TheGWBSTaskName = detail.TheGWBSTaskName;
                            resdtl.TechnicalParameters = detail.TechnicalParameters;
                            resdtl.Price = detail.Price;
                            resdtl.Master = ResReceipt;
                            this.dgDetail[colSelete.Name, i].Value = true;
                            this.dgDetail[colArmourDemand.Name, i].Value = detail.DiagramNumber;
                            this.dgDetail[colMaterialType.Name, i].Value = detail.MaterialName;
                            this.dgDetail[colMaterialType.Name, i].Tag = detail.MaterialResource;
                            this.dgDetail[colSpec.Name, i].Value = detail.MaterialSpec;
                            this.dgDetail[colState.Name, i].Value = ResourceRequireReceiptDetailState.有效;
                            this.dgDetail[colJGDemand.Name, i].Value = detail.FirstOfferRequireQuantity.ToString();
                            this.dgDetail[colPlanQuantity.Name, i].Value = detail.PlannedCostQuantity;
                            this.dgDetail[colPlanType.Name, i].Value = EnumUtil<PlanRequireType>.GetDescription(detail.RequireType);
                            if (detail.RequireType == PlanRequireType.计划内需求)
                            {
                                this.dgDetail[colSumDemand.Name, i].Value = detail.PlanInRequireQuantity;
                            }
                            if (detail.RequireType == PlanRequireType.计划外需求)
                            {
                                this.dgDetail[colSumDemand.Name, i].Value = detail.PlanOutRequireQuantity;
                            }
                            this.dgDetail[colQuantityUnit.Name, i].Tag = detail.QuantityUnitGUID;
                            this.dgDetail[colQuantityUnit.Name, i].Value = detail.QuantityUnitName;
                            this.dgDetail[colTechnicalParameters.Name, i].Value = detail.TechnicalParameters;
                            this.dgDetail.Rows[i].Tag = resdtl;
                        }
                    }
                    else
                    {
                        FlashScreen.Close();
                        MessageBox.Show("没有获得滚动需求信息！");
                    }
                }
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("生成信息出错。\n" + ex.Message);
                return;
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        /// <summary>
        /// 保存数据前校验数据
        /// </summary>
        /// <returns></returns>
        private bool ValidView()
        {
            bool validity = true;
            string validMessage = "";
            if (this.dgDetail.Rows.Count == 0)
            {
                MessageBox.Show("明细不能为空！");
                return false;
            }
            if (comPlanType.Text == "")
            {
                MessageBox.Show("计划类型不能为空！");
                return false;
            }
            if (txtPlanName.Text == "")
            {
                MessageBox.Show("计划名称不能为空！");
                return false;
            }
            //明细表数据校验
            foreach (DataGridViewRow dr in dgDetail.Rows)
            {
                //最后一行不进行校验
                if (dr.IsNewRow) break;
                if (dr.Cells[colMaterialType.Name].Value == null)
                {
                    MessageBox.Show("资源类型不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colMaterialType.Name];
                    return false;
                }
                if (dr.Cells[colJGDemand.Name].Value == null)
                {
                    MessageBox.Show("甲供需求量不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colJGDemand.Name];
                    return false;
                }
                else
                {
                    string temp_quantity = dr.Cells[colJGDemand.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                        MessageBox.Show("甲供需求量应输入数字！");
                }
                if (dr.Cells[colPlanQuantity.Name].Value == null)
                {
                    MessageBox.Show("计划成本量不允许为空！");
                    dgDetail.CurrentCell = dr.Cells[colPlanQuantity.Name];
                    return false;
                }
                else
                {
                    string temp_quantity = dr.Cells[colPlanQuantity.Name].Value.ToString();
                    validity = CommonMethod.VeryValid(temp_quantity);
                    if (validity == false)
                        MessageBox.Show("甲供需求量应输入数字！");
                }
                if ((bool)dr.Cells[colSelete.Name].Value)
                {
                    if (ClientUtil.ToDecimal(dr.Cells[colSumDemand.Name].Value) == 0)
                    {
                        MessageBox.Show("计划需求总量不能为0 ！");
                        dgDetail.CurrentCell = dr.Cells[colSumDemand.Name];
                        return false;
                    }
                    else
                    {
                        string temp_quantity = dr.Cells[colSumDemand.Name].Value.ToString();
                        validity = CommonMethod.VeryValid(temp_quantity);
                        if (validity == false)
                            MessageBox.Show("计划需求总量应输入数字！");
                    }
                }

            }
            return true;
        }


        //删除
        void btnDelete_Click(object sender, EventArgs e)
        {
            if (ResReceipt.State != ResourceRequirePlanState.冻结 && ResReceipt.State != ResourceRequirePlanState.发布 && ResReceipt.State != ResourceRequirePlanState.提交)
            {
                bool flag = false;
                IList list = new ArrayList();
                for (int i = dgDetail.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)this.dgDetail[colSelete.Name, i].Value)
                    {
                        string _selectValue = dgDetail.Rows[i].Cells["colSelete"].EditedFormattedValue.ToString();
                        if (_selectValue == "True")
                        {
                            //物理删除表格中的数据信息
                            dgDetail.Rows.Remove(dgDetail.Rows[i]);
                            flag = true;
                        }
                    }
                    else
                    {
                        this.dgDetail[colSelete.Name, i].Value = true;
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
        void reSet(ResourceRequireReceipt ResReceipt)
        {
            comPlanType.Text = ResReceipt.ResourceRequirePlanTypeWord;
            txtPlanName.Text = ResReceipt.ReceiptName;
            txtDemandPlanName.Text = ResReceipt.ResourceRequirePlanName;
            txtDemandPlanName.Tag = ResReceipt.ResourceRequirePlan;
            comState.Text = EnumUtil<ResourceRequirePlanState>.GetDescription(ResReceipt.State);
            System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
            li.Text = ResReceipt.ResourceRequirePlanTypeWord;
            li.Value = ResReceipt.ResourceRequirePlanTypeCode;
            comPlanType.Items.Add(li);
            txtOperationOrg.Tag = ResReceipt.OpgOrgInfo;
            txtOperationOrg.Text = ResReceipt.OpgOrgInfoName;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheResReceipt", resReceipt));
            IList list = model.ResourceRequirePlanSrv.ObjectQuery(oq);
            dgDetail.Rows.Clear();
            foreach (ResourceRequireReceiptDetail var in list)
            {
                int i = this.dgDetail.Rows.Add();
                this.dgDetail[colSelete.Name, i].Value = true;
                this.dgDetail[colArmourDemand.Name, i].Value = var.DiagramNumber;
                this.dgDetail[colMaterialType.Name, i].Tag = var.MaterialResource;
                this.dgDetail[colMaterialType.Name, i].Value = var.MaterialName;
                this.dgDetail[colState.Name, i].Value = EnumUtil<ResourceRequirePlanDetailState>.GetDescription(var.State);
                this.dgDetail[colJGDemand.Name, i].Value = var.FirstOfferRequireQuantity;
                this.dgDetail[colSpec.Name, i].Value = var.MaterialSpec;
                this.dgDetail[colPlanType.Name, i].Value = EnumUtil<PlanRequireType>.GetDescription(var.RequireType);
                this.dgDetail[colPlanQuantity.Name, i].Value = var.PlannedCostQuantity;
                this.dgDetail[colQuantityUnit.Name, i].Value = var.QuantityUnitName;
                this.dgDetail[colSumDemand.Name, i].Value = var.PlanInRequireQuantity + var.PlanOutRequireQuantity;
                this.dgDetail[colQuantityUnit.Name, i].Tag = var.QuantityUnitGUID;
                this.dgDetail[colTechnicalParameters.Name, i].Value = var.TechnicalParameters;
                comProjectTask.Tag = var.TheGWBSTaskGUID;
                comProjectTask.Text = var.TheGWBSTaskName;
                this.dgDetail.Rows[i].Tag = var;
            }
            if (ResReceipt.State == ResourceRequirePlanState.提交)
            {
                controlsLock();
            }
        }
        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ResReceipt.State != ResourceRequirePlanState.冻结 && ResReceipt.State != ResourceRequirePlanState.发布 && ResReceipt.State != ResourceRequirePlanState.提交)
                {
                    if (!ValidView()) return;
                    if (dgDetail.Rows.Count == 0)
                    {
                        MessageBox.Show("没有要保存的信息！");
                    }
                    else
                    {
                        saveManage("保存");
                        ResReceipt = model.SaveResourceReceipt(ResReceipt);
                        reSet(ResReceipt);
                        MessageBox.Show("保存成功！");
                    }
                }

            }
            catch (Exception err)
            {
                MessageBox.Show("数据错误：" + ExceptionUtil.ExceptionMessage(err));
            }
        }

        void saveManage(string strName)
        {

            if (strName == "提交")
            {
                ResReceipt.State = ResourceRequirePlanState.提交;
            }
            ResReceipt.SchedulingProductionName = txtPlanName.Text;
            ResReceipt.SchedulingProduction = txtPlanName.Tag as WeekScheduleMaster;
            ResReceipt.ResourceRequirePlan = txtDemandPlanName.Tag as ResourceRequirePlan;
            ResReceipt.ResourceRequirePlanName = txtDemandPlanName.Text;
            ResReceipt.ResourceRequirePlanTypeWord = comPlanType.Text;
            System.Web.UI.WebControls.ListItem li = comPlanType.SelectedItem as System.Web.UI.WebControls.ListItem;
            ResReceipt.ResourceRequirePlanTypeWord = li.Text;
            ResReceipt.ResourceRequirePlanTypeCode = li.Value;
            if (txtOperationOrg.Tag != null)
            {
                ResReceipt.OpgOrgInfo = txtOperationOrg.Tag as OperationOrgInfo;
                ResReceipt.OpgOrgInfoName = txtOperationOrg.Text;
            }
            ResReceipt.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
            ResReceipt.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;
            ResReceipt.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;
            ResReceipt.ReceiptName = txtPlanName.Text;
            ResReceipt.Details.Clear();
            foreach (DataGridViewRow var in this.dgDetail.Rows)
            {
                if (var.IsNewRow) break;
                ResourceRequireReceiptDetail curBillDtl = new ResourceRequireReceiptDetail();
                if (var.Tag != null)
                {
                    curBillDtl = var.Tag as ResourceRequireReceiptDetail;
                    ResReceipt.Details.Add(curBillDtl);
                }
                curBillDtl.DiagramNumber = ClientUtil.ToString(var.Cells[colArmourDemand.Name].Value);
                curBillDtl.FirstOfferRequireQuantity = ClientUtil.ToDecimal(var.Cells[colJGDemand.Name].Value);

                if (var.Cells[colMaterialType.Name].Tag != null)
                {
                    curBillDtl.MaterialCode = (var.Cells[colMaterialType.Name].Tag as Material).Code;
                    curBillDtl.MaterialResource = var.Cells[colMaterialType.Name].Tag as Material;
                    curBillDtl.MaterialName = (var.Cells[colMaterialType.Name].Tag as Material).Name;
                    curBillDtl.MaterialStuff = (var.Cells[colMaterialType.Name].Tag as Material).Stuff;

                }

                curBillDtl.MaterialSpec = ClientUtil.ToString(var.Cells[colSpec.Name].Value);
                if (curBillDtl.RequireType == PlanRequireType.计划内需求)
                {
                    curBillDtl.PlanInRequireQuantity = ClientUtil.ToDecimal(var.Cells[colSumDemand.Name].Value);
                }
                curBillDtl.PlannedCostQuantity = ClientUtil.ToDecimal(var.Cells[colPlanQuantity.Name].Value);
                if (curBillDtl.RequireType == PlanRequireType.计划外需求)
                {
                    curBillDtl.PlanOutRequireQuantity = ClientUtil.ToDecimal(var.Cells[colSumDemand.Name].Value);
                }
                curBillDtl.State = EnumUtil<ResourceRequireReceiptDetailState>.FromDescription(var.Cells[colState.Name].Value);
                if (comProjectTask.Tag != null)
                {
                    curBillDtl.TheGWBSTaskGUID = comProjectTask.Tag as GWBSTree;
                    curBillDtl.TheGWBSSysCode = (comProjectTask.Tag as GWBSTree).SysCode;
                    curBillDtl.TheGWBSTaskName = comProjectTask.Text;
                }
                curBillDtl.QuantityUnitGUID = var.Cells[colQuantityUnit.Name].Tag as StandardUnit;
                curBillDtl.QuantityUnitName = ClientUtil.ToString(var.Cells[colQuantityUnit.Name].Value);
                curBillDtl.TechnicalParameters = ClientUtil.ToString(var.Cells[colTechnicalParameters.Name].Value);
                curBillDtl.TheResReceipt = ResReceipt;
                var.Tag = curBillDtl;
            }
        }

        //放弃
        void btnGaveup_Click(object sender, EventArgs e)
        {
            this.btnBuild.FindForm().Close();

        }
        //导出excel
        void btnExcel_Click(object sender, EventArgs e)
        {
            //if (dgDetail.Rows.Count == 0)
            //{
            //    MessageBox.Show("表格中没有信息，不可导出到Excel！");
            //}
            //else
            //{
            //    grid.Rows.Clear();
            //    for (int i = 0; i < dgDetail.Rows.Count; i++)
            //    {
            //        if (this.dgDetail[colSelete.Name, i].Value != null)
            //        {
            //            if ((bool)this.dgDetail[colSelete.Name, i].Value)
            //            {
            //                string _selectValue = dgDetail.Rows[i].Cells["colSelete"].EditedFormattedValue.ToString();
            //                if (_selectValue == "True")
            //                {
            //                    int k = grid.Rows.Add();
            //                    grid[colTH.Name, k].Value = dgDetail[colArmourDemand.Name, i].Value;
            //                    grid[colJG.Name, k].Value = dgDetail[colJGDemand.Name, i].Value;
            //                    grid[colMaterial.Name, k].Value = dgDetail[colMaterialType.Name, i].Value;
            //                    grid[colSpes.Name, k].Value = dgDetail[colSpec.Name, i].Value;
            //                    grid[ColJHCB.Name, k].Value = dgDetail[colPlanQuantity.Name, i].Value;
            //                    grid[colPlanType1.Name, k].Value = dgDetail[colPlanType.Name, i].Value;
            //                    grid[colStates.Name, k].Value = dgDetail[colState.Name, i].Value;
            //                    grid[colUnit.Name, k].Value = dgDetail[colQuantityUnit.Name, i].Value;
            //                    grid[colJHZ.Name, k].Value = dgDetail[colSumDemand.Name, i].Value;
            //                    grid[colTechnicalParameters1.Name, k].Value = dgDetail[colTechnicalParameters.Name, i].Value;
            //                }
            //            }
            //        }
            //    }
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgDetail, true);
            //}
        }
        //提交
        void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string errMsg = "";
                if (ResReceipt.State != ResourceRequirePlanState.冻结 && ResReceipt.State != ResourceRequirePlanState.发布 && ResReceipt.State != ResourceRequirePlanState.提交)
                {
                    if ((txtOperationOrg.Text != null || txtOperationOrg.Text != "") && (txtPlanName.Text != null || txtPlanName.Text != ""))
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
                                saveManage("提交");
                                ResReceipt = model.SaveResourceReceipt(ResReceipt);
                                reSet(ResReceipt);
                                string code = ResReceipt.ResourceRequirePlanTypeCode;
                                if (code.Substring(0, 1) == "1")
                                {
                                    IList listDemand = list[1] as IList;
                                    foreach (DemandMasterPlanMaster mat in listDemand)
                                    {
                                        //appModel.SendMessage(mat.Id, "DemandMasterPlanMaster");
                                    }
                                }
                                if (code.Substring(0, 1) == "3")
                                {
                                    IList listSupply = list[1] as IList;
                                    foreach (SupplyPlanMaster mat in listSupply)
                                    {
                                        //appModel.SendMessage(mat.Id, "SupplyPlanMaster");
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("请先保存数据！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("没有核算组织和计划名称，不可提交！");
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
        /// 物料编码列，支持鼠标双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colMaterialType.Name))
            {
                DataGridViewRow theCurrentRow = this.dgDetail.CurrentRow;
                CommonMaterial materialSelector = new CommonMaterial();
                DataGridViewCell cell = this.dgDetail[e.ColumnIndex, e.RowIndex];
                object tempValue = cell.EditedFormattedValue;
                if (tempValue != null && !tempValue.Equals(""))
                {
                    materialSelector.OpenSelect();
                }
                else
                {
                    materialSelector.OpenSelect();
                }
                IList list = materialSelector.Result;
                foreach (Application.Resource.MaterialResource.Domain.Material theMaterial in list)
                {
                    int i = dgDetail.Rows.Add();
                    this.dgDetail[colMaterialType.Name, i].Tag = theMaterial;
                    this.dgDetail[colMaterialType.Name, i].Value = theMaterial.Name;
                    this.dgDetail[colState.Name, i].Value = ResourceRequireReceiptDetailState.有效;
                    i++;
                }
            }
            if (this.dgDetail.Columns[e.ColumnIndex].Name.Equals(colQuantityUnit.Name))
            {
                StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                if (su != null)
                {
                    this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Tag = su;
                    this.dgDetail.CurrentRow.Cells[colQuantityUnit.Name].Value = su.Name;
                }
            }
        }
    }
}
