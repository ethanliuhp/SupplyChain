using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Iesi.Collections;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Iesi.Collections.Generic;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// 收料入库单
    /// </summary>
    [Serializable]
    public class StockIn : BasicStockIn
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
