using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Domain
{
    /// <summary>
    /// 费用划账单
    /// </summary>
    [Serializable]
    public class ExpensesRowBill : BaseMaster
    {
        private DateTime issueDate;
        private string paymentUnit;
        private string collectionUnit;
        private string digest;
        private Double totalMoney;

        /// <summary>
        /// 签发日期
        /// </summary>
        virtual public DateTime IssueDate
        {
            get { return issueDate; }
            set { issueDate = value; }
        }
        /// <summary>
        /// 付款单位
        /// </summary>
        virtual public string PaymentUnit
        {
            get { return paymentUnit; }
            set { paymentUnit = value; }
        }
        /// <summary>
        /// 收款单位
        /// </summary>
        virtual public string CollectionUnit
        {
            get { return collectionUnit; }
            set { collectionUnit = value; }
        }
        /// <summary>
        /// 摘要
        /// </summary>
        virtual public string Digest
        {
            get { return digest; }
            set { digest = value; }
        }
        /// <summary>
        /// 总金额
        /// </summary>
        virtual public Double TotalMoney
        {
            get { return totalMoney; }
            set { totalMoney = value; }
        }
    }
}
