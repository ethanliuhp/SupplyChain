﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.Basic.Template;
using Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData;
using Application.Business.Erp.SupplyChain.Client.ProjectDocumentMng;
using Application.Business.Erp.SupplyChain.Client.Util;
using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using FlexCell;
using IRPServiceModel.Basic;
using IRPServiceModel.Domain.Document;
using Iesi.Collections.Generic;
using NHibernate.Criterion;
using VirtualMachine.Component.Util;
using VirtualMachine.Component.WinControls.CommonForm.FlashScreenMng;
using VirtualMachine.Component.WinControls.Controls;
using VirtualMachine.Core;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public partial class VFundSchemeCalculate : TBasicDataView
    {
        private MFinanceMultData mOperate;
        private FundPlanningMaster selectFundScheme;
        private int projectTaxType = -1;
        private FundSchemeOperate fundSchemeOperate;
        private bool isEditAmount = false;
        private BackgroundWorker loadWorker;
        private BackgroundWorker createWorker;
        private MDocumentCategory msrv = new MDocumentCategory();
        private VFundSchemeProperty propertyForm;

        public VFundSchemeCalculate()
        {
            InitializeComponent();

            InitEvents();

            InitReport();

            ucProjectSelector1.InitData();
        }

        #region 初始化

        private void InitEvents()
        {
            ucProjectSelector1.AfterSelectProjectEvent += AfterSelectProject;
            cmbFundScheme.SelectedIndexChanged += cmbFundScheme_SelectedIndexChanged;

            btnCreate.Click += btnCreate_Click;
            btnSave.Click += btnSave_Click;
            btnUnDo.Click += btnUnDo_Click;
            btnDelete.Click += btnDelete_Click;
            btnExport.Click += btnExport_Click;
            btnCompute.Click += btnCompute_Click;
            btnSubmit.Click += btnSubmit_Click;

            rptGridIndRate.LeaveCell += rptGridIndRate_LeaveCell;
            rptGridBalance.LeaveCell += rptGridBalance_LeaveCell;
            rptGridGether.LeaveCell += rptGridGether_LeaveCell;
            rptGridPayment.LeaveCell += rptGridPayment_LeaveCell;
            rptGridContrast.LeaveCell += rptGridContrast_LeaveCell;

            contextMenuStrip1.Opening += new CancelEventHandler(contextMenuStrip1_Opening);
            tsMenuCancelEdit.Click += new EventHandler(tsMenuCancelEdit_Click);
            tsMenuEdit.Click += new EventHandler(tsMenuEdit_Click);

            loadWorker = new BackgroundWorker();
            loadWorker.DoWork += new DoWorkEventHandler(loadWorker_DoWork);
            loadWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadWorker_RunWorkerCompleted);

            createWorker = new BackgroundWorker();
            createWorker.DoWork += new DoWorkEventHandler(createWorker_DoWork);
            createWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(createWorker_RunWorkerCompleted);

            InitGridDoubleClickEvent();
        }

        private void InitGridDoubleClickEvent()
        {
            foreach (TabPage tp in tbContent.TabPages)
            {
                var grid = FindFlexGrid(tp);
                if (grid != null)
                {
                    grid.DoubleClick += new Grid.DoubleClickEventHandler(grid_DoubleClick);
                }
            }
        }

        private CustomFlexGrid FindFlexGrid(TabPage tp)
        {
            foreach (var ct in tp.Controls)
            {
                if (ct is CustomFlexGrid)
                {
                    return ct as CustomFlexGrid;
                }
            }

            return null;
        }

        private void InitReport()
        {
            foreach (TabPage tp in tbContent.TabPages)
            {
                var grid = FindFlexGrid(tp);
                if (grid != null)
                {
                    grid.Tag = null;
                    grid.EnterKeyMoveTo = MoveToEnum.NextRow;

                    LoadTempleteFile(grid, tp.Tag + ".flx");

                    for (int i = 1; i < grid.Cols; i++)
                    {
                        grid.Cell(0, i).Text = i.ToString();
                    }
                }
            }
        }

        private void LoadTempleteFile(CustomFlexGrid grid, string sReportPath)
        {
            ExploreFile eFile = new ExploreFile();
            string path = eFile.Path;
            if (eFile.IfExistFileInServer(sReportPath))
            {
                eFile.CreateTempleteFileFromServer(sReportPath);
                //载入格式和数据
                grid.OpenFile(path + "\\" + sReportPath); //载入格式
                grid.SelectionStart = 0;
            }
            else
            {
                MessageBox.Show("未找到模板格式文件【" + sReportPath + "】");
            }
        }

        private void AfterSelectProject(object sender)
        {
            var dlg = sender as UcProjectSelector;
            if (dlg == null || dlg.SelectedProject == null)
            {
                return;
            }

            if (mOperate == null)
            {
                mOperate = new MFinanceMultData();
            }

            projectTaxType = dlg.SelectedProject.TaxType;
            if (projectTaxType == 0)
            {
                lbInfo.Tag = lbInfo.Text = "计税类型：简易征收（进项税人工、分包3%，其他为0；销项税3%）";
            }
            else
            {
                lbInfo.Tag = lbInfo.Text = "计税类型：一般征收（进项税分包、安装11%，人工、混凝土、材料其他、水电、其他直接费3%，钢材、设备租赁、其他机械费17%，间接费用进项税率取测算值；销项税11%）";
            }
            fundSchemeOperate = new FundSchemeOperate(projectTaxType);

            var list = mOperate.FinanceMultDataSrv.GetFundSchemeByProject(dlg.SelectedProject.Id)
                .OfType<FundPlanningMaster>().ToList();
            if (list.Count > 0)
            {
                list.Insert(0, new FundPlanningMaster());
            }
            cmbFundScheme.DataSource = list;
            cmbFundScheme.DisplayMember = "Code";
            cmbFundScheme.ValueMember = "Code";
        }

        #endregion

        #region 从数据库读取数据并加载
        
        public void LoadFundScheme()
        {
            try
            {
                loadWorker.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载资金策划数据失败：" + ex);
            }
        }

        private void RefreshBindObject()
        {
            if(selectFundScheme==null)
            {
                return;
            }

            var rptAmountList =
                    mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeReportAmount)).
                        OfType<FundSchemeReportAmount>().ToList();
            fundSchemeOperate.RefreshCellsBindingData(rptGridAmount, rptAmountList, 5);
            fundSchemeOperate.RefreshCellsBindingData(rptGridTax, rptAmountList, 5);

            var getList =
                mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeGathering)).
                    OfType<FundSchemeGathering>().ToList();
            fundSchemeOperate.RefreshCellsBindingData(rptGridGether, getList, 5);

            var taxList =
                mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeIndirectTaxRate))
                    .OfType<FundSchemeIndirectTaxRate>().ToList();
            fundSchemeOperate.RefreshCellsBindingData(rptGridIndRate, taxList, 3);

            var payList =
                mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemePayment)).OfType
                    <FundSchemePayment>().ToList();
            fundSchemeOperate.RefreshCellsBindingData(rptGridPayment, payList, 5);

            var feeList =
                mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeFinanceFee)).
                    OfType<FundSchemeFinanceFee>().ToList();
            fundSchemeOperate.RefreshCellsBindingData(rptGridFee, feeList, 3);

            var smyList =
                mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeSummary)).OfType
                    <FundSchemeSummary>().ToList();
            fundSchemeOperate.RefreshCellsBindingData(rptGridSummary, smyList, 5);

            var contrastList =
                mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeContrast)).
                    OfType<FundSchemeContrast>().ToList();
            fundSchemeOperate.RefreshCellsBindingData(rptGridContrast, contrastList, 5);

            var cashCostList =
                mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeCashCostRate)).
                    OfType<FundSchemeCashCostRate>().ToList();
            fundSchemeOperate.RefreshCellsBindingData(rptGridBalance, cashCostList, 0);
        }

        #endregion

        #region 重新生成测算表数据

        private bool ClearFundSchemeDetail()
        {
            if (selectFundScheme == null)
            {
                return false;
            }

            selectFundScheme = mOperate.FinanceMultDataSrv.ClearFundSchemeDetail(selectFundScheme.Id);
            if (selectFundScheme != null)
            {
                InitReport();

                fundSchemeOperate.Clear();
            }

            return true;
        }

        private void CreateFundSchemeData()
        {
            if (selectFundScheme == null)
            {
                return;
            }

            var list = mOperate.FinanceMultDataSrv.QueryFundSchemeReportAmountByScheme(selectFundScheme.Id)
                .OfType<FundSchemeReportAmount>().ToList();
            selectFundScheme.CostCalculationDtl = new HashedSet<FundSchemeReportAmount>();
            selectFundScheme.CostCalculationDtl.AddAll(list);

            #region 间接费税率
            var taxRateList = new List<FundSchemeIndirectTaxRate>();
            for (int i = 4; i < rptGridIndRate.Rows; i++)
            {
                taxRateList.Add(IndirectTaxRateRowToModel(i));
            }
            selectFundScheme.IndirectInputCalculate = new HashedSet<FundSchemeIndirectTaxRate>();
            selectFundScheme.IndirectInputCalculate.AddAll(taxRateList);
            #endregion

            #region 财务费用
            var feeList = new List<FundSchemeFinanceFee>();
            foreach (var item in list)
            {
                var feeItem = new FundSchemeFinanceFee();
                feeItem.Year = item.Year;
                feeItem.Month = item.Month;
                feeItem.JobNameLink = item.JobNameLink;
                feeItem.Master = item.Master;
                feeItem.ItemGuid = item.ItemGuid;
                feeItem.RowIndex = item.RowIndex;

                feeList.Add(feeItem);
            }
            selectFundScheme.FinanceFeeCalculate = new HashedSet<FundSchemeFinanceFee>();
            selectFundScheme.FinanceFeeCalculate.AddAll(feeList);
            #endregion

            #region 收款
            var getList = new List<FundSchemeGathering>();
            foreach (var item in list)
            {
                var getItem = new FundSchemeGathering();
                getItem.Year = item.Year;
                getItem.Month = item.Month;
                getItem.JobNameLink = item.JobNameLink;
                getItem.Master = item.Master;
                getItem.ItemGuid = item.ItemGuid;
                getItem.RowIndex = item.RowIndex;

                getList.Add(getItem);
            }
            selectFundScheme.GatheringCalculationDtl = new HashedSet<FundSchemeGathering>();
            selectFundScheme.GatheringCalculationDtl.AddAll(getList);
            #endregion

            #region 付款
            var payList = new List<FundSchemePayment>();
            foreach (var item in list)
            {
                var payItem = new FundSchemePayment();
                payItem.Year = item.Year;
                payItem.Month = item.Month;
                payItem.JobNameLink = item.JobNameLink;
                payItem.Master = item.Master;
                payItem.ItemGuid = item.ItemGuid;
                payItem.RowIndex = item.RowIndex;

                payList.Add(payItem);
            }
            selectFundScheme.PaymentCalculationDtl = new HashedSet<FundSchemePayment>();
            selectFundScheme.PaymentCalculationDtl.AddAll(payList);
            #endregion

            #region 策划表
            var smyList = new List<FundSchemeSummary>();
            foreach (var item in list)
            {
                var smyItem = new FundSchemeSummary();
                smyItem.Year = item.Year;
                smyItem.Month = item.Month;
                smyItem.JobNameLink = item.JobNameLink;
                smyItem.Master = item.Master;
                smyItem.ItemGuid = item.ItemGuid;
                smyItem.RowIndex = item.RowIndex;

                smyList.Add(smyItem);
            } 
            selectFundScheme.FundSummaryDtl = new HashedSet<FundSchemeSummary>();
            selectFundScheme.FundSummaryDtl.AddAll(smyList);
            #endregion
            
            #region 对比表
            var contList = new List<FundSchemeContrast>();
            foreach (var item in list)
            {
                var contItem = new FundSchemeContrast();
                contItem.Year = item.Year;
                contItem.Month = item.Month;
                contItem.JobNameLink = item.JobNameLink;
                contItem.Master = item.Master;
                contItem.ItemGuid = item.ItemGuid;
                contItem.RowIndex = item.RowIndex;

                contList.Add(contItem);
            }
            selectFundScheme.FundCalculateContrastDtl = new HashedSet<FundSchemeContrast>();
            selectFundScheme.FundCalculateContrastDtl.AddAll(contList);
            #endregion

            #region 平衡表

            var balanceList = CreateIncomeCostCalculate(list.Last());
            balanceList.AddRange(CreateCashCostRate(payList.Last(), balanceList.Last().RowIndex + 5));
            CreateGetherPayBalance(balanceList);

            selectFundScheme.CashCostRateCalculationDtl = new HashedSet<FundSchemeCashCostRate>();
            selectFundScheme.CashCostRateCalculationDtl.AddAll(balanceList);
            #endregion

            loadWorker_RunWorkerCompleted(loadWorker, null);
        }
        
        private List<FundSchemeCashCostRate> CreateIncomeCostCalculate(FundSchemeReportAmount totalItem)
        {
            if (totalItem == null)
            {
                return null;
            }

            var valueList = new List<KeyValuePair<string, decimal>>();
            var selfIncome = totalItem.TotalEngineeringFee + totalItem.TotalMeasureFee;
            var income = selfIncome + totalItem.TotalInnerSetup + totalItem.TotalSubcontractor;
            valueList.Add(new KeyValuePair<string, decimal>("收入合计", income));
            valueList.Add(new KeyValuePair<string, decimal>("其中：自行收入", selfIncome));
            valueList.Add(new KeyValuePair<string, decimal>("      内部安装", totalItem.TotalInnerSetup));
            valueList.Add(new KeyValuePair<string, decimal>("      甲分包等", totalItem.TotalSubcontractor));

            var cost = totalItem.CurrentCommonSpecialCost + totalItem.CurrentNodePaySpecialCost
                       + totalItem.CurrentLaborCost + totalItem.CurrentSteelCost + totalItem.CurrentConcreteCost
                       + totalItem.CurrentOtherMaterialCost + totalItem.CurrentLeasingCost
                       + totalItem.CurrentUtilitiesCost + totalItem.CurrentOtherEquipmentCost
                       + totalItem.CurrentGovernmentFee + totalItem.CurrentOtherDirectCost
                       + totalItem.CurrentIndirectCost;
            valueList.Add(new KeyValuePair<string, decimal>("成本合计",
                                                            cost + totalItem.CurrentInnerSetupCost +
                                                            totalItem.CurrentSubcontractorCost));
            valueList.Add(new KeyValuePair<string, decimal>("其中：自行成本", cost));
            valueList.Add(new KeyValuePair<string, decimal>("      内部安装", totalItem.CurrentInnerSetupCost));
            valueList.Add(new KeyValuePair<string, decimal>("      甲分包等", totalItem.CurrentSubcontractorCost));
            valueList.Add(new KeyValuePair<string, decimal>("财务费用", totalItem.CurrentFinanceFee));
            valueList.Add(new KeyValuePair<string, decimal>("应交税附", totalItem.AccruedTax));

            var profit = income - cost - totalItem.CurrentFinanceFee - totalItem.AccruedTax;
            valueList.Add(new KeyValuePair<string, decimal>("总利润", profit));

            var profitRate = income == 0 ? 0 : profit/income;
            valueList.Add(new KeyValuePair<string, decimal>("总利润率", profitRate));

            var selfProfitRate = selfIncome == 0 ? 0 : profit/selfIncome;
            valueList.Add(new KeyValuePair<string, decimal>("  其中：自行利润率", selfProfitRate));
            var inproportion = ucProjectSelector1.SelectedProject.Inproportion;
            valueList.Add(new KeyValuePair<string, decimal>("目标上交利润率",
                                                            inproportion > 1 ? inproportion/100 : inproportion));
            valueList.Add(new KeyValuePair<string, decimal>("编制资金策划的基准利润率", profitRate));

            var list = new List<FundSchemeCashCostRate>();
            var rowIndex = 3;
            foreach (var kv in valueList)
            {
                var item = new FundSchemeCashCostRate();
                item.Master = totalItem.Master;
                item.DataType = 1;
                item.ItemGuid = Guid.NewGuid().ToString();
                item.RowIndex = rowIndex++;
                item.SecondCategory = kv.Key;
                item.CostMoney = kv.Value;

                list.Add(item);
            }

            return list;
        }

        private List<FundSchemeCashCostRate> CreateCashCostRate(FundSchemePayment totalItem, int startRow)
        {
            if (totalItem == null)
            {
                return null;
            }

            var list = new List<FundSchemeCashCostRate>();
            var item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "人工费";
            item.SecondCategory = "劳务费";
            item.CostMoney = totalItem.CurrentLaborCost;
            item.CashRateUnCompleted = 0.0m;
            item.CashRateCompleted = 0.0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "分包工程";
            item.SecondCategory = "一般专项工程";
            item.CostMoney = totalItem.CurrentCommonSpecialCost;
            item.CashRateUnCompleted = 0.0m;
            item.CashRateCompleted = 0.0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "分包工程";
            item.SecondCategory = "节点付款专项工程";
            item.CostMoney = totalItem.CurrentNodePaySpecialCost;
            item.CashRateUnCompleted = 0.0m;
            item.CashRateCompleted = 0.0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "分包工程";
            item.SecondCategory = "内部安装";
            item.CostMoney = totalItem.CurrentInnerSetupCost;
            item.CashRateUnCompleted = 0.0m;
            item.CashRateCompleted = 0.0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "分包工程";
            item.SecondCategory = "甲指分包";
            item.CostMoney = totalItem.CurrentSubcontractorCost;
            item.CashRateUnCompleted = 0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "材料费";
            item.SecondCategory = "钢材";
            item.CostMoney = totalItem.CurrentSteelCost;
            item.CashRateUnCompleted = 0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "材料费";
            item.SecondCategory = "混凝土";
            item.CostMoney = totalItem.CurrentConcreteCost;
            item.CashRateUnCompleted = 0.0m;
            item.CashRateCompleted = 0.0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "材料费";
            item.SecondCategory = "其他材料";
            item.CostMoney = totalItem.CurrentOtherMaterialCost;
            item.CashRateUnCompleted = 0.0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "机械使用费";
            item.SecondCategory = "设备租赁费";
            item.CostMoney = totalItem.CurrentLeasingCost;
            item.CashRateUnCompleted = 0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "机械使用费";
            item.SecondCategory = "水电费";
            item.CostMoney = totalItem.CurrentUtilitiesCost;
            item.CashRateUnCompleted = 0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "机械使用费";
            item.SecondCategory = "其他机械成本";
            item.CostMoney = totalItem.CurrentOtherEquipmentCost;
            item.CashRateUnCompleted = 0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "其他直接费";
            item.SecondCategory = "政府规费";
            item.CostMoney = totalItem.CurrentGovernmentFee;
            item.CashRateUnCompleted = 0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "其他直接费";
            item.SecondCategory = "其他费";
            item.CostMoney = totalItem.CurrentOtherDirectCost;
            item.CashRateUnCompleted = 0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "间接费";
            item.SecondCategory = "工程间接费用";
            item.CostMoney = totalItem.CurrentIndirectCost;
            item.CashRateUnCompleted = 0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "财务费用";
            item.SecondCategory = "资金利息支出(收入)等";
            item.CostMoney = totalItem.CurrentFinanceFee;
            item.CashRateUnCompleted = 0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow++;
            item.FisrtCategory = "应交税附";
            item.SecondCategory = "税附";
            item.CostMoney = totalItem.CurrentSurchargePay;
            item.CashRateUnCompleted = 0m;
            item.CashRateCompleted = 0m;
            list.Add(item);

            var totalMoney = list.Sum(v => v.CostMoney);
            foreach (var a in list)
            {
                if (totalMoney != 0)
                {
                    a.CostProportion = a.CostMoney/totalMoney;
                }
                a.Master = totalItem.Master;
                a.CostRateUnCompleted = a.CashRateUnCompleted*a.CostProportion;
                a.CostRateCompleted = a.CashRateCompleted*a.CostProportion;
            }

            item = new FundSchemeCashCostRate();
            item.Master = totalItem.Master;
            item.DataType = 2;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = startRow;
            item.FisrtCategory = " ";
            item.SecondCategory = "合计";
            item.CostMoney = totalMoney;
            item.CostProportion = list.Sum(v => v.CostProportion);
            item.CostRateCompleted = list.Sum(v => v.CostRateCompleted);
            item.CostRateUnCompleted = list.Sum(v => v.CostRateUnCompleted);
            list.Add(item);

            return list;
        }

        private void CreateGetherPayBalance(List<FundSchemeCashCostRate> list)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            var profitRate = list.Find(v => v.DataType == 1 && v.SecondCategory.Equals("总利润率"));
            var cashCostRate = list.FindLast(v => v.DataType == 2);

            var item = new FundSchemeCashCostRate();
            item.Master = cashCostRate.Master;
            item.DataType = 3;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = cashCostRate.RowIndex + 4;
            item.SecondCategory = "在建主体未完";
            item.CostMoney = cashCostRate.CostRateUnCompleted;
            item.CostProportion = profitRate.CostMoney;
            item.CashRateUnCompleted = 0.0m;
            item.CashRateCompleted = item.CostMoney*(1 - item.CostProportion)/(1 + item.CashRateUnCompleted);
            item.CostRateUnCompleted = 0.0m;
            item.CostRateCompleted = item.CostRateUnCompleted - item.CashRateCompleted;
            list.Add(item);

            item = new FundSchemeCashCostRate();
            item.Master = cashCostRate.Master;
            item.DataType = 3;
            item.ItemGuid = Guid.NewGuid().ToString();
            item.RowIndex = cashCostRate.RowIndex + 5;
            item.SecondCategory = "在建主体已完";
            item.CostMoney = cashCostRate.CostRateCompleted;
            item.CostProportion = profitRate.CostMoney;
            item.CashRateUnCompleted = 0.0m;
            item.CashRateCompleted = item.CostMoney*(1 - item.CostProportion)/(1 + item.CashRateUnCompleted);
            item.CostRateUnCompleted = 0.0m;
            item.CostRateCompleted = item.CostRateUnCompleted - item.CashRateCompleted;
            list.Add(item);
        }

        private void CreateCells()
        {
            var rptAmountList = selectFundScheme.CostCalculationDtl.OfType<FundSchemeReportAmount>().ToList();
            fundSchemeOperate.Clear();
            fundSchemeOperate.CreateMasterCells(selectFundScheme, tPageMaster.Tag.ToString(), rptGridMaster.Name);
            fundSchemeOperate.CreateAmountCells(rptAmountList, tPageAmount.Tag.ToString(), rptGridAmount.Name);
            fundSchemeOperate.CreateCostTaxCells(rptAmountList, tPageCostTax.Tag.ToString(), rptGridTax.Name);

            var getList = selectFundScheme.GatheringCalculationDtl.OfType<FundSchemeGathering>().ToList();
            fundSchemeOperate.CreateGetherCells(getList, tPageGether.Tag.ToString(), rptGridGether.Name);

            var taxList = selectFundScheme.IndirectInputCalculate.OfType<FundSchemeIndirectTaxRate>().ToList();
            fundSchemeOperate.CreateTaxRateCells(taxList, tPageTaxRate.Tag.ToString(), rptGridIndRate.Name);

            var payList = selectFundScheme.PaymentCalculationDtl.OfType<FundSchemePayment>().ToList();
            fundSchemeOperate.CreatePaymentCelss(payList, tPagePayment.Tag.ToString(), rptGridPayment.Name);

            var feeList = selectFundScheme.FinanceFeeCalculate.OfType<FundSchemeFinanceFee>().ToList();
            fundSchemeOperate.CreateFinanceFeeCells(feeList, tPageFee.Tag.ToString(), rptGridFee.Name);

            var smyList = selectFundScheme.FundSummaryDtl.OfType<FundSchemeSummary>().ToList();
            fundSchemeOperate.CreateSummaryCells(smyList, tPageSummary.Tag.ToString(), rptGridSummary.Name);

            var contrastList = selectFundScheme.FundCalculateContrastDtl.OfType<FundSchemeContrast>().ToList();
            fundSchemeOperate.CreateContrastCells(contrastList, tPageContrast.Tag.ToString(), rptGridContrast.Name);

            var cashCostList = selectFundScheme.CashCostRateCalculationDtl.OfType<FundSchemeCashCostRate>().ToList();
            fundSchemeOperate.CreateBalanceCells(cashCostList, tPageBalance.Tag.ToString(), rptGridBalance.Name, rptAmountList.Count + 5);

            fundSchemeOperate.ComputeFormula();
        }

        #endregion

        #region 从单元格获取数据

        private decimal GetCellValue(CustomFlexGrid grid, int rowIndex, int colIndex)
        {
            if (rowIndex <= 0 || colIndex <= 0)
            {
                return 0;
            }

            var txt = grid.Cell(rowIndex, colIndex).Text.Trim();
            bool isPercent = false;
            if (txt.EndsWith("%"))
            {
                isPercent = true;
                txt = txt.Replace("%", "");
            }

            decimal tmp;
            decimal.TryParse(txt, out tmp);

            if (isPercent)
            {
                tmp = tmp/100;
            }

            return tmp;
        }

        private FundSchemeIndirectTaxRate IndirectTaxRateRowToModel(int rowIndex)
        {
            if (rowIndex <= 0 || rowIndex > rptGridIndRate.Rows)
            {
                return null;
            }

            var list = rptGridIndRate.Tag as List<FundSchemeIndirectTaxRate>;
            if (list == null)
            {
                list = new List<FundSchemeIndirectTaxRate>();
                rptGridIndRate.Tag = list;
            }

            var itemKey = rptGridIndRate.Cell(rowIndex, 7).Tag;
            var item = list.Find(f => !string.IsNullOrEmpty(f.ItemGuid) && f.ItemGuid.Equals(itemKey));
            if (item == null)
            {
                item = new FundSchemeIndirectTaxRate();
                item.Master = selectFundScheme;
                item.ItemGuid = Guid.NewGuid().ToString();
                item.RowIndex = rowIndex - 3;
            }

            var colIndex = 1;
            item.SerialNumber = Convert.ToInt32(GetCellValue(rptGridIndRate, rowIndex, colIndex++));
            item.FirstSubjectCode = rptGridIndRate.Cell(rowIndex, colIndex).Tag;
            item.FirstSubjectName = rptGridIndRate.Cell(rowIndex, colIndex++).Text;
            item.SecondSubjectCode = rptGridIndRate.Cell(rowIndex, colIndex).Tag;
            item.SecondSubjectName = rptGridIndRate.Cell(rowIndex, colIndex++).Text;
            item.ThirdSubjectCode = rptGridIndRate.Cell(rowIndex, colIndex).Tag;
            item.ThirdSubjectName = rptGridIndRate.Cell(rowIndex, colIndex++).Text;
            item.AppropriationBudget = GetCellValue(rptGridIndRate, rowIndex, colIndex++);
            item.InputTax = GetCellValue(rptGridIndRate, rowIndex, colIndex++);
            item.DeductibleInput = GetCellValue(rptGridIndRate, rowIndex, colIndex++);
            item.CompilationBasis = rptGridIndRate.Cell(rowIndex, colIndex).Text;

            if (rowIndex == rptGridIndRate.Rows - 2)
            {
                item.FirstSubjectName = "合         计";
            }
            else if (rowIndex == rptGridIndRate.Rows - 1)
            {
                item.FirstSubjectName = "可抵扣进项税占比";
            }

            return item;
        }

        #endregion

        private void SetButtonEnable()
        {
            var isEdit = selectFundScheme != null && selectFundScheme.DocState == DocumentState.Edit;

            btnCreate.Enabled = isEdit;
            btnCompute.Enabled = isEdit;
            btnDelete.Enabled = isEdit;
            btnSave.Enabled = isEdit;
            btnUnDo.Enabled = isEdit;
            btnSubmit.Enabled = isEdit;

            lbInfo.Text =
                string.Format("策划表状态：{0}  {1}", ClientUtil.GetDocStateName(selectFundScheme.DocState), lbInfo.Tag);
        }

        private void SetAmountEditArea(bool isReadOnly)
        {
            var range = rptGridAmount.Range(6, 3, rptGridAmount.Rows - 3, 6);
            range.Locked = isReadOnly;
            range.BackColor = isReadOnly ? Color.LightGray : Color.White;

            range = rptGridAmount.Range(6, 17, rptGridAmount.Rows - 3, 30);
            range.Locked = isReadOnly;
            range.BackColor = isReadOnly ? Color.LightGray : Color.White;
        }

        private bool ExportToPdfAndUpload()
        {
            if (selectFundScheme == null)
            {
                return false;
            }
            try
            {
                var tempDir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), selectFundScheme.Code);
                if (!System.IO.Directory.Exists(tempDir))
                {
                    System.IO.Directory.CreateDirectory(tempDir);
                }

                var fileList = new List<string>();
                foreach (TabPage tp in tbContent.TabPages)
                {
                    if (tp.Text.StartsWith("表10") || (!tp.Text.StartsWith("表1") && !tp.Text.StartsWith("表3") && !tp.Text.StartsWith("表4")))
                    {
                        continue;
                    }

                    var grid = FindFlexGrid(tp);
                    if (grid == null)
                    {
                        continue;
                    }

                    var deskFile = System.IO.Path.Combine(tempDir, tp.Text + ".pdf");
                    FlexCell.PageSetup pageSetup = grid.PageSetup;
                    pageSetup.Landscape = true;
                    pageSetup.LeftMargin = 1f;
                    pageSetup.RightMargin = 1f;
                    pageSetup.TopMargin = 1f;
                    pageSetup.BottomMargin = 1f;

                    var ps = new System.Drawing.Printing.PaperSize("A3", 1169, 1654);
                    pageSetup.PaperSize = ps;

                    if(grid.ExportToPDF(deskFile))
                    {
                        fileList.Add(deskFile);
                    }
                }

                var docFile = DownloadDocFile(tempDir);
                if (!string.IsNullOrEmpty(docFile))
                {
                    fileList.Add(docFile);
                }

                //上传
                return FileUploadUtil.UploadFiles(fileList, selectFundScheme.Code);
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private string DownloadDocFile(string tmpDir)
        {
            if (selectFundScheme == null)
            {
                return null;
            }

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("ProObjectGUID", selectFundScheme.Id));
            IList listObj = msrv.ObjectQuery(typeof (ProObjectRelaDocument), oq);
            if (listObj == null || listObj.Count == 0)
            {
                return null;
            }

            oq.Criterions.Clear();
            Disjunction dis = new Disjunction();
            foreach (ProObjectRelaDocument obj in listObj)
            {
                dis.Add(Expression.Eq("Id", obj.DocumentGUID));
            }
            oq.AddCriterion(dis);
            oq.AddFetchMode("ListFiles", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("ListFiles.TheFileCabinet", NHibernate.FetchMode.Eager);

            var docList = msrv.ObjectQuery(typeof (DocumentMaster), oq).OfType<DocumentMaster>().ToList();
            if (docList.Count == 0)
            {
                return null;
            }

            var doc = docList.Find(d => d.ListFiles.Any(b => b.ExtendName == ".docx"));
            if (doc == null)
            {
                return null;
            }

            try
            {
                var docFile = doc.ListFiles.OfType<DocumentDetail>().ToList().Find(a => a.ExtendName == ".docx");
                string baseAddress = @docFile.TheFileCabinet.TransportProtocal.ToString().ToLower() + "://" +
                                     docFile.TheFileCabinet.ServerName + "/" + docFile.TheFileCabinet.Path + "/";
                string address = baseAddress + docFile.FilePartPath;

                string downloadPath = System.IO.Path.Combine(tmpDir, "施工项目资金策划书.docx"); 
                UtilityClass.WebClientObj.DownloadFile(address, downloadPath);

                return downloadPath;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public delegate void SetControlEnableDelegate(Control ctrl, bool bl);
        private void SetControlEnable(Control ctrl, bool bl)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke(new SetControlEnableDelegate(SetControlEnable), new object[]
                                                                                {
                                                                                    ctrl, bl
                                                                                }
                    );
            }
            else
            {
                ctrl.Enabled = bl;
            }
        }

        public delegate void SetGridValuesDelegate(CustomFlexGrid grid);
        private void SetGridCellsValue(CustomFlexGrid grid)
        {
            if(grid.InvokeRequired)
            {
                grid.Invoke(new SetGridValuesDelegate(SetGridCellsValue), new object[] { grid });
            }
            else
            {
                fundSchemeOperate.DisplayCells(grid);
            }
        }

        void createWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetControlEnable(btnCompute, true);
            SetControlEnable(btnCreate, true);
            SetControlEnable(btnDelete, true);
            SetControlEnable(btnSave, true);
            SetControlEnable(btnUnDo, true);
            SetControlEnable(btnSubmit, true);
            SetControlEnable(btnExport, true);

            FlashScreen.Close();
        }

        void createWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SetControlEnable(btnCompute, false);
            SetControlEnable(btnCreate, false);
            SetControlEnable(btnDelete, false);
            SetControlEnable(btnSave, false);
            SetControlEnable(btnUnDo, false);
            SetControlEnable(btnSubmit, false); 
            SetControlEnable(btnExport, false);

            FlashScreen.Show("计算公式创建中，请稍候...");

            CreateCells();

            fundSchemeOperate.ComputeFormula();

            FlashScreen.Flash("更新数据中，请稍候...");
            foreach (TabPage tp in tbContent.TabPages)
            {
                var grid = FindFlexGrid(tp);
                if (grid != null)
                {
                    SetGridCellsValue(grid);
                }
            }
        }

        void loadWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            InitReport();

            fundSchemeOperate.LoadFundSchemeMaster(rptGridMaster, selectFundScheme);
            fundSchemeOperate.LoadReportAmount(rptGridAmount, selectFundScheme);
            fundSchemeOperate.LoadCostTax(rptGridTax, selectFundScheme);
            fundSchemeOperate.LoadPayment(rptGridPayment, selectFundScheme);
            fundSchemeOperate.LoadGether(rptGridGether, selectFundScheme);
            fundSchemeOperate.LoadFinanceFee(rptGridFee, selectFundScheme);
            fundSchemeOperate.LoadFundCashCostRate(rptGridBalance, selectFundScheme);
            fundSchemeOperate.LoadIndirectTaxRate(rptGridIndRate, selectFundScheme);
            fundSchemeOperate.LoadFundSummary(rptGridSummary, selectFundScheme);
            fundSchemeOperate.LoadFundContrast(rptGridContrast, selectFundScheme);

            FlashScreen.Close();

            if (selectFundScheme.DocState == DocumentState.Edit)
            {
                createWorker.RunWorkerAsync();
            }
        }

        void loadWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            FlashScreen.Show("数据加载中，请稍候...");

            var rptAmountList =
                mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeReportAmount)).
                    OfType<FundSchemeReportAmount>().ToList();

            selectFundScheme.CostCalculationDtl = new HashedSet<FundSchemeReportAmount>();
            selectFundScheme.CostCalculationDtl.AddAll(rptAmountList);

            var payList = mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemePayment)).OfType<FundSchemePayment>().ToList();
            selectFundScheme.PaymentCalculationDtl = new HashedSet<FundSchemePayment>();
            selectFundScheme.PaymentCalculationDtl.AddAll(payList);

            var getList = mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeGathering)).OfType<FundSchemeGathering>().ToList();
            selectFundScheme.GatheringCalculationDtl = new HashedSet<FundSchemeGathering>();
            selectFundScheme.GatheringCalculationDtl.AddAll(getList);

            var feeList = mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeFinanceFee)).OfType<FundSchemeFinanceFee>().ToList();
            selectFundScheme.FinanceFeeCalculate = new HashedSet<FundSchemeFinanceFee>();
            selectFundScheme.FinanceFeeCalculate.AddAll(feeList);

            var cashList = mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeCashCostRate)).OfType<FundSchemeCashCostRate>().ToList();
            selectFundScheme.CashCostRateCalculationDtl = new HashedSet<FundSchemeCashCostRate>();
            selectFundScheme.CashCostRateCalculationDtl.AddAll(cashList);

            var rateList = mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeIndirectTaxRate)).OfType<FundSchemeIndirectTaxRate>().ToList();
            selectFundScheme.IndirectInputCalculate = new HashedSet<FundSchemeIndirectTaxRate>();
            selectFundScheme.IndirectInputCalculate.AddAll(rateList);

            var sumList = mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeSummary)).OfType<FundSchemeSummary>().ToList();
            selectFundScheme.FundSummaryDtl = new HashedSet<FundSchemeSummary>();
            selectFundScheme.FundSummaryDtl.AddAll(sumList);

            var contList = mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof(FundSchemeContrast)).OfType<FundSchemeContrast>().ToList();
            selectFundScheme.FundCalculateContrastDtl = new HashedSet<FundSchemeContrast>();
            selectFundScheme.FundCalculateContrastDtl.AddAll(contList);
        }

        private void cmbFundScheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectFundScheme = cmbFundScheme.SelectedItem as FundPlanningMaster;
            if (selectFundScheme == null || string.IsNullOrEmpty(selectFundScheme.Id))
            {
                selectFundScheme = null;
                return;
            }

            SetButtonEnable();

            LoadFundScheme();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (selectFundScheme == null || selectFundScheme.DocState != DocumentState.Edit)
            {
                return;
            }

            if (
                MessageBox.Show("重新生成将清空原数据且不可恢复，您确认要重新生成？", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {
                try
                {
                    FlashScreen.Show("正在重新生成，请稍等…");

                    if (!ClearFundSchemeDetail())
                    {
                        MessageBox.Show("清空该策划下的明细数据失败，重新生成已终止");
                        return;
                    }

                    CreateFundSchemeData();

                    FlashScreen.Close();
                }
                catch (Exception ex)
                {
                    FlashScreen.Close();

                    MessageBox.Show("重新生成失败：" + ex.Message);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectFundScheme == null || selectFundScheme.DocState != DocumentState.Edit)
                {
                    return;
                }

                FlashScreen.Show("数据保存中，请稍等…");

                selectFundScheme.CalculateProfitRate = Convert.ToDecimal(rptGridMaster.Cell(6, 2).DoubleValue);
                selectFundScheme.CostCashRate = Convert.ToDecimal(rptGridMaster.Cell(7, 2).DoubleValue);
                selectFundScheme.BreakevenPoint = Convert.ToDecimal(rptGridMaster.Cell(7, 4).DoubleValue);

                var rptAmountList = new List<FundSchemeReportAmount>();
                fundSchemeOperate.GetGridBindingData(rptGridAmount, rptAmountList);
                selectFundScheme.CostCalculationDtl.Clear();
                selectFundScheme.CostCalculationDtl.AddAll(rptAmountList);

                var getherList = new List<FundSchemeGathering>();
                fundSchemeOperate.GetGridBindingData(rptGridGether, getherList);
                selectFundScheme.GatheringCalculationDtl.Clear();
                selectFundScheme.GatheringCalculationDtl.AddAll(getherList);

                var taxRateList = new List<FundSchemeIndirectTaxRate>();
                fundSchemeOperate.GetGridBindingData(rptGridIndRate, taxRateList);
                selectFundScheme.IndirectInputCalculate.Clear();
                selectFundScheme.IndirectInputCalculate.AddAll(taxRateList);

                var payList = new List<FundSchemePayment>();
                fundSchemeOperate.GetGridBindingData(rptGridPayment, payList);
                selectFundScheme.PaymentCalculationDtl.Clear();
                selectFundScheme.PaymentCalculationDtl.AddAll(payList);

                var feeList = new List<FundSchemeFinanceFee>();
                fundSchemeOperate.GetGridBindingData(rptGridFee, feeList);
                selectFundScheme.FinanceFeeCalculate.Clear();
                selectFundScheme.FinanceFeeCalculate.AddAll(feeList);

                var sumList = new List<FundSchemeSummary>();
                fundSchemeOperate.GetGridBindingData(rptGridSummary, sumList);
                selectFundScheme.FundSummaryDtl.Clear();
                selectFundScheme.FundSummaryDtl.AddAll(sumList);

                var contList = new List<FundSchemeContrast>();
                fundSchemeOperate.GetGridBindingData(rptGridContrast, contList);
                selectFundScheme.FundCalculateContrastDtl.Clear();
                selectFundScheme.FundCalculateContrastDtl.AddAll(contList);

                var cashCostList = new List<FundSchemeCashCostRate>();
                fundSchemeOperate.GetGridBindingData(rptGridBalance, cashCostList);
                selectFundScheme.CashCostRateCalculationDtl.Clear();
                selectFundScheme.CashCostRateCalculationDtl.AddAll(cashCostList);

                selectFundScheme = mOperate.FinanceMultDataSrv.SaveFundScheme(selectFundScheme);
                if (selectFundScheme != null && !string.IsNullOrEmpty(selectFundScheme.Id))
                {
                    RefreshBindObject();
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }

                FlashScreen.Close();
            }
            catch (Exception ex)
            {
                FlashScreen.Close();
                MessageBox.Show("保存数据失败：" + ex.Message);
            }
        }

        private void btnUnDo_Click(object sender, EventArgs e)
        {
            if (selectFundScheme == null || selectFundScheme.DocState != DocumentState.Edit)
            {
                return;
            }

            LoadFundScheme();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectFundScheme == null || selectFundScheme.DocState != DocumentState.Edit)
            {
                MessageBox.Show("请选择编辑中的资金策划表");
                return;
            }

            if (MessageBox.Show("确认要删除数据，删除后不可恢复？", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (!ClearFundSchemeDetail())
            {
                MessageBox.Show("删除数据失败");
            }
            else
            {
                InitReport();

                MessageBox.Show("删除成功");
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (selectFundScheme == null)
            {
                return;
            }

            //ExportToPdfAndUpload();
            //return;

            var saveDialg = new SaveFileDialog();
            saveDialg.FileName = "项目" + selectFundScheme.ProjectName + selectFundScheme.Code;
            saveDialg.Filter = "Excel文件(*.xls)|*.xls";
            if (saveDialg.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            FlashScreen.Show("数据导出中，请稍等…");

            var deskFile = saveDialg.FileName;
            var tmpFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(),
                                                 string.Concat(Guid.NewGuid().ToString(), ".xls"));
            foreach (TabPage tp in tbContent.TabPages)
            {
                var grid = FindFlexGrid(tp);
                if (grid == null)
                {
                    continue;
                }

                if (!System.IO.File.Exists(deskFile))
                {
                    grid.ExportToExcel(deskFile, tp.Text, true, true);
                }
                else
                {
                    grid.ExportToExcel(tmpFile, tp.Text, true, true);
                    grid.MergeExcel(deskFile, tmpFile, true);
                }
            }

            FlashScreen.Close();

            if (MessageBox.Show("导出成功，是否打开文件查看？", "打开确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(saveDialg.FileName);
            }
        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            if (selectFundScheme == null || selectFundScheme.DocState != DocumentState.Edit)
            {
                return;
            }

            var tableHeaders = new List<string>();
            tableHeaders.Add("表5");
            tableHeaders.Add("表7");
            tableHeaders.Add("表8");
            tableHeaders.Add("表9");
            tableHeaders.Add("表10");
            tableHeaders.Add("表3");
            tableHeaders.Add("表2");

            var tpTitle = tbContent.SelectedTab.Text;
            if (!tableHeaders.Exists(t => tpTitle.StartsWith(t)))
            {
                return;
            }

            FlashScreen.Show("正在重新计算中，请稍候…");

            var changeCells = new List<FundSchemeCell>();
            var tabIndex = 0;
            if (tpTitle.StartsWith(tableHeaders[tabIndex++]) || !chkIsOnlyOne.Checked)
            {
                for (int i = 6; i < rptGridAmount.Rows - 1; i++)
                {
                    for (var j = 3; j <= 30; j++)
                    {
                        if ((j > 6 && j < 10) || (j > 13 && j < 16))//剔除全公式列
                        {
                            continue;
                        }

                        var val = GetCellValue(rptGridAmount, i, j);
                        var cell = fundSchemeOperate.SetCellValue(rptGridAmount.Name, i, j, val);
                        if (cell != null)
                        {
                            changeCells.Add(cell);
                        }
                    }
                }
            }
            if (tpTitle.StartsWith(tableHeaders[tabIndex++]) || !chkIsOnlyOne.Checked)
            {
                for (int i = 4; i < rptGridIndRate.Rows - 3; i++)
                {
                    if (rptGridIndRate.Cell(i, 5).Locked)
                    {
                        continue;
                    }

                    var val = GetCellValue(rptGridIndRate, i, 5);
                    var cell = fundSchemeOperate.SetCellValue(rptGridIndRate.Name, i, 5, val);
                    if (cell != null)
                    {
                        changeCells.Add(cell);
                    }
                }
            }
            if (tpTitle.StartsWith(tableHeaders[tabIndex++]) || !chkIsOnlyOne.Checked)
            {
                var val = GetCellValue(rptGridFee, 4, rptGridFee.Cols - 1);
                var cell = fundSchemeOperate.SetCellValue(rptGridIndRate.Name, 4, rptGridFee.Cols - 1, val);
                if (cell != null)
                {
                    changeCells.Add(cell);
                }
            }
            if (tpTitle.StartsWith(tableHeaders[tabIndex++]) || !chkIsOnlyOne.Checked)
            {
                var cols = new List<int>() {3, 5, 12, 18, 19, 20, 23, 24, 25};
                for (int i = 6; i < rptGridGether.Rows - 1; i++)
                {
                    foreach (var j in cols)
                    {
                        var val = GetCellValue(rptGridGether, i, j);
                        var cell = fundSchemeOperate.SetCellValue(rptGridGether.Name, i, j, val);
                        if (cell != null)
                        {
                            changeCells.Add(cell);
                        }
                    }
                }
            }
            if (tpTitle.StartsWith(tableHeaders[tabIndex++]) || !chkIsOnlyOne.Checked)
            {
                var cols = new List<int>() {19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 41};
                for (int i = 6; i < rptGridPayment.Rows - 1; i++)
                {
                    foreach (var j in cols)
                    {
                        var val = GetCellValue(rptGridPayment, i, j);
                        var cell = fundSchemeOperate.SetCellValue(rptGridPayment.Name, i, j, val);
                        if (cell != null)
                        {
                            changeCells.Add(cell);
                        }
                    }
                }
            }
            if (tpTitle.StartsWith(tableHeaders[tabIndex++]) || !chkIsOnlyOne.Checked)
            {
                var cols = new List<int>() {2, 3, 4, 5, 14, 15, 16};
                for (int i = 6; i < rptGridContrast.Rows - 1; i++)
                {
                    foreach (var j in cols)
                    {
                        var val = GetCellValue(rptGridContrast, i, j);
                        var cell = fundSchemeOperate.SetCellValue(rptGridContrast.Name, i, j, val);
                        if (cell != null)
                        {
                            changeCells.Add(cell);
                        }
                    }
                }
            }
            if (tpTitle.StartsWith(tableHeaders[tabIndex]) || !chkIsOnlyOne.Checked)
            {
                for (int i = 22; i < 38; i++)
                {
                    for (int j = 5; j < 7; j++)
                    {
                        var val = GetCellValue(rptGridBalance, i, j);
                        if (val > 1 && val <= 100)
                        {
                            val = val / 100;
                        }
                        else if (val > 100)
                        {
                            val = 1;
                        }

                        var cell = fundSchemeOperate.SetCellValue(rptGridBalance.Name, i, j, val);
                        if (cell != null)
                        {
                            changeCells.Add(cell);
                        }
                    }
                }

                var cols = new List<int>() { 4, 6 };
                for (int i = 42; i < 44; i++)
                {
                    foreach (var j in cols)
                    {
                        var val = GetCellValue(rptGridBalance, i, j);
                        if (val > 1 && val <= 100)
                        {
                            val = val/100;
                        }
                        else if(val > 100)
                        {
                            val = 1;
                        }

                        if (j == 4 && val < 0)
                        {
                            val = 0;
                        }

                        var cell = fundSchemeOperate.SetCellValue(rptGridBalance.Name, i, j, val);
                        if (cell != null)
                        {
                            changeCells.Add(cell);
                        }
                    }
                }
            }

            fundSchemeOperate.ComputeFormulaOnCellChanged(changeCells);
            FlashScreen.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (selectFundScheme == null || selectFundScheme.DocState != DocumentState.Edit)
            {
                MessageBox.Show("只能提交编辑中的资金策划表，请重新选择");
                return;
            }

            var amountList =
                mOperate.FinanceMultDataSrv.GetFundSchemeDetail(selectFundScheme, typeof (FundSchemeReportAmount));
            if (amountList == null || amountList.Count == 0)
            {
                MessageBox.Show("该策划表没有测算数据，请先生成数据再提交审核");
                return;
            }

            if (MessageBox.Show("您确认要提交审核？", "提交确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                DialogResult.No)
            {
                return;
            }

            FlashScreen.Show("提交审核中，请稍等…");

            selectFundScheme.DocState = DocumentState.InAudit;
            selectFundScheme = mOperate.FinanceMultDataSrv.SaveFundScheme(selectFundScheme);

            SetButtonEnable();

            if (ExportToPdfAndUpload())
            {
                MessageBox.Show("提交审核成功");
            }
            else
            {
                MessageBox.Show("提交审核成功，但审核文件上传失败");
            }

            FlashScreen.Close();
        }

        private void rptGridIndRate_LeaveCell(object sender, Grid.LeaveCellEventArgs e)
        {
            if(!chkAutoCompute.Checked)
            {
                return;
            }

            if (e.Col != 5 || e.Row < 3 || e.Row >= rptGridIndRate.Rows - 2)
            {
                return;
            }

            fundSchemeOperate.ComputeFormulaOnCellChanged(rptGridIndRate.Name, e.Row, e.Col,
                                                          GetCellValue(rptGridIndRate, e.Row, e.Col));
        }

        private void rptGridBalance_LeaveCell(object sender, Grid.LeaveCellEventArgs e)
        {
            if (!chkAutoCompute.Checked)
            {
                return;
            }

            var ignoreRows = new List<int>();
            for (var i = 1; i < rptGridBalance.Rows; i++)
            {
                if (string.IsNullOrEmpty(rptGridBalance.Cell(i, 2).Tag))
                {
                    ignoreRows.Add(i);
                }
            }

            if (ignoreRows.Contains(e.Row) || e.Row < 16 || e.Col < 3)
            {
                return;
            }

            fundSchemeOperate.ComputeFormulaOnCellChanged(rptGridBalance.Name, e.Row, e.Col,
                                                          GetCellValue(rptGridBalance, e.Row, e.Col));
        }

        private void rptGridGether_LeaveCell(object sender, Grid.LeaveCellEventArgs e)
        {
            if (!chkAutoCompute.Checked)
            {
                return;
            }

            if (!((e.Col >= 18 && e.Col <= 20) || e.Col == 5) || e.Row <= 5 || e.Row >= rptGridGether.Rows - 1)
            {
                return;
            }

            fundSchemeOperate.ComputeFormulaOnCellChanged(rptGridGether.Name, e.Row, e.Col,
                                                          GetCellValue(rptGridGether, e.Row, e.Col));
        }

        private void rptGridPayment_LeaveCell(object sender, Grid.LeaveCellEventArgs e)
        {
            if (!chkAutoCompute.Checked)
            {
                return;
            }

            if (e.Col < 19 || (e.Col > 32 && e.Col != 41) || e.Row <= 5 || e.Row >= rptGridGether.Rows - 1)
            {
                return;
            }

            fundSchemeOperate.ComputeFormulaOnCellChanged(rptGridPayment.Name, e.Row, e.Col,
                                                          GetCellValue(rptGridPayment, e.Row, e.Col));
        }

        private void rptGridContrast_LeaveCell(object sender, Grid.LeaveCellEventArgs e)
        {
            if (!chkAutoCompute.Checked)
            {
                return;
            }

            if ((e.Col != 2 && e.Col != 4) || e.Row >= rptGridContrast.Rows - 1 || e.Row <= 5)
            {
                return;
            }

            fundSchemeOperate.ComputeFormulaOnCellChanged(rptGridContrast.Name, e.Row, e.Col,
                                                          GetCellValue(rptGridContrast, e.Row, e.Col));
        }

        void grid_DoubleClick(object Sender, EventArgs e)
        {
            var grid = Sender as CustomFlexGrid;
            if (grid == null)
            {
                return;
            }

            var fCell = fundSchemeOperate.GetSchemeCell(grid.Name, grid.ActiveCell);
            if (fCell != null)
            {
                if (propertyForm == null)
                {
                    propertyForm = new VFundSchemeProperty(fundSchemeOperate, grid, fCell);
                    propertyForm.Closed += new EventHandler(propertyForm_Closed);
                    propertyForm.btnCheckData.Click += new EventHandler(btnCheckData_Click);
                    propertyForm.Owner = this;
                    propertyForm.TopMost = true;
                    propertyForm.Show();
                }
                else
                {
                    propertyForm.ShowPropertys(fundSchemeOperate, grid, fCell);
                }
            }
        }

        private void btnCheckData_Click(object sender, EventArgs e)
        {
            var chs = new Dictionary<string, List<string>>();

            //收款检查
            var results = new List<string>();
            var vl1 = 0m;
            var cell1 =
                fundSchemeOperate.GetSchemeCell(rptGridGether.Name,
                                                rptGridGether.Cell(rptGridGether.Rows - 2, rptGridGether.Cols - 1));
            if (cell1 != null)
            {
                vl1 = cell1.CellValue ?? 0;
            }
            results.Add(vl1.ToString("N2"));

            var vl2 = 0m;
            for (int i = 6; i < 9; i++)
            {
                var cell2 =
                    fundSchemeOperate.GetSchemeCell(rptGridSummary.Name, rptGridSummary.Cell(rptGridSummary.Rows - 2, i));
                if (cell2 != null)
                {
                    vl2 += cell2.CellValue ?? 0;
                }
            }

            results.Add(vl2.ToString("N2"));
            results.Add((vl1 - vl2).ToString("N2"));
            chs.Add("表9与表4累计收款检查", results);

            //支出检查
            results = new List<string>();
            cell1 =
                fundSchemeOperate.GetSchemeCell(rptGridPayment.Name, rptGridPayment.Cell(rptGridPayment.Rows - 2, 35));
            vl1 = cell1 == null ? 0 : cell1.CellValue ?? 0;
            results.Add(vl1.ToString("N2"));

            vl2 = 0;
            for (int i = 12; i < 15; i++)
            {
                var cell2 =
                    fundSchemeOperate.GetSchemeCell(rptGridSummary.Name, rptGridSummary.Cell(rptGridSummary.Rows - 2, i));
                if (cell2 != null)
                {
                    vl2 += cell2.CellValue ?? 0;
                }
            }
            results.Add(vl2.ToString("N2"));
            results.Add((vl1 - vl2).ToString("N2"));
            chs.Add("表10与表4累计支出检查", results);

            //累计资金存量检查
            results = new List<string>();
            cell1 =
                fundSchemeOperate.GetSchemeCell(rptGridSummary.Name, rptGridSummary.Cell(rptGridSummary.Rows - 2, 22));
            vl1 = cell1 == null ? 0 : cell1.CellValue ?? 0;
            results.Add(vl1.ToString("N2"));

            cell1 =
                fundSchemeOperate.GetSchemeCell(rptGridContrast.Name, rptGridContrast.Cell(rptGridContrast.Rows - 2, 13));
            vl2 = cell1 == null ? 0 : cell1.CellValue ?? 0;

            results.Add(vl2.ToString("N2"));
            results.Add((vl1 - vl2).ToString("N2"));
            chs.Add("表4与表3累计资金存量检查", results);

            propertyForm.ShowCheckResult(chs);
        }

        private void propertyForm_Closed(object sender, EventArgs e)
        {
            propertyForm = null;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            tsMenuCancelEdit.Enabled = isEditAmount;
            tsMenuEdit.Enabled = !isEditAmount;
        }

        private void tsMenuEdit_Click(object sender, EventArgs e)
        {
            isEditAmount = true;

            SetAmountEditArea(!isEditAmount);
        }

        private void tsMenuCancelEdit_Click(object sender, EventArgs e)
        {
            isEditAmount = false;

            SetAmountEditArea(!isEditAmount);
        }
    }
}
