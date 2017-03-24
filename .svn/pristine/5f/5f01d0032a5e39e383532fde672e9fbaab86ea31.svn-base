using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain
{
    /// <summary>
    /// 调拨出库红单明细
    /// </summary>
    [Serializable]
    public class StockMoveOutRedDtl:BasicStockOutDtl
    {
        private decimal quantityTemp;
        private decimal movePrice;
        private decimal moveMoney;

        /// <summary>
        /// 调拨金额
        /// </summary>
        public virtual decimal MoveMoney
        {
            get { return moveMoney; }
            set { moveMoney = value; }
        }

        /// <summary>
        /// 调拨单价
        /// </summary>
        public virtual decimal MovePrice
        {
            get { return movePrice; }
            set { movePrice = value; }
        }

        virtual public decimal QuantityTemp
        {
            get { return quantityTemp; }
            set { quantityTemp = value; }
        }
    }
}
