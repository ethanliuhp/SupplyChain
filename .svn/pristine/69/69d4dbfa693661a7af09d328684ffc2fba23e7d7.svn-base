using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// 入库单明细关系
    /// </summary>
    [Entity]
    [Serializable]
    public class StockInDtlRlt : BusEntityDetRelation
    {
        private StockIn theStockIn;
        private StockInDtl theStockInDtl;

        /// <summary>
        /// 主表关系
        /// </summary>
        public override BusinessEntity BackwardMain
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
        /// <summary>
        /// 明细关系
        /// </summary>
        public override BusinessEntityDetails BackwardDetail
        {
            get
            {
                return theStockInDtl;
            }
            set
            {
                theStockInDtl = value as StockInDtl;
            }
        }
    }
}
