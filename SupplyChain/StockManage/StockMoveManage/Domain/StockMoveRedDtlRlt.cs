using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain
{
    /// <summary>
    /// 转仓单明细关系
    /// </summary>
    [Serializable]
    public class StockMoveRedDtlRlt : BusEntityDetRelation
    {
        private StockMoveRed theStockMove;
        private StockMoveRedDtl theStockMoveDtl;

        /// <summary>
        /// 主表关系
        /// </summary>
        public override BusinessEntity BackwardMain
        {
            get
            {
                return theStockMove;
            }
            set
            {
                theStockMove = value as StockMoveRed;
            }
        }
        /// <summary>
        /// 明细关系
        /// </summary>
        public override BusinessEntityDetails BackwardDetail
        {
            get
            {
                return theStockMoveDtl;
            }
            set
            {
                theStockMoveDtl = value as StockMoveRedDtl;
            }
        }
    }
}
