using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using System.Collections;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VRollingResourceRequirePlanMaintain : TBasicDataView
    {
        MRollingDemandPlan model = new MRollingDemandPlan();

        private ResourceRequirePlan result;
        public ResourceRequirePlan Result
        {
            get { return result; }
            set { result = value; }
        }
        /// <summary>
        /// 操作{滚动资源需求计划}
        /// </summary>
        private ResourceRequirePlan operationResourceRequirePlan;
        private string opcode = "";//操作码
        private CurrentProjectInfo projectInfo = null;
        private PersonInfo loginPerson = null;
        private GWBSTree wbs = null;//操作工程任务
        //private bool isNew = false;//一个标记 用作判断是否重新获取
        private IList opDetailList;

        private IList deleteList = new ArrayList();
        //private IList updateList = new ArrayList();

        public VRollingResourceRequirePlanMaintain()
        {
            InitializeComponent();
        }

        public VRollingResourceRequirePlanMaintain(ResourceRequirePlan rrp, string strOpcode)
        {
            InitializeComponent();
            projectInfo = StaticMethod.GetProjectInfo();
            loginPerson = ConstObject.LoginPersonInfo;
            wbs = new GWBSTree();
            opDetailList = new ArrayList();
            operationResourceRequirePlan = rrp;
            opcode = strOpcode;

            if (opcode == "新增")
            {
                operationResourceRequirePlan = new ResourceRequirePlan();
                operationResourceRequirePlan.ProjectId = projectInfo.Id;
                operationResourceRequirePlan.ProjectName = projectInfo.Name;
                operationResourceRequirePlan.CreateDate = DateTime.Now;
                operationResourceRequirePlan.HandlePerson = loginPerson;
                operationResourceRequirePlan.HandlePersonName = loginPerson.Name;
                operationResourceRequirePlan.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                operationResourceRequirePlan.CreatePerson = loginPerson;
                operationResourceRequirePlan.CreatePersonName = loginPerson.Name;
                operationResourceRequirePlan.State = ResourceRequirePlanState.制定;
                ObjectQuery oq = new ObjectQuery();
                //wbs.State = 1;
                oq.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));
                oq.AddCriterion(Expression.Eq("CategoryNodeType", VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType.RootNode));
                oq.AddCriterion(Expression.Eq("State", 1));
                //wbs = model.ObjectQuery(typeof(GWBSTree), oq)[0] as GWBSTree;
                IList listGWGS = model.ObjectQuery(typeof(GWBSTree), oq);
                if (listGWGS != null && listGWGS.Count > 0)
                {
                    wbs = listGWGS[0] as GWBSTree;
                }
                operationResourceRequirePlan.TheGWBSTreeGUID = wbs;
                operationResourceRequirePlan.TheGWBSTreeName = wbs.Name;
                operationResourceRequirePlan.TheGWBSTreeSyscode = wbs.SysCode;
            }
            else
            {
                wbs = operationResourceRequirePlan.TheGWBSTreeGUID;
            }
            InitData();


            this.Shown += new EventHandler(VRollingResourceRequirePlanMaintain_Shown);
        }

        void VRollingResourceRequirePlanMaintain_Shown(object sender, EventArgs e)
        {
            txtPlanName.Focus();
        }
        void InitData()
        {
            ShowResourceRequirePlan(operationResourceRequirePlan);
            txtGWBSTree.Text = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), operationResourceRequirePlan.TheGWBSTreeName, operationResourceRequirePlan.TheGWBSTreeSyscode);
            InitEvents();
            InitControls();
        }
        void InitEvents()
        {
            btnSelectGWBSTree.Click += new EventHandler(btnSelectGWBSTree_Click);
            btnGetBudgetResourcesDemand.Click += new EventHandler(btnGetBudgetResourcesDemand_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnRelease.Click += new EventHandler(btnRelease_Click);
            btnQuit.Click += new EventHandler(btnQuit_Click);
            gridResourceRequireDetail.MouseClick += new MouseEventHandler(gridResourceRequireDetail_MouseClick);
            gridResourceRequireDetail.CellMouseClick += new DataGridViewCellMouseEventHandler(gridResourceRequireDetail_CellMouseClick);
            mnuResourceRequirePlanDetail.ItemClicked += new ToolStripItemClickedEventHandler(mnuResourceRequirePlanDetail_ItemClicked);

            //gridResourceRequireDetail.CellEndEdit += new DataGridViewCellEventHandler(gridResourceRequireDetail_CellEndEdit);
            gridResourceRequireDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridResourceRequireDetail_CellValidating);
            //gridResourceRequireDetail.CellValidated += new DataGridViewCellEventHandler(gridResourceRequireDetail_CellValidated);

            gridResourceRequireDetail.CellDoubleClick += new DataGridViewCellEventHandler(gridResourceRequireDetail_CellDoubleClick);
            btnSetQuality.Click += new EventHandler(btnSetQuality_Click);
        }

        void btnSetQuality_Click(object sender, EventArgs e)
        {

            if (gridResourceRequireDetail.Rows.Count == 0 || gridResourceRequireDetail[colQualityStandards.Name, 0].Value == null)
            {
                return;
            }
            string value = gridResourceRequireDetail[colQualityStandards.Name, 0].Value.ToString();
            foreach (DataGridViewRow var in this.gridResourceRequireDetail.Rows)
            {
                if (var.IsNewRow) break;


                var.Cells[colQualityStandards.Name].Value = value;
                ResourceRequirePlanDetail dtl = var.Tag as ResourceRequirePlanDetail;
                dtl.QualityStandards = value;
                var.Tag = dtl;
            }
        }

        void InitControls()
        {
            txtCreateTime.ReadOnly = true;
            txtPerson.ReadOnly = true;
            txtState.ReadOnly = true;
            txtGWBSTree.ReadOnly = true;

            if (opcode != "新增")
            {
                btnSelectGWBSTree.Enabled = false;
            }
            if (opcode == "显示")
            {
                btnGetBudgetResourcesDemand.Enabled = false;
                btnSave.Enabled = false;
                txtPlanName.ReadOnly = true;
                if (operationResourceRequirePlan.State != ResourceRequirePlanState.制定)
                {
                    btnRelease.Enabled = false;
                }
                btnQuit.Text = "关闭";
                gridResourceRequireDetail.ReadOnly = true;
                btnSetQuality.Enabled = false;
                //btnSelectGWBSTree.Enabled = false;
            }
        }


        #region 加载数据
        /// <summary>
        /// 加载滚动资源需求计划
        /// </summary>
        /// <param name="rrp"></param>
        void ShowResourceRequirePlan(ResourceRequirePlan rrp)
        {
            txtPlanName.Text = rrp.RequirePlanVersion;
            txtPerson.Text = rrp.HandlePersonName;
            txtCreateTime.Text = rrp.CreateDate.ToString();
            txtState.Text = rrp.State.ToString();
            ShowResourceRequireDetail(rrp);
        }
        /// <summary>
        /// 加载滚动资源需求计划明细
        /// </summary>
        /// <param name="rrp"></param>
        void ShowResourceRequireDetail(ResourceRequirePlan rrp)
        {
            gridResourceRequireDetail.Rows.Clear();
            IList list = SearchResourceRequireDetail(rrp);
            if (list != null && list.Count > 0)
            {
                List<ResourceRequirePlanDetail> rrpList = list.OfType<ResourceRequirePlanDetail>().ToList<ResourceRequirePlanDetail>();
                var query = from q in rrpList
                            orderby q.TheGWBSSysCode ascending
                            select q;

                foreach (ResourceRequirePlanDetail rrpDtl in query)
                {
                    AddGridResourceRequireDetail(rrpDtl);
                }
            }
        }
        IList SearchResourceRequireDetail(ResourceRequirePlan rrp)
        {
            if (!string.IsNullOrEmpty(rrp.Id))
            {
                //ResourceRequirePlanDetail d = new ResourceRequirePlanDetail();
                // d.TheResourceRequirePlan

                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("TheResourceRequirePlan.Id", rrp.Id));
                oq.AddFetchMode("TheGWBSTaskGUID", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("MaterialResource", NHibernate.FetchMode.Eager);
                return model.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
            }
            return null;
        }
        void AddGridResourceRequireDetail(ResourceRequirePlanDetail rrpDtl)
        {
            int rowIndex = gridResourceRequireDetail.Rows.Add();
            DataGridViewRow row = gridResourceRequireDetail.Rows[rowIndex];
            //row.Cells[TaskPath.Name].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), rrpDtl.TheGWBSTaskName, rrpDtl.TheGWBSSysCode);
            row.Cells[TaskPath.Name].Value = rrpDtl.TheGWBSTaskGUID.FullPath;
            row.Cells[ResourceType.Name].Value = rrpDtl.MaterialName;
            row.Cells[MaterialSpec.Name].Value = rrpDtl.MaterialSpec;
            row.Cells[DiagramNumber.Name].Value = rrpDtl.DiagramNumber;
            row.Cells[PlanType.Name].Value = rrpDtl.RequireType;
            //row.Cells[PlanInRequireQuantity.Name].Value = rrpDtl.PlanInRequireQuantity;
            //row.Cells[PlanOutRequireQuantity.Name].Value = rrpDtl.PlanOutRequireQuantity;
            //row.Cells[PlanTotalRequire.Name].Value = rrpDtl.PlanInRequireQuantity + rrpDtl.PlanOutRequireQuantity;
            row.Cells[PlannedCostQuantity.Name].Value = rrpDtl.PlannedCostQuantity;
            row.Cells[FirstOfferRequireQuantity.Name].Value = rrpDtl.FirstOfferRequireQuantity;
            row.Cells[MonthPlanPublishQuantity.Name].Value = rrpDtl.MonthPlanPublishQuantity;
            row.Cells[DailyPlanPublishQuantity.Name].Value = rrpDtl.DailyPlanPublishQuantity;
            row.Cells[ExecutedQuantity.Name].Value = rrpDtl.ExecutedQuantity;
            row.Cells[QuantityUnitName.Name].Value = rrpDtl.QuantityUnitName;
            row.Cells[State.Name].Value = rrpDtl.State;
            row.Cells[Descript.Name].Value = rrpDtl.Descript;
            row.Cells[TechnicalParameters.Name].Value = rrpDtl.TechnicalParameters;
            row.Cells[colQualityStandards.Name].Value = rrpDtl.QualityStandards;
            Decimal planTotalRequire = 0;
            if (rrpDtl.RequireType == PlanRequireType.计划内需求)
            {
                row.Cells[PlanTotalRequire.Name].Value = rrpDtl.PlanInRequireQuantity;
                planTotalRequire = rrpDtl.PlanInRequireQuantity;
                //row.Cells[PlanOutRequireQuantity.Name].ReadOnly = true;
                //row.Cells[PlanOutRequireQuantity.Name].Style.BackColor = ColorTranslator.FromHtml("#C0C0C0");
            }
            else
            {
                row.Cells[PlanTotalRequire.Name].Value = rrpDtl.PlanOutRequireQuantity;
                planTotalRequire = rrpDtl.PlanOutRequireQuantity;
                //row.Cells[PlanInRequireQuantity.Name].ReadOnly = true;
                //row.Cells[PlanInRequireQuantity.Name].Style.BackColor = ColorTranslator.FromHtml("#C0C0C0");
            }
            if (rrpDtl.PlannedCostQuantity != planTotalRequire)
            {
                row.Cells[PlanTotalRequire.Name].Style.BackColor = ColorTranslator.FromHtml("#D7E8FE");
            }
            row.Cells[SupplyPlanPublishQuantity.Name].Value = rrpDtl.SupplyPlanPublishQuantity;
            row.Tag = rrpDtl;
        }
        #endregion


        //void gridResourceRequireDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanInRequireQuantity.Name].ColumnIndex
        //        || e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanOutRequireQuantity.Name].ColumnIndex)
        //    {
        //        try
        //        {
        //            decimal planInRequireQuantity = Convert.ToDecimal(gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanInRequireQuantity.Name].Value.ToString());
        //            decimal planOutRequireQuantity = Convert.ToDecimal(gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanOutRequireQuantity.Name].Value.ToString());
        //            gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanTotalRequire.Name].Value = planInRequireQuantity + planOutRequireQuantity;

        //            ResourceRequirePlanDetail d = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
        //            d.PlanInRequireQuantity = planInRequireQuantity;
        //            d.PlanOutRequireQuantity = planOutRequireQuantity;
        //            gridResourceRequireDetail.Rows[e.RowIndex].Tag = d;
        //        }
        //        catch (Exception)
        //        {
        //            MessageBox.Show("数据输入有误，请重新输入！");
        //            //gridResourceRequireDetail.CurrentCell = gridResourceRequireDetail[e.ColumnIndex, e.RowIndex];
        //            //gridResourceRequireDetail[e.ColumnIndex, e.RowIndex]
        //        }
        //    }
        //}

        void gridResourceRequireDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            //if (e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanInRequireQuantity.Name].ColumnIndex
            //    || e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanOutRequireQuantity.Name].ColumnIndex)
            //{
            //    try
            //    {
            //        decimal planInRequireQuantity = Convert.ToDecimal(gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanInRequireQuantity.Name].EditedFormattedValue.ToString());
            //        decimal planOutRequireQuantity = Convert.ToDecimal(gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanOutRequireQuantity.Name].EditedFormattedValue.ToString());
            //        gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanTotalRequire.Name].Value = planInRequireQuantity + planOutRequireQuantity;

            //        ResourceRequirePlanDetail d = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
            //        d.PlanInRequireQuantity = planInRequireQuantity;
            //        d.PlanOutRequireQuantity = planOutRequireQuantity;
            //        gridResourceRequireDetail.Rows[e.RowIndex].Tag = d;
            //    }
            //    catch (Exception)
            //    {
            //        MessageBox.Show("数据输入有误，请重新输入！");
            //        e.Cancel = true;
            //    }
            //}
            if (e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanTotalRequire.Name].ColumnIndex)
            {
                try
                {
                    decimal planTotalRequire = Convert.ToDecimal(gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanTotalRequire.Name].EditedFormattedValue.ToString());
                    ResourceRequirePlanDetail d = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                    if (d.RequireType == PlanRequireType.计划内需求)
                    {
                        if (d.PlanInRequireQuantity != planTotalRequire)
                        {
                            d.PlanInRequireQuantity = planTotalRequire;
                            gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanTotalRequire.Name].Style.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        }
                    }
                    else
                    {
                        if (d.PlanOutRequireQuantity != planTotalRequire)
                        {
                            d.PlanOutRequireQuantity = planTotalRequire;
                            gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanTotalRequire.Name].Style.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                        }
                    }
                    gridResourceRequireDetail.Rows[e.RowIndex].Tag = d;
                }
                catch (Exception)
                {
                    MessageBox.Show("数据输入有误，请重新输入！");
                    e.Cancel = true;
                }
            }
            if (e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[Descript.Name].ColumnIndex)
            {
                ResourceRequirePlanDetail d = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                d.Descript = gridResourceRequireDetail.Rows[e.RowIndex].Cells[Descript.Name].EditedFormattedValue.ToString();
                gridResourceRequireDetail.Rows[e.RowIndex].Tag = d;
            }
            if (e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[TechnicalParameters.Name].ColumnIndex)
            {
                ResourceRequirePlanDetail d = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                d.TechnicalParameters = gridResourceRequireDetail.Rows[e.RowIndex].Cells[TechnicalParameters.Name].EditedFormattedValue.ToString();
                gridResourceRequireDetail.Rows[e.RowIndex].Tag = d;
            }
            if (e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[colQualityStandards.Name].ColumnIndex)
            {
                ResourceRequirePlanDetail d = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                d.QualityStandards = gridResourceRequireDetail.Rows[e.RowIndex].Cells[colQualityStandards.Name].EditedFormattedValue.ToString();
                gridResourceRequireDetail.Rows[e.RowIndex].Tag = d;
            }
            if (e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[SupplyPlanPublishQuantity.Name].ColumnIndex)
            {
                ResourceRequirePlanDetail d = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                d.SupplyPlanPublishQuantity = ClientUtil.ToDecimal(gridResourceRequireDetail.Rows[e.RowIndex].Cells[SupplyPlanPublishQuantity.Name].EditedFormattedValue);
                gridResourceRequireDetail.Rows[e.RowIndex].Tag = d;
            }
        }

        //void gridResourceRequireDetail_CellValidated(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanInRequireQuantity.Name].ColumnIndex
        //        || e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanOutRequireQuantity.Name].ColumnIndex)
        //    {
        //        try
        //        {
        //            decimal planInRequireQuantity = Convert.ToDecimal(gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanInRequireQuantity.Name].Value.ToString());
        //            decimal planOutRequireQuantity = Convert.ToDecimal(gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanOutRequireQuantity.Name].Value.ToString());
        //            gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanTotalRequire.Name].Value = planInRequireQuantity + planOutRequireQuantity;

        //            ResourceRequirePlanDetail d = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
        //            d.PlanInRequireQuantity = planInRequireQuantity;
        //            d.PlanOutRequireQuantity = planOutRequireQuantity;
        //            gridResourceRequireDetail.Rows[e.RowIndex].Tag = d;
        //        }
        //        catch (Exception)
        //        {
        //            MessageBox.Show("数据输入有误，请重新输入！");
        //        }
        //    }
        //}

        void gridResourceRequireDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[TaskPath.Name].ColumnIndex)
                {
                    VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(wbs);
                    frm.IsSelectSingleNode = true;
                    frm.ShowDialog();
                    List<TreeNode> list = frm.SelectResult;
                    if (list.Count > 0)
                    {
                        //bool flag = false;
                        GWBSTree checkWBS = (list[0] as TreeNode).Tag as GWBSTree;
                        ResourceRequirePlanDetail dtl = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                        foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                        {
                            ResourceRequirePlanDetail d = row.Tag as ResourceRequirePlanDetail;
                            if (row.Index != e.RowIndex)
                            {
                                if (dtl.MaterialResource.Id == d.MaterialResource.Id && dtl.DiagramNumber == d.DiagramNumber)
                                {
                                    if (checkWBS.SysCode.Contains(d.TheGWBSSysCode) || d.TheGWBSSysCode.Contains(checkWBS.SysCode))
                                    {
                                        MessageBox.Show("具有隶属关系的任务节点“" + row.Cells[TaskPath.Name].Value.ToString() + "”已存在相同资源", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                            }
                        }
                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[TaskPath.Name].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), checkWBS.Name, checkWBS.SysCode);
                        dtl.TheGWBSSysCode = checkWBS.SysCode;
                        dtl.TheGWBSTaskGUID = checkWBS;
                        dtl.TheGWBSTaskName = checkWBS.Name;
                        gridResourceRequireDetail.Rows[e.RowIndex].Tag = dtl;
                    }
                }
                if (e.ColumnIndex == gridResourceRequireDetail.Rows[e.RowIndex].Cells[QuantityUnitName.Name].ColumnIndex)
                {
                    StandardUnit su = UCL.Locate("计量单位维护", StandardUnitExcuteType.OpenSelect) as StandardUnit;
                    if (su != null)
                    {

                        this.gridResourceRequireDetail.CurrentRow.Cells[QuantityUnitName.Name].Tag = su;
                        this.gridResourceRequireDetail.CurrentRow.Cells[QuantityUnitName.Name].Value = su.Name;

                        ResourceRequirePlanDetail r = this.gridResourceRequireDetail.CurrentRow.Tag as ResourceRequirePlanDetail;
                        r.QuantityUnitGUID = su;
                        r.QuantityUnitName = su.Name;

                        this.gridResourceRequireDetail.CurrentRow.Tag = r;
                    }
                }
            }
        }



        #region 滚动资源需求计划明细右键菜单
        void gridResourceRequireDetail_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (opcode != "显示" && (operationResourceRequirePlan.State == ResourceRequirePlanState.制定 || operationResourceRequirePlan.State == ResourceRequirePlanState.发布))
                {
                    mnuResourceRequirePlanDetail.Items[itemAdd.Name].Enabled = true;
                    mnuResourceRequirePlanDetail.Items[itemDelete.Name].Enabled = false;
                    mnuResourceRequirePlanDetail.Items[itemAddPlanIn.Name].Enabled = false;
                    mnuResourceRequirePlanDetail.Items[查看合并明细.Name].Enabled = false;
                    mnuResourceRequirePlanDetail.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        void gridResourceRequireDetail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (opcode != "显示")
                {
                    if (e.RowIndex >= 0)
                    {
                        if (gridResourceRequireDetail.Rows[e.RowIndex].Selected == false)
                        {
                            gridResourceRequireDetail.ClearSelection();
                            gridResourceRequireDetail.Rows[e.RowIndex].Selected = true;
                        }
                        mnuResourceRequirePlanDetail.Items[itemAdd.Name].Enabled = true;
                        mnuResourceRequirePlanDetail.Items[itemDelete.Name].Enabled = true;
                        mnuResourceRequirePlanDetail.Items[itemAddPlanIn.Name].Enabled = true;
                        mnuResourceRequirePlanDetail.Items[查看合并明细.Name].Enabled = true;
                        mnuResourceRequirePlanDetail.Show(MousePosition.X, MousePosition.Y);
                    }
                }
                else
                {
                    if (e.RowIndex >= 0)
                    {
                        if (gridResourceRequireDetail.Rows[e.RowIndex].Selected == false)
                        {
                            gridResourceRequireDetail.ClearSelection();
                            gridResourceRequireDetail.Rows[e.RowIndex].Selected = true;
                        }
                        mnuResourceRequirePlanDetail.Items[itemAdd.Name].Enabled = false;
                        mnuResourceRequirePlanDetail.Items[itemDelete.Name].Enabled = false;
                        mnuResourceRequirePlanDetail.Items[itemAddPlanIn.Name].Enabled = false;
                        mnuResourceRequirePlanDetail.Items[查看合并明细.Name].Enabled = true;
                        mnuResourceRequirePlanDetail.Show(MousePosition.X, MousePosition.Y);
                    }
                }
            }
        }

        void mnuResourceRequirePlanDetail_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == itemAdd.Name || e.ClickedItem.Name == itemAddPlanIn.Name)//新增计划外、外工程量
            {
                mnuResourceRequirePlanDetail.Hide();
                List<ResourceRequirePlanDetail> rrpdList = new List<ResourceRequirePlanDetail>();
                ResourceRequirePlanDetail opResourceRequirePlanDetail = new ResourceRequirePlanDetail();
                if (gridResourceRequireDetail.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                    {
                        ResourceRequirePlanDetail rrpd = row.Tag as ResourceRequirePlanDetail;
                        rrpdList.Add(rrpd);
                    }
                }
                if (gridResourceRequireDetail.SelectedRows.Count > 0)
                {
                    opResourceRequirePlanDetail = gridResourceRequireDetail.SelectedRows[0].Tag as ResourceRequirePlanDetail;
                }
                if (e.ClickedItem.Name == itemAdd.Name)
                {
                    VAddUnplannedResourceDemand frm = new VAddUnplannedResourceDemand(opResourceRequirePlanDetail, rrpdList);
                    frm.typeIsInOrOut = PlanRequireType.计划外需求;
                    frm.rootNode = operationResourceRequirePlan.TheGWBSTreeGUID;
                    frm.ShowDialog();
                    ResourceRequirePlanDetail resultRRPD = frm.Result;
                    if (resultRRPD != null)
                    {
                        AddGridResourceRequireDetail(resultRRPD);
                    }
                }
                else
                {
                    //VAddPlanInResourceDemand frm = new VAddPlanInResourceDemand(opResourceRequirePlanDetail, rrpdList);
                    //frm.rootNode = operationResourceRequirePlan.TheGWBSTreeGUID;
                    //frm.ShowDialog();
                    //IList resultList = frm.ResultList;
                    //if (resultList != null && resultList.Count > 0)
                    //{
                    //    foreach (ResourceRequirePlanDetail dtl in resultList)
                    //    {
                    //        AddGridResourceRequireDetail(dtl);
                    //    }
                    //}

                    VAddUnplannedResourceDemand frm = new VAddUnplannedResourceDemand(opResourceRequirePlanDetail, rrpdList);
                    frm.typeIsInOrOut = PlanRequireType.计划内需求;
                    frm.rootNode = operationResourceRequirePlan.TheGWBSTreeGUID;
                    frm.ShowDialog();
                    ResourceRequirePlanDetail resultRRPD = frm.Result;
                    if (resultRRPD != null)
                    {
                        AddGridResourceRequireDetail(resultRRPD);
                    }
                }
            }
            else if (e.ClickedItem.Name == itemDelete.Name)//else if (e.ClickedItem.Text.Trim() == "删除")
            {
                mnuResourceRequirePlanDetail.Hide();
                ResourceRequirePlanDetail rrpd = gridResourceRequireDetail.SelectedRows[0].Tag as ResourceRequirePlanDetail;
                if (rrpd.State == ResourceRequirePlanDetailState.作废)
                {
                    MessageBox.Show("已是作废状态！");
                    return;
                }
                if (MessageBox.Show("确定删除吗？", "删除提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (rrpd.State == ResourceRequirePlanDetailState.编制)
                    {
                        if (rrpd.Id != null)
                        {
                            //IList deleteList = new ArrayList();
                            deleteList.Add(rrpd);
                            //model.DeleteResourceRequirePlanDetail(deleteList);
                        }
                        //MessageBox.Show("删除成功");
                        gridResourceRequireDetail.Rows.Remove(gridResourceRequireDetail.SelectedRows[0]);
                    }
                    else
                    {
                        rrpd.State = ResourceRequirePlanDetailState.作废;
                        //model.SaveOrUpdateResourcePlanDetail(rrpd);
                        //updateList.Add(rrpd);
                        MessageBox.Show("已作废");
                        gridResourceRequireDetail.SelectedRows[0].Cells[State.Name].Value = rrpd.State;
                        gridResourceRequireDetail.SelectedRows[0].Tag = rrpd;
                    }
                }
            }
            else if (e.ClickedItem.Name == 查看合并明细.Name)
            {
                mnuResourceRequirePlanDetail.Hide();
                ResourceRequirePlanDetail rrpd = gridResourceRequireDetail.SelectedRows[0].Tag as ResourceRequirePlanDetail;
                VShowGWBSDetailCostSubject frm = new VShowGWBSDetailCostSubject(rrpd, operationResourceRequirePlan);
                frm.ShowDialog();
            }
        }
        #endregion

        #region 按钮操作
        /// <summary>
        /// 选择工程任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSelectGWBSTree_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree_OnlyOne frm = new VSelectGWBSTree_OnlyOne(new MGWBSTree());
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
        /// <summary>
        /// 获取预算资源需求量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnGetBudgetResourcesDemand_Click(object sender, EventArgs e)
        {
            try
            {
                if (operationResourceRequirePlan.TheGWBSTreeGUID.Id != wbs.Id)
                {
                    operationResourceRequirePlan.TheGWBSTreeGUID = wbs;
                    operationResourceRequirePlan.TheGWBSTreeName = wbs.Name;
                    operationResourceRequirePlan.TheGWBSTreeSyscode = wbs.SysCode;
                }
                FlashScreen.Show("正在获取预算资源需求量......");
                IList rrpDtlList1 = model.GetBudgetResourcesDemand(operationResourceRequirePlan, projectInfo, wbs);
                IList rrpDtlList = new ArrayList();
                if (rrpDtlList1 != null && rrpDtlList1.Count >= 0)
                {
                    foreach (ResourceRequirePlanDetail d in rrpDtlList1)
                    {
                        if (d.PlannedCostQuantity != 0)
                        {
                            rrpDtlList.Add(d);
                        }
                    }
                }
                if (btnSelectGWBSTree.Enabled)
                {
                    gridResourceRequireDetail.Rows.Clear();
                    if (rrpDtlList != null && rrpDtlList.Count > 0)
                    {

                        List<ResourceRequirePlanDetail> list = rrpDtlList.OfType<ResourceRequirePlanDetail>().ToList<ResourceRequirePlanDetail>();
                        var quey = from q in list
                                   orderby q.TheGWBSSysCode ascending
                                   select q;

                        foreach (ResourceRequirePlanDetail rrpDtl in quey)
                        {
                            AddGridResourceRequireDetail(rrpDtl);
                        }
                    }
                }
                else
                {
                    #region  更新资源需求量
                    string messages = "";
                    foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                    {
                        bool flag = false;//旧数据里某资源是否存在新获取的数据里
                        bool isRemove = false;//是否把新集合里的对象移除
                        ResourceRequirePlanDetail removeDtl = new ResourceRequirePlanDetail();
                        ResourceRequirePlanDetail rrrDtlOld = row.Tag as ResourceRequirePlanDetail;
                        for (int i = 0; i < rrpDtlList.Count; i++)
                        {
                            ResourceRequirePlanDetail rrpDtlNew = rrpDtlList[i] as ResourceRequirePlanDetail;
                            removeDtl = rrpDtlNew;
                            if (rrpDtlNew.DiagramNumber == rrrDtlOld.DiagramNumber && rrpDtlNew.RequireType == rrrDtlOld.RequireType)
                            {
                                bool flagIsEqual = false;
                                if (rrpDtlNew.MaterialResource == null || rrrDtlOld.MaterialResource == null)
                                {
                                    if (rrpDtlNew.MaterialResource == null && rrrDtlOld.MaterialResource == null)
                                    {
                                        flagIsEqual = true;
                                    }
                                }
                                else
                                {
                                    if (rrpDtlNew.MaterialResource.Id == rrrDtlOld.MaterialResource.Id)
                                    {
                                        flagIsEqual = true;
                                    }
                                }
                                if (flagIsEqual)
                                {
                                    flag = true;
                                    if (rrpDtlNew.TheGWBSSysCode == rrrDtlOld.TheGWBSSysCode)
                                    {
                                        #region 修改数据
                                        if (rrrDtlOld.PlanInRequireQuantity == rrrDtlOld.PlannedCostQuantity)
                                        {
                                            if (rrrDtlOld.PlannedCostQuantity != rrpDtlNew.PlannedCostQuantity)
                                            {
                                                rrrDtlOld.PlanInRequireQuantity = rrpDtlNew.PlanInRequireQuantity;
                                                rrrDtlOld.PlannedCostQuantity = rrpDtlNew.PlannedCostQuantity;
                                                row.Cells[PlanTotalRequire.Name].Value = rrpDtlNew.PlanInRequireQuantity;
                                                row.Cells[PlannedCostQuantity.Name].Value = rrpDtlNew.PlannedCostQuantity;
                                            }
                                        }
                                        else
                                        {
                                            if (rrrDtlOld.PlannedCostQuantity != rrpDtlNew.PlannedCostQuantity)
                                            {
                                                rrrDtlOld.PlannedCostQuantity = rrpDtlNew.PlannedCostQuantity;
                                                row.Cells[PlannedCostQuantity.Name].Value = rrpDtlNew.PlannedCostQuantity;
                                            }
                                            row.Cells[PlanTotalRequire.Name].Style.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                                        }
                                        isRemove = true;
                                        break;
                                        #endregion
                                    }
                                    else if (rrpDtlNew.TheGWBSSysCode.Contains(rrrDtlOld.TheGWBSSysCode))
                                    {
                                        messages += "资源类型“" + rrrDtlOld.MaterialName + "“被删除且该在该核算节点“" + row.Cells[TaskPath.Name].Value.ToString() + "”的下属核算节点上提了相同的资源类型";
                                        isRemove = true;
                                        break;
                                    }
                                    else if (rrrDtlOld.TheGWBSSysCode.Contains(rrpDtlNew.TheGWBSSysCode))
                                    {
                                        messages += "在核算节点“" + row.Cells[TaskPath.Name].Value.ToString() + "“的父核算节点上提了相同资源；";
                                        isRemove = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (!flag && rrrDtlOld.RequireType == PlanRequireType.计划内需求)
                        {
                            rrrDtlOld.PlannedCostQuantity = 0;
                            rrrDtlOld.State = ResourceRequirePlanDetailState.作废;
                            row.Cells[PlanTotalRequire.Name].Style.BackColor = ColorTranslator.FromHtml("#D7E8FE");
                            row.Cells[State.Name].Value = rrrDtlOld.State;
                            row.Cells[PlannedCostQuantity.Name].Value = 0;
                        }
                        if (isRemove)
                        {
                            rrpDtlList.Remove(removeDtl);
                        }

                        row.Tag = rrrDtlOld;
                    }
                    if (rrpDtlList.Count > 0)
                    {
                        foreach (ResourceRequirePlanDetail dtl in rrpDtlList)
                        {
                            AddGridResourceRequireDetail(dtl);
                        }
                    }
                    if (messages != "")
                    {
                        FlashScreen.Close();
                        MessageBox.Show(messages, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    #endregion
                }
                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("获取失败：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        //void btnGetBudgetResourcesDemand_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        IList updateList = new ArrayList();
        //        IList addList = new ArrayList();
        //        FlashScreen.Show("正在获取预算资源需求量......");
        //        IList addOrUpdateList = model.GetBudgetResourcesDemand(operationResourceRequirePlan, projectInfo,wbs);
        //        if (addOrUpdateList != null && addOrUpdateList.Count > 0)
        //        {
        //            foreach (ResourceRequirePlanDetail d in addOrUpdateList)
        //            {
        //                if (d.Id == null)
        //                {
        //                    addList.Add(d);
        //                }
        //                else
        //                {
        //                    updateList.Add(d);
        //                }
        //            }
        //        }
        //        #region 加载数据
        //        if (updateList != null && updateList.Count > 0)
        //        {
        //            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
        //            {
        //                ResourceRequirePlanDetail rd = row.Tag as ResourceRequirePlanDetail;
        //                ResourceRequirePlanDetail remove = new ResourceRequirePlanDetail();
        //                for (int i = 0; i < updateList.Count; i++)
        //                {
        //                    ResourceRequirePlanDetail d = updateList[i] as ResourceRequirePlanDetail;
        //                    if (d.Id == rd.Id)
        //                    {
        //                        UpdateGridResourceRequireDetail(d, row);
        //                        remove = d;
        //                        break;
        //                    }
        //                }
        //                //updateList.Remove(remove);
        //            }
        //        }
        //        if (addList != null && addList.Count > 0)
        //        {
        //            foreach (ResourceRequirePlanDetail d in addList)
        //            {
        //                bool flag = true;
        //                foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
        //                {
        //                    ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
        //                    if (dtl.Id == null && dtl.MaterialResource.Id == d.MaterialResource.Id && dtl.DiagramNumber == d.DiagramNumber && dtl.TheGWBSTaskGUID.Id == d.TheGWBSTaskGUID.Id)
        //                    {
        //                        UpdateGridResourceRequireDetail(d, row);
        //                        flag = false;
        //                    }
        //                }
        //                if (flag)
        //                {
        //                    AddGridResourceRequireDetail(d);
        //                }
        //            }
        //        }
        //        #endregion
        //        //FlashScreen.Close();
        //        //MessageBox.Show("获取成功！");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("获取失败：" + ExceptionUtil.ExceptionMessage(ex));
        //        //FlashScreen.Close();
        //    }
        //    finally
        //    {
        //        FlashScreen.Close();
        //    }
        //    //btnGetBudgetResourcesDemand.Enabled = false;
        //}

        void UpdateGridResourceRequireDetail(ResourceRequirePlanDetail d, DataGridViewRow row)
        {
            row.Cells[TaskPath.Name].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), d.TheGWBSTaskName, d.TheGWBSSysCode);
            row.Cells[ResourceType.Name].Value = d.MaterialName;
            row.Cells[DiagramNumber.Name].Value = d.DiagramNumber;
            row.Cells[PlanType.Name].Value = d.RequireType;
            row.Cells[PlanInRequireQuantity.Name].Value = d.PlanInRequireQuantity;
            if (ClientUtil.ToDecimal(row.Cells[PlanOutRequireQuantity.Name].Value) != 0)
                row.Cells[PlanOutRequireQuantity.Name].Value = ClientUtil.ToDecimal(row.Cells[PlanOutRequireQuantity.Name].Value);
            else
                row.Cells[PlanOutRequireQuantity.Name].Value = d.PlanOutRequireQuantity;
            row.Cells[PlanTotalRequire.Name].Value = d.PlanInRequireQuantity + d.PlanOutRequireQuantity;
            row.Cells[PlannedCostQuantity.Name].Value = d.PlannedCostQuantity;
            row.Cells[FirstOfferRequireQuantity.Name].Value = d.FirstOfferRequireQuantity;
            row.Cells[MonthPlanPublishQuantity.Name].Value = d.MonthPlanPublishQuantity;
            row.Cells[DailyPlanPublishQuantity.Name].Value = d.DailyPlanPublishQuantity;
            row.Cells[SupplyPlanPublishQuantity.Name].Value = d.SupplyPlanPublishQuantity;
            row.Cells[ExecutedQuantity.Name].Value = d.ExecutedQuantity;
            row.Cells[QuantityUnitName.Name].Value = d.QuantityUnitName;
            row.Cells[State.Name].Value = d.State;

            row.Tag = d;
        }


        //保存
        void btnSave_Click(object sender, EventArgs e)
        {
            if (!Verify()) return;
            try
            {
                operationResourceRequirePlan.RequirePlanVersion = txtPlanName.Text.Trim();
                IList rrpdList = new ArrayList();

                if (gridResourceRequireDetail.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                    {
                        ResourceRequirePlanDetail rrpd = row.Tag as ResourceRequirePlanDetail;
                        rrpdList.Add(rrpd);
                    }
                }
                Hashtable ht = model.SaveOrUpdateResourcePlanAndDetail(operationResourceRequirePlan, rrpdList, false, deleteList);
                result = operationResourceRequirePlan = (ResourceRequirePlan)ht["plan"];
                opDetailList = (IList)ht["list"];
                MessageBox.Show("保存成功！");
                //isNew = false;
                btnSelectGWBSTree.Enabled = false;
                gridResourceRequireDetail.Rows.Clear();
                foreach (ResourceRequirePlanDetail dtl in opDetailList)
                {
                    AddGridResourceRequireDetail(dtl);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败，错误信息：" + ExceptionUtil.ExceptionMessage(ex));
            }


        }
        //发布
        void btnRelease_Click(object sender, EventArgs e)
        {
            if (!Verify()) return;
            //if (operationResourceRequirePlan.State != ResourceRequirePlanState.制定)
            //{
            //    MessageBox.Show("滚动资源需求计划制定状态下才能发布！");
            //    return;
            //}
            try
            {
                operationResourceRequirePlan.RequirePlanVersion = txtPlanName.Text.Trim();
                operationResourceRequirePlan.State = ResourceRequirePlanState.发布;
                operationResourceRequirePlan.AuditDate = DateTime.Now;
                IList rrpdList = new ArrayList();

                if (gridResourceRequireDetail.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                    {
                        ResourceRequirePlanDetail rrpd = row.Tag as ResourceRequirePlanDetail;
                        rrpdList.Add(rrpd);
                    }
                }
                Hashtable ht = model.SaveOrUpdateResourcePlanAndDetail(operationResourceRequirePlan, rrpdList, true, deleteList);
                result = operationResourceRequirePlan = (ResourceRequirePlan)ht["plan"];
                opDetailList = (IList)ht["list"];
                MessageBox.Show("发布成功！");
                //btnSelectGWBSTree.Enabled = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发布失败,错误信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        //放弃
        void btnQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool Verify()
        {
            if (txtPlanName.Text.Trim() == "")
            {
                MessageBox.Show("请输入计划名称！");
                return false;
            }
            if (gridResourceRequireDetail.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                {
                    ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                    if (dtl.State != ResourceRequirePlanDetailState.作废)
                    {
                        if (ClientUtil.ToDecimal(row.Cells[PlanTotalRequire.Name].EditedFormattedValue.ToString()) <= 0)
                        {
                            MessageBox.Show("计划需求量不能为零为零，请检查！");
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        #endregion


    }
}
