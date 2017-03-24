using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockOut.BasicDomain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockOut.Domain
{
    /// <summary>
    /// 出库单明细
    /// </summary>
    [Entity]
    [Serializable]
    public class MatHireStockOutDtl : MatHireBasicStockOutDtl
    {
        private Iesi.Collections.Generic.ISet<MatHireStockOutDtlSeq> stockOutDtlSeqList = new Iesi.Collections.Generic.HashedSet<MatHireStockOutDtlSeq>();

        /// <summary>
        /// 出库时序明细
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<MatHireStockOutDtlSeq> StockOutDtlSeqList
        {
            get { return stockOutDtlSeqList; }
            set { stockOutDtlSeqList = value; }
        }

        /// <summary>
        /// 添加出库时序明细
        /// </summary>
        /// <param name="detail"></param>
        public virtual void AddDetails(MatHireStockOutDtlSeq detail)
        {
            detail.StockOutDtl = this;
            stockOutDtlSeqList.Add(detail);
        }


    }
}
