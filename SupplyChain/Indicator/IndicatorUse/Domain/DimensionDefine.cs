using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 维度定义Domain
    /// </summary>
    [Serializable]
    [Entity]
    public class DimensionDefine
    {
        private string id;
        private long version = -1;
        private int state = 1;
        private DimensionCategory category;
        private string name;
        private string calExpression;
        private decimal factor;
        private string calTypeCode;
        private string calTypeName;

        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        virtual public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 维度分类
        /// </summary>
        virtual public DimensionCategory Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// 维度名称
        /// </summary>
        virtual public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 计算类型
        /// </summary>
        virtual public string CalTypeCode
        {
            get { return calTypeCode; }
            set { calTypeCode = value; }
        }

        /// <summary>
        /// 计算类型名称
        /// </summary>
        virtual public string CalTypeName
        {
            get { return calTypeName; }
            set { calTypeName = value; }
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
        /// 权重
        /// </summary>
        virtual public decimal Factor
        {
            get { return factor; }
            set { factor = value; }
        }

    }
}
