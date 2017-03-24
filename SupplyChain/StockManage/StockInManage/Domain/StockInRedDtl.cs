using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// 入库单明细(红单)
    /// </summary>
    [Serializable]
    public class StockInRedDtl : BasicStockInDtl
    {
        private decimal quantityTemp;
        private decimal newPrice;

        /// <summary>
        /// 新单价
        /// </summary>
        public virtual decimal NewPrice
        {
            get { return newPrice; }
            set { newPrice = value; }
        }

        virtual public decimal QuantityTemp
        {
            get { return quantityTemp; }
            set { quantityTemp = value; }
        }        

    }
}
