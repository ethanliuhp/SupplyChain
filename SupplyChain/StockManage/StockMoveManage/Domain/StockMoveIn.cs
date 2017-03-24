using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Iesi.Collections.Generic;
using Iesi.Collections;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain
{
    /// <summary>
    /// 调拨入库单
    /// </summary>
    [Serializable]
    public class StockMoveIn: BasicStockIn
    {
        private string moveOutProjectId;
        private string moveOutProjectName;
        private int materialProvider;

        /// <summary>
        /// 是否甲供
        /// </summary>
        public virtual int MaterialProvider
        {
            get { return materialProvider; }
            set { materialProvider = value; }
        }

        /// <summary>
        /// 调出项目名称
        /// </summary>
        public virtual string MoveOutProjectName
        {
            get { return moveOutProjectName; }
            set { moveOutProjectName = value; }
        }

        /// <summary>
        /// 调出项目ID
        /// </summary>
        public virtual string MoveOutProjectId
        {
            get { return moveOutProjectId; }
            set { moveOutProjectId = value; }
        }

    }
}
