using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain
{
    /// <summary>
    /// 调拨出库单
    /// </summary>
    [Serializable]
    public class StockMoveOut:BasicStockOut
    {
        private string moveInProjectId;
        private string moveInProjectName;

        /// <summary>
        /// 调入单位名称
        /// </summary>
        public virtual string MoveInProjectName
        {
            get { return moveInProjectName; }
            set { moveInProjectName = value; }
        }

        /// <summary>
        /// 调入单位
        /// </summary>
        public virtual string MoveInProjectId
        {
            get { return moveInProjectId; }
            set { moveInProjectId = value; }
        }
    }
}
