using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public class FlexCellData
    {
        private int row = 0;
        private int col = 0;
        private double result = 0;
        private bool ifCalCell = false;
        private string measureId;
        private string linkOtherDimId;
        private string express;

        /// <summary>
        /// 行索引
        /// </summary>
        virtual public int Row
        {
            get { return row; }
            set { row = value; }
        }

        /// <summary>
        /// 列索引
        /// </summary>
        virtual public int Col
        {
            get { return col; }
            set { col = value; }
        }

        /// <summary>
        /// 结果值
        /// </summary>
        virtual public double Result
        {
            get { return result; }
            set { result = value; }
        }

        /// <summary>
        /// 是否是计算值
        /// </summary>
        virtual public bool IfCalCell
        {
            get { return ifCalCell; }
            set { ifCalCell = value; }
        }

        /// <summary>
        /// 度量维度ID
        /// </summary>
        virtual public string MeasureId
        {
            get { return measureId; }
            set { measureId = value; }
        }

        /// <summary>
        /// 其他维度值的联接
        /// </summary>
        virtual public string LinkOtherDimId
        {
            get { return linkOtherDimId; }
            set { linkOtherDimId = value; }
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        virtual public string Express
        {
            get { return express; }
            set { express = value; }
        }
    }
}
