using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Iesi.Collections.Generic;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    //票据主表
    [Serializable]
    [Entity]
    public class AcceptanceBill : BaseMaster
    {
        private string billType;
        private string billNo;
        private string acceptPerson;
        private DateTime expireDate;
        private decimal sumMoney;
        private PaymentDetail paymentMxId;
        private GatheringDetail gatheringMxId;

        /// <summary>
        /// 收款明细ID
        /// </summary>
        virtual public GatheringDetail GatheringMxId
        {
            get { return gatheringMxId; }
            set { gatheringMxId = value; }
        }
        /// <summary>
        /// 付款明细ID
        /// </summary>
        virtual public PaymentDetail PaymentMxId
        {
            get { return paymentMxId; }
            set { paymentMxId = value; }
        }
        /// <summary>
        /// 出票人
        /// </summary>
        virtual public string AcceptPerson
        {
            get { return acceptPerson; }
            set { acceptPerson = value; }
        }
        /// <summary>
        /// 票据类型
        /// </summary>
        virtual public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }

        /// <summary>
        /// 票据号码
        /// </summary>
        virtual public string BillNo
        {
            get { return billNo; }
            set { billNo = value; }
        }

        /// <summary>
        /// 票据到期日
        /// </summary>
        virtual public DateTime ExpireDate
        {
            get { return expireDate; }
            set { expireDate = value; }
        }

        /// <summary>
        /// 金额 
        /// </summary>
        virtual public decimal SumMoney
        {
            get { return sumMoney; }
            set { sumMoney = value; }
        }
    }
}
