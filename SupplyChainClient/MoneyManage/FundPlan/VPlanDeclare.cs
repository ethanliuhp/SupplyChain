using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using FlexCell;
using Iesi.Collections.Generic;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public partial class VPlanDeclare : TMasterDetailView
    {
        private const string XIAOJI = "小计";
        private const string HEJI = "合计";
        private List<OperationOrgInfo> companyOrg;
        private MFinanceMultData mOperate = new MFinanceMultData();
        private CurrentProjectInfo currentProject;
        private OperationOrgInfo ownOrg;
        private int loginOrgtype = 0;
        private ProjectFundPlanMaster projectFundPlan;
        private FilialeFundPlanMaster filialeFundPlan;
        private BaseMaster curBillMaster;

        public VPlanDeclare()
        {
            InitializeComponent();

            InitData();

            InitEvents();

            SetTabPageVisible();
        }

        ///<summary>
        /// 当前单据
        ///</summary>
        public ProjectFundPlanMaster CurProjectFundPlanMaster
        {
            set { this.projectFundPlan = value; }
            get { return this.projectFundPlan; }
        }

        public void Start(string Id)
        {
            try
            {
                if (Id == "空")
                {
                    RefreshState(MainViewState.Initialize);
                }
                else
                {
                    ObjectQuery objQuery = new ObjectQuery();
                    objQuery.AddCriterion(Expression.Eq("Id", Id));

                    if (currentProject != null)
                    {
                        var list = mOperate.FinanceMultDataSrv.GetProjectFundPlanMasterByOQ(objQuery);
                        if (list == null || list.Count == 0)
                        {
                            MessageBox.Show("获取单据信息失败");
                            return;
                        }
                        projectFundPlan = list[0] as ProjectFundPlanMaster;
                        curBillMaster = projectFundPlan;
                    }
                    else
                    {
                        var list = mOperate.FinanceMultDataSrv.GetFilialeFundPlanMasterByOQ(objQuery);
                        if (list == null || list.Count == 0)
                        {
                            MessageBox.Show("获取单据信息失败");
                            return;
                        }
                        filialeFundPlan = list[0] as FilialeFundPlanMaster;
                        curBillMaster = filialeFundPlan;
                    }

                    txtCode.Text = curBillMaster.Code;
                    txtState.Text = ClientUtil.GetDocStateName(curBillMaster.DocState);

                    if (!ModelToView())
                    {
                        MessageBox.Show("加载数据失败");
                    }

                    SetGridState();
                    RefreshState(MainViewState.Browser);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("视图启动出错：" + ExceptionUtil.ExceptionMessage(e));
            }
        }

        public override void RefreshState(MainViewState state)
        {
            base.RefreshState(state);
            //控制表格
            switch (state)
            {
                case MainViewState.AddNew:
                case MainViewState.Modify:
                    cmbYear.Enabled = cmbMonth.Enabled = true;
                    break;
                case MainViewState.Initialize:
                case MainViewState.Browser:
                    cmbYear.Enabled = cmbMonth.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        public override void RefreshControls(MainViewState state)
        {
            base.RefreshControls(state);

            //控制自身控件
            if (ViewState == MainViewState.AddNew || ViewState == MainViewState.Modify)
            {
                ObjectLock.Unlock(pnlFloor, true);
            }
            else
            {
                ObjectLock.Lock(pnlFloor, true);
            }

            //永久锁定
            object[] os = new object[] {txtCode, txtState};
            ObjectLock.Lock(os);
        }

        public override bool NewView()
        {
            base.NewView();

            ClearView();

            SetGridState();
            return true;
        }

        public override bool ModifyView()
        {
            if (curBillMaster.DocState == DocumentState.Edit || curBillMaster.DocState == DocumentState.Valid)
            {
                base.ModifyView();

                return true;
            }

            string message = "此单状态为【{0}】，不能修改！";
            message = string.Format(message, ClientUtil.GetDocStateName(curBillMaster.DocState));
            MessageBox.Show(message);

            return false;
        }

        public override bool SaveView()
        {
            return SaveOrSubmit(1, true);
        }

        public override bool SubmitView()
        {
            return SaveOrSubmit(2, true);
        }

        public override bool DeleteView()
        {
            if (curBillMaster.DocState == DocumentState.Valid || curBillMaster.DocState == DocumentState.Edit)
            {
                if (!mOperate.FinanceMultDataSrv.Delete(curBillMaster)) return false;

                LogData log = new LogData();
                log.BillId = curBillMaster.Id;
                log.BillType = "资金计划";
                log.Code = curBillMaster.Code;
                log.OperType = "删除";
                log.Descript = "";
                log.OperPerson = ConstObject.LoginPersonInfo.Name;
                log.ProjectName = curBillMaster.ProjectName;
                StaticMethod.InsertLogData(log);

                ClearView();

                MessageBox.Show("删除成功！");
                return true;
            }

            MessageBox.Show("此单状态为【" + ClientUtil.GetDocStateName(curBillMaster.DocState) + "】，不能删除！");
            return false;
        }

        public override bool CancelView()
        {
            return true;
        }

        public override bool Preview()
        {
            return false;
        }

        public override bool Print()
        {
            return false;
        }

        public override bool Export()
        {
            return false;
        }

        public override void RefreshView()
        {
            ModelToView();
        }

        private bool ValidView()
        {
            if (currentProject != null)
            {
                if (projectFundPlan == null)
                {
                    return false;
                }

                var errItem =
                    projectFundPlan.Details.OfType<ProjectFundPlanDetail>().FirstOrDefault(
                        a => a.PlanPayment > a.CumulativeExpireDue);
                if (errItem != null)
                {
                    MessageBox.Show(string.Format("【{0}】的【{1}】的计划支付金额超出最大限额【{2}】",
                        errItem.CreditorUnitLeadingOfficial,
                        errItem.FundPaymentCategory,
                        errItem.CumulativeExpireDue.ToString("N4")));
                    return false;
                }
            }

            return true;
        }

        private bool ViewToModel()
        {
            if (currentProject != null)
            {
                CreateOtherExpend();

                ProjectExpendCellToModel();
            }
            else
            {
                CreateOfficeExpend();
            }

            if (!ValidView())
            {
                return false;
            }

            return true;
        }

        private bool ModelToView()
        {
            if (currentProject != null)
            {
                FundPlanOperate.DisplayOtherPlanDetail(projectFundPlan, gdOtherExpend,
                                                       string.Concat(tpOtherExpend.Tag, ".flx"));

                FundPlanOperate.DispalyProjectPlanDetail(projectFundPlan, gdProjectExpend,
                                                         string.Concat(tpProjectExpend.Tag, ".flx"));

                FundPlanOperate.DisplayProjectPlanFundFlow(projectFundPlan, gdProjectReport);

                FundPlanOperate.DisplayProjectReportDetail(projectFundPlan, gdProjectReport);
            }
            else
            {
                FundPlanOperate.DisplayFilialePlanMaster(filialeFundPlan, gdCompanyReport,
                                                         string.Concat(tpCompanyReport.Tag, ".flx"));

                FundPlanOperate.DisplayFilialePlanReportDetail(filialeFundPlan, gdCompanyReport);

                FundPlanOperate.DisplayOfficePlanDetail(filialeFundPlan, gdOfficeExpend,
                                                        string.Concat(tPOfficeExpend.Tag, ".flx"));

                var list = mOperate.FinanceMultDataSrv.GetFilialeFundPlanProjectDetail(filialeFundPlan);
                if (list != null)
                {
                    foreach (ProjectFundPlanMaster plan in list)
                    {
                        var project = mOperate.CurrentProjectSrv.GetProjectById(plan.ProjectId);
                        if (project != null)
                        {
                            plan.ProjectState = ((EnumProjectCurrState)project.ProjectCurrState).ToString();
                        }
                    }
                }
                
                FundPlanOperate.DisplayFilialePlanProjectDetailsByName(list, gdTotalExpend1,
                                                                       string.Concat(tpTotalExpend1.Tag, ".flx"));
                gdTotalExpend1.Cell(2, 1).Text = "申报单位：" + filialeFundPlan.DeclareUnit;
                gdTotalExpend1.Cell(2, 4).Text = "申报日期：" + filialeFundPlan.CreateDate.ToString("yyyy-MM-dd");

                FundPlanOperate.DisplayFilialePlanProjectDetailsByState(list, gdTotalExpend2,
                                                                       string.Concat(tpTotalExpend2.Tag, ".flx"));
                gdTotalExpend2.Cell(2, 1).Text = "申报单位：" + filialeFundPlan.DeclareUnit;
                gdTotalExpend2.Cell(2, 4).Text = "申报日期：" + filialeFundPlan.CreateDate.ToString("yyyy-MM-dd");
            }

            return true;
        }

        private void ClearView()
        {
            ClearControl(pnlFloor);

            InitFlex();

            InitGridTitle();

            txtState.Text = ClientUtil.GetDocStateName(DocumentState.Edit);
        }

        private void ClearControl(Control c)
        {
            foreach (Control cd in c.Controls)
            {
                ClearControl(cd);
            }
            //自定义控件清空
            if (c is CustomEdit)
            {
                c.Tag = null;
                c.Text = "";
            }
            else if (c is CustomDataGridView)
            {
                (c as CustomDataGridView).Rows.Clear();
            }
        }

        private void InitData()
        {
            var startYear = 2006;
            var endYear = DateTime.Now.Year;
            var yearList = new List<int>();
            for (int i = endYear; i >= startYear; i--)
            {
                yearList.Add(i);
            }
            cmbYear.DataSource = yearList;

            var monList = new List<int>();
            for (var i = 1; i <= 12; i++)
            {
                monList.Add(i);
            }
            cmbMonth.DataSource = monList;
            cmbMonth.SelectedItem = DateTime.Now.Month;

            GetCompanyOrg();

            InitFlex();

            InitGridTitle();
        }

        private void InitEvents()
        {
            tspMenuDelete.Click += new EventHandler(tspMenuDelete_Click);
            tspMenuInsert.Click += new EventHandler(tspMenuInsert_Click);
            tspMenuRefresh.Click += new EventHandler(tspMenuRefresh_Click);
            contextMenuStrip1.Opening += new CancelEventHandler(contextMenuStrip1_Opening);

            gdOtherExpend.CellChange += new FlexCell.Grid.CellChangeEventHandler(gdOtherExpend_CellChange);
            gdOfficeExpend.CellChange += new Grid.CellChangeEventHandler(gdOfficeExpend_CellChange);
            gdProjectExpend.CellChange += new Grid.CellChangeEventHandler(gdProjectExpend_CellChange);
            gdProjectReport.CellChange += new Grid.CellChangeEventHandler(gdProjectReport_CellChange);
            gdCompanyReport.CellChange += new Grid.CellChangeEventHandler(gdCompanyReport_CellChange);

            btnGetData.Click += new EventHandler(btnGetData_Click);
        }

        private void InitFlex()
        {
            foreach (TabPage tPage in tabControl.TabPages)
            {
                if (tPage.Tag == null)
                {
                    continue;
                }

                var fName = string.Concat(tPage.Tag, ".flx");
                var grid = FindGrid(tPage);
                if (grid != null)
                {
                    FundPlanOperate.LoadFlexFile(fName, grid);
                    grid.DefaultRowHeight = 23;
                    if (grid.Name == gdOtherExpend.Name || grid.Name == gdOfficeExpend.Name)
                    {
                        CommonUtil.SetGridColumnLockState(grid, new List<int>() {1, 2}, true);
                    }
                }
            }
        }

        private void SetGridState()
        {
            var isLocked = curBillMaster != null && curBillMaster.DocState != DocumentState.Edit;

            foreach (TabPage tPage in tabControl.TabPages)
            {
                if (tPage.Tag == null)
                {
                    continue;
                }

                var fName = string.Concat(tPage.Tag, ".flx");
                var grid = FindGrid(tPage);
                if (grid != null)
                {
                    grid.Locked = isLocked;
                }
            }
        }

        private void GetCompanyOrg()
        {
            var list = mOperate.FinanceMultDataSrv.GetSubCompanySyscodeList();
            if (list == null || list.Count == 0)
            {
                return;
            }

            list.Add(TransUtil.CompanyOpgSyscode);
            ObjectQuery objQuery = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (var sysCode in list)
            {
                dis.Add(Expression.Eq("SysCode", sysCode));
            }
            objQuery.AddCriterion(dis);

            companyOrg = mOperate.FinanceMultDataSrv.Query(typeof (OperationOrgInfo), objQuery)
                .OfType<OperationOrgInfo>().ToList();

            SetOrgType();
        }

        private CustomFlexGrid FindGrid(TabPage tPage)
        {
            if (tPage == null)
            {
                return null;
            }

            foreach (Control ctrl in tPage.Controls)
            {
                if (ctrl is CustomFlexGrid)
                {
                    return ctrl as CustomFlexGrid;
                }
            }

            return null;
        }

        private void SetOrgType()
        {
            if (currentProject == null)
            {
                currentProject = StaticMethod.GetProjectInfo();
            }

            if (currentProject != null && currentProject.Code.Equals(CommonUtil.CompanyProjectCode)) //项目
            {
                currentProject = null;
            }

            ownOrg = companyOrg.Find(c => ConstObject.TheOperationOrg.SysCode.StartsWith(c.SysCode));
            if (ownOrg.SysCode == TransUtil.CompanyOpgSyscode)
            {
                loginOrgtype = 1; //总部
            }
            else if (ownOrg.Name.Contains("直管"))
            {
                loginOrgtype = 2; //直管
            }
            else
            {
                loginOrgtype = 3; //分公司
            }
        }

        private void SetTabPageVisible()
        {
            if (currentProject != null)
            {
                tpCompanyReport.Parent = null;
                tpOtherExpend.Parent = tabControl;
                tpProjectExpend.Parent = tabControl;
                tpProjectReport.Parent = tabControl;
                tpTotalExpend1.Parent = null;
                tpTotalExpend2.Parent = null;
                tPOfficeExpend.Parent = null;
            }
            else if (loginOrgtype == 1)
            {
                tpCompanyReport.Parent = null;
                tpOtherExpend.Parent = null;
                tpProjectExpend.Parent = null;
                tpProjectReport.Parent = null;
                tpTotalExpend1.Parent = null;
                tpTotalExpend2.Parent = null;
                tPOfficeExpend.Parent = tabControl;
            }
            else if (loginOrgtype == 2)
            {
                tpCompanyReport.Parent = null;
                tpOtherExpend.Parent = tabControl;
                tpProjectExpend.Parent = tabControl;
                tpProjectReport.Parent = tabControl;
                tpTotalExpend1.Parent = null;
                tpTotalExpend2.Parent = null;
                tPOfficeExpend.Parent = null;
            }
            else if (loginOrgtype == 3)
            {
                tpCompanyReport.Parent = tabControl;
                tpOtherExpend.Parent = null;
                tpProjectExpend.Parent = null;
                tpProjectReport.Parent = null;
                tpTotalExpend1.Parent = tabControl;
                tpTotalExpend2.Parent = tabControl;
                tPOfficeExpend.Parent = tabControl;
            }
        }

        private void InitGridTitle()
        {
            if (ownOrg != null)
            {
                gdCompanyReport.Cell(1, 1).Text = string.Format("{0}资金支付计划申报表", ownOrg.Name);
                gdCompanyReport.Cell(2, 1).Text = string.Format("申报单位名称：{0}", ownOrg.Name);
                gdOfficeExpend.Cell(2, 1).Text = string.Format("申报单位名称：{0}", ownOrg.Name);
                gdTotalExpend1.Cell(2, 1).Text = string.Format("申报单位名称：{0}", ownOrg.Name);
                gdTotalExpend2.Cell(2, 1).Text = string.Format("申报单位名称：{0}", ownOrg.Name);
            }

            if (currentProject != null)
            {
                gdOtherExpend.Cell(2, 1).Text = string.Format("申报项目名称：	{0}", currentProject.Name);
                gdProjectExpend.Cell(2, 1).Text = string.Format("申报项目名称：	{0}", currentProject.Name);
                gdProjectReport.Cell(2, 1).Text = string.Format("申报项目名称：	{0}", currentProject.Name);
            }

            var rptDate = DateTime.Now.Date.ToString("yyyy年MM月dd日");
            gdCompanyReport.Cell(2, 6).Text = string.Format("申报日期：{0}", rptDate);
            gdOfficeExpend.Cell(2, 3).Text = string.Format("申报日期：{0}", rptDate);
            gdOtherExpend.Cell(2, 3).Text = string.Format("申报日期：{0}", rptDate);
            gdProjectExpend.Cell(2, 9).Text = string.Format("申报日期：{0}", rptDate);
            gdProjectReport.Cell(2, 7).Text = string.Format("申报日期：{0}", rptDate);
            gdTotalExpend1.Cell(2, 4).Text = string.Format("申报日期：{0}", rptDate);
            gdTotalExpend2.Cell(1, 1).Text = string.Format("{0}年{1}月资金计划项目申报汇总表", DateTime.Now.Year, DateTime.Now.Month);
            gdTotalExpend2.Cell(2, 4).Text = string.Format("申报日期：{0}", rptDate);
        }

        private bool SaveOrSubmit(int opType,bool isShowMes)
        {
            if (!ViewToModel())
            {
                return false;
            }

            if (currentProject != null)
            {
                if (opType != 1)
                {
                    projectFundPlan.SubmitDate = DateTime.Now;
                    projectFundPlan.DocState = DocumentState.InAudit;
                }
                else
                {
                    projectFundPlan.DocState = DocumentState.Edit;
                }
                projectFundPlan = mOperate.FinanceMultDataSrv.SaveProjectFundPlan(projectFundPlan);

                txtCode.Text = projectFundPlan.Code;
                txtState.Text = ClientUtil.GetDocStateName(projectFundPlan.DocState);
            }
            else
            {
                if (opType != 1)
                {
                    filialeFundPlan.SubmitDate = DateTime.Now;
                    filialeFundPlan.DocState = DocumentState.InAudit;
                }
                else
                {
                    filialeFundPlan.DocState = DocumentState.Edit;
                }
                filialeFundPlan = mOperate.FinanceMultDataSrv.SaveFilialeFundPlan(filialeFundPlan);
                txtCode.Text = filialeFundPlan.Code;
                txtState.Text = ClientUtil.GetDocStateName(filialeFundPlan.DocState);
            }

            this.ViewCaption = ViewName + "-" + txtCode.Text;
            if (isShowMes)
            {
                MessageBox.Show(string.Format("{0}成功", opType == 1 ? "保存" : "提交"));
            }

            ModelToView();
            return true;
        }

        private void NewProjectPlan()
        {
            if (projectFundPlan == null)
            {
                projectFundPlan = new ProjectFundPlanMaster();
                projectFundPlan.AttachBusinessOrg = ownOrg;
                projectFundPlan.AttachBusinessOrgName = ownOrg.Name;
                projectFundPlan.CreateDate = DateTime.Now;
                projectFundPlan.CreatePerson = ConstObject.TheLogin.ThePerson;
                projectFundPlan.CreatePersonName = projectFundPlan.CreatePerson.Name;
                projectFundPlan.DeclareDate = DateTime.Now.Date;
                projectFundPlan.DocState = DocumentState.Edit;
                projectFundPlan.IsReport = loginOrgtype == 3 ? 1 : 0;
                projectFundPlan.OperOrgInfo = ConstObject.TheOperationOrg;
                projectFundPlan.OperOrgInfoName = projectFundPlan.OperOrgInfo.Name;
                projectFundPlan.OpgSysCode = projectFundPlan.OperOrgInfo.SysCode;
                projectFundPlan.ProjectId = currentProject.Id;
                projectFundPlan.ProjectName = currentProject.Name;
                projectFundPlan.ReportUnit = projectFundPlan.OperOrgInfoName;
                projectFundPlan.Unit = "元";
                projectFundPlan.ApprovalAmount = 0;
            }

            projectFundPlan.CreateYear = TransUtil.ToInt(cmbYear.Text);
            projectFundPlan.CreateMonth = TransUtil.ToInt(cmbMonth.Text);
            projectFundPlan = mOperate.FinanceMultDataSrv.GetProjectFundFlow(projectFundPlan);
            projectFundPlan.ContractAppointGatheringRatio = currentProject.ContractCollectRatio/100;
            projectFundPlan.ContractAccountsDue = currentProject.ContractCollectRatio/100*currentProject.ProjectCost;
            projectFundPlan.FundStock = projectFundPlan.CumulativeGathering - projectFundPlan.CumulativePayment;
            if (projectFundPlan.FinanceConfirmTaxIncome != 0)
            {
                projectFundPlan.ActualGatheringRatio = Math.Round(projectFundPlan.CumulativeGathering/
                                                                  projectFundPlan.FinanceConfirmTaxIncome, 4);
            }

            curBillMaster = projectFundPlan;
        }

        private void NewFilialePlan()
        {
            if (filialeFundPlan == null)
            {
                filialeFundPlan = new FilialeFundPlanMaster();
                filialeFundPlan.CreateDate = DateTime.Now;
                filialeFundPlan.CreatePerson = ConstObject.TheLogin.ThePerson;
                filialeFundPlan.CreatePersonName = filialeFundPlan.CreatePerson.Name;
                filialeFundPlan.DeclareDate = DateTime.Now.Date;
                filialeFundPlan.DeclarePerson = ConstObject.TheLogin.ThePerson.Id;
                filialeFundPlan.DeclareUnit = ownOrg.Name;
                filialeFundPlan.DeclareOrg = ownOrg;
                filialeFundPlan.DocState = DocumentState.Edit;
                filialeFundPlan.OperOrgInfo = ConstObject.TheOperationOrg;
                filialeFundPlan.OperOrgInfoName = filialeFundPlan.OperOrgInfo.Name;
                filialeFundPlan.OpgSysCode = filialeFundPlan.OperOrgInfo.SysCode;
                filialeFundPlan.Unit = "元";
                filialeFundPlan.Approval = 0;
            }

            filialeFundPlan.CreateYear = TransUtil.ToInt(cmbYear.Text);
            filialeFundPlan.CreateMonth = TransUtil.ToInt(cmbMonth.Text);
            filialeFundPlan = mOperate.FinanceMultDataSrv.GetFilialeFundPlanFlow(filialeFundPlan);
            filialeFundPlan.CumulativeCurrentYearFundFlow = filialeFundPlan.CumulativeCurrentYearGathering
                - filialeFundPlan.CumulativeCurrentYearPayment;

            curBillMaster = filialeFundPlan;
        }

        private void CreateOtherExpend()
        {
            if (projectFundPlan == null)
            {
                NewProjectPlan();
            }

            var payRange = string.Empty;
            projectFundPlan.OtherPayDetails.Clear();
            for (int i = 4; i < gdOtherExpend.Rows; i++)
            {
                if (string.IsNullOrEmpty(payRange))
                {
                    payRange = gdOtherExpend.Cell(i, 1).Text;
                }

                if (!string.IsNullOrEmpty(payRange) && payRange.Replace(" ", "").Contains(HEJI))
                {
                    payRange = string.Empty;
                    continue;
                }

                var numStr = gdOtherExpend.Cell(i, 2).Text;
                if (!string.IsNullOrEmpty(numStr) && numStr.Replace(" ", "").Contains(XIAOJI))
                {
                    payRange = string.Empty;
                    continue;
                }

                var dt = new ProjectOtherPayPlanDetail();
                dt.Master = projectFundPlan;
                projectFundPlan.OtherPayDetails.Add(dt);
                dt.PayScope = payRange;

                var numTmp = 0;
                if (int.TryParse(numStr, out numTmp))
                {
                    dt.OrderNumber = numTmp;
                }

                dt.CostDetail = gdOtherExpend.Cell(i, 3).Text;

                var payTmp = 0m;
                if (decimal.TryParse(gdOtherExpend.Cell(i, 4).Text, out payTmp))
                {
                    dt.PlanDeclarePayment = payTmp;
                }
                dt.Descript = gdOtherExpend.Cell(i, 5).Text;
                dt.TempData = Guid.NewGuid().ToString();
            }

            projectFundPlan.PresentMonthPayment =
                projectFundPlan.Details.OfType<ProjectFundPlanDetail>().Sum(a => a.PlanPayment)
                + projectFundPlan.OtherPayDetails.OfType<ProjectOtherPayPlanDetail>().Sum(b => b.PlanDeclarePayment);
        }

        private void CreateOfficeExpend()
        {
            if (filialeFundPlan == null)
            {
                NewFilialePlan();
            }

            var payRange = string.Empty;
            filialeFundPlan.OfficeFundPlanDetails.Clear();
            for (int i = 4; i < gdOfficeExpend.Rows; i++)
            {
                if (string.IsNullOrEmpty(payRange))
                {
                    payRange = gdOfficeExpend.Cell(i, 1).Text;
                }

                if (!string.IsNullOrEmpty(payRange) && payRange.Replace(" ", "").Contains(HEJI))
                {
                    payRange = string.Empty;
                    continue;
                }

                var numStr = gdOfficeExpend.Cell(i, 2).Text;
                if (string.IsNullOrEmpty(numStr) || numStr.Replace(" ", "").Contains(XIAOJI))
                {
                    payRange = string.Empty;
                    continue;
                }

                var dt = new OfficeFundPlanPayDetail();
                dt.Master = filialeFundPlan;
                filialeFundPlan.OfficeFundPlanDetails.Add(dt);

                dt.PayScope = payRange;

                var numTmp = 0;
                if (int.TryParse(numStr, out numTmp))
                {
                    dt.OrderNumber = numTmp;
                }

                dt.CostDetail = gdOfficeExpend.Cell(i, 3).Text;

                var payTmp = 0m;
                if (decimal.TryParse(gdOfficeExpend.Cell(i, 4).Text, out payTmp))
                {
                    dt.PlanDeclarePayment = payTmp;
                }
                dt.Descript = gdOfficeExpend.Cell(i, 5).Text;
                dt.TempData = Guid.NewGuid().ToString();
            }

            filialeFundPlan.PresentMonthPlanPayment =
                filialeFundPlan.Details.OfType<FilialeFundPlanDetail>().Sum(a => a.PresentMonthPlanPayment)
                + filialeFundPlan.OfficeFundPlanDetails.OfType<OfficeFundPlanPayDetail>().Sum(b => b.PlanDeclarePayment);
        }

        private void CreateProjectFundPlanDetail()
        {
            if (projectFundPlan == null)
            {
                NewProjectPlan();
            }

            projectFundPlan = mOperate.FinanceMultDataSrv.CreateProjectFundDetail(projectFundPlan);
            projectFundPlan.PresentMonthPayment =
                projectFundPlan.Details.OfType<ProjectFundPlanDetail>().Sum(a => a.PlanPayment)
                + projectFundPlan.OtherPayDetails.OfType<ProjectOtherPayPlanDetail>().Sum(b => b.PlanDeclarePayment);
            projectFundPlan.MonthEndCumulativeFundStock = projectFundPlan.FundStock +
                                                          projectFundPlan.PresentMonthGathering -
                                                          projectFundPlan.PresentMonthPayment;
        }

        private void CreateFilialePlanDetail(List<ProjectFundPlanMaster> projectPlans)
        {
            if (filialeFundPlan == null)
            {
                NewFilialePlan();
            }

            filialeFundPlan.PresentMonthGathering = projectPlans.Sum(a => a.PresentMonthGathering);
            filialeFundPlan.PresentMonthPlanPayment = projectPlans.Sum(a => a.PresentMonthPayment);
            filialeFundPlan.PresentMonthSpendableFund = filialeFundPlan.TillLastMonthFundStock
                                                        + filialeFundPlan.PresentMonthGathering
                                                        - filialeFundPlan.ThereinSuperviseAccountFund;
            filialeFundPlan.CurrentYearFundNetFlow = filialeFundPlan.CumulativeCurrentYearFundFlow
                                                     + filialeFundPlan.PresentMonthGathering
                                                     - filialeFundPlan.PresentMonthPlanPayment;

            filialeFundPlan.Details.Clear();
            var states = projectPlans.GroupBy(a => a.ProjectState);
            foreach (var st in states)
            {
                var plans = st.ToList();
                var projectDetails = new List<ProjectFundPlanDetail>();
                var stepDetails = new List<ProjectFundPlanDetail>();
                foreach (ProjectFundPlanMaster plan in plans)
                {
                    projectDetails.AddRange(plan.Details.OfType<ProjectFundPlanDetail>());
                    if (plan.ProjectType == 2)
                    {
                        stepDetails.AddRange(plan.Details.OfType<ProjectFundPlanDetail>());
                    }
                }

                var detail = new FilialeFundPlanDetail();
                detail.Master = filialeFundPlan;
                detail.ProjectCategory = st.Key;
                detail.CumulativeSettlement = projectDetails.Sum(a => a.CumulativeSettlement);
                detail.CumulativePayment = projectDetails.Sum(a => a.CumulativePayment);
                detail.CumulativeArrears = projectDetails.Sum(a => a.CumulativeArrears);
                detail.CumulativeExpireDue = projectDetails.Sum(a => a.CumulativeExpireDue);
                detail.PresentMonthPlanPayment = projectDetails.Sum(a => a.PlanPayment);
                detail.ThisMonthInstallFilialePayment = stepDetails.Sum(a => a.PlanPayment);
                detail.Descript = string.Join(":", projectDetails.TakeWhile(s => !string.IsNullOrEmpty(s.Descript))
                                                       .Select(a => a.Descript).ToArray());

                filialeFundPlan.Details.Add(detail);
            }
        }

        private void ProjectExpendCellToModel()
        {
            if (projectFundPlan == null || projectFundPlan.Details == null)
            {
                return;
            }

            for (int i = 5; i < gdProjectExpend.Rows; i++)
            {
                var cellKey = gdProjectExpend.Cell(i, 2).Tag;
                if (string.IsNullOrEmpty(cellKey))
                {
                    continue;
                }

                var detail =
                    projectFundPlan.Details.OfType<ProjectFundPlanDetail>().FirstOrDefault(
                        a => a.TempData == cellKey || a.Id == cellKey);
                if (detail == null)
                {
                    continue;
                }

                detail.Descript = gdProjectExpend.Cell(i, 15).Text;
                var cellVal = gdProjectExpend.Cell(i, 13).Text;
                var valTmp = 0m;
                if (decimal.TryParse(cellVal, out valTmp))
                {
                    detail.PlanPayment = valTmp;
                }
            }

            projectFundPlan.PresentMonthPayment =
                projectFundPlan.Details.OfType<ProjectFundPlanDetail>().Sum(a => a.PlanPayment)
                + projectFundPlan.OtherPayDetails.OfType<ProjectOtherPayPlanDetail>().Sum(b => b.PlanDeclarePayment);
        }

        private void ReComputeRowNo(CustomFlexGrid grid, int changeRowIndex)
        {
            var startRowIndex = 4;
            for (var i = changeRowIndex; i > startRowIndex; i--)
            {
                var txt = grid.Cell(i, 2).Text;
                if (!string.IsNullOrEmpty(txt) && txt.Replace(" ", "").Contains(XIAOJI))
                {
                    startRowIndex = i + 1;
                    break;
                }
            }

            var rowNo = 0;
            for (var j = startRowIndex; j < grid.Rows; j++)
            {
                var txt = grid.Cell(j, 2).Text;
                if (!string.IsNullOrEmpty(txt) && txt.Replace(" ", "").Contains(XIAOJI))
                {
                    CommonUtil.SetGridColumnLockState(grid, new List<int>() {1, 2}, false);
                    var range = grid.Range(startRowIndex, 1, j, 1);
                    range.Merge();
                    CommonUtil.SetGridColumnLockState(grid, new List<int>() {1, 2}, true);
                    break;
                }

                rowNo++;
                grid.Cell(j, 2).Text = rowNo.ToString();
            }
        }

        private void ReComputeSubTotal(CustomFlexGrid grid, int changeRowIndex, int changeColIndex)
        {
            var startRowIndex = 4;
            var txt = string.Empty;
            for (var i = changeRowIndex; i > startRowIndex; i--)
            {
                txt = grid.Cell(i, 2).Text;
                if (!string.IsNullOrEmpty(txt) && txt.Replace(" ", "").Contains(XIAOJI))
                {
                    startRowIndex = i + 1;
                    break;
                }
            }

            var subTotal = 0m;
            for (var j = startRowIndex; j < grid.Rows; j++)
            {
                txt = grid.Cell(j, 2).Text;
                if (!string.IsNullOrEmpty(txt) && txt.Replace(" ", "").Contains(XIAOJI))
                {
                    grid.Cell(j, changeColIndex).Text = subTotal.ToString("N2");
                    break;
                }

                decimal tmp;
                txt = grid.Cell(j, changeColIndex).Text;
                if (decimal.TryParse(txt, out tmp))
                {
                    subTotal += tmp;
                }
            }
        }

        private void ReComputeTotal(CustomFlexGrid grid, int subColIndex, int valueColIndex, int totalColIndex)
        {
            var total = 0m;
            for (int i = 0; i < grid.Rows; i++)
            {
                var txt = grid.Cell(i, subColIndex).Text;
                if (!string.IsNullOrEmpty(txt) && txt.Replace(" ", "").Contains(XIAOJI))
                {
                    txt = grid.Cell(i, valueColIndex).Text;
                    decimal tmp;
                    if (decimal.TryParse(txt, out tmp))
                    {
                        total += tmp;
                    }
                }

                txt = grid.Cell(i, totalColIndex).Text;
                if (!string.IsNullOrEmpty(txt) && txt.Replace(" ", "").Contains(HEJI))
                {
                    grid.Cell(i, valueColIndex).Text = total.ToString("N2");
                }
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var isMenuEnable = tabControl.SelectedTab == tpOtherExpend
                               || tabControl.SelectedTab == tPOfficeExpend;
            tspMenuDelete.Enabled =
                tspMenuInsert.Enabled =
                isMenuEnable && curBillMaster != null && curBillMaster.DocState == DocumentState.Edit;

            tspMenuRefresh.Visible = tabControl.SelectedTab == tpProjectReport 
                || tabControl.SelectedTab == tpCompanyReport;
        }

        private void tspMenuInsert_Click(object sender, EventArgs e)
        {
            var grid = FindGrid(tabControl.SelectedTab);
            if (grid == null)
            {
                return;
            }

            var range = grid.Selection;
            if (range == null)
            {
                MessageBox.Show("请选择要插入行位置");
                return;
            }

            var selRowIndex = range.FirstRow;
            if (selRowIndex <= 3)
            {
                MessageBox.Show("当前位置不能插入行");
                return;
            }
            var insRowCount = range.LastRow - selRowIndex + 1;
            grid.InsertRow(selRowIndex, insRowCount);

            ReComputeRowNo(grid, selRowIndex);
        }

        private void tspMenuDelete_Click(object sender, EventArgs e)
        {
            var grid = FindGrid(tabControl.SelectedTab);
            if (grid == null)
            {
                return;
            }

            var range = grid.Selection;
            if (range == null)
            {
                MessageBox.Show("请选择要删除的行");
                return;
            }

            var selRowIndex = range.FirstRow;
            if (selRowIndex <= 3)
            {
                MessageBox.Show("请选择数据行删除");
                return;
            }

            var isNeedSave = false;
            for (var i = selRowIndex; i <= range.LastRow; i++)
            {
                var txt1 = grid.Cell(i, 1).Text;
                var txt2 = grid.Cell(i, 2).Text;
                if (!string.IsNullOrEmpty(txt1) && txt1.Replace(" ", "").Contains(HEJI))
                {
                    MessageBox.Show("所选区域包含合计行，不能删除");
                    return;
                }
                else if (!string.IsNullOrEmpty(txt2) && txt2.Replace(" ", "").Contains(XIAOJI))
                {
                    MessageBox.Show("所选区域包含小计行，不能删除");
                    return;
                }

                var id = grid.Cell(i, 2).Tag;
                if (!string.IsNullOrEmpty(id) && projectFundPlan != null)
                {
                    var detail = projectFundPlan.OtherPayDetails.FirstOrDefault(a => a.Id == id);
                    if (detail != null)
                    {
                        projectFundPlan.OtherPayDetails.Remove(detail);
                        isNeedSave = true;
                    }
                }
            }

            range.DeleteByRow();

            ReComputeRowNo(grid, selRowIndex);

            ReComputeSubTotal(grid, selRowIndex - 1, 4);

            if(isNeedSave)
            {
                SaveOrSubmit(1, false);
            }
        }

        private void tspMenuRefresh_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tpProjectReport)
            {
                ProjectExpendCellToModel();

                FundPlanOperate.DisplayProjectPlanFundFlow(projectFundPlan, gdProjectReport);

                FundPlanOperate.DisplayProjectReportDetail(projectFundPlan, gdProjectReport);
            }
            else if (tabControl.SelectedTab == tpCompanyReport)
            {
                CreateOfficeExpend();

                FundPlanOperate.DisplayFilialePlanReportDetail(filialeFundPlan, gdCompanyReport);
            }
        }

        private void gdOfficeExpend_CellChange(object sender, Grid.CellChangeEventArgs e)
        {
            if (e.Col != 4)
            {
                return;
            }

            var txt = gdOfficeExpend.Cell(e.Row, 2).Text;
            if (!string.IsNullOrEmpty(txt) && txt.Replace(" ", "").Contains(XIAOJI))
            {
                return;
            }

            ReComputeSubTotal(gdOfficeExpend, e.Row, e.Col);

            ReComputeTotal(gdOfficeExpend, 2, e.Col, 1);
        }

        private void gdOtherExpend_CellChange(object sender, Grid.CellChangeEventArgs e)
        {
            if (e.Col != 4)
            {
                return;
            }

            var txt = gdOtherExpend.Cell(e.Row, 2).Text;
            if (!string.IsNullOrEmpty(txt) && txt.Replace(" ", "").Contains(XIAOJI))
            {
                return;
            }

            ReComputeSubTotal(gdOtherExpend, e.Row, e.Col);

            ReComputeTotal(gdOtherExpend, 2, e.Col, 1);
        }

        private void gdProjectExpend_CellChange(object sender, Grid.CellChangeEventArgs e)
        {
            if (e.Col != 13)
            {
                return;
            }

            var txt = gdProjectExpend.Cell(e.Row, 2).Text;
            if (!string.IsNullOrEmpty(txt) && txt.Replace(" ", "").Contains(XIAOJI))
            {
                return;
            }

            var valTmp = 0m;
            txt = gdProjectExpend.Cell(e.Row, e.Col).Text;
            decimal.TryParse(txt, out valTmp);

            var cellKey = gdProjectExpend.Cell(e.Row, 2).Tag;
            if (string.IsNullOrEmpty(cellKey))
            {
                return;
            }

            var detailItem =
                projectFundPlan.Details.OfType<ProjectFundPlanDetail>().FirstOrDefault(
                    a => a.Id == cellKey || a.TempData == cellKey);
            if (detailItem == null)
            {
                return;
            }
            if (detailItem.CumulativeSettlement != 0)
            {
                detailItem.PlanPaymentRatio =
                    Math.Round((detailItem.CumulativePayment + valTmp)/detailItem.CumulativeSettlement, 4);
            }
            gdProjectExpend.Cell(e.Row, 14).Text = detailItem.PlanPaymentRatio.ToString("P2");

            ReComputeSubTotal(gdProjectExpend, e.Row, e.Col);

            ReComputeTotal(gdProjectExpend, 2, e.Col, 1);
        }

        private void gdProjectReport_CellChange(object sender, Grid.CellChangeEventArgs e)
        {
            if (e.Row != 6 || (e.Col != 1 && e.Col != 9))
            {
                return;
            }

            decimal valTmp = 0;
            if (!decimal.TryParse(gdProjectReport.Cell(e.Row, e.Col).Text, out valTmp))
            {
                return;
            }

            if (e.Col == 1 && valTmp != 0)
            {
                projectFundPlan.FinanceConfirmTaxIncome = valTmp;
                projectFundPlan.ActualGatheringRatio = Math.Round(projectFundPlan.CumulativeGathering/valTmp, 4);
                gdProjectReport.Cell(e.Row, 8).Text = projectFundPlan.ActualGatheringRatio.ToString("P2");
            }
            else if (e.Col == 9)
            {
                projectFundPlan.PresentMonthGathering = valTmp;
                projectFundPlan.MonthEndCumulativeFundStock = projectFundPlan.FundStock +
                                                              projectFundPlan.PresentMonthGathering -
                                                              projectFundPlan.PresentMonthPayment;

                gdProjectReport.Cell(e.Row, 11).Text = projectFundPlan.MonthEndCumulativeFundStock.ToString("N2");
            }
        }

        private void gdCompanyReport_CellChange(object sender, Grid.CellChangeEventArgs e)
        {
            var txt = gdCompanyReport.Cell(e.Row, e.Col).Text.Trim();
            var tmp = 0m;
            decimal.TryParse(txt, out tmp);
            if (e.Row == 6 && e.Col == 1)
            {
                if (tmp != 0)
                {
                    txt = gdCompanyReport.Cell(e.Row, e.Col + 1).Text.Trim();
                    var tmp2 = 0m;
                    decimal.TryParse(txt, out tmp2);
                    filialeFundPlan.CumulativeCurrentYearCashRatio = tmp2/tmp;
                    gdCompanyReport.Cell(e.Row, e.Col + 2).Text = filialeFundPlan.CumulativeCurrentYearCashRatio.ToString("P2");
                }

                filialeFundPlan.FinanceConfirmTaxIncome = tmp;
            }
            else if (e.Row == 10)
            {
                if (e.Col == 1)
                {
                    filialeFundPlan.TillLastMonthFundStock = tmp;
                }
                else if (e.Col == 2)
                {
                    filialeFundPlan.ThereinLoan = tmp;
                }
                else if (e.Col == 3)
                {
                    filialeFundPlan.ThereinSuperviseAccountFund = tmp;
                }
                else if (e.Col == 4)
                {
                    filialeFundPlan.ThereinBankAccept = tmp;
                }

                filialeFundPlan.PresentMonthSpendableFund = filialeFundPlan.TillLastMonthFundStock
                                                            + filialeFundPlan.PresentMonthGathering
                                                            - filialeFundPlan.ThereinSuperviseAccountFund;

                gdCompanyReport.Cell(e.Row, 6).Text = filialeFundPlan.PresentMonthSpendableFund.ToString("N2");
            }
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在获取资金计划明细数据...");

            if (currentProject != null)
            {
                NewProjectPlan();

                FundPlanOperate.DisplayProjectPlanFundFlow(projectFundPlan, gdProjectReport);
            }
            else
            {
                NewFilialePlan();

                FundPlanOperate.DisplayFilialePlanMaster(filialeFundPlan, gdCompanyReport,
                                                         string.Concat(tpCompanyReport.Tag, ".flx"));
            }

            if (currentProject != null)
            {
                CreateProjectFundPlanDetail();

                FundPlanOperate.DispalyProjectPlanDetail(projectFundPlan, gdProjectExpend,
                                                         string.Concat(tpProjectExpend.Tag, ".flx"));

                FundPlanOperate.DisplayProjectPlanFundFlow(projectFundPlan, gdProjectReport);

                FundPlanOperate.DisplayProjectReportDetail(projectFundPlan, gdProjectReport);

                tabControl.SelectedTab = tpProjectExpend;
            }
            else
            {
                var list = mOperate.FinanceMultDataSrv.GetFilialeFundPlanProjectDetail(filialeFundPlan);
                if (list != null)
                {
                    foreach (ProjectFundPlanMaster plan in list)
                    {
                        var project = mOperate.CurrentProjectSrv.GetProjectById(plan.ProjectId);
                        if (project != null)
                        {
                            plan.ProjectState = ((EnumProjectCurrState)project.ProjectCurrState).ToString();
                            plan.ProjectType = project.ProjectType;
                        }
                    }

                    CreateFilialePlanDetail(list.OfType<ProjectFundPlanMaster>().ToList());
                }

                FundPlanOperate.DisplayFilialePlanMaster(filialeFundPlan, gdCompanyReport,
                                                         string.Concat(tpCompanyReport.Tag, ".flx"));

                FundPlanOperate.DisplayFilialePlanReportDetail(filialeFundPlan, gdCompanyReport);

                FundPlanOperate.DisplayFilialePlanProjectDetailsByName(list, gdTotalExpend1,
                                                                       string.Concat(tpTotalExpend1.Tag, ".flx"));

                FundPlanOperate.DisplayFilialePlanProjectDetailsByState(list, gdTotalExpend2,
                                                                       string.Concat(tpTotalExpend2.Tag, ".flx"));

                tabControl.SelectedTab = tpTotalExpend1;
            }

            FlashScreen.Close();
        }

    }
}
