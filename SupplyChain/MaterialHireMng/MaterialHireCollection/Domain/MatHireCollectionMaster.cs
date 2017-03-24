using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;


namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain
{
    /// <summary>
    /// 料具收料单主表
    /// </summary>

    [Serializable]
    public class MatHireCollectionMaster : BaseMaster
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
        private MatHireOrderMaster contract;
        private string  contractCode;
        private decimal transportCharge;
        private EnumMatHireType matHireType;
        private string billCode;
        /// <summary>料具站手工录入单据号</summary>
        public virtual string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }
        /// <summary>
        /// 发料类型  普通料具 钢管 碗扣
        /// </summary>
        public virtual EnumMatHireType MatHireType
        {
            get { return matHireType; }
            set { matHireType = value; }
        }
        /// <summary>
        /// 结算规则
        /// </summary>
        public virtual string BalRule
        {
            get { return balRule; }
            set { balRule = value; }
        }
        /// <summary>
        /// 料具合同
        /// </summary>
        public virtual MatHireOrderMaster Contract
        {
            get { return contract; }
            set { contract = value; }
        }
        /// <summary>
        /// 料具合同编号
        /// </summary>
        public virtual string  ContractCode
        {
            get { return contractCode; }
            set { contractCode = value; }
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

        private Iesi.Collections.Generic.ISet<MatHireCollectionNotQtyCost> matNotQtyCosts = new Iesi.Collections.Generic.HashedSet<MatHireCollectionNotQtyCost>();
        virtual public Iesi.Collections.Generic.ISet<MatHireCollectionNotQtyCost> MatNotQtyCosts
        {
            get { return matNotQtyCosts; }
            set { matNotQtyCosts = value; }
        }

        /// <summary>
        /// 增加料具非数量费用
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatNotQtyCost(MatHireCollectionNotQtyCost detail)
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
    public enum EnumMatHireType
    {
        普通料具 = 0,
        钢管 = 1,
        碗扣 = 2,
        其他=100
    }
}
