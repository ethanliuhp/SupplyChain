using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Iesi.Collections.Generic;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    //付款单主表
    [Serializable]
    [Entity]
    public class PaymentMaster : BaseMaster
    {
        private string theSupplierRelationInfo;
        private string theSupplierName;
        private string theCustomerRelationInfo;
        private string theCustomerName;
        private string accountTitleID;
        private string accountTitleName;
        private string accountTitleCode;
        private string accountTitleSyscode;
        private string bankAccountNo;
        private string bankName;
        private string bankAddress;
        private decimal addPayMoney;
        private decimal addInvoiceMoney;
        private decimal addBalMoney;
        private int ifProjectMoney;

        private Iesi.Collections.Generic.ISet<PaymentInvoice> listInvoice =
            new Iesi.Collections.Generic.HashedSet<PaymentInvoice>();

        /// <summary>
        /// 会计科目
        /// </summary>
        public virtual string AccountTitleID
        {
            get { return accountTitleID; }
            set { accountTitleID = value; }
        }

        /// <summary>
        /// 会计科目名称
        /// </summary>
        public virtual string AccountTitleName
        {
            get { return accountTitleName; }
            set { accountTitleName = value; }
        }

        /// <summary>
        /// 会计科目编码
        /// </summary>
        public virtual string AccountTitleCode
        {
            get { return accountTitleCode; }
            set { accountTitleCode = value; }
        }

        /// <summary>
        /// 会计科目层次码
        /// </summary>
        public virtual string AccountTitleSyscode
        {
            get { return accountTitleSyscode; }
            set { accountTitleSyscode = value; }
        }

        /// <summary>
        /// 银行账号
        /// </summary>
        public virtual string BankAccountNo
        {
            get { return bankAccountNo; }
            set { bankAccountNo = value; }
        }

        /// <summary>
        /// 开户行/行号
        /// </summary>
        public virtual string BankName
        {
            get { return bankName; }
            set { bankName = value; }
        }

        /// <summary>
        /// 开户地址
        /// </summary>
        public virtual string BankAddress
        {
            get { return bankAddress; }
            set { bankAddress = value; }
        }

        /// <summary>
        /// 累计结算
        /// </summary>
        public virtual decimal AddBalMoney
        {
            get { return addBalMoney; }
            set { addBalMoney = value; }
        }

        /// <summary>
        /// 累计付款
        /// </summary>
        public virtual decimal AddPayMoney
        {
            get { return addPayMoney; }
            set { addPayMoney = value; }
        }

        /// <summary>
        /// 累计发票金额
        /// </summary>
        public virtual decimal AddInvoiceMoney
        {
            get { return addInvoiceMoney; }
            set { addInvoiceMoney = value; }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual string TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string TheSupplierName
        {
            get { return theSupplierName; }
            set { theSupplierName = value; }
        }

        /// <summary>
        /// 客户
        /// </summary>
        public virtual string TheCustomerRelationInfo
        {
            get { return theCustomerRelationInfo; }
            set { theCustomerRelationInfo = value; }
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string TheCustomerName
        {
            get { return theCustomerName; }
            set { theCustomerName = value; }
        }

        /// <summary>
        /// 付款发票
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<PaymentInvoice> ListInvoice
        {
            get { return listInvoice; }
            set { listInvoice = value; }
        }

        /// <summary>
        /// 是否工程款(0:工程款 1:非工程款)
        /// </summary>
        public virtual int IfProjectMoney
        {
            get { return ifProjectMoney; }
            set { ifProjectMoney = value; }
        }

        private string originalCollectionUnit;

        /// <summary>
        /// 原收款单位
        /// </summary>
        public virtual string OriginalCollectionUnit
        {
            get { return originalCollectionUnit; }
            set { originalCollectionUnit = value; }
        }

        private string paymentClause;

        /// <summary>
        /// 合同支付条件
        /// </summary>
        public virtual string PaymentClause
        {
            get { return paymentClause; }
            set { paymentClause = value; }
        }

        private ProjectFundPlanDetail fundPlan;

        /// <summary>
        /// 关联资金计划明细
        /// </summary>
        public virtual ProjectFundPlanDetail FundPlan
        {
            get { return fundPlan; }
            set { fundPlan = value; }
        }

        private string fundPlanCode;

        /// <summary>
        /// 资金计划Code
        /// </summary>
        public virtual string FundPlanCode
        {
            get { return fundPlanCode; }
            set { fundPlanCode = value; }
        }

        private DateTime refundDate;

        /// <summary>
        /// 报销（还款）时间
        /// </summary>
        public virtual DateTime RefundDate
        {
            get { return refundDate; }
            set { refundDate = value; }
        }

        private PaymentRequest _requestBill;
        /// <summary>
        /// 资金支付申请单
        /// </summary>
        public virtual PaymentRequest RequestBill
        {
            get { return _requestBill; }
            set { _requestBill = value; }
        }
    }
}
