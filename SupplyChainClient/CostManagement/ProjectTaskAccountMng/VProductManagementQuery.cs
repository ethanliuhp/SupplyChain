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
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public partial class VProductManagementQuery : TBasicDataView
    {
        /// <summary>
        /// 当前菜单所属的权限菜单
        /// </summary>
        AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        MProjectTaskAccount model = new MProjectTaskAccount();
        MProductionMng modelProduct = new MProductionMng();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();


        public VProductManagementQuery()
        {
            InitializeComponent();

            InitForm();

            RefreshControls(MainViewState.Browser);
        }

        private void InitForm()
        {
            InitEvents();

            DateTime serverTime = model.GetServerTime();
            dtAccountStartTime.Value = serverTime.Date.AddDays(-7);
            dtAccountEndDate.Value = serverTime.Date;

            projectInfo = StaticMethod.GetProjectInfo();
        }

        private void InitEvents()
        {
            btnSelectAccountTaskRootNode.Click += new EventHandler(btnSelectAccountTaskRootNode_Click);
            btnSelectBalOrg.Click += new EventHandler(btnSelectBalOrg_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnExportExcel.Click += new EventHandler(btnExportExcel_Click);
        }

        void btnExportExcel_Click(object sender, EventArgs e)
        {
            if (gridDetail.Rows.Count == 0)
            {
                MessageBox.Show("当前没有要导出的数据！");
                return;
            }
            gridDetail.SelectAll();
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(gridDetail, true);
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                FlashScreen.Show("正在查询，请稍候........");

                DateTime startTime = dtAccountStartTime.Value;
                DateTime endTime = dtAccountEndDate.Value.AddDays(1);
                GWBSTree accountTask = null;
                SubContractProject taskHander = null;
                if (txtAccountRootNode.Text.Trim() != "" && txtAccountRootNode.Tag != null)
                    accountTask = txtAccountRootNode.Tag as GWBSTree;
                if (txtBalOrg.Text.Trim() != "" && txtBalOrg.Tag != null)
                    taskHander = txtBalOrg.Tag as SubContractProject;

                ObjectQuery oqAccountMaster = new ObjectQuery();
                oqAccountMaster.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oqAccountMaster.AddCriterion(Expression.Ge("CreateDate", startTime));
                oqAccountMaster.AddCriterion(Expression.Lt("CreateDate", endTime));
                oqAccountMaster.AddOrder(NHibernate.Criterion.Order.Asc("CreateDate"));

                ObjectQuery oqAccount = new ObjectQuery();
                oqAccount.AddCriterion(Expression.Eq("TheProjectGUID", projectInfo.Id));

                if (taskHander != null)
                    oqAccount.AddCriterion(Expression.Eq("BearerGUID.Id", taskHander.Id));
                else if (txtBalOrg.Text.Trim() != "")
                    oqAccount.AddCriterion(Expression.Like("BearerName", txtBalOrg.Text.Trim(), MatchMode.Anywhere));

                if (accountTask != null)
                    oqAccount.AddCriterion(Expression.Like("AccountTaskNodeSyscode", accountTask.SysCode, MatchMode.Start));
                else if (txtAccountRootNode.Text.Trim() != "")
                    oqAccount.AddCriterion(Expression.Like("AccountTaskNodeName", txtAccountRootNode.Text.Trim(), MatchMode.Anywhere));

                if (txtOwner.Text.Trim() != "" && txtOwner.Result != null && txtOwner.Result.Count > 0)
                {
                    PersonInfo p = txtOwner.Result[0] as PersonInfo;
                    if (p != null)
                        oqAccount.AddCriterion(Expression.Eq("ResponsiblePerson.Id", p.Id));//工长
                }
                oqAccount.AddFetchMode("AccountTaskNodeGUID", NHibernate.FetchMode.Eager);
                oqAccount.AddFetchMode("ProjectTaskDtlGUID", NHibernate.FetchMode.Eager);
                oqAccount.AddFetchMode("ProjectTaskDtlGUID.TheCostItem", NHibernate.FetchMode.Eager);

                //查询工程任务确认单
                ObjectQuery oqConfirmMaster = new ObjectQuery();
                oqConfirmMaster.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
                oqConfirmMaster.AddCriterion(Expression.Eq("BillType", EnumConfirmBillType.虚拟工单));

                ObjectQuery oqConfirm = new ObjectQuery();
                oqConfirm.AddCriterion(Expression.Eq("AccountingState", EnumGWBSTaskConfirmAccountingState.未核算));
                oqConfirm.AddCriterion(Expression.Gt("ActualCompletedQuantity", Convert.ToDecimal(0)));
                oqConfirm.AddCriterion(Expression.Ge("RealOperationDate", startTime));
                oqConfirm.AddCriterion(Expression.Lt("RealOperationDate", endTime));

                if (taskHander != null)
                    oqConfirm.AddCriterion(Expression.Eq("TaskHandler.Id", taskHander.Id));
                else if (txtBalOrg.Text.Trim() != "")
                    oqConfirm.AddCriterion(Expression.Like("TaskHandlerName", txtBalOrg.Text.Trim(), MatchMode.Anywhere));
                if (accountTask != null)
                    oqConfirm.AddCriterion(Expression.Like("GwbsSysCode", accountTask.SysCode, MatchMode.Start));
                else if (txtAccountRootNode.Text.Trim() != "")
                    oqConfirm.AddCriterion(Expression.Like("GWBSTreeName", txtAccountRootNode.Text.Trim(), MatchMode.Anywhere));
                if (txtOwner.Text.Trim() != "" && txtOwner.Result != null && txtOwner.Result.Count > 0)
                {
                    PersonInfo p = txtOwner.Result[0] as PersonInfo;
                    if (p != null)
                        oqConfirm.AddCriterion(Expression.Eq("CreatePerson.Id", p.Id));//工长
                }
                oqConfirm.AddFetchMode("GWBSTree", NHibernate.FetchMode.Eager);
                oqConfirm.AddFetchMode("GWBSDetail", NHibernate.FetchMode.Eager);
                oqConfirm.AddFetchMode("GWBSDetail.TheCostItem", NHibernate.FetchMode.Eager);


                IList listResult = modelProduct.ProductionManagementSrv.GetTaskConfirmInfo(oqAccountMaster, oqAccount, oqConfirmMaster, oqConfirm);
                IList listAccountDtl = listResult[0] as IList;
                IList listConfirmDtl = listResult[1] as IList;

                gridDetail.Rows.Clear();

                AddAccountDetailInGridByAccountDtl(listAccountDtl);
                AddAccountDetailInGridByConfirmDtl(listConfirmDtl);

                decimal accountConfirmQnyCount = listAccountDtl.OfType<ProjectTaskDetailAccount>().Sum(p => p.AccountProjectAmount);
                decimal accountQnyCount = listAccountDtl.OfType<ProjectTaskDetailAccount>().Sum(p => p.AccountProjectAmount);
                decimal confirmQnyCount = listConfirmDtl.OfType<GWBSTaskConfirm>().Sum(p => p.QuantiyAfterConfirm - p.QuantityBeforeConfirm);

                lblConfirmQnyCount.Text = string.Format("{0:N}", accountConfirmQnyCount + confirmQnyCount);
                lblAccountQnyCount.Text = string.Format("{0:N}", accountQnyCount);

                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show(ExceptionUtil.ExceptionMessage(ex));
            }
            finally
            {
                FlashScreen.Close();
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

        private void AddAccountDetailInGridByAccountDtl(IList listAccountDtl)
        {
            foreach (ProjectTaskDetailAccount dtl in listAccountDtl)
            {
                int index = gridDetail.Rows.Add();
                DataGridViewRow row = gridDetail.Rows[index];

                row.Cells[DtlProjectTaskNode.Name].Value = dtl.AccountTaskNodeName;
                if (dtl.AccountTaskNodeGUID != null)
                {
                    row.Cells[DtlProjectTaskNode.Name].ToolTipText = dtl.AccountTaskNodeGUID.FullPath;
                }
                row.Cells[DtlProjectTaskDetail.Name].Value = dtl.ProjectTaskDtlName;
                row.Cells[DtlIsAccount.Name].Value = "已核算";
                row.Cells[DtlAccountTime.Name].Value = dtl.TheAccountBill.CreateDate;
                row.Cells[DtlTaskBearer.Name].Value = dtl.BearerName;
                row.Cells[DtlOwner.Name].Value = dtl.ResponsiblePersonName;
                row.Cells[DtlMatFeeBalanceFlag.Name].Value = dtl.MatFeeBlanceFlag.ToString();
                row.Cells[DtlConfirmProjectAmount.Name].Value = dtl.ConfirmQuantity;
                row.Cells[DtlAccountProjectAmount.Name].Value = dtl.AccountProjectAmount;
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
        }
        private void AddAccountDetailInGridByConfirmDtl(IList listConfirmDtl)
        {
            foreach (GWBSTaskConfirm dtl in listConfirmDtl)
            {
                int index = gridDetail.Rows.Add();
                DataGridViewRow row = gridDetail.Rows[index];

                row.Cells[DtlProjectTaskNode.Name].Value = dtl.GWBSTreeName;
                row.Cells[DtlProjectTaskDetail.Name].Value = dtl.GWBSDetailName;
                if (dtl.GWBSTree != null)
                {
                    row.Cells[DtlProjectTaskNode.Name].ToolTipText = dtl.GWBSTree.FullPath;
                }
                row.Cells[DtlTaskBearer.Name].Value = dtl.TaskHandlerName;
                row.Cells[DtlIsAccount.Name].Value = "未核算";
                row.Cells[DtlConfirmTime.Name].Value = dtl.RealOperationDate;
                row.Cells[DtlOwner.Name].Value = dtl.CreatePersonName;
                row.Cells[DtlMatFeeBalanceFlag.Name].Value = dtl.MaterialFeeSettlementFlag.ToString();
                row.Cells[DtlConfirmProjectAmount.Name].Value = dtl.QuantiyAfterConfirm - dtl.QuantityBeforeConfirm;
                if (dtl.GWBSDetail != null)
                {
                    row.Cells[DtlMainResourceName.Name].Value = dtl.GWBSDetail.MainResourceTypeName;
                    row.Cells[DtlMainResourceSpec.Name].Value = dtl.GWBSDetail.MainResourceTypeSpec;
                    row.Cells[DtlDigramNumber.Name].Value = dtl.GWBSDetail.DiagramNumber;
                    if (dtl.GWBSDetail.TheCostItem != null)
                        row.Cells[DtlCostItemQuotaCode.Name].Value = dtl.GWBSDetail.TheCostItem.QuotaCode;

                }
                row.Tag = dtl;
            }
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

        //选择结算单位
        void btnSelectBalOrg_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vSubProject = new VContractExcuteSelector();
            vSubProject.ShowDialog();
            IList list = vSubProject.Result;
            if (list == null || list.Count == 0) return;

            SubContractProject subProject = list[0] as SubContractProject;
            this.txtBalOrg.Text = subProject.BearerOrgName;
            this.txtBalOrg.Tag = subProject;
        }
    }
}
