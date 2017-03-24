using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;
namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    [Entity]
    public class ProjectFundPlanDetail : BaseDetail
    {
        private decimal _quota;
        /// <summary>分配额</summary>
        public virtual decimal Quota
        {
            get { return _quota; }
            set { _quota = value; }
        }

        private decimal _contractAmount;
        /// <summary>合同额</summary>
        public virtual decimal ContractAmount
        {
            get { return _contractAmount; }
            set { _contractAmount = value; }
        }

        private decimal _contractPaymentRatio;
        /// <summary>合同付款比例</summary>
        public virtual decimal ContractPaymentRatio
        {
            get { return _contractPaymentRatio; }
            set { _contractPaymentRatio = value; }
        }

        private decimal _planPayment;
        /// <summary>计划付款</summary>
        public virtual decimal PlanPayment
        {
            get { return _planPayment; }
            set { _planPayment = value; }
        }

        private decimal _planPaymentRatio;
        /// <summary>计划后付款比例</summary>
        public virtual decimal PlanPaymentRatio
        {
            get { return _planPaymentRatio; }
            set { _planPaymentRatio = value; }
        }

        private decimal _cumulativeExpireDue;
        /// <summary>累计到期应付款</summary>
        public virtual decimal CumulativeExpireDue
        {
            get { return _cumulativeExpireDue; }
            set { _cumulativeExpireDue = value; }
        }

        private decimal _cumulativePayment;
        /// <summary>累计付款</summary>
        public virtual decimal CumulativePayment
        {
            get { return _cumulativePayment; }
            set { _cumulativePayment = value; }
        }

        private decimal _cumulativeSettlement;
        /// <summary>累计结算</summary>
        public virtual decimal CumulativeSettlement
        {
            get { return _cumulativeSettlement; }
            set { _cumulativeSettlement = value; }
        }

        private decimal _cumulativeArrears;
        /// <summary>累计欠款额</summary>
        public virtual decimal CumulativeArrears
        {
            get { return _cumulativeArrears; }
            set { _cumulativeArrears = value; }
        }

        private decimal _precedingMonthSettlement;
        /// <summary>上月结算额</summary>
        public virtual decimal PrecedingMonthSettlement
        {
            get { return _precedingMonthSettlement; }
            set { _precedingMonthSettlement = value; }
        }

        private decimal _actualPayment;
        /// <summary>实际支付额</summary>
        public virtual decimal ActualPayment
        {
            get { return _actualPayment; }
            set { _actualPayment = value; }
        }

        private int _orderNumber;
        /// <summary>序号</summary>
        public virtual int OrderNumber
        {
            get { return _orderNumber; }
            set { _orderNumber = value; }
        }

        private decimal _paymentRatio;
        /// <summary>已支付比例</summary>
        public virtual decimal PaymentRatio
        {
            get { return _paymentRatio; }
            set { _paymentRatio = value; }
        }

        private string _creditorUnitLeadingOfficial;
        /// <summary>债权单位名称及负责人</summary>
        public virtual string CreditorUnitLeadingOfficial
        {
            get { return _creditorUnitLeadingOfficial; }
            set { _creditorUnitLeadingOfficial = value; }
        }

        private string _fundPaymentCategory;
        /// <summary>资金支出类别</summary>
        public virtual string FundPaymentCategory
        {
            get { return _fundPaymentCategory; }
            set { _fundPaymentCategory = value; }
        }

        private string _jobContent;
        /// <summary>工作内容</summary>
        public virtual string JobContent
        {
            get { return _jobContent; }
            set { _jobContent = value; }
        }

        private decimal _cumulativeExcutePay;
        /// <summary>
        /// 累计支付执行金额
        /// </summary>
        public virtual decimal CumulativeExcutePay
        {
            get { return _cumulativeExcutePay; }
            set { _cumulativeExcutePay = value; }
        }
    }
}
