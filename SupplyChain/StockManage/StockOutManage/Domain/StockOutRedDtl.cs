using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain
{
    /// <summary>
    /// ³ö¿âºìµ¥Ã÷Ï¸
    /// </summary>
    [Serializable]
    public class StockOutRedDtl : BasicStockOutDtl
    {
        private decimal quantityTemp;
        virtual public decimal QuantityTemp
        {
            get { return quantityTemp; }
            set { quantityTemp = value; }
        }
    }
}
