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
    /// ������ⵥ
    /// </summary>
    [Serializable]
    public class StockMoveIn: BasicStockIn
    {
        private string moveOutProjectId;
        private string moveOutProjectName;
        private int materialProvider;

        /// <summary>
        /// �Ƿ�׹�
        /// </summary>
        public virtual int MaterialProvider
        {
            get { return materialProvider; }
            set { materialProvider = value; }
        }

        /// <summary>
        /// ������Ŀ����
        /// </summary>
        public virtual string MoveOutProjectName
        {
            get { return moveOutProjectName; }
            set { moveOutProjectName = value; }
        }

        /// <summary>
        /// ������ĿID
        /// </summary>
        public virtual string MoveOutProjectId
        {
            get { return moveOutProjectId; }
            set { moveOutProjectId = value; }
        }

    }
}
