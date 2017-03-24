using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain
{
    /// <summary>
    /// 料具退料单
    /// </summary>
    [Serializable]
    public class MaterialReturnMaster : BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;
        private string oldContractNum;
        private decimal sumExtMoney;
        private decimal sumExitQuantity;

        private SupplierRelationInfo theRank;
        private string theRankName;
        private int sumBusQty;
        private int returnType;

        private string balRule;
        private int balYear;
        private int balMonth;
        private int balState;
        private decimal transportCharge;
        private EnumMatHireType matHireType;
        /// <summary>
        /// 结算规则
        /// </summary>
        public virtual string BalRule
        {
            get { return balRule; }
            set { balRule = value; }
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

        private Iesi.Collections.Generic.ISet<MaterialReturnNotQtyCost> matReturnNotQtyCosts = new Iesi.Collections.Generic.HashedSet<MaterialReturnNotQtyCost>();
        virtual public Iesi.Collections.Generic.ISet<MaterialReturnNotQtyCost> MatReturnNotQtyCosts
        {
            get { return matReturnNotQtyCosts; }
            set { matReturnNotQtyCosts = value; }
        }

        /// <summary>
        /// 增加非数量费用
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatReturnNotQtyCosts(MaterialReturnNotQtyCost detail)
        {
            detail.Master = this;
            MatReturnNotQtyCosts.Add(detail);
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
        /// <summary>
        /// 退场总数量
        /// </summary>
        virtual public decimal SumExitQuantity
        {
            get { return sumExitQuantity; }
            set { sumExitQuantity = value; }
        }
        /// <summary>
        /// 总车数
        /// </summary>
        virtual public int SumBusQty
        {
            get { return sumBusQty; }
            set { sumBusQty = value; }
        }
        /// <summary>
        /// 退料类型  1：料具退料单  2：料具退料单(损耗)
        /// </summary>
        virtual public int ReturnType
        {
            get { return returnType; }
            set { returnType = value; }
        }
        /// <summary>
        /// 料具类型
        /// </summary>
        virtual public EnumMatHireType MatHireType
        {
            get { return matHireType; }
            set { matHireType = value; }
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
