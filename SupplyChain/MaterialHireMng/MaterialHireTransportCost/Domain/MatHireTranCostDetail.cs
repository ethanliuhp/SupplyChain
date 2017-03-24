using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireTransportCost.Domain
{
    /// <summary>
    /// 运输费明细
    /// </summary>
    [Serializable]
   public  class MatHireTranCostDetail : BaseDetail
    {
       
        decimal transportMoney;
        decimal dispatchMoney;
       
        /// <summary> 运输费 </summary>
        virtual public decimal TransportMoney
        {
            get { return transportMoney; }
            set { transportMoney = value; }
        }
        /// <summary> 配送费 </summary>
        virtual public decimal DispatchMoney
        {
            get { return dispatchMoney; }
            set { dispatchMoney = value; }
        }
         
    }
}
