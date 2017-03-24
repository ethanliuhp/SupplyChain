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
    public partial class VGenerateExecDemandPlanBak : TBasicDataView
    {
        public MRollingDemandPlan model;
        public MDailyPlanMng MDailyPlan = new MDailyPlanMng();
        public MLaborDemandPlanMng MLaborDemandPlan = new MLaborDemandPlanMng();
        public MMonthlyPlanMng MMonthlyPlan = new MMonthlyPlanMng();
        public MDemandMasterPlanMng MDemandMasterPlan = new MDemandMasterPlanMng();

        private CurrentProjectInfo projectInfo = null;

        public VGenerateExecDemandPlanBak(MRollingDemandPlan mot)
        {
            model = mot;

            InitializeComponent();

            InitForm();
        }

        private void InitForm()
        {
            projectInfo = StaticMethod.GetProjectInfo();

            InitEvents();

            foreach (string type in Enum.GetNames(typeof(ResourceRequirePlanType)))
            {
                cbPlanTypeQuery.Items.Add(type);
            }
            cbPlanTypeQuery.SelectedIndex = 0;

            foreach (string state in Enum.GetNames(typeof(ResourceRequirePlanDetailState)))
            {
                cbStateQuery.Items.Add(state);
            }

            DateTime now = model.GetServerTime();
            dtMadeStartDateQuery.Value = now.Date.AddMonths(-1);
            dtMadeEndDateQuery.Value = now.Date;


            //生成执行需求计划
            foreach (string s in Enum.GetNames(typeof(RemandPlanType)))
            {
                cbPlanType.Items.Add(s);
            }
            if (cbPlanType.Items.Count > 0)
                cbPlanType.SelectedIndex = 0;
        }

        private void InitEvents()
        {
            btnQuery.Click += new EventHandler(btnQuery_Click);

            btnAddDetailToSelectList.Click += new EventHandler(btnAddDetailToSelectList_Click);
            btnRemoveLineByQueryDetail.Click += new EventHandler(btnRemoveLineByQueryDetail_Click);

            btnGenerateRemandPlan.Click += new EventHandler(btnGenerateRemandPlan_Click);
            btnRemoveLineBySelectDetail.Click += new EventHandler(btnRemoveLineBySelectDetail_Click);
            btnClearSelectDetail.Click += new EventHandler(btnClearSelectDetail_Click);

            cbPlanTypeQuery.SelectedIndexChanged += new EventHandler(cbPlanTypeQuery_SelectedIndexChanged);
            btnSelectTaskNode.Click += new EventHandler(btnSelectTaskNode_Click);
        }

        void cbPlanTypeQuery_SelectedIndexChanged(object sender, EventArgs e)
        {
            string planType = cbPlanTypeQuery.SelectedItem.ToString();

            cbPlanNameQuery.Text = "";
            cbPlanNameQuery.Items.Clear();
            //资源需求计划名称
            VBasicDataOptr.InitBasicDataByCurrProjectInfo(planType, cbPlanNameQuery, true);

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
                    AddResourceRequireDetailInGrid(tempDtl, false, true);
                }

                gridPlanDetail.ClearSelection();
            }
        }

        private void UpdateRowRecordCount()
        {
            lblSelectCount.Text = "共" + gridSelectPlanDetail.Rows.Count + "条记录";
        }

        void btnAddDetailToSelectList_Click(object sender, EventArgs e)
        {
            if (gridPlanDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择添加的行！");
                return;
            }

            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridPlanDetail.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }

            listRowIndex.Sort();
            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                ResourceRequirePlanDetail dtl = gridPlanDetail.Rows[listRowIndex[i]].Tag as ResourceRequirePlanDetail;
                AddResourceRequireDetailInSelectGrid(dtl, true, true);

                gridPlanDetail.Rows.RemoveAt(listRowIndex[i]);
            }

            UpdateRowRecordCount();
        }
        void btnRemoveLineByQueryDetail_Click(object sender, EventArgs e)
        {
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridPlanDetail.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }
            listRowIndex.Sort();

            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridPlanDetail.Rows.RemoveAt(listRowIndex[i]);
            }
        }

        void btnGenerateRemandPlan_Click(object sender, EventArgs e)
        {
            if (gridSelectPlanDetail.Rows.Count == 0 || gridSelectPlanDetail.SelectedRows.Count == 0)
            {
                MessageBox.Show("请在需求计划列表中选择需求计划信息！");
                return;
            }
            //判断用户需要生成的计划类型

            if (cbPlanType.Text == RemandPlanType.日常需求计划.ToString())
            {
                //校验当前选中周计划明细的资源类型
                if (ValidMaterialType(RemandPlanType.日常需求计划))
                {
                    MessageBox.Show("日常需求计划的资源类型必须是物资类型,请移除非物资类型的需求计划！");
                    return;
                }
                if (MessageBox.Show("是否要生成日常需求计划？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //生成日常需求计划
                    DailyPlanMaster master = new DailyPlanMaster();
                    //主表信息
                    master.CreatePerson = ConstObject.LoginPersonInfo;
                    master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                    master.CreateDate = ConstObject.LoginDate;
                    master.CreateYear = ConstObject.LoginDate.Year;
                    master.CreateMonth = ConstObject.LoginDate.Month;
                    master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                    master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                    master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                    master.HandlePerson = ConstObject.LoginPersonInfo;
                    master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                    master.DocState = DocumentState.Edit;
                    //归属项目
                    CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        master.ProjectId = projectInfo.Id;
                        master.ProjectName = projectInfo.Name;
                    }
                    master.RealOperationDate = CommonMethod.GetServerDateTime();

                    //明细信息

                    foreach (DataGridViewRow var in gridSelectPlanDetail.Rows)
                    {
                        DailyPlanDetail theDailyPlanDetail = new DailyPlanDetail();

                        ResourceRequirePlanDetail theResourceRequirePlanDetail = var.Tag as ResourceRequirePlanDetail;
                        if (theResourceRequirePlanDetail != null)
                        {
                            theDailyPlanDetail.MaterialResource = theResourceRequirePlanDetail.MaterialResource;
                            theDailyPlanDetail.MaterialCode = theResourceRequirePlanDetail.MaterialCode;
                            theDailyPlanDetail.MaterialName = theResourceRequirePlanDetail.MaterialName;
                            theDailyPlanDetail.MaterialSpec = theResourceRequirePlanDetail.MaterialSpec;
                            theDailyPlanDetail.Quantity = theResourceRequirePlanDetail.PlanRequireQuantity;
                            theDailyPlanDetail.ProjectTask = theResourceRequirePlanDetail.TheGWBSTaskGUID;
                            theDailyPlanDetail.ProjectTaskName = theResourceRequirePlanDetail.TheGWBSTaskName;
                            theDailyPlanDetail.ForwardDetailId = theResourceRequirePlanDetail.Id;
                            theDailyPlanDetail.UsedPart = theResourceRequirePlanDetail.TheGWBSTaskGUID;
                            theDailyPlanDetail.UsedPartName = theResourceRequirePlanDetail.TheGWBSTaskName;
                        }
                        master.AddDetail(theDailyPlanDetail);
                    }
                    MDailyPlan.DailyPlanSrv.SaveDailyPlan(master);
                    MessageBox.Show("生成日常需求计划成功！");
                }
            }
            else if (this.cbPlanType.Text == RemandPlanType.月度需求计划.ToString())
            {
                //校验当前选中周计划明细的资源类型
                if (ValidMaterialType(RemandPlanType.日常需求计划))
                {
                    MessageBox.Show("月度需求计划的资源类型必须是物资类型,请移除非物资类型的需求计划！");
                    return;
                }
                if (MessageBox.Show("是否要生成月度需求计划？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    VGenerateRequirePlanSetYearMonth frm = new VGenerateRequirePlanSetYearMonth();
                    frm.ShowDialog();
                    if (frm.IsOk)
                    {
                        int fiscalYear = frm.AccountYear;
                        int fiscalMonth = frm.AccountMonth;
                        MonthlyPlanMaster master = new MonthlyPlanMaster();
                        //主表信息
                        master.CreatePerson = ConstObject.LoginPersonInfo;
                        master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                        master.CreateDate = ConstObject.LoginDate;
                        master.CreateYear = ConstObject.LoginDate.Year;
                        master.CreateMonth = ConstObject.LoginDate.Month;
                        master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                        master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                        master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                        master.HandlePerson = ConstObject.LoginPersonInfo;
                        master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                        master.DocState = DocumentState.Edit;
                        //年份和月份
                        master.Year = fiscalYear;
                        master.Month = fiscalMonth;
                        //归属项目
                        CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                        if (projectInfo != null)
                        {
                            master.ProjectId = projectInfo.Id;
                            master.ProjectName = projectInfo.Name;
                        }
                        master.RealOperationDate = CommonMethod.GetServerDateTime();
                        master.MonthePlanType = "月度计划";

                        foreach (DataGridViewRow var in gridSelectPlanDetail.Rows)
                        {
                            MonthlyPlanDetail theMonthlyPlanDetail = new MonthlyPlanDetail();

                            ResourceRequirePlanDetail theResourceRequirePlanDetail = var.Tag as ResourceRequirePlanDetail;
                            if (theResourceRequirePlanDetail != null)
                            {
                                theMonthlyPlanDetail.MaterialResource = theResourceRequirePlanDetail.MaterialResource;
                                theMonthlyPlanDetail.MaterialCode = theResourceRequirePlanDetail.MaterialCode;
                                theMonthlyPlanDetail.MaterialName = theResourceRequirePlanDetail.MaterialName;
                                theMonthlyPlanDetail.MaterialSpec = theResourceRequirePlanDetail.MaterialSpec;
                                theMonthlyPlanDetail.Quantity = theResourceRequirePlanDetail.PlanRequireQuantity;
                                theMonthlyPlanDetail.ForwardDetailId = theResourceRequirePlanDetail.Id;
                                theMonthlyPlanDetail.UsedPart = theResourceRequirePlanDetail.TheGWBSTaskGUID;
                                theMonthlyPlanDetail.UsedPartName = theResourceRequirePlanDetail.TheGWBSTaskName;
                            }
                            master.AddDetail(theMonthlyPlanDetail);
                        }
                        MMonthlyPlan.MonthlyPlanSrv.SaveMonthlyPlan(master);
                        MessageBox.Show("生成月度需求计划成功！");

                    }
                }
            }
            else if (cbPlanType.Text == RemandPlanType.节点需求计划.ToString())
            {
                //校验当前选中周计划明细的资源类型
                if (ValidMaterialType(RemandPlanType.节点需求计划))
                {
                    MessageBox.Show("节点需求计划的资源类型必须是物资类型,请移除非物资类型的需求计划！");
                    return;
                }
                if (MessageBox.Show("是否要生成节点需求计划？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MonthlyPlanMaster master = new MonthlyPlanMaster();
                    //主表信息
                    master.CreatePerson = ConstObject.LoginPersonInfo;
                    master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                    master.CreateDate = ConstObject.LoginDate;
                    master.CreateYear = ConstObject.LoginDate.Year;
                    master.CreateMonth = ConstObject.LoginDate.Month;
                    master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                    master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                    master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                    master.HandlePerson = ConstObject.LoginPersonInfo;
                    master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                    master.DocState = DocumentState.Edit;
                    //归属项目
                    CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        master.ProjectId = projectInfo.Id;
                        master.ProjectName = projectInfo.Name;
                    }
                    master.RealOperationDate = CommonMethod.GetServerDateTime();
                    master.MonthePlanType = "节点计划";

                    foreach (DataGridViewRow var in gridSelectPlanDetail.Rows)
                    {
                        MonthlyPlanDetail theMonthlyPlanDetail = new MonthlyPlanDetail();

                        ResourceRequirePlanDetail theResourceRequirePlanDetail = var.Tag as ResourceRequirePlanDetail;
                        if (theResourceRequirePlanDetail != null)
                        {
                            theMonthlyPlanDetail.MaterialResource = theResourceRequirePlanDetail.MaterialResource;
                            theMonthlyPlanDetail.MaterialCode = theResourceRequirePlanDetail.MaterialCode;
                            theMonthlyPlanDetail.MaterialName = theResourceRequirePlanDetail.MaterialName;
                            theMonthlyPlanDetail.MaterialSpec = theResourceRequirePlanDetail.MaterialSpec;
                            theMonthlyPlanDetail.Quantity = theResourceRequirePlanDetail.PlanRequireQuantity;
                            theMonthlyPlanDetail.ForwardDetailId = theResourceRequirePlanDetail.Id;
                        }
                        master.AddDetail(theMonthlyPlanDetail);
                    }
                    MMonthlyPlan.MonthlyPlanSrv.SaveMonthlyPlan(master);
                    MessageBox.Show("生成节点需求计划成功！");
                }
            }
            else if (cbPlanType.Text == RemandPlanType.劳务需求计划.ToString())
            {
                //校验当前选中周计划明细的资源类型
                if (ValidMaterialType(RemandPlanType.劳务需求计划))
                {
                    MessageBox.Show("劳务需求计划的资源类型必须是物资类型,请移除非劳务类型的需求计划！");
                    return;
                }
                if (MessageBox.Show("是否要生成劳务需求计划？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //生成劳务需求计划
                    LaborDemandPlanMaster master = new LaborDemandPlanMaster();
                    //主表信息
                    master.CreatePerson = ConstObject.LoginPersonInfo;
                    master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                    master.CreateDate = ConstObject.LoginDate;
                    master.ReportTime = ConstObject.LoginDate;
                    master.CreateYear = ConstObject.LoginDate.Year;
                    master.CreateMonth = ConstObject.LoginDate.Month;
                    master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                    master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                    master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                    master.HandlePerson = ConstObject.LoginPersonInfo;
                    master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                    master.DocState = DocumentState.Edit;
                    //归属项目
                    CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        master.ProjectId = projectInfo.Id;
                        master.ProjectName = projectInfo.Name;
                    }
                    master.RealOperationDate = CommonMethod.GetServerDateTime();

                    //明细
                    foreach (DataGridViewRow var in gridSelectPlanDetail.Rows)
                    {
                        LaborDemandPlanDetail theLaborDemandPlanDetail = new LaborDemandPlanDetail();

                        ResourceRequirePlanDetail theResourceRequirePlanDetail = var.Tag as ResourceRequirePlanDetail;
                        if (theResourceRequirePlanDetail != null)
                        {
                            theLaborDemandPlanDetail.MaterialResource = theResourceRequirePlanDetail.MaterialResource;
                            theLaborDemandPlanDetail.MaterialCode = theResourceRequirePlanDetail.MaterialCode;
                            theLaborDemandPlanDetail.MaterialName = theResourceRequirePlanDetail.MaterialName;
                            theLaborDemandPlanDetail.MaterialSpec = theResourceRequirePlanDetail.MaterialSpec;
                            theLaborDemandPlanDetail.LaborRankInTime = ClientUtil.ToDateTime(theResourceRequirePlanDetail.PlanBeginApproachDate);
                            theLaborDemandPlanDetail.UsedRankType = ClientUtil.ToString(theResourceRequirePlanDetail.ServiceType);
                            theLaborDemandPlanDetail.Quantity = theResourceRequirePlanDetail.PlanRequireQuantity;
                            theLaborDemandPlanDetail.ProjectTask = theResourceRequirePlanDetail.TheGWBSTaskGUID;
                            theLaborDemandPlanDetail.ProjectTaskName = theResourceRequirePlanDetail.TheGWBSTaskName;
                            theLaborDemandPlanDetail.ForwardDetailId = theResourceRequirePlanDetail.Id;
                        }
                        master.AddDetail(theLaborDemandPlanDetail);
                    }
                    MLaborDemandPlan.LaborDemandPlanSrv.SaveLaborDemandPlan(master);
                    MessageBox.Show("生成劳务需求计划成功！");
                }

            }
            else if (cbPlanType.Text == RemandPlanType.需求总计划.ToString())
            {
                //校验当前选中周计划明细的资源类型
                if (ValidMaterialType(RemandPlanType.需求总计划))
                {
                    MessageBox.Show("需求总计划的资源类型必须是物资类型,请移除非劳务类型的需求计划！");
                    return;
                }
                if (MessageBox.Show("是否要生成需求总计划？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DemandMasterPlanMaster master = new DemandMasterPlanMaster();
                    //主表信息
                    master.CreatePerson = ConstObject.LoginPersonInfo;
                    master.CreatePersonName = ConstObject.LoginPersonInfo.Name;
                    master.CreateDate = ConstObject.LoginDate;
                    master.CreateYear = ConstObject.LoginDate.Year;
                    master.CreateMonth = ConstObject.LoginDate.Month;
                    master.OperOrgInfoName = ConstObject.TheLogin.TheOperationOrgInfo.Name;//登录人姓名
                    master.OperOrgInfo = ConstObject.TheLogin.TheOperationOrgInfo;//
                    master.OpgSysCode = ConstObject.TheLogin.TheOperationOrgInfo.SysCode;
                    master.HandlePerson = ConstObject.LoginPersonInfo;
                    master.HandlePersonName = ConstObject.LoginPersonInfo.Name;
                    master.DocState = DocumentState.Edit;
                    //归属项目
                    CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                    if (projectInfo != null)
                    {
                        master.ProjectId = projectInfo.Id;
                        master.ProjectName = projectInfo.Name;
                    }
                    master.RealOperationDate = CommonMethod.GetServerDateTime();

                    //明细信息
                    foreach (DataGridViewRow var in gridSelectPlanDetail.Rows)
                    {
                        DemandMasterPlanDetail theDemandMasterPlanDetail = new DemandMasterPlanDetail();

                        ResourceRequirePlanDetail theResourceRequirePlanDetail = var.Tag as ResourceRequirePlanDetail;
                        if (theResourceRequirePlanDetail != null)
                        {
                            theDemandMasterPlanDetail.MaterialResource = theResourceRequirePlanDetail.MaterialResource;
                            theDemandMasterPlanDetail.MaterialCode = theResourceRequirePlanDetail.MaterialCode;
                            theDemandMasterPlanDetail.MaterialName = theResourceRequirePlanDetail.MaterialName;
                            theDemandMasterPlanDetail.MaterialSpec = theResourceRequirePlanDetail.MaterialSpec;
                            theDemandMasterPlanDetail.Quantity = theResourceRequirePlanDetail.PlanRequireQuantity;
                            theDemandMasterPlanDetail.ForwardDetailId = theResourceRequirePlanDetail.Id;
                        }
                        master.AddDetail(theDemandMasterPlanDetail);
                    }
                    MDemandMasterPlan.SaveDemandMasterPlanMaster(master);
                    MessageBox.Show("生成需求总计划成功！");
                }
            }
            else
            {
                return;
            }

            //移除选中行 
            btnRemoveLineBySelectDetail_Click(sender, new EventArgs());
        }

        /// <summary>
        /// 校验当前选中周计划明细的资源类型
        /// </summary>
        /// <param name="PlanType"></param>
        /// <returns></returns>
        private bool ValidMaterialType(RemandPlanType PlanType)
        {
            int count = 0;
            if (PlanType == RemandPlanType.需求总计划 || PlanType == RemandPlanType.日常需求计划 || PlanType == RemandPlanType.月度需求计划 || PlanType == RemandPlanType.节点需求计划)
            {

                foreach (DataGridViewRow var in gridSelectPlanDetail.SelectedRows)
                {
                    ResourceRequirePlanDetail theResourceRequirePlanDetail = var.Tag as ResourceRequirePlanDetail;
                    if (theResourceRequirePlanDetail.BuildResourceTypeName == ResourceRequirePlanDetailResourceType.劳务.ToString())
                    {
                        count++;
                    }
                }
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (PlanType == RemandPlanType.劳务需求计划)
            {
                foreach (DataGridViewRow var in gridSelectPlanDetail.SelectedRows)
                {
                    ResourceRequirePlanDetail theResourceRequirePlanDetail = var.Tag as ResourceRequirePlanDetail;
                    if (theResourceRequirePlanDetail.BuildResourceTypeName == ResourceRequirePlanDetailResourceType.物资.ToString())
                    {
                        count++;
                    }
                }
                if (count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        void btnRemoveLineBySelectDetail_Click(object sender, EventArgs e)
        {
            List<int> listRowIndex = new List<int>();
            foreach (DataGridViewRow row in gridSelectPlanDetail.SelectedRows)
            {
                listRowIndex.Add(row.Index);
            }
            listRowIndex.Sort();

            for (int i = listRowIndex.Count - 1; i > -1; i--)
            {
                gridSelectPlanDetail.Rows.RemoveAt(listRowIndex[i]);
            }

            UpdateRowRecordCount();
        }
        void btnClearSelectDetail_Click(object sender, EventArgs e)
        {
            gridSelectPlanDetail.Rows.Clear();

            UpdateRowRecordCount();
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
            int index = gridSelectPlanDetail.Rows.Add();
            DataGridViewRow row = gridSelectPlanDetail.Rows[index];

            row.Cells[SelectTaskName.Name].Value = dtl.TheGWBSTaskName;
            row.Cells[SelectTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.TheGWBSTaskName, dtl.TheGWBSSysCode);

            row.Cells[SelectState.Name].Value = dtl.State;

            row.Cells[SelectResourceType.Name].Value = dtl.BuildResourceTypeName;
            row.Cells[SelectServiceType.Name].Value = dtl.ServiceType;

            row.Cells[SelectResourceName.Name].Value = dtl.MaterialName;
            row.Cells[SelectResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[SelectResourceQuality.Name].Value = dtl.MaterialStuff;
            row.Cells[SelectSpec.Name].Value = dtl.MaterialSpec;

            if (dtl.FirstOfferRequireQuantity != 0)
                row.Cells[SelectFirstOfferQuantity.Name].Value = dtl.FirstOfferRequireQuantity.ToString();
            if (dtl.ResponsibilityRequireQuantity != 0)
                row.Cells[SelectResponsibilityQuantity.Name].Value = dtl.ResponsibilityRequireQuantity.ToString();
            if (dtl.PlanRequireQuantity != 0)
                row.Cells[SelectPlanQuantity.Name].Value = dtl.PlanRequireQuantity.ToString();
            if (dtl.MonthPlanPublishQuantity != 0)
                row.Cells[SelectMonthlyPlanQuantity.Name].Value = dtl.MonthPlanPublishQuantity.ToString();
            if (dtl.ApproachPlanPublishQuantity != 0)
                row.Cells[SelectApproachPlanQuantity.Name].Value = dtl.ApproachPlanPublishQuantity.ToString();
            if (dtl.ApproachPlanExecuteQuantity != 0)
                row.Cells[SelectApproachExecQuantity.Name].Value = dtl.ApproachPlanExecuteQuantity.ToString();

            if (dtl.PlanBeginApproachDate != null)
                row.Cells[SelectPlanStartTime.Name].Value = dtl.PlanBeginApproachDate.Value;
            else
                row.Cells[SelectPlanStartTime.Name].Value = model.GetServerTime().Date;

            if (dtl.PlanEndApproachDate != null)
                row.Cells[SelectPlanEndTime.Name].Value = dtl.PlanEndApproachDate.Value;
            else
                row.Cells[SelectPlanEndTime.Name].Value = model.GetServerTime().Date;

            row.Cells[SelectCreateTime.Name].Value = dtl.CreateTime.ToString();

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridSelectPlanDetail.CurrentCell = row.Cells[0];
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

    ///// <summary>
    ///// 需求计划类型
    ///// </summary>
    //public enum RemandPlanType
    //{
    //    需求总计划 = 1,
    //    节点需求计划 = 2,
    //    月度需求计划 = 3,
    //    日常需求计划 = 4,
    //    劳务需求计划 = 5
    //}
}
