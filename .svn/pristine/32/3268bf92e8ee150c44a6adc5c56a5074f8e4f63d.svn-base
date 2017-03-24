using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 指标值区间定义
    /// </summary>
    [Serializable]
    [Entity]
    public class DimensionScope
    {
        private string id;
        private long version = -1;
        private DimensionCategory category;
        private int scopeType;
        private string scopeName;
        private double beginValue;
        private double endValue;
        private double score;
        private string remark;

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
        /// 指标分类定义
        /// </summary>
        virtual public DimensionCategory Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// 区间类型
        /// </summary>
        virtual public int ScopeType
        {
            get { return scopeType; }
            set { scopeType = value; }
        }

        /// <summary>
        /// 区间名称
        /// </summary>
        virtual public string ScopeName
        {
            get { return scopeName; }
            set { scopeName = value; }
        }

        /// <summary>
        /// 开始值
        /// </summary>
        virtual public double BeginValue
        {
            get { return beginValue; }
            set { beginValue = value; }
        }

        /// <summary>
        /// 结束值
        /// </summary>
        virtual public double EndValue
        {
            get { return endValue; }
            set { endValue = value; }
        }

        /// <summary>
        /// 分值
        /// </summary>
        virtual public double Score
        {
            get { return score; }
            set { score = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
    }
}
