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
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core;
using NHibernate.Criterion;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Threading;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public partial class VProjectTaskAccountBillQuery : TBasicDataView
    {
        public CProjectTaskAccount cAccount;
        /// <summary>
        /// 当前菜单所属的权限菜单
        /// </summary>
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        public MProjectTaskAccount model = new MProjectTaskAccount();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();

        public VProjectTaskAccountBillQuery()
        {
            InitializeComponent();

            InitForm();

            RefreshControls(MainViewState.Browser);
        }
        public VProjectTaskAccountBillQuery(CProjectTaskAccount c)
        {
            InitializeComponent();

            cAccount = c;

            InitForm();

            RefreshControls(MainViewState.Browser);
        }


        private void InitForm()
        {
            InitEvents();

            DateTime serverTime = model.GetServerTime();
            dtAccountStartTime.Value = serverTime.Date.AddDays(-7);
            dtAccountEndDate.Value = serverTime.Date;

            gridAccountBill.ReadOnly = true;

            cbState.Items.Add("");
            foreach (DocumentState state in Enum.GetValues(typeof(DocumentState)))
            {
                string desc = ClientUtil.GetDocStateName(state);
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem();
                li.Text = desc;
                li.Value = ((int)state).ToString();
                cbState.Items.Add(li);
            }

            projectInfo = StaticMethod.GetProjectInfo();
        }

        private void InitEvents()
        {
            btnSelectAccountTaskRootNode.Click += new EventHandler(btnSelectAccountTaskRootNode_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);

            gridAccountBill.CellClick += new DataGridViewCellEventHandler(gridAccountBill_CellClick);
            gridAccountBill.CellDoubleClick += new DataGridViewCellEventHandler(gridAccountBill_CellDoubleClick);

            gridDetail.CellClick += new DataGridViewCellEventHandler(gridDetail_CellClick);
        }


        void btnQuery_Click(object sender, EventArgs e)
        {
            string startCode = txtStartBillCode.Text.Trim();
            DateTime startTime = dtAccountStartTime.Value;
            DateTime endTime = dtAccountEndDate.Value.AddDays(1);
            GWBSTree accountTask = null;
            if (txtAccountRootNode.Text.Trim() != "" && txtAccountRootNode.Tag != null)
                accountTask = txtAccountRootNode.Tag as GWBSTree;


            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));

            oq.AddCriterion(Expression.Ge("CreateDate", startTime));
            oq.AddCriterion(Expression.Lt("CreateDate", endTime));

            if (startCode != "")
                oq.AddCriterion(Expression.Like("Code", startCode, MatchMode.Anywhere));

            if (accountTask != null)
                //oq.AddCriterion(Expression.Eq("AccountRange.Id", accountTask.Id));
                oq.AddCriterion(Expression.Like("AccountTaskSyscode", accountTask.SysCode, MatchMode.Start));
            else if (txtAccountRootNode.Text.Trim() != "")
                oq.AddCriterion(Expression.Like("AccountTaskName", txtAccountRootNode.Text.Trim(), MatchMode.Anywhere));

            if (cbState.Text != "" && cbState.SelectedItem != null)
                oq.AddCriterion(Expression.Eq("DocState", (DocumentState)Convert.ToInt32((cbState.SelectedItem as System.Web.UI.WebControls.ListItem).Value)));

            if (txtOwner.Text.Trim() != "" && txtOwner.Result != null && txtOwner.Result.Count > 0)
            {
                PersonInfo p = txtOwner.Result[0] as PersonInfo;
                if (p != null)
                    oq.AddCriterion(Expression.Eq("AccountPersonGUID.Id", p.Id));
            }
            #region 过滤数据权限
            if (StaticMethod.IsEnabledDataAuth && ConstObject.IsSystemAdministrator() == false && TheAuthMenu != null)//不是系统管理员需要过滤数据权限
            {
                //1.查询数据权限配置
                ObjectQuery oqAuth = new ObjectQuery();
                oqAuth.AddCriterion(Expression.Eq("AuthMenu.Id", TheAuthMenu.Id));

                Disjunction disAuth = new Disjunction();
                foreach (OperationRole role in ConstObject.TheRoles)
                {
                    disAuth.Add(Expression.Eq("AppRole.Id", role.Id));
                }
                oqAuth.AddCriterion(disAuth);

                IEnumerable<AuthConfigOrgSysCode> listAuth = model.ObjectQuery(typeof(AuthConfigOrgSysCode), oqAuth).OfType<AuthConfigOrgSysCode>();

                if (listAuth.Count() > 0)//如果配置了权限规则
                {
                    var query = from a in listAuth
                                where a.ApplyRule == AuthOrgSysCodeRuleEnum.无约束
                                select a;

                    if (query.Count() == 0)//未设置“无约束”规则
                    {
                        disAuth = new Disjunction();

                        //2.根据数据权限定义的规则过滤数据
                        query = from a in listAuth
                                where a.ApplyRule == AuthOrgSysCodeRuleEnum.该核算组织的
                                select a;
                        if (query.Count() > 0)//如果配置中包含操作“该核算组织的”的权限，则无需再添加其它规则
                        {
                            disAuth.Add(Expression.Like("OpgSysCode", ConstObject.TheLogin.TheAccountOrgInfo.SysCode, MatchMode.Start));
                        }
                        else
                        {
                            foreach (AuthConfigOrgSysCode config in listAuth)
                            {
                                WeekScheduleMaster m = new WeekScheduleMaster();

                                if (config.ApplyRule == AuthOrgSysCodeRuleEnum.本人的)
                                {
                                    disAuth.Add(Expression.Eq("HandlePerson.Id", ConstObject.LoginPersonInfo.Id));
                                }
                                else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.本部门的)
                                {
                                    disAuth.Add(Expression.Eq("HandleOrg.Id", ConstObject.TheOperationOrg.Id));
                                }
                                else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.兄弟部门的)
                                {
                                    string theSysCode = ConstObject.TheOperationOrg.SysCode;
                                    if (!string.IsNullOrEmpty(theSysCode) && theSysCode.IndexOf(".") > -1)
                                    {
                                        //获取父组织层次码
                                        theSysCode = theSysCode.Substring(0, theSysCode.Length - 1);
                                        theSysCode = theSysCode.Substring(0, theSysCode.LastIndexOf("."));

                                        AbstractCriterion exp = Expression.And(Expression.Eq("HandOrgLevel", ConstObject.TheOperationOrg.Level),
                                            Expression.And(Expression.Like("OpgSysCode", theSysCode, MatchMode.Start), Expression.Not(Expression.Eq("HandleOrg.Id", ConstObject.TheOperationOrg.Id))));
                                        disAuth.Add(exp);
                                    }
                                }
                                else if (config.ApplyRule == AuthOrgSysCodeRuleEnum.下属部门的)//允许看下级部门的
                                {
                                    disAuth.Add(Expression.And(Expression.Like("OpgSysCode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start), Expression.Not(Expression.Eq("HandleOrg.Id", ConstObject.TheOperationOrg.Id))));
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
                                            disAuth.Add(Expression.Eq("OpgSysCode", sysCode));
                                        }
                                    }
                                }
                            }
                        }

                        string term = disAuth.ToString();
                        if (term != "()")//不加条件时为()
                            oq.AddCriterion(disAuth);
                    }
                }
                else//未配置数据权限缺省为查看本部门和下属部门的数据
                {
                    oq.AddCriterion(Expression.Like("OpgSysCode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start));
                }
            }
            #endregion
            oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateDate"));
            //oq.AddFetchMode("AccountRange", NHibernate.FetchMode.Eager);

            gridAccountBill.Rows.Clear();
            IList list = model.ObjectQuery(typeof(ProjectTaskAccountBill), oq);
            foreach (ProjectTaskAccountBill bill in list)
            {
                AddAccountInfoInGrid(bill);
            }

            if (gridAccountBill.Rows.Count > 0)
            {
                gridAccountBill.Rows[0].Selected = true;
                gridAccountBill_CellClick(gridAccountBill, new DataGridViewCellEventArgs(0, 0));
            }
            else
            {
                gridDetail.Rows.Clear();
                gridDetailSummary.Rows.Clear();
                gridDetailSubject.Rows.Clear();
            }
        }

        void gridAccountBill_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                ProjectTaskAccountBill bill = gridAccountBill.Rows[e.RowIndex].Tag as ProjectTaskAccountBill;
                if (bill != null)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheAccountBill.Id", bill.Id));
                    oq.AddFetchMode("TheCostItem", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("ProjectTaskDtlGUID", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("ProjectTaskDtlGUID.TheCostItem", NHibernate.FetchMode.Eager);
                    IList listDtl = model.ObjectQuery(typeof(ProjectTaskDetailAccount), oq);

                    gridDetail.Rows.Clear();
                    foreach (ProjectTaskDetailAccount dtl in listDtl)
                    {
                        AddAccountDetailInGrid(dtl);
                    }
                    gridDetail.ClearSelection();


                    oq.Criterions.Clear();
                    oq.FetchModes.Clear();
                    oq.AddCriterion(Expression.Eq("TheAccountBill.Id", bill.Id));
                    oq.AddFetchMode("ProjectTaskDtlGUID", NHibernate.FetchMode.Eager);
                    oq.AddFetchMode("ProjectTaskDtlGUID.TheCostItem", NHibernate.FetchMode.Eager);
                    listDtl = model.ObjectQuery(typeof(ProjectTaskDetailAccountSummary), oq);

                    gridDetailSummary.Rows.Clear();
                    foreach (ProjectTaskDetailAccountSummary dtl in listDtl)
                    {
                        AddAccountDetailSummaryInGrid(dtl);
                    }
                    gridDetailSummary.ClearSelection();

                    if (gridDetail.Rows.Count > 0)
                    {
                        gridDetail.Rows[0].Selected = true;
                        gridDetail_CellClick(gridDetail, new DataGridViewCellEventArgs(0, 0));
                    }
                    else
                    {
                        gridDetailSubject.Rows.Clear();
                    }
                }
            }
        }

        void gridAccountBill_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex > -1 && e.ColumnIndex > -1)
            //{
            //    ProjectTaskAccountBill bill = gridAccountBill.Rows[e.RowIndex].Tag as ProjectTaskAccountBill;
            //    if (bill != null && cAccount != null)
            //    {
            //        cAccount.Find(bill.Code, bill.Id);
            //    }
            //}
        }

        void gridDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
            {
                ProjectTaskDetailAccount billDtl = gridDetail.Rows[e.RowIndex].Tag as ProjectTaskDetailAccount;
                if (billDtl != null)
                {
                    ObjectQuery oq = new ObjectQuery();
                    oq.AddCriterion(Expression.Eq("TheAccountDetail.Id", billDtl.Id));
                    IList listDtl = model.ObjectQuery(typeof(ProjectTaskDetailAccountSubject), oq);

                    gridDetailSubject.Rows.Clear();
                    foreach (ProjectTaskDetailAccountSubject dtl in listDtl)
                    {
                        AddAccountDetailSubjectInGrid(dtl);
                    }
                    gridDetailSubject.ClearSelection();

                }
            }
        }

        /// <summary>
        /// 刷新状态(按钮状态)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
        }

        /// <summary>
        /// 刷新控件(窗体中的控件)
        /// </summary>
        /// <param name="state"></param>
        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            switch (state)
            {
                case MainViewState.AddNew:
                    break;
                case MainViewState.Modify:
                    break;
                case MainViewState.Browser:
                    break;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public override void RefreshView()
        {
            try
            {
                btnQuery_Click(btnQuery, new EventArgs());

                RefreshControls(MainViewState.Browser);
            }
            catch (Exception e)
            {
                MessageBox.Show("数据刷新错误：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override bool DeleteView()
        {
            try
            {
                if (gridAccountBill.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要删除的任务核算单！");
                    return false;
                }


                ProjectTaskAccountBill curBillMaster = gridAccountBill.SelectedRows[0].Tag as ProjectTaskAccountBill;
                if (curBillMaster.DocState == DocumentState.Edit)
                {
                    IList list = new ArrayList();
                    list.Add(curBillMaster);
                    if (!model.DeleteProjectTaskAccount(list))
                        return false;

                    LogData log = new LogData();
                    log.BillId = curBillMaster.Id;
                    log.BillType = "工程任务核算单";
                    log.Code = curBillMaster.Code;
                    log.OperType = "删除";
                    log.Descript = "";
                    log.OperPerson = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.ConstObject.LoginPersonInfo.Name;
                    log.ProjectName = curBillMaster.ProjectName;
                    StaticMethod.InsertLogData(log);

                    gridAccountBill.Rows.RemoveAt(gridAccountBill.SelectedRows[0].Index);
                    gridDetail.Rows.Clear();
                    gridDetailSummary.Rows.Clear();
                    gridDetailSubject.Rows.Clear();

                    return true;
                }
                MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show("数据删除错误：" + ExceptionUtil.ExceptionMessage(e));
                return false;
            }
        }


        private void AddAccountInfoInGrid(ProjectTaskAccountBill bill)
        {
            int index = gridAccountBill.Rows.Add();
            DataGridViewRow row = gridAccountBill.Rows[index];

            row.Cells[AccountCode.Name].Value = bill.Code;
            row.Cells[AccountRange.Name].Value = bill.AccountTaskName;
            row.Cells[AccountBillingTime.Name].Value = bill.CreateDate.ToString();
            row.Cells[AccountPerson.Name].Value = bill.AccountPersonName;
            row.Cells[AccountStartTime.Name].Value = bill.BeginTime.ToShortDateString();
            row.Cells[AccountEndTime.Name].Value = bill.EndTime.ToShortDateString();
            row.Cells[AccountState.Name].Value = ClientUtil.GetDocStateName(bill.DocState);
            row.Cells[AccountRemark.Name].Value = bill.Remark;

            row.Tag = bill;
        }

        private void AddAccountDetailInGrid(ProjectTaskDetailAccount dtl)
        {
            int index = gridDetail.Rows.Add();
            DataGridViewRow row = gridDetail.Rows[index];

            row.Cells[DtlProjectTaskNode.Name].Value = dtl.AccountTaskNodeName;
            //row.Cells[DtlProjectTaskNode.Name].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.AccountTaskNodeName, dtl.AccountTaskNodeSyscode);

            row.Cells[DtlProjectTaskDetail.Name].Value = dtl.ProjectTaskDtlName;

            row.Cells[DtlTaskBearer.Name].Value = dtl.BearerName;
            row.Cells[DtlOwner.Name].Value = dtl.ResponsiblePersonName;

            row.Cells[DtlMatFeeBalanceFlag.Name].Value = dtl.MatFeeBlanceFlag.ToString();

            row.Cells[DtlConfirmProjectAmount.Name].Value = dtl.ConfirmQuantity;
            row.Cells[DtlAddupAccQuantity.Name].Value = dtl.AddupAccountQuantity;
            row.Cells[DtlAddupAccProgress.Name].Value = dtl.AddupAccountProgress;
            row.Cells[DtlAccountProjectAmount.Name].Value = dtl.AccountProjectAmount;
            row.Cells[DtlQuantityUnit.Name].Value = dtl.QuantityUnitName;

            row.Cells[DtlAccountPrice.Name].Value = dtl.AccountPrice;
            row.Cells[DtlPriceUnit.Name].Value = dtl.PriceUnitName;
            row.Cells[DtlAccountTotalPrice.Name].Value = dtl.AccountTotalPrice;

            row.Cells[DtlCurrAccFigureProgress.Name].Value = StaticMethod.DecimelTrimEnd0(dtl.CurrAccFigureProgress * 100);

            row.Cells[DtlRemark.Name].Value = dtl.Remark;
            //row.Cells[DtlAccDate.Name].Value = dtl.i
            row.Cells[DtlSettleDate.Name].Value = dtl.BalanceState.ToString();

            if (dtl.ProjectTaskDtlGUID != null)
            {
                row.Cells[DtlMainResourceName.Name].Value = dtl.ProjectTaskDtlGUID.MainResourceTypeName;
                row.Cells[DtlMainResourceSpec.Name].Value = dtl.ProjectTaskDtlGUID.MainResourceTypeSpec;
                row.Cells[DtlDigramNumber.Name].Value = dtl.ProjectTaskDtlGUID.DiagramNumber;
                if (dtl.ProjectTaskDtlGUID.TheCostItem != null)
                    row.Cells[DtlCostItemQuotaCode.Name].Value = dtl.ProjectTaskDtlGUID.TheCostItem.QuotaCode;
            }
            row.Tag = dtl;
        }

        private void AddAccountDetailSummaryInGrid(ProjectTaskDetailAccountSummary dtl)
        {
            int index = gridDetailSummary.Rows.Add();
            DataGridViewRow row = gridDetailSummary.Rows[index];

            row.Cells[SumProjectTaskNode.Name].Value = dtl.AccountNodeName;
            //row.Cells[SumProjectTaskNode.Name].Value = StaticMethod.GetCategorTreeFullPath(typeof(GWBSTree), dtl.AccountNodeName, dtl.AccountNodeSysCode);

            row.Cells[SumProjectTaskDetail.Name].Value = dtl.ProjectTaskDtlName;

            row.Cells[SumCurrAccountProjectAmount.Name].Value = dtl.AccountProjectAmount;
            row.Cells[SumCurrAccFigureProgress.Name].Value = StaticMethod.DecimelTrimEnd0(dtl.CurrAccFigureProgress);

            row.Cells[SumAddupAccProjectAmount.Name].Value = dtl.AddupAccQuantity;
            row.Cells[SumAddupAccFigureProgress.Name].Value = StaticMethod.DecimelTrimEnd0(dtl.AddupAccFigureProgress);

            row.Cells[SumAccountTotalPrice.Name].Value = dtl.AccountTotalPrice;

            row.Cells[SumProjectAmountUnit.Name].Value = dtl.ProjectAmountUnitName;
            row.Cells[SumPriceUnit.Name].Value = dtl.PriceUnitName;

            row.Cells[SumState.Name].Value = dtl.State.ToString();
            row.Cells[SumRemark.Name].Value = dtl.Remark;

            if (dtl.ProjectTaskDtlGUID != null)
            {
                row.Cells[SumMainResourceName.Name].Value = dtl.ProjectTaskDtlGUID.MainResourceTypeName;
                row.Cells[SumMainResourceSpec.Name].Value = dtl.ProjectTaskDtlGUID.MainResourceTypeSpec;
                row.Cells[SumDigramNumber.Name].Value = dtl.ProjectTaskDtlGUID.DiagramNumber;
                if (dtl.ProjectTaskDtlGUID.TheCostItem != null)
                    row.Cells[SumCostItemQuotaCode.Name].Value = dtl.ProjectTaskDtlGUID.TheCostItem.QuotaCode;
            }

            row.Tag = dtl;
        }

        private void AddAccountDetailSubjectInGrid(ProjectTaskDetailAccountSubject dtl)
        {
            int index = gridDetailSubject.Rows.Add();
            DataGridViewRow row = gridDetailSubject.Rows[index];
            row.Cells[DtlCostName.Name].Value = dtl.BestaetigtCostSubjectName;
            row.Cells[DtlCostAccountSubject.Name].Value = dtl.CostingSubjectName;
            row.Cells[DtlResourceType.Name].Value = dtl.ResourceTypeName;
            row.Cells[DtlResourceTypeQuanlity.Name].Value = dtl.ResourceTypeQuality;
            row.Cells[DtlResourceTypeSpec.Name].Value = dtl.ResourceTypeSpec;
            row.Cells[DtlAccQuotaQuantity.Name].Value = dtl.AccountQuantity;
            row.Cells[DtlAccQuantityPrice.Name].Value = dtl.AccountPrice;
            row.Cells[DtlAccProjectAmountPrice.Name].Value = dtl.AccWorkQnyPrice;
            row.Cells[DtlAccUsageQuantity.Name].Value = dtl.AccUsageQny;
            row.Cells[DtlAccTotalPrice.Name].Value = dtl.AccountTotalPrice;
            row.Cells[DtlCurrContractIncomeQny.Name].Value = dtl.CurrContractIncomeQny;
            row.Cells[DtlCurrIncomeContractTotal.Name].Value = dtl.CurrContractIncomeTotal;
            row.Cells[DtlCurrResponsibleCostQny.Name].Value = dtl.CurrResponsibleCostQny;
            row.Cells[DtlCurrResponsibleCostTotal.Name].Value = dtl.CurrResponsibleCostTotal;

            row.Cells[DiagramNumber.Name].Value = dtl.DiagramNumber;

            row.Tag = dtl;
        }

        private string ToDecimailString(decimal value)
        {
            return decimal.Round(value, 3).ToString();
        }

        void btnSelectAccountTaskRootNode_Click(object sender, EventArgs e)
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
                    txtAccountRootNode.Text = task.Name;
                    txtAccountRootNode.Tag = task;
                }
            }
        }
    }
}
