using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain
{
    /// <summary>
    /// 转仓单审核
    /// </summary>
    [Serializable]
    public class StockMoveRedRst : AuditResult
    {
        private StockMoveRed theStockMove;
        /// <summary>
        /// 审核主表
        /// </summary>
        public override BusinessEntity TheProcess
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
    }
}
