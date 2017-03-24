using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    [Entity]
    public class BorrowedOrderDetail : BaseDetail
    {
        private string borrowedType;
        private string borrowedPurpose;
        private DateTime borrowedDate;
        private DateTime createTime;

        /// <summary>
        /// 借款性质
        /// </summary>
        public virtual string BorrowedType
        {
            get { return borrowedType; }
            set { borrowedType = value; }
        }

        /// <summary>
        /// 借款用途
        /// </summary>
        public virtual string BorrowedPurpose
        {
            get { return borrowedPurpose; }
            set { borrowedPurpose = value; }
        }

        /// <summary>
        /// 借款日期
        /// </summary>
        public virtual DateTime BorrowedDate
        {
            get { return borrowedDate; }
            set { borrowedDate = value; }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        private DateTime? refundDate;

        /// <summary>
        /// 还款时间
        /// </summary>
        public virtual  DateTime? RefundDate
        {
            get { return refundDate; }
            set { refundDate = value; }
        }

        private decimal cashMoney;

        /// <summary>
        /// 现金
        /// </summary>
        public virtual decimal CashMoney
        {
            get { return cashMoney; }
            set { cashMoney = value; }
        }

        private decimal checkMoney;

        /// <summary>
        /// 支票金额
        /// </summary>
        public virtual decimal CheckMoney
        {
            get { return checkMoney; }
            set { checkMoney = value; }
        }

        private string checkNo;

        /// <summary>
        /// 支票号
        /// </summary>
        public virtual string CheckNo
        {
            get { return checkNo; }
            set { checkNo = value; }
        }

        private decimal exchangeMoney;

        /// <summary>
        /// 汇票金额
        /// </summary>
        public virtual decimal ExchangeMoney
        {
            get { return exchangeMoney; }
            set { exchangeMoney = value; }
        }

        private string exchangeNo;

        /// <summary>
        /// 汇票号
        /// </summary>
        public virtual string ExchangeNo
        {
            get { return exchangeNo; }
            set { exchangeNo = value; }
        }
    }
}
