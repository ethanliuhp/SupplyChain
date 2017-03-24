using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialCollectionMng.Domain
{
    /// <summary>
    /// 料具收料单主表
    /// </summary>

    [Serializable]
    public class MaterialCollectionMaster : BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;
        private string oldContractNum;
        private decimal sumExtMoney;

        private SupplierRelationInfo theRank;
        private string theRankName;

        private string balRule;
        private int balYear;
        private int balMonth;
        private int balState;
        private string contractId;
        private decimal transportCharge;

        /// <summary>
        /// 结算规则
        /// </summary>
        public virtual string BalRule
        {
            get { return balRule; }
            set { balRule = value; }
        }
        /// <summary>
        /// 料具合同ID
        /// </summary>
        public virtual string ContractId
        {
            get { return contractId; }
            set { contractId = value; }
        }
        /// <summary>
        /// 使用队伍
        /// </summary>
        virtual public SupplierRelationInfo TheRank
        {
            get { return theRank; }
            set { theRank = value; }
        }
        /// <summary>
        /// 使用队伍名称
        /// </summary>
        virtual public string TheRankName
        {
            get { return theRankName; }
            set { theRankName = value; }
        }

        private Iesi.Collections.Generic.ISet<MaterialNotQtyCost> matNotQtyCosts = new Iesi.Collections.Generic.HashedSet<MaterialNotQtyCost>();
        virtual public Iesi.Collections.Generic.ISet<MaterialNotQtyCost> MatNotQtyCosts
        {
            get { return matNotQtyCosts; }
            set { matNotQtyCosts = value; }
        }

        /// <summary>
        /// 增加料具非数量费用
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatNotQtyCost(MaterialNotQtyCost detail)
        {
            detail.Master = this;
            MatNotQtyCosts.Add(detail);
        }

        /// <summary>
        /// 出租方/供应商
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }
        /// <summary>
        /// 出租方/供应商名称
        /// </summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        /// <summary>
        /// 原始合同号
        /// </summary>
        virtual public string OldContractNum
        {
            get { return oldContractNum; }
            set { oldContractNum = value; }
        }
        /// <summary>
        /// 附加费用总金额
        /// </summary>
        virtual public decimal SumExtMoney
        {
            get { return sumExtMoney; }
            set { sumExtMoney = value; }
        }

        private int sumBusQty;
        /// <summary>
        /// 总车数
        /// </summary>
        virtual public int SumBusQty
        {
            get { return sumBusQty; }
            set { sumBusQty = value; }
        }

        /// <summary>
        /// 结算会计年
        /// </summary>
        virtual public int BalYear
        {
            get { return balYear; }
            set { balYear = value; }
        }
        /// <summary>
        /// 结算会计月
        /// </summary>
        virtual public int BalMonth
        {
            get { return balMonth; }
            set { balMonth = value; }
        }
        /// <summary>
        /// 结算状态 0：未结算  1; 已结算
        /// </summary>
        virtual public int BalState
        {
            get { return balState; }
            set { balState = value; }
        }
        /// <summary>
        /// 运输费
        /// </summary>
        virtual public decimal TransportCharge
        {
            get { return transportCharge; }
            set { transportCharge = value; }
        }

    }
}
