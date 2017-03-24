using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public class FundSchemeFormula
    {
        private FundSchemeFormula()
        {
            IsComputed = false;
        }

        public FundSchemeFormula(FundSchemeCell cell)
            : this()
        {
            Cell = cell;
        }

        /// <summary>
        /// 公式表达式
        /// </summary>
        public string FormulaExpression { get; set; }

        /// <summary>
        /// 可计算表达式
        /// </summary>
        public string ComputeExpression { get; set; }

        /// <summary>
        /// 公式所在单元格
        /// </summary>
        public FundSchemeCell Cell { get; set; }

        /// <summary>
        /// 公式是否可以计算
        /// </summary>
        public bool IsCanCompute
        {
            get { return !string.IsNullOrEmpty(ComputeExpression) && !ComputeExpression.Contains("!"); }
        }

        /// <summary>
        /// 是否已经计算
        /// </summary>
        public bool IsComputed { get; set; }

        private decimal GetExpressionValue(string exp)
        {
            decimal tmp = 0;
            var result = CommonUtil.CalculateExpression(exp, 4);
            decimal.TryParse(result, out tmp);

            return tmp;
        }

        /// <summary>
        /// 计算公式
        /// </summary>
        public void Calculate()
        {
            if (!IsCanCompute)
            {
                return;
            }

            if (ComputeExpression.StartsWith("Max"))
            {
                var start = ComputeExpression.IndexOf('(') + 1;
                var values = ComputeExpression.Substring(start, ComputeExpression.Length - start - 1).Split(',');
                Cell.CellValue = Math.Max(GetExpressionValue(values[0]), GetExpressionValue(values[1]));
            }
            else
            {
                Cell.CellValue = GetExpressionValue(ComputeExpression);
            }

            Cell.UpdateBindObjectValue();

            IsComputed = true;
        }

        /// <summary>
        /// 汇总公式
        /// </summary>
        /// <param name="shortName">表名</param>
        /// <param name="startRow">起始行</param>
        /// <param name="endRow">结束行</param>
        /// <param name="startCol">起始列</param>
        /// <param name="endCol">结束列</param>
        /// <returns>(公式)</returns>
        public static string ToSumFormula(string shortName, int startRow, int endRow, int startCol, int endCol)
        {
            List<string> sb = new List<string>();
            for (int i = startRow; i <= endRow; i++)
            {
                for (var j = startCol; j <= endCol; j++)
                {
                    sb.Add(FundSchemeCell.ToCellAddress(shortName, i, j));
                }
            }

            return string.Format("({0})", string.Join(" + ", sb.ToArray()));
        }
    }
}
