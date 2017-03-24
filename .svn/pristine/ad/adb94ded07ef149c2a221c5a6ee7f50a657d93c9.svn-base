using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain
{
    /// <summary>
    /// 调拨入库红单明细
    /// </summary>
    [Serializable]
    public class StockMoveInRedDtl : BasicStockInDtl
    {
        private decimal quantityTemp;
        private decimal newPrice;

        /// <summary>
        /// 新单价
        /// </summary>
        public virtual decimal NewPrice
        {
            get { return newPrice; }
            set { newPrice = value; }
        }

        /// <summary>
        /// 临时数量
        /// </summary>
        virtual public decimal QuantityTemp
        {
            get { return quantityTemp; }
            set { quantityTemp = value; }
        }
    }
}
