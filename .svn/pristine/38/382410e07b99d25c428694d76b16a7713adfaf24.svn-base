using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// 入库红单主表关系
    /// </summary>
    [Entity]
    [Serializable]
    public class StockInRedRlt : BusEntityRelation
    {
        private StockInRed theStockInRed;
        /// <summary>
        /// 后继主表关系
        /// </summary>
        public override BusinessEntity BackwardBusEntity
        {
            get
            {
                return theStockInRed;
            }
            set
            {
                theStockInRed = value as StockInRed;
            }
        }
    }
}
