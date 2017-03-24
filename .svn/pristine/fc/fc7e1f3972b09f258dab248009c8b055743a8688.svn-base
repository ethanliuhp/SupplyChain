using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockIn.BasicDomain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockIn.Domain
{
    /// <summary>
    /// 收料入库单
    /// </summary>
    [Serializable]
    public class MatHireStockIn:MatHireBasicStockIn
    {
        private string cph;
        private string associatedOrder;
        private string associatedPlan;

        /// <summary>
        /// 关联计划
        /// </summary>
        public virtual string AssociatedPlan
        {
            get { return associatedPlan; }
            set { associatedPlan = value; }
        }

        /// <summary>
        /// 关联合同
        /// </summary>
        public virtual string AssociatedOrder
        {
            get { return associatedOrder; }
            set { associatedOrder = value; }
        }

        /// <summary>
        /// 车牌号
        /// </summary>
        public virtual string Cph
        {
            get { return cph; }
            set { cph = value; }
        }

    }
}
