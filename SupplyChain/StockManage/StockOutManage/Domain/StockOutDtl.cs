using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain
{
    /// <summary>
    /// 出库单明细
    /// </summary>
    [Entity]
    [Serializable]
    public class StockOutDtl : BasicStockOutDtl
    {
        private Iesi.Collections.Generic.ISet<StockOutDtlSeq> stockOutDtlSeqList = new Iesi.Collections.Generic.HashedSet<StockOutDtlSeq>();

        /// <summary>
        /// 出库时序明细
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<StockOutDtlSeq> StockOutDtlSeqList
        {
            get { return stockOutDtlSeqList; }
            set { stockOutDtlSeqList = value; }
        }

        /// <summary>
        /// 添加出库时序明细
        /// </summary>
        /// <param name="detail"></param>
        public virtual void AddDetails(StockOutDtlSeq detail)
        {
            detail.StockOutDtl = this;
            stockOutDtlSeqList.Add(detail);
        }
         
   
    }
}
