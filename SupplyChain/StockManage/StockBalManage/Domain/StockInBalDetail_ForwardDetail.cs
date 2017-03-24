using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain
{
    /// <summary>
    /// 验收结算单明细 前驱明细
    /// </summary>
    [Serializable]
    public class StockInBalDetail_ForwardDetail
    {
        private string id;
        private string forwardDetailId;
        private decimal quantity;
        private StockInBalDetail _StockInBalDetail;

        /// <summary>
        /// 验收结算明细
        /// </summary>
        public virtual StockInBalDetail StockInBalDetail
        {
            get { return _StockInBalDetail; }
            set { _StockInBalDetail = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// 前驱ID
        /// </summary>
        public virtual string ForwardDetailId
        {
            get { return forwardDetailId; }
            set { forwardDetailId = value; }
        }

        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
