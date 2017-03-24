using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockIn.Domain
{
    /// <summary>
    /// 入库冲红时序表
    /// </summary>
    [Serializable]
    [Entity]
    public class MatHireStockInDtlSeq
    {
        private string id;
        private string stockInRedId;
        private string stockInRedDtlId;
        private int step;
        private string descript;
        private string material;
        private string materialCode;
        private string materialName;
        private string materialSpec;
        private decimal quantity;
        private decimal price;
        private string stockOutDtlId;

        /// <summary>
        /// 出库明细Id
        /// </summary>
        public virtual string StockOutDtlId
        {
            get { return stockOutDtlId; }
            set { stockOutDtlId = value; }
        }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// 规格
        /// </summary>
        public virtual string MaterialSpec
        {
            get { return materialSpec; }
            set { materialSpec = value; }
        }

        /// <summary>
        /// 物资名称
        /// </summary>
        public virtual string MaterialName
        {
            get { return materialName; }
            set { materialName = value; }
        }

        /// <summary>
        /// 物资编码
        /// </summary>
        public virtual string MaterialCode
        {
            get { return materialCode; }
            set { materialCode = value; }
        }

        /// <summary>
        /// 物资
        /// </summary>
        public virtual string Material
        {
            get { return material; }
            set { material = value; }
        }

        /// <summary>
        /// 操作描述
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 操作顺序
        /// </summary>
        public virtual int Step
        {
            get { return step; }
            set { step = value; }
        }

        /// <summary>
        /// 入库红单明细ID
        /// </summary>
        public virtual string StockInRedDtlId
        {
            get { return stockInRedDtlId; }
            set { stockInRedDtlId = value; }
        }

        /// <summary>
        /// 入库红单ID
        /// </summary>
        public virtual string StockInRedId
        {
            get { return stockInRedId; }
            set { stockInRedId = value; }
        }

        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}

