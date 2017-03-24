using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockOut.BasicDomain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockOut.Domain
{
    /// <summary>
    /// 出库红单
    /// </summary>
    [Serializable]
    public class MatHireStockOutRed : MatHireBasicStockOut
    {
        private int isLimited;

        /// <summary>
        /// 是否限额
        /// </summary>
        virtual public int IsLimited
        {
            get { return isLimited; }
            set { isLimited = value; }
        }
    }
}
