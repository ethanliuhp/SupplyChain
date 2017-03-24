﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using FlexCell;
using VirtualMachine.Component.WinControls.Controls;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public class FundSchemeOperate
    {
        private const string TOTAL_COLUMN_TEXT = "合计";
        private const string PROJECT_COMPLETED_CHECK = "项目竣工验收";
        private const string COMPLETED_SETTLEMENT = "完工结算";
        private const string GUARANTEE_PERIOD = "质保期";
        private const int DEFAULT_ROW_HEIGHT = 23;

        private int projectTaxtType;
        private List<FundSchemeCell> cells;
        private List<FundSchemeFormula> formulas;
        private List<CustomFlexGrid> grids;
        private decimal outPutTaxRate = 0;
        private decimal inputTaxRate = 0;
        private decimal showUnit = 1m;
        private Dictionary<CustomFlexGrid, int> gridStartRows;
        private Dictionary<CustomFlexGrid, int> gridStartCols;

        public FundSchemeOperate(int taxType)
        {
            projectTaxtType = taxType;
            outPutTaxRate = projectTaxtType == 0 ? 0.03m : 0.11m;
            inputTaxRate = projectTaxtType == 0 ? 0.03m : 0.11m;

            cells = new List<FundSchemeCell>();
            formulas = new List<FundSchemeFormula>();
            grids = new List<CustomFlexGrid>();
            gridStartRows = new Dictionary<CustomFlexGrid, int>();
            gridStartCols = new Dictionary<CustomFlexGrid, int>();
        }

        private string FormarteCellValue(decimal val, string fmt)
        {
            if (!string.IsNullOrEmpty(fmt))
            {
                return val.ToString(fmt);
            }
            else if (val == 0)
            {
                return "-";
            }
            else
            {
                return (val / ShowUnit).ToString("N2");
            }
        }

        private string FormarteCellValue(decimal val)
        {
            if (val == 0)
            {
                return "-";
            }
            else
            {
                return (val / ShowUnit).ToString("N2");
            }
        }

        public void Clear()
        {
            cells.Clear();
            formulas.Clear();
            grids.Clear();
        }

        public decimal ShowUnit
        {
            get { return showUnit; }
            set { showUnit = value; }
        }

        public void SetFrozen(bool isFrozen)
        {
            foreach (var grid in gridStartRows.Keys)
            {
                grid.FrozenRows = isFrozen ? gridStartRows[grid] : 0;
            }

            foreach (var grid in gridStartCols.Keys)
            {
                grid.FrozenCols = isFrozen ? gridStartCols[grid] : 0;
            }
        }

        public FundSchemeCell GetSchemeCell(string tbName ,Cell cell)
        {
            if (cells == null || cell == null)
            {
                return null;
            }

            return cells.Find(a => a.ShortName == tbName && a.RowIndex == cell.Row && a.ColIndex == cell.Col);
        }

        public int GetFormulaCount(string shotTbName)
        {
            if (formulas == null)
            {
                return 0;
            }

            return formulas.Count(a => a.Cell.ShortName == shotTbName);
        }

        #region 创建单元格

        public void CreateMasterCells(FundPlanningMaster master, string rptName, string shortName)
        {
            if (master == null)
            {
                return;
            }

            var cell = new FundSchemeCell(rptName, shortName, 6, 2);
            cell.BindObject = master;
            cell.DataMember = "CalculateProfitRate";
            cell.Formatter = "P2";
            cell.Formula = new FundSchemeFormula(cell);
            cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridBalance", 15, 3);
            cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
            formulas.Add(cell.Formula);
            cells.Add(cell);

            cell = new FundSchemeCell(rptName, shortName, 7, 2);
            cell.BindObject = master;
            cell.DataMember = "CostCashRate";
            cell.Formatter = "P2";
            cell.Formula = new FundSchemeFormula(cell);
            cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridBalance", 38, 7);
            cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
            formulas.Add(cell.Formula);
            cells.Add(cell);

            cell = new FundSchemeCell(rptName, shortName, 7, 4);
            cell.BindObject = master;
            cell.DataMember = "BreakevenPoint";
            cell.Formatter = "P2";
            cell.Formula = new FundSchemeFormula(cell);
            cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridBalance", 42, 5);
            cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
            formulas.Add(cell.Formula);
            cells.Add(cell);
        }

        public void CreateAmountCells(List<FundSchemeReportAmount> list, string rptName, string shortName)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            for (var i = 0; i < list.Count; i++)
            {
                var rIndex = i + 6;

                var cell = new FundSchemeCell(rptName, shortName, rIndex, 3);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentEngineeringFee";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentEngineeringFee;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 4);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentMeasureFee";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentMeasureFee;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 5);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInnerSetup";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentInnerSetup;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 6);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubcontractor";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentSubcontractor;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 7);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentNoTaxCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = 
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 3, 6);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 8);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOutputTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1), outPutTaxRate);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 9);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubTotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 7, 8);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 10);
                cell.BindObject = list[i];
                cell.DataMember = "TotalEngineeringFee";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.CellValue = list[i].TotalEngineeringFee;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, 10),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 3)
                            );
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 11);
                cell.BindObject = list[i];
                cell.DataMember = "TotalMeasureFee";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.CellValue = list[i].TotalMeasureFee;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, 11),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 4)
                            );
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 12);
                cell.BindObject = list[i];
                cell.DataMember = "TotalInnerSetup";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.CellValue = list[i].TotalInnerSetup;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, 12),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 5)
                            );
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 13);
                cell.BindObject = list[i];
                cell.DataMember = "TotalSubcontractor";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.CellValue = list[i].TotalSubcontractor;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, 13),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 6)
                            );
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 14);
                cell.BindObject = list[i];
                cell.DataMember = "TotalNoTaxCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 10, 13);
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = 
                        string.Format("{0} + {1}", FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex),
                                                   FundSchemeCell.ToCellAddress(shortName, rIndex, 7)
                                     );
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 15);
                cell.BindObject = list[i];
                cell.DataMember = "TotalOutputTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    formulas.Add(cell.Formula);
                }
                else if (i > 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = 
                        string.Format("{0} + {1}",
                            FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex),
                            FundSchemeCell.ToCellAddress(shortName, rIndex, 8)
                        );
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, 14), outPutTaxRate);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 16);
                cell.BindObject = list[i];
                cell.DataMember = "TotalSubTotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 14, 15);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 17);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentCommonSpecialCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentCommonSpecialCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 18);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentNodePaySpecialCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentNodePaySpecialCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 19);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInnerSetupCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentInnerSetupCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 20);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubcontractorCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentSubcontractorCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 21);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentLaborCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentLaborCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 22);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSteelCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentSteelCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 23);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentConcreteCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentConcreteCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 24);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherMaterialCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentOtherMaterialCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 25);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentLeasingCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentLeasingCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 26);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentUtilitiesCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentUtilitiesCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 27);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherEquipmentCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentOtherEquipmentCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 28);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentGovernmentFee";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentGovernmentFee;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 29);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherDirectCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentOtherDirectCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 30);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentIndirectCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentIndirectCost;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 31);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentCostSubtotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 17, 30);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 32);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentTaxTotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridTax", rIndex, 31);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 33);
                cell.BindObject = list[i];
                cell.DataMember = "RemainderOfVAT";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex + 1);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i > 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0} - {1} + {2} - {3}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 8),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 32),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, 33),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, 34)
                            );
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0} - {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 15),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 32)
                            );
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 34);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentPayVAT";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex, 33);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 35);
                cell.BindObject = list[i];
                cell.DataMember = "AccruedTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0} * 0.12", FundSchemeCell.ToCellAddress(shortName, rIndex, 33));
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 36);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentFinanceFee";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = 
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridFee", rIndex - 2, 8);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 37);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentProfit";
                cells.Add(cell);
                if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else if(i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0} - {1} - {2} - {3}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 7),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 31),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 35),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 36)
                            );
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 38);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentCumulativeProfit";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0} - {1} - {2} - {3}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 14),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 31),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 35),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 36)
                            );
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 37),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, 38)
                            );
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 39);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentCumulativeRate";
                cell.Formatter = "P2";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression =
                    string.Format("IIF({0}=0,0,{1}/IIF({0}=0,1,{0}))",
                                  FundSchemeCell.ToCellAddress(shortName, rIndex, 14),
                                  FundSchemeCell.ToCellAddress(shortName, rIndex, 38)
                        );
                formulas.Add(cell.Formula);
            }
        }

        public void CreateCostTaxCells(List<FundSchemeReportAmount> list, string rptName, string shortName)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            for (var i = 0; i < list.Count; i++)
            {
                var rIndex = i + 6;

                var cell = new FundSchemeCell(rptName, shortName, rIndex, 3);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentCommonSpecialCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = 
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 17);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 4);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentCommonSpecialCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = 
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = 
                        cell.Formula.ComputeExpression =
                             string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, 3), inputTaxRate);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 5);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentNodePaySpecialCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = 
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression =
                                                     FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 18);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 6);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentNodePaySpecialCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, 5), inputTaxRate);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 7);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInnerSetupCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 19);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 8);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInnerSetupCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, 7), inputTaxRate);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 9);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubcontractorCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression =
                                                     cell.Formula.FormulaExpression =
                                                     FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 20);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 10);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubcontractorCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, 9), inputTaxRate);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 11);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentLaborCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression = 
                                                     FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 21);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 12);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentLaborCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        string.Format("{0}*0.03", FundSchemeCell.ToCellAddress(shortName, rIndex, 11));
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 13);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSteelCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 22);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 14);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSteelCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, 13), projectTaxtType == 0 ? 0 : 0.17);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 15);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentConcreteCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 23);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 16);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentConcreteCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, 15), projectTaxtType == 0 ? 0 : 0.03);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 17);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherMaterialCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 24);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 18);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherMaterialCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, 17), projectTaxtType == 0 ? 0 : 0.03);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 19);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentLeasingCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 25);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 20);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentLeasingCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, 19), projectTaxtType == 0 ? 0 : 0.17);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 21);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentUtilitiesCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 26);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 22);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentUtilitiesCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, 21), projectTaxtType == 0 ? 0 : 0.03);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 23);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherEquipmentCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 27);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 24);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherEquipmentCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression =
                                                     string.Format("{0}*{1}",
                                                                   FundSchemeCell.ToCellAddress(shortName, rIndex, 23),
                                                                   projectTaxtType == 0 ? 0 : 0.17);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 25);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentGovernmentFee";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 28);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 26);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherDirectCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 29);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 27);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherDirectCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression =
                                                     string.Format("{0}*{1}",
                                                                   FundSchemeCell.ToCellAddress(shortName, rIndex, 26),
                                                                   projectTaxtType == 0 ? 0 : 0.03);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 28);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentIndirectCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression =
                        cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 30);
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 29);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentIndirectCostTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression =
                                                     string.Format("{0}*{1}",
                                                                   FundSchemeCell.ToCellAddress(shortName, rIndex, 28),
                                                                   projectTaxtType == 0
                                                                       ? "0"
                                                                       : FundSchemeCell.ToCellAddress("rptGridIndRate",
                                                                                                      57, 7));
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 30);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentCostSubtotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression =
                                                     string.Format(
                                                         "{0}+{1}+{2}+{3}+{4}+{5}+{6}+{7}+{8}+{9}+{10}+{11}+{12}+{13}",
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 3),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 5),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 7),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 9),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 11),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 13),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 15),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 17),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 19),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 21),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 23),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 25),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 26),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 28)
                                                         );
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 31);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentTaxTotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1,
                                                                                    cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression =
                                                     string.Format(
                                                         "{0}+{1}+{2}+{3}+{4}+{5}+{6}+{7}+{8}+{9}+{10}+{11}+{12}",
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 4),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 6),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 8),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 10),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 12),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 14),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 16),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 18),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 20),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 22),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 24),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 27),
                                                         FundSchemeCell.ToCellAddress(shortName, rIndex, 29)
                                                         );
                    formulas.Add(cell.Formula);
                }
            }
        }

        public void CreateTaxRateCells(List<FundSchemeIndirectTaxRate> list, string rptName, string shortName)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            var sumStart = 4;
            var subTotalRows = new List<int>();
            for (var i = 0; i < list.Count; i++)
            {
                var rIndex = i + 4;
                var isSubTotal = !string.IsNullOrEmpty(list[i].SecondSubjectName) &&
                                 list[i].SecondSubjectName.Replace(" ", "").Equals("小计");
                if (isSubTotal)
                {
                    subTotalRows.Add(rIndex);
                }
                var isTotal = !string.IsNullOrEmpty(list[i].FirstSubjectName) &&
                              list[i].FirstSubjectName.Replace(" ", "").Equals("合计");

                var cell = new FundSchemeCell(rptName, shortName, rIndex, 5);
                cell.BindObject = list[i];
                cell.DataMember = "AppropriationBudget";
                cells.Add(cell);
                if (isSubTotal)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        FundSchemeFormula.ToSumFormula(shortName, sumStart, rIndex - 1, 5, 5);
                    formulas.Add(cell.Formula);
                }
                else if (isTotal)
                {
                    List<string> fms = new List<string>();
                    foreach (var subTotalRow in subTotalRows)
                    {
                        fms.Add(FundSchemeCell.ToCellAddress(shortName, subTotalRow, 5));
                    }
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression = string.Join("+", fms.ToArray());
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].AppropriationBudget;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 6);
                cell.BindObject = list[i];
                cell.DataMember = "InputTax";
                cell.CellValue = list[i].InputTax;
                cell.Formatter = "P2";
                cells.Add(cell);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 7);
                cell.BindObject = list[i];
                cell.DataMember = "DeductibleInput";
                cells.Add(cell);

                if (isSubTotal)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression =
                        FundSchemeFormula.ToSumFormula(shortName, sumStart, rIndex - 1, 7, 7);
                    formulas.Add(cell.Formula);

                    sumStart = rIndex + 1;
                }
                else if (isTotal)
                {
                    List<string> fms = new List<string>();
                    foreach (var subTotalRow in subTotalRows)
                    {
                        fms.Add(FundSchemeCell.ToCellAddress(shortName, subTotalRow, 7));
                    }
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression = string.Join("+", fms.ToArray());
                    formulas.Add(cell.Formula);
                }
                else if (i == list.Count - 1)
                {
                    cell.Formatter = "P2";
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        cell.Formula.ComputeExpression = string.Format("IIF({0}>0,{1}/{0},0)",
                                                                       FundSchemeCell.ToCellAddress(shortName,
                                                                                                    rIndex - 1, 5),
                                                                       FundSchemeCell.ToCellAddress(shortName,
                                                                                                    rIndex - 1, 7)
                                                             );
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = cell.Formula.ComputeExpression = string.Format("{0}*{1}",
                                                                                                    FundSchemeCell.
                                                                                                        ToCellAddress(
                                                                                                            shortName,
                                                                                                            rIndex, 5),
                                                                                                    FundSchemeCell.
                                                                                                        ToCellAddress(
                                                                                                            shortName,
                                                                                                            rIndex, 6)
                                                                                          );
                    formulas.Add(cell.Formula);
                }
            }
        }

        public void CreateFinanceFeeCells(List<FundSchemeFinanceFee> list, string rptName, string shortName)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            for (var i = 0; i < list.Count; i++)
            {
                var rIndex = i + 4;
                var cell = new FundSchemeCell(rptName, shortName, rIndex, 2);
                cell.BindObject = list[i];
                cell.DataMember = "TotalGethering";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridGether", rIndex + 2, 27);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 3);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentPayment";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 4, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1} + {2} + {3}",
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex + 2, 33),
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex + 2, 39),
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex + 2, 40),
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex + 2, 41)
                            );
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 4);
                cell.BindObject = list[i];
                cell.DataMember = "TotalPayment";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1} + {2} + {3}",
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex + 2, 33),
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex + 2, 39),
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex + 2, 40),
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex + 2, 41)
                            );
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 3),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, 4)
                            );
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 5);
                cell.BindObject = list[i];
                cell.DataMember = "CurrencyHandIn";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 4, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridSummary", rIndex + 2, 24);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 6);
                cell.BindObject = list[i];
                cell.DataMember = "TotalCurrencyHandIn";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridSummary", rIndex + 2, 24);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 5),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, 6)
                            );
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 7);
                cell.BindObject = list[i];
                cell.DataMember = "TotalBalance";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression =
                    string.Format("{0} - {1} - {2}",
                                  FundSchemeCell.ToCellAddress(shortName, rIndex, 2),
                                  FundSchemeCell.ToCellAddress(shortName, rIndex, 4),
                                  FundSchemeCell.ToCellAddress(shortName, rIndex, 6)
                        );
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 8);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentFinanceFee";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 4, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.CellValue = list[i].CurrentFinanceFee;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("IIF({0}<0,-{0}*0.005,-{0}*0.0025)",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 7)
                            );
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
            }
        }

        public void CreateGetherCells(List<FundSchemeGathering> list, string rptName, string shortName)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            for (var i = 0; i < list.Count; i++)
            {
                var rIndex = i + 6;
                var cell = new FundSchemeCell(rptName, shortName, rIndex, 3);
                cell.BindObject = list[i];
                cell.DataMember = "ContractGetherRate";
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].ContractGetherRate;
                }
                cell.Formatter = "P2";
                cells.Add(cell);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 4);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentVoluntarilyAmount";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression =
                    string.Format("{0} + {1}",
                                  FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 3),
                                  FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 4)
                        );
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 5);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOptimizeAmount";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentOptimizeAmount;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 6);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInnerSetUp";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 5);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 7);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubContract";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 6);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 8);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentNoTaxAmount";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 4, cell.ColIndex - 1);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 9);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOutputTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 10);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubtotalAmount";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, cell.ColIndex - 2, cell.ColIndex - 1);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 11);
                cell.BindObject = list[i];
                cell.DataMember = "TotalVoluntarilyAmount";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula("rptGridAmount", rIndex, rIndex, 10, 11);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 12);
                cell.BindObject = list[i];
                cell.DataMember = "TotalOptimizeAmount";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.CellValue = list[i].TotalOptimizeAmount;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 5),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 13);
                cell.BindObject = list[i];
                cell.DataMember = "TotalInnerSetUp";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 12);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 14);
                cell.BindObject = list[i];
                cell.DataMember = "TotalSubContract";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 13);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 15);
                cell.BindObject = list[i];
                cell.DataMember = "TotalNoTaxAmount";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 11, 14);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 16);
                cell.BindObject = list[i];
                cell.DataMember = "TotalOutputTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*{1}", FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 17);
                cell.BindObject = list[i];
                cell.DataMember = "TotalSubtotalAmount";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 15, 16);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 18);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentVoluntarilyGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentVoluntarilyGether;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 19);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInnerSetUpGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentInnerSetUpGether;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 20);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubContractGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentSubContractGether;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 21);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOutputTaxGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("({0})*{1}",
                                      FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 18, 20), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 22);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentGetherTotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 18, 21);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 23);
                cell.BindObject = list[i];
                cell.DataMember = "TotalVoluntarilyGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.CellValue = list[i].TotalVoluntarilyGether;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 18),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 24);
                cell.BindObject = list[i];
                cell.DataMember = "TotalInnerSetUpGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.CellValue = list[i].TotalInnerSetUpGether;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 19),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 25);
                cell.BindObject = list[i];
                cell.DataMember = "TotalSubContractGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.CellValue = list[i].TotalSubContractGether;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 20),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 26);
                cell.BindObject = list[i];
                cell.DataMember = "TotalOutputTaxGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("({0})*{1}",
                                      FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 23, 25), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 21),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 27);
                cell.BindObject = list[i];
                cell.DataMember = "TotalGetherWithTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 23, 26);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
            }
        }

        public void CreatePaymentCelss(List<FundSchemePayment> list, string rptName, string shortName)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            for (var i = 0; i < list.Count; i++)
            {
                var rIndex = i + 6;

                var cell = new FundSchemeCell(rptName, shortName, rIndex, 2);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentCommonSpecialCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 3, 4);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 3);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentNodePaySpecialCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 5, 6);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 4);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInnerSetupCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 7, 8);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 5);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubcontractorCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 9, 10);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 6);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentLaborCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 11, 12);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 7);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSteelCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 13, 14);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 8);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentConcreteCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 15, 16);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 9);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherMaterialCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 17, 18);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 10);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentLeasingCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 19, 20);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 11);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentUtilitiesCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 21, 22);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 12);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherEquipmentCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 23, 24);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 13);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentGovernmentFee";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridTax", rIndex, 25);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 14);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherDirectCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 26, 27);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 15);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentIndirectCost";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridTax", rIndex, rIndex, 28, 29);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 16);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentCostSubtotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 2, 15);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 17);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentFinanceFee";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression =
                    FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 36);
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 18);
                cell.BindObject = list[i];
                cell.DataMember = "TotalCost";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression =
                    FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, cell.ColIndex - 2, cell.ColIndex - 1);
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 19);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentCommonSpecialPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentCommonSpecialPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 20);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentNodePaySpecialPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentNodePaySpecialPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 21);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInnerSetupPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentInnerSetupPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 22);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubcontractorPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentSubcontractorPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 23);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentLaborPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentLaborPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 24);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSteelPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentSteelPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 25);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentConcretePay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentConcretePay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 26);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherMaterialPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentOtherMaterialPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 27);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentLeasingPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentLeasingPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 28);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentUtilitiesPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentUtilitiesPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 29);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherEquipmentPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentOtherEquipmentPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 30);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentGovernmentFeePay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentGovernmentFeePay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 31);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherDirectPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentOtherDirectPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 32);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentIndirectPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex, 15);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 33);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentTaxPaySubtotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 19, 32);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 34);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentFinanceFeePay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex, 17);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 35);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentPaySubtotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 33, 34);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 36);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOutputTaxGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridGether", rIndex, 26);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridGether", rIndex, 21);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 37);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInputTaxTotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridAmount", rIndex, 32);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 38);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentUnPayVAT";
                cells.Add(cell);
                if (i == list.Count - 1 || i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} - {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 2),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} - {1} + {2} - {3}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 2),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex + 1));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 39);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentPayedVAT";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 40);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSurchargePay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*0.12", FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 41);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentOtherPay;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 42);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentPayTotal";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 35),
                                      FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 39, 41));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 43);
                cell.BindObject = list[i];
                cell.DataMember = "TotalPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
            }
        }

        public void CreateSummaryCells(List<FundSchemeSummary> list, string rptName, string shortName)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            var fundScheme = list[0].Master as FundPlanningMaster;
            var handInRate = fundScheme == null ? 0 : fundScheme.TargetHandin/100;
            for (var i = 0; i < list.Count; i++)
            {
                var rIndex = i + 6;
                
                var cell = new FundSchemeCell(rptName, shortName, rIndex, 3);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentVoluntarilyGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*(1+{1})", FundSchemeCell.ToCellAddress("rptGridGether", rIndex, 18), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 4);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInnerSetUpGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*(1+{1})", FundSchemeCell.ToCellAddress("rptGridGether", rIndex, 19), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 5);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubContractGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*(1+{1})", FundSchemeCell.ToCellAddress("rptGridGether", rIndex, 20), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 6);
                cell.BindObject = list[i];
                cell.DataMember = "TotalVoluntarilyGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*(1+{1})", FundSchemeCell.ToCellAddress("rptGridGether", rIndex, 23), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}+{1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 3),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 7);
                cell.BindObject = list[i];
                cell.DataMember = "TotalInnerSetUpGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*(1+{1})", FundSchemeCell.ToCellAddress("rptGridGether", rIndex, 24), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}+{1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 4),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 8);
                cell.BindObject = list[i];
                cell.DataMember = "TotalSubContractGether";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*(1+{1})", FundSchemeCell.ToCellAddress("rptGridGether", rIndex, 25), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}+{1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 5),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 9);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentVoluntarilyPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} - {1} - {2}",
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 35),
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 22),
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 21));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 10);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInnerSetupPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 21);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 11);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSubcontractorPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 22);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 12);
                cell.BindObject = list[i];
                cell.DataMember = "TotalVoluntarilyPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} - {1} - {2}",
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 35),
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 22),
                                      FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 21));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 9),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 13);
                cell.BindObject = list[i];
                cell.DataMember = "TotalInnerSetupPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 21);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 10),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 14);
                cell.BindObject = list[i];
                cell.DataMember = "TotalSubcontractorPay";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 22);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 11),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 15);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOutputTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 36);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 16);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentInputTax";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 37);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 17);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentUnPayVAT";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 38);
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 18);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentPayedVAT";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 39);
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 19);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentSurchargePay";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 40);
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 20);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentOtherPay";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress("rptGridPayment", rIndex, 41);
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 21);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentBalance";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}-{1}-{2}",
                                      FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 3, 5),
                                      FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 9, 11),
                                      FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 18 ,20)
                                     );
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }


                cell = new FundSchemeCell(rptName, shortName, rIndex, 22);
                cell.BindObject = list[i];
                cell.DataMember = "TotalBalance";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = string.Format("{0}-{1}-{2}",
                                      FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 6, 8),
                                      FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 12, 14),
                                      FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 18, 20)
                                     );
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 23);
                cell.BindObject = list[i];
                cell.DataMember = "BalanceRate";
                cell.Formatter = "P2";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression =
                    string.Format("IIF({0}=0,0,{1}/(IIF({0}=0,1,{0})))",
                                  FundSchemeFormula.ToSumFormula(shortName, rIndex, rIndex, 6, 8),
                                  FundSchemeCell.ToCellAddress(shortName, rIndex, 22));
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 24);
                cell.BindObject = list[i];
                cell.DataMember = "CurrencyHandin";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (list[i].JobNameLink.Equals(PROJECT_COMPLETED_CHECK))
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("({0}/(1+{3})*{2} - ({1}))*0.4",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex + 3, 6),
                                      FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex,
                                                                     cell.ColIndex), handInRate, outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (list[i].JobNameLink.Equals(COMPLETED_SETTLEMENT))
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("({0}/(1+{3})*{2} - ({1}))*0.4",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex + 2, 6),
                                      FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 2, cell.ColIndex,
                                                                     cell.ColIndex), handInRate, outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (list[i].JobNameLink.Equals(GUARANTEE_PERIOD))
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("({0}/(1+{3})*{2} - ({1}))*0.2",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex + 1, 6),
                                      FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 3, cell.ColIndex,
                                                                     cell.ColIndex), handInRate, outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}/(1+{1})*0.1", FundSchemeCell.ToCellAddress(shortName, rIndex, 3), outPutTaxRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 25);
                cell.BindObject = list[i];
                cell.DataMember = "TargetStock";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                if (i == 0)
                {
                    cell.Formula.FormulaExpression = string.Format("{0} - {1}",
                                                                   FundSchemeCell.ToCellAddress(shortName, rIndex, 22),
                                                                   FundSchemeCell.ToCellAddress(shortName, rIndex, 24));
                }
                else
                {
                    cell.Formula.FormulaExpression = string.Format("{0} - {1} + {2}",
                                                                   FundSchemeCell.ToCellAddress(shortName, rIndex, 21),
                                                                   FundSchemeCell.ToCellAddress(shortName, rIndex, 24),
                                                                   FundSchemeCell.ToCellAddress(shortName, rIndex - 1, 25));
                }
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);
            }
        }

        public void CreateContrastCells(List<FundSchemeContrast> list, string rptName, string shortName)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            for (var i = 0; i < list.Count; i++)
            {
                var rIndex = i + 6;

                var cell = new FundSchemeCell(rptName, shortName, rIndex, 2);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentGethering";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentGethering;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 3);
                cell.BindObject = list[i];
                cell.DataMember = "TotalGethering";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.CellValue = list[i].TotalGethering;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 4);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentPayment";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.CellValue = list[i].CurrentPayment;
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 5);
                cell.BindObject = list[i];
                cell.DataMember = "TotalPayment";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.CellValue = list[i].TotalPayment;
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 6);
                cell.BindObject = list[i];
                cell.DataMember = "CurrentBalance";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} - {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 2),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 4));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 7);
                cell.BindObject = list[i];
                cell.DataMember = "TotalBalance";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} - {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 3),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 5));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 8);
                cell.BindObject = list[i];
                cell.DataMember = "SchemeCurrentGethering";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula("rptGridSummary", rIndex, rIndex, 3, 5);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 9);
                cell.BindObject = list[i];
                cell.DataMember = "SchemeTotalGethering";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeFormula.ToSumFormula("rptGridSummary", rIndex, rIndex, 6, 8);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 10);
                cell.BindObject = list[i];
                cell.DataMember = "SchemeCurrentPayment";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeFormula.ToSumFormula("rptGridSummary", rIndex, rIndex, 9, 11),
                                      FundSchemeFormula.ToSumFormula("rptGridSummary", rIndex, rIndex, 18, 20));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 11);
                cell.BindObject = list[i];
                cell.DataMember = "SchemeTotalPayment";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeFormula.ToSumFormula("rptGridSummary", rIndex, rIndex, 12, 14),
                                      FundSchemeFormula.ToSumFormula("rptGridSummary", rIndex, rIndex, 18, 20));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 12);
                cell.BindObject = list[i];
                cell.DataMember = "SchemeCurrentBalance";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 6, rIndex - 1, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} - {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 8),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 10));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 13);
                cell.BindObject = list[i];
                cell.DataMember = "SchemeTotalBalance";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} - {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 9),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 11));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, cell.ColIndex - 1),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 14);
                cell.BindObject = list[i];
                cell.DataMember = "SchemeYearGethering";
                cells.Add(cell);
                if (list[i].Month == 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, rIndex, 8);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if(i == 0)
                {
                    cell.CellValue = list[i].SchemeYearGethering;
                }
                else if(i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 8),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 15);
                cell.BindObject = list[i];
                cell.DataMember = "SchemeYearPayment";
                cells.Add(cell);
                if (list[i].Month == 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex, 10);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 0)
                {
                    cell.CellValue = list[i].SchemeYearPayment;
                }
                else if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression = "0";
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 10),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 16);
                cell.BindObject = list[i];
                cell.DataMember = "SchemeYearFlow";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression =
                    string.Format("{0} - {1}",
                                  FundSchemeCell.ToCellAddress(shortName, rIndex, 14),
                                  FundSchemeCell.ToCellAddress(shortName, rIndex, 15));
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 17);
                cell.BindObject = list[i];
                cell.DataMember = "ContrastEffect";
                cells.Add(cell);
                if (i == list.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex - 1, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} - {1}",
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 13),
                                      FundSchemeCell.ToCellAddress(shortName, rIndex, 7));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
            }
        }

        public void CreateBalanceCells(List<FundSchemeCashCostRate> list, string rptName, string shortName, int totalRowIndex)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            var costList = list.FindAll(d => d.DataType == 1);
            var rIndex = 0;
            var fundScheme = list[0].Master as FundPlanningMaster;
            var handInRate = fundScheme == null ? 0 : fundScheme.TargetHandin;
            for (var i = 0; i < costList.Count; i++)
            {
                rIndex = costList[i].RowIndex;

                var cell = new FundSchemeCell(rptName, shortName, rIndex, 3);
                cell.BindObject = costList[i];
                cell.DataMember = "CostMoney";
                cells.Add(cell);
                if (rIndex == 3)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 4, 6, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 4)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula("rptGridAmount", totalRowIndex, totalRowIndex, 10, 11);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 5)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridAmount", totalRowIndex, 12);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 6)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridAmount", totalRowIndex, 13);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 7)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, 8, 10, cell.ColIndex, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 8)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} + {1}",
                                      FundSchemeFormula.ToSumFormula("rptGridAmount", totalRowIndex, totalRowIndex, 17,
                                                                     18),
                                      FundSchemeFormula.ToSumFormula("rptGridAmount", totalRowIndex, totalRowIndex, 21,
                                                                     30));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 9)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridAmount", totalRowIndex, 19);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 10)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridAmount", totalRowIndex, 20);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 11)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridAmount", totalRowIndex, 36);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 12)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridAmount", totalRowIndex, 35);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 13)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0} - {1} - {2} - {3}",
                                      FundSchemeCell.ToCellAddress(shortName, 3, cell.ColIndex),
                                      FundSchemeCell.ToCellAddress(shortName, 7, cell.ColIndex),
                                      FundSchemeCell.ToCellAddress(shortName, 11, cell.ColIndex),
                                      FundSchemeCell.ToCellAddress(shortName, 12, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 14)
                {
                    cell.Formatter = "P2";
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("IIF({0}=0,0,{1}/(IIF({0}=0,1,{0})))",
                                      FundSchemeCell.ToCellAddress(shortName, 3, cell.ColIndex),
                                      FundSchemeCell.ToCellAddress(shortName, 13, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 15)
                {
                    cell.Formatter = "P2";
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("IIF({0}=0,0,{1}/(IIF({0}=0,1,{0})))",
                                      FundSchemeCell.ToCellAddress(shortName, 4, cell.ColIndex),
                                      FundSchemeCell.ToCellAddress(shortName, 13, cell.ColIndex));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 16)
                {
                    cell.Formatter = "P2";
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = string.Format("{0}/100", handInRate);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (rIndex == 17)
                {
                    cell.Formatter = "P2";
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression = FundSchemeCell.ToCellAddress(shortName, 14, cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
            }

            var lastRowIndex = costList.Last().RowIndex;
            var cashList = list.FindAll(d => d.DataType == 2);
            for (var i = 0; i < cashList.Count; i++)
            {
                rIndex = cashList[i].RowIndex;
                var cell = new FundSchemeCell(rptName, shortName, rIndex, 3);
                cell.BindObject = cashList[i];
                cell.DataMember = "CostMoney";
                cells.Add(cell);
                if (i == cashList.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, lastRowIndex + 5, rIndex - 1, cell.ColIndex,
                                                       cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    var colMap = new Dictionary<int, int>();
                    colMap.Add(lastRowIndex + 5, 6);
                    colMap.Add(lastRowIndex + 6, 2);
                    colMap.Add(lastRowIndex + 7, 3);
                    colMap.Add(lastRowIndex + 8, 4);
                    colMap.Add(lastRowIndex + 9, 5);
                    colMap.Add(lastRowIndex + 10, 7);
                    colMap.Add(lastRowIndex + 11, 8);
                    colMap.Add(lastRowIndex + 12, 9);
                    colMap.Add(lastRowIndex + 13, 10);
                    colMap.Add(lastRowIndex + 14, 11);
                    colMap.Add(lastRowIndex + 15, 12);
                    colMap.Add(lastRowIndex + 16, 13);
                    colMap.Add(lastRowIndex + 17, 14);
                    colMap.Add(lastRowIndex + 18, 15);
                    colMap.Add(lastRowIndex + 19, 17);
                    colMap.Add(lastRowIndex + 20, 40);

                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress("rptGridPayment", totalRowIndex, colMap[rIndex]);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 4);
                cell.BindObject = cashList[i];
                cell.DataMember = "CostProportion";
                cell.Formatter = "P2";
                cells.Add(cell);
                if (i == cashList.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, lastRowIndex + 5, rIndex - 1, cell.ColIndex,
                                                       cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("IIF({0}=0,0,{1}/(IIF({0}=0,1,{0})))",
                                      FundSchemeCell.ToCellAddress(shortName, cashList.Last().RowIndex,
                                                                   cell.ColIndex - 1),
                                      FundSchemeCell.ToCellAddress(shortName, cell.RowIndex,
                                                                   cell.ColIndex - 1)
                            );
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 5);
                cell.BindObject = cashList[i];
                cell.DataMember = "CashRateUnCompleted";
                cell.CellValue = cashList[i].CashRateUnCompleted;
                cell.Formatter = "P2";
                cells.Add(cell);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 6);
                cell.BindObject = cashList[i];
                cell.DataMember = "CashRateCompleted";
                cell.CellValue = cashList[i].CashRateCompleted;
                cell.Formatter = "P2";
                cells.Add(cell);

                cell = new FundSchemeCell(rptName, shortName, rIndex, 7);
                cell.BindObject = cashList[i];
                cell.DataMember = "CostRateUnCompleted";
                cell.Formatter = "P2";
                cells.Add(cell);
                if (i == cashList.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, lastRowIndex + 5, rIndex - 1, cell.ColIndex,
                                                       cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*{1}",
                                      FundSchemeCell.ToCellAddress(shortName, cell.RowIndex, 4),
                                      FundSchemeCell.ToCellAddress(shortName, cell.RowIndex, 5));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, rIndex, 8);
                cell.BindObject = cashList[i];
                cell.DataMember = "CostRateCompleted";
                cell.Formatter = "P2";
                cells.Add(cell);
                if (i == cashList.Count - 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeFormula.ToSumFormula(shortName, lastRowIndex + 5, rIndex - 1, cell.ColIndex,
                                                       cell.ColIndex);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        string.Format("{0}*{1}",
                                      FundSchemeCell.ToCellAddress(shortName, cell.RowIndex, 4),
                                      FundSchemeCell.ToCellAddress(shortName, cell.RowIndex, 6));
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
            }

            var banList = list.FindAll(d => d.DataType == 3);
            for (int i = 0; i < banList.Count; i++)
            {
                var cell = new FundSchemeCell(rptName, shortName, banList[i].RowIndex, 2);
                cell.BindObject = banList[i];
                cell.DataMember = "CostMoney";
                cell.Formatter = "P2";
                cells.Add(cell);
                if (i == 0)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex, 7);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }
                else if (i == 1)
                {
                    cell.Formula = new FundSchemeFormula(cell);
                    cell.Formula.FormulaExpression =
                        FundSchemeCell.ToCellAddress(shortName, rIndex, 8);
                    cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                    formulas.Add(cell.Formula);
                }

                cell = new FundSchemeCell(rptName, shortName, banList[i].RowIndex, 3);
                cell.BindObject = banList[i];
                cell.DataMember = "CostProportion";
                cell.Formatter = "P2";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression =
                    FundSchemeCell.ToCellAddress(shortName, 14, 3);
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, banList[i].RowIndex, 4);
                cell.BindObject = banList[i];
                cell.DataMember = "CashRateUnCompleted";
                cell.CellValue = banList[i].CashRateUnCompleted;
                cell.Formatter = "P2";
                cells.Add(cell);

                cell = new FundSchemeCell(rptName, shortName, banList[i].RowIndex, 5);
                cell.BindObject = banList[i];
                cell.DataMember = "CashRateCompleted";
                cell.Formatter = "P2";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression =
                    string.Format("{0}*(1-{1})/(1+{2})",
                                  FundSchemeCell.ToCellAddress(shortName, cell.RowIndex, 2),
                                  FundSchemeCell.ToCellAddress(shortName, cell.RowIndex, 3),
                                  FundSchemeCell.ToCellAddress(shortName, cell.RowIndex, 4));
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);

                cell = new FundSchemeCell(rptName, shortName, banList[i].RowIndex, 6);
                cell.BindObject = banList[i];
                cell.DataMember = "CostRateUnCompleted";
                cell.CellValue = banList[i].CostRateUnCompleted;
                cell.Formatter = "P2";
                cells.Add(cell);

                cell = new FundSchemeCell(rptName, shortName, banList[i].RowIndex, 7);
                cell.BindObject = banList[i];
                cell.DataMember = "CostRateCompleted";
                cell.Formatter = "P2";
                cells.Add(cell);
                cell.Formula = new FundSchemeFormula(cell);
                cell.Formula.FormulaExpression =
                    string.Format("{0} - {1}",
                                  FundSchemeCell.ToCellAddress(shortName, cell.RowIndex, 6),
                                  FundSchemeCell.ToCellAddress(shortName, cell.RowIndex, 5));
                cell.Formula.ComputeExpression = cell.Formula.FormulaExpression;
                formulas.Add(cell.Formula);
            }
        }

        #endregion

        #region 按单元格显示数据

        public void DisplayCells(CustomFlexGrid grid)
        {
            if (grid == null || cells.Count == 0)
            {
                return;
            }
            if (!grids.Contains(grid))
            {
                grids.Add(grid);
            }
            grid.DefaultRowHeight = 23;
            var amountCells = cells.FindAll(c => c.ShortName.Equals(grid.Name));
            foreach (var cell in amountCells)
            {
                var gdCell = grid.Cell(cell.RowIndex, cell.ColIndex);
                gdCell.Text = FormarteCellValue(cell.CellValue ?? 0, cell.Formatter);
                gdCell.set_Border(EdgeEnum.Left, LineStyleEnum.Thin);
                gdCell.set_Border(EdgeEnum.Right, LineStyleEnum.Thin);
                gdCell.set_Border(EdgeEnum.Top, LineStyleEnum.Thin);
                gdCell.set_Border(EdgeEnum.Bottom, LineStyleEnum.Thin);
                gdCell.Locked = cell.Formula != null;
                if (gdCell.Locked)
                {
                    gdCell.BackColor = System.Drawing.Color.Gainsboro;
                }
                else
                {
                    gdCell.BackColor = System.Drawing.Color.White;
                }

                grid.Column(cell.ColIndex).AutoFit();
            }
        }

        #endregion

        #region  公式计算

        public void ComputeFormula()
        {
            ComputeFormula(formulas);
        }

        private void ComputeFormula(List<FundSchemeFormula> fms)
        {
            if (fms == null || fms.Count == 0 || cells == null)
            {
                return;
            }

            var unComputeFormulas = fms.FindAll(f => !f.IsComputed);
            var oldCount = 0;
            int counter = 0;

            while (unComputeFormulas.Count > 0 && unComputeFormulas.Count != oldCount)
            {
                counter++;
                oldCount = unComputeFormulas.Count;

                var parameterCells =
                    cells.FindAll(
                        c =>
                        unComputeFormulas.Exists(f => f.FormulaExpression.Contains(c.CellAddress)) &&
                        c.CellValue != null);
                foreach (var cl in parameterCells)
                {
                    unComputeFormulas.ForEach(
                        f => f.ComputeExpression = f.ComputeExpression.Replace(cl.CellAddress, cl.CellValue.ToString()));
                }

                unComputeFormulas.FindAll(c => c.IsCanCompute).ForEach(fm => fm.Calculate());

                unComputeFormulas = fms.FindAll(f => !f.IsComputed);
            }
        }

        private List<FundSchemeFormula> GetRelationFormula(FundSchemeCell cell)
        {
            var fms = formulas.FindAll(f => f.FormulaExpression.Contains(cell.CellAddress));
            if (fms.Count == 0)
            {
                return null;
            }

            var refs = formulas.FindAll(f => fms.Exists(ef => f.FormulaExpression.Contains(ef.Cell.CellAddress)));
            refs = refs.FindAll(f => !fms.Contains(f));
            while (refs.Count > 0)
            {
                fms.AddRange(refs);

                refs = formulas.FindAll(f => refs.Exists(ef => f.FormulaExpression.Contains(ef.Cell.CellAddress)));
                refs = refs.FindAll(f => !fms.Contains(f));
            }

            return fms;
        }

        private List<FundSchemeFormula> GetRelationFormula(List<FundSchemeCell> cellList)
        {
            if (cellList == null)
            {
                return null;
            }
            var fms = formulas.FindAll(f => cellList.Exists(c => f.FormulaExpression.Contains(c.CellAddress)));
            if (fms.Count == 0)
            {
                return null;
            }

            var refs = formulas.FindAll(f => fms.Exists(ef => f.FormulaExpression.Contains(ef.Cell.CellAddress)));
            refs = refs.FindAll(f => !fms.Contains(f));
            while (refs.Count > 0)
            {
                fms.AddRange(refs);

                refs = formulas.FindAll(f => refs.Exists(ef => f.FormulaExpression.Contains(ef.Cell.CellAddress)));
                refs = refs.FindAll(f => !fms.Contains(f));
            }

            return fms;
        }

        public void ComputeFormulaOnCellChanged(string gridName, int rIndex, int cIndex, decimal val)
        {
            var cell = cells.Find(c => c.ShortName.Equals(gridName) && c.RowIndex == rIndex && c.ColIndex == cIndex);
            if (cell == null || cell.Formula != null)
            {
                return;
            }

            cell.CellValue = val;
            cell.UpdateBindObjectValue();

            var fmList = GetRelationFormula(cell);
            if (fmList == null)
            {
                return;
            }

            foreach (var fm in fmList)
            {
                fm.ComputeExpression = fm.FormulaExpression;
                fm.Cell.CellValue = null;
                fm.IsComputed = false;
            }

            ComputeFormula(fmList);

            foreach (var fm in fmList)
            {
                var grid = grids.Find(g => g.Name == fm.Cell.ShortName);
                grid.Cell(fm.Cell.RowIndex, fm.Cell.ColIndex).Text = FormarteCellValue(fm.Cell.CellValue ?? 0,
                                                                                       fm.Cell.Formatter);
            }
        }

        public void ComputeFormulaOnCellChanged(List<FundSchemeCell> changeCells)
        {
            var fmList = GetRelationFormula(changeCells);
            foreach (var fm in fmList)
            {
                fm.ComputeExpression = fm.FormulaExpression;
                fm.Cell.CellValue = null;
                fm.IsComputed = false;
            }

            ComputeFormula(fmList);

            foreach (var fm in fmList)
            {
                var grid = grids.Find(g => g.Name == fm.Cell.ShortName);
                grid.Cell(fm.Cell.RowIndex, fm.Cell.ColIndex).Text = 
                    FormarteCellValue(fm.Cell.CellValue ?? 0, fm.Cell.Formatter);
                grid.Column(fm.Cell.ColIndex).AutoFit();
            }

            foreach (var changeCell in changeCells)
            {
                var grid = grids.Find(g => g.Name == changeCell.ShortName);
                grid.Cell(changeCell.RowIndex, changeCell.ColIndex).Text = 
                    FormarteCellValue(changeCell.CellValue ?? 0, changeCell.Formatter);
                grid.Column(changeCell.ColIndex).AutoFit();
            }
        }

        #endregion

        #region 获取／设置绑定对象

        public FundSchemeCell SetCellValue(string gridName, int rIndex, int cIndex, decimal val)
        {
            if (cells == null)
            {
                return null;
            }

            var cell = cells.Find(c => c.ShortName.Equals(gridName) && c.RowIndex == rIndex && c.ColIndex == cIndex);
            if (cell == null || cell.Formula != null)
            {
                return null;
            }

            cell.CellValue = val;
            cell.UpdateBindObjectValue();

            return cell;
        }

        public void GetGridBindingData(CustomFlexGrid grid, IList dataList)
        {
            if (grid == null || cells == null)
            {
                return;
            }

            var dataCells = cells.FindAll(c => c.ShortName == grid.Name).GroupBy(b => b.RowIndex);
            foreach (var dataCell in dataCells)
            {
                dataList.Add(dataCell.First().BindObject);
            }
        }

        public void RefreshCellsBindingData(CustomFlexGrid grid, IList list, int headerRow)
        {
            if (grid == null || cells == null || list == null || list.Count == 0)
            {
                return;
            }

            var dataCells = cells.FindAll(c => c.ShortName == grid.Name).GroupBy(b => b.RowIndex);
            foreach (var rows in dataCells)
            {
                object obj = null;
                foreach (var data in list)
                {
                    var rIndex = (int) data.GetType().GetProperty("RowIndex").GetValue(data, null) + headerRow;
                    if (rIndex == rows.Key)
                    {
                        obj = data;
                        break;
                    }
                }

                if (obj == null)
                {
                    throw new Exception("行" + rows.Key + "没有找到对应的绑定对象");
                }

                foreach (var cell in rows)
                {
                    cell.BindObject = obj;
                }
            }
        }

        #endregion

        #region 从数据库加载并显示

        private void SetReportProjectNameAndUnit(CustomFlexGrid grid, int row, FundPlanningMaster fundScheme)
        {
            if (fundScheme == null || grid == null)
            {
                return;
            }

            grid.EnterKeyMoveTo = MoveToEnum.NextRow;
            for (int i = 1; i < grid.Cols; i++)
            {
                var txt = grid.Cell(row, i).Text.Trim();
                if (txt.Contains("单位"))
                {
                    grid.Cell(row, i).Text = string.Format("单位：{0}", fundScheme.Unit);
                }
                else if (txt.Contains("项目名称"))
                {
                    grid.Cell(row, i).Text = string.Format("项目名称：{0}", fundScheme.ProjectName);
                }
            }
        }

        private void SetGridReadOnlyStyle(CustomFlexGrid grid, int dataRow)
        {
            var range = grid.Range(dataRow, 1, grid.Rows - 1, grid.Cols - 1);
            range.Locked = true;
            range.set_Borders(EdgeEnum.Inside, LineStyleEnum.Thin);
            grid.DefaultRowHeight = DEFAULT_ROW_HEIGHT;
        }

        private void SetDataAreaStyle(CustomFlexGrid grid, int dataCol)
        {
            for (int i = dataCol; i < grid.Cols; i++)
            {
                grid.Column(i).AutoFit();
                grid.Column(i).Alignment = AlignmentEnum.RightCenter;
            }
        }

        public void LoadFundSchemeMaster(CustomFlexGrid grid, FundPlanningMaster fundScheme)
        {
            if (grid == null || fundScheme == null)
            {
                return;
            }

            grid.Cell(2, 1).Text = "项目名称：" + fundScheme.ProjectName;
            grid.Cell(3, 2).Text = FormarteCellValue(fundScheme.ProjectCost) + fundScheme.Unit;
            grid.Cell(3, 4).Text = fundScheme.ContractDuration;
            grid.Cell(4, 2).Text = fundScheme.GatheringCondition;
            grid.Cell(4, 4).Text = fundScheme.CreateDate.ToString("yyyy年M月d日");
            grid.Cell(5, 2).Text = fundScheme.SpecialDescript;
            grid.Cell(5, 4).Text = fundScheme.SchemeTime;
            grid.Cell(6, 2).Text = string.Format("{0:P2}", fundScheme.CalculateProfitRate > 1 ? fundScheme.CalculateProfitRate / 100 : fundScheme.CalculateProfitRate);
            grid.Cell(6, 4).Text = string.Format("{0:P2}", fundScheme.TargetHandin > 1 ? fundScheme.TargetHandin / 100 : fundScheme.TargetHandin);
            grid.Cell(7, 2).Text = string.Format("{0:P2}", fundScheme.CostCashRate > 1 ? fundScheme.CostCashRate / 100 : fundScheme.CostCashRate);
            grid.Cell(7, 4).Text = string.Format("{0:P2}", fundScheme.BreakevenPoint > 1 ? fundScheme.BreakevenPoint / 100 : fundScheme.BreakevenPoint);
            grid.Cell(8, 2).Text = fundScheme.CalculateSituation;
            grid.Cell(9, 2).Text = fundScheme.PlanningTarget;
            grid.Cell(10, 2).Text = fundScheme.FinancePerson;
            grid.Cell(10, 4).Text = fundScheme.BusinessPerson;
            grid.Cell(11, 2).Text = fundScheme.TechnologyPerson;
            grid.Cell(11, 4).Text = fundScheme.DirectorPerson;

            SetGridReadOnlyStyle(grid, 1);

            for (int i = 3; i < grid.Rows; i++)
            {
                grid.Row(i).AutoFit();
            }
        }

        public void LoadReportAmount(CustomFlexGrid grid, FundPlanningMaster fundScheme)
        {
            if (grid == null || fundScheme == null || fundScheme.CostCalculationDtl.Count == 0)
            {
                return;
            }

            SetReportProjectNameAndUnit(grid, 2, fundScheme);

            var startIndex = 5;
            if (!gridStartRows.ContainsKey(grid))
            {
                gridStartRows.Add(grid, startIndex);
            }
            grid.InsertRow(startIndex + 1, fundScheme.CostCalculationDtl.Count);
            foreach (var item in fundScheme.CostCalculationDtl)
            {
                var colIndex = 1;
                var rowIndex = startIndex + item.RowIndex;

                if (item.Year > 0 && item.Month > 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = string.Format("{0}年{1}月", item.Year, item.Month);
                }
                else
                {
                    colIndex++;
                }

                grid.Cell(rowIndex, 1).Tag = item.ItemGuid;
                grid.Cell(rowIndex, colIndex++).Text = item.JobNameLink;
                if (!gridStartCols.ContainsKey(grid))
                {
                    gridStartCols.Add(grid, colIndex - 1);
                }
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentEngineeringFee);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentMeasureFee);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInnerSetup);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubcontractor);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentNoTaxCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOutputTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubTotal);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalEngineeringFee);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalMeasureFee);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalInnerSetup);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalSubcontractor);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalNoTaxCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalOutputTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalSubTotal);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentCommonSpecialCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentNodePaySpecialCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInnerSetupCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubcontractorCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentLaborCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSteelCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentConcreteCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherMaterialCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentLeasingCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentUtilitiesCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherEquipmentCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentGovernmentFee);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherDirectCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentIndirectCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentCostSubtotal);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentTaxTotal);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.RemainderOfVAT);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentPayVAT);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.AccruedTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentFinanceFee);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentProfit);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentCumulativeProfit);
                grid.Cell(rowIndex, colIndex).Text = string.Format("{0:0.00%}", item.CurrentCumulativeRate);
            }

            SetGridReadOnlyStyle(grid, startIndex + 1);
            SetDataAreaStyle(grid, 3);
        }

        public void LoadCostTax(CustomFlexGrid grid, FundPlanningMaster fundScheme)
        {
            if (grid == null || fundScheme == null || fundScheme.CostCalculationDtl.Count == 0)
            {
                return;
            }

            SetReportProjectNameAndUnit(grid, 2, fundScheme);

            var startIndex = 5;
            if (!gridStartRows.ContainsKey(grid))
            {
                gridStartRows.Add(grid, startIndex);
            }
            grid.InsertRow(startIndex + 1, fundScheme.CostCalculationDtl.Count);
            foreach (var item in fundScheme.CostCalculationDtl)
            {
                var colIndex = 1;
                var rowIndex = startIndex + item.RowIndex;

                if (item.Year > 0 && item.Month > 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = string.Format("{0}年{1}月", item.Year, item.Month);
                }
                else
                {
                    colIndex++;
                }

                grid.Cell(rowIndex, 1).Tag = item.ItemGuid;
                grid.Cell(rowIndex, colIndex++).Text = item.JobNameLink;
                if (!gridStartCols.ContainsKey(grid))
                {
                    gridStartCols.Add(grid, colIndex - 1);
                }
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentCommonSpecialCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentCommonSpecialCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentNodePaySpecialCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentNodePaySpecialCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInnerSetupCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInnerSetupCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubcontractorCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubcontractorCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentLaborCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentLaborCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSteelCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSteelCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentConcreteCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentConcreteCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherMaterialCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherMaterialCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentLeasingCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentLeasingCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentUtilitiesCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentUtilitiesCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherEquipmentCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherEquipmentCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentGovernmentFee);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherDirectCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherDirectCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentIndirectCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentIndirectCostTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentCostSubtotal);
                grid.Cell(rowIndex, colIndex).Text = FormarteCellValue(item.CurrentTaxTotal);
            }

            SetGridReadOnlyStyle(grid, startIndex + 1);
            SetDataAreaStyle(grid, 3);
        }

        public void LoadGether(CustomFlexGrid grid, FundPlanningMaster fundScheme)
        {
            if (grid == null || fundScheme == null || fundScheme.GatheringCalculationDtl.Count == 0)
            {
                return;
            }

            SetReportProjectNameAndUnit(grid, 2, fundScheme);

            var startIndex = 5;
            if (!gridStartRows.ContainsKey(grid))
            {
                gridStartRows.Add(grid, startIndex);
            }
            grid.InsertRow(startIndex + 1, fundScheme.GatheringCalculationDtl.Count);
            foreach (var item in fundScheme.GatheringCalculationDtl)
            {
                var colIndex = 1;
                var rowIndex = startIndex + item.RowIndex;

                if (item.Year > 0 && item.Month > 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = string.Format("{0}年{1}月", item.Year, item.Month);
                }
                else
                {
                    colIndex++;
                }

                grid.Cell(rowIndex, 1).Tag = item.ItemGuid;
                grid.Cell(rowIndex, colIndex++).Text = item.JobNameLink;
                if (!gridStartCols.ContainsKey(grid))
                {
                    gridStartCols.Add(grid, colIndex - 1);
                }
                grid.Cell(rowIndex, colIndex++).Text = string.Format("{0:0.00%}", item.ContractGetherRate);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentVoluntarilyAmount);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOptimizeAmount);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInnerSetUp);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubContract);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentNoTaxAmount);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOutputTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubtotalAmount);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalVoluntarilyAmount);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalOptimizeAmount);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalInnerSetUp);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalSubContract);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalNoTaxAmount);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalOutputTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalSubtotalAmount);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentVoluntarilyGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInnerSetUpGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubContractGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOutputTaxGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentGetherTotal);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalVoluntarilyGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalInnerSetUpGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalSubContractGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalOutputTaxGether);
                grid.Cell(rowIndex, colIndex).Text = FormarteCellValue(item.TotalGetherWithTax);
            }

            SetGridReadOnlyStyle(grid, startIndex + 1);
            SetDataAreaStyle(grid, 3);
        }

        public void LoadPayment(CustomFlexGrid grid, FundPlanningMaster fundScheme)
        {
            if (grid == null || fundScheme == null || fundScheme.PaymentCalculationDtl.Count == 0)
            {
                return;
            }

            SetReportProjectNameAndUnit(grid, 2, fundScheme);

            var startIndex = 5;
            if (!gridStartRows.ContainsKey(grid))
            {
                gridStartRows.Add(grid, startIndex);
            }
            grid.InsertRow(startIndex + 1, fundScheme.PaymentCalculationDtl.Count);
            foreach (var item in fundScheme.PaymentCalculationDtl)
            {
                var colIndex = 1;
                var rowIndex = startIndex + item.RowIndex;
                grid.Cell(rowIndex, colIndex).Tag = item.ItemGuid;
                if (item.Year > 0 && item.Month > 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = string.Format("{0}年{1}月", item.Year, item.Month);
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = item.JobNameLink;
                }
                if (!gridStartCols.ContainsKey(grid))
                {
                    gridStartCols.Add(grid, colIndex - 1);
                }
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentCommonSpecialCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentNodePaySpecialCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInnerSetupCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubcontractorCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentLaborCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSteelCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentConcreteCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherMaterialCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentLeasingCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentUtilitiesCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherEquipmentCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentGovernmentFee);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherDirectCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentIndirectCost);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentCostSubtotal);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentFinanceFee);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentCostSubtotal + item.CurrentFinanceFee);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentCommonSpecialPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentNodePaySpecialPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInnerSetupPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubcontractorPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentLaborPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSteelPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentConcretePay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherMaterialPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentLeasingPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentUtilitiesPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherEquipmentPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentGovernmentFeePay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherDirectPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentIndirectPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentTaxPaySubtotal);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentFinanceFeePay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentPaySubtotal);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOutputTaxGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInputTaxTotal);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentUnPayVAT);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentPayedVAT);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSurchargePay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentPayTotal);
                grid.Cell(rowIndex, colIndex).Text = FormarteCellValue(item.TotalPay);
            }

            SetGridReadOnlyStyle(grid, startIndex + 1);
            SetDataAreaStyle(grid, 2);
        }

        public void LoadFinanceFee(CustomFlexGrid grid, FundPlanningMaster fundScheme)
        {
            if (grid == null || fundScheme == null || fundScheme.FinanceFeeCalculate.Count == 0)
            {
                return;
            }

            SetReportProjectNameAndUnit(grid, 2, fundScheme);

            var startIndex = 3;
            if (!gridStartRows.ContainsKey(grid))
            {
                gridStartRows.Add(grid, startIndex);
            }
            grid.InsertRow(startIndex + 1, fundScheme.FinanceFeeCalculate.Count);
            foreach (var item in fundScheme.FinanceFeeCalculate)
            {
                var colIndex = 1;
                var rowIndex = startIndex + item.RowIndex;
                if (item.Year > 0 && item.Month > 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = string.Format("{0}年{1}月", item.Year, item.Month);
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = item.JobNameLink;
                }
                if (!gridStartCols.ContainsKey(grid))
                {
                    gridStartCols.Add(grid, colIndex - 1);
                }
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalGethering);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentPayment);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalPayment);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrencyHandIn);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalCurrencyHandIn);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalBalance);
                grid.Cell(rowIndex, colIndex).Text = FormarteCellValue(item.CurrentFinanceFee);
            }

            SetGridReadOnlyStyle(grid, startIndex + 1);
            SetDataAreaStyle(grid, 2);
        }

        public void LoadFundSummary(CustomFlexGrid grid, FundPlanningMaster fundScheme)
        {
            if (grid == null || fundScheme == null || fundScheme.FundSummaryDtl.Count == 0)
            {
                return;
            }

            SetReportProjectNameAndUnit(grid, 2, fundScheme);

            var startIndex = 5;
            if (!gridStartRows.ContainsKey(grid))
            {
                gridStartRows.Add(grid, startIndex);
            }
            grid.InsertRow(startIndex + 1, fundScheme.FundSummaryDtl.Count);
            foreach (var item in fundScheme.FundSummaryDtl)
            {
                var colIndex = 1;
                var rowIndex = startIndex + item.RowIndex;
                if (item.Year > 0 && item.Month > 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = string.Format("{0}年{1}月", item.Year, item.Month);
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = string.Empty;
                }
                grid.Cell(rowIndex, colIndex++).Text = item.JobNameLink;
                if (!gridStartCols.ContainsKey(grid))
                {
                    gridStartCols.Add(grid, colIndex - 1);
                }
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentVoluntarilyGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInnerSetUpGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubContractGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalVoluntarilyGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalInnerSetUpGether);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalSubContractGether);

                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentVoluntarilyPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInnerSetupPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSubcontractorPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalVoluntarilyPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalInnerSetupPay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalSubcontractorPay);

                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOutputTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentInputTax);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentUnPayVAT);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentPayedVAT);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentSurchargePay);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentOtherPay);

                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentBalance);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalBalance);
                grid.Cell(rowIndex, colIndex++).Text = string.Format("{0:0.00%}", item.BalanceRate);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrencyHandin);
                grid.Cell(rowIndex, colIndex).Text = FormarteCellValue(item.TargetStock);
            }

            SetGridReadOnlyStyle(grid, startIndex + 1);
            SetDataAreaStyle(grid, 3);
        }

        public void LoadFundContrast(CustomFlexGrid grid, FundPlanningMaster fundScheme)
        {
            if (grid == null || fundScheme == null || fundScheme.FundCalculateContrastDtl.Count == 0)
            {
                return;
            }

            SetReportProjectNameAndUnit(grid, 2, fundScheme);

            var startIndex = 5;
            if (!gridStartRows.ContainsKey(grid))
            {
                gridStartRows.Add(grid, startIndex);
            }
            grid.InsertRow(startIndex + 1, fundScheme.FundCalculateContrastDtl.Count);
            foreach (var item in fundScheme.FundCalculateContrastDtl)
            {
                var colIndex = 1;
                var rowIndex = startIndex + item.RowIndex;
                if (item.Year > 0 && item.Month > 0)
                {
                    grid.Cell(rowIndex, colIndex++).Text = string.Format("{0}年{1}月", item.Year, item.Month);
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = item.JobNameLink;
                }

                if (!gridStartCols.ContainsKey(grid))
                {
                    gridStartCols.Add(grid, colIndex - 1);
                }
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentGethering);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalGethering);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentPayment);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalPayment);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.CurrentBalance);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.TotalBalance);

                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.SchemeCurrentGethering);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.SchemeTotalGethering);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.SchemeCurrentPayment);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.SchemeTotalPayment);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.SchemeCurrentBalance);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.SchemeTotalBalance);

                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.SchemeYearGethering);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.SchemeYearPayment);
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.SchemeYearFlow);
                grid.Cell(rowIndex, colIndex).Text = FormarteCellValue(item.ContrastEffect);
            }

            SetGridReadOnlyStyle(grid, startIndex + 1);
            SetDataAreaStyle(grid, 2);
        }

        public void LoadFundCashCostRate(CustomFlexGrid grid, FundPlanningMaster fundScheme)
        {
            if (grid == null || fundScheme == null || fundScheme.CashCostRateCalculationDtl.Count == 0)
            {
                return;
            }

            foreach (var item in fundScheme.CashCostRateCalculationDtl)
            {
                if (item.DataType == 1)
                {
                    grid.Cell(item.RowIndex, 2).Text = item.SecondCategory;
                    if (item.SecondCategory.Contains("率"))
                    {
                        grid.Cell(item.RowIndex, 3).Text = 
                            string.Format("{0:0.00%}", item.CostMoney > 1 ? item.CostMoney/100 : item.CostMoney);
                    }
                    else
                    {
                        grid.Cell(item.RowIndex, 3).Text = FormarteCellValue(item.CostMoney);
                    }
                }
                else
                {
                    var colIndex = 1;
                    grid.Cell(item.RowIndex, 2).Tag = item.ItemGuid;
                    if (!string.IsNullOrEmpty(item.FisrtCategory))
                    {
                        grid.Cell(item.RowIndex, colIndex++).Text = item.FisrtCategory;
                    }
                    grid.Cell(item.RowIndex, colIndex++).Text = item.SecondCategory;
                    if (item.DataType == 2)
                    {
                        grid.Cell(item.RowIndex, colIndex++).Text = FormarteCellValue(item.CostMoney);
                    }
                    else
                    {
                        grid.Cell(item.RowIndex, colIndex++).Text = string.Format("{0:0.00%}", item.CostMoney);
                    }
                    grid.Cell(item.RowIndex, colIndex++).Text = string.Format("{0:0.00%}", item.CostProportion);
                    grid.Cell(item.RowIndex, colIndex++).Text = string.Format("{0:0.00%}", item.CashRateUnCompleted);
                    grid.Cell(item.RowIndex, colIndex++).Text = string.Format("{0:0.00%}", item.CashRateCompleted);
                    grid.Cell(item.RowIndex, colIndex++).Text = string.Format("{0:0.00%}", item.CostRateUnCompleted);
                    grid.Cell(item.RowIndex, colIndex).Text = string.Format("{0:0.00%}", item.CostRateCompleted);
                }
            }

            SetGridReadOnlyStyle(grid, 1);
        }

        public void LoadIndirectTaxRate(CustomFlexGrid grid, FundPlanningMaster fundScheme)
        {
            if (grid == null || fundScheme == null || fundScheme.IndirectInputCalculate.Count == 0)
            {
                return;
            }

            grid.Cell(2, 5).Text = string.Format("工期：{0}", fundScheme.ContractDuration);
            SetReportProjectNameAndUnit(grid, 2, fundScheme);

            var startRowIndex = 3;
            if (!gridStartRows.ContainsKey(grid))
            {
                gridStartRows.Add(grid, startRowIndex);
            }
            foreach (var item in fundScheme.IndirectInputCalculate)
            {
                var colIndex = 1;
                var rowIndex = startRowIndex + item.RowIndex;

                grid.Cell(rowIndex, 7).Tag = item.ItemGuid;
                grid.Cell(rowIndex, colIndex++).Text = item.SerialNumber.ToString();
                grid.Cell(rowIndex, colIndex).Tag = item.FirstSubjectCode;
                grid.Cell(rowIndex, colIndex++).Text = item.FirstSubjectName;
                grid.Cell(rowIndex, colIndex).Tag = item.SecondSubjectCode;
                grid.Cell(rowIndex, colIndex++).Text = item.SecondSubjectName;
                grid.Cell(rowIndex, colIndex).Tag = item.ThirdSubjectCode;
                grid.Cell(rowIndex, colIndex++).Text = item.ThirdSubjectName;
                grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.AppropriationBudget);
                grid.Cell(rowIndex, colIndex++).Text = item.InputTax.ToString("P2");
                if (rowIndex == grid.Rows - 1)
                {
                    grid.Cell(rowIndex, colIndex++).Text =
                        item.DeductibleInput == 0 ? "-" : FormarteCellValue(item.DeductibleInput, "P2");
                }
                else
                {
                    grid.Cell(rowIndex, colIndex++).Text = FormarteCellValue(item.DeductibleInput);
                }
                grid.Cell(rowIndex, colIndex).Text = item.CompilationBasis;

                if (rowIndex >= grid.Rows - 2)
                {
                    grid.Cell(rowIndex, 1).Text = item.FirstSubjectName;
                }
            }

            SetGridReadOnlyStyle(grid, startRowIndex);
        }

        #endregion
    }
}