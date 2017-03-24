using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using FlexCell;
using NHibernate;
using NHibernate.Criterion;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan
{
    public partial class VFundAssessmentQuery : TBasicDataView
    {
        private const int REPORT_UNIT = 10000;
        private CurrentProjectInfo currentProject;
        private MFinanceMultData mOperate;

        public VFundAssessmentQuery()
        {
            InitializeComponent();

            InitData();

            InitEvents();
        }

        private void InitData()
        {
            currentProject = StaticMethod.GetProjectInfo();
            if (currentProject != null && !currentProject.Code.Equals(CommonUtil.CompanyProjectCode))
            {
                txtOperationOrg.Text = ConstObject.TheOperationOrg.Name;
                txtOperationOrg.Tag = ConstObject.TheOperationOrg;
                btnOperationOrg.Visible = false;
            }
            else
            {
                currentProject = null;
            }

            mOperate = new MFinanceMultData();

            InitGrid();

            dtpBegin.Value = DateTime.Now.AddMonths(-3);
        }

        private void InitGrid()
        {
            FundPlanOperate.LoadFlexFile(string.Concat(gdAssesscashDetail.Tag, ".flx"), gdAssesscashDetail);

            FundPlanOperate.LoadFlexFile(string.Concat(gdInterestDetail.Tag, ".flx"), gdInterestDetail);

            gdAssesscashDetail.Locked = gdInterestDetail.Locked = true;
            gdAssesscashDetail.DefaultRowHeight = gdInterestDetail.DefaultRowHeight = 25;
        }

        private void InitEvents()
        {
            btnOperationOrg.Click += btnOperationOrg_Click;
            btnQuery.Click += new EventHandler(btnQuery_Click);
        }

        private void DisplayResult(IList  queryResult)
        {
            InitGrid();

            var assessCashQueryResult = queryResult.OfType<FundAssessmentMaster>().ToList().FindAll(a => a.Code.Contains("考核兑现"));
            var minDate = assessCashQueryResult.Min(a => a.QueryDate);
            var maxDate = assessCashQueryResult.Max(a => a.QueryDate);

            var comp = string.Format("计算期间：{0}年{1}月-{2}年{3}月", minDate.Year, minDate.Month, maxDate.Year, maxDate.Month);
            gdAssesscashDetail.Cell(2, 1).Text = comp;

            var insertAssesscashColCount = assessCashQueryResult.Count;
            gdAssesscashDetail.Locked = false;
            gdAssesscashDetail.InsertCol(gdAssesscashDetail.Cols - 1, insertAssesscashColCount - 1);
            gdAssesscashDetail.Range(2, 1, 2, gdAssesscashDetail.Cols - 2).Merge();
            gdAssesscashDetail.Range(3, 2, 3, gdAssesscashDetail.Cols - 1).Merge();
            gdAssesscashDetail.Locked = true;

            for (var i = 0; i < insertAssesscashColCount; i++)
            {
                var currentMaster = assessCashQueryResult[i];
                if (currentMaster == null)
                {
                    continue;
                }
                var rIndex = 3;
                var cAssesscashIndex = 2 + i;

                #region 资金考核兑现
                gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = currentMaster.ProjectName;
                gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = currentMaster.ProjectState;
                gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = string.Format("{0}月", currentMaster.CreateMonth);
                gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = (currentMaster.CurrentSchemeTarget / REPORT_UNIT).ToString("N4");
                gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = (currentMaster.CurrentCashBalance / REPORT_UNIT).ToString("N4");

                var assDetail = currentMaster.AssessCashDetails.FirstOrDefault();
                if (assDetail != null)
                {
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text =
                        (assDetail.CentralPurchase + assDetail.InnerInstall
                         + assDetail.OtherContractPay + assDetail.OtherAdjust / REPORT_UNIT).ToString("N4");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = (assDetail.CentralPurchase / REPORT_UNIT).ToString("N4");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = (assDetail.InnerInstall / REPORT_UNIT).ToString("N4");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = (assDetail.OtherContractPay / REPORT_UNIT).ToString("N4");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = (assDetail.OtherAdjust / REPORT_UNIT).ToString("N4");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = (assDetail.RealCashBalance / REPORT_UNIT).ToString("N4");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = (assDetail.AssessCardinal / REPORT_UNIT).ToString("N4");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = (assDetail.CashMoney / REPORT_UNIT).ToString("N4");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = (assDetail.DeductionItem / REPORT_UNIT).ToString("N4");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = assDetail.WarnRate.ToString("P2");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = assDetail.WarnLevel;
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = assDetail.ApprovalDeduction.ToString("P2");
                    gdAssesscashDetail.Cell(rIndex++, cAssesscashIndex).Text = assDetail.ApprovalRate.ToString("P2");
                    gdAssesscashDetail.Cell(rIndex, cAssesscashIndex).Text = (assDetail.AssessCashMoney / REPORT_UNIT).ToString("N4");
                }

                gdAssesscashDetail.Column(cAssesscashIndex).AutoFit();
                gdAssesscashDetail.Range(3, cAssesscashIndex, rIndex, cAssesscashIndex).FontBold = false;
                gdAssesscashDetail.InsertCol(cAssesscashIndex, 1);
                #endregion
            }

            var interestComputeQueryResult = queryResult.OfType<FundAssessmentMaster>().ToList().FindAll(a => a.Code.Contains("利息计算"));
            minDate = assessCashQueryResult.Min(a => a.QueryDate);
            maxDate = assessCashQueryResult.Max(a => a.QueryDate);

            comp = string.Format("计算期间：{0}年{1}月-{2}年{3}月", minDate.Year, minDate.Month, maxDate.Year, maxDate.Month);
            gdInterestDetail.Cell(2, 1).Text = comp;

            var insertInterestComputeColCount = interestComputeQueryResult.Count;
            gdInterestDetail.Locked = false;
            gdInterestDetail.InsertCol(gdInterestDetail.Cols - 1, insertInterestComputeColCount - 1);
            gdInterestDetail.Range(2, 1, 2, gdInterestDetail.Cols - 2).Merge();
            gdInterestDetail.Range(3, 2, 3, gdInterestDetail.Cols - 1).Merge();
            gdInterestDetail.Locked = true;
            for (var i = 0; i < insertInterestComputeColCount; i++)
            {
                var currentMaster = interestComputeQueryResult[i];
                if (currentMaster == null)
                {
                    continue;
                }
                var rIndex = 3;
                var cIndex = 2+i;

                #region 资金计息
                gdInterestDetail.Cell(rIndex++, cIndex).Text = currentMaster.ProjectName;
                gdInterestDetail.Cell(rIndex++, cIndex).Text = string.Format("{0}月", currentMaster.CreateMonth);
                gdInterestDetail.Cell(rIndex++, cIndex).Text = currentMaster.ProjectState;
                gdInterestDetail.Cell(rIndex++, cIndex).Text = (currentMaster.CurrentSchemeTarget / REPORT_UNIT).ToString("N4");
                gdInterestDetail.Cell(rIndex++, cIndex).Text = (currentMaster.CurrentCashBalance / REPORT_UNIT).ToString("N4");

                var interestDetail = currentMaster.Details.FirstOrDefault() as FundInterestDetail;
                if (interestDetail != null)
                {
                    gdInterestDetail.Cell(rIndex++, cIndex).Text = (interestDetail.CashBalanceInScheme / REPORT_UNIT).ToString("N4");
                    gdInterestDetail.Cell(rIndex++, cIndex).Text = (interestDetail.CashBorrowInScheme / REPORT_UNIT).ToString("N4");
                    gdInterestDetail.Cell(rIndex++, cIndex).Text = (interestDetail.CashBalanceOutScheme / REPORT_UNIT).ToString("N4");
                    gdInterestDetail.Cell(rIndex++, cIndex).Text = (interestDetail.CashBorrowOutScheme / REPORT_UNIT).ToString("N4");
                    gdInterestDetail.Cell(rIndex++, cIndex).Text = (interestDetail.InterestCost / REPORT_UNIT).ToString("N4");
                    gdInterestDetail.Cell(rIndex++, cIndex).Text = (interestDetail.SettlementMoney / REPORT_UNIT).ToString("N4");
                    gdInterestDetail.Cell(rIndex++, cIndex).Text = currentMaster.GatheringRate.ToString("P2");
                    gdInterestDetail.Cell(rIndex++, cIndex).Text = (currentMaster.CurrentRealGet / REPORT_UNIT).ToString("N4");
                    gdInterestDetail.Cell(rIndex++, cIndex).Text = (interestDetail.ReceivableDebt / REPORT_UNIT).ToString("N4");
                    gdInterestDetail.Cell(rIndex, cIndex).Text = (interestDetail.CompleteInterestCost / REPORT_UNIT).ToString("N4");

                }
                gdInterestDetail.Column(cIndex).AutoFit();
                gdInterestDetail.Range(3, cIndex, rIndex, cIndex).FontBold = false;
                #endregion
            }

            #region 资金计息格式调整
            var range = gdInterestDetail.Range(3, 1, gdInterestDetail.Rows - 1, gdInterestDetail.Cols - 1);
            range.set_Borders(EdgeEnum.Inside, LineStyleEnum.Thin);

            range = gdInterestDetail.Range(3, 2, 5, gdInterestDetail.Cols - 1);
            range.Alignment = AlignmentEnum.CenterCenter;
            range.FontSize = 11;

            range = gdInterestDetail.Range(6, 2, gdInterestDetail.Rows - 1, gdInterestDetail.Cols - 1);
            range.Alignment = AlignmentEnum.RightCenter;
            range.FontSize = 11;
            #endregion

            #region 资金考核兑现格式调整
            range = gdAssesscashDetail.Range(3, 1, gdAssesscashDetail.Rows - 1, gdAssesscashDetail.Cols - 1);
            range.set_Borders(EdgeEnum.Inside, LineStyleEnum.Thin);

            range = gdAssesscashDetail.Range(3, 2, 5, gdAssesscashDetail.Cols - 1);
            range.Alignment = AlignmentEnum.CenterCenter;
            range.FontSize = 11;

            range = gdAssesscashDetail.Range(6, 2, gdAssesscashDetail.Rows - 1, gdAssesscashDetail.Cols - 1);
            range.Alignment = AlignmentEnum.RightCenter;
            range.FontSize = 11;
            #endregion
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            if (currentProject == null)
            {
                MessageBox.Show("请选择项目");
                return;
            }

            var bgDate = new DateTime(dtpBegin.Value.Year, dtpBegin.Value.Month, 1);
            var endDate = new DateTime(dtpEnd.Value.Year, dtpEnd.Value.Month, 1);
            if (endDate < bgDate)
            {
                MessageBox.Show("查询结束期间不能小于开始期间，请重新选择");
                return;
            }

            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", currentProject.Id));
            objQuery.AddCriterion(Expression.Ge("QueryDate", bgDate));
            objQuery.AddCriterion(Expression.Le("QueryDate", endDate));
            objQuery.AddFetchMode("Details", FetchMode.Eager);
            objQuery.AddFetchMode("AssessCashDetails", FetchMode.Eager);
            objQuery.AddOrder(new Order("CreateYear", true));
            objQuery.AddOrder(new Order("CreateMonth", true));

            var list = mOperate.FinanceMultDataSrv.Query(typeof (FundAssessmentMaster), objQuery);
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("没有符合查询条件的记录");
                return;
            }

            DisplayResult(list);
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

                var dm = mOperate.CurrentProjectSrv.GetProjectInfoByOpgId(selOrg.Id);
                if (dm != null)
                {
                    currentProject = new CurrentProjectInfo();
                    currentProject.Id = TransUtil.ToString(dm.Name1);
                    currentProject.Name = TransUtil.ToString(dm.Name2);
                    currentProject.ProjectCurrState = TransUtil.ToInt(dm.Name3);
                }
            }
        }
    }
}
