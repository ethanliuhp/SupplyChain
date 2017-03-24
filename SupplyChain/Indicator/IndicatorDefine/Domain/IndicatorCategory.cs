using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorDefine.Domain
{
    /// <summary>
    /// 指标分类
    /// </summary>
    [Serializable]
    [Entity]
    public class IndicatorCategory : CategoryNode
    {
        private IList indicatorDefinitions =new ArrayList();

        /// <summary>
        /// 指标定义
        /// </summary>
        virtual public IList IndicatorDefinitions
        {
            get { return indicatorDefinitions; }
            set { indicatorDefinitions = value; }
        }
    }
}
