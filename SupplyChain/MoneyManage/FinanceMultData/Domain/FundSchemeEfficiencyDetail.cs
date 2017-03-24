using System;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    [Serializable]
    [Entity]
    public class FundSchemeEfficiencyDetail : Base.Domain.BaseDetail
    {
        private int _createYear;

        /// <summary>年份</summary>
        public virtual int CreateYear
        {
            get { return _createYear; }
            set { _createYear = value; }
        }

        private int _createMonth;

        /// <summary>月份</summary>
        public virtual int CreateMonth
        {
            get { return _createMonth; }
            set { _createMonth = value; }
        }

        private decimal _schemeIncome;

        /// <summary>本期策划收入目标</summary>
        public virtual decimal SchemeIncome
        {
            get { return _schemeIncome; }
            set { _schemeIncome = value; }
        }

        private decimal _totalSchemeIncome;

        /// <summary>累计策划收入目标</summary>
        public virtual decimal TotalSchemeIncome
        {
            get { return _totalSchemeIncome; }
            set { _totalSchemeIncome = value; }
        }

        private decimal _schemeGether;

        /// <summary>本期策划收款目标</summary>
        public virtual decimal SchemeGether
        {
            get { return _schemeGether; }
            set { _schemeGether = value; }
        }

        private decimal _totalSchemeGether;

        /// <summary>累计策划收款目标</summary>
        public virtual decimal TotalSchemeGether
        {
            get { return _totalSchemeGether; }
            set { _totalSchemeGether = value; }
        }

        private decimal _schemePay;

        /// <summary>本期策划支出目标</summary>
        public virtual decimal SchemePay
        {
            get { return _schemePay; }
            set { _schemePay = value; }
        }

        private decimal _totalSchemePay;

        /// <summary>累计策划支出目标</summary>
        public virtual decimal TotalSchemePay
        {
            get { return _totalSchemePay; }
            set { _totalSchemePay = value; }
        }

        private decimal _schemeBalance;

        /// <summary>本期策划存量目标</summary>
        public virtual decimal SchemeBalance
        {
            get { return _schemeBalance; }
            set { _schemeBalance = value; }
        }

        private decimal _totalSchemeBalance;

        /// <summary>累计策划存量目标</summary>
        public virtual decimal TotalSchemeBalance
        {
            get { return _totalSchemeBalance; }
            set { _totalSchemeBalance = value; }
        }

        private decimal _schemeMoneyDue;

        /// <summary>策划货币上交目标</summary>
        public virtual decimal SchemeMoneyDue
        {
            get { return _schemeMoneyDue; }
            set { _schemeMoneyDue = value; }
        }

        private decimal _schemeBalanceNoMoneyDue;

        /// <summary>货币上交后的目标存量</summary>
        public virtual decimal SchemeBalanceNoMoneyDue
        {
            get { return _schemeBalanceNoMoneyDue; }
            set { _schemeBalanceNoMoneyDue = value; }
        }

        private decimal _currentIncome;

        /// <summary>本期收入</summary>
        public virtual decimal CurrentIncome
        {
            get { return _currentIncome; }
            set { _currentIncome = value; }
        }

        private decimal _totalIncome;

        /// <summary>累计收入</summary>
        public virtual decimal TotalIncome
        {
            get { return _totalIncome; }
            set { _totalIncome = value; }
        }

        private decimal _incomeEfficiency;

        /// <summary>与策划对比成效</summary>
        public virtual decimal IncomeEfficiency
        {
            get { return _incomeEfficiency; }
            set { _incomeEfficiency = value; }
        }

        private decimal _currentGether;

        /// <summary>本期申报收款</summary>
        public virtual decimal CurrentGether
        {
            get { return _currentGether; }
            set { _currentGether = value; }
        }

        private decimal _getherReference;

        /// <summary>本期下达参考值</summary>
        public virtual decimal GetherReference
        {
            get { return _getherReference; }
            set { _getherReference = value; }
        }

        private decimal _currentGetherPlan;

        /// <summary>本期下达计划</summary>
        public virtual decimal CurrentGetherPlan
        {
            get { return _currentGetherPlan; }
            set { _currentGetherPlan = value; }
        }

        private decimal _realGether;

        /// <summary>本期实际收款</summary>
        public virtual decimal RealGether
        {
            get { return _realGether; }
            set { _realGether = value; }
        }

        private decimal _totalGether;

        /// <summary>累计收款</summary>
        public virtual decimal TotalGether
        {
            get { return _totalGether; }
            set { _totalGether = value; }
        }

        private decimal _getherEfficiency;

        /// <summary>收款策划成效对比</summary>
        public virtual decimal GetherEfficiency
        {
            get { return _getherEfficiency; }
            set { _getherEfficiency = value; }
        }

        private decimal _currentPay;

        /// <summary>本期资金计划申报支付金额</summary>
        public virtual decimal CurrentPay
        {
            get { return _currentPay; }
            set { _currentPay = value; }
        }

        private decimal _payReference;

        /// <summary>本期批复参考值</summary>
        public virtual decimal PayReference
        {
            get { return _payReference; }
            set { _payReference = value; }
        }

        private decimal _currentApprovePay;

        /// <summary>本期批复计划</summary>
        public virtual decimal CurrentApprovePay
        {
            get { return _currentApprovePay; }
            set { _currentApprovePay = value; }
        }

        private decimal _previewRealPay;

        /// <summary>支付上期资金计划</summary>
        public virtual decimal PreviewRealPay
        {
            get { return _previewRealPay; }
            set { _previewRealPay = value; }
        }

        private decimal _currentRealPay;

        /// <summary>本期资金计划执行支付金额</summary>
        public virtual decimal CurrentRealPay
        {
            get { return _currentRealPay; }
            set { _currentRealPay = value; }
        }

        private decimal _totalPay;

        /// <summary>累计支出</summary>
        public virtual decimal TotalPay
        {
            get { return _totalPay; }
            set { _totalPay = value; }
        }

        private decimal _payEfficiency;

        /// <summary>支付与策划对比成效</summary>
        public virtual decimal PayEfficiency
        {
            get { return _payEfficiency; }
            set { _payEfficiency = value; }
        }

        private decimal _lastTotalBalance;

        /// <summary>期末累计实际存量</summary>
        public virtual decimal LastTotalBalance
        {
            get { return _lastTotalBalance; }
            set { _lastTotalBalance = value; }
        }

        private decimal _centralPurchase;

        /// <summary>集采应付欠款</summary>
        public virtual decimal CentralPurchase
        {
            get { return _centralPurchase; }
            set { _centralPurchase = value; }
        }

        private decimal _innerInstall;

        /// <summary>按业主实际支付率应付内部安装欠款</summary>
        public virtual decimal InnerInstall
        {
            get { return _innerInstall; }
            set { _innerInstall = value; }
        }

        private decimal _otherContractPay;

        /// <summary>除集采、内部安装外其他需按合同应付欠款</summary>
        public virtual decimal OtherContractPay
        {
            get { return _otherContractPay; }
            set { _otherContractPay = value; }
        }

        private decimal _lastAdjustBalance;

        /// <summary>调整后期末存量</summary>
        public virtual decimal LastAdjustBalance
        {
            get { return _lastAdjustBalance; }
            set { _lastAdjustBalance = value; }
        }

        private decimal _balanceEfficiency;

        /// <summary>资金存量与策划对比成效</summary>
        public virtual decimal BalanceEfficiency
        {
            get { return _balanceEfficiency; }
            set { _balanceEfficiency = value; }
        }

    }
}
