using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;
using VirtualMachine.Core;
using System.Collections;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using AuthManagerLib.AuthMng.AuthConfigMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.SubContractBalanceMng
{
    public partial class VSubContractBalanceQuery : TBasicDataView
    {
        public CSubContractBalance cBalance;
        public AuthManagerLib.AuthMng.MenusMng.Domain.Menus TheAuthMenu = null;
        public MSubContractBalance model = new MSubContractBalance();
        CurrentProjectInfo projectInfo = new CurrentProjectInfo();
        private IList subjectList = null;//存放 人材机 核算科目

        public VSubContractBalanceQuery()
        {
            InitializeComponent();
            InitForm();
            RefreshControls(MainViewState.Browser);
        }

        public VSubContractBalanceQuery(CSubContractBalance c)
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
            dtpBalanceDateBegin.Value = serverTime.Date.AddMonths(-1);
            dtAccountStartTime.Value = serverTime.Date.AddDays(-7);
            dtAccountEndDate.Value = serverTime.Date;
            gridAccountBill.ReadOnly = true;
            projectInfo = StaticMethod.GetProjectInfo();
            if (projectInfo != null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrg.Tag = ConstObject.TheOperationOrg;
                btnOperationOrg.Visible = false;
                txtOperationOrgDetail.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrgDetail.Tag = ConstObject.TheOperationOrg;
                btnOperationOrgDetail.Visible = false;
            }
            btnAll.Checked = true;
        }

        private void InitEvents()
        {
            btnSelect.Click += new EventHandler(btnSelect_Click);
            btnBalanceSelect.Click += new EventHandler(btnBalanceSelect_Click);
            this.btnExcelBill.Click += new EventHandler(btnExcelBill_Click);
            btnSelectBalOrg.Click += new EventHandler(btnSelectBalOrg_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);
            gridAccountBill.CellContentClick += new DataGridViewCellEventHandler(gridAccountBill_CellContentClick);
            dgBalanceDtl.CellContentClick += new DataGridViewCellEventHandler(dgBalanceDtl_CellContentClick);
            this.gridAccountBill.SelectionChanged += new EventHandler(gridAccountBill_SelectionChanged);
            this.dgDetail.SelectionChanged += new EventHandler(dgDetail_SelectionChanged);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnSelectCostSubject.Click += new EventHandler(btnSelectSubject_Click);
            this.btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            this.btnOperationOrgDetail.Click += new EventHandler(btnOperationOrgDetail_Click);
        }


        private void btnOperationOrg_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                var selOrg = frm.Result[0] as OperationOrgInfo;
                txtOperationOrg.Tag = selOrg;
                txtOperationOrg.Text = selOrg.Name;
            }
        }
        private void btnOperationOrgDetail_Click(object sender, EventArgs e)
        {
            VCommonOperationOrgSelect frm = new VCommonOperationOrgSelect(true);
            frm.ShowDialog();
            if (frm.Result != null && frm.Result.Count > 0)
            {
                var selOrg = frm.Result[0] as OperationOrgInfo;
                txtOperationOrgDetail.Tag = selOrg;
                txtOperationOrgDetail.Text = selOrg.Name;
            }
        }

        void btnSelectSubject_Click(object sender, EventArgs e)
        {
            VSelectCostAccountSubject frm = new VSelectCostAccountSubject(new MCostAccountSubject());
            frm.IsLeafSelect = false;
            frm.ShowDialog();
            CostAccountSubject cost = frm.SelectAccountSubject;
            if (cost != null)
            {
                this.txtCostSubject.Text = cost.Name;
                this.txtCostSubject.Tag = cost;
            }
        }
        void btnBalanceSelect_Click(object sender, EventArgs e)
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
                    txtBalance.Text = task.Name;
                    txtBalance.Tag = task;
                }
            }
        }

        void btnExcelBill_Click(object sender, EventArgs e)
        {
            Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.ExcelClass.SaveDataGridViewToExcel(this.dgBalanceDtl, true);
        }

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

        void btnSelect_Click(object sender, EventArgs e)
        {
            VContractExcuteSelector vces = new VContractExcuteSelector();
            vces.StartPosition = FormStartPosition.CenterScreen;
            vces.ShowDialog();
            if (vces.Result != null && vces.Result.Count > 0)
            {
                SubContractProject scp = vces.Result[0] as SubContractProject;
                txtBalanceName.Text = scp.BearerOrgName;
                txtBalanceName.Tag = scp;
            }
        }

        void btnQuery_Click(object sender, EventArgs e)
        {
            DateTime startTime = dtAccountStartTime.Value;
            DateTime endTime = dtAccountEndDate.Value.AddDays(1);

            SubContractProject subProject = txtBalOrg.Tag as SubContractProject;
            string sSubProjectName = txtBalOrg.Text.Trim();
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Ge("CreateDate", startTime));
            oq.AddCriterion(Expression.Lt("CreateDate", endTime));
            //oq.AddCriterion(Expression.Eq("ProjectId", projectInfo.Id));
            var org = txtOperationOrg.Tag as OperationOrgInfo;
            if (org != null)
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", org.SysCode, MatchMode.Start));
            }
            else
            {
                oq.AddCriterion(Expression.Like("OpgSysCode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start));
            }
            //单据
            if (txtCodeBeginBill.Text != "")
            {
                oq.AddCriterion(Expression.Like("Code", txtCodeBeginBill.Text, MatchMode.Anywhere));
            }

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
            else if (!string.IsNullOrEmpty(sSubProjectName))
            {
                oq.AddCriterion(Expression.Like("TheSubContractProject.BearerOrgName", sSubProjectName,MatchMode.Anywhere));
              
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
                                    disAuth.Add(Expression.And(Expression.Like("OpgSysCode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start)
                                        , Expression.Not(Expression.Eq("HandleOrg.Id", ConstObject.TheOperationOrg.Id))));
                                    //有权限规则的不支持选择范围
                                    //if (org != null) disAuth.Add(Expression.And(Expression.Like("OpgSysCode", org.SysCode, MatchMode.Start)
                                    //    , Expression.Not(Expression.Eq("HandleOrg.Id", org.Id))));
                                    //else disAuth.Add(Expression.And(Expression.Like("OpgSysCode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start)
                                    //    , Expression.Not(Expression.Eq("HandleOrg.Id", ConstObject.TheOperationOrg.Id))));
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
                    //oq.AddCriterion(Expression.Like("OpgSysCode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start));
                    if (org != null) oq.AddCriterion(Expression.Like("OpgSysCode", org.SysCode, MatchMode.Start));
                    else oq.AddCriterion(Expression.Like("OpgSysCode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start));
                }
            }
            #endregion
            //oq.AddOrder(NHibernate.Criterion.Order.Asc("CreateDate"));
            oq.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheSubContractProject", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("Details.Details", NHibernate.FetchMode.Eager);
            gridAccountBill.Rows.Clear();
            IList list = model.ObjectQuery(typeof(SubContractBalanceBill), oq);

            //查询人材机核算科目
            oq.Criterions.Clear();
            oq.FetchModes.Clear();
            Disjunction dis = new Disjunction();
            dis.Add(Expression.Eq("Code", "C5110121"));
            dis.Add(Expression.Eq("Code", "C5110122"));
            dis.Add(Expression.Eq("Code", "C5110123"));
            oq.AddCriterion(dis);
            subjectList = model.ObjectQuery(typeof(CostAccountSubject), oq);

            ShowMasterList(list);

        }

        void gridAccountBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = gridAccountBill[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == BalanceCode.Name)
            {
                projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo == null || string.Equals(projectInfo.Code, CommonUtil.CompanyProjectCode))//分公司和公司无法打印
                {
                    MessageBox.Show("公司或者分公司无法打印分包结算单");
                     
                    return;
                }
                else
                {
                    SubContractBalanceBill bill = gridAccountBill.Rows[e.RowIndex].Tag as SubContractBalanceBill;
                    if (bill.DocState != DocumentState.InExecute)
                    {
                        MessageBox.Show("分包结算单[" + bill.Code + "]未审核完，不能打印！");
                        return;
                    }
                    VSubContractBalReport frm = new VSubContractBalReport(bill);
                    if (frm.ifSign == true)
                    {
                        frm.ShowDialog();
                    }
                }
                
            }
        }

        void dgBalanceDtl_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;
            DataGridViewCell dgvCell = dgBalanceDtl[e.ColumnIndex, e.RowIndex];
            if (dgvCell.Value == null || string.IsNullOrEmpty(dgvCell.Value.ToString())) return;
            if (dgvCell.OwningColumn.Name == colBalanceCode.Name)
            {
                projectInfo = StaticMethod.GetProjectInfo();
                if (projectInfo == null || string.Equals(projectInfo.Code, CommonUtil.CompanyProjectCode))//分公司和公司无法打印
                {
                    MessageBox.Show("公司或者分公司无法打印分包结算单");

                    return;
                }
                else
                {
                    string masterId = dgBalanceDtl.Rows[e.RowIndex].Tag.ToString();
                    SubContractBalanceBill bill = model.SubBalSrv.GetSubContractBalanceMasterById(masterId);
                    //gridAccountBill.Rows[e.RowIndex].Tag as SubContractBalanceBill;
                    VSubContractBalReport frm = new VSubContractBalReport(bill);
                    frm.ShowDialog();
                }
            }

        }
        private void ShowMasterList(IList list)
        {
            gridAccountBill.Rows.Clear();
            dgDetail.Rows.Clear();
            dgSubject.Rows.Clear();
            if (list == null || list.Count == 0) return;
            decimal sumMoney = 0;
            foreach (SubContractBalanceBill master in list)
            {
                int rowIndex = gridAccountBill.Rows.Add();
                gridAccountBill.Rows[rowIndex].Tag = master;
                gridAccountBill[this.BalanceCode.Name, rowIndex].Value = master.Code;
                gridAccountBill[SubContractUnit.Name, rowIndex].Value = master.SubContractUnitName;
                gridAccountBill[this.SubBalMoney.Name, rowIndex].Value = master.BalanceMoney;
                object money = master.BalanceMoney;
                if (money != null)
                {
                    sumMoney += decimal.Parse(money.ToString());
                }
                gridAccountBill[BalanceRange.Name, rowIndex].Value = master.BalanceTaskName;
                gridAccountBill[BalanceOwner.Name, rowIndex].Value = master.CreatePersonName;
                gridAccountBill[BalanceBillingTime.Name, rowIndex].Value = master.CreateDate.ToShortDateString();
                gridAccountBill[this.BalanceStartTime.Name, rowIndex].Value = master.BeginTime.ToShortDateString();
                gridAccountBill[this.BalanceEndTime.Name, rowIndex].Value = master.EndTime.ToShortDateString();
                gridAccountBill[this.BalanceState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);

            }
            this.txtSumMoney.Text = sumMoney.ToString("#,###.####");
            gridAccountBill.CurrentCell = gridAccountBill[1, 0];
            gridAccountBill_SelectionChanged(gridAccountBill, new EventArgs());
            lblRecordTotal.Text = "共【" + list.Count + "】条记录";

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
            decimal subjectDtlCoPrice = 0;//分包结算单人材机耗用合价
            foreach (SubContractBalanceDetail detail in subContractBalanceDetailList)
            {
                int rowIndex = dgDetail.Rows.Add();
                dgDetail.Rows[rowIndex].Tag = detail;
                dgDetail[this.colBalanceBase.Name, rowIndex].Value = detail.BalanceBase;
                dgDetail[this.colDTaskNode.Name, rowIndex].Value = detail.BalanceTaskName;
                dgDetail[this.colDTaskMx.Name, rowIndex].Value = detail.BalanceTaskDtlName;
                dgDetail[this.colBalMoney.Name, rowIndex].Value = detail.BalanceTotalPrice;
                dgDetail[this.colBalPrice.Name, rowIndex].Value = detail.BalancePrice;
                dgDetail[this.colBalQty.Name, rowIndex].Value = detail.BalacneQuantity;
                dgDetail[this.colForwardType.Name, rowIndex].Value = detail.FontBillType.ToString();
                dgDetail[this.colHandlePerson.Name, rowIndex].Value = detail.HandlePersonName;
                dgDetail[this.colQtyUnit.Name, rowIndex].Value = detail.QuantityUnitName;
                dgDetail[this.colPriceUnit.Name, rowIndex].Value = detail.PriceUnitName;
                dgDetail[this.colUseDescript.Name, rowIndex].Value = detail.Remarks;

                if (detail.BalanceTaskDtl != null && detail.BalanceTaskDtl.Id != "0")
                {
                    dgDetail[this.colMainResourceName.Name, rowIndex].Value = detail.BalanceTaskDtl.MainResourceTypeName;
                    dgDetail[this.colMainResourceSpec.Name, rowIndex].Value = detail.BalanceTaskDtl.MainResourceTypeSpec;
                    dgDetail[this.colDigramNumber.Name, rowIndex].Value = detail.BalanceTaskDtl.DiagramNumber;
                    if (detail.BalanceTaskDtl.TheCostItem != null)
                        dgDetail[this.colCostItemQuotaCode.Name, rowIndex].Value = detail.BalanceTaskDtl.TheCostItem.QuotaCode;
                }
            }
            foreach (SubContractBalanceDetail scb in subContractBalanceDetailList)
            {
                foreach (SubContractBalanceSubjectDtl dtlConsume in scb.Details)
                {
                    foreach (CostAccountSubject s in subjectList)
                    {
                        if (dtlConsume.BalanceSubjectGUID.Id == s.Id)
                        {
                            subjectDtlCoPrice += dtlConsume.BalanceTotalPrice;
                            break;
                        }
                    }
                }
            }
 
            dgDetail_SelectionChanged(master, new EventArgs());
            txtRCJCoPrice.Text = subjectDtlCoPrice.ToString();
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

        void btnSearch_Click(object sender, EventArgs e)
        {
            string condition = "";
            projectInfo = StaticMethod.GetProjectInfo();
            if (this.txtCodeBegin.Text != "")
            {
                condition = condition + " and t1.Code like '%" + this.txtCodeBegin.Text + "%'";
            }
            if (this.btnYes.Checked)
            {
                condition += "and t1.monthaccbillid is not null ";
            }
            if (this.btnNo.Checked)
            {
                condition += "and t1.monthaccbillid is null ";
            }
            if (StaticMethod.IsUseSQLServer())
            {
                condition += " and t1.CreateDate>='" + dtpBalanceDateBegin.Value.Date.ToShortDateString() + "' and t1.CreateDate<'" + dtpBalanceDateEnd.Value.AddDays(1).Date.ToShortDateString() + "'";
            }
            else
            {
                condition += " and t1.CreateDate>=to_date('" + dtpBalanceDateBegin.Value.Date.ToShortDateString() + "','yyyy-mm-dd') and t1.CreateDate<to_date('" + dtpBalanceDateEnd.Value.AddDays(1).Date.ToShortDateString() + "','yyyy-mm-dd')";
            }
            if (!txtPersonName.Text.Trim().Equals("") && txtPersonName.Result != null)
            {
                condition = condition + " and t1.CreatePersonName='" + (txtPersonName.Result[0] as PersonInfo).Name + "'";
            }
            //物资
            if (!txtMaterial.Text.Trim().Equals("") && txtMaterial.Result != null)
            {
                condition = condition + " and t3.Resourcetypename='" + txtMaterial.Text + "'";
            }
            // GWBS
            GWBSTree selectGWBS = txtBalance.Tag as GWBSTree;
            if (this.txtBalance.Text != "" && selectGWBS != null)
            {
                condition += "and t2.balancetasksyscode like '%" + selectGWBS.Id + "%'";

            }
            // 科目
            CostAccountSubject selectCostSub = txtCostSubject.Tag as CostAccountSubject;
            if (this.txtCostSubject.Text != "" && selectCostSub != null)
            {
                condition += "and t3.balancesubjectsyscode like '%" + selectCostSub.Id + "%'";

            }
            if (txtBalanceName.Text != "")
            {
                condition = condition + " and t1.subcontractunitname like '%" + txtBalanceName.Text + "%'";
            }
//#if DEBUG
//            if (projectInfo!=null && !projectInfo.Code.Equals(CommonUtil.CompanyProjectCode))
//            {
//                condition = condition + "and t1.projectid ='" + projectInfo.Id + "'";
//            }
//#else
//            condition = condition + "and t1.projectid ='" + projectInfo.Id + "'";
//#endif

            //CurrentProjectInfo projectInfo = StaticMethod.GetProjectInfo();
            //condition += "and t1.ProjectId = '" + projectInfo.Id + "'";
            var org = txtOperationOrgDetail.Tag as OperationOrgInfo;
            if (org != null) condition += String.Format(" and t1.OpgSysCode like '{0}%'", org.SysCode);
            else condition += String.Format(" and t1.OpgSysCode like '{0}%'", ConstObject.TheOperationOrg.SysCode);
            //if (org != null)
            //{
            //    objectQuery.AddCriterion(Expression.Like("OpgSysCode", org.SysCode, MatchMode.Start));
            //}
            //else
            //{
            //    objectQuery.AddCriterion(Expression.Like("OpgSysCode", ConstObject.TheOperationOrg.SysCode, MatchMode.Start));
            //}


            DataSet ds = model.SubBalSrv.SubContractBalanceQuery(condition);
            this.dgBalanceDtl.Rows.Clear();

            DataTable dt = ds.Tables[0];

            decimal sumMoney = 0;
            foreach (DataRow row in dt.Rows)
            {
                int i = dgBalanceDtl.Rows.Add();
                dgBalanceDtl[colBalanceCode.Name, i].Value = row["code"];
                dgBalanceDtl.Rows[i].Tag = row["Id"];
                dgBalanceDtl[colSubContractUnit.Name, i].Value = row["subcontractunitname"];
                dgBalanceDtl[colSubBalMoney.Name, i].Value = row["balancemoney"];
                dgBalanceDtl[colBalanceRange.Name, i].Value = row["balancetaskname"];
                dgBalanceDtl[colBalanceOwner.Name, i].Value = row["createpersonname"];
                dgBalanceDtl[colBalanceBillingTime.Name, i].Value = ClientUtil.ToDateTime(row["createdate"]).ToShortDateString();
                dgBalanceDtl[colBalanceStartTime.Name, i].Value = ClientUtil.ToDateTime(row["begintime"]).ToShortDateString();
                dgBalanceDtl[colBalanceEndTime.Name, i].Value = ClientUtil.ToDateTime(row["endtime"]).ToShortDateString();
                dgBalanceDtl[colBalanceState.Name, i].Value = ClientUtil.GetDocStateName(int.Parse(row["state"].ToString()));
                dgBalanceDtl[colBalanceTaskName.Name, i].Value = row["dtltaskname"];
                dgBalanceDtl[colBalanceTaskDtlName.Name, i].Value = row["balancetaskdtlname"];
                dgBalanceDtl[this.colBalanceBalBase.Name, i].Value = row["balancebase"];
                dgBalanceDtl[colFontBillType.Name, i].Value = Enum.GetName(typeof(FrontBillType), Convert.ToInt32(row["fontbilltype"].ToString()));
                dgBalanceDtl[DtlHandlePerson.Name, i].Value = row["HandlePersonName"].ToString();
                dgBalanceDtl[colBalacneQuantity.Name, i].Value = row["balancequantity"];
                dgBalanceDtl[colBalancePrice.Name, i].Value = row["balanceprice"];
                dgBalanceDtl[colBalanceTotalPrice.Name, i].Value = row["balancetotalprice"];
                dgBalanceDtl[this.colMonthCost.Name, i].Value = ClientUtil.ToString(row["monthaccbillid"]) == "" ? "否" : "是";

                sumMoney += ClientUtil.ToDecimal(row["balancetotalprice"]);
                dgBalanceDtl[colQuantityUnitName.Name, i].Value = row["quantityunitname"];
                dgBalanceDtl[colPriceUnitName.Name, i].Value = row["priceunitname"];
                dgBalanceDtl[colUsedAccount.Name, i].Value = row["usedescript"];
                dgBalanceDtl[CostName.Name, i].Value = row["costname"];
                dgBalanceDtl[ResourceTypeName.Name, i].Value = row["resourcetypename"];
                dgBalanceDtl[ResourceTypeSpec.Name, i].Value = row["resourcetypespec"];
                dgBalanceDtl[DiagramNumber.Name, i].Value = row["diagramnumber"];
                dgBalanceDtl[BalanceSubjectName.Name, i].Value = row["balancesubjectname"];
            }
            lblRecordTotalBill.Text = "共【" + dt.Rows.Count + "】条记录";
            this.txtSumQuantityBill.Text = sumMoney.ToString("#,###.####");
            this.dgBalanceDtl.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        }
    }
}
