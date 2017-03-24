using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain
{
    /// <summary>
    /// 出库红单主表关系
    /// </summary>
    [Entity]
    [Serializable]
    public class StockOutRedRlt : BusEntityRelation
    {
        private StockOutRed theStockOutRed;
        /// <summary>
        /// 后继主表
        /// </summary>
        public override BusinessEntity BackwardBusEntity
        {
            get
            {
                return theStockOutRed;
            }
            set
            {
                theStockOutRed = value as StockOutRed;
            }
        }
    }
}
