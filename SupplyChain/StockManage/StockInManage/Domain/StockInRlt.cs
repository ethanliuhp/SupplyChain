using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// 入库单主表关系
    /// </summary>
    [Entity]
    [Serializable]
    public class StockInRlt : BusEntityRelation
    {
        private StockIn theStockIn;
        /// <summary>
        /// 后继主表
        /// </summary>
        public override BusinessEntity BackwardBusEntity
        {
            get
            {
                return theStockIn;
            }
            set
            {
                theStockIn = value as StockIn;
            }
        }
    }
}