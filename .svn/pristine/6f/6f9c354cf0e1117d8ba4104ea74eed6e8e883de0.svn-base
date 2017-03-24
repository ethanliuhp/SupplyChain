using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using VirtualMachine.Component.WinControls.Controls;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Util;
using VirtualMachine.Core;
using NHibernate.Criterion;
using System.Collections;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.WinMVC.generic;
using FlexCell;
using Application.Business.Erp.SupplyChain.Base.Domain;
using C1.Win.C1FlexGrid;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public partial class VPlanDeclareAllot : TBasicDataView
    {
        private CurrentProjectInfo projectInfo = null;
        private List<OperationOrgInfo> companyOrg;
        private MFinanceMultData mOperate = new MFinanceMultData();
        private CurrentProjectInfo currentProject;
        private OperationOrgInfo ownOrg;
        private int loginOrgtype = 0;

        private string allotCellName = "分配额";
        private int gdOfficeExpendAllotInsertStartIndex = 5;//插入"分配额"的列号
        private int gdCompanyReportAllotInsertStartIndex = 9;
        private int gdOtherExpendAllotInsertStartIndex = 5;
        private int gdProjectExpendAllotInsertStartIndex = 15;
        private int gdTotalExpend2AllotInsertStartIndex = 11;

        private int gdOtherExpendSelectedRowNum = 0;//保存时所选当前数据的行号
        private int gdProjectExpendSelectedRowNum = 0;
        private int gdOfficeExpendSelectedRowNum = 0;
        private int gdCompanyReportSelectedRowNum = 0;
        private int gdTotalExpend2SelectedRowNum = 0;

        private int saveUpdateOtherPayPlanDetailsSucceed = 0;//保存成功，值为1；输入分配值过大，保存不成功，值为-1；表中没有记录，值为0
        private int saveUpdateProjectPlanDetailsSucceed = 0;
        private int saveUpdateOfficePlanDetailsSucceed = 0;
        private int saveUpdateTotalExpend2Succeed = 0;

        private decimal filialeFundPlanDeclareAllot = 0;//分公司资金支付计划申报表的"分配额"，要从两张flx表中获取数据
        private decimal filialeOfficeTotalQuota = 0;//分公司机关资金支付计划申报表的"分配额"
        private decimal filialeTotal2Quota = 0;//分公司汇总表2"分配额"
        private decimal filialeTotalPlanDeclarePayment = 0;//分公司计划申报支出

        public VPlanDeclareAllot()
        {
            InitializeComponent();

            InitData();
            InitEvent();

            SetTabPageVisible();

            this.dgMaster.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        public void InitEvent()
        {
            gdOtherExpend.EditRow += new FlexCell.Grid.EditRowEventHandler(gdOtherExpend_EditRow);
            gdProjectExpend.EditRow += new FlexCell.Grid.EditRowEventHandler(gdProjectExpend_EditRow);
            gdOfficeExpend.EditRow += new FlexCell.Grid.EditRowEventHandler(gdOfficeExpend_EditRow);
            gdCompanyReport.EditRow += new FlexCell.Grid.EditRowEventHandler(gdCompanyReport_EditRow);
            gdTotalExpend2.EditRow += new FlexCell.Grid.EditRowEventHandler(gdTotalExpend2_EditRow);
            dgMaster.CellDoubleClick += new DataGridViewCellEventHandler(dgMaster_CellDoubleClick);
        }

        public void InitData()
        {
            this.Text = "资金计划分配";
            this.dtpBeginCreateDate.Value = ConstObject.TheLogin.LoginDate.AddDays(-7);
            this.dtpEndCreateDate.Value = ConstObject.TheLogin.LoginDate;

            projectInfo = StaticMethod.GetProjectInfo();

            InitFlex();

            GetCompanyOrg();
        }

        private void InitFlex()
        {
            foreach (TabPage tPage in tabControl1.TabPages)
            {
                if (tPage.Tag == null)
                {
                    continue;
                }

                var fName = string.Concat(tPage.Tag, ".flx");
                var grid = FindGrid(tPage);
                if (grid != null)
                {
                    LoadFlexFile(fName, grid);
                }
            }
            addAllAllotColumn();
        }

        /// <summary>
        /// 给各表增加"分配额"列
        /// </summary>
        private void addAllAllotColumn()
        {
            //机关资金计划支出明细表增加"分配额"列
            gdOfficeExpend.Locked = false;
            gdOfficeExpend.InsertCol(gdOfficeExpendAllotInsertStartIndex, 1);
            gdOfficeExpend.Cell(3, gdOfficeExpendAllotInsertStartIndex).Text = allotCellName;

            //分公司资金支付计划申报表增加"分配额"列
            gdCompanyReport.InsertCol(gdCompanyReportAllotInsertStartIndex, 1);
            gdCompanyReport.Range(12, gdCompanyReportAllotInsertStartIndex, 13, gdCompanyReportAllotInsertStartIndex).Merge();
            gdCompanyReport.Cell(12, gdCompanyReportAllotInsertStartIndex).Text = allotCellName;

            //项目资金计划其他支出明细表增加"分配额"列
            gdOtherExpend.InsertCol(gdOtherExpendAllotInsertStartIndex, 1);
            gdOtherExpend.Cell(3, gdOtherExpendAllotInsertStartIndex).Text = allotCellName;
            gdOtherExpend.Range(2, gdOtherExpendAllotInsertStartIndex - 1, 2, gdOtherExpendAllotInsertStartIndex + 1).Merge();
            gdOtherExpend.Cell(2, gdOtherExpendAllotInsertStartIndex - 1).Text = "单位：元";
            gdOtherExpend.Cell(2, gdOtherExpendAllotInsertStartIndex + 1).Text = "";
            CommonUtil.SetGridRangeLockState(gdOtherExpend.Range(gdOtherExpendAllotInsertStartIndex - 1, 1, gdOtherExpend.Rows - 2, gdOtherExpendAllotInsertStartIndex - 1), true);
            CommonUtil.SetGridRangeLockState(gdOtherExpend.Range(4, gdOtherExpendAllotInsertStartIndex + 1, gdOtherExpend.Rows - 2, gdOtherExpendAllotInsertStartIndex + 1), true);

            //项目资金支付计划申报明细表增加"分配额"列
            gdProjectExpend.InsertCol(gdProjectExpendAllotInsertStartIndex, 1);
            gdProjectExpend.Range(3, gdProjectExpendAllotInsertStartIndex, 4, gdProjectExpendAllotInsertStartIndex).Merge();
            gdProjectExpend.Cell(3, gdProjectExpendAllotInsertStartIndex).Text = allotCellName;
            gdProjectExpend.Range(2, gdProjectExpendAllotInsertStartIndex - 1, 2, gdProjectExpendAllotInsertStartIndex + 1).Merge();
            gdProjectExpend.Cell(2, gdProjectExpendAllotInsertStartIndex - 1).Text = "单位：元";
            gdProjectExpend.Cell(2, gdProjectExpendAllotInsertStartIndex + 1).Text = "";
            CommonUtil.SetGridRangeLockState(gdProjectExpend.Range(5, 1, gdProjectExpend.Rows - 1, gdProjectExpendAllotInsertStartIndex - 1), true);
            CommonUtil.SetGridRangeLockState(gdProjectExpend.Range(5, gdProjectExpendAllotInsertStartIndex + 1, gdProjectExpend.Rows - 1, gdProjectExpendAllotInsertStartIndex + 1), true);

            //资金计划申报汇总表2增加"分配额"列
            gdTotalExpend2.InsertCol(gdTotalExpend2AllotInsertStartIndex, 1);
            gdTotalExpend2.Range(3, gdTotalExpend2AllotInsertStartIndex, 4, gdTotalExpend2AllotInsertStartIndex).Merge();
            gdTotalExpend2.Cell(3, gdTotalExpend2AllotInsertStartIndex).Text = allotCellName;
        }

        private void GetCompanyOrg()
        {
            var list = mOperate.FinanceMultDataSrv.GetSubCompanySyscodeList();
            if (list == null || list.Count == 0)
            {
                return;
            }

            ObjectQuery objQuery = new ObjectQuery();
            Disjunction dis = new Disjunction();
            foreach (var sysCode in list)
            {
                dis.Add(Expression.Eq("SysCode", sysCode));
            }
            objQuery.AddCriterion(dis);

            companyOrg = mOperate.FinanceMultDataSrv.Query(typeof(OperationOrgInfo), objQuery)
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
            if (ownOrg == null)
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
            if (currentProject != null)//默认,公司直管登录，显示项目资金计划其他支出明细表,资金计划申报明细表,项目资金支付计划申报表三张表
            {
                tpCompanyReport.Parent = null;
                tpOtherExpend.Parent = tabControl1;
                tpProjectExpend.Parent = tabControl1;
                tpProjectReport.Parent = tabControl1;
                tpTotalExpend1.Parent = null;
                tpTotalExpend2.Parent = null;
                tPOfficeExpend.Parent = null;
            }
            else if (loginOrgtype == 1)//总部,admin登录,机关资金计划支出明细表
            {
                tpCompanyReport.Parent = null;
                tpOtherExpend.Parent = null;
                tpProjectExpend.Parent = null;
                tpProjectReport.Parent = null;
                tpTotalExpend1.Parent = null;
                tpTotalExpend2.Parent = null;
                tPOfficeExpend.Parent = tabControl1;
            }
            else if (loginOrgtype == 2)//公司直管
            {
                tpCompanyReport.Parent = null;
                tpOtherExpend.Parent = tabControl1;
                tpProjectExpend.Parent = tabControl1;
                tpProjectReport.Parent = tabControl1;
                tpTotalExpend1.Parent = null;
                tpTotalExpend2.Parent = null;
                tPOfficeExpend.Parent = null;
            }
            else if (loginOrgtype == 3)//分公司相关用户登录,分公司资金支付计划申报表,资金计划申报汇总表1,资金计划申报汇总表2,机关资金计划支出明细表四张表
            {
                tpCompanyReport.Parent = tabControl1;
                tpOtherExpend.Parent = null;
                tpProjectExpend.Parent = null;
                tpProjectReport.Parent = null;
                tpTotalExpend1.Parent = tabControl1;
                tpTotalExpend2.Parent = tabControl1;
                tPOfficeExpend.Parent = tabControl1;
            }
        }

        private void LoadFlexFile(string flxname, CustomFlexGrid grid)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(flxname))
            {
                eFile.CreateTempleteFileFromServer(flxname);

                grid.OpenFile(path + "\\" + flxname); //载入格式
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            FlashScreen.Show("正在查询信息......");
            ObjectQuery objectQuery = new ObjectQuery();

            if (txtCode.Text != "")
            {
                objectQuery.AddCriterion(Expression.Like("Code", txtCode.Text, MatchMode.Anywhere));
            }

            //业务时间
            objectQuery.AddCriterion(Expression.Ge("CreateDate", this.dtpBeginCreateDate.Value.Date));
            objectQuery.AddCriterion(Expression.Lt("CreateDate", this.dtpEndCreateDate.Value.AddDays(1).Date));

            //只显示当前登录用户的单据
            objectQuery.AddCriterion(Expression.Eq("CreatePersonName", ConstObject.TheLogin.ThePerson.Name));

            //只显示“已审核”的单据
            objectQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));

            if (loginOrgtype == 1 || loginOrgtype == 3)
            {
                var list = mOperate.FinanceMultDataSrv.GetFilialeFundPlanMasterByOQ(objectQuery);
                fillFlexCellByMasterData(loginOrgtype, list);
            }
            else if (loginOrgtype == 2)
            {
                var list = mOperate.FinanceMultDataSrv.GetProjectFundPlanMasterByOQ(objectQuery);
                fillFlexCellByMasterData(loginOrgtype, list);
            }
        }

        private void fillFlexCellByMasterData(int type, IList list)
        {
            try
            {
                dgMaster.Rows.Clear();
                if (type == 1 || type == 3)
                {
                    foreach (FilialeFundPlanMaster master in list)
                    {
                        int rowIndex = dgMaster.Rows.Add();
                        dgMaster.Rows[rowIndex].Tag = master;
                        dgMaster[colCode.Name, rowIndex].Value = master.Code;
                        dgMaster[colPresentMonthPayment.Name, rowIndex].Value = master.PresentMonthPlanPayment.ToString();
                        dgMaster[colApprovalAmount.Name, rowIndex].Value = master.Approval.ToString();
                        dgMaster[colDeclareDate.Name, rowIndex].Value = master.DeclareDate.ToShortDateString();
                        dgMaster[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                        dgMaster[colCreateDate.Name, rowIndex].Value = master.DeclareDate.ToShortDateString();
                        dgMaster[colCreatePersonName.Name, rowIndex].Value = master.CreatePersonName;
                    }
                }
                else if (type == 2)
                {
                    foreach (ProjectFundPlanMaster master in list)
                    {
                        int rowIndex = dgMaster.Rows.Add();
                        dgMaster.Rows[rowIndex].Tag = master;
                        dgMaster[colCode.Name, rowIndex].Value = master.Code;
                        dgMaster[colPresentMonthPayment.Name, rowIndex].Value = master.PresentMonthPayment.ToString();
                        dgMaster[colApprovalAmount.Name, rowIndex].Value = master.ApprovalAmount.ToString();
                        dgMaster[colDeclareDate.Name, rowIndex].Value = master.DeclareDate.ToShortDateString();
                        dgMaster[colState.Name, rowIndex].Value = ClientUtil.GetDocStateName(master.DocState);
                        dgMaster[colCreateDate.Name, rowIndex].Value = master.DeclareDate.ToShortDateString();
                        dgMaster[colCreatePersonName.Name, rowIndex].Value = master.CreatePersonName;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询数据出错。\n" + ex.Message);
            }
            finally
            {
                FlashScreen.Close();
            }
        }

        private void dgMaster_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (loginOrgtype == 1)//总部,显示机关资金计划支出明细
            {
                FilialeFundPlanMaster master = dgMaster.CurrentRow.Tag as FilialeFundPlanMaster;
                if (master == null) return;
                foreach (TabPage page in tabControl1.TabPages)
                {
                    if (page.Text == "机关资金计划支出明细表")
                    {
                        FundPlanOperate.DisplayOfficePlanDetail(master, gdOfficeExpend, "机关资金计划支出明细表.flx");
                    }
                }
            }
            else if (loginOrgtype == 2)//公司直管
            {
                ProjectFundPlanMaster master = dgMaster.CurrentRow.Tag as ProjectFundPlanMaster;
                if (master == null) return;
                foreach (TabPage page in tabControl1.TabPages)
                {
                    if (page.Text == "项目资金计划其他支出明细表")
                    {
                        DisplayThisProjectOtherPlanDetail(master);
                    }
                    else if (page.Text == "资金计划申报明细表")
                    {
                        DispalyThisProjectPlanDetail(master);
                    }
                    else if (page.Text == "项目资金支付计划申报表")
                    {
                        DisplayThisProjectReportDetail(master);
                    }
                }
            }
            else if (loginOrgtype == 3)//分公司
            {
                FilialeFundPlanMaster master = dgMaster.CurrentRow.Tag as FilialeFundPlanMaster;
                if (master == null) return;
                foreach (TabPage page in tabControl1.TabPages)
                {
                    if (page.Text == "机关资金计划支出明细表")
                    {
                        DisplayThisOfficePlanDetail(master);
                    }
                    else if (page.Text == "资金计划申报汇总表1")
                    {
                        var list = mOperate.FinanceMultDataSrv.GetFilialeFundPlanProjectDetail(master);
                        FundPlanOperate.DisplayFilialePlanProjectDetailsByName(list, gdTotalExpend1, "单位资金支付计划申报汇总表.flx");
                    }
                    else if (page.Text == "资金计划申报汇总表2")
                    {
                        DisplayThisTotalExpend2(master);
                    }
                    else if (page.Text == "分公司资金支付计划申报表")
                    {
                        DisplayThisFilialePlanMaster(master);
                    }
                }
            }
        }
        
        private void DisplayThisProjectOtherPlanDetail(ProjectFundPlanMaster master)
        {
            FundPlanOperate.DisplayProjectOtherPlanDetail(master, gdOtherExpend, "项目资金支付计划申报其他支出明细表.flx");
            gdOtherExpend.InsertCol(gdOtherExpendAllotInsertStartIndex, 1);
            gdOtherExpend.Cell(3, gdOtherExpendAllotInsertStartIndex).Text = allotCellName;
            gdOtherExpend.Range(2, gdOtherExpendAllotInsertStartIndex - 1, 2, gdOtherExpendAllotInsertStartIndex + 1).Merge();
            gdOtherExpend.Cell(2, gdOtherExpendAllotInsertStartIndex - 1).Text = "单位：元";
            gdOtherExpend.Cell(2, gdOtherExpendAllotInsertStartIndex + 1).Text = "";
            CommonUtil.SetGridRangeLockState(gdOtherExpend.Range(gdOtherExpendAllotInsertStartIndex - 1, 1, gdOtherExpend.Rows - 2, gdOtherExpendAllotInsertStartIndex - 1), true);
            CommonUtil.SetGridRangeLockState(gdOtherExpend.Range(4, gdOtherExpendAllotInsertStartIndex + 1, gdOtherExpend.Rows - 2, gdOtherExpendAllotInsertStartIndex + 1), true);

            List<ProjectOtherPayPlanDetail> otherDetails = master.OtherPayDetails.OfType<ProjectOtherPayPlanDetail>().ToList();
            List<FundPlanFlexGridOperate> otherPlanDetailOperate = FundPlanOperate.OtherPlanDetailOperate;
            for (int rowIndex = 4; rowIndex < gdOtherExpend.Rows; rowIndex++)
            {
                if (otherPlanDetailOperate.Find(a => a.CurrentRowNumber == rowIndex) != null)
                {
                    FundPlanFlexGridOperate operate = otherPlanDetailOperate.Find(a => a.CurrentRowNumber == rowIndex);
                    if (operate.DataRow)
                    {
                        ProjectOtherPayPlanDetail oneDetail = otherDetails.Find(a => a.Id == operate.Id);
                        if (oneDetail.Quota != 0)
                        {
                            gdOtherExpend.Cell(rowIndex, 5).Text = oneDetail.Quota.ToString("N2");
                        }
                    }
                    else
                    {
                        gdOtherExpend.Cell(rowIndex, 5).Text = operate.SubtotalQuota.ToString("N2");
                        gdOtherExpend.Cell(rowIndex, 5).FontBold = true;
                    }
                }
            }
        }

        private void DispalyThisProjectPlanDetail(ProjectFundPlanMaster master)
        {
            FundPlanOperate.DispalyProjectPlanDetail(master, gdProjectExpend, "项目资金支付计划申报明细表.flx");
            gdProjectExpend.InsertCol(gdProjectExpendAllotInsertStartIndex, 1);
            gdProjectExpend.Range(3, gdProjectExpendAllotInsertStartIndex, 4, gdProjectExpendAllotInsertStartIndex).Merge();
            gdProjectExpend.Cell(3, gdProjectExpendAllotInsertStartIndex).Text = allotCellName;
            gdProjectExpend.Range(2, gdProjectExpendAllotInsertStartIndex - 1, 2, gdProjectExpendAllotInsertStartIndex + 1).Merge();
            gdProjectExpend.Cell(2, gdProjectExpendAllotInsertStartIndex - 1).Text = "单位：元";
            gdProjectExpend.Cell(2, gdProjectExpendAllotInsertStartIndex + 1).Text = "";
            CommonUtil.SetGridRangeLockState(gdProjectExpend.Range(5, 1, gdProjectExpend.Rows - 1, gdProjectExpendAllotInsertStartIndex - 1), true);
            CommonUtil.SetGridRangeLockState(gdProjectExpend.Range(5, gdProjectExpendAllotInsertStartIndex + 1, gdProjectExpend.Rows - 1, gdProjectExpendAllotInsertStartIndex + 1), true);

            List<ProjectFundPlanDetail> details = master.Details.OfType<ProjectFundPlanDetail>().ToList();
            List<FundPlanFlexGridOperate> projectPlanDetailOperate = FundPlanOperate.ProjectPlanDetailOperate;
            for (int rowIndex = 5; rowIndex < gdProjectExpend.Rows; rowIndex++)
            {
                if (projectPlanDetailOperate.Find(a => a.CurrentRowNumber == rowIndex) != null)
                {
                    FundPlanFlexGridOperate operate = projectPlanDetailOperate.Find(a => a.CurrentRowNumber == rowIndex);
                    if (operate.DataRow)
                    {
                        ProjectFundPlanDetail oneDetail = details.Find(a => a.Id == operate.Id);
                        if (oneDetail.Quota != 0)
                        {
                            gdProjectExpend.Cell(rowIndex, 15).Text = oneDetail.Quota.ToString("N2");
                        }
                    }
                    else
                    {
                        gdProjectExpend.Cell(rowIndex, 15).FontBold = true;
                        gdProjectExpend.Cell(rowIndex, 15).Text = operate.SubtotalQuota.ToString("N2");
                    }
                }
            }
        }

        private void DisplayThisProjectReportDetail(ProjectFundPlanMaster master)
        {
            FundPlanOperate.DisplayProjectReportDetail(master, gdProjectReport, "项目资金支付计划申报分配表.flx");

            List<FundPlanFlexGridOperate> otherPlanCostTypeOperate = FundPlanOperate.OtherPlanCostTypeOperate;
            List<FundPlanFlexGridOperate> projectPlanCostTypeOperate = FundPlanOperate.ProjectPlanCostTypeOperate;
            int rowIndex = 10;
            decimal totalQuota = 0;
            if (projectPlanCostTypeOperate.Count > 0)
            {
                foreach (FundPlanFlexGridOperate operate in projectPlanCostTypeOperate)
                {
                    if (!string.IsNullOrEmpty(gdProjectReport.Cell(rowIndex, 1).Text))
                    {
                        string costTypeString = gdProjectReport.Cell(rowIndex, 1).Text;
                        if (projectPlanCostTypeOperate.Find(a => a.CostType == FundPlanOperate.getCostType(costTypeString)) != null)
                        {
                            FundPlanFlexGridOperate currentOpearte = projectPlanCostTypeOperate.Find(a => a.CostType == FundPlanOperate.getCostType(costTypeString));
                            gdProjectReport.Cell(rowIndex, 10).Text = operate.SubtotalQuota.ToString("N2");
                            totalQuota += operate.SubtotalQuota;
                        }
                    }
                    rowIndex++;
                }
            }
            if (otherPlanCostTypeOperate.Count > 0)
            {
                FundPlanFlexGridOperate operate = otherPlanCostTypeOperate.ElementAt(0);
                gdProjectReport.Cell(rowIndex, 10).Text = operate.SubtotalQuota.ToString("N2");
                totalQuota += operate.SubtotalQuota;
                rowIndex++;
            }
            gdProjectReport.Cell(rowIndex, 10).Text = totalQuota.ToString("N2");
            int rowStartIndex = 10;
            CommonUtil.SetGridRangeLockState(gdProjectReport.Range(rowStartIndex, 10, gdProjectReport.Rows - 1, 10), true);
        }

        private void DisplayThisOfficePlanDetail(FilialeFundPlanMaster master)
        {
            FundPlanOperate.DisplayOfficePlanDetail(master, gdOfficeExpend, "机关资金计划支出明细表.flx");

            gdOfficeExpend.InsertCol(gdOfficeExpendAllotInsertStartIndex, 1);
            gdOfficeExpend.Cell(3, gdOfficeExpendAllotInsertStartIndex).Text = allotCellName;
            CommonUtil.SetGridRangeLockState(gdOfficeExpend.Range(4, 4, gdOfficeExpend.Rows - 1, 4), true);
            CommonUtil.SetGridRangeLockState(gdOfficeExpend.Range(4, 6, gdOfficeExpend.Rows - 1, 6), true);

            List<OfficeFundPlanPayDetail> officeDetails = master.OfficeFundPlanDetails.
                OfType<OfficeFundPlanPayDetail>().ToList();
            List<FundPlanFlexGridOperate> officePlanDetailOperate = FundPlanOperate.OfficePlanDetailOperate;
            filialeOfficeTotalQuota = 0;
            filialeFundPlanDeclareAllot = 0;
            for (int rowIndex = 4; rowIndex < gdOfficeExpend.Rows; rowIndex++)
            {
                if (officePlanDetailOperate.Find(a => a.CurrentRowNumber == rowIndex) != null)
                {
                    FundPlanFlexGridOperate operate = officePlanDetailOperate.Find(a => a.CurrentRowNumber == rowIndex);
                    if (operate.DataRow)
                    {
                        OfficeFundPlanPayDetail oneDetail = officeDetails.Find(a => a.Id == operate.Id);
                        if (oneDetail.Quota != 0)
                        {
                            gdOfficeExpend.Cell(rowIndex, 5).Text = oneDetail.Quota.ToString("N2");
                        }
                    }
                    else
                    {
                        gdOfficeExpend.Cell(rowIndex, 5).Text = operate.SubtotalQuota.ToString("N2");
                        gdOfficeExpend.Cell(rowIndex, 5).FontBold = true;
                        if (gdOfficeExpend.Cell(rowIndex, 1).Text == "合计")
                        {
                            filialeOfficeTotalQuota += operate.SubtotalQuota;
                        }
                    }
                }
            }
            FundPlanFlexGridOperate declarePaymentOperate = officePlanDetailOperate.Find(a => a.MoneyType == "计划支出");
            filialeTotalPlanDeclarePayment = declarePaymentOperate.AllTypeTotalSumMoeny;
            filialeFundPlanDeclareAllot += filialeOfficeTotalQuota;
        }

        private void DisplayThisTotalExpend2(FilialeFundPlanMaster master)
        {
            gdTotalExpend2.Locked = false;
            List<ProjectFundPlanMaster> list = (List<ProjectFundPlanMaster>)
                mOperate.FinanceMultDataSrv.GetFilialeFundPlanProjectDetail(master);
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
            FundPlanOperate.DisplayFilialePlanProjectDetailsByState(list, gdTotalExpend2, "月资金计划项目申报汇总表.flx");

            //资金计划申报汇总表2增加"分配额"列
            gdTotalExpend2.InsertCol(gdTotalExpend2AllotInsertStartIndex, 1);
            gdTotalExpend2.Range(3, gdTotalExpend2AllotInsertStartIndex, 4, gdTotalExpend2AllotInsertStartIndex).Merge();
            gdTotalExpend2.Cell(3, gdTotalExpend2AllotInsertStartIndex).Text = allotCellName;

            CommonUtil.SetGridRangeLockState(gdTotalExpend2.Range(5, 1, gdTotalExpend2.Rows - 1, 10), true);
            CommonUtil.SetGridRangeLockState(gdTotalExpend2.Range(5, 12, gdTotalExpend2.Rows - 1, gdTotalExpend2.Cols - 1), true);

            List<FundPlanFlexGridOperate> operates = FundPlanOperate.TotalExpend2Operate;
            decimal subtotalQuota = 0;//小计
            filialeTotal2Quota = 0;//总计
            for (int rowIndex = 5; rowIndex < gdTotalExpend2.Rows; rowIndex++)
            {
                if (operates.Find(a => a.CurrentRowNumber == rowIndex) != null)
                {
                    FundPlanFlexGridOperate operate = operates.Find(a => a.CurrentRowNumber == rowIndex);
                    ProjectFundPlanMaster oneMaster = list.Find(a => a.Id == operate.Id);
                    gdTotalExpend2.Cell(rowIndex, gdTotalExpend2AllotInsertStartIndex).Text = oneMaster.ApprovalAmount.ToString();
                    subtotalQuota += oneMaster.ApprovalAmount;
                }
                if (gdTotalExpend2.Cell(rowIndex, 2).Text == "小计")
                {
                    gdTotalExpend2.Cell(rowIndex, gdTotalExpend2AllotInsertStartIndex).Text = subtotalQuota.ToString("N2");
                    gdTotalExpend2.Cell(rowIndex, gdTotalExpend2AllotInsertStartIndex).FontBold = true;
                    filialeTotal2Quota += subtotalQuota;
                    subtotalQuota = 0;
                }
                if (gdTotalExpend2.Cell(rowIndex, 1).Text == "合计")
                {
                    gdTotalExpend2.Cell(rowIndex, gdTotalExpend2AllotInsertStartIndex).Text = filialeTotal2Quota.ToString("N2");
                    gdTotalExpend2.Cell(rowIndex, gdTotalExpend2AllotInsertStartIndex).FontBold = true;
                }
            }
            filialeFundPlanDeclareAllot += filialeTotal2Quota;
        }

        private void DisplayThisFilialePlanMaster(FilialeFundPlanMaster master)
        {
            FundPlanOperate.DisplayFilialePlanMaster(master, gdCompanyReport, "分公司资金支付计划申报表.flx");
            gdCompanyReport.InsertCol(gdCompanyReportAllotInsertStartIndex, 1);
            gdCompanyReport.Range(12, gdCompanyReportAllotInsertStartIndex, 13, gdCompanyReportAllotInsertStartIndex).Merge();
            gdCompanyReport.Cell(12, gdCompanyReportAllotInsertStartIndex).Text = allotCellName;

            List<FilialeFundPlanDetail> filialeDetails = master.Details.OfType<FilialeFundPlanDetail>().ToList();
            List<FundPlanFlexGridOperate> filialePlanDetailOperate = FundPlanOperate.FilialePlanDetailOperate;
            int currentRowIndex = 0;
            int rowIndex = 14;
            for (rowIndex = 14; rowIndex < gdCompanyReport.Rows; rowIndex++)
            {
                if (filialePlanDetailOperate.Find(a => a.CurrentRowNumber == rowIndex) != null)
                {
                    FundPlanFlexGridOperate operate = filialePlanDetailOperate.Find(a => a.CurrentRowNumber == rowIndex);
                    if (operate.DataRow)
                    {
                        FilialeFundPlanDetail oneDetail = filialeDetails.Find(a => a.Id == operate.Id);
                        gdCompanyReport.Cell(rowIndex, 9).Text = filialeTotal2Quota.ToString("N2");
                    }
                    else
                    {
                        
                    }
                }
                else
                {
                    gdCompanyReport.Cell(rowIndex, 9).Text = filialeTotal2Quota.ToString("N2");
                }
                currentRowIndex = rowIndex;
            }
            gdCompanyReport.AddItem("", true);
            currentRowIndex++;
            //添加机关资金计划分配额
            gdCompanyReport.Cell(currentRowIndex, 1).Text = "分公司机关";
            gdCompanyReport.Cell(currentRowIndex, 6).Text = filialeTotalPlanDeclarePayment.ToString();
            gdCompanyReport.Cell(currentRowIndex, 9).Text = filialeOfficeTotalQuota.ToString();
            gdCompanyReport.AddItem("", true);
            currentRowIndex++;
            gdCompanyReport.Cell(currentRowIndex, 1).Text = "合计";
            decimal totalQuota = filialeOfficeTotalQuota + filialeTotal2Quota;
            gdCompanyReport.Cell(currentRowIndex, 9).Text = totalQuota.ToString();
            filialeOfficeTotalQuota = 0;
            filialeTotal2Quota = 0;
            filialeTotalPlanDeclarePayment = 0;
            gdCompanyReport.Range(currentRowIndex - 2, 8, currentRowIndex, 8).Merge();
            gdCompanyReport.Cell(currentRowIndex - 2, 8).Text = master.Approval.ToString();
            int colIndex = 2;
            for (int i = colIndex; i < gdCompanyReport.Cols; i++)
            {
                gdCompanyReport.Cell(currentRowIndex, i).Text =
                    ComputeColumnTotalValue(gdCompanyReport, 14, gdCompanyReport.Rows - 1, i).ToString("N2");
            }
            CommonUtil.SetFlexGridBorder(gdCompanyReport, 14);
            CommonUtil.SetFlexGridColumnAutoFit(gdCompanyReport);
            gdCompanyReport.Range(currentRowIndex - 2, 1, currentRowIndex, 10).Alignment = AlignmentEnum.RightCenter;
            CommonUtil.SetGridRangeLockState(gdCompanyReport.Range(6, 1, gdCompanyReport.Rows - 1, gdCompanyReport.Cols - 1), true);
        }

        private decimal ComputeColumnTotalValue(CustomFlexGrid grid, int startRow, int endRow, int colIndex)
        {
            decimal totalVal = 0;
            for (int i = startRow; i < endRow; i++)
            {
                var txt = grid.Cell(i, colIndex).Text.Trim();
                var tmp = 0m;
                if (decimal.TryParse(txt, out tmp))
                {
                    totalVal += tmp;
                }
            }

            return totalVal;
        }

        public static bool IsNum(String str)// 判断是否是数字字符串
        {
            if (str == "" || str == null)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (!Char.IsNumber(str, i))
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 公司直管登录，保存数据
        /// </summary>
        /// <param name="master"></param>
        private void saveUpdateOtherPayPlanDetails(ProjectFundPlanMaster master)
        {
            List<ProjectOtherPayPlanDetail> otherDetails = master.OtherPayDetails.OfType<ProjectOtherPayPlanDetail>().ToList();
            List<FundPlanFlexGridOperate> otherPlanDetailOperate = FundPlanOperate.OtherPlanDetailOperate;
            List<ProjectOtherPayPlanDetail> allDataRowDetails = new List<ProjectOtherPayPlanDetail>();
            List<string> AllotGreaterRowNumList = new List<string>();//输入的分配额值过大
            if (otherDetails.Count == 0)
            {
                saveUpdateOtherPayPlanDetailsSucceed = 0;
            }
            foreach (FundPlanFlexGridOperate operate in otherPlanDetailOperate)
            {
                int rowIndex = operate.CurrentRowNumber;
                if (!string.IsNullOrEmpty(gdOtherExpend.Cell(rowIndex, 5).Text))
                {
                    if (operate.DataRow)
                    {
                        operate.Quota = gdOtherExpend.Cell(rowIndex, 5).Text;
                    }
                }
            }
            List<FundPlanFlexGridOperate> otherPlanDetailDataRowOperate = otherPlanDetailOperate.FindAll(a => a.DataRow == true);
            foreach (FundPlanFlexGridOperate operate in otherPlanDetailDataRowOperate)
            {
                ProjectOtherPayPlanDetail oneDetail = otherDetails.Find(a => a.Id == operate.Id);
                oneDetail.ParentId = master.Id;
                decimal quota = 0;
                decimal.TryParse(operate.Quota, out quota);
                if (quota <= oneDetail.PlanDeclarePayment & quota <= master.ApprovalAmount)
                {
                    oneDetail.Quota = quota;
                    allDataRowDetails.Add(oneDetail);
                }
                else
                {
                    AllotGreaterRowNumList.Add(operate.CurrentRowNumber.ToString());
                }
            }
            if (AllotGreaterRowNumList.Count == 0)
            {
                foreach (FundPlanFlexGridOperate operate in otherPlanDetailOperate)
                {
                    if (!string.IsNullOrEmpty(operate.Quota))
                    {
                        gdOtherExpend.Cell(operate.CurrentRowNumber, 5).ForeColor = Color.Black;
                    }
                }
                var savedDataList = mOperate.FinanceMultDataSrv.SaveOtherPayPlanDetails(allDataRowDetails);
                if (savedDataList.Count > 0)
                {
                    saveUpdateOtherPayPlanDetailsSucceed = 1;
                }
            }
            else
            {
                saveUpdateOtherPayPlanDetailsSucceed = -1;
                foreach (string rowStr in AllotGreaterRowNumList)
                {
                    gdOtherExpend.Cell(int.Parse(rowStr), 5).ForeColor = Color.Red;
                }
                MessageBox.Show(string.Format("第 {0} 行，分配额大于计划申报支出金额或审批额或审批额，请确认后重新输入！",
                                              string.Join(",", AllotGreaterRowNumList.ToArray())));
            }
        }

        private void saveUpdateProjectPlanDetails(ProjectFundPlanMaster master)
        {
            List<ProjectFundPlanDetail> details = master.Details.OfType<ProjectFundPlanDetail>().ToList();
            List<FundPlanFlexGridOperate> projectPlanDetailOperate = FundPlanOperate.ProjectPlanDetailOperate;
            List<ProjectFundPlanDetail> allDataRowDetails = new List<ProjectFundPlanDetail>();
            List<string> AllotGreaterRowNumList = new List<string>();//输入的分配额值过大
            if (details.Count == 0)
            {
                saveUpdateProjectPlanDetailsSucceed = 0;
            }
            foreach (FundPlanFlexGridOperate operate in projectPlanDetailOperate)
            {
                int rowIndex = operate.CurrentRowNumber;
                if (!string.IsNullOrEmpty(gdProjectExpend.Cell(rowIndex, 15).Text))
                {
                    if (operate.DataRow)
                    {
                        operate.Quota = gdProjectExpend.Cell(rowIndex, 15).Text;
                    }
                }
            }
            List<FundPlanFlexGridOperate> planDetailDataRowOperate = projectPlanDetailOperate.FindAll(a => a.DataRow == true);
            foreach (FundPlanFlexGridOperate operate in planDetailDataRowOperate)
            {
                ProjectFundPlanDetail oneDetail = details.Find(a => a.Id == operate.Id);
                decimal quota = 0;
                decimal.TryParse(operate.Quota, out quota);
                if (quota <= oneDetail.PlanPayment & quota <= master.ApprovalAmount)
                {
                    oneDetail.Quota = quota;
                    allDataRowDetails.Add(oneDetail);
                }
                else
                {
                    AllotGreaterRowNumList.Add(operate.CurrentRowNumber.ToString());
                }

            }
            if (AllotGreaterRowNumList.Count == 0)
            {
                foreach (FundPlanFlexGridOperate operate in projectPlanDetailOperate)
                {
                    if (!string.IsNullOrEmpty(operate.Quota))
                    {
                        gdProjectExpend.Cell(operate.CurrentRowNumber, 15).ForeColor = Color.Black;
                    }
                }
                var dataList = mOperate.FinanceMultDataSrv.SaveProjectPlanDetails(allDataRowDetails);
                if (dataList.Count > 0)
                {
                    saveUpdateProjectPlanDetailsSucceed = 1;
                }
            }
            else
            {
                saveUpdateProjectPlanDetailsSucceed = -1;
                foreach (string rowStr in AllotGreaterRowNumList)
                {
                    gdProjectExpend.Cell(int.Parse(rowStr), 15).ForeColor = Color.Red;
                }
                MessageBox.Show(string.Format("第 {0} 行，分配额大于计划申报付款金额，请确认后重新输入！",
                                              string.Join(",", AllotGreaterRowNumList.ToArray())));
            }
        }

        /// <summary>
        /// 分公司登录，保存数据
        /// </summary>
        /// <param name="master"></param>
        private void saveUpdateOfficePlanDetails(FilialeFundPlanMaster master)
        {
            if (master.OfficeFundPlanDetails == null)
            {
                return;
            }
            List<OfficeFundPlanPayDetail> officeDetails = master.OfficeFundPlanDetails.OfType<OfficeFundPlanPayDetail>()
                            .ToList();
            List<FundPlanFlexGridOperate> officePlanDetailOperate = FundPlanOperate.OfficePlanDetailOperate;
            List<OfficeFundPlanPayDetail> allDataRowDetails = new List<OfficeFundPlanPayDetail>();
            List<string> AllotGreaterRowNumList = new List<string>();//输入的分配额值过大
            if (officeDetails.Count == 0)
            {
                saveUpdateOfficePlanDetailsSucceed = 0;
            }
            foreach (FundPlanFlexGridOperate operate in officePlanDetailOperate)
            {
                int rowIndex = operate.CurrentRowNumber;
                if (!string.IsNullOrEmpty(gdOfficeExpend.Cell(rowIndex, 5).Text))
                {
                    if (operate.DataRow)
                    {
                        operate.Quota = gdOfficeExpend.Cell(rowIndex, 5).Text;
                    }
                }
            }

            List<FundPlanFlexGridOperate> officeDetailDataRowOperate = officePlanDetailOperate.FindAll(a => a.DataRow == true);
            foreach (FundPlanFlexGridOperate operate in officeDetailDataRowOperate)
            {
                OfficeFundPlanPayDetail oneDetail = officeDetails.Find(a => a.Id == operate.Id);
                decimal quota = 0;
                decimal.TryParse(operate.Quota, out quota);
                oneDetail.Quota = quota;
                if (quota <= oneDetail.PlanDeclarePayment & quota <= master.Approval)
                {
                    oneDetail.Quota = quota;
                    allDataRowDetails.Add(oneDetail);
                }
                else
                {
                    AllotGreaterRowNumList.Add(operate.CurrentRowNumber.ToString());
                }
            }
            if (AllotGreaterRowNumList.Count == 0)
            {
                foreach (FundPlanFlexGridOperate operate in officePlanDetailOperate)
                {
                    if (!string.IsNullOrEmpty(operate.Quota))
                    {
                        gdOfficeExpend.Cell(operate.CurrentRowNumber, 5).ForeColor = Color.Black;
                    }
                }
                var savedDataList = mOperate.FinanceMultDataSrv.SaveOfficePlanDetails(allDataRowDetails);
                if (savedDataList.Count > 0)
                {
                    saveUpdateOfficePlanDetailsSucceed = 1;
                }
            }
            else
            {
                saveUpdateOfficePlanDetailsSucceed = -1;
                foreach (string rowStr in AllotGreaterRowNumList)
                {
                    gdOfficeExpend.Cell(int.Parse(rowStr), 5).ForeColor = Color.Red;
                }
                MessageBox.Show(string.Format("第 {0} 行，分配额大于计划申报支出金额或审批额，请确认后重新输入！",
                                              string.Join(",", AllotGreaterRowNumList.ToArray())));
            }
        }

        private void saveUpdateTotalExpend2(FilialeFundPlanMaster master)
        {
            List<ProjectFundPlanMaster> list = 
                (List<ProjectFundPlanMaster>)mOperate.FinanceMultDataSrv.GetFilialeFundPlanProjectDetail(master);
            List<FundPlanFlexGridOperate> operates = FundPlanOperate.TotalExpend2Operate;
            List<ProjectFundPlanMaster> allDataRows = new List<ProjectFundPlanMaster>();
            List<string> AllotGreaterRowNumList = new List<string>();//输入的分配额值过大
            if (list == null)
            {
                saveUpdateTotalExpend2Succeed = 0;
            }
            if (list != null)
            {
                for (int rowIndex = 5; rowIndex < gdTotalExpend2.Rows;rowIndex++)
                {
                    string columnName = gdTotalExpend2.Cell(rowIndex, 2).Text;
                    if (!string.IsNullOrEmpty(columnName) &
                        columnName != "小计" &
                        columnName != "合计"&
                        gdTotalExpend2.Cell(rowIndex, 1).Text!="合计")
                    {
                        List<ProjectFundPlanMaster> projectList = list.FindAll(a=>a.ProjectName==columnName);
                        foreach (ProjectFundPlanMaster oneProject in projectList)
                        {
                            string allotString = gdTotalExpend2.Cell(rowIndex, 11).Text;
                            decimal quota = 0;
                            Boolean parseSuccess = decimal.TryParse(allotString, out quota);
                            if(decimal.Parse(allotString)<=decimal.Parse(gdTotalExpend2.Cell(rowIndex, 12).Text)
                                & decimal.Parse(allotString)<=master.Approval)
                            {
                                oneProject.ApprovalAmount = parseSuccess ? decimal.Parse(allotString) : quota;
                                allDataRows.Add(oneProject);
                            }                                            
                            else
                            {
                                AllotGreaterRowNumList.Add(rowIndex.ToString());
                            }
                        }
                    }
                }
                if (AllotGreaterRowNumList.Count == 0)
                {
                    foreach(FundPlanFlexGridOperate operate in operates)
                    {
                        gdTotalExpend2.Cell(operate.CurrentRowNumber,11).ForeColor=Color.Black;
                    }
                    var savedDataList = mOperate.FinanceMultDataSrv.SaveProjectFundPlans(allDataRows);
                    if (savedDataList.Count > 0)
                    {
                        saveUpdateTotalExpend2Succeed = 1;
                    }
                 }
                else
                {
                    saveUpdateTotalExpend2Succeed = -1;
                    foreach (string rowStr in AllotGreaterRowNumList)
                    {
                        gdTotalExpend2.Cell(int.Parse(rowStr), 11).ForeColor = Color.Red;
                    }
                    MessageBox.Show(string.Format("第 {0} 行，分配额大于本月付款小计金额或者审批额，请确认后重新输入！",
                                                  string.Join(",", AllotGreaterRowNumList.ToArray())));
                }
            }
        }

        private void saveUpdateFilialePlanDetails(FilialeFundPlanMaster master)
        {
            List<FilialeFundPlanDetail> filialeDetails = master.Details.OfType<FilialeFundPlanDetail>().ToList();
            List<FundPlanFlexGridOperate> filialeDetailOperate = FundPlanOperate.FilialePlanDetailOperate;
            List<FilialeFundPlanDetail> allDataRowDetails = new List<FilialeFundPlanDetail>();
            foreach (FundPlanFlexGridOperate operate in filialeDetailOperate)
            {
                int rowIndex = operate.CurrentRowNumber;
                if (!string.IsNullOrEmpty(gdCompanyReport.Cell(rowIndex, 9).Text))
                {
                    if (operate.DataRow)
                    {
                        operate.Quota = gdCompanyReport.Cell(rowIndex, 9).Text;
                    }
                }
            }

            List<FundPlanFlexGridOperate> filialeDetaillDataRowOperate = filialeDetailOperate.FindAll(a => a.DataRow == true);
            foreach (FundPlanFlexGridOperate operate in filialeDetaillDataRowOperate)
            {
                FilialeFundPlanDetail oneDetail = filialeDetails.Find(a => a.Id == operate.Id);
                decimal quota = 0;
                decimal.TryParse(operate.Quota, out quota);
                oneDetail.Quota = quota;
                allDataRowDetails.Add(oneDetail);
            }
            mOperate.FinanceMultDataSrv.SaveFilialePlanDetails(allDataRowDetails);
        }

        /// <summary>
        /// 批量修改数据，并保存至数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (loginOrgtype == 1)//总部,显示机关资金计划支出明细
            {
                if (dgMaster.CurrentRow != null)
                {
                    FilialeFundPlanMaster master = dgMaster.CurrentRow.Tag as FilialeFundPlanMaster;
                }
                else
                {
                    MessageBox.Show("请选择要编辑的单据！");
                    return;
                }
            }
            else if (loginOrgtype == 2)
            {
                if (dgMaster.CurrentRow != null)
                {
                    ProjectFundPlanMaster master = dgMaster.CurrentRow.Tag as ProjectFundPlanMaster;
                    foreach (TabPage tPage in tabControl1.TabPages)
                    {
                        if (tPage.Text == "项目资金计划其他支出明细表")
                        {
                            saveUpdateOtherPayPlanDetails(master);
                        }
                        else if (tPage.Text == "资金计划申报明细表")
                        {
                            saveUpdateProjectPlanDetails(master);
                        }
                    }
                    if (saveUpdateOtherPayPlanDetailsSucceed + saveUpdateProjectPlanDetailsSucceed > 0)
                    {
                        MessageBox.Show("数据保存成功！");
                    }
                }
                else
                {
                    MessageBox.Show("请选择要编辑的单据！");
                    return;
                }
            }
            if (loginOrgtype == 3)
            {
                if (dgMaster.CurrentRow != null)
                {
                    FilialeFundPlanMaster master = dgMaster.CurrentRow.Tag as FilialeFundPlanMaster;
                    foreach (TabPage tPage in tabControl1.TabPages)
                    {
                        if (tPage.Text == "机关资金计划支出明细表")
                        {
                            saveUpdateOfficePlanDetails(master);
                        }
                        else if (tPage.Text == "资金计划申报汇总表2")
                        {
                            saveUpdateTotalExpend2(master);
                        }
                        else if (tPage.Text == "分公司资金支付计划申报表")
                        {
                            saveUpdateFilialePlanDetails(master);
                        }
                    }
                    if (saveUpdateOfficePlanDetailsSucceed + saveUpdateTotalExpend2Succeed > 0)
                    {
                        MessageBox.Show("数据保存成功！");
                    }
                }
                else
                {
                    MessageBox.Show("请选择要编辑的单据！");
                    return;
                }
            }
        }

        private void gdOtherExpend_EditRow(object sender, EventArgs e)
        {
            gdOtherExpendSelectedRowNum = gdOtherExpend.MouseRow;
        }

        private void gdProjectExpend_EditRow(object sender, EventArgs e)
        {
            gdProjectExpendSelectedRowNum = gdProjectExpend.MouseRow;
        }

        private void gdOfficeExpend_EditRow(object sender, EventArgs e)
        {
            gdOfficeExpendSelectedRowNum = gdOfficeExpend.MouseRow;
        }

        private void gdCompanyReport_EditRow(object sender, EventArgs e)
        {
            gdCompanyReportSelectedRowNum = gdCompanyReport.MouseRow;
        }

        private void gdTotalExpend2_EditRow(object sender, EventArgs e)
        {
            gdTotalExpend2SelectedRowNum = gdTotalExpend2.MouseRow;
        }

        /// <summary>
        /// 修改单条记录，预留方法
        /// </summary>
        private void modifySingleRecordData()
        {
            ProjectFundPlanMaster master = dgMaster.CurrentRow.Tag as ProjectFundPlanMaster;
            if (master == null) return;
            foreach (TabPage tPage in tabControl1.TabPages)
            {
                if (tPage.Text == "项目资金计划其他支出明细表")
                {
                    modifyOtherPayDetailRecord(master);
                }
                else if (tPage.Text == "资金计划申报明细表")
                {
                    modifyProjectFundDetailRecord(master);
                }
            }
        }

        /// <summary>
        /// 修改单条记录，预留方法
        /// </summary>
        /// <param name="master"></param>
        private void modifyProjectFundDetailRecord(ProjectFundPlanMaster master)
        {
            List<ProjectFundPlanDetail> details = master.Details.OfType<ProjectFundPlanDetail>().ToList();
            List<FundPlanFlexGridOperate> projectPlanDetailOperate = FundPlanOperate.ProjectPlanDetailOperate;
            List<FundPlanFlexGridOperate> dataRowOperate = projectPlanDetailOperate.FindAll(a => a.DataRow == true);
            if (!string.IsNullOrEmpty(gdProjectExpend.Cell(gdProjectExpendSelectedRowNum, 15).Text))
            {
                if (dataRowOperate.Find(a => a.CurrentRowNumber == gdProjectExpendSelectedRowNum) != null)
                {
                    FundPlanFlexGridOperate operate = dataRowOperate.Find(a => a.CurrentRowNumber == gdProjectExpendSelectedRowNum);
                    if (operate.DataRow)
                    {
                        ProjectFundPlanDetail oneDetail = details.Find(a => a.Id == operate.Id);
                        int quota = int.Parse(gdProjectExpend.Cell(gdProjectExpendSelectedRowNum, 15).Text);
                        if (quota != 0)
                        {
                            if (quota > oneDetail.PlanPayment)
                            {
                                MessageBox.Show("分配额应该小于或等于计划申报支出金额，第" + gdProjectExpendSelectedRowNum + "行分配额,请重新输入！");
                            }
                            else
                            {
                                oneDetail.Quota = quota;
                                mOperate.FinanceMultDataSrv.SaveProjectPlanDetail(oneDetail);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 修改单条记录，预留方法
        /// </summary>
        /// <param name="master"></param>
        private void modifyOtherPayDetailRecord(ProjectFundPlanMaster master)
        {
            List<ProjectOtherPayPlanDetail> otherDetails = master.OtherPayDetails.OfType<ProjectOtherPayPlanDetail>().ToList();
            List<FundPlanFlexGridOperate> otherPlanDetailOperate = FundPlanOperate.OtherPlanDetailOperate;
            List<FundPlanFlexGridOperate> otherPlanDetailDataRowOperate = otherPlanDetailOperate.FindAll(a => a.DataRow == true);
            if (!string.IsNullOrEmpty(gdOtherExpend.Cell(gdOtherExpendSelectedRowNum, 5).Text))
            {
                if (otherPlanDetailDataRowOperate.Find(a => a.CurrentRowNumber == gdOtherExpendSelectedRowNum) != null)
                {
                    FundPlanFlexGridOperate operate = otherPlanDetailDataRowOperate.Find(a => a.CurrentRowNumber == gdOtherExpendSelectedRowNum);
                    if (operate.DataRow)
                    {
                        ProjectOtherPayPlanDetail oneDetail = otherDetails.Find(a => a.Id == operate.Id);
                        int quota = int.Parse(gdOtherExpend.Cell(gdOtherExpendSelectedRowNum, 5).Text);
                        if (quota != 0)
                        {
                            if (quota > oneDetail.PlanDeclarePayment)
                            {
                                MessageBox.Show("分配额应该小于或等于计划申报支出金额，第" + gdOtherExpendSelectedRowNum + "行分配额,请重新输入！");
                            }
                            else
                            {
                                oneDetail.Quota = quota;
                                mOperate.FinanceMultDataSrv.SaveOtherPayPlanDetail(oneDetail);
                            }
                        }
                    }
                }
            }
        }
    }
}
