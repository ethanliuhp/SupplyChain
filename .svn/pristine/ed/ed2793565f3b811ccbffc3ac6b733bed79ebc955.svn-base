using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 维度分类
    /// </summary>
    [Serializable]
    [Entity]
    public class DimensionCategory : CategoryNode
    {
        private IList dimensionDefines = new ArrayList();//维度值
        private IList dimensionScope =new ArrayList();//指标区间定义
        private string dimRegId;
        private string resourceId;
        private string calExpression;
        private decimal factor = 0;
        private string calTypeCode;
        private string calTypeName;
        private string code;//别名
        private string dimUnit;//计量单位
        private string dimUnitName;//计量单位名称
        private string ifEconomy;//是否技经指标

        /// <summary>
        /// 计算类型名称
        /// </summary>
        virtual public string CalTypeName
        {
            get { return calTypeName; }
            set { calTypeName = value; }
        }

        /// <summary>
        /// 计算类型编码
        /// </summary>
        virtual public string CalTypeCode
        {
            get { return calTypeCode; }
            set { calTypeCode = value; }
        }
        /// <summary>
        /// 别名
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }
        /// <summary>
        /// 计量单位
        /// </summary>
        virtual public string DimUnit
        {
            get { return dimUnit; }
            set { dimUnit = value; }
        }
        /// <summary>
        /// 计量单位名称
        /// </summary>
        virtual public string DimUnitName
        {
            get { return dimUnitName; }
            set { dimUnitName = value; }
        }
        /// <summary>
        /// 是否技经指标
        /// </summary>
        virtual public string IfEconomy
        {
            get { return ifEconomy; }
            set { ifEconomy = value; }
        }

        /// <summary>
        /// 权重
        /// </summary>
        virtual public decimal Factor
        {
            get { return factor; }
            set { factor = value; }
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        virtual public string CalExpression
        {
            get { return calExpression; }
            set { calExpression = value; }
        }

        /// <summary>
        /// 保存外部来源的维度的资源Id
        /// </summary>
        virtual public string ResourceId
        {
            get { return resourceId; }
            set { resourceId = value; }
        }

        /// <summary>
        /// 维度注册表
        /// </summary>
        virtual public string DimRegId
        {
            get { return dimRegId; }
            set { dimRegId = value; }
        }

        /// <summary>
        /// 维度
        /// </summary>
        virtual public IList DimensionDefines
        {
            get { return dimensionDefines; }
            set { dimensionDefines = value; }
        }

        /// <summary>
        /// 指标区间定义
        /// </summary>
        virtual public IList DimensionScope
        {
            get { return dimensionScope; }
            set { dimensionScope = value; }
        }

    }
}
