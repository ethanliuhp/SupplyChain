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
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.MaterialReource;
using Application.Business.Erp.SupplyChain.Client.Main;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public partial class VGenerateExecDemandPlanSelect : TBasicDataView
    {
        public MRollingDemandPlan model;
        public MDailyPlanMng MDailyPlan = new MDailyPlanMng();
        public MLaborDemandPlanMng MLaborDemandPlan = new MLaborDemandPlanMng();
        public MMonthlyPlanMng MMonthlyPlan = new MMonthlyPlanMng();
        public MDemandMasterPlanMng MDemandMasterPlan = new MDemandMasterPlanMng();
        private Hashtable ht_matcat = new Hashtable();

        string frontBillType = ResourceRequirePlanType.滚动需求计划.ToString();

        private CurrentProjectInfo projectInfo = null;

        RemandPlanType optPlanType = RemandPlanType.节点需求计划;

        /// <summary>
        /// 当前操作日常需求计划
        /// </summary>
        DailyPlanMaster theDailyPlanMaster = null;
        /// <summary>
        /// 月度需求计划
        /// </summary>
        MonthlyPlanMaster theMonthPlanMaster = null;
        int AccountYear = DateTime.Now.Year;
        int AccountMonth = DateTime.Now.Month;

        /// <summary>
        /// 节点需求计划
        /// </summary>
        MonthlyPlanMaster theNodePlanMaster = null;
        /// <summary>
        /// 劳务需求计划
        /// </summary>
        LaborDemandPlanMaster theLaborPlanMaster = null;
        /// <summary>
        /// 总需求计划
        /// </summary>
        DemandMasterPlanMaster theMasterPlanMaster = null;

        public VGenerateExecDemandPlanSelect(MRollingDemandPlan mot)
        {
            model = mot;
            InitializeComponent();
            InitForm();
        }

        private void InitForm()
        {
            projectInfo = StaticMethod.GetProjectInfo();
            InitEvents();
            //生成执行需求计划
            foreach (string s in Enum.GetNames(typeof(RemandPlanType)))
            {
                cbPlanType.Items.Add(s);
            }
            if (cbPlanType.Items.Count > 0)
                cbPlanType.SelectedIndex = 0;
            //状态
            foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
            {
                cbState.Items.Add(ClientUtil.GetDocStateName(state));
            }

            if (tabControlPlan.TabPages.Contains(tabPage日常需求计划) == true)
                tabControlPlan.TabPages.Remove(tabPage日常需求计划);
            if (tabControlPlan.TabPages.Contains(tabPage节点需求计划) == true)
                tabControlPlan.TabPages.Remove(tabPage节点需求计划);
            if (tabControlPlan.TabPages.Contains(tabPage劳务需求计划) == true)
                tabControlPlan.TabPages.Remove(tabPage劳务需求计划);
            if (tabControlPlan.TabPages.Contains(tabPage需求总计划) == false)
                tabControlPlan.TabPages.Add(tabPage需求总计划);
            if (tabControlPlan.TabPages.Contains(tabPage月度需求计划) == true)
                tabControlPlan.TabPages.Remove(tabPage月度需求计划);

            dtMadeBillStartDate.Value = DateTime.Now.AddMonths(-1);
            dtMadeBillEndDate.Value = DateTime.Now;

            ht_matcat = model.Mm.GetFirstMatInfo();
        }

        private void InitEvents()
        {
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            dgSearchResult.CellClick += new DataGridViewCellEventHandler(dgSearchResult_CellClick);
            gridServicePlan.CellDoubleClick += new DataGridViewCellEventHandler(gridServicePlan_CellDoubleClick);
        }

        /// <summary>
        /// 工种明细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridServicePlan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == gridServicePlan.Rows[e.RowIndex].Cells[colServiceWorkTypesDetail.Name].ColumnIndex)
            {
                LaborDemandPlanDetail bill = gridServicePlan.Rows[e.RowIndex].Tag as LaborDemandPlanDetail;
                //LaborDemandWorkerType worker = new LaborDemandWorkerType();
                //worker.Master.Id
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Master.Id", bill.Id));
                IList list = model.ObjectQuery(typeof(LaborDemandWorkerType), oq);
                if (list != null && list.Count > 0)
                {
                    VWorkerDetails vwd = new VWorkerDetails(list);
                    vwd.ShowDialog();
                }
                else
                {
                    MessageBox.Show("没有工种明细！");
                }
            }
        }

        /// <summary>
        /// 动态关联需求计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dgSearchResult_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadPlanDetail(e.RowIndex);
        }

        /// <summary>
        /// 加载计划明细
        /// </summary>
        /// <param name="rowIndex"></param>
        void LoadPlanDetail(int rowIndex)
        {
            gridDayPlan.Rows.Clear();
            gridMonthPlan.Rows.Clear();
            gridNodePlan.Rows.Clear();
            gridServicePlan.Rows.Clear();
            gridMasterPlan.Rows.Clear();

            if (rowIndex == -1)
            {
                PlanTypeShow();
                return;
            }

            if (optPlanType == RemandPlanType.日常需求计划)
            {
                DailyPlanMaster bill = dgSearchResult.Rows[rowIndex].Tag as DailyPlanMaster;
                Start(optPlanType, bill.Code, bill.Id);
            }
            else if (optPlanType == RemandPlanType.月度需求计划)
            {
                MonthlyPlanMaster bill = dgSearchResult.Rows[rowIndex].Tag as MonthlyPlanMaster;
                Start(optPlanType, bill.Code, bill.Id);
            }
            else if (optPlanType == RemandPlanType.节点需求计划)
            {
                MonthlyPlanMaster bill = dgSearchResult.Rows[rowIndex].Tag as MonthlyPlanMaster;
                Start(optPlanType, bill.Code, bill.Id);
            }
            else if (optPlanType == RemandPlanType.劳务需求计划)
            {
                LaborDemandPlanMaster bill = dgSearchResult.Rows[rowIndex].Tag as LaborDemandPlanMaster;
                Start(optPlanType, bill.Code, bill.Id);
            }
            else if (optPlanType == RemandPlanType.需求总计划)
            {
                DemandMasterPlanMaster bill = dgSearchResult.Rows[rowIndex].Tag as DemandMasterPlanMaster;
                Start(optPlanType, bill.Code, bill.Id);
            }
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                optPlanType = VirtualMachine.Component.Util.EnumUtil<RemandPlanType>.FromDescription(cbPlanType.SelectedItem.ToString());
                string planName = txtPlanName.Text.Trim();
                DocumentState state = 0;
                foreach (DocumentState s in Enum.GetValues(typeof(DocumentState)))
                {
                    if (cbState.Text == ClientUtil.GetDocStateName(s))
                        state = s;
                }


                DateTime madeStartDate = dtMadeBillStartDate.Value.Date;
                DateTime madeEndDate = dtMadeBillEndDate.Value.Date.AddDays(1).AddSeconds(-1);

                if (madeStartDate > madeEndDate)
                {
                    MessageBox.Show("制单起始日期不能大于结束日期！");
                    dtMadeBillEndDate.Focus();
                    return;
                }

                ObjectQuery oq = new ObjectQuery();
                //默认制单人和项目作为查询条件
                PersonInfo loginUser = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo;
                CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
                if (loginUser == null || string.IsNullOrEmpty(loginUser.Id))
                {
                    MessageBox.Show("未获取到登录用户信息，查询终止!");
                    return;
                }
                else if (projectInfo == null || string.IsNullOrEmpty(projectInfo.Id))
                {
                    MessageBox.Show("未获取到当前项目信息，查询终止!");
                    return;
                }

                oq.AddCriterion(Expression.Eq("HandlePerson.Id", loginUser.Id));
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

                oq.AddCriterion(Expression.Ge("CreateDate", madeStartDate));
                oq.AddCriterion(Expression.Le("CreateDate", madeEndDate));

                oq.AddCriterion(Expression.Eq("PlanType", ExecuteDemandPlanTypeEnum.滚动计划));

                if (!string.IsNullOrEmpty(planName))
                    oq.AddCriterion(Expression.Like("PlanName", planName, MatchMode.Anywhere));
                if(state!=0)
                    oq.AddCriterion(Expression.Eq("DocState",state));
                oq.AddOrder(NHibernate.Criterion.Order.Desc("CreateDate"));

                IList list = null;

                if (optPlanType == RemandPlanType.日常需求计划)
                {
                    list = model.ObjectQuery(typeof(DailyPlanMaster), oq);
                }
                else if (optPlanType == RemandPlanType.月度需求计划)
                {
                    oq.AddCriterion(Expression.Eq("MonthePlanType", "月度计划"));
                    list = model.ObjectQuery(typeof(MonthlyPlanMaster), oq);
                }
                else if (optPlanType == RemandPlanType.节点需求计划)
                {
                    oq.AddCriterion(Expression.Eq("MonthePlanType", "节点计划"));
                    list = model.ObjectQuery(typeof(MonthlyPlanMaster), oq);
                }
                else if (optPlanType == RemandPlanType.劳务需求计划)
                {
                    list = model.ObjectQuery(typeof(LaborDemandPlanMaster), oq);
                }
                else if (optPlanType == RemandPlanType.需求总计划)
                {
                    list = model.ObjectQuery(typeof(DemandMasterPlanMaster), oq);
                }

                RefreshData(optPlanType, list);

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询出错，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        public void RefreshData(RemandPlanType planType, IList lst)
        {
            optPlanType = planType;

            //从Model中取数
            dgSearchResult.Rows.Clear();

            if (optPlanType == RemandPlanType.日常需求计划)
            {
                foreach (DailyPlanMaster obj in lst)
                {
                    int r = dgSearchResult.Rows.Add();
                    DataGridViewRow dr = dgSearchResult.Rows[r];
                    //dr.Cells["No."].Value = r + 1;
                    dr.Cells["Id"].Value = obj.Id;
                    dr.Cells["Code"].Value = obj.Code;
                    dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);

                    dr.Cells["PlanType"].Value = obj.PlanType;
                    dr.Cells["PlanName"].Value = obj.PlanName;
                    dr.Cells["CreateTime"].Value = obj.CreateDate;

                    dr.Cells["Descript"].Value = obj.Descript;

                    dr.Tag = obj;
                }
            }
            else if (optPlanType == RemandPlanType.月度需求计划)
            {
                foreach (MonthlyPlanMaster obj in lst)
                {
                    int r = dgSearchResult.Rows.Add();
                    DataGridViewRow dr = dgSearchResult.Rows[r];
                    // dr.Cells["No."].Value = r + 1;
                    dr.Cells["Id"].Value = obj.Id;
                    dr.Cells["Code"].Value = obj.Code;
                    dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);

                    dr.Cells["PlanType"].Value = obj.PlanType;
                    dr.Cells["PlanName"].Value = obj.PlanName;
                    dr.Cells["CreateTime"].Value = obj.CreateDate;

                    dr.Cells["Descript"].Value = obj.Descript;

                    dr.Tag = obj;
                }
            }
            else if (optPlanType == RemandPlanType.节点需求计划)
            {
                foreach (MonthlyPlanMaster obj in lst)
                {
                    int r = dgSearchResult.Rows.Add();
                    DataGridViewRow dr = dgSearchResult.Rows[r];
                    //dr.Cells["No."].Value = r + 1;
                    dr.Cells["Id"].Value = obj.Id;
                    dr.Cells["Code"].Value = obj.Code;
                    dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);

                    dr.Cells["PlanType"].Value = obj.PlanType;
                    dr.Cells["PlanName"].Value = obj.PlanName;
                    dr.Cells["CreateTime"].Value = obj.CreateDate;

                    dr.Cells["Descript"].Value = obj.Descript;

                    dr.Tag = obj;
                }
            }
            else if (optPlanType == RemandPlanType.劳务需求计划)
            {
                foreach (LaborDemandPlanMaster obj in lst)
                {
                    int r = dgSearchResult.Rows.Add();
                    DataGridViewRow dr = dgSearchResult.Rows[r];
                    //dr.Cells["No."].Value = r + 1;
                    dr.Cells["Id"].Value = obj.Id;
                    dr.Cells["Code"].Value = obj.Code;
                    dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);

                    dr.Cells["PlanType"].Value = obj.PlanType;
                    dr.Cells["PlanName"].Value = obj.PlanName;
                    dr.Cells["CreateTime"].Value = obj.CreateDate;

                    dr.Cells["Descript"].Value = obj.Descript;

                    dr.Tag = obj;
                }
            }
            else if (optPlanType == RemandPlanType.需求总计划)
            {
                foreach (DemandMasterPlanMaster obj in lst)
                {
                    int r = dgSearchResult.Rows.Add();
                    DataGridViewRow dr = dgSearchResult.Rows[r];
                    //dr.Cells["No."].Value = r + 1;
                    dr.Cells["Id"].Value = obj.Id;
                    dr.Cells["Code"].Value = obj.Code;
                    dr.Cells["State"].Value = ClientUtil.GetDocStateName(obj.DocState);

                    dr.Cells["PlanType"].Value = obj.PlanType;
                    dr.Cells["PlanName"].Value = obj.PlanName;
                    dr.Cells["CreateTime"].Value = obj.CreateDate;

                    dr.Cells["Descript"].Value = obj.Descript;

                    dr.Tag = obj;
                }
            }
            this.dgSearchResult.AutoResizeColumns();
            if (dgSearchResult.Rows.Count > 0)
                LoadPlanDetail(0);
            else
                LoadPlanDetail(-1);
        }



        private void AddMasterPlanDetailInGrid(DemandMasterPlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridMasterPlan.Rows.Add();
            DataGridViewRow row = gridMasterPlan.Rows[index];

            row.Cells[colSumResourcesType.Name].Value = dtl.MaterialCategoryName;
            row.Cells[colSumResourceName.Name].Value = dtl.MaterialName;
            row.Cells[colSumResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[colSumSpec.Name].Value = dtl.MaterialSpec;

            row.Cells[colSumUsedPart.Name].Value = dtl.UsedPartName;
            row.Cells[colSumUsedPart.Name].Tag = dtl.UsedPart;

            //row.Cells[colSumUsedPart.Name].Value = dtl.UsedPartName;
            //row.Cells[colSumUsedPart.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.ProjectTaskName, dtl.ProjectTaskSysCode);

            row.Cells[colSumQuantity.Name].Value = dtl.Quantity;

            row.Cells[colSumQuantityUnit.Name].Value = dtl.MatStandardUnitName;
            row.Cells[colSumQuantityUnit.Name].Tag = dtl.MatStandardUnit;

            row.Cells[colSumRemark.Name].Value = dtl.Descript;

            row.Cells[colQuerlity.Name].Value = dtl.QualityStandard;

            row.Cells[colSumDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridMasterPlan.CurrentCell = row.Cells[0];
        }

        private void AddDayPlanDetailInGrid(DailyPlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridDayPlan.Rows.Add();
            DataGridViewRow row = gridDayPlan.Rows[index];

            row.Cells[colDayProjectName.Name].Value = dtl.UsedPartName;
            row.Cells[colDayProjectName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.ProjectTaskName, dtl.ProjectTaskSysCode);

            row.Cells[colDayResourceName.Name].Value = dtl.MaterialName;
            row.Cells[colDayResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[colDaySpec.Name].Value = dtl.MaterialSpec;

            row.Cells[colDayUsedTeam.Name].Value = dtl.UsedRankName;
            row.Cells[colDayUsedTeam.Name].Tag = dtl.UsedRank;

            row.Cells[colDayQuantity.Name].Value = dtl.Quantity;
            row.Cells[colDayQuantityUnit.Name].Value = dtl.MatStandardUnitName;

            row.Cells[colEnterDate.Name].Value = dtl.ApproachDate;

            row.Cells[colDayRemark.Name].Value = dtl.Descript;

            row.Cells[colDayQuality.Name].Value = dtl.QualityStandard;

            row.Cells[colDayPurchaseQuantify.Name].Value = dtl.SupplyQuantity;
            row.Cells[colDayUsePart.Name].Value = dtl.UsedPart;
            row.Cells[colDayNeedQuantify.Name].Value = dtl.Quantity;
            row.Cells[colDayAccumulatedIntoPlantAmount.Name].Value = dtl.SendBackQuantity;

            row.Cells[colDayDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridDayPlan.CurrentCell = row.Cells[0];

        }

        private void AddMonthPlanDetailInGrid(MonthlyPlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridMonthPlan.Rows.Add();
            DataGridViewRow row = gridMonthPlan.Rows[index];

            row.Cells[colMonthProjectTaskName.Name].Value = dtl.ProjectTaskName;
            row.Cells[colMonthProjectTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.ProjectTaskName, dtl.ProjectTaskSysCode);

            row.Cells[colMonthResourceName.Name].Value = dtl.MaterialName;
            row.Cells[colMonthResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[colMonthMaterialSpec.Name].Value = dtl.MaterialSpec;

            row.Cells[colMonthUsedTeam.Name].Value = dtl.UsedRankName;
            row.Cells[colMonthUsedTeam.Name].Tag = dtl.UsedRank;

            row.Cells[colMonthQuantity.Name].Value = dtl.Quantity;
            row.Cells[colMonthQuantityUnit.Name].Value = dtl.MatStandardUnitName;

            row.Cells[colMonthRemark.Name].Value = dtl.Descript;
            //质量标准
            row.Cells[colMonthQuality.Name].Value = dtl.QualityStandard;

            row.Cells[colMonthPlanQuantity.Name].Value = dtl.RefQuantity; //dtl.UsedPartName //dtl.RealInQuantity
            row.Cells[colMonthUsePosition.Name].Value = dtl.UsedPartName;
            row.Cells[colMonthDemandQuantity.Name].Value = dtl.NeedQuantity;
            row.Cells[colMonthActualIntoPlantAmount.Name].Value = dtl.RealInQuantity;

            row.Cells[colMonthDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridMonthPlan.CurrentCell = row.Cells[0];

        }

        private void AddNodePlanDetailInGrid(MonthlyPlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridNodePlan.Rows.Add();
            DataGridViewRow row = gridNodePlan.Rows[index];

            row.Cells[colNodeTaskName.Name].Value = dtl.UsedPartName;
            row.Cells[colNodeTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.ProjectTaskName, dtl.ProjectTaskSysCode);

            row.Cells[colNodeResourceName.Name].Value = dtl.MaterialName;
            row.Cells[colNodeResourceCode.Name].Value = dtl.MaterialCode;
            row.Cells[colNodeMaterialSpec.Name].Value = dtl.MaterialSpec;

            row.Cells[colNodeUsedTeam.Name].Value = dtl.UsedRankName;
            row.Cells[colNodeUsedTeam.Name].Tag = dtl.UsedRank;

            row.Cells[colNodeQuantity.Name].Value = dtl.Quantity;

            row.Cells[colNodeQuantityUnit.Name].Value = dtl.MatStandardUnitName;

            row.Cells[colNodeRemark.Name].Value = dtl.Descript;

            row.Cells[colNodeQuality.Name].Value = dtl.QualityStandard;

            row.Cells[colNodeDiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridNodePlan.CurrentCell = row.Cells[0];

        }

        private void AddServicePlanDetailInGrid(LaborDemandPlanDetail dtl, bool isSetCurrentCell, bool isReadOnly)
        {
            int index = gridServicePlan.Rows.Add();
            DataGridViewRow row = gridServicePlan.Rows[index];

            row.Cells[colServiceProjectTaskName.Name].Value = dtl.ProjectTaskName;
            row.Cells[colServiceProjectTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.ProjectTaskName, dtl.ProjectTaskSysCode);

            row.Cells[colServiceBearType.Name].Value = dtl.UsedRankType;
            row.Cells[colServiceProjectQuantity.Name].Value = dtl.Quantity;
            row.Cells[colServiceProjectQnyUnit.Name].Value = dtl.ProjectQuantityUnitName;

            if (dtl.LaborRankInTime != null)
                row.Cells[colServiceBearIncomeDate.Name].Value = StaticMethod.GetShowDateTimeStr(dtl.LaborRankInTime.Value, false);

            row.Cells[colServiceMainWorkContent.Name].Value = dtl.MainJobDescript;
            row.Cells[colServiceQuanlityRequire.Name].Value = dtl.QualitySafetyRequirement;

            row.Cells[colServiceWorkTypesDetail.Name].ToolTipText = "双击可查看工种";

            //row.Cells[colServicePlanLaborDemandNumber.Name].Value = dtl

            row.Tag = dtl;

            row.ReadOnly = isReadOnly;

            if (isSetCurrentCell)
                gridServicePlan.CurrentCell = row.Cells[0];

        }


        public void Start(RemandPlanType planType, string code, string GUID)
        {
            try
            {
                optPlanType = planType;

                if (optPlanType == RemandPlanType.日常需求计划)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList list = model.ObjectQuery(typeof(DailyPlanMaster), oq);
                    if (list.Count > 0)
                    {
                        theDailyPlanMaster = list[0] as DailyPlanMaster;
                        ModelToView();
                        //RefreshState(MainViewState.Browser);
                    }
                }
                else if (optPlanType == RemandPlanType.月度需求计划)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList list = model.ObjectQuery(typeof(MonthlyPlanMaster), oq);
                    if (list.Count > 0)
                    {
                        theMonthPlanMaster = list[0] as MonthlyPlanMaster;
                        ModelToView();
                        //RefreshState(MainViewState.Browser);
                    }
                }
                else if (optPlanType == RemandPlanType.节点需求计划)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList list = model.ObjectQuery(typeof(MonthlyPlanMaster), oq);
                    if (list.Count > 0)
                    {
                        theNodePlanMaster = list[0] as MonthlyPlanMaster;
                        ModelToView();
                        //RefreshState(MainViewState.Browser);
                    }
                }
                else if (optPlanType == RemandPlanType.劳务需求计划)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
                    IList list = model.ObjectQuery(typeof(LaborDemandPlanMaster), oq);
                    if (list.Count > 0)
                    {
                        theLaborPlanMaster = list[0] as LaborDemandPlanMaster;
                        ModelToView();
                        //RefreshState(MainViewState.Browser);
                    }
                }
                else if (optPlanType == RemandPlanType.需求总计划)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("Id", GUID));
                    oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);

                    IList list = model.ObjectQuery(typeof(DemandMasterPlanMaster), oq);
                    if (list.Count > 0)
                    {
                        theMasterPlanMaster = list[0] as DemandMasterPlanMaster;
                        ModelToView();
                        //RefreshState(MainViewState.Browser);
                    }
                }
                PlanTypeShow();
               
            }
            catch (Exception e)
            {
                MessageBox.Show("动态加载出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        void PlanTypeShow()
        {
            if (optPlanType == RemandPlanType.日常需求计划)
            {
                if (tabControlPlan.TabPages.Contains(tabPage日常需求计划) == false)
                    tabControlPlan.TabPages.Add(tabPage日常需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage节点需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage节点需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage劳务需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage劳务需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage需求总计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage需求总计划);
                if (tabControlPlan.TabPages.Contains(tabPage月度需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage月度需求计划);
            }
            else if (optPlanType == RemandPlanType.月度需求计划)
            {
                if (tabControlPlan.TabPages.Contains(tabPage日常需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage日常需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage节点需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage节点需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage劳务需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage劳务需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage需求总计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage需求总计划);
                if (tabControlPlan.TabPages.Contains(tabPage月度需求计划) == false)
                    tabControlPlan.TabPages.Add(tabPage月度需求计划);
            }
            else if (optPlanType == RemandPlanType.节点需求计划)
            {
                if (tabControlPlan.TabPages.Contains(tabPage日常需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage日常需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage节点需求计划) == false)
                    tabControlPlan.TabPages.Add(tabPage节点需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage劳务需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage劳务需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage需求总计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage需求总计划);
                if (tabControlPlan.TabPages.Contains(tabPage月度需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage月度需求计划);

            }
            else if (optPlanType == RemandPlanType.劳务需求计划)
            {
                if (tabControlPlan.TabPages.Contains(tabPage日常需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage日常需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage节点需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage节点需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage劳务需求计划) == false)
                    tabControlPlan.TabPages.Add(tabPage劳务需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage需求总计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage需求总计划);
                if (tabControlPlan.TabPages.Contains(tabPage月度需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage月度需求计划);
            }
            else if (optPlanType == RemandPlanType.需求总计划)
            {
                if (tabControlPlan.TabPages.Contains(tabPage日常需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage日常需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage节点需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage节点需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage劳务需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage劳务需求计划);
                if (tabControlPlan.TabPages.Contains(tabPage需求总计划) == false)
                    tabControlPlan.TabPages.Add(tabPage需求总计划);
                if (tabControlPlan.TabPages.Contains(tabPage月度需求计划) == true)
                    tabControlPlan.TabPages.Remove(tabPage月度需求计划);
            }

        }

        private bool ModelToView()
        {
            try
            {
                if (optPlanType == RemandPlanType.日常需求计划)
                {
                    //gridDayPlan.Rows.Clear();
                    if (theDailyPlanMaster.Details.Count > 0)
                    {
                        foreach (DailyPlanDetail dtl in theDailyPlanMaster.Details)
                        {
                            AddDayPlanDetailInGrid(dtl, false, false);
                        }
                    }
                }
                else if (optPlanType == RemandPlanType.月度需求计划)
                {
                    //gridMonthPlan.Rows.Clear();
                    if (theMonthPlanMaster.Details.Count > 0)
                    {
                        foreach (MonthlyPlanDetail dtl in theMonthPlanMaster.Details)
                        {
                            AddMonthPlanDetailInGrid(dtl, false, false);
                        }
                    }
                }
                else if (optPlanType == RemandPlanType.节点需求计划)
                {
                    //gridNodePlan.Rows.Clear();
                    if (theNodePlanMaster.Details.Count > 0)
                    {
                        foreach (MonthlyPlanDetail dtl in theNodePlanMaster.Details)
                        {
                            AddNodePlanDetailInGrid(dtl, false, false);
                        }
                    }
                }
                else if (optPlanType == RemandPlanType.劳务需求计划)
                {
                    //gridServicePlan.Rows.Clear();
                    if (theLaborPlanMaster.Details.Count > 0)
                    {
                        foreach (LaborDemandPlanDetail dtl in theLaborPlanMaster.Details)
                        {
                            AddServicePlanDetailInGrid(dtl, false, false);
                        }
                    }
                }
                else if (optPlanType == RemandPlanType.需求总计划)
                {
                    //gridMasterPlan.Rows.Clear();
                    if (theMasterPlanMaster.Details.Count > 0)
                    {
                        foreach (DemandMasterPlanDetail dtl in theMasterPlanMaster.Details)
                        {
                            AddMasterPlanDetailInGrid(dtl, false, false);
                        }
                    }
                }


                return true;
            }
            catch (Exception e)
            {
                throw e;
                //MessageBox.Show("数据映射错误：" + StaticMethod.ExceptionMessage(e));
                //return false;
            }
        }

    }
}
