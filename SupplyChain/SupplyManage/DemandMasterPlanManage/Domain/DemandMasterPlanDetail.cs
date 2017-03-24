using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain
{
    /// <summary>
    /// 需求总计划明细
    /// </summary>
    [Serializable]
    public class DemandMasterPlanDetail : BaseSupplyDetail
    {
        private decimal supplyLeftQuantity;
        private decimal demandLeftQuantity;
        private string technologyParameter;
        virtual public string TechnologyParameter
        {
            get { return technologyParameter; }
            set { technologyParameter = value; }
        }
        /// <summary>
        /// 采购剩余数量
        /// </summary>
        virtual public decimal SupplyLeftQuantity
        {
            get { return supplyLeftQuantity; }
            set { supplyLeftQuantity = value; }
        }
        /// <summary>
        ///需求剩余数量
        /// </summary>
        virtual public decimal DemandLeftQuantity
        {
            get { return demandLeftQuantity; }
            set { demandLeftQuantity = value; }
        }
    }
}
