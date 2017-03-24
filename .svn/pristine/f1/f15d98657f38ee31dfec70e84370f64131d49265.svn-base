using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain
{
    /// <summary>
    /// 分科目费用明细
    /// </summary>
    [Serializable]
    public class MaterialSubjectDetail : BaseDetail
    {
        private StandardUnit priceUnit;
        private string priceUnitName;
        private StandardUnit dateUnit;
        private string dateUnitName;
        private StandardUnit quantityUnit;
        private string quantityUnitName;

        ///<summary>
        /// 价格单位
        ///</summary>
        virtual public StandardUnit PriceUnit
        {
            get { return priceUnit; }
            set { priceUnit = value; }
        }
        ///<summary>
        ///价格单位名称
        ///</summary>
        virtual public string PriceUnitName
        {
            get { return priceUnitName; }
            set { priceUnitName = value; }
        }
        ///<summary>
        /// 时间单位
        ///</summary>
        virtual public StandardUnit DateUnit
        {
            get { return dateUnit; }
            set { dateUnit = value; }
        }
        ///<summary>
        ///时间单位名称
        ///</summary>
        virtual public string DateUnitName
        {
            get { return dateUnitName; }
            set { dateUnitName = value; }
        }
        ///<summary>
        /// 数量单位
        ///</summary>
        virtual public StandardUnit QuantityUnit
        {
            get { return quantityUnit; }
            set { quantityUnit = value; }
        }
        ///<summary>
        ///数量单位名称
        ///</summary>
        virtual public string QuantityUnitName
        {
            get { return quantityUnitName; }
            set { quantityUnitName = value; }
        }
        private string costName;
        ///<summary>
        /// 成本名称
        ///</summary>
        virtual public string CostName
        {
            get { return costName; }
            set { costName = value; }
        }
        private decimal settlePrice;
        ///<summary>
        /// 结算单价
        ///</summary>
        virtual public decimal SettlePrice
        {
            get { return settlePrice; }
            set { settlePrice = value; }
        }
        private decimal settleMoney;
        ///<summary>
        /// 结算合价
        ///</summary>
        virtual public decimal SettleMoney
        {
            get { return settleMoney; }
            set { settleMoney = value; }
        }
        private CostAccountSubject settleSubject;
        ///<summary>
        /// 结算科目
        ///</summary>
        virtual public CostAccountSubject SettleSubject
        {
            get { return settleSubject; }
            set { settleSubject = value; }
        }
        private string settleSubjectName;
        ///<summary>
        /// 结算科目名称
        ///</summary>
        virtual public string SettleSubjectName
        {
            get { return settleSubjectName; }
            set { settleSubjectName = value; }
        }
        private string settleSubjectSyscode;
        ///<summary>
        /// 结算科目层次码
        ///</summary>
        virtual public string SettleSubjectSyscode
        {
            get { return settleSubjectSyscode; }
            set { settleSubjectSyscode = value; }
        }
        private decimal settleQuantity;
        ///<summary>
        /// 结算数量
        ///</summary>
        virtual public decimal SettleQuantity
        {
            get { return settleQuantity; }
            set { settleQuantity = value; }
        }
        private Material materialType;
        ///<summary>
        /// 资源类型
        ///</summary>
        virtual public Material MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
        }
        private string materialTypeName;
        ///<summary>
        /// 资源类型名称
        ///</summary>
        virtual public string MaterialTypeName
        {
            get { return materialTypeName; }
            set { materialTypeName = value; }
        }
        private string materialTypeStuff;
        ///<summary>
        /// 资源类型材质
        ///</summary>
        virtual public string MaterialTypeStuff
        {
            get { return materialTypeStuff; }
            set { materialTypeStuff = value; }
        }
        private string materialTypeSpec;
        ///<summary>
        /// 资源类型规格
        ///</summary>
        virtual public string MaterialTypeSpec
        {
            get { return materialTypeSpec; }
            set { materialTypeSpec = value; }
        }
        private decimal settleDate;
        ///<summary>
        /// 租赁时间
        ///</summary>
        virtual public decimal SettleDate
        {
            get { return settleDate; }
            set { settleDate = value; }
        }

        private MaterialRentalSettlementDetail masterCost;
        /// <summary>
        /// 费用主表ID
        /// </summary>
        virtual public MaterialRentalSettlementDetail MasterCost
        {
            get { return masterCost; }
            set { masterCost = value; }
        }

    }
}
