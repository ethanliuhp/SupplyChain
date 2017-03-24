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

using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Threading;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using NHibernate.Criterion;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;


namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VSubContractBalanceBillQuery : TBasicDataView
    {
        public CSubContractBalance cBalance;
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        public MSubContractBalance model = new MSubContractBalance();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();

        public VSubContractBalanceBillQuery()
        {
            InitializeComponent();
            InitForm();
            RefreshControls(MainViewState.Browser);
        }
        public VSubContractBalanceBillQuery(CSubContractBalance c)
        {
            InitializeComponent();
            cBalance = c;
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
            projectInfo = StaticMethod.GetProjectInfo();
        }

        private void InitEvents()
        {

            btnSelectAccountTaskRootNode.Click += new EventHandler(btnSelectAccountTaskRootNode_Click);
            btnSelectBalOrg.Click += new EventHandler(btnSelectBalOrg_Click);

            btnQuery.Click += new EventHandler(btnQuery_Click);
            gridAccountBill.CellContentClick += new DataGridViewCellEventHandler(gridAccountBill_CellContentClick);

            this.gridAccountBill.SelectionChanged += new EventHandler(gridAccountBill_SelectionChanged);
            this.dgDetail.SelectionChanged += new EventHandler(dgDetail_SelectionChanged);
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
            VContractExcuteSelector vces = new VContractExcuteSelector();
            vces.StartPosition = FormStartPosition.CenterScreen;
            vces.ShowDialog();
            if (vces.Result != null && vces.Result.Count > 0)
            {
                SubContractProject scp = vces.Result[0] as SubContractProject;
                txtBalOrg.Text = scp.BearerOrgName;
                txtBalOrg.Tag = scp;
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            DateTime startTime = dtAccountStartTime.Value;
            DateTime endTime = dtAccountEndDate.Value.AddDays(1);
            GWBSTree accountTask = txtAccountRootNode.Tag as GWBSTree;

            SubContractProject subProject = txtBalOrg.Tag as SubContractProject;

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("CreateDate", startTime));
            oq.AddCriterion(Expression.Lt("CreateDate", endTime));
            oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            if (accountTask != null && ClientUtil.ToString(txtAccountRootNode.Text) != "")
                oq.AddCriterion(Expression.Eq("BalanceRange.Id", accountTask.Id));
            else if (ClientUtil.ToString(txtAccountRootNode.Text) != "")
                oq.AddCriterion(Expression.Like("BalanceTaskName", txtAccountRootNode.Text.Trim(), MatchMode.Anywhere));

            if (txtOwner.Text.Trim() != "" && txtOwner.Result != null && txtOwner.Result.Count > 0)
            {
                PersonInfo p = txtOwner.Result[0] as PersonInfo;
                if (p != null)
                    oq.AddCriterion(Expression.Eq("CreatePerson.Id", p.Id));
            }

            if (subProject != null)
            {
                oq.AddCriterion(Expression.Eq("TheSubContractProject.Id", subProject.Id));
            }
            #region 过滤数据权限
            if (StaticMethod.IsEnabledDataAuth && ConstObject.TheLogin.TheAccountOrgInfo != null && ConstObject.IsSystemAdministrator() == false && TheAuthMenu != null)//不是系统管理员需要过滤数据权限
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

                IEnumerable<AuthConfigOrgSysCode> listAuth = model.SubBalSrv.ObjectQuery(typeof(AuthConfigOrgSysCode), oqAuth).OfType<AuthConfigOrgSysCode>();

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
            //oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateDate"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheSubContractProject", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            gridAccountBill.Rows.Clear();
            IList list = model.ObjectQuery(typeof(SubContractBalanceBill), oq);
            ShowMasterList(list);

        }

        void gridAccountBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = gridAccountBill[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == BalanceCode.Name)
            {
                SubContractBalanceBill bill = gridAccountBill.Rows[e.RowIndex].Tag as SubContractBalanceBill;
                VSubContractBalReport frm = new VSubContractBalReport(bill);
                frm.ShowDialog();
            }

        }

        //显示主表
        private void ShowMasterList(IList list)
        {
            gridAccountBill.Rows.Clear();
            dgDetail.Rows.Clear();
            dgSubject.Rows.Clear();
            if (list == null || list.Count == 0) return;
            foreach (SubContractBalanceBill master in list)
            {
                int rowIndex = gridAccountBill.Rows.Add();
                gridAccountBill.Rows[rowIndex].Tag = master;
                gridAccountBill[this.BalanceCode.Name, rowIndex].Value = master.Code;
                gridAccountBill[SubContractUnit.Name, rowIndex].Value = master.SubContractUnitName;
                gridAccountBill[this.SubBalMoney.Name, rowIndex].Value = master.BalanceMoney;
                gridAccountBill[BalanceRange.Name, rowIndex].Value = master.BalanceTaskName;
                gridAccountBill[BalanceOwner.Name, rowIndex].Value = master.CreatePersonName;
                gridAccountBill[BalanceBillingTime.Name, rowIndex].Value = master.CreateDate.ToShortDateString();
                gridAccountBill[this.BalanceStartTime.Name, rowIndex].Value = master.BeginTime.ToShortDateString();
                gridAccountBill[this.BalanceEndTime.Name, rowIndex].Value = master.EndTime.ToShortDateString();
                gridAccountBill[this.BalanceState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
            }
            gridAccountBill.CurrentCell = gridAccountBill[1, 0];
            gridAccountBill_SelectionChanged(gridAccountBill, new EventArgs());
        }

        void gridAccountBill_SelectionChanged(object sender, EventArgs e)
        {
            this.dgDetail.Rows.Clear();

            SubContractBalanceBill master = gridAccountBill.CurrentRow.Tag as SubContractBalanceBill;
            if (master == null) return;
            //SubContractBalanceDetail d = new SubContractBalanceDetail();
            //d.Details
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Master.Id", master.Id));
            oq.AddFetchMode("BalanceTaskDtl", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("BalanceTaskDtl.TheCostItem", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList subContractBalanceDetailList = model.ObjectQuery(typeof(SubContractBalanceDetail), oq);
            //分包结算明细
            foreach (SubContractBalanceDetail detail in subContractBalanceDetailList)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = detail;
                dgDetail[this.colDTaskNode.Name, rowIndex].Value = detail.BalanceTaskName;
                dgDetail[this.colDTaskMx.Name, rowIndex].Value = detail.BalanceTaskDtlName;
                dgDetail[this.colBalMoney.Name, rowIndex].Value = detail.BalanceTotalPrice;
                dgDetail[this.colBalPrice.Name, rowIndex].Value = detail.BalancePrice;
                dgDetail[this.colBalQty.Name, rowIndex].Value = detail.BalacneQuantity;
                dgDetail[this.colForwardType.Name, rowIndex].Value = detail.FontBillType.ToString();
                dgDetail[this.DtlHandlePerson.Name, rowIndex].Value = detail.HandlePersonName;
                dgDetail[this.colQtyUnit.Name, rowIndex].Value = detail.QuantityUnitName;
                dgDetail[this.colPriceUnit.Name, rowIndex].Value = detail.PriceUnitName;
                dgDetail[this.colUseDescript.Name, rowIndex].Value = detail.Remarks;//备注

                if (detail.BalanceTaskDtl != null && detail.BalanceTaskDtl.Id != "0")
                {
                    dgDetail[this.colMainResourceName.Name, rowIndex].Value = detail.BalanceTaskDtl.MainResourceTypeName;
                    dgDetail[this.colMainResourceSpec.Name, rowIndex].Value = detail.BalanceTaskDtl.MainResourceTypeSpec;
                    dgDetail[this.colDigramNumber.Name, rowIndex].Value = detail.BalanceTaskDtl.DiagramNumber;
                    if (detail.BalanceTaskDtl.TheCostItem != null)
                        dgDetail[this.colCostItemQuotaCode.Name, rowIndex].Value = detail.BalanceTaskDtl.TheCostItem.QuotaCode;
                }
            }
            dgDetail_SelectionChanged(master, new EventArgs());
        }

        void dgDetail_SelectionChanged(object sender, EventArgs e)
        {
            this.dgSubject.Rows.Clear();

            SubContractBalanceDetail detail = dgDetail.CurrentRow.Tag as SubContractBalanceDetail;
            if (detail == null) return;
            //分包结算物资耗用
            foreach (SubContractBalanceSubjectDtl dtlConsume in detail.Details)
            {
                int rowIndex = this.dgSubject.Rows.Add();
                dgSubject.Rows[rowIndex].Tag = dtlConsume;
                dgSubject[this.colCostName.Name, rowIndex].Value = dtlConsume.CostName;
                dgSubject[this.colResourceType.Name, rowIndex].Value = dtlConsume.ResourceTypeName;
                dgSubject[this.colResourceSpec.Name, rowIndex].Value = dtlConsume.ResourceTypeSpec;
                dgSubject[this.colResDiagramNumber.Name, rowIndex].Value = dtlConsume.DiagramNumber;
                dgSubject[this.colBalSubject.Name, rowIndex].Value = dtlConsume.BalanceSubjectName;
                dgSubject[this.colBalQty1.Name, rowIndex].Value = dtlConsume.BalanceQuantity;
                dgSubject[this.colBalPrice1.Name, rowIndex].Value = dtlConsume.BalancePrice;
                dgSubject[this.colBalMoney1.Name, rowIndex].Value = dtlConsume.BalanceTotalPrice;
                dgSubject[this.colQtyUnit1.Name, rowIndex].Value = dtlConsume.QuantityUnitName;
                dgSubject[this.colPriceUnit1.Name, rowIndex].Value = dtlConsume.PriceUnitName;
            }
        }

    }
}
