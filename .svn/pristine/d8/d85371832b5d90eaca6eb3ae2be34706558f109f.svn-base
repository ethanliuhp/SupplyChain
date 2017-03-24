using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Iesi.Collections.Generic;
using Iesi.Collections;
using System.Collections;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// 入库单(红单)
    /// </summary>
    [Serializable]
    public class StockInRed : BasicStockIn
    {
        private EnumForRedType forRedType;

        /// <summary>
        /// 冲红类型
        /// </summary>
        public virtual EnumForRedType ForRedType
        {
            get { return forRedType; }
            set { forRedType = value; }
        }
    }
}
