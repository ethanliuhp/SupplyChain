using System;
using VirtualMachine.Core.Attributes;
namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    [Serializable]
    [Entity]
    public class FundSchemeCashCostRate:Base.Domain.BaseDetail
    {
        private string _fisrtCategory;
        /// <summary>一级成本分类</summary>
        public virtual string FisrtCategory
        {
            get { return _fisrtCategory; }
            set { _fisrtCategory = value; }
        }

        private string _secondCategory;
        /// <summary>二级成本分类</summary>
        public virtual string SecondCategory
        {
            get { return _secondCategory; }
            set { _secondCategory = value; }
        }

        private decimal _costMoney;
        /// <summary>成本金额</summary>
        public virtual decimal CostMoney
        {
            get { return _costMoney; }
            set { _costMoney = value; }
        }

        private decimal _costProportion;
        /// <summary>成本占比</summary>
        public virtual decimal CostProportion
        {
            get { return _costProportion; }
            set { _costProportion = value; }
        }

        private decimal _cashRateUnCompleted;
        /// <summary>主体结构未完过程中合同平均付现率</summary>
        public virtual decimal CashRateUnCompleted
        {
            get { return _cashRateUnCompleted; }
            set { _cashRateUnCompleted = value; }
        }

        private decimal _cashRateCompleted;
        /// <summary>主体结构已完过程中合同平均付现率</summary>
        public virtual decimal CashRateCompleted
        {
            get { return _cashRateCompleted; }
            set { _cashRateCompleted = value; }
        }

        private decimal _costRateUnCompleted;
        /// <summary>主体结构未完平均付现成本率</summary>
        public virtual decimal CostRateUnCompleted
        {
            get { return _costRateUnCompleted; }
            set { _costRateUnCompleted = value; }
        }

        private decimal _costRateCompleted;
        /// <summary>主体结构已完平均付现成本率</summary>
        public virtual decimal CostRateCompleted
        {
            get { return _costRateCompleted; }
            set { _costRateCompleted = value; }
        }

        private int _rowIndex;
        /// <summary>行号</summary>
        public virtual int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        private string _itemGuid;
        /// <summary>行标识</summary>
        public virtual string ItemGuid
        {
            get { return _itemGuid; }
            set { _itemGuid = value; }
        }

        private int _dataType;
        /// <summary>数据类型 1收入、成本、利润测算 2付现成本率测算 3收支平衡点测算</summary>
        public virtual int DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
    }
}
