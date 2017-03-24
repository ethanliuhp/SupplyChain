using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain
{
    /// <summary>
    /// 出库红单明细关系
    /// </summary>
    [Entity]
    [Serializable]
    public class StockOutRedDtlRlt : BusEntityDetRelation
    {
        private StockOutRed theStockOutRed;
        private StockOutRedDtl theStockOutRedDtl;

        /// <summary>
        /// 主表关系
        /// </summary>
        public override BusinessEntity BackwardMain
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
        /// <summary>
        /// 明细关系
        /// </summary>
        public override BusinessEntityDetails BackwardDetail
        {
            get
            {
                return theStockOutRedDtl;
            }
            set
            {
                theStockOutRedDtl = value as StockOutRedDtl;
            }
        }
    }
}
