using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// 入库红单明细关系
    /// </summary>
    [Entity]
    [Serializable]
    public class StockInRedDtlRlt : BusEntityDetRelation
    {
        private StockInRed theStockInRed;
        private StockInRedDtl theStockInRedDtl;

        /// <summary>
        /// 主表关系
        /// </summary>
        public override BusinessEntity BackwardMain
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
        /// <summary>
        /// 明细关系
        /// </summary>
        public override BusinessEntityDetails BackwardDetail
        {
            get
            {
                return theStockInRedDtl;
            }
            set
            {
                theStockInRedDtl = value as StockInRedDtl;
            }
        }
    }
}
