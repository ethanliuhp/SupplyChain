using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    public class IndirectCostDetail : BaseDetail
    {
        
        private AccountTitleTree accountTitle;
        private string accountTitleID;
        private string accountTitleName;
        private string accountTitleCode;
        private string accountTitleSyscode;
        private decimal budgetMoney;
        //private decimal actualMoney;
        private string partnerType;
        private EnumAccountSymbol accountSymbol;
        
        private OperationOrgInfo orgInfo;
        private string orgInfoID;
        private string orgInfoSysCode;
        private string orgInfoName;
        private EnumCostType costType;
        private string descript;
        public virtual decimal Rate
        {
            get { return BudgetMoney == 0 ? 0 : (Math.Round(Money / BudgetMoney, 2) * 100); }
        }
        /// <summary>
        /// 会计科目ID
        /// </summary>
        public virtual string AccountTitleID
        {
            get
            {
                if (string.IsNullOrEmpty(accountTitleID))
                {
                    accountTitleID = AccountTitle == null ? "" : AccountTitle.Id;
                }
                return accountTitleID;
            }
            set
            {
                accountTitleID = value;
            }
        }
        /// <summary>
        /// 会计科目
        /// </summary>
        public virtual AccountTitleTree AccountTitle
        {
            get { return accountTitle; }
            set { accountTitle = value; }
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
        virtual public string AccountTitleSyscode
        {
            get { return accountTitleSyscode; }
            set { accountTitleSyscode = value; }
        }
        /// <summary>
        /// 预算金额
        /// </summary>
        public virtual decimal BudgetMoney
        {
            get { return budgetMoney; }
            set { budgetMoney = value; }
        }
        /// <summary>
        /// 实际开支
        /// </summary>
        //public virtual decimal ActualMoney
        //{
        //    get { return actualMoney; }
        //    set { actualMoney = value; }
        //}
        /// <summary>
        /// 伙伴类型
        /// </summary>
        public virtual string PartnerType
        {
            get { return partnerType; }
            set { partnerType = value; }
        }
        /// <summary>
        /// 会计科目标志
        /// </summary>
        public virtual EnumAccountSymbol AccountSymbol
        {
            get { return accountSymbol; }
            set { accountSymbol = value; }
        }
        /// <summary>
        /// 所属组织
        /// </summary>
        public virtual OperationOrgInfo OrgInfo
        {
            get { return orgInfo; }
            set { orgInfo = value; }
        }
        /// <summary>
        /// 组织ID
        /// </summary>
        public virtual string OrgInfoID
        {
            get { return orgInfoID; }
            set { orgInfoID = value; }
        }
       /// <summary>
       /// 所属组织的code
       /// </summary>
        public virtual string OrgInfoSysCode
        {
            get { return orgInfoSysCode; }
            set { orgInfoSysCode = value; }
        }
        /// <summary>
        /// 所属组织名称
        /// </summary>
        public virtual string OrgInfoName
        {
            get { return orgInfoName; }
            set { orgInfoName = value; }
        }
        /// <summary>
        /// 费用类型
        /// </summary>
        public virtual EnumCostType CostType
        {
            get { return costType; }
            set { costType = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }
    }
    public enum EnumCostType
    {
        间接费用=0,
        其他应收=1,
        其他应付=2,
        管理费用=3,
        其他=4
    }
    public enum EnumAccountSymbol
    {
        借款标志=0,
        利润标志=1,
        财务费用标志=2,
        上交标志=3,
        其他=4
    }
}
