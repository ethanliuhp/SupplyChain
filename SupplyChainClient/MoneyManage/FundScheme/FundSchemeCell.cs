using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage
{
    public class FundSchemeCell
    {
        private FundSchemeCell()
        {

        }

        public FundSchemeCell(string tName, string shortName, int rIndex, int cIndex)
            : this()
        {
            TableName = tName;
            ShortName = shortName;
            RowIndex = rIndex;
            ColIndex = cIndex;
        }

        /// <summary>
        /// 单元格所在表全称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表名简称
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// 单元格行号
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 单元格列号
        /// </summary>
        public int ColIndex { get; set; }

        /// <summary>
        /// 单元格值
        /// </summary>
        public decimal? CellValue { get; set; }

        /// <summary>
        /// 单元格绑定对象
        /// </summary>
        public object BindObject { get; set; }

        /// <summary>
        /// 单元格绑定属性
        /// </summary>
        public string DataMember { get; set; }

        /// <summary>
        /// 格式化输出，P2
        /// </summary>
        public string Formatter { get; set; }

        /// <summary>
        /// 单元格公式
        /// </summary>
        public FundSchemeFormula Formula { get; set; }

        public string CellAddress
        {
            get { return string.Format("![{0}_{1}_{2}]", ShortName, RowIndex, ColIndex); }
        }

        public void UpdateBindObjectValue()
        {
            var tp = BindObject.GetType();
            var prs = tp.GetProperties();
            foreach (var pi in prs)
            {
                if(pi.Name.Equals(DataMember))
                {
                    pi.SetValue(BindObject, CellValue, null);
                    break;
                }
            }
        }

        public static string ToCellAddress(string shortName,int rIndex,int cIndex)
        {
            return string.Format("![{0}_{1}_{2}]", shortName, rIndex, cIndex);
        }
    }
}
