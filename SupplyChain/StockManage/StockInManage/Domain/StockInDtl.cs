using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.Util;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// 收料入库单明细
    /// </summary>
    [Serializable]
    public class StockInDtl : BasicStockInDtl
    {
        private decimal balQuantity;
        private string originalContractNo;
        private string supplyOrderDetailId;
        private decimal quantityTemp;
        private string calculate;
        private string appearanceQuality;
        private string weightBillRelation;
        ///<summary>
        ///计算式
        /// </summary>
        public virtual string Calculate
        {
            get { return calculate; }
            set { calculate = value; }
        }
        ///<summary>
        ///外观质量
        /// </summary>
        public virtual string AppearanceQuality
        {
            get { return appearanceQuality; }
            set { appearanceQuality = value; }
        }
        /// <summary>
        /// 临时数量
        /// </summary>
        public virtual decimal QuantityTemp
        {
            get { return quantityTemp; }
            set { quantityTemp = value; }
        }

        /// <summary>
        /// 采购合同明细ID
        /// </summary>
        public virtual string SupplyOrderDetailId
        {
            get { return supplyOrderDetailId; }
            set { supplyOrderDetailId = value; }
        }

        /// <summary>
        /// 原始合同号
        /// </summary>
        public virtual string OriginalContractNo
        {
            get { return originalContractNo; }
            set { originalContractNo = value; }
        }

        /// <summary>
        /// 剩余结算数量
        /// </summary>
        public virtual decimal BalQuantity
        {
            get { return balQuantity; }
            set { balQuantity = value; }
        }
        /// <summary>
        /// 过磅关系表ID
        /// </summary>
        public virtual string WeightBillRelationID
        {
            get { return this.weightBillRelation; }
            set { this.weightBillRelation = value; }
        }
        /// <summary>
        /// 过磅材料 临时用的
        /// </summary>
        public virtual WeightBillDetail WeightBillDetail { get; set; }
        private bool isUpdateWeightBillDetail = false;
         
      
    }
}
