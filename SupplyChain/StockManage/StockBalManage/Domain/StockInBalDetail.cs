using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain
{
    /// <summary>
    /// 验收结算单明细
    /// </summary>
    [Serializable]
    public class StockInBalDetail:BaseStockInBalDetail
    {
        private ISet<StockInBalDetail_ForwardDetail> details = new HashedSet<StockInBalDetail_ForwardDetail>();

        public virtual ISet<StockInBalDetail_ForwardDetail> ForwardDetails
        {
            get { return details; }
            set { details = value; }
        }

        public virtual void AddForwardDetail(StockInBalDetail_ForwardDetail detail)
        {
            detail.StockInBalDetail = this;
            details.Add(detail);
        }
    }
}
