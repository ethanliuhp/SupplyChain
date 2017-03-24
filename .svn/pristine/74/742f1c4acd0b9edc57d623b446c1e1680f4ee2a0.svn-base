using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.OrganizationResource;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FundPlan;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using FlexCell;
using NHibernate;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public partial class VFundSchemeEfficiency : TBasicDataView
    {
        private CurrentProjectInfo currentProject;
        private MFinanceMultData mOperate;
        private Color editColor;
        private FundSchemeEfficiencyMaster currentEfficiency;

        public VFundSchemeEfficiency()
        {
            InitializeComponent();

            InitEvents();

            InitData();

            InitGrid();
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

            editColor = ColorTranslator.FromHtml("#D2FBD6");
        }

        private void InitGrid()
        {
            FundPlanOperate.LoadFlexFile(string.Concat(gdReport.Tag, ".flx"), gdReport);

            gdReport.DefaultRowHeight = 25;
            gdReport.Locked = true;
            gdReport.FrozenRows = 5;
            gdReport.FrozenCols = 1;
        }

        private void InitEvents()
        {
            btnOperationOrg.Click += new EventHandler(btnOperationOrg_Click);
            btnQuery.Click += new EventHandler(btnQuery_Click);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnRebuild.Click += new EventHandler(btnRebuild_Click);

            gdReport.CellChange += new Grid.CellChangeEventHandler(gdReport_CellChange);
        }

        private FundPlanningMaster GetScheme()
        {
            ObjectQuery objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", currentProject.Id));
            objQuery.AddCriterion(Expression.Eq("DocState", DocumentState.InExecute));
            objQuery.AddCriterion(Expression.Le("SchemeBeginDate", new DateTime(dtpYear.Value.Year, 1, 1)));
            objQuery.AddCriterion(Expression.Ge("SchemeEndDate", new DateTime(dtpYear.Value.Year, 12, 31)));
            objQuery.AddFetchMode("CostCalculationDtl", FetchMode.Eager);
            objQuery.AddFetchMode("FundSummaryDtl", FetchMode.Eager);

            var list = mOperate.FinanceMultDataSrv.Query(typeof (FundPlanningMaster), objQuery);
            if (list == null || list.Count == 0)
            {
                return null;
            }
            else
            {
                return list[0] as FundPlanningMaster;
            }
        }

        private void NewEfficiencyMaster()
        {
            if (currentEfficiency == null)
            {
                currentEfficiency = new FundSchemeEfficiencyMaster();
                currentEfficiency.ProjectId = currentProject.Id;
                currentEfficiency.ProjectName = currentProject.Name;
                currentEfficiency.CreateYear = dtpYear.Value.Year;
                currentEfficiency.CreateDate = DateTime.Now;
                currentEfficiency.CreatePerson = ConstObject.TheLogin.ThePerson;
                currentEfficiency.CreatePersonName = currentEfficiency.CreatePerson.Name;
                currentEfficiency.OperOrgInfo = ConstObject.TheOperationOrg;
                currentEfficiency.OperOrgInfoName = currentEfficiency.OperOrgInfo.Name;
                currentEfficiency.OpgSysCode = currentEfficiency.OperOrgInfo.SysCode;
                currentEfficiency.DocState = DocumentState.Edit;
            }

            currentEfficiency.LastModifyBy = ConstObject.TheLogin.ThePerson.Name;
            currentEfficiency.LastModifyDate = DateTime.Now;
        }

        private int FindRowIndex(string rowKey)
        {
            for (int i = 6; i < gdReport.Rows - 1; i++)
            {
                if (gdReport.Cell(i, 1).Text == rowKey)
                {
                    return i;
                }
            }
            return 0;
        }

        private void SetDisplayStyle()
        {
            #region 合计

            gdReport.Cell(gdReport.Rows - 1, 1).Text = "合计";
            for (int i = 2; i < gdReport.Cols; i++)
            {
                ComputeTotal(i);
            }

            #endregion

            #region 格式调整

            var startRow = 6;
            var range = gdReport.Range(startRow, 1, gdReport.Rows - 1, gdReport.Cols - 1);
            range.set_Borders(EdgeEnum.Inside, LineStyleEnum.Thin);
            range.Alignment = AlignmentEnum.RightCenter;

            var unLockedColumns = new List<int>() { 15, 17 };
            for (int i = 1; i < gdReport.Cols - 1; i++)
            {
                if (unLockedColumns.Contains(i))
                {
                    range = gdReport.Range(startRow, i, gdReport.Rows - 2, i);
                    range.Locked = false;
                    range.BackColor = editColor;

                    gdReport.Cell(gdReport.Rows - 1, i).Locked = true;
                }
                else
                {
                    gdReport.Column(i).Locked = true;
                }
            }

            #endregion
        }

        private void ComputeTotal(int cIndex)
        {
            var totalVal = 0m;
            for (int j = 6; j < gdReport.Rows - 1; j++)
            {
                var tmp = 0m;
                decimal.TryParse(gdReport.Cell(j, cIndex).Text, out tmp);
                totalVal += tmp;
            }
            gdReport.Cell(gdReport.Rows - 1, cIndex).Text = totalVal.ToString("N2");
            gdReport.Column(cIndex).AutoFit();
        }

        private void CreateEfficiencyData(FundPlanningMaster plan)
        {
            InitGrid();

            if (plan == null)
            {
                return;
            }

            var incomeDetails =
               plan.CostCalculationDtl.ToList().FindAll(
                   a => a.Year == dtpYear.Value.Year || (a.Year == dtpYear.Value.Year - 1 && a.Month == 12));
            var sumDetails =
                plan.FundSummaryDtl.ToList().FindAll(
                    a => a.Year == dtpYear.Value.Year || (a.Year == dtpYear.Value.Year - 1 && a.Month == 12));

            var details = from incomeDetail in incomeDetails
                          join sumDetail in sumDetails
                              on new { incomeDetail.Year, incomeDetail.Month } equals
                              new { sumDetail.Year, sumDetail.Month } into temp
                          from tt in temp.DefaultIfEmpty()
                          orderby incomeDetail.Year, incomeDetail.Month
                          select new
                          {
                              IncomeDetail = incomeDetail,
                              SumDetail = tt
                          };

            gdReport.Locked = false;
            gdReport.InsertRow(gdReport.Rows - 1, details.Count());
            var startRow = 6;
            var cIndex = 1;

            #region 资金策划
            for (int i = 0; i < details.Count(); i++)
            {
                var dt = details.ElementAt(i);
                cIndex = 1;
                var rIndex = startRow + i;
                gdReport.Cell(rIndex, cIndex++).Text = string.Format("{0}年{1}月", dt.IncomeDetail.Year,
                                                                     dt.IncomeDetail.Month);
                gdReport.Cell(rIndex, cIndex++).Text = dt.IncomeDetail.CurrentSubTotal.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.IncomeDetail.TotalSubTotal.ToString("N2");
                if (dt.SumDetail != null)
                {
                    var gether = dt.SumDetail.CurrentVoluntarilyGether
                                 + dt.SumDetail.CurrentInnerSetUpGether
                                 + dt.SumDetail.CurrentSubContractGether;
                    gdReport.Cell(rIndex, cIndex++).Text = gether.ToString("N2");
                    gdReport.Cell(rIndex, cIndex++).Text = (dt.SumDetail.TotalVoluntarilyGether
                                                            + dt.SumDetail.TotalInnerSetUpGether
                                                            + dt.SumDetail.TotalSubContractGether).ToString("N2");
                    gdReport.Cell(rIndex, cIndex++).Text = (dt.SumDetail.CurrentVoluntarilyPay
                                                            + dt.SumDetail.CurrentInnerSetupPay
                                                            + dt.SumDetail.CurrentSubcontractorPay).ToString("N2");
                    gdReport.Cell(rIndex, cIndex++).Text = (dt.SumDetail.TotalVoluntarilyPay
                                                            + dt.SumDetail.TotalInnerSetupPay
                                                            + dt.SumDetail.TotalSubcontractorPay).ToString("N2");
                    gdReport.Cell(rIndex, cIndex++).Text = dt.SumDetail.CurrentBalance.ToString("N2");
                    gdReport.Cell(rIndex, cIndex++).Text = dt.SumDetail.TotalBalance.ToString("N2");
                    gdReport.Cell(rIndex, cIndex++).Text = (gether * 0.1m).ToString("N2");
                    gdReport.Cell(rIndex, cIndex++).Text = (dt.SumDetail.TotalBalance - gether * 0.1m).ToString("N2");
                }
            }

            #endregion

            #region 收入对比

            var ds = mOperate.FinanceMultDataSrv.QueryMainBusinessInComeReport(
                currentProject.Id, string.Empty, dtpYear.Value.Year, 12, false);
            if (ds.Tables.Count > 0)
            {
                var lastSum = 0m;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var dr = ds.Tables[0].Rows[i];
                    var ym = string.Format("{0}年{1}月", dr[0], dr[1]);
                    var rIndex = FindRowIndex(ym);
                    if (rIndex == 0)
                    {
                        continue;
                    }

                    var thisSum = TransUtil.ToDecimal(dr["sumMoney"]);
                    gdReport.Cell(rIndex, cIndex).Text = thisSum.ToString("N2");
                    gdReport.Cell(rIndex, cIndex + 1).Text = (thisSum + lastSum).ToString("N2");
                    var incomeTarget = TransUtil.ToDecimal(gdReport.Cell(rIndex, 3).Text);
                    if (incomeTarget == 0)
                    {
                        gdReport.Cell(rIndex, cIndex + 2).Text = (0).ToString("P2");
                    }
                    else
                    {
                        gdReport.Cell(rIndex, cIndex + 2).Text =
                            ((thisSum + lastSum - incomeTarget) / incomeTarget).ToString("P2");
                    }

                    lastSum = thisSum + lastSum;
                }
            }

            #endregion

            #region 收款计划对比

            var bgDate = new DateTime(dtpYear.Value.Year, 1, 1);
            var endDate = new DateTime(dtpYear.Value.Year, 12, 31);
            var getList =
                mOperate.FinanceMultDataSrv.QueryProjectGatheringAccountReport(currentProject.Id, bgDate, endDate);
            if (getList != null)
            {
                var getTotalList = getList.OfType<DataDomain>().GroupBy(a => TransUtil.ToDateTime(a.Name3).ToString("yyyy年M月"));
                var lastGet = 0m;
                foreach (var getTotal in getTotalList)
                {
                    var rIndex = FindRowIndex(getTotal.Key);
                    if (rIndex == 0)
                    {
                        continue;
                    }

                    var thisVal = getTotal.Sum(a => TransUtil.ToDecimal(a.Name14));
                    gdReport.Cell(rIndex, 18).Text = thisVal.ToString("N2");
                    gdReport.Cell(rIndex, 19).Text = (thisVal + lastGet).ToString("N2");
                    var getTarget = TransUtil.ToDecimal(gdReport.Cell(rIndex, 5).Text);
                    if (getTarget == 0)
                    {
                        gdReport.Cell(rIndex, 20).Text = (0).ToString("P2");
                    }
                    else
                    {
                        gdReport.Cell(rIndex, 20).Text =
                            ((thisVal + lastGet - getTarget) / getTarget).ToString("P2");
                    }

                    lastGet = thisVal + lastGet;
                }
            }

            var mustGetDs =
                mOperate.FinanceMultDataSrv.QueryProjectNotGatheringAccountReport(currentProject.Id, bgDate, endDate);
            if (mustGetDs != null && mustGetDs.Tables.Count > 0)
            {
                var mustTotalList =
                    mustGetDs.Tables[0].Select("confirmdate is not null").AsEnumerable()
                        .GroupBy(a => a.Field<DateTime>("confirmdate").ToString("yyyy年M月"));
                foreach (var mustTotal in mustTotalList)
                {
                    var rIndex = FindRowIndex(mustTotal.Key);
                    if (rIndex == 0)
                    {
                        continue;
                    }

                    var schemeVal = 0m;
                    decimal.TryParse(gdReport.Cell(rIndex, 5).Text, out schemeVal);

                    var realVal = 0m;
                    decimal.TryParse(gdReport.Cell(rIndex - 1, 19).Text, out realVal);

                    var shouldVal = mustTotal.Sum(a => a.Field<decimal>("acctgatheringmoney"))
                                    - mustTotal.Sum(a => a.Field<decimal>("Gathering"));
                    var cellVal = Math.Max(shouldVal, Math.Max(schemeVal, realVal));
                    gdReport.Cell(rIndex, 16).Text = cellVal.ToString("N2");
                }
            }

            #endregion

            #region 支付计划对比

            #endregion

            #region 资金存量对比

            #endregion

            SetDisplayStyle();
        }

        private void QueryEfficiency()
        {
            if (currentProject == null)
            {
                MessageBox.Show("请选择项目");
                return;
            }

            var objQuery = new ObjectQuery();
            objQuery.AddCriterion(Expression.Eq("ProjectId", currentProject.Id));
            objQuery.AddCriterion(Expression.Eq("CreateYear", dtpYear.Value.Year));
            objQuery.AddFetchMode("Details", FetchMode.Eager);

            var list = mOperate.FinanceMultDataSrv.Query(typeof (FundSchemeEfficiencyMaster), objQuery);
            if (list != null && list.Count > 0)
            {
                currentEfficiency = list.OfType<FundSchemeEfficiencyMaster>().FirstOrDefault();
            }
            else
            {
                currentEfficiency = null;
            }
        }

        private void ModelToView()
        {
            InitGrid();

            if (currentEfficiency == null)
            {
                return;
            }

            gdReport.Locked = false;
            gdReport.InsertRow(gdReport.Rows - 1, currentEfficiency.Details.Count);
            var startRow = 6;
            var cIndex = 1;

            for (int i = 0; i < currentEfficiency.Details.Count; i++)
            {
                var dt = currentEfficiency.Details.ElementAt(i) as FundSchemeEfficiencyDetail;
                cIndex = 1;
                var rIndex = startRow + i;
                gdReport.Cell(rIndex, cIndex).Tag = dt.Id;

                #region 资金策划

                gdReport.Cell(rIndex, cIndex++).Text = string.Format("{0}年{1}月", dt.CreateYear, dt.CreateMonth);
                gdReport.Cell(rIndex, cIndex++).Text = dt.SchemeIncome.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.TotalSchemeIncome.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.SchemeGether.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.TotalSchemeGether.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.SchemePay.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.TotalSchemePay.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.SchemeBalance.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.TotalSchemeBalance.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.SchemeMoneyDue.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.SchemeBalanceNoMoneyDue.ToString("N2");

                #endregion

                #region 收入对比

                gdReport.Cell(rIndex, cIndex++).Text = dt.CurrentIncome.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.TotalIncome.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.IncomeEfficiency.ToString("P2");

                #endregion

                #region 资金收款计划对比

                gdReport.Cell(rIndex, cIndex++).Text = dt.CurrentGether.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.GetherReference.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.CurrentGetherPlan.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.RealGether.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.TotalGether.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.GetherEfficiency.ToString("P2");

                #endregion

                #region 资金支付计划对比

                gdReport.Cell(rIndex, cIndex++).Text = dt.CurrentPay.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.PayReference.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.CurrentApprovePay.ToString("P2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.PreviewRealPay.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.CurrentRealPay.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.TotalPay.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.PayEfficiency.ToString("P2");

                #endregion

                #region 资金存量对比

                gdReport.Cell(rIndex, cIndex++).Text = dt.LastTotalBalance.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.CentralPurchase.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.InnerInstall.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.OtherContractPay.ToString("N2");
                gdReport.Cell(rIndex, cIndex++).Text = dt.LastAdjustBalance.ToString("N2");
                gdReport.Cell(rIndex, cIndex).Text = dt.BalanceEfficiency.ToString("P2");

                #endregion
            }

            SetDisplayStyle();
        }

        private void ViewToModel()
        {
            NewEfficiencyMaster();

            currentEfficiency.Details.Clear();
            for (var i = 6; i < gdReport.Rows - 1; i++)
            {
                var cIndex = 1;

                var id = gdReport.Cell(i, cIndex).Tag;
                var ym = gdReport.Cell(i, cIndex++).Text;
                var yIndex = ym.IndexOf('年');
                var mIndex = ym.IndexOf('月');

                if (yIndex <= 0 || mIndex <= 0)
                {
                    continue;
                }

                var detail = new FundSchemeEfficiencyDetail();
                detail.Id = string.IsNullOrEmpty(id) ? null : id;
                detail.CreateYear = TransUtil.ToInt(ym.Substring(0, yIndex));
                detail.CreateMonth = TransUtil.ToInt(ym.Substring(yIndex + 1, mIndex - yIndex - 1));
                detail.SchemeIncome = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.TotalSchemeIncome = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.SchemeGether = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.TotalSchemeGether = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.SchemePay = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.TotalSchemePay = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.SchemeBalance = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.TotalSchemeBalance = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.SchemeMoneyDue = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.SchemeBalanceNoMoneyDue = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.CurrentIncome = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.TotalIncome = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.IncomeEfficiency = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).DoubleValue);
                detail.CurrentGether = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.GetherReference = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.CurrentGetherPlan = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.RealGether = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.TotalGether = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.GetherEfficiency = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).DoubleValue);
                detail.CurrentPay = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.PayReference = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.CurrentApprovePay = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.PreviewRealPay = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.CurrentRealPay = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.TotalPay = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.PayEfficiency = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).DoubleValue);
                detail.LastTotalBalance = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.CentralPurchase = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.InnerInstall = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.OtherContractPay = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.LastAdjustBalance = TransUtil.ToDecimal(gdReport.Cell(i, cIndex++).Text);
                detail.BalanceEfficiency = TransUtil.ToDecimal(gdReport.Cell(i, cIndex).DoubleValue);
                currentEfficiency.AddDetail(detail);
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            QueryEfficiency();

            if (currentEfficiency == null || currentEfficiency.Details.Count == 0)
            {
                CreateEfficiencyData(GetScheme());
            }
            else
            {
                ModelToView();
            }
        }

        private void btnRebuild_Click(object sender, EventArgs e)
        {
            CreateEfficiencyData(GetScheme());
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ViewToModel();

            if (currentEfficiency == null)
            {
                MessageBox.Show("没有数据可供保存，请点击获取数据");
                return;
            }

            currentEfficiency = mOperate.FinanceMultDataSrv.SaveSchemeEfficiency(currentEfficiency);
            MessageBox.Show("保存成功");

            ModelToView();
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
                    currentProject.ContractCollectRatio = TransUtil.ToDecimal(dm.Name4)/100m;
                }
            }
        }

        private void gdReport_CellChange(object sender, Grid.CellChangeEventArgs e)
        {
            if (e.Col == 15 || e.Col == 17)
            {
                ComputeTotal(e.Col);
            }
        }
    }
}
