using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using System.Collections;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.ScheduleUI;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan
{
    public partial class VScheduleSelector : TBasicDataView
    {
        private MProductionMng model = new MProductionMng();

        private Hashtable detailHashtable = new Hashtable();

        private PlanTypeQueryEnum optPlanType = PlanTypeQueryEnum.项目周进度计划;

        CurrentProjectInfo projectInfo = null;

        private IList result = new ArrayList();
        /// <summary>
        /// 返回结果
        /// </summary>
        virtual public IList Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// 选择的权限的菜单
        /// </summary>
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;

        public VScheduleSelector()
        {
            InitializeComponent();
            InitData();
            InitEvents();
        }

        private void InitEvents()
        {
            cbPlanType.SelectedIndexChanged += new EventHandler(cbPlanType_SelectedIndexChanged);

            btnQuery.Click += new EventHandler(btnQuery_Click);

            gridMaster.CellClick += new DataGridViewCellEventHandler(gridMaster_CellClick);

            btnOK.Click += new EventHandler(btnOK_Click);
        }

        private void InitData()
        {
            projectInfo = StaticMethod.GetProjectInfo();

            foreach (string planType in Enum.GetNames(typeof(PlanTypeQueryEnum)))
            {
                cbPlanType.Items.Add(planType);
            }
            cbPlanType.SelectedIndex = 0;

            optPlanType = VirtualMachine.Component.Util.EnumUtil<PlanTypeQueryEnum>.FromDescription(cbPlanType.SelectedItem.ToString());

            dtBeginCreateDate.Value = DateTime.Now.AddMonths(-1);
            dtEndCreateDate.Value = DateTime.Now;
        }

        void cbPlanType_SelectedIndexChanged(object sender, EventArgs e)
        {
            optPlanType = VirtualMachine.Component.Util.EnumUtil<PlanTypeQueryEnum>.FromDescription(cbPlanType.SelectedItem.ToString());

            SetPlanName(optPlanType);
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            this.result.Clear();
            if (this.gridMaster.SelectedRows.Count == 0)
            {
                MessageBox.Show("没有可以引用的进度计划！");
                return;
            }
            DataGridViewRow theCurrentRow = this.gridMaster.CurrentRow;
            result.Add(this.gridMaster.CurrentRow.Tag);
            this.btnOK.FindForm().Close();
        }

        void gridMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;

            WeekScheduleMaster master = gridMaster.Rows[e.RowIndex].Tag as WeekScheduleMaster;

            gridDetail.Rows.Clear();
            if (master != null)
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
                oq.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
                //oq.AddFetchMode("SubContractProject", NHibernate.FetchMode.Eager);

                IList listDtl = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleDetail), oq);
                AddWeekPlanDetailInGrid(listDtl);
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
          
            if (optPlanType == PlanTypeQueryEnum.项目周进度计划 || optPlanType == PlanTypeQueryEnum.月进度计划)
            {
                gridDetail.Rows.Clear();
                gridMaster.Rows.Clear();

                EnumExecScheduleType planType = EnumExecScheduleType.周进度计划;
                if (optPlanType == PlanTypeQueryEnum.月进度计划)
                    planType = EnumExecScheduleType.月度进度计划;

                oq.AddCriterion(Expression.Eq("ExecScheduleType", planType));

                //oq.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));

                oq.AddCriterion(Expression.Eq("SummaryStatus", EnumSummaryStatus.汇总生成));
                
                if (txtHandlePerson.Text.Trim() != "" && txtHandlePerson.Result != null && txtHandlePerson.Result.Count > 0)
                {
                    PersonInfo per = txtHandlePerson.Result[0] as PersonInfo;
                    oq.AddCriterion(Expression.Eq("HandlePerson.Id", per.Id));
                }

                oq.AddCriterion(Expression.Ge("CreateDate", dtBeginCreateDate.Value.Date));
                oq.AddCriterion(Expression.Lt("CreateDate", dtEndCreateDate.Value.Date.AddDays(1)));

                //过滤数据权限
                if (TheAuthMenu != null)
                {
                    ObjectQuery oqAuth = new ObjectQuery();
                    oqAuth.AddCriterion(Expression.Eq("AuthMenu.Id", TheAuthMenu.Id));
                    IList listAuth = model.ProductionManagementSrv.ObjectQuery(typeof(AuthConfigOrgSysCode), oqAuth);
                    if (listAuth.Count > 0)
                    {
                        Disjunction disAuth = new Disjunction();

                        foreach (AuthConfigOrgSysCode config in listAuth)
                        {
                            WeekScheduleMaster m = new WeekScheduleMaster();

                            if (config.ApplyRule == AuthOrgSysCodeRuleEnum.本人的)
                            {
                                disAuth.Add(Expression.Eq("HandlePerson.Id", ConstObject.LoginPersonInfo.Id));
                            }
                            else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.本部门的)
                            {
                                disAuth.Add(Expression.Eq("HandlePersonSyscode", ConstObject.TheOperationOrg.SysCode));
                            }
                            else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.兄弟部门的)
                            {
                                string theSysCode = ConstObject.TheOperationOrg.SysCode;
                                if (!string.IsNullOrEmpty(theSysCode) && theSysCode.IndexOf(".") > -1)
                                {
                                    theSysCode = theSysCode.Substring(0, theSysCode.LastIndexOf("."));//获取父组织层次码

                                    AbstractCriterion exp = Expression.And(Expression.Eq("HandOrgLevel", ConstObject.TheOperationOrg.Level), Expression.And(Expression.Like("HandlePersonSyscode", theSysCode, MatchMode.Start),
                                        Expression.Not(Expression.Eq("HandlePersonSyscode", ConstObject.TheOperationOrg.SysCode))));

                                    disAuth.Add(exp);

                                }
                            }
                            else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.下属部门的)//允许看下级部门的
                            {
                                disAuth.Add(Expression.And(Expression.Like("HandlePersonSyscode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start), Expression.Not(Expression.Eq("HandlePersonSyscode", ConstObject.TheOperationOrg.SysCode))));
                            }
                            else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.上级的)//允许看上级部门的
                            {
                                string theSysCode = ConstObject.TheOperationOrg.SysCode;
                                if (!string.IsNullOrEmpty(theSysCode))
                                {
                                    string[] sysCodes = theSysCode.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);

                                    for (int i = 0; i < sysCodes.Length - 1; i++)
                                    {
                                        string sysCode = "";
                                        for (int j = 0; j <= i; j++)
                                        {
                                            sysCode += sysCodes[j] + ".";
                                        }

                                        disAuth.Add(Expression.Eq("HandlePersonSyscode", sysCode));
                                    }
                                }
                            }
                        }
                        string term = disAuth.ToString();
                        if (term != "()")
                            oq.AddCriterion(disAuth);
                    }
                }

                oq.AddOrder(Order.Asc("Code"));

                IList list = model.ProductionManagementSrv.GetObjects(typeof(WeekScheduleMaster), oq);

                gridMaster.Rows.Clear();
                AddWeekPlanMasterInfoInGrid(list);

                if (gridMaster.Rows.Count > 0)
                {
                    gridMaster.CurrentCell = gridMaster.Rows[0].Cells[0];
                    gridMaster_CellClick(gridMaster, new DataGridViewCellEventArgs(0, 0));
                }
            }

        }

        private void SetPlanName(PlanTypeQueryEnum queryPlanType)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

                
                if (queryPlanType == PlanTypeQueryEnum.项目周进度计划)
                {
                    oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.周进度计划));
                    oq.AddCriterion(Expression.Eq("SummaryStatus", EnumSummaryStatus.汇总生成));
                }
                else if (queryPlanType == PlanTypeQueryEnum.月进度计划)
                {
                    oq.AddCriterion(Expression.Eq("ExecScheduleType", EnumExecScheduleType.月度进度计划));
                }

                IList listMaster = model.ProductionManagementSrv.ObjectQuery(typeof(WeekScheduleMaster), oq);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作失败，异常信息：" + ExceptionUtil.ExceptionMessage(ex));
            }
        }

        private void AddWeekPlanMasterInfoInGrid(IList listMaster)
        {
            foreach (WeekScheduleMaster master in listMaster)
            {
                int index = gridMaster.Rows.Add();
                DataGridViewRow row = gridMaster.Rows[index];

                row.Cells[colMasterPlanName.Name].Value = master.PlanName;
                row.Cells[colMasterPlanState.Name].Value = ClientUtil.GetDocStateName(master.DocState);
                row.Cells[colMasterSumState.Name].Value = master.SummaryStatus;
                row.Cells[colMasterHandlePerson.Name].Value = master.HandlePersonName;
                row.Cells[colMasterCreateDate.Name].Value = master.CreateDate.ToString();
                row.Cells[colMasterPlannedBeginDate.Name].Value = master.PlannedBeginDate.ToShortDateString();
                row.Cells[colMasterPlannedDateEnd.Name].Value = master.PlannedEndDate.ToShortDateString();
                row.Cells[colMasterDescript.Name].Value = master.Descript;

                row.Tag = master;
            }
        }

         private void AddWeekPlanMasterInfoInGrid(WeekScheduleMaster master)
         {
             int index = gridMaster.Rows.Add();
             DataGridViewRow row = gridMaster.Rows[index];

             row.Cells[colMasterPlanName.Name].Value = master.PlanName;
             row.Cells[colMasterPlanState.Name].Value = ClientUtil.GetDocStateName(master.DocState);
             row.Cells[colMasterSumState.Name].Value = master.SummaryStatus;
             row.Cells[colMasterHandlePerson.Name].Value = master.HandlePersonName;
             row.Cells[colMasterCreateDate.Name].Value = master.CreateDate.ToString();
             row.Cells[colMasterPlannedBeginDate.Name].Value = master.PlannedBeginDate.ToShortDateString();
             row.Cells[colMasterPlannedDateEnd.Name].Value = master.PlannedEndDate.ToShortDateString();
             row.Cells[colMasterDescript.Name].Value = master.Descript;

             row.Tag = master;
         }

         private void AddWeekPlanDetailInGrid(IList listDtl)
         {
             foreach (WeekScheduleDetail dtl in listDtl)
             {
                 int index = gridDetail.Rows.Add();
                 DataGridViewRow row = gridDetail.Rows[index];

                 if (dtl.GWBSTree != null)
                 {
                     row.Cells[colDtlTaskName.Name].Value = dtl.GWBSTreeName;
                     row.Cells[colDtlTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.GWBSTreeName, dtl.GWBSTreeSysCode);
                 }

                 row.Cells[colDtlWorkContent.Name].Value = dtl.MainTaskContent;

                 row.Cells[colDtlBear.Name].Value = dtl.SupplierName;

                 row.Cells[colDtlPlanBeginTime.Name].Value = dtl.PlannedBeginDate.ToShortDateString();
                 row.Cells[colDtlPlanOverTime.Name].Value = dtl.PlannedEndDate.ToShortDateString();

                 row.Cells[colDtlPlannedDuration.Name].Value = dtl.PlannedDuration;
                 row.Cells[colDtlPlannedWrokQuantity.Name].Value = dtl.PlannedWrokload;

                 row.Cells[colDtlFiguregress.Name].Value = dtl.TaskCompletedPercent;

                 row.Cells[colDtlCheckState.Name].ToolTipText = StaticMethod.GetCheckStateShowText(dtl.TaskCheckState);
                 row.Cells[colDtlCheckState.Name].Value = dtl.TaskCheckState;

                 row.Cells[colDtlRemark.Name].Value = dtl.Descript;

                 row.Tag = dtl;
             }
         }

         private void AddWeekPlanDetailInGrid(WeekScheduleDetail dtl)
         {
             int index = gridDetail.Rows.Add();
             DataGridViewRow row = gridDetail.Rows[index];

             if (dtl.GWBSTree != null)
             {
                 row.Cells[colDtlTaskName.Name].Value = dtl.GWBSTree.Name;
                 row.Cells[colDtlTaskName.Name].ToolTipText = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.GWBSTree);
             }

             row.Cells[colDtlWorkContent.Name].Value = dtl.MainTaskContent;

             row.Cells[colDtlBear.Name].Value = dtl.SupplierName;

             row.Cells[colDtlPlanBeginTime.Name].Value = dtl.PlannedBeginDate.ToShortDateString();
             row.Cells[colDtlPlanOverTime.Name].Value = dtl.PlannedEndDate.ToShortDateString();

             row.Cells[colDtlPlannedDuration.Name].Value = dtl.PlannedDuration;
             row.Cells[colDtlPlannedWrokQuantity.Name].Value = dtl.PlannedWrokload;

             row.Cells[colDtlFiguregress.Name].Value = dtl.TaskCompletedPercent;

             row.Cells[colDtlCheckState.Name].ToolTipText = StaticMethod.GetCheckStateShowText(dtl.TaskCheckState);
             row.Cells[colDtlCheckState.Name].Value = dtl.TaskCheckState;

             row.Cells[colDtlRemark.Name].Value = dtl.Descript;

             row.Tag = dtl;
         }
    }

    /// <summary>
    /// 查询计划类型
    /// </summary>
    public enum PlanTypeQueryEnum
    {
        [Description("项目周进度计划")]
        项目周进度计划 = 1,
        [Description("月进度计划")]
        月进度计划 = 2
    }
}
