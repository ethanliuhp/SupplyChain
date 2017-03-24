using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    //付款明细
    [Serializable]
    [Entity]
    public class PaymentDetail : BaseDetail
    {
        private decimal paymentMoney;
        private AcceptanceBill acceptBillID;
        private decimal inMaterialMoney;
        private decimal liveMoney;
        private decimal otherMoney;

        /// <summary>
        /// 付款金额
        /// </summary>
        public virtual decimal PaymentMoney
        {
            get { return paymentMoney; }
            set { paymentMoney = value; }
        }

        /// <summary>
        /// 关联票据ID
        /// </summary>
        public virtual AcceptanceBill AcceptBillID
        {
            get { return acceptBillID; }
            set { acceptBillID = value; }
        }

        /// <summary>
        /// 调入材料
        /// </summary>
        public virtual decimal InMaterialMoney
        {
            get { return inMaterialMoney; }
            set { inMaterialMoney = value; }
        }

        /// <summary>
        /// 生活费用
        /// </summary>
        public virtual decimal LiveMoney
        {
            get { return liveMoney; }
            set { liveMoney = value; }
        }

        /// <summary>
        /// 其他款项目
        /// </summary>
        public virtual decimal OtherMoney
        {
            get { return otherMoney; }
            set { otherMoney = value; }
        }

    }
}
