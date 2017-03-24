using System;
using Iesi.Collections.Generic;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    [Serializable]
    [Entity]
    public class PaymentRequest : BaseMaster
    {
        private string _requestType;

        /// <summary>申请类型</summary>
        public virtual string RequestType
        {
            get { return _requestType; }
            set { _requestType = value; }
        }

        private decimal _currentPlanGether;

        /// <summary>本月计划收款</summary>
        public virtual decimal CurrentPlanGether
        {
            get { return _currentPlanGether; }
            set { _currentPlanGether = value; }
        }

        private decimal _currentRealGether;

        /// <summary>本月实际收款</summary>
        public virtual decimal CurrentRealGether
        {
            get { return _currentRealGether; }
            set { _currentRealGether = value; }
        }

        private decimal _currentPlanPay;

        /// <summary>本月计划付款</summary>
        public virtual decimal CurrentPlanPay
        {
            get { return _currentPlanPay; }
            set { _currentPlanPay = value; }
        }

        private decimal _currentRealPay;

        /// <summary>本月实际付款</summary>
        public virtual decimal CurrentRealPay
        {
            get { return _currentRealPay; }
            set { _currentRealPay = value; }
        }

        private Iesi.Collections.Generic.ISet<PaymentMaster> _payments=new HashedSet<PaymentMaster>();

        /// <summary>
        /// 资金支付审批单
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<PaymentMaster> Payments
        {
            get { return _payments; }
            set { _payments = value; }
        }
    }
}
