﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Iesi.Collections.Generic;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    //付款发票主表
    [Serializable]
    [Entity]
    public class PaymentInvoice : BaseMaster
    {

        private string invoiceCode;
        private string invoiceNo;
        private string invoiceType;
        private string paymentID;
        private SupplierRelationInfo theSupplierRelationInfo;
        private string theSupplierName;
        private decimal sumMoney;
        private PaymentMaster master;
        private string ifDeduction;
        private string accountTitleID;
        private string accountTitleName;
        private string accountTitleCode;
        private string accountTitleSyscode;
        private string supplierScale;
        private decimal taxRate;
        private decimal taxMoney;
        private string transferType;
        private decimal transferTax;

        private string temp1;
        private string temp2;

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
        /// 是否抵扣
        /// </summary>
        public virtual string IfDeduction
        {
            get { return ifDeduction; }
            set { ifDeduction = value; }
        }

        /// <summary>
        /// 发票代码
        /// </summary>
        public virtual string InvoiceCode
        {
            get { return invoiceCode; }
            set { invoiceCode = value; }
        }

        /// <summary>
        /// 发票号码
        /// </summary>
        public virtual string InvoiceNo
        {
            get { return invoiceNo; }
            set { invoiceNo = value; }
        }

        /// <summary>
        /// 付款ID
        /// </summary>
        public virtual string PaymentID
        {
            get { return paymentID; }
            set { paymentID = value; }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual SupplierRelationInfo TheSupplierRelationInfo
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
        /// 金额 
        /// </summary>
        public override decimal SumMoney
        {
            get { return sumMoney; }
            set { sumMoney = value; }
        }

        /// <summary>
        /// 付款单 
        /// </summary>
        public virtual PaymentMaster Master
        {
            get { return master; }
            set { master = value; }
        }

        /// <summary>
        /// 纳税人规模 
        /// </summary>
        public virtual string SupplierScale
        {
            get { return supplierScale; }
            set { supplierScale = value; }
        }

        /// <summary>
        /// 发票类型 
        /// </summary>
        public virtual string InvoiceType
        {
            get { return invoiceType; }
            set { invoiceType = value; }
        }

        /// <summary>
        /// 税率
        /// </summary>
        public virtual decimal TaxRate
        {
            get { return taxRate; }
            set { taxRate = value; }
        }

        /// <summary>
        /// 税金
        /// </summary>
        public virtual decimal TaxMoney
        {
            get { return taxMoney; }
            set { taxMoney = value; }
        }

        /// <summary>
        /// 转出类型
        /// </summary>
        public virtual string TransferType
        {
            get { return transferType; }
            set { transferType = value; }
        }

        /// <summary>
        /// 转出金额
        /// </summary>
        public virtual decimal TransferTax
        {
            get { return transferTax; }
            set { transferTax = value; }
        }

        /// <summary>
        /// 临时归属分公司
        /// </summary>
        public virtual string Temp1
        {
            get { return temp1; }
            set { temp1 = value; }
        }

        /// <summary>
        /// 临时纳税识别号
        /// </summary>
        public virtual string Temp2
        {
            get { return temp2; }
            set { temp2 = value; }
        }

        private string settlementType;

        /// <summary>
        /// 发票对应结算单类型
        /// </summary>
        public virtual string SettlementType
        {
            get { return settlementType; }
            set { settlementType = value; }
        }

        private decimal surplusMoney;

        /// <summary>
        /// 发票关联剩余金额
        /// </summary>
        public virtual decimal SurplusMoney
        {
            get { return surplusMoney; }
            set { surplusMoney = value; }
        }

        private ISet<InvoiceSettlementRelation> settlements = new HashedSet<InvoiceSettlementRelation>();
        /// <summary>
        /// 关联结算单
        /// </summary>
        public virtual ISet<InvoiceSettlementRelation> Settlements
        {
            get { return settlements; }
            set { settlements = value; }
        }
    }
}