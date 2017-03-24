using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain
{
    /// <summary>
    /// 调拨出库明细
    /// </summary>
    [Serializable]
    public class StockMoveOutDtl:BasicStockOutDtl
    {
        private Iesi.Collections.Generic.ISet<StockOutDtlSeq> stockOutDtlSeqList = new Iesi.Collections.Generic.HashedSet<StockOutDtlSeq>();
        private decimal movePrice;
        private decimal moveMoney;

        /// <summary>
        /// 旧的出库金额
        /// </summary>
        public virtual decimal MoveMoney
        {
            get { return moveMoney; }
            set { moveMoney = value; }
        }

        /// <summary>
        /// 旧的出库单价
        /// </summary>
        public virtual decimal MovePrice
        {
            get { return movePrice; }
            set { movePrice = value; }
        }

        /// <summary>
        /// 出库时序明细
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<StockOutDtlSeq> StockOutDtlSeqList
        {
            get { return stockOutDtlSeqList; }
            set { stockOutDtlSeqList = value; }
        }

        /// <summary>
        /// 添加出库时序明细
        /// </summary>
        /// <param name="detail"></param>
        public virtual void AddDetails(StockOutDtlSeq detail)
        {
            detail.StockOutDtl = this;
            stockOutDtlSeqList.Add(detail);
        }
    }
}
