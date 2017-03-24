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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VRollingDemandPlanBak : TBasicDataView
    {
        private ResourceRequirePlan optPlan = null;
        private GWBSTree optGWBS = null;

        public MRollingDemandPlan model;

        public VRollingDemandPlanBak(MRollingDemandPlan mot)
        {
            model = mot;

            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            InitEvents();

            //资源需求计划类型
            IList list = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetBasicDataByName(VBasicDataOptr.PLAN_ResourceRequirePlanType);
            if (list != null)
            {
                foreach (BasicDataOptr bdo in list)
                {
                    cbPlanType.Items.Add(bdo.BasicName);
                }
            }
            if (cbPlanType.Items.Count > 0)
                cbPlanType.SelectedIndex = 0;

            foreach (string type in Enum.GetNames(typeof(ResourceRequirePlanDetailResourceType)))
            {
                ResourceType.Items.Add(type);
            }

            DateTime now = model.GetServerTime();
            dtMadeStartDateQuery.Value = now.Date.AddDays(-30);
            dtMadeEndDateQuery.Value = now.Date;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);


            cbPlanType.SelectedIndexChanged += new EventHandler(cbPlanType_SelectedIndexChanged);

            btnSaveMaster.Click += new EventHandler(btnSaveMaster_Click);
            btnPublishMaster.Click += new EventHandler(btnPublishMaster_Click);
            btnFreezeMaster.Click += new EventHandler(btnFreezeMaster_Click);
            btnCancellationMaster.Click += new EventHandler(btnCancellationMaster_Click);

            btnSelectWBSNode.Click += new EventHandler(btnSelectWBSNode_Click);
            tvwCategory.AfterSelect += new TreeViewEventHandler(tvwCategory_AfterSelect);

            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnPublish.Click += new EventHandler(btnPublish_Click);
            btnCancellation.Click += new EventHandler(btnCancellation_Click);

            gridResourceRequireDetail.CellValidating += new DataGridViewCellValidatingEventHandler(gridResourceRequireDetail_CellValidating);

            gridResourceRequireDetail.CellEndEdit += new DataGridViewCellEventHandler(gridResourceRequireDetail_CellEndEdit);

            gridResourceRequireDetail.CellDoubleClick += new DataGridViewCellEventHandler(gridResourceRequireDetail_CellDoubleClick);
        }

        #region 资源需求计划主表
        //查询
        private void btnQuery_Click(object sender, EventArgs e)
        {
            //保存校验
            bool isSaveFlag = true;
            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                if (!row.ReadOnly)
                {
                    isSaveFlag = false;
                    break;
                }
            }
            if (!isSaveFlag)
            {
                if (MessageBox.Show("明细尚未保存，要保存吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!ValidateSaveDetail())
                        return;
                }
            }


            string planVersion = txtPlanVersionQuery.Text.Trim();

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

            PersonInfo responsibilityPerson = null;
            if (txtResponsibilityPersonQuery.Result != null && txtResponsibilityPersonQuery.Result.Count > 0)
                responsibilityPerson = txtResponsibilityPersonQuery.Result[0] as PersonInfo;

            string spec = txtSpecification.Text.Trim();
            string nodeName = txtNodeNameQuery.Text.Trim();

            ObjectQuery oq = new ObjectQuery();

            ObjectQuery oqMain = new ObjectQuery();

            if (responsibilityPerson != null)
                oqMain.AddCriterion(Expression.Eq("HandlePerson", responsibilityPerson.Id));

            if (!string.IsNullOrEmpty(planVersion))
                oqMain.AddCriterion(Expression.Like("RequirePlanVersion", planVersion, MatchMode.Anywhere));

            IList listPlan = null;
            if (oqMain.Criterions.Count > 0)//处理关联查询不能使用模糊查询
            {
                listPlan = model.ObjectQuery(typeof(ResourceRequirePlan), oqMain);
                if (listPlan.Count > 0)
                {
                    Disjunction dis = new Disjunction();
                    foreach (ResourceRequirePlan plan in listPlan)
                    {
                        dis.Add(Expression.Eq("TheResourceRequirePlan.Id", plan.Id));
                    }
                    oq.AddCriterion(dis);

                    oq.AddCriterion(Expression.Ge("TheResourceRequirePlan.CreatDate", madeStartDate));
                    oq.AddCriterion(Expression.Le("TheResourceRequirePlan.CreatDate", madeEndDate));

                    if (!string.IsNullOrEmpty(spec))
                        oq.AddCriterion(Expression.Like("MaterialSpec", spec, MatchMode.Anywhere));

                    if (!string.IsNullOrEmpty(nodeName))
                        oq.AddCriterion(Expression.Like("TheGWBSTaskName", nodeName, MatchMode.Anywhere));
                }
            }
            else
            {
                oq.AddCriterion(Expression.Ge("TheResourceRequirePlan.CreatDate", madeStartDate));
                oq.AddCriterion(Expression.Le("TheResourceRequirePlan.CreatDate", madeEndDate));


                if (!string.IsNullOrEmpty(spec))
                    oq.AddCriterion(Expression.Like("MaterialSpec", spec, MatchMode.Anywhere));

                if (!string.IsNullOrEmpty(nodeName))
                    oq.AddCriterion(Expression.Like("TheGWBSTaskName", nodeName, MatchMode.Anywhere));
            }

            IList list = null;
            if (oq.Criterions.Count > 0)
            {
                oq.AddOrder(NHibernate.Criterion.Order.Asc("CreatDate"));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("TheGWBSTaskName"));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("BuildResourceTypeName"));
                oq.AddOrder(NHibernate.Criterion.Order.Asc("ServiceType"));

                list = model.ObjectQuery(typeof(ResourceRequirePlanDetail), oq);
            }


            gridResourceRequireDetail.Rows.Clear();
            if (list != null)
            {
                foreach (ResourceRequirePlanDetail tempDtl in list)
                {
                    AddResourceRequireDetailInGrid(tempDtl, false, true);
                }

                gridResourceRequireDetail.ClearSelection();
            }
        }

        void btnSelectWBSNode_Click(object sender, EventArgs e)
        {
            VSelectGWBSTree frm = new VSelectGWBSTree(new MGWBSTree());
            frm.IsTreeSelect = true;
            frm.ShowDialog();
            if (frm.SelectResult.Count > 0)
            {
                tvwCategory.Nodes.Clear();

                TreeNode selectRootNode = frm.SelectResult[0];
                TreeNode root = new TreeNode();
                root.Name = selectRootNode.Name;
                root.Text = selectRootNode.Text;
                root.Tag = selectRootNode.Tag;
                tvwCategory.Nodes.Add(root);

                LoadSelectTaskTree(root, selectRootNode);
            }
        }

        private void tvwCategory_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                optGWBS = tvwCategory.SelectedNode.Tag as GWBSTree;

                gridResourceRequireDetail.ClearSelection();

                bool selectEndRowFlag = false;//滚动条移动到选择行最后
                for (int i = gridResourceRequireDetail.Rows.Count - 1; i > -1; i--)
                {
                    DataGridViewRow row = gridResourceRequireDetail.Rows[i];

                    ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                    if (dtl.TheGWBSTaskGUID.Id == optGWBS.Id)
                    {
                        row.Selected = true;

                        if (selectEndRowFlag == false)
                        {
                            selectEndRowFlag = true;
                            gridResourceRequireDetail.CurrentCell = row.Cells[0];
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ee));
            }
        }

        private void LoadSelectTaskTree(TreeNode parentNode, TreeNode selectNode)
        {
            foreach (TreeNode tn in selectNode.Nodes)
            {
                TreeNode node = new TreeNode();
                node.Name = tn.Name;
                node.Text = tn.Text;
                node.Tag = tn.Tag;
                parentNode.Nodes.Add(node);

                LoadSelectTaskTree(node, tn);
            }
        }

        void cbPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isSaveFlag = true;
            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                if (!row.ReadOnly)
                {
                    isSaveFlag = false;
                    break;
                }
            }
            if (!isSaveFlag)
            {
                if (MessageBox.Show("明细尚未保存，要保存吗？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!ValidateSaveDetail())
                        return;
                }
            }

            string type = cbPlanType.SelectedItem.ToString();

            ObjectQuery oq = new ObjectQuery();

            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            oq.AddCriterion(Expression.Eq("PlanType", type));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList list = model.ObjectQuery(typeof(ResourceRequirePlan), oq);

            if (list.Count > 0)
            {
                optPlan = list[0] as ResourceRequirePlan;
            }
            else
                optPlan = null;

            ClearRequirePlan();

            LoadResourceRequirePlanData(true);
        }

        private void ClearRequirePlan()
        {
            txtOwner.Text = "";
            txtPlanVersion.Text = "";
            txtCreateTime.Text = "";
            txtState.Text = "";

            //optGWBS = null;
            //tvwCategory.Nodes.Clear();
            gridResourceRequireDetail.Rows.Clear();
        }

        private void LoadResourceRequirePlanData(bool isLoadDetails)
        {
            if (optPlan != null)
            {
                txtOwner.Text = optPlan.HandlePersonName;
                txtPlanVersion.Text = optPlan.RequirePlanVersion;
                txtCreateTime.Text = optPlan.CreateDate.ToString();
                txtState.Text = optPlan.State.ToString();

                if (isLoadDetails)
                {
                    var query = from dtl in optPlan.Details
                                //orderby dtl.ServiceType ascending
                                orderby dtl.BuildResourceTypeName ascending
                                orderby dtl.TheGWBSTaskName ascending
                                orderby dtl.CreateTime ascending
                                select dtl;

                    foreach (ResourceRequirePlanDetail dtl in query)
                    {
                        AddResourceRequireDetailInGrid(dtl, false, true);
                    }
                }
            }
        }

        void btnSaveMaster_Click(object sender, EventArgs e)
        {
            if (cbPlanType.SelectedItem == null || string.IsNullOrEmpty(cbPlanType.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择计划类型！");
                return;
            }
            try
            {
                if (optPlan == null)
                {
                    optPlan = new ResourceRequirePlan();
                    optPlan.PlanType = cbPlanType.SelectedItem.ToString();
                    optPlan.HandlePerson = ConstObject.LoginPersonInfo;
                    optPlan.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                    optPlan.OwnerOrgSysCode = ConstObject.TheOperationOrg.SysCode;
                    optPlan.CreateDate = model.GetServerTime();
                    optPlan.RequirePlanVersion = optPlan.PlanType + string.Format("{0:yyyyMMdd}", optPlan.CreateDate);

                    optPlan.State = ResourceRequirePlanState.制定;

                    CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        optPlan.ProjectId = projectInfo.Id;
                        optPlan.ProjectName = projectInfo.Name;
                    }

                    optPlan = model.SaveOrUpdateResourceRequirePlan(optPlan);

                    LogData log = new LogData();
                    log.BillId = optPlan.Id;
                    log.BillType = "滚动需求计划";
                    log.Code = optPlan.Code;
                    log.OperType = "新增";
                    log.Descript = "";
                    log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = optPlan.ProjectName;
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);
                }

                LoadResourceRequirePlanData(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnPublishMaster_Click(object sender, EventArgs e)
        {
            if (cbPlanType.SelectedItem == null || string.IsNullOrEmpty(cbPlanType.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择计划类型！");
                return;
            }
            if (optPlan == null)
            {
                MessageBox.Show("请先保存计划！");
                return;
            }
            else if (optPlan.State != ResourceRequirePlanState.制定 && optPlan.State != ResourceRequirePlanState.冻结)
            {
                MessageBox.Show("只能发布状态为‘制定’或‘冻结’的计划！");
                return;
            }

            try
            {
                optPlan.State = ResourceRequirePlanState.发布;

                optPlan = model.SaveOrUpdateResourceRequirePlan(optPlan);

                LoadResourceRequirePlanData(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnFreezeMaster_Click(object sender, EventArgs e)
        {
            if (cbPlanType.SelectedItem == null || string.IsNullOrEmpty(cbPlanType.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择计划类型！");
                return;
            }
            if (optPlan == null)
            {
                MessageBox.Show("请先保存计划！");
                return;
            }
            else if (optPlan.State != ResourceRequirePlanState.发布)
            {
                MessageBox.Show("只能冻结状态为‘发布’的计划！");
                return;
            }

            try
            {
                optPlan.State = ResourceRequirePlanState.冻结;

                optPlan = model.SaveOrUpdateResourceRequirePlan(optPlan);

                LoadResourceRequirePlanData(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        void btnCancellationMaster_Click(object sender, EventArgs e)
        {
            if (cbPlanType.SelectedItem == null || string.IsNullOrEmpty(cbPlanType.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择计划类型！");
                return;
            }
            if (optPlan == null)
            {
                MessageBox.Show("请先保存计划！");
                return;
            }
            else if (optPlan.State != ResourceRequirePlanState.发布 && optPlan.State != ResourceRequirePlanState.冻结)
            {
                MessageBox.Show("只能作废状态为‘发布’或‘冻结’的计划！");
                return;
            }

            try
            {
                optPlan.State = ResourceRequirePlanState.作废;

                optPlan = model.SaveOrUpdateResourceRequirePlan(optPlan);

                LoadResourceRequirePlanData(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        #endregion

        #region 资源需求计划明细
        void btnAdd_Click(object sender, EventArgs e)
        {
            if (optPlan == null)
            {
                MessageBox.Show("请先保存主表信息！");
                btnSaveMaster.Focus();
                return;
            }
            else if (optGWBS == null)
            {
                MessageBox.Show("请先选择一个工程任务节点！");
                btnSelectWBSNode.Focus();
                return;
            }

            ResourceRequirePlanDetail dtl = new ResourceRequirePlanDetail();
            dtl.TheGWBSTaskGUID = optGWBS;
            dtl.TheGWBSTaskName = optGWBS.Name;

            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            if (projectInfo != null)
            {
                dtl.TheProjectGUID = projectInfo.Id;
                dtl.TheProjectName = projectInfo.Name;
            }
            dtl.State = ResourceRequirePlanDetailState.编制;
            DateTime serverTime = model.GetServerTime();
            dtl.StateUpdateTime = serverTime;
            dtl.PlanBeginApproachDate = serverTime.Date;
            dtl.PlanEndApproachDate = serverTime.Date;

            AddResourceRequireDetailInGrid(dtl, true, false);

            gridResourceRequireDetail.BeginEdit(false);
        }

        void gridResourceRequireDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1 && gridResourceRequireDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                ResourceRequirePlanDetail dtl = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ServiceType.Name &&
                    gridResourceRequireDetail.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString() == "劳务")//劳务类型
                {
                    VSelectServiceType frm = new VSelectServiceType();
                    frm.ShowDialog();
                    if (frm.SelectServiceType.Count > 0)
                    {
                        dtl.ServiceType = frm.SelectServiceType[0];
                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[ServiceType.Name].Value = dtl.ServiceType;
                        gridResourceRequireDetail.Rows[e.RowIndex].Tag = dtl;

                        CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                        DateTime serverTime = model.GetServerTime();
                        for (int i = 1; i < frm.SelectServiceType.Count; i++)
                        {
                            ResourceRequirePlanDetail tempDtl = new ResourceRequirePlanDetail();
                            tempDtl.TheGWBSTaskGUID = dtl.TheGWBSTaskGUID;
                            tempDtl.TheGWBSTaskName = dtl.TheGWBSTaskName;
                            if (projectInfo != null)
                            {
                                tempDtl.TheProjectGUID = projectInfo.Id;
                                tempDtl.TheProjectName = projectInfo.Name;
                            }
                            tempDtl.State = ResourceRequirePlanDetailState.编制;
                            tempDtl.StateUpdateTime = serverTime;
                            tempDtl.PlanBeginApproachDate = serverTime.Date;
                            tempDtl.PlanEndApproachDate = serverTime.Date;

                            tempDtl.BuildResourceTypeName = ResourceRequirePlanDetailResourceType.劳务.ToString();

                            tempDtl.ServiceType = frm.SelectServiceType[i];

                            InsertResourceRequireDetailInGrid(tempDtl, e.RowIndex);
                        }
                    }
                }
                else if ((gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ResourceCode.Name ||
                    gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ResourceName.Name ||
                    gridResourceRequireDetail.Columns[e.ColumnIndex].Name == Spec.Name)
                    && gridResourceRequireDetail.Rows[e.RowIndex].Cells[ResourceType.Name].Value.ToString() == "物资")//选择物料
                {
                    CommonMaterial materialSelector = new CommonMaterial();
                    materialSelector.OpenSelect();

                    IList list = materialSelector.Result;
                    if (list.Count > 0)
                    {
                        Material mat = list[0] as Material;
                        dtl.MaterialResource = mat;
                        dtl.MaterialCode = mat.Code;
                        dtl.MaterialName = mat.Name;
                        dtl.MaterialSpec = mat.Specification;

                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[ResourceCode.Name].Value = dtl.MaterialCode;
                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[ResourceName.Name].Value = dtl.MaterialName;
                        gridResourceRequireDetail.Rows[e.RowIndex].Cells[Spec.Name].Value = dtl.MaterialSpec;
                        gridResourceRequireDetail.Rows[e.RowIndex].Tag = dtl;

                        CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                        DateTime serverTime = model.GetServerTime();
                        for (int i = 1; i < list.Count; i++)
                        {
                            Material theMaterial = list[i] as Material;
                            ResourceRequirePlanDetail tempDtl = new ResourceRequirePlanDetail();
                            tempDtl.TheGWBSTaskGUID = dtl.TheGWBSTaskGUID;
                            tempDtl.TheGWBSTaskName = dtl.TheGWBSTaskName;
                            if (projectInfo != null)
                            {
                                tempDtl.TheProjectGUID = projectInfo.Id;
                                tempDtl.TheProjectName = projectInfo.Name;
                            }
                            tempDtl.State = ResourceRequirePlanDetailState.编制;
                            tempDtl.StateUpdateTime = serverTime;
                            tempDtl.PlanBeginApproachDate = serverTime.Date;
                            tempDtl.PlanEndApproachDate = serverTime.Date;

                            tempDtl.BuildResourceTypeName = ResourceRequirePlanDetailResourceType.物资.ToString();

                            tempDtl.MaterialResource = theMaterial;
                            tempDtl.MaterialCode = theMaterial.Code;
                            tempDtl.MaterialName = theMaterial.Name;
                            tempDtl.MaterialSpec = theMaterial.Specification;

                            InsertResourceRequireDetailInGrid(tempDtl, e.RowIndex);
                        }

                        gridResourceRequireDetail.CurrentCell = gridResourceRequireDetail.Rows[e.RowIndex].Cells[ResourceName.Name];
                    }
                }
            }
        }

        void gridResourceRequireDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridResourceRequireDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                //object value = gridResourceRequireDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].FormattedValue;
                object value = e.FormattedValue;
                if (value != null)
                {
                    try
                    {
                        string colName = gridResourceRequireDetail.Columns[e.ColumnIndex].Name;
                        if (colName == FirstOfferQuantity.Name || colName == ResponsibilityQuantity.Name || colName == PlanQuantity.Name
                            || colName == MonthlyPlanQuantity.Name || colName == ApproachPlanQuantity.Name || colName == ApproachExecQuantity.Name)//甲供需求量
                        {
                            if (value.ToString() != "")
                                ClientUtil.ToDecimal(value);
                        }
                        if (colName == PlanEndTime.Name)//开始时间
                        {
                            DateTime startTime = Convert.ToDateTime(gridResourceRequireDetail.Rows[e.RowIndex].Cells[PlanStartTime.Name].Value);
                            DateTime endTime = Convert.ToDateTime(value);
                            if (endTime < startTime)
                            {
                                MessageBox.Show("结束时间不能小于起始时间！");
                                e.Cancel = true;
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("输入格式不正确！");
                        e.Cancel = true;
                    }
                }
            }
        }

        void gridResourceRequireDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && gridResourceRequireDetail.Rows[e.RowIndex].ReadOnly == false)
            {
                object tempValue = gridResourceRequireDetail.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                string value = "";
                if (tempValue != null)
                    value = tempValue.ToString().Trim();


                ResourceRequirePlanDetail dtl = gridResourceRequireDetail.Rows[e.RowIndex].Tag as ResourceRequirePlanDetail;
                if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ResourceType.Name)//资源类型
                {
                    //dtl.BuildResourceTypeName = value.ToString();
                    if (value.ToString() == ResourceRequirePlanDetailResourceType.劳务.ToString())
                    {
                        if (string.IsNullOrEmpty(dtl.BuildResourceTypeName) || dtl.BuildResourceTypeName == "物资")//修改了该值
                        {
                            dtl.BuildResourceTypeName = value.ToString();

                            VSelectServiceType frm = new VSelectServiceType();
                            frm.ShowDialog();
                            if (frm.SelectServiceType.Count > 0)
                            {
                                dtl.ServiceType = frm.SelectServiceType[0];
                                dtl.MaterialResource = null;
                                dtl.MaterialCode = "";
                                dtl.MaterialName = "";
                                dtl.MaterialSpec = "";

                                UpdateResourceRequireDetailInGrid(e.RowIndex, dtl, false);

                                CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                                DateTime serverTime = model.GetServerTime();
                                for (int i = 1; i < frm.SelectServiceType.Count; i++)
                                {
                                    ResourceRequirePlanDetail tempDtl = new ResourceRequirePlanDetail();
                                    tempDtl.TheGWBSTaskGUID = dtl.TheGWBSTaskGUID;
                                    tempDtl.TheGWBSTaskName = dtl.TheGWBSTaskName;
                                    if (projectInfo != null)
                                    {
                                        tempDtl.TheProjectGUID = projectInfo.Id;
                                        tempDtl.TheProjectName = projectInfo.Name;
                                    }
                                    tempDtl.State = ResourceRequirePlanDetailState.编制;
                                    tempDtl.StateUpdateTime = serverTime;
                                    tempDtl.PlanBeginApproachDate = serverTime.Date;
                                    tempDtl.PlanEndApproachDate = serverTime.Date;

                                    tempDtl.BuildResourceTypeName = ResourceRequirePlanDetailResourceType.劳务.ToString();


                                    tempDtl.ServiceType = frm.SelectServiceType[i];

                                    InsertResourceRequireDetailInGrid(tempDtl, e.RowIndex);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(dtl.BuildResourceTypeName) || dtl.BuildResourceTypeName == "劳务")//修改了该值
                        {
                            dtl.BuildResourceTypeName = value.ToString();
                            dtl.ServiceType = "";
                            gridResourceRequireDetail.Rows[e.RowIndex].Cells["ServiceType"].Value = "";
                            gridResourceRequireDetail.Rows[e.RowIndex].Tag = dtl;
                        }
                    }
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ResourceCode.Name &&
                    string.IsNullOrEmpty(value))//物料编号 该处只判断删除物料的情况
                {
                    dtl.MaterialResource = null;
                    dtl.MaterialCode = "";
                    dtl.MaterialName = "";
                    dtl.MaterialSpec = "";

                    UpdateResourceRequireDetailInGrid(e.RowIndex, dtl, false);
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == FirstOfferQuantity.Name)//甲供需求量
                {
                    if (!string.IsNullOrEmpty(value))
                        dtl.FirstOfferRequireQuantity = ClientUtil.ToDecimal(value);
                    else
                        dtl.FirstOfferRequireQuantity = 0;
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ResponsibilityQuantity.Name)//责任需求量
                {
                    if (!string.IsNullOrEmpty(value))
                        dtl.ResponsibilityRequireQuantity = ClientUtil.ToDecimal(value);
                    else
                        dtl.ResponsibilityRequireQuantity = 0;
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == PlanQuantity.Name)//计划需求量
                {
                    if (!string.IsNullOrEmpty(value))
                        dtl.PlanRequireQuantity = ClientUtil.ToDecimal(value);
                    else
                        dtl.PlanRequireQuantity = 0;
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == MonthlyPlanQuantity.Name)//月计划累计量
                {
                    if (!string.IsNullOrEmpty(value))
                        dtl.MonthPlanPublishQuantity = ClientUtil.ToDecimal(value);
                    else
                        dtl.MonthPlanPublishQuantity = 0;
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ApproachPlanQuantity.Name)//进场计划累计量
                {
                    if (!string.IsNullOrEmpty(value))
                        dtl.ApproachPlanPublishQuantity = ClientUtil.ToDecimal(value);
                    else
                        dtl.ApproachPlanPublishQuantity = 0;
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == ApproachExecQuantity.Name)//进场计划执行量
                {
                    if (!string.IsNullOrEmpty(value))
                        dtl.ApproachPlanExecuteQuantity = ClientUtil.ToDecimal(value);
                    else
                        dtl.ApproachPlanExecuteQuantity = 0;
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == PlanStartTime.Name)//计划开始时间
                {
                    dtl.PlanBeginApproachDate = Convert.ToDateTime(value);
                }
                else if (gridResourceRequireDetail.Columns[e.ColumnIndex].Name == PlanEndTime.Name)//计划结束时间
                {
                    dtl.PlanEndApproachDate = Convert.ToDateTime(value);
                }
            }
        }

        private void AddResourceRequireDetailInGrid(ResourceRequirePlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridResourceRequireDetail.Rows.Add();
            DataGridViewRow row = gridResourceRequireDetail.Rows[index];
            row.Cells["TaskName"].Value = dtl.TheGWBSTaskName;
            row.Cells["ResourceType"].Value = dtl.BuildResourceTypeName;
            row.Cells["ServiceType"].Value = dtl.ServiceType;
            row.Cells["ResourceCode"].Value = dtl.MaterialCode;
            row.Cells["ResourceName"].Value = dtl.MaterialName;
            row.Cells["Spec"].Value = dtl.MaterialSpec;

            if (dtl.FirstOfferRequireQuantity != 0)
                row.Cells["FirstOfferQuantity"].Value = dtl.FirstOfferRequireQuantity.ToString();
            if (dtl.ResponsibilityRequireQuantity != 0)
                row.Cells["ResponsibilityQuantity"].Value = dtl.ResponsibilityRequireQuantity.ToString();
            if (dtl.PlanRequireQuantity != 0)
                row.Cells["PlanQuantity"].Value = dtl.PlanRequireQuantity.ToString();
            if (dtl.MonthPlanPublishQuantity != 0)
                row.Cells["MonthlyPlanQuantity"].Value = dtl.MonthPlanPublishQuantity.ToString();
            if (dtl.ApproachPlanPublishQuantity != 0)
                row.Cells["ApproachPlanQuantity"].Value = dtl.ApproachPlanPublishQuantity.ToString();
            if (dtl.ApproachPlanExecuteQuantity != 0)
                row.Cells["ApproachExecQuantity"].Value = dtl.ApproachPlanExecuteQuantity.ToString();

            if (dtl.PlanBeginApproachDate != null)
                row.Cells["PlanStartTime"].Value = dtl.PlanBeginApproachDate.Value;
            else
                row.Cells["PlanStartTime"].Value = model.GetServerTime().Date;

            if (dtl.PlanEndApproachDate != null)
                row.Cells["PlanEndTime"].Value = dtl.PlanEndApproachDate.Value;
            else
                row.Cells["PlanEndTime"].Value = model.GetServerTime().Date;

            row.Cells["State"].Value = dtl.State.ToString();
            row.Cells["CreatDate"].Value = dtl.CreateTime.ToString();

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridResourceRequireDetail.CurrentCell = row.Cells["ResourceType"];
        }
        private void InsertResourceRequireDetailInGrid(ResourceRequirePlanDetail dtl, int beforeIndex)
        {
            gridResourceRequireDetail.Rows.Insert(beforeIndex + 1, 1);
            DataGridViewRow row = gridResourceRequireDetail.Rows[beforeIndex + 1];
            row.Cells["TaskName"].Value = dtl.TheGWBSTaskName;
            row.Cells["ResourceType"].Value = dtl.BuildResourceTypeName;
            row.Cells["ServiceType"].Value = dtl.ServiceType;
            row.Cells["ResourceCode"].Value = dtl.MaterialCode;
            row.Cells["ResourceName"].Value = dtl.MaterialName;
            row.Cells["Spec"].Value = dtl.MaterialSpec;

            if (dtl.FirstOfferRequireQuantity != 0)
                row.Cells["FirstOfferQuantity"].Value = dtl.FirstOfferRequireQuantity.ToString();
            if (dtl.ResponsibilityRequireQuantity != 0)
                row.Cells["ResponsibilityQuantity"].Value = dtl.ResponsibilityRequireQuantity.ToString();
            if (dtl.PlanRequireQuantity != 0)
                row.Cells["PlanQuantity"].Value = dtl.PlanRequireQuantity.ToString();
            if (dtl.MonthPlanPublishQuantity != 0)
                row.Cells["MonthlyPlanQuantity"].Value = dtl.MonthPlanPublishQuantity.ToString();
            if (dtl.ApproachPlanPublishQuantity != 0)
                row.Cells["ApproachPlanQuantity"].Value = dtl.ApproachPlanPublishQuantity.ToString();
            if (dtl.ApproachPlanExecuteQuantity != 0)
                row.Cells["ApproachExecQuantity"].Value = dtl.ApproachPlanExecuteQuantity.ToString();

            if (dtl.PlanBeginApproachDate != null)
                row.Cells["PlanStartTime"].Value = dtl.PlanBeginApproachDate.Value;
            else
                row.Cells["PlanStartTime"].Value = model.GetServerTime().Date;

            if (dtl.PlanEndApproachDate != null)
                row.Cells["PlanEndTime"].Value = dtl.PlanEndApproachDate.Value;
            else
                row.Cells["PlanEndTime"].Value = model.GetServerTime().Date;

            row.Cells["State"].Value = dtl.State.ToString();
            row.Cells["CreatDate"].Value = dtl.CreateTime.ToString();

            row.Tag = dtl;
        }
        private void UpdateResourceRequireDetailInGrid(ResourceRequirePlanDetail dtl, bool isSetCurrentCell)
        {
            foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
            {
                ResourceRequirePlanDetail tempDtl = row.Tag as ResourceRequirePlanDetail;
                if (!string.IsNullOrEmpty(tempDtl.Id) && tempDtl.Id == dtl.Id)
                {
                    row.Cells["TaskName"].Value = dtl.TheGWBSTaskName;
                    row.Cells["ResourceType"].Value = dtl.BuildResourceTypeName;
                    row.Cells["ServiceType"].Value = dtl.ServiceType;
                    row.Cells["ResourceCode"].Value = dtl.MaterialCode;
                    row.Cells["ResourceName"].Value = dtl.MaterialName;
                    row.Cells["Spec"].Value = dtl.MaterialSpec;

                    if (dtl.FirstOfferRequireQuantity != 0)
                        row.Cells["FirstOfferQuantity"].Value = dtl.FirstOfferRequireQuantity.ToString();
                    if (dtl.ResponsibilityRequireQuantity != 0)
                        row.Cells["ResponsibilityQuantity"].Value = dtl.ResponsibilityRequireQuantity.ToString();
                    if (dtl.PlanRequireQuantity != 0)
                        row.Cells["PlanQuantity"].Value = dtl.PlanRequireQuantity.ToString();
                    if (dtl.MonthPlanPublishQuantity != 0)
                        row.Cells["MonthlyPlanQuantity"].Value = dtl.MonthPlanPublishQuantity.ToString();
                    if (dtl.ApproachPlanPublishQuantity != 0)
                        row.Cells["ApproachPlanQuantity"].Value = dtl.ApproachPlanPublishQuantity.ToString();
                    if (dtl.ApproachPlanExecuteQuantity != 0)
                        row.Cells["ApproachExecQuantity"].Value = dtl.ApproachPlanExecuteQuantity.ToString();

                    if (dtl.PlanBeginApproachDate != null)
                        row.Cells["PlanStartTime"].Value = dtl.PlanBeginApproachDate.Value;
                    else
                        row.Cells["PlanStartTime"].Value = model.GetServerTime().Date;

                    if (dtl.PlanEndApproachDate != null)
                        row.Cells["PlanEndTime"].Value = dtl.PlanEndApproachDate.Value;
                    else
                        row.Cells["PlanEndTime"].Value = model.GetServerTime().Date;

                    row.Cells["State"].Value = dtl.State.ToString();
                    row.Cells["CreatDate"].Value = dtl.CreateTime.ToString();

                    row.Tag = dtl;

                    if (isSetCurrentCell)
                        gridResourceRequireDetail.CurrentCell = row.Cells["ResourceType"];

                    break;
                }
            }
        }
        private void UpdateResourceRequireDetailInGrid(int updateRowIndex, ResourceRequirePlanDetail dtl, bool isSetCurrentCell)
        {
            DataGridViewRow row = gridResourceRequireDetail.Rows[updateRowIndex];
            row.Cells["TaskName"].Value = dtl.TheGWBSTaskName;
            row.Cells["ResourceType"].Value = dtl.BuildResourceTypeName;
            row.Cells["ServiceType"].Value = dtl.ServiceType;
            row.Cells["ResourceCode"].Value = dtl.MaterialCode;
            row.Cells["ResourceName"].Value = dtl.MaterialName;
            row.Cells["Spec"].Value = dtl.MaterialSpec;

            if (dtl.FirstOfferRequireQuantity != 0)
                row.Cells["FirstOfferQuantity"].Value = dtl.FirstOfferRequireQuantity.ToString();
            if (dtl.ResponsibilityRequireQuantity != 0)
                row.Cells["ResponsibilityQuantity"].Value = dtl.ResponsibilityRequireQuantity.ToString();
            if (dtl.PlanRequireQuantity != 0)
                row.Cells["PlanQuantity"].Value = dtl.PlanRequireQuantity.ToString();
            if (dtl.MonthPlanPublishQuantity != 0)
                row.Cells["MonthlyPlanQuantity"].Value = dtl.MonthPlanPublishQuantity.ToString();
            if (dtl.ApproachPlanPublishQuantity != 0)
                row.Cells["ApproachPlanQuantity"].Value = dtl.ApproachPlanPublishQuantity.ToString();
            if (dtl.ApproachPlanExecuteQuantity != 0)
                row.Cells["ApproachExecQuantity"].Value = dtl.ApproachPlanExecuteQuantity.ToString();

            if (dtl.PlanBeginApproachDate != null)
                row.Cells["PlanStartTime"].Value = dtl.PlanBeginApproachDate.Value;
            else
                row.Cells["PlanStartTime"].Value = model.GetServerTime().Date;

            if (dtl.PlanEndApproachDate != null)
                row.Cells["PlanEndTime"].Value = dtl.PlanEndApproachDate.Value;
            else
                row.Cells["PlanEndTime"].Value = model.GetServerTime().Date;

            row.Cells["State"].Value = dtl.State.ToString();
            row.Cells["CreatDate"].Value = dtl.CreateTime.ToString();

            row.Tag = dtl;

            if (isSetCurrentCell)
                gridResourceRequireDetail.CurrentCell = row.Cells["ResourceType"];

        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (gridResourceRequireDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要修改的行！");
                return;
            }
            if (gridResourceRequireDetail.SelectedRows.Count > 1)
            {
                MessageBox.Show("一次只能修改一行！");
                return;
            }
            ResourceRequirePlanDetail dtl = gridResourceRequireDetail.SelectedRows[0].Tag as ResourceRequirePlanDetail;
            if (dtl.State != ResourceRequirePlanDetailState.编制)
            {
                MessageBox.Show("请选择状态为‘编制’的计划明细进行修改！");
                return;
            }

            gridResourceRequireDetail.CurrentCell = gridResourceRequireDetail.SelectedRows[0].Cells[ResourceType.Name];
            gridResourceRequireDetail.SelectedRows[0].ReadOnly = false;
            gridResourceRequireDetail.SelectedRows[0].Selected = false;
            gridResourceRequireDetail.BeginEdit(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gridResourceRequireDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要删除的行！");
                return;
            }
            try
            {

                IList list = new List<ResourceRequirePlanDetail>();
                List<int> listRowIndex = new List<int>();
                foreach (DataGridViewRow row in gridResourceRequireDetail.SelectedRows)
                {
                    ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                    if (dtl.State == ResourceRequirePlanDetailState.编制)
                    {
                        if (!string.IsNullOrEmpty(dtl.Id))
                        {
                            list.Add(dtl);
                        }
                        listRowIndex.Add(row.Index);
                    }
                }

                if (listRowIndex.Count == 0)
                {
                    MessageBox.Show("选择中没有符合删除的行，请选择状态为‘编制’计划明细！");
                    return;
                }

                if (MessageBox.Show("删除后不能恢复，您确认要删除选择的计划明细吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;

                if (list.Count > 0)
                {
                    model.DeleteResourceRequirePlanDetail(list);

                    foreach (ResourceRequirePlanDetail planDtl in list)
                    {
                        LogData log = new LogData();
                        log.BillId = planDtl.Id;
                        log.BillType = "滚动需求计划明细";
                        log.Code = "";
                        log.OperType = "删除";
                        log.Descript = "";
                        log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                        log.ProjectName = planDtl.TheProjectName;
                        Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);
                    }
                }

                listRowIndex.Sort();
                for (int i = listRowIndex.Count - 1; i > -1; i--)
                {
                    gridResourceRequireDetail.Rows.RemoveAt(listRowIndex[i]);
                }

                gridResourceRequireDetail.ClearSelection();

            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateSaveDetail())
                MessageBox.Show("保存成功！");
        }

        private bool ValidateSaveDetail()
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Id", optPlan.Id));
                oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                IList listTemp = model.ObjectQuery(typeof(ResourceRequirePlan), oq);
                optPlan = listTemp[0] as ResourceRequirePlan;


                List<int> listRowIndex = new List<int>();
                List<int> listDetailIndex = new List<int>();
                foreach (DataGridViewRow row in gridResourceRequireDetail.Rows)
                {
                    if (!row.ReadOnly)
                    {
                        int detailIndex = 0;

                        ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                        if (!string.IsNullOrEmpty(dtl.Id))
                        {
                            for (int i = 0; i < optPlan.Details.Count; i++)
                            {
                                ResourceRequirePlanDetail temp = optPlan.Details.ElementAt(i);
                                if (temp.Id == dtl.Id)
                                {
                                    temp.BuildResourceTypeName = dtl.BuildResourceTypeName;
                                    temp.ServiceType = dtl.ServiceType;

                                    temp.MaterialResource = dtl.MaterialResource;
                                    temp.MaterialCode = dtl.MaterialCode;
                                    temp.MaterialName = dtl.MaterialName;
                                    temp.MaterialSpec = dtl.MaterialSpec;

                                    temp.FirstOfferRequireQuantity = dtl.FirstOfferRequireQuantity;
                                    temp.ResponsibilityRequireQuantity = dtl.ResponsibilityRequireQuantity;
                                    temp.PlanRequireQuantity = dtl.PlanRequireQuantity;

                                    temp.MonthPlanPublishQuantity = dtl.MonthPlanPublishQuantity;
                                    temp.ApproachPlanPublishQuantity = dtl.ApproachPlanPublishQuantity;
                                    temp.ApproachPlanExecuteQuantity = dtl.ApproachPlanExecuteQuantity;

                                    temp.PlanBeginApproachDate = dtl.PlanBeginApproachDate;
                                    temp.PlanEndApproachDate = dtl.PlanEndApproachDate;

                                    detailIndex = i;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            dtl.TheResourceRequirePlan = optPlan;
                            optPlan.Details.Add(dtl);

                            detailIndex = optPlan.Details.Count - 1;
                        }

                        listRowIndex.Add(row.Index);

                        listDetailIndex.Add(detailIndex);
                    }
                }

                if (listRowIndex.Count == 0)
                {
                    return true;
                }

                IList listLog = new ArrayList();

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    ResourceRequirePlanDetail oldPlanDtl = gridResourceRequireDetail.Rows[rowIndex].Tag as ResourceRequirePlanDetail;

                    LogData log = new LogData();
                    log.BillId = oldPlanDtl.Id;
                    log.BillType = "滚动需求计划明细";
                    log.Code = "";
                    if (string.IsNullOrEmpty(oldPlanDtl.Id))
                        log.OperType = "新增";
                    else
                        log.OperType = "修改";
                    log.Descript = "";
                    log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = oldPlanDtl.TheProjectName;
                    listLog.Add(log);
                    //Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);
                }

                optPlan = model.SaveOrUpdateResourceRequirePlan(optPlan);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];

                    ResourceRequirePlanDetail planDtl = optPlan.Details.ElementAt(listDetailIndex[i]);

                    LogData log = listLog[i] as LogData;
                    log.BillId = planDtl.Id;
                    Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.InsertLogData(log);

                    gridResourceRequireDetail.Rows[rowIndex].Tag = planDtl;
                    gridResourceRequireDetail.Rows[rowIndex].ReadOnly = true;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
                return false;
            }
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (gridResourceRequireDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要发布的行！");
                return;
            }
            IList list = new List<ResourceRequirePlanDetail>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridResourceRequireDetail.SelectedRows)
            {
                ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                if (!string.IsNullOrEmpty(dtl.Id) && dtl.State == ResourceRequirePlanDetailState.编制)
                {
                    ResourceRequirePlanDetail temp = model.GetObjectById(typeof(ResourceRequirePlanDetail), dtl.Id) as ResourceRequirePlanDetail;
                    temp.State = ResourceRequirePlanDetailState.发布;

                    list.Add(temp);

                    listRowIndex.Add(row.Index);
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("选择中没有需要发布的计划明细，请选择状态为‘编制’的计划明细！\n如果有未保存的明细，请先保存后再发布！");
                return;
            }

            try
            {
                list = model.SaveOrUpdateResourcePlanDetail(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];
                    gridResourceRequireDetail.Rows[rowIndex].Tag = list[i];
                    gridResourceRequireDetail.Rows[rowIndex].Cells["State"].Value = (list[i] as ResourceRequirePlanDetail).State.ToString();
                    gridResourceRequireDetail.Rows[rowIndex].ReadOnly = true;
                }

                MessageBox.Show("发布成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("发布失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void btnCancellation_Click(object sender, EventArgs e)
        {
            if (gridResourceRequireDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择要作废的行！");
                return;
            }
            IList list = new List<ResourceRequirePlanDetail>();
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridResourceRequireDetail.SelectedRows)
            {
                ResourceRequirePlanDetail dtl = row.Tag as ResourceRequirePlanDetail;
                if (!string.IsNullOrEmpty(dtl.Id) && dtl.State == ResourceRequirePlanDetailState.发布)
                {
                    ResourceRequirePlanDetail temp = model.GetObjectById(typeof(ResourceRequirePlanDetail), dtl.Id) as ResourceRequirePlanDetail;
                    temp.State = ResourceRequirePlanDetailState.作废;

                    list.Add(temp);

                    listRowIndex.Add(row.Index);
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("选择中没有符合作废的计划明细，请选择状态为‘发布’的计划明细！");
                return;
            }

            try
            {
                list = model.SaveOrUpdateResourcePlanDetail(list);

                for (int i = 0; i < listRowIndex.Count; i++)
                {
                    int rowIndex = listRowIndex[i];

                    gridResourceRequireDetail.Rows[rowIndex].Tag = list[i];
                    gridResourceRequireDetail.Rows[rowIndex].Cells["State"].Value = (list[i] as ResourceRequirePlanDetail).State.ToString();
                    gridResourceRequireDetail.Rows[rowIndex].ReadOnly = true;
                }

                MessageBox.Show("作废设置成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show("作废设置失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }
        #endregion

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
