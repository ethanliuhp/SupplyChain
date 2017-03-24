using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using FlexCell;
using VirtualMachine.Component.WinControls.Controls;
using System.Windows;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public class FundPlanOperate
    {
        private const string XIAOJI = "小计";
        private const string HEJI = "合计";
        private static List<FundPlanFlexGridOperate> otherPlanDetailOperate = new List<FundPlanFlexGridOperate>();
        private static List<FundPlanFlexGridOperate> projectPlanDetailOperate = new List<FundPlanFlexGridOperate>();
        private static List<FundPlanFlexGridOperate> officePlanDetailOperate = new List<FundPlanFlexGridOperate>();
        private static List<FundPlanFlexGridOperate> filialePlanDetailOperate = new List<FundPlanFlexGridOperate>();
        private static List<FundPlanFlexGridOperate> totalExpend2Operate = new List<FundPlanFlexGridOperate>();

        private static List<FundPlanFlexGridOperate> otherPlanCostTypeOperate = new List<FundPlanFlexGridOperate>();
        private static List<FundPlanFlexGridOperate> projectPlanCostTypeOperate = new List<FundPlanFlexGridOperate>();

        public static List<FundPlanFlexGridOperate> ProjectPlanDetailOperate
        {
            get { return FundPlanOperate.projectPlanDetailOperate; }
            set { FundPlanOperate.projectPlanDetailOperate = value; }
        }

        public static List<FundPlanFlexGridOperate> OtherPlanDetailOperate
        {
            get { return FundPlanOperate.otherPlanDetailOperate; }
            set { FundPlanOperate.otherPlanDetailOperate = value; }
        }

        public static List<FundPlanFlexGridOperate> OtherPlanCostTypeOperate
        {
            get { return FundPlanOperate.otherPlanCostTypeOperate; }
            set { FundPlanOperate.otherPlanCostTypeOperate = value; }
        }

        public static List<FundPlanFlexGridOperate> ProjectPlanCostTypeOperate
        {
            get { return FundPlanOperate.projectPlanCostTypeOperate; }
            set { FundPlanOperate.projectPlanCostTypeOperate = value; }
        }

        public static List<FundPlanFlexGridOperate> OfficePlanDetailOperate
        {
            get { return FundPlanOperate.officePlanDetailOperate; }
            set { FundPlanOperate.officePlanDetailOperate = value; }
        }

        public static List<FundPlanFlexGridOperate> FilialePlanDetailOperate
        {
            get { return FundPlanOperate.filialePlanDetailOperate; }
            set { FundPlanOperate.filialePlanDetailOperate = value; }
        }

        public static List<FundPlanFlexGridOperate> TotalExpend2Operate
        {
            get { return FundPlanOperate.totalExpend2Operate; }
            set { FundPlanOperate.totalExpend2Operate = value; }
        }

        public static void LoadFlexFile(string flxname, CustomFlexGrid grid)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(flxname))
            {
                eFile.CreateTempleteFileFromServer(flxname);

                grid.OpenFile(path + "\\" + flxname); //载入格式
            }
            grid.EnterKeyMoveTo = MoveToEnum.NextRow;
        }

        /// <summary>
        /// 显示分公司资金支付计划申报表
        /// </summary>
        /// <param name="filialeFundPlan">分公司资金计划</param>
        /// <param name="gdCompanyReport">Grid</param>
        /// <param name="flxName">Grid模板文件名</param>
        public static void DisplayFilialePlanMaster(FilialeFundPlanMaster filialeFundPlan,
                                                    CustomFlexGrid gdCompanyReport, string flxName)
        {
            if (filialeFundPlan == null)
            {
                return;
            }

            LoadFlexFile(flxName, gdCompanyReport);
            FilialePlanDetailOperate.Clear();
            gdCompanyReport.Cell(2, 1).Text = "申报单位：" + filialeFundPlan.DeclareUnit;;
            gdCompanyReport.Cell(2, 6).Text = "申报日期：" + filialeFundPlan.CreateDate.ToString("yyyy-MM-dd");

            var startRowIndex = 6;//单位资金流量起始行号
            var startColIndex = 1;

            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.FinanceConfirmTaxIncome.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.CumulativeCurrentYearGathering.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.CumulativeCurrentYearCashRatio.ToString("P2");
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.CumulativeCurrentYearPayment.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.CumulativeCurrentYearFundFlow.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.PresentMonthGathering.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.PresentMonthPlanPayment.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex).Text = filialeFundPlan.CurrentYearFundNetFlow.ToString("N2");

            var range = gdCompanyReport.Range(startRowIndex, 2, startRowIndex, gdCompanyReport.Cols - 1);
            CommonUtil.SetGridRangeLockState(range, true);

            startRowIndex = 10;//单位资金存量起始行号
            startColIndex = 1;
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.TillLastMonthFundStock.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.ThereinLoan.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.ThereinSuperviseAccountFund.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.ThereinBankAccept.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = filialeFundPlan.PresentMonthGathering.ToString("N2");
            gdCompanyReport.Cell(startRowIndex, startColIndex).Text = filialeFundPlan.PresentMonthSpendableFund.ToString("N2");

            range = gdCompanyReport.Range(startRowIndex, 5, startRowIndex, gdCompanyReport.Cols - 1);

            startRowIndex = 14;//资金支付计划及审批起始行号
            Iesi.Collections.Generic.ISet<BaseDetail> detailSet = filialeFundPlan.Details;
            foreach (FilialeFundPlanDetail detail in detailSet)
            {
                startColIndex = 1;
                gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = detail.ProjectCategory;
                gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = detail.CumulativeSettlement.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = detail.CumulativePayment.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = detail.CumulativeArrears.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = detail.CumulativeExpireDue.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = detail.CumulativeExpireDue.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, startColIndex++).Text = detail.ThisMonthInstallFilialePayment.ToString("N2");
                FilialePlanDetailOperate.Add(new FundPlanFlexGridOperate(detail.Id, filialeFundPlan.Id, startRowIndex, true, gdCompanyReport));
                startRowIndex++;
            }

            CommonUtil.SetGridRangeLockState(range, true);
        }

        /// <summary>
        /// 显示机关资金计划支出明细表
        /// </summary>
        /// <param name="filialeFundPlan">分公司资金计划</param>
        /// <param name="gdOfficeExpend">Grid</param>
        /// <param name="flxName">Grid模板文件名</param>
        public static void DisplayOfficePlanDetail(FilialeFundPlanMaster filialeFundPlan,
                                                    CustomFlexGrid gdOfficeExpend, string flxName)
        {
            if (filialeFundPlan == null)
            {
                return;
            }
            LoadFlexFile(flxName, gdOfficeExpend);
            gdOfficeExpend.Cell(2, 1).Text = "申报单位：" + filialeFundPlan.DeclareUnit;
            gdOfficeExpend.Cell(2, 3).Text = "申报日期：" + filialeFundPlan.CreateDate.ToString("yyyy-MM-dd");
            gdOfficeExpend.Cell(2, 4).Text = "";
            gdOfficeExpend.Cell(2, 5).Text = "单位：元";

            officePlanDetailOperate.Clear();
            var startRowIndex = 4;
            var oldRow = startRowIndex;
            var officeDetails = filialeFundPlan.OfficeFundPlanDetails.OfType<OfficeFundPlanPayDetail>().ToList();
            if (officeDetails.Count == 0)
            {
                return;
            }

            var groupBySet = officeDetails.GroupBy(a => a.PayScope);
            gdOfficeExpend.FrozenRows = startRowIndex - 1;
            gdOfficeExpend.FrozenCols = 3;
            var insertRow = gdOfficeExpend.Rows - 1 - startRowIndex - officeDetails.Count() - groupBySet.Count();
            if (insertRow < 0)
            {
                gdOfficeExpend.InsertRow(startRowIndex, insertRow * -1);
            }
            else
            {
                gdOfficeExpend.Rows = gdOfficeExpend.Rows - insertRow;
            }
            CommonUtil.SetFlexGridBorder(gdOfficeExpend, startRowIndex);
            gdOfficeExpend.Range(startRowIndex, 4, gdOfficeExpend.Rows - 1, 4).Alignment = AlignmentEnum.RightCenter;
            gdOfficeExpend.Range(startRowIndex, 1, gdOfficeExpend.Rows - 1, 1).MergeCells = false;
            CommonUtil.SetFlexGridBorder(gdOfficeExpend, startRowIndex);

            decimal totalQuota = 0;
            foreach (var gp in groupBySet)
            {
                var gpDetails = gp.ToList();
                var seqNo = 1;
                decimal subtotalQuota = 0;
                foreach (OfficeFundPlanPayDetail detail in gpDetails)
                {
                    var startColIndex = 1;

                    detail.OrderNumber = seqNo;
                    gdOfficeExpend.Cell(startRowIndex, startColIndex).FontBold = false;
                    gdOfficeExpend.Cell(startRowIndex, startColIndex++).Text = detail.PayScope;
                    gdOfficeExpend.Cell(startRowIndex, startColIndex).Tag = string.IsNullOrEmpty(detail.Id)
                                                                                 ? detail.TempData
                                                                                 : detail.Id;
                    gdOfficeExpend.Cell(startRowIndex, startColIndex).FontBold = false;
                    gdOfficeExpend.Cell(startRowIndex, startColIndex++).Text = detail.OrderNumber.ToString();
                    gdOfficeExpend.Cell(startRowIndex, startColIndex).FontBold = false;
                    gdOfficeExpend.Cell(startRowIndex, startColIndex++).Text = detail.CostDetail;
                    gdOfficeExpend.Cell(startRowIndex, startColIndex).FontBold = false;
                    gdOfficeExpend.Cell(startRowIndex, startColIndex++).Text = detail.PlanDeclarePayment.ToString();
                    gdOfficeExpend.Cell(startRowIndex, startColIndex).FontBold = false;
                    gdOfficeExpend.Cell(startRowIndex, startColIndex).Text = detail.Descript;

                    officePlanDetailOperate.Add(new FundPlanFlexGridOperate(detail.Id, filialeFundPlan.Id, startRowIndex, true, gdOfficeExpend));
                    subtotalQuota += detail.Quota;
                    startRowIndex++;
                    seqNo++;
                }
                //小计
                gdOfficeExpend.Cell(startRowIndex, 2).Text = XIAOJI;
                var range = gdOfficeExpend.Range(startRowIndex, 2, startRowIndex, 3);
                range.Merge();
                range.Alignment = AlignmentEnum.CenterCenter;

                gdOfficeExpend.Cell(startRowIndex, 4).Text = gpDetails.Sum(a => a.PlanDeclarePayment).ToString("N2");
                range = gdOfficeExpend.Range(startRowIndex, 4, startRowIndex, 5);
                CommonUtil.SetGridRangeLockState(range, true);

                gdOfficeExpend.Range(oldRow, 1, startRowIndex, 1).Merge();
                officePlanDetailOperate.Add(new FundPlanFlexGridOperate(startRowIndex, subtotalQuota, false, gdOfficeExpend));
                totalQuota += subtotalQuota;
                startRowIndex++;
                oldRow = startRowIndex;
            }
            //合计
            gdOfficeExpend.Cell(startRowIndex, 1).Text = HEJI;
            var totalRange = gdOfficeExpend.Range(startRowIndex, 1, startRowIndex, 3);
            totalRange.Merge();
            totalRange.Alignment = AlignmentEnum.CenterCenter;

            decimal totalPlanDeclarePayment = officeDetails.Sum(a => a.PlanDeclarePayment);
            gdOfficeExpend.Cell(startRowIndex, 4).Text = officeDetails.Sum(a => a.PlanDeclarePayment).ToString("N2");
            totalRange = gdOfficeExpend.Range(startRowIndex, 3, startRowIndex, 5);
            officePlanDetailOperate.Add(new FundPlanFlexGridOperate(startRowIndex, totalQuota, false, gdOfficeExpend));
            officePlanDetailOperate.Add(new FundPlanFlexGridOperate("计划支出", totalPlanDeclarePayment, false, gdOfficeExpend));
            CommonUtil.SetGridRangeLockState(totalRange, true);

            CommonUtil.SetGridColumnLockState(gdOfficeExpend, new List<int>() {1, 2}, true);
        }

        /// <summary>
        /// 显示项目资金计划其他支出明细表
        /// </summary>
        /// <param name="projectFundPlan">项目资金计划</param>
        /// <param name="gdOtherExpend">Grid</param>
        /// <param name="flxName">Grid模板文件名</param>
        public static void DisplayProjectOtherPlanDetail(ProjectFundPlanMaster projectFundPlan,
                                                    CustomFlexGrid gdOtherExpend, string flxName)
        {
            if (projectFundPlan == null)
            {
                return;
            }
            otherPlanDetailOperate.Clear();
            otherPlanCostTypeOperate.Clear();
            LoadFlexFile(flxName, gdOtherExpend);
            gdOtherExpend.Cell(2, 1).Text = string.Format("申报项目名称：	{0}", projectFundPlan.ProjectName);
            gdOtherExpend.Cell(2, 3).Text = "申报日期：" + projectFundPlan.CreateDate.ToString("yyyy-MM-dd");
            gdOtherExpend.Cell(2, 4).Text = "单位：";

            var startRowIndex = 4;
            var oldRow = startRowIndex;

            var otherDetails = projectFundPlan.OtherPayDetails.OfType<ProjectOtherPayPlanDetail>().ToList();
            if (otherDetails.Count == 0)
            {
                return;
            }
            var groupBySet = otherDetails.GroupBy(a => a.PayScope);
            gdOtherExpend.FrozenRows = startRowIndex - 1;
            gdOtherExpend.FrozenCols = 3;
            gdOtherExpend.InsertRow(startRowIndex, otherDetails.Count() + groupBySet.Count() + 1);
            CommonUtil.SetFlexGridBorder(gdOtherExpend, startRowIndex);

            decimal totalQuota = 0;
            foreach (var gp in groupBySet)
            {
                var gpDetails = gp.ToList();
                var seqNo = 1;
                decimal subtotalQuota = 0;
                foreach (ProjectOtherPayPlanDetail detail in gpDetails)
                {
                    var startColIndex = 1;

                    detail.OrderNumber = seqNo;
                    gdOtherExpend.Cell(startRowIndex, startColIndex).FontBold = false;
                    gdOtherExpend.Cell(startRowIndex, startColIndex++).Text = detail.PayScope;
                    gdOtherExpend.Cell(startRowIndex, startColIndex).Tag = string.IsNullOrEmpty(detail.Id)
                                                                                 ? detail.TempData
                                                                                 : detail.Id;
                    gdOtherExpend.Cell(startRowIndex, startColIndex).FontBold = false;
                    gdOtherExpend.Cell(startRowIndex, startColIndex++).Text = detail.OrderNumber.ToString();
                    gdOtherExpend.Cell(startRowIndex, startColIndex).FontBold = false;
                    gdOtherExpend.Cell(startRowIndex, startColIndex++).Text = detail.CostDetail;
                    gdOtherExpend.Cell(startRowIndex, startColIndex).FontBold = false;
                    gdOtherExpend.Cell(startRowIndex, startColIndex++).Text = detail.PlanDeclarePayment.ToString();
                    gdOtherExpend.Cell(startRowIndex, startColIndex).FontBold = false;
                    gdOtherExpend.Cell(startRowIndex, startColIndex++).Text = detail.Descript;
                    subtotalQuota += detail.Quota;
                    otherPlanDetailOperate.Add(new FundPlanFlexGridOperate(detail.Id, projectFundPlan.Id, startRowIndex, true, gdOtherExpend));
                    startRowIndex++;
                    seqNo++;
                }

                //小计
                gdOtherExpend.Cell(startRowIndex, 2).Text = XIAOJI;
                var range = gdOtherExpend.Range(startRowIndex, 2, startRowIndex, 3);
                range.Merge();
                range.Alignment = AlignmentEnum.CenterCenter;

                gdOtherExpend.Cell(startRowIndex, 4).Text = gpDetails.Sum(a => a.PlanDeclarePayment).ToString("N2");
                gdOtherExpend.Range(oldRow, 1, startRowIndex, 1).Merge();
                otherPlanDetailOperate.Add(new FundPlanFlexGridOperate(startRowIndex, subtotalQuota,false, gdOtherExpend));
                totalQuota += subtotalQuota;
                startRowIndex++;
                oldRow = startRowIndex;
            }
            //合计
            gdOtherExpend.Cell(startRowIndex, 1).Text = HEJI;
            var totalRange = gdOtherExpend.Range(startRowIndex, 1, startRowIndex, 3);
            otherPlanDetailOperate.Add(new FundPlanFlexGridOperate(startRowIndex, totalQuota, false, gdOtherExpend));
            FundPlanFlexGridOperate onePlanCostTypeOperate = new FundPlanFlexGridOperate(5, totalQuota, gdOtherExpend);
            otherPlanCostTypeOperate.Add(onePlanCostTypeOperate);
            totalRange.Merge();
            totalRange.Alignment = AlignmentEnum.CenterCenter;

            gdOtherExpend.Cell(startRowIndex, 4).Text = otherDetails.Sum(a => a.PlanDeclarePayment).ToString("N2");

            //隐藏
            for (int index = startRowIndex + 1; index < gdOtherExpend.Rows; index++)
            {
                gdOtherExpend.Row(index).Visible = false;
            }
        }

        /// <summary>
        /// 显示资金计划申报明细表
        /// </summary>
        /// <param name="projectFundPlan">项目资金计划</param>
        /// <param name="gdProjectExpend">Grid</param>
        /// <param name="flxName">Grid模板文件名</param>
        public static void DispalyProjectPlanDetail(ProjectFundPlanMaster projectFundPlan,
                                                    CustomFlexGrid gdProjectExpend, string flxName)
        {
            if (projectFundPlan == null)
            {
                return;
            }
            projectPlanDetailOperate.Clear();
            projectPlanCostTypeOperate.Clear();
            LoadFlexFile(flxName, gdProjectExpend);
            gdProjectExpend.Cell(2, 1).Text = string.Format("申报项目名称：	{0}", projectFundPlan.ProjectName);
            gdProjectExpend.Cell(2, 9).Text = "申报日期：" + projectFundPlan.CreateDate.ToString("yyyy-MM-dd");

            var startRowIndex = 5;
            var oldRow = startRowIndex;

            var details = projectFundPlan.Details.OfType<ProjectFundPlanDetail>().ToList();
            if (details.Count == 0)
            {
                return;
            }
            var gps = details.GroupBy(a => a.FundPaymentCategory);

            gdProjectExpend.FrozenRows = startRowIndex - 1;
            gdProjectExpend.FrozenCols = 3;
            gdProjectExpend.InsertRow(startRowIndex, details.Count() + gps.Count());
            CommonUtil.SetFlexGridBorder(gdProjectExpend, startRowIndex);
            gdProjectExpend.Range(startRowIndex, 4, gdProjectExpend.Rows - 1, 5).Alignment = AlignmentEnum.RightCenter;
            gdProjectExpend.Range(startRowIndex, 7, gdProjectExpend.Rows - 1, 14).Alignment = AlignmentEnum.RightCenter;
            var totalSettlement = 0m;
            var totalPayment = 0m;
            var payRate = 0m; 
            var planPay = 0m;
            decimal totalQuota = 0;
            foreach (var gp in gps)
            {
                var gpDetails = gp.ToList();
                var seqNo = 1;
                decimal subtotalQuota = 0;
                foreach (ProjectFundPlanDetail detail in gpDetails)
                {
                    var startColIndex = 1;

                    detail.OrderNumber = seqNo;

                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.FundPaymentCategory;

                    gdProjectExpend.Cell(startRowIndex, startColIndex).Tag = string.IsNullOrEmpty(detail.Id)
                                                                                 ? detail.TempData
                                                                                 : detail.Id;
                    gdProjectExpend.Cell(startRowIndex, startColIndex).Alignment = AlignmentEnum.CenterCenter;
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.OrderNumber.ToString();

                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.CreditorUnitLeadingOfficial;
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.ContractAmount.ToString("N2");
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text =
                        detail.ContractPaymentRatio.ToString("P2");
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.JobContent;
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text =
                        detail.CumulativeSettlement.ToString("N2");
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text =
                        detail.PrecedingMonthSettlement.ToString("N2");
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.CumulativePayment.ToString("N2");
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.CumulativeArrears.ToString("N2");
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.CumulativeExpireDue.ToString("N2");
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.PaymentRatio.ToString("P2");
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.PlanPayment.ToString("N2");
                    gdProjectExpend.Cell(startRowIndex, startColIndex++).Text = detail.PlanPaymentRatio.ToString("P2");
                    gdProjectExpend.Cell(startRowIndex, startColIndex).Text = detail.Descript;
                    subtotalQuota += detail.Quota;
                    projectPlanDetailOperate.Add(new FundPlanFlexGridOperate(detail.Id, projectFundPlan.Id, startRowIndex, true, gdProjectExpend));
                    startRowIndex++;
                    seqNo++;
                }

                //小计
                gdProjectExpend.Cell(startRowIndex, 2).Text = XIAOJI;
                var range = gdProjectExpend.Range(startRowIndex, 2, startRowIndex, 3);
                range.Merge();
                range.Alignment = AlignmentEnum.CenterCenter;

                gdProjectExpend.Cell(startRowIndex, 4).Text = gpDetails.Sum(a => a.ContractAmount).ToString("N2");
                totalSettlement = gpDetails.Sum(a => a.CumulativeSettlement);
                gdProjectExpend.Cell(startRowIndex, 7).Text = totalSettlement.ToString("N2");
                gdProjectExpend.Cell(startRowIndex, 8).Text =
                    gpDetails.Sum(a => a.PrecedingMonthSettlement).ToString("N2");
                totalPayment = gpDetails.Sum(a => a.CumulativePayment);
                gdProjectExpend.Cell(startRowIndex, 9).Text = totalPayment.ToString("N2");
                gdProjectExpend.Cell(startRowIndex, 10).Text = gpDetails.Sum(a => a.CumulativeArrears).ToString("N2");
                gdProjectExpend.Cell(startRowIndex, 11).Text = gpDetails.Sum(a => a.CumulativeExpireDue).ToString("N2");
                payRate = totalSettlement == 0 ? 0 : Math.Round(totalPayment/totalSettlement, 4);
                gdProjectExpend.Cell(startRowIndex, 12).Text = payRate.ToString("P2");
                planPay = gpDetails.Sum(a => a.PlanPayment);
                gdProjectExpend.Cell(startRowIndex, 13).Text = planPay.ToString("N2");
                payRate = totalSettlement == 0 ? 0 : Math.Round((totalPayment + planPay) / totalSettlement, 4);
                gdProjectExpend.Cell(startRowIndex, 14).Text = payRate.ToString("P2");
                range = gdProjectExpend.Range(startRowIndex, 13, startRowIndex, 15);
                //CommonUtil.SetGridRangeLockState(range, true);
                gdProjectExpend.Range(oldRow, 1, startRowIndex, 1).Merge();
                projectPlanDetailOperate.Add(new FundPlanFlexGridOperate(startRowIndex, subtotalQuota, false, gdProjectExpend));
                ProjectFundPlanDetail oneDetail = (ProjectFundPlanDetail)gpDetails.ElementAt(0);
                int costType = getCostType(oneDetail.FundPaymentCategory);
                projectPlanCostTypeOperate.Add(new FundPlanFlexGridOperate(costType, subtotalQuota, gdProjectExpend));
                totalQuota += subtotalQuota;
                startRowIndex++;
                oldRow = startRowIndex;
            }

            //合计
            gdProjectExpend.Cell(startRowIndex, 1).Text = HEJI;
            var totalRange = gdProjectExpend.Range(startRowIndex, 1, startRowIndex, 3);
            totalRange.Merge();
            totalRange.Alignment = AlignmentEnum.CenterCenter;

            gdProjectExpend.Cell(startRowIndex, 4).Text = details.Sum(a => a.ContractAmount).ToString("N2");
            totalSettlement = details.Sum(a => a.CumulativeSettlement);
            gdProjectExpend.Cell(startRowIndex, 7).Text = totalSettlement.ToString("N2");
            gdProjectExpend.Cell(startRowIndex, 8).Text = details.Sum(a => a.PrecedingMonthSettlement).ToString("N2");
            totalPayment = details.Sum(a => a.CumulativePayment);
            gdProjectExpend.Cell(startRowIndex, 9).Text = totalPayment.ToString("N2");
            gdProjectExpend.Cell(startRowIndex, 10).Text = details.Sum(a => a.CumulativeArrears).ToString("N2");
            gdProjectExpend.Cell(startRowIndex, 11).Text = details.Sum(a => a.CumulativeExpireDue).ToString("N2");
            payRate = totalSettlement == 0 ? 0 : Math.Round(totalPayment / totalSettlement, 4);
            gdProjectExpend.Cell(startRowIndex, 12).Text = payRate.ToString("P2");
            planPay = details.Sum(a => a.PlanPayment);
            gdProjectExpend.Cell(startRowIndex, 13).Text = planPay.ToString("N2");
            payRate = totalSettlement == 0 ? 0 : Math.Round((totalPayment + planPay) / totalSettlement, 4);
            gdProjectExpend.Cell(startRowIndex, 14).Text = payRate.ToString("P2");
            totalRange = gdProjectExpend.Range(startRowIndex, 13, startRowIndex, 15);
            //CommonUtil.SetGridRangeLockState(totalRange, true);
            projectPlanDetailOperate.Add(new FundPlanFlexGridOperate(startRowIndex, totalQuota, false, gdProjectExpend));
            CommonUtil.SetFlexGridColumnAutoFit(gdProjectExpend);
            //CommonUtil.SetGridColumnLockState(gdProjectExpend,
            //                                 new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14}, true);
        }

        public static int getCostType(string costTypeStr)
        {
            int costType = 0;
            if (costTypeStr.Contains("人工"))
            {
                costType = 1;
            }
            else if (costTypeStr.Contains("机械"))
            {
                costType = 2;
            }
            else if (costTypeStr.Contains("材料"))
            {
                costType = 3;
            }
            else if (costTypeStr.Contains("分包"))
            {
                costType = 4;
            }
            return costType;
        }

        /// <summary>
        /// 显示项目资金计划其他支出明细
        /// </summary>
        /// <param name="projectFundPlan"></param>
        /// <param name="gdOtherExpend"></param>
        /// <param name="flxName"></param>
        public static void DisplayOtherPlanDetail(ProjectFundPlanMaster projectFundPlan, CustomFlexGrid gdOtherExpend,
                                                  string flxName)
        {
            if (projectFundPlan == null)
            {
                return;
            }

            LoadFlexFile(flxName, gdOtherExpend);

            gdOtherExpend.Cell(2, 1).Text = string.Format("申报项目名称：	{0}", projectFundPlan.ProjectName);
            gdOtherExpend.Cell(2, 3).Text = "申报日期：" + projectFundPlan.CreateDate.ToString("yyyy-MM-dd");

            var startRowIndex = 4;
            var oldRow = startRowIndex;

            var details = projectFundPlan.OtherPayDetails.ToList();
            if (details.Count == 0)
            {
                return;
            }
            var gps = details.GroupBy(a => a.PayScope);

            var insertRow = gdOtherExpend.Rows - 1 - startRowIndex - details.Count() - gps.Count();
            if (insertRow < 0)
            {
                gdOtherExpend.InsertRow(startRowIndex, insertRow*-1);
            }
            else
            {
                gdOtherExpend.Rows = gdOtherExpend.Rows - insertRow;
            }

            CommonUtil.SetFlexGridBorder(gdOtherExpend, startRowIndex);
            gdOtherExpend.Range(startRowIndex, 4, gdOtherExpend.Rows - 1, 4).Alignment = AlignmentEnum.RightCenter;
            gdOtherExpend.Range(startRowIndex, 1, gdOtherExpend.Rows - 1, gdOtherExpend.Cols - 1).MergeCells = false;
            foreach (var gp in gps)
            {
                var gpDetails = gp.ToList();
                var seqNo = 1;
                foreach (ProjectOtherPayPlanDetail detail in gpDetails)
                {
                    var startColIndex = 1;

                    detail.OrderNumber = seqNo;

                    gdOtherExpend.Cell(startRowIndex, startColIndex++).Text = detail.PayScope;
                    gdOtherExpend.Cell(startRowIndex, startColIndex).Tag = string.IsNullOrEmpty(detail.Id)
                                                                               ? detail.TempData
                                                                               : detail.Id;

                    gdOtherExpend.Cell(startRowIndex, startColIndex++).Text = detail.OrderNumber.ToString();
                    gdOtherExpend.Cell(startRowIndex, startColIndex++).Text = detail.CostDetail;
                    gdOtherExpend.Cell(startRowIndex, startColIndex++).Text = detail.PlanDeclarePayment.ToString("N2");
                    gdOtherExpend.Cell(startRowIndex, startColIndex).Text = detail.Descript;

                    startRowIndex++;
                    seqNo++;
                }

                //小计
                gdOtherExpend.Cell(startRowIndex, 2).Text = XIAOJI;
                var range = gdOtherExpend.Range(startRowIndex, 2, startRowIndex, 3);
                range.Merge();
                range.Alignment = AlignmentEnum.CenterCenter;

                gdOtherExpend.Cell(startRowIndex, 4).Text = gpDetails.Sum(a => a.PlanDeclarePayment).ToString("N2");
                range = gdOtherExpend.Range(startRowIndex, 3, startRowIndex, 5);
                CommonUtil.SetGridRangeLockState(range, true);

                gdOtherExpend.Range(oldRow, 1, startRowIndex, 1).Merge();

                startRowIndex++;
                oldRow = startRowIndex;
            }

            //合计
            gdOtherExpend.Cell(startRowIndex, 1).Text = HEJI;
            var totalRange = gdOtherExpend.Range(startRowIndex, 1, startRowIndex, 3);
            totalRange.Merge();
            totalRange.Alignment = AlignmentEnum.CenterCenter;

            gdOtherExpend.Cell(startRowIndex, 4).Text = details.Sum(a => a.PlanDeclarePayment).ToString("N2");
            totalRange = gdOtherExpend.Range(startRowIndex, 3, startRowIndex, 5);
            CommonUtil.SetGridRangeLockState(totalRange, true);

            CommonUtil.SetGridColumnLockState(gdOtherExpend, new List<int>() { 1, 2 }, true);
        }
        /// <summary>
        /// 显示项目资金计划项目现金流
        /// </summary>
        /// <param name="projectFundPlan"></param>
        /// <param name="gdProjectReport"></param>
        public static void DisplayProjectPlanFundFlow(ProjectFundPlanMaster projectFundPlan, CustomFlexGrid gdProjectReport)
        {
            if (projectFundPlan == null)
            {
                return;
            }

            gdProjectReport.Cell(2, 1).Text = string.Format("申报项目名称：	{0}", projectFundPlan.ProjectName);
            gdProjectReport.Cell(2, 7).Text = string.Format("申报日期：{0}", projectFundPlan.CreateDate.ToString("yyyy-MM-dd"));

            var colIndex = 1;
            gdProjectReport.Cell(6, colIndex++).Text = projectFundPlan.FinanceConfirmTaxIncome.ToString("N2");
            gdProjectReport.Cell(6, colIndex++).Text = projectFundPlan.OwnerActualAffirmMeterage.ToString("N2");
            gdProjectReport.Cell(6, colIndex++).Text = projectFundPlan.ContractAccountsDue.ToString("N2");
            gdProjectReport.Cell(6, colIndex++).Text = projectFundPlan.CumulativeGathering.ToString("N2");
            gdProjectReport.Cell(6, colIndex++).Text = projectFundPlan.CumulativePayment.ToString("N2");
            gdProjectReport.Cell(6, colIndex++).Text = projectFundPlan.FundStock.ToString("N2");
            gdProjectReport.Cell(6, colIndex++).Text = projectFundPlan.ContractAppointGatheringRatio.ToString("P2");
            gdProjectReport.Cell(6, colIndex++).Text = projectFundPlan.ActualGatheringRatio.ToString("P2");
            gdProjectReport.Cell(6, colIndex++).Text = projectFundPlan.PresentMonthGathering.ToString("N2");
            gdProjectReport.Cell(6, colIndex++).Text = projectFundPlan.PresentMonthPayment.ToString("N2");
            gdProjectReport.Cell(6, colIndex).Text = projectFundPlan.MonthEndCumulativeFundStock.ToString("N2");

            gdProjectReport.Range(6, 1, 6, 11).Alignment = AlignmentEnum.RightCenter;
            CommonUtil.SetGridRangeLockState(gdProjectReport.Range(6, 2, 6, 8), true);
            CommonUtil.SetGridRangeLockState(gdProjectReport.Range(6, 10, 6, 11), true);
        }

        /// <summary>
        /// 显示项目资金计划申报明细
        /// </summary>
        /// <param name="projectFundPlan"></param>
        /// <param name="gdProjectReport"></param>
        public static void DisplayProjectReportDetail(ProjectFundPlanMaster projectFundPlan, CustomFlexGrid gdProjectReport)
        {
            if (projectFundPlan == null)
            {
                return;
            }

            var totals = projectFundPlan.Details.OfType<ProjectFundPlanDetail>()
                .GroupBy(t => t.FundPaymentCategory)
                .Select(g => new
                                 {
                                     FundPaymentCategory = g.Key,
                                     TotalSettlement = g.Sum(dt => dt.CumulativeSettlement),
                                     TotalPayment = g.Sum(dt => dt.CumulativePayment),
                                     TotalArrears = g.Sum(dt => dt.CumulativeArrears),
                                     TotalExpireDue = g.Sum(dt => dt.CumulativeExpireDue),
                                     TotalPlanPayment = g.Sum(dt => dt.PlanPayment),
                                     Remark =
                                 string.Join(";", g.TakeWhile(s => !string.IsNullOrEmpty(s.Descript))
                                                      .Select(s => s.Descript)
                                                      .ToArray())
                                 });

            var startRowIndex = 10;
            var insertRow = gdProjectReport.Rows - 1 - startRowIndex - totals.Count() - 1;
            if (insertRow < 0)
            {
                gdProjectReport.InsertRow(startRowIndex, insertRow * -1);
            }
            CommonUtil.SetGridRangeLockState(
                gdProjectReport.Range(startRowIndex, 1, gdProjectReport.Rows - 1, gdProjectReport.Cols - 1), false);
            var colIndex = 1;
            foreach (var total in totals)
            {
                colIndex = 1;
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.FundPaymentCategory;
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.TotalSettlement.ToString("N2");
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.TotalPayment.ToString("N2");
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.TotalArrears.ToString("N2");
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.TotalExpireDue.ToString("N2");
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.TotalPlanPayment.ToString("N2");
                gdProjectReport.Cell(startRowIndex, ++colIndex).Text = total.Remark;

                gdProjectReport.Range(startRowIndex, colIndex, startRowIndex, colIndex + 3).Merge();

                startRowIndex++;
            }
            //其他支出
            var totalOther = projectFundPlan.OtherPayDetails.Sum(a => a.PlanDeclarePayment);
            gdProjectReport.Cell(startRowIndex, 1).Text = "其他支出";
            gdProjectReport.Cell(startRowIndex, 6).Text = totalOther.ToString("N2");
            gdProjectReport.Cell(startRowIndex, 8).Text =
                string.Join(";",
                            projectFundPlan.OtherPayDetails
                                .TakeWhile(a => !string.IsNullOrEmpty(a.Descript))
                                .Select(a => a.Descript).ToArray());
            gdProjectReport.Range(startRowIndex, 8, startRowIndex, 11).Merge();
            startRowIndex++;

            //合计
            colIndex = 1;
            gdProjectReport.Cell(startRowIndex, colIndex++).Text = HEJI;
            gdProjectReport.Cell(startRowIndex, colIndex++).Text = totals.Sum(d => d.TotalSettlement).ToString("N2");
            gdProjectReport.Cell(startRowIndex, colIndex++).Text = totals.Sum(d => d.TotalPayment).ToString("N2");
            gdProjectReport.Cell(startRowIndex, colIndex++).Text = totals.Sum(d => d.TotalArrears).ToString("N2");
            gdProjectReport.Cell(startRowIndex, colIndex++).Text = totals.Sum(d => d.TotalExpireDue).ToString("N2");
            gdProjectReport.Cell(startRowIndex, colIndex).Text = (totals.Sum(d => d.TotalPlanPayment) + totalOther).ToString("N2");
            gdProjectReport.Range(startRowIndex, colIndex + 2, startRowIndex, colIndex + 5).Merge();

            gdProjectReport.Cell(10, 7).Text = projectFundPlan.ApprovalAmount.ToString("N2");
            gdProjectReport.Range(10, 7, startRowIndex, 7).Merge();
            gdProjectReport.Range(10, 2, startRowIndex, 7).Alignment = AlignmentEnum.RightCenter;

            CommonUtil.SetGridRangeLockState(gdProjectReport.Range(10, 1, startRowIndex, gdProjectReport.Cols - 1), true);
            CommonUtil.SetFlexGridBorder(gdProjectReport, 10);
            CommonUtil.SetFlexGridColumnAutoFit(gdProjectReport);
        }

        /// <summary>
        /// 显示分公司项目资金计划申报明细汇总，按项目名称汇总
        /// </summary>
        /// <param name="projDetails"></param>
        /// <param name="gdTotalExpend1"></param>
        /// <param name="flxName"></param>
        public static void DisplayFilialePlanProjectDetailsByName(IList projDetails, CustomFlexGrid gdTotalExpend1, string flxName)//单位资金支付计划申报汇总表1
        {
            if (projDetails == null)
            {
                return;
            }

            LoadFlexFile(flxName, gdTotalExpend1);

            gdTotalExpend1.Locked = false;

            var startIndex = 4;
            var colIndex = 1;
            var preRow = startIndex;
            var allDetails = new List<ProjectFundPlanDetail>();

            foreach (ProjectFundPlanMaster planMaster in projDetails)
            {
                gdTotalExpend1.Cell(2, 1).Text = "申报单位：" + planMaster.AttachBusinessOrgName;
                gdTotalExpend1.Cell(2, 4).Text = "申报日期：" + planMaster.CreateDate.ToString("yyyy-MM-dd");
                gdTotalExpend1.Cell(2, 4).WrapText = true;

                var detailTotals = planMaster.Details.OfType<ProjectFundPlanDetail>()
                    .GroupBy(a => a.FundPaymentCategory)
                    .Select(b => new
                                     {
                                         FundPaymentCategory = b.Key,
                                         TotalSettlement = b.Sum(v => v.CumulativeSettlement),
                                         TotalPayment = b.Sum(v => v.CumulativePayment),
                                         TotalArrears = b.Sum(v => v.CumulativeArrears),
                                         TotalPlanPayment = b.Sum(v => v.PlanPayment)
                                     });

                if (detailTotals.Count() == 0)
                {
                    continue;
                }
                
                allDetails.AddRange(planMaster.Details.OfType<ProjectFundPlanDetail>());

                gdTotalExpend1.InsertRow(startIndex, detailTotals.Count() + 1);
                gdTotalExpend1.Range(startIndex, 1, gdTotalExpend1.Rows - 1, gdTotalExpend1.Cols - 1).FontBold = false;

                foreach (var detailTotal in detailTotals)
                {
                    colIndex = 1;
                    gdTotalExpend1.Cell(startIndex, colIndex++).Text = planMaster.ProjectName;
                    gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotal.FundPaymentCategory;
                    gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotal.TotalSettlement.ToString("N2");
                    gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotal.TotalPayment.ToString("N2");
                    gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotal.TotalArrears.ToString("N2");
                    var payRate = detailTotal.TotalSettlement == 0
                                      ? 0m
                                      : detailTotal.TotalPayment/detailTotal.TotalSettlement;
                    gdTotalExpend1.Cell(startIndex, colIndex++).Text = payRate.ToString("P2");
                    gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotal.TotalPlanPayment.ToString("N2");
                    payRate = detailTotal.TotalSettlement == 0
                                  ? 0m
                                  : (detailTotal.TotalPayment + detailTotal.TotalPayment)/detailTotal.TotalSettlement;
                    gdTotalExpend1.Cell(startIndex, colIndex).Text = payRate.ToString("P2");

                    startIndex++;
                }

                //小计
                colIndex = 1;
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = planMaster.ProjectName;
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = XIAOJI;
                var tmp1 = detailTotals.Sum(a => a.TotalSettlement);
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = tmp1.ToString("N2");
                var tmp2 = detailTotals.Sum(a => a.TotalPayment);
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = tmp2.ToString("N2");
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotals.Sum(a => a.TotalArrears).ToString("N2");
                var tmp3 = tmp1 == 0 ? 0 : tmp2/tmp1;
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = tmp3.ToString("P2");
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotals.Sum(a => a.TotalPlanPayment).ToString("N2");
                tmp3 = tmp1 == 0 ? 0 : (tmp2 + detailTotals.Sum(a => a.TotalPlanPayment)) / tmp1;
                gdTotalExpend1.Cell(startIndex, colIndex).Text = tmp3.ToString("P2");

                gdTotalExpend1.Range(startIndex, 1, startIndex, colIndex).FontBold = true;
                gdTotalExpend1.Range(preRow, 1, startIndex, 1).Merge();
                gdTotalExpend1.Range(preRow, 3, startIndex, colIndex).Alignment = AlignmentEnum.RightCenter;

                startIndex++;
                preRow = startIndex;
            }

            //分公司机关费用

            //合计
            var totals = allDetails
                .GroupBy(a => a.FundPaymentCategory)
                .Select(b => new
                                 {
                                     FundPaymentCategory = b.Key,
                                     TotalSettlement = b.Sum(v => v.CumulativeSettlement),
                                     TotalPayment = b.Sum(v => v.CumulativePayment),
                                     TotalArrears = b.Sum(v => v.CumulativeArrears),
                                     TotalPlanPayment = b.Sum(v => v.PlanPayment)
                                 });
            gdTotalExpend1.InsertRow(startIndex, totals.Count() + 1);
            gdTotalExpend1.Range(startIndex, 1, gdTotalExpend1.Rows - 1, gdTotalExpend1.Cols - 1).FontBold = false;
            foreach (var detailTotal in totals)
            {
                colIndex = 1;
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = HEJI;
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotal.FundPaymentCategory;
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotal.TotalSettlement.ToString("N2");
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotal.TotalPayment.ToString("N2");
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotal.TotalArrears.ToString("N2");
                var payRate = detailTotal.TotalSettlement == 0
                                  ? 0m
                                  : detailTotal.TotalPayment / detailTotal.TotalSettlement;
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = payRate.ToString("P2");
                gdTotalExpend1.Cell(startIndex, colIndex++).Text = detailTotal.TotalPlanPayment.ToString("N2");
                payRate = detailTotal.TotalSettlement == 0
                              ? 0m
                              : (detailTotal.TotalPayment + detailTotal.TotalPayment) / detailTotal.TotalSettlement;
                gdTotalExpend1.Cell(startIndex, colIndex).Text = payRate.ToString("P2");

                startIndex++;
            }

            //合计汇总
            colIndex = 1;
            gdTotalExpend1.Cell(startIndex, colIndex++).Text = HEJI;
            gdTotalExpend1.Cell(startIndex, colIndex++).Text = XIAOJI;
            var tmp11 = totals.Sum(a => a.TotalSettlement);
            gdTotalExpend1.Cell(startIndex, colIndex++).Text = tmp11.ToString("N2");
            var tmp21 = totals.Sum(a => a.TotalPayment);
            gdTotalExpend1.Cell(startIndex, colIndex++).Text = tmp21.ToString("N2");
            gdTotalExpend1.Cell(startIndex, colIndex++).Text = totals.Sum(a => a.TotalArrears).ToString("N2");
            var tmp31 = tmp11 == 0 ? 0 : tmp21 / tmp11;
            gdTotalExpend1.Cell(startIndex, colIndex++).Text = tmp31.ToString("P2");
            gdTotalExpend1.Cell(startIndex, colIndex++).Text = totals.Sum(a => a.TotalPlanPayment).ToString("N2");
            tmp31 = tmp11 == 0 ? 0 : (tmp21 + totals.Sum(a => a.TotalPlanPayment)) / tmp11;
            gdTotalExpend1.Cell(startIndex, colIndex).Text = tmp31.ToString("P2");

            gdTotalExpend1.Range(startIndex, 1, startIndex, colIndex).FontBold = true;
            gdTotalExpend1.Range(preRow, 1, startIndex, 1).Merge();
            gdTotalExpend1.Range(preRow, 3, startIndex, colIndex).Alignment = AlignmentEnum.RightCenter;

            CommonUtil.SetFlexGridColumnAutoFit(gdTotalExpend1);
            CommonUtil.SetFlexGridBorder(gdTotalExpend1, 4);
            CommonUtil.SetGridRangeLockState(gdTotalExpend1.Range
                (4, 1, gdTotalExpend1.Rows - 1, gdTotalExpend1.Cols - 1), true);
        }

        private static decimal ComputeColumnTotalValue(CustomFlexGrid grid, int startRow, int endRow, int colIndex, int totalColIndex)
        {
            decimal totalVal = 0;
            for (int i = startRow; i < endRow; i++)
            {
                var txt = grid.Cell(i, totalColIndex).Text.Trim().Replace(" ", "");
                if (txt == XIAOJI)
                {
                    continue;
                }

                txt = grid.Cell(i, colIndex).Text.Trim();
                var tmp = 0m;
                if (decimal.TryParse(txt, out tmp))
                {
                    totalVal += tmp;
                }
            }

            return totalVal;
        }

        /// <summary>
        /// 显示分公司项目资金计划申报明细汇总，按项目状态汇总
        /// </summary>
        /// <param name="projDetails"></param>
        /// <param name="gdTotalExpend2"></param>
        /// <param name="flxName"></param>
        public static void DisplayFilialePlanProjectDetailsByState(IList projDetails, CustomFlexGrid gdTotalExpend2, string flxName)
        {
            if (projDetails == null)
            {
                return;
            }

            LoadFlexFile(flxName, gdTotalExpend2);
            gdTotalExpend2.Locked = false;
            TotalExpend2Operate.Clear();

            if (projDetails.Count > 0)
            {
                ProjectFundPlanMaster oneMaster = projDetails.OfType<ProjectFundPlanMaster>().ElementAt(0);
                gdTotalExpend2.Cell(2, 1).Text = "申报单位：" + oneMaster.AttachBusinessOrgName;
                gdTotalExpend2.Cell(2, 4).Text = "申报日期：" + oneMaster.CreateDate.ToString("yyyy-MM-dd");
            }

            var startIndex = 5;
            var preRow = startIndex;
            var colIndex = 1;
            var projStates = projDetails.OfType<ProjectFundPlanMaster>().GroupBy(a => a.ProjectState);
            gdTotalExpend2.InsertRow(startIndex, projStates.Count() + projDetails.Count);

            foreach (var projState in projStates)
            {
                var plans = projState.ToList();
                foreach (var projPlan in plans)
                {
                    colIndex = 1;
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text = projState.Key;
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text = projPlan.ProjectName;
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text = projPlan.FinanceConfirmTaxIncome.ToString("N2");
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text = projPlan.OwnerActualAffirmMeterage.ToString("N2");
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text = projPlan.CumulativeGathering.ToString("N2");

                    var details = projPlan.Details.OfType<ProjectFundPlanDetail>();
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text =
                        details.Sum(a => a.CumulativePayment).ToString("N2");
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text = projPlan.FundStock.ToString("N2");
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text =
                        details.Sum(a => a.CumulativeArrears).ToString("N2");
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text =
                        details.Sum(a => a.CumulativeExpireDue).ToString("N2");
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text = projPlan.PresentMonthGathering.ToString("N2");
                    gdTotalExpend2.Cell(startIndex, colIndex++).Text = details.Sum(a => a.PlanPayment).ToString("N2");

                    var stepPlan = 0m;
                    if (projPlan.ProjectType == 2)
                    {
                        stepPlan = details.Sum(a => a.PlanPayment);
                    }
                    gdTotalExpend2.Cell(startIndex, colIndex).Text = stepPlan.ToString("N2");
                    TotalExpend2Operate.Add(new FundPlanFlexGridOperate(projPlan.Id, startIndex, projPlan.ApprovalAmount,
                                                                        true, gdTotalExpend2));
                    startIndex++;
                }

                //小计
                colIndex = 1;
                gdTotalExpend2.Cell(startIndex, colIndex++).Text = projState.Key;
                gdTotalExpend2.Cell(startIndex, colIndex++).Text = XIAOJI;
                for (int i = colIndex; i < gdTotalExpend2.Cols; i++)
                {
                    gdTotalExpend2.Cell(startIndex, i).Text =
                        ComputeColumnTotalValue(gdTotalExpend2, preRow, startIndex, i, colIndex - 1).ToString("N2");
                }

                gdTotalExpend2.Range(preRow, 1, startIndex, 1).Merge();

                startIndex++;
                preRow = startIndex;
            }

            //合计
            colIndex = 1;
            gdTotalExpend2.Cell(startIndex, colIndex++).Text = HEJI;
            gdTotalExpend2.Cell(startIndex, colIndex++).Text = HEJI;
            for (int i = colIndex; i < gdTotalExpend2.Cols; i++)
            {
                gdTotalExpend2.Cell(startIndex, i).Text =
                    ComputeColumnTotalValue(gdTotalExpend2, 5, startIndex, i, colIndex - 1).ToString("N2");
            }

            var range = gdTotalExpend2.Range(startIndex, 1, startIndex, 2);
            range.Merge();
            range.Alignment = AlignmentEnum.CenterCenter;
            gdTotalExpend2.Range(5, 3, startIndex, gdTotalExpend2.Cols - 1).Alignment = AlignmentEnum.RightCenter;
            CommonUtil.SetFlexGridColumnAutoFit(gdTotalExpend2);
            CommonUtil.SetFlexGridBorder(gdTotalExpend2, 5);
        }

        /// <summary>
        /// 显示分公司资金计划申报明细
        /// </summary>
        /// <param name="filialeFundPlan"></param>
        /// <param name="gdCompanyReport"></param>
        public static void DisplayFilialePlanReportDetail(FilialeFundPlanMaster filialeFundPlan, CustomFlexGrid gdCompanyReport)
        {
            if (filialeFundPlan == null)
            {
                return;
            }

            var details = filialeFundPlan.Details.OfType<FilialeFundPlanDetail>().ToList();
            var startRowIndex = 14;
            gdCompanyReport.Locked = false;
            var insertRow = gdCompanyReport.Rows - 1 - startRowIndex - details.Count() - 1;
            if (insertRow < 0)
            {
                gdCompanyReport.InsertRow(startRowIndex, insertRow * -1);
            }
            CommonUtil.SetGridRangeLockState(
                gdCompanyReport.Range(startRowIndex, 1, gdCompanyReport.Rows - 1, gdCompanyReport.Cols - 1), false);

            var colIndex = 1;
            foreach (var dt in details)
            {
                colIndex = 1;
                gdCompanyReport.Cell(startRowIndex, colIndex++).Text = dt.ProjectCategory;
                gdCompanyReport.Cell(startRowIndex, colIndex++).Text = dt.CumulativeSettlement.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, colIndex++).Text = dt.CumulativePayment.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, colIndex++).Text = dt.CumulativeArrears.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, colIndex++).Text = dt.CumulativeExpireDue.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, colIndex++).Text = dt.PresentMonthPlanPayment.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, colIndex++).Text = dt.ThisMonthInstallFilialePayment.ToString("N2");
                gdCompanyReport.Cell(startRowIndex, ++colIndex).Text = dt.Descript;

                startRowIndex++;
            }
            //分公司机关
            gdCompanyReport.Cell(startRowIndex, 1).Text = "分公司机关";
            var totalOfficeExpend = filialeFundPlan.OfficeFundPlanDetails.Sum(a => a.PlanDeclarePayment);
            gdCompanyReport.Cell(startRowIndex, 6).Text = totalOfficeExpend.ToString("N2");
            startRowIndex++;

            //合计
            colIndex = 1;
            gdCompanyReport.Cell(startRowIndex, colIndex++).Text = HEJI;
            gdCompanyReport.Cell(startRowIndex, colIndex++).Text = details.Sum(d => d.CumulativeSettlement).ToString("N2");
            gdCompanyReport.Cell(startRowIndex, colIndex++).Text = details.Sum(d => d.CumulativePayment).ToString("N2");
            gdCompanyReport.Cell(startRowIndex, colIndex++).Text = details.Sum(d => d.CumulativeArrears).ToString("N2");
            gdCompanyReport.Cell(startRowIndex, colIndex++).Text = details.Sum(d => d.CumulativeExpireDue).ToString("N2");
            gdCompanyReport.Cell(startRowIndex, colIndex++).Text = (details.Sum(d => d.PresentMonthPlanPayment) + totalOfficeExpend).ToString("N2");
            gdCompanyReport.Cell(startRowIndex, colIndex).Text = details.Sum(d => d.ThisMonthInstallFilialePayment).ToString("N2");

            gdCompanyReport.Cell(14, 8).Text = filialeFundPlan.Approval.ToString("N2");
            gdCompanyReport.Range(14, 8, startRowIndex, 8).Merge();
            gdCompanyReport.Range(14, 2, startRowIndex, 8).Alignment = AlignmentEnum.RightCenter;

            CommonUtil.SetGridRangeLockState(gdCompanyReport.Range(14, 1, startRowIndex, gdCompanyReport.Cols - 1), true);
            CommonUtil.SetFlexGridBorder(gdCompanyReport, 14);
            CommonUtil.SetFlexGridColumnAutoFit(gdCompanyReport);
        }

        /// <summary>
        /// 显示项目资金计划申报明细
        /// </summary>
        /// <param name="projectFundPlan"></param>
        /// <param name="gdProjectReport"></param>
        public static void DisplayProjectReportDetail(ProjectFundPlanMaster projectFundPlan, CustomFlexGrid gdProjectReport,string flxName)
        {
            if (projectFundPlan == null)
            {
                return;
            }
            LoadFlexFile(flxName, gdProjectReport);

            gdProjectReport.Cell(2, 1).Text = string.Format("申报项目名称：	{0}", projectFundPlan.ProjectName);
            gdProjectReport.Cell(2, 7).Text = string.Format("申报日期：{0}", projectFundPlan.CreateDate.ToString("yyyy-MM-dd"));
            var colPlanFundFlowIndex = 1;
            gdProjectReport.Cell(6, colPlanFundFlowIndex++).Text = projectFundPlan.FinanceConfirmTaxIncome.ToString("N2");
            gdProjectReport.Cell(6, colPlanFundFlowIndex++).Text = projectFundPlan.OwnerActualAffirmMeterage.ToString("N2");
            gdProjectReport.Cell(6, colPlanFundFlowIndex++).Text = projectFundPlan.ContractAccountsDue.ToString("N2");
            gdProjectReport.Cell(6, colPlanFundFlowIndex++).Text = projectFundPlan.CumulativeGathering.ToString("N2");
            gdProjectReport.Cell(6, colPlanFundFlowIndex++).Text = projectFundPlan.CumulativePayment.ToString("N2");
            gdProjectReport.Cell(6, colPlanFundFlowIndex++).Text = projectFundPlan.FundStock.ToString("N2");
            gdProjectReport.Cell(6, colPlanFundFlowIndex++).Text = projectFundPlan.ContractAppointGatheringRatio.ToString("P2");
            gdProjectReport.Cell(6, colPlanFundFlowIndex++).Text = projectFundPlan.ActualGatheringRatio.ToString("P2");
            gdProjectReport.Cell(6, colPlanFundFlowIndex++).Text = projectFundPlan.PresentMonthGathering.ToString("N2");
            gdProjectReport.Cell(6, colPlanFundFlowIndex++).Text = projectFundPlan.PresentMonthPayment.ToString("N2");
            gdProjectReport.Cell(6, colPlanFundFlowIndex).Text = projectFundPlan.MonthEndCumulativeFundStock.ToString("N2");

            gdProjectReport.Range(6, 1, 6, 11).Alignment = AlignmentEnum.RightCenter;
            CommonUtil.SetGridRangeLockState(gdProjectReport.Range(6, 2, 6, 8), true);
            CommonUtil.SetGridRangeLockState(gdProjectReport.Range(6, 10, 6, 11), true);


            var totals = projectFundPlan.Details.OfType<ProjectFundPlanDetail>()
                .GroupBy(t => t.FundPaymentCategory)
                .Select(g => new
                {
                    FundPaymentCategory = g.Key,
                    TotalSettlement = g.Sum(dt => dt.CumulativeSettlement),
                    TotalPayment = g.Sum(dt => dt.CumulativePayment),
                    TotalArrears = g.Sum(dt => dt.CumulativeArrears),
                    TotalExpireDue = g.Sum(dt => dt.CumulativeExpireDue),
                    TotalPlanPayment = g.Sum(dt => dt.PlanPayment),
                    Remark =
                string.Join(";", g.TakeWhile(s => !string.IsNullOrEmpty(s.Descript))
                                     .Select(s => s.Descript)
                                     .ToArray())
                });

            var startRowIndex = 10;
            var insertRow = gdProjectReport.Rows - 1 - startRowIndex - totals.Count() - 1;
            if (insertRow < 0)
            {
                gdProjectReport.InsertRow(startRowIndex, insertRow * -1);
            }
            CommonUtil.SetGridRangeLockState(
                gdProjectReport.Range(startRowIndex, 1, gdProjectReport.Rows - 1, gdProjectReport.Cols - 1), false);
            var colIndex = 1;
            foreach (var total in totals)
            {
                colIndex = 1;
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.FundPaymentCategory;
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.TotalSettlement.ToString("N2");
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.TotalPayment.ToString("N2");
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.TotalArrears.ToString("N2");
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.TotalExpireDue.ToString("N2");
                gdProjectReport.Cell(startRowIndex, colIndex++).Text = total.TotalPlanPayment.ToString("N2");
                gdProjectReport.Cell(startRowIndex, ++colIndex).Text = total.Remark;

                gdProjectReport.Range(startRowIndex, colIndex, startRowIndex, colIndex + 1).Merge();
                gdProjectReport.Range(startRowIndex, colIndex + 2, startRowIndex, colIndex + 3).Merge();
                startRowIndex++;
            }
            gdProjectReport.Range(10, 7, startRowIndex + 1, 7).Merge();
            //其他支出
            var totalOther = projectFundPlan.OtherPayDetails.Sum(a => a.PlanDeclarePayment);
            gdProjectReport.Cell(startRowIndex, 1).Text = "其他支出";
            gdProjectReport.Cell(startRowIndex, 6).Text = totalOther.ToString("N2");
            gdProjectReport.Cell(startRowIndex, 8).Text =
                string.Join(";",
                            projectFundPlan.OtherPayDetails
                                .TakeWhile(a => !string.IsNullOrEmpty(a.Descript))
                                .Select(a => a.Descript).ToArray());
            gdProjectReport.Range(startRowIndex, 8, startRowIndex, 9).Merge();
            gdProjectReport.Range(startRowIndex, 10, startRowIndex, 11).Merge();
            startRowIndex++;

            //合计
            colIndex = 1;
            gdProjectReport.Cell(startRowIndex, colIndex++).Text = HEJI;
            gdProjectReport.Cell(startRowIndex, colIndex++).Text = totals.Sum(d => d.TotalSettlement).ToString("N2");
            gdProjectReport.Cell(startRowIndex, colIndex++).Text = totals.Sum(d => d.TotalPayment).ToString("N2");
            gdProjectReport.Cell(startRowIndex, colIndex++).Text = totals.Sum(d => d.TotalArrears).ToString("N2");
            gdProjectReport.Cell(startRowIndex, colIndex++).Text = totals.Sum(d => d.TotalExpireDue).ToString("N2");
            gdProjectReport.Cell(startRowIndex, colIndex).Text = (totals.Sum(d => d.TotalPlanPayment) + totalOther).ToString("N2");

            gdProjectReport.Cell(10, 7).Text = projectFundPlan.ApprovalAmount.ToString("N2");
            gdProjectReport.Range(10, 2, startRowIndex, 7).Alignment = AlignmentEnum.RightCenter;

            CommonUtil.SetGridRangeLockState(gdProjectReport.Range(10, 1, startRowIndex, gdProjectReport.Cols - 3), true);
            CommonUtil.SetFlexGridBorder(gdProjectReport, 10);
            CommonUtil.SetFlexGridColumnAutoFit(gdProjectReport);
        }
    }
}
