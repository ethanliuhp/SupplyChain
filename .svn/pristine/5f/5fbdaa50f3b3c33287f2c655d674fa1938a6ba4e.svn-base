using System;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    [Serializable]
    [Entity]
    public class FundSchemeFinanceFee : Base.Domain.BaseDetail
    {
        private int _year;

        /// <summary>年份</summary>
        public virtual int Year
        {
            get { return _year; }
            set { _year = value; }
        }

        private int _month;

        /// <summary>月份</summary>
        public virtual int Month
        {
            get { return _month; }
            set { _month = value; }
        }

        private string _jobNameLink;

        /// <summary>施工任务名称联接</summary>
        public virtual string JobNameLink
        {
            get { return _jobNameLink; }
            set { _jobNameLink = value; }
        }

        private decimal _totalGethering;

        /// <summary>累计收款</summary>
        public virtual decimal TotalGethering
        {
            get { return _totalGethering; }
            set { _totalGethering = value; }
        }

        private decimal _currentPayment;

        /// <summary>本期支出（不含财务费用）</summary>
        public virtual decimal CurrentPayment
        {
            get { return _currentPayment; }
            set { _currentPayment = value; }
        }

        private decimal _totalPayment;

        /// <summary>累计支出（不含财务费用）</summary>
        public virtual decimal TotalPayment
        {
            get { return _totalPayment; }
            set { _totalPayment = value; }
        }

        private decimal _totalBalance;

        /// <summary>累计资金结存（不含财务费用）</summary>
        public virtual decimal TotalBalance
        {
            get { return _totalBalance; }
            set { _totalBalance = value; }
        }

        private decimal _currentFinanceFee;

        /// <summary>本期应计财务费用</summary>
        public virtual decimal CurrentFinanceFee
        {
            get { return _currentFinanceFee; }
            set { _currentFinanceFee = value; }
        }

        private int _rowIndex;

        /// <summary>行号</summary>
        public virtual int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        private string _itemGuid;
        /// <summary>
        /// 行标识
        /// </summary>
        public virtual string ItemGuid
        {
            get { return _itemGuid; }
            set { _itemGuid = value; }
        }

        private decimal _currencyHandIn;
        /// <summary>
        /// 本期货币上交
        /// </summary>
        public virtual decimal CurrencyHandIn
        {
            get { return _currencyHandIn; }
            set { _currencyHandIn = value; }
        }

        private decimal _totalCurrencyHandIn;
        /// <summary>
        /// 累计货币上交
        /// </summary>
        public virtual decimal TotalCurrencyHandIn
        {
            get { return _totalCurrencyHandIn; }
            set { _totalCurrencyHandIn = value; }
        }
    }
}
