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
    //收款发票主表
    [Serializable]
    [Entity]
    public class GatheringInvoice : BaseMaster
    {
        private string invoiceCode;
        private string invoiceNo;
        private string gatheringID;
        private CustomerRelationInfo theCustomerRelationInfo;
        private string theCustomerName;
        private decimal sumMoney;
        private GatheringMaster master;

        /// <summary>
        /// 发票代码
        /// </summary>
        virtual public string InvoiceCode
        {
            get { return invoiceCode; }
            set { invoiceCode = value; }
        }

        /// <summary>
        /// 发票号码
        /// </summary>
        virtual public string InvoiceNo
        {
            get { return invoiceNo; }
            set { invoiceNo = value; }
        }

        /// <summary>
        /// 收款ID
        /// </summary>
        virtual public string GatheringID
        {
            get { return gatheringID; }
            set { gatheringID = value; }
        }

        /// <summary>
        /// 客户
        /// </summary>
        virtual public CustomerRelationInfo TheCustomerRelationInfo
        {
            get { return theCustomerRelationInfo; }
            set { theCustomerRelationInfo = value; }
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        virtual public string TheCustomerName
        {
            get { return theCustomerName; }
            set { theCustomerName = value; }
        }
        /// <summary>
        /// 金额 
        /// </summary>
        override public decimal SumMoney
        {
            get { return sumMoney; }
            set { sumMoney = value; }
        }
        /// <summary>
        /// 收款单 
        /// </summary>
        virtual public GatheringMaster Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
