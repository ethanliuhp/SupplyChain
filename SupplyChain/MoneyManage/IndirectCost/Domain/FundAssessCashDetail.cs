using System;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    [Entity]
    public class FundAssessCashDetail : Base.Domain.BaseDetail
    {
        private decimal _centralPurchase;

        /// <summary>集采应付欠款</summary>
        public virtual decimal CentralPurchase
        {
            get { return _centralPurchase; }
            set { _centralPurchase = value; }
        }

        private decimal _innerInstall;

        /// <summary>应付内部安装欠款</summary>
        public virtual decimal InnerInstall
        {
            get { return _innerInstall; }
            set { _innerInstall = value; }
        }

        private decimal _otherContractPay;

        /// <summary>其他需按合同应付欠款</summary>
        public virtual decimal OtherContractPay
        {
            get { return _otherContractPay; }
            set { _otherContractPay = value; }
        }

        private decimal _otherAdjust;

        /// <summary>其他调整加减项</summary>
        public virtual decimal OtherAdjust
        {
            get { return _otherAdjust; }
            set { _otherAdjust = value; }
        }

        private decimal _realCashBalance;

        /// <summary>本月调整后实际资金结存</summary>
        public virtual decimal RealCashBalance
        {
            get { return _realCashBalance; }
            set { _realCashBalance = value; }
        }

        private decimal _assessCardinal;

        /// <summary>月度资金存量考核基数</summary>
        public virtual decimal AssessCardinal
        {
            get { return _assessCardinal; }
            set { _assessCardinal = value; }
        }

        private decimal _cashMoney;

        /// <summary>兑现金额</summary>
        public virtual decimal CashMoney
        {
            get { return _cashMoney; }
            set { _cashMoney = value; }
        }

        private decimal _deductionItem;

        /// <summary>扣减项</summary>
        public virtual decimal DeductionItem
        {
            get { return _deductionItem; }
            set { _deductionItem = value; }
        }

        private string _warnLevel;

        /// <summary>风险预警等级</summary>
        public virtual string WarnLevel
        {
            get { return _warnLevel; }
            set { _warnLevel = value; }
        }

        private decimal _warnRate;

        /// <summary>风险预警扣减比例</summary>
        public virtual decimal WarnRate
        {
            get { return _warnRate; }
            set { _warnRate = value; }
        }

        private decimal _approvalRate;

        /// <summary>确权率</summary>
        public virtual decimal ApprovalRate
        {
            get { return _approvalRate; }
            set { _approvalRate = value; }
        }

        private decimal _approvalDeduction;

        /// <summary>确权率扣减比例</summary>
        public virtual decimal ApprovalDeduction
        {
            get { return _approvalDeduction; }
            set { _approvalDeduction = value; }
        }

        private decimal _assessCashMoney;

        /// <summary>考核兑现金额</summary>
        public virtual decimal AssessCashMoney
        {
            get { return _assessCashMoney; }
            set { _assessCashMoney = value; }
        }
    }
}
