using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockIn.BasicDomain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockIn.Domain
{
    /// <summary>
    /// 入库单明细(红单)
    /// </summary>
    [Serializable]
    public class MatHireStockInRedDtl : MatHireBasicStockInDtl
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
