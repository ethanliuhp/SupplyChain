using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockOut.BasicDomain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockOut.Domain
{
    /// <summary>
    /// 出库红单明细
    /// </summary>
    [Serializable]
    public class MatHireStockOutRedDtl : MatHireBasicStockOutDtl
    {
        private decimal quantityTemp;
        virtual public decimal QuantityTemp
        {
            get { return quantityTemp; }
            set { quantityTemp = value; }
        }
    }
}
