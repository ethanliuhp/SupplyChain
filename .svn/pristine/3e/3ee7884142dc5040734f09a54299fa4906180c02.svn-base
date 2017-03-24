using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using IRPServiceModel.Domain.Basic;

namespace IRPServiceModel.Domain.PaymentOrder
{
    public enum EnumPaymentType
    {
        银行转帐 = 0,
        现金付款 = 1
    }
   /// <summary>
    ///  付款单主表
   /// </summary>
    [Serializable]

    public class PaymentOrderMaster : BasicMasterBill
    {
        private EnumPaymentType paymentType;
        private PersonInfo payee;
        private string payeeName;
        private string theBankCode;
        private string theBankName;
        private decimal money;
        private string describe;
        private Iesi.Collections.Generic.ISet<PaymentOrderDetial> details = new Iesi.Collections.Generic.HashedSet<PaymentOrderDetial>();
       
        /// <summary>
        /// 付款金额
        /// </summary>
        virtual public decimal Money
        {
            get
            {
                money = 0;
                if (details != null)
                {
                    foreach (PaymentOrderDetial oDetial in details)
                    {
                        money += oDetial.Money;
                    }
                }
                return money;
            }
            set { money = value; }
        }

        /// <summary>
        /// 付款方式
        /// </summary>
        virtual public EnumPaymentType PaymentType
        {
            get { return paymentType; }
            set { paymentType = value; }
        }
     
   
        /// <summary>
        /// 收款方
        /// </summary>
        virtual public PersonInfo Payee
        {
            get { return payee; }
            set { payee = value; }
        }
        /// <summary>
        /// 收款方名称
        /// </summary>
        virtual public string  PayeeName
        {
            get { return payeeName; }
            set { payeeName = value; }
        }
     
        /// <summary>
        /// 银行账号
        /// </summary>
        virtual public string TheBankCode
        {
            get { return theBankCode; }
            set { theBankCode = value; }
        }
        /// <summary>
        /// 银行名称
        /// </summary>
        virtual public string TheBankName
        {
            get { return theBankName; }
            set { theBankName = value; }
        }
        /// <summary>
        /// 明细
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<PaymentOrderDetial> Details
        {
            get { return details; }
            set { details = value; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Describe
        {
            get { return describe; }
            set { describe = value; }
        }

    }
}

 
