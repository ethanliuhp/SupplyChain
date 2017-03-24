using System;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    [Entity]
    public class FundInterestDetail : Base.Domain.BaseDetail
    {
        private decimal _cashBalanceInScheme;

        /// <summary>策划内资金结余</summary>
        public virtual decimal CashBalanceInScheme
        {
            get { return _cashBalanceInScheme; }
            set { _cashBalanceInScheme = value; }
        }

        private decimal _cashBorrowInScheme;

        /// <summary>策划内资金借款</summary>
        public virtual decimal CashBorrowInScheme
        {
            get { return _cashBorrowInScheme; }
            set { _cashBorrowInScheme = value; }
        }

        private decimal _cashBalanceOutScheme;

        /// <summary>策划外资金结余</summary>
        public virtual decimal CashBalanceOutScheme
        {
            get { return _cashBalanceOutScheme; }
            set { _cashBalanceOutScheme = value; }
        }

        private decimal _cashBorrowOutScheme;

        /// <summary>策划外资金借款</summary>
        public virtual decimal CashBorrowOutScheme
        {
            get { return _cashBorrowOutScheme; }
            set { _cashBorrowOutScheme = value; }
        }

        private decimal _interestCost;

        /// <summary>未完工项目利息费用</summary>
        public virtual decimal InterestCost
        {
            get { return _interestCost; }
            set { _interestCost = value; }
        }

        private decimal _settlementMoney;
        /// <summary>结算金额</summary>
        public virtual decimal SettlementMoney
        {
            get { return _settlementMoney; }
            set { _settlementMoney = value; }
        }

        private decimal _receivableDebt;
        /// <summary>应收欠款</summary>
        public virtual decimal ReceivableDebt
        {
            get { return _receivableDebt; }
            set { _receivableDebt = value; }
        }

        private decimal _completeInterestCost;

        /// <summary>完工项目利息</summary>
        public virtual decimal CompleteInterestCost
        {
            get { return _completeInterestCost; }
            set { _completeInterestCost = value; }
        }
    }
}
