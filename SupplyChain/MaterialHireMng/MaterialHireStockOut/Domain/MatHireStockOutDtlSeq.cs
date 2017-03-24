using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockOut.BasicDomain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockOut.Domain
{
    /// <summary>
    /// 出库明细时序
    /// </summary>
    [Serializable]
    public class MatHireStockOutDtlSeq : MatHireBasicStockOutDtlSeq
    {
        private MatHireBasicStockOutDtl stockOutDtl;

        /// <summary>
        /// 出库明细
        /// </summary>
        public virtual MatHireBasicStockOutDtl StockOutDtl
        {
            get { return stockOutDtl; }
            set { stockOutDtl = value; }
        }
    }
}
