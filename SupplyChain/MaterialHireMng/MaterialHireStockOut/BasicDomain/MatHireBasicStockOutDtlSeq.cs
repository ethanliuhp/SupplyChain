using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockOut.BasicDomain
{
    /// <summary>
    /// 出库明细时序
    /// </summary>
    [Serializable]
    [Entity]
    public class MatHireBasicStockOutDtlSeq
    {
        private string id;
        private string stockInDtlId;
        private decimal price;
        private decimal quantity;
        private DateTime createDate = new DateTime(1900, 1, 1);
        private decimal remainQuantity;
        private string stockRelId;
        private DateTime seqCreateDate = new DateTime(1900, 1, 1);

        /// <summary>
        /// 库存时序生成日期
        /// </summary>
        public virtual DateTime SeqCreateDate
        {
            get { return seqCreateDate; }
            set { seqCreateDate = value; }
        }

        /// <summary>
        /// 库存ID
        /// </summary>
        public virtual string StockRelId
        {
            get { return stockRelId; }
            set { stockRelId = value; }
        }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public virtual decimal RemainQuantity
        {
            get { return remainQuantity; }
            set { remainQuantity = value; }
        }

        /// <summary>
        /// 生成日期
        /// </summary>
        public virtual DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// 金额
        /// </summary>
        public virtual decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
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
        /// 入库明细ID
        /// </summary>
        public virtual string StockInDtlId
        {
            get { return stockInDtlId; }
            set { stockInDtlId = value; }
        }

        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
