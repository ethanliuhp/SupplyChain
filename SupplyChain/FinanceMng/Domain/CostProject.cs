using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.MaterialResource.Domain;
using System.ComponentModel;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.FinancialResource.RelateClass;
using Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Domain;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain
{

    [Serializable]
    [Entity]
     public class CostProject : CategoryNode
    {
        private string accLevelName;
        private enmCostType costType;
        private enmCostProjectType costProjectType;
        private int influxCodex;
        private AccountTitleInfo accTitle;
        private ExpenseItem expenseItem;
        private string unit;
        /// <summary>
        /// 费用项目
        /// </summary>
        virtual public ExpenseItem ExpenseItem
        {
            get { return expenseItem; }
            set { expenseItem = value; }
        }
        /// <summary>
        /// 费用分级名称
        /// </summary>
        virtual public string AccLevelName
        {
            get { return accLevelName; }
            set { accLevelName = value; }
        }
        //
        /// <summary>
        /// 费用分类
        /// </summary>
        virtual public enmCostType CostType
        { 
            get { return costType; }
            set { costType= value; }
        }
        /// <summary>
        /// 费用项目类型
        /// </summary>
        virtual public enmCostProjectType CostProjectType
        {
            get { return costProjectType; }
            set { costProjectType = value; }
        }
        /// <summary>
        /// 汇集规则 0 +   1 -
        /// </summary>
        virtual public int InfluxCodex
        {
            get { return influxCodex; }
            set { influxCodex = value; }
        }

        /// <summary>
        /// 费用项目对应科目
        /// </summary>
        virtual public AccountTitleInfo AccTitle
        {
            get { return accTitle; }
            set { accTitle = value; }
        }
        /// <summary>
        /// 单位
        /// </summary>
        virtual public string Unit
        {
            get { return unit; }
            set { unit = value; }
        }

    }
}
