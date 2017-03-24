using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    [Entity]
    public class FilialeFundPlanMaster : BaseMaster
    {
        private decimal _currentYearFundNetFlow;

        /// <summary>本年资金净流量</summary>
        public virtual decimal CurrentYearFundNetFlow
        {
            get { return _currentYearFundNetFlow; }
            set { _currentYearFundNetFlow = value; }
        }

        private decimal _presentMonthPlanPayment;

        /// <summary>本月计划付款</summary>
        public virtual decimal PresentMonthPlanPayment
        {
            get { return _presentMonthPlanPayment; }
            set { _presentMonthPlanPayment = value; }
        }

        private decimal _presentMonthGathering;

        /// <summary>本月计划收款</summary>
        public virtual decimal PresentMonthGathering
        {
            get { return _presentMonthGathering; }
            set { _presentMonthGathering = value; }
        }

        private decimal _presentMonthSpendableFund;

        /// <summary>本月预计可使用资金</summary>
        public virtual decimal PresentMonthSpendableFund
        {
            get { return _presentMonthSpendableFund; }
            set { _presentMonthSpendableFund = value; }
        }

        private decimal _financeConfirmTaxIncome;

        /// <summary>财务确认含税收入</summary>
        public virtual decimal FinanceConfirmTaxIncome
        {
            get { return _financeConfirmTaxIncome; }
            set { _financeConfirmTaxIncome = value; }
        }

        private string _unit;

        /// <summary>单位</summary>
        public virtual string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        private decimal _tillLastMonthFundStock;

        /// <summary>截止上月资金存量</summary>
        public virtual decimal TillLastMonthFundStock
        {
            get { return _tillLastMonthFundStock; }
            set { _tillLastMonthFundStock = value; }
        }

        private decimal _cumulativeCurrentYearPayment;

        /// <summary>累计本年付款</summary>
        public virtual decimal CumulativeCurrentYearPayment
        {
            get { return _cumulativeCurrentYearPayment; }
            set { _cumulativeCurrentYearPayment = value; }
        }

        private decimal _cumulativeCurrentYearGathering;

        /// <summary>累计本年收款</summary>
        public virtual decimal CumulativeCurrentYearGathering
        {
            get { return _cumulativeCurrentYearGathering; }
            set { _cumulativeCurrentYearGathering = value; }
        }

        private decimal _cumulativeCurrentYearCashRatio;

        /// <summary>累计本年收现率</summary>
        public virtual decimal CumulativeCurrentYearCashRatio
        {
            get { return _cumulativeCurrentYearCashRatio; }
            set { _cumulativeCurrentYearCashRatio = value; }
        }

        private decimal _cumulativeCurrentYearFundFlow;

        /// <summary>累计本年资金净流量</summary>
        public virtual decimal CumulativeCurrentYearFundFlow
        {
            get { return _cumulativeCurrentYearFundFlow; }
            set { _cumulativeCurrentYearFundFlow = value; }
        }

        private decimal _thereinSuperviseAccountFund;

        /// <summary>其中监管账户资金</summary>
        public virtual decimal ThereinSuperviseAccountFund
        {
            get { return _thereinSuperviseAccountFund; }
            set { _thereinSuperviseAccountFund = value; }
        }

        private decimal _thereinLoan;

        /// <summary>其中借款</summary>
        public virtual decimal ThereinLoan
        {
            get { return _thereinLoan; }
            set { _thereinLoan = value; }
        }

        private decimal _thereinBankAccept;

        /// <summary>其中银行承兑和商业承兑</summary>
        public virtual decimal ThereinBankAccept
        {
            get { return _thereinBankAccept; }
            set { _thereinBankAccept = value; }
        }

        private string _declareUnit;

        /// <summary>申报单位名称</summary>
        public virtual string DeclareUnit
        {
            get { return _declareUnit; }
            set { _declareUnit = value; }
        }

        private OperationOrgInfo _declareOrg;

        /// <summary>
        /// 申报单位
        /// </summary>
        public virtual OperationOrgInfo DeclareOrg
        {
            get { return _declareOrg; }
            set { _declareOrg = value; }
        }

        private string _declarePerson;

        /// <summary>申报人</summary>
        public virtual string DeclarePerson
        {
            get { return _declarePerson; }
            set { _declarePerson = value; }
        }

        private DateTime _declareDate;

        /// <summary>申报日期</summary>
        public virtual DateTime DeclareDate
        {
            get { return _declareDate; }
            set { _declareDate = value; }
        }

        private decimal _approval;

        /// <summary>审批额</summary>
        public virtual decimal Approval
        {
            get { return _approval; }
            set { _approval = value; }
        }

        private Iesi.Collections.Generic.ISet<OfficeFundPlanPayDetail> _officeFundPlanDetails =
            new Iesi.Collections.Generic.HashedSet<OfficeFundPlanPayDetail>();

        public virtual Iesi.Collections.Generic.ISet<OfficeFundPlanPayDetail> OfficeFundPlanDetails
        {
            get { return _officeFundPlanDetails; }
            set { _officeFundPlanDetails = value; }
        }
    }
}
