using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockIn.BasicDomain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockIn.Domain
{
    /// <summary>
    /// 收料入库单明细
    /// </summary>
    [Serializable]
    public class MatHireStockInDtl : MatHireBasicStockInDtl
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
        /// 结算数量
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
       // public virtual WeightBillDetail WeightBillDetail { get; set; }
       // private bool isUpdateWeightBillDetail = false;


    }
}
