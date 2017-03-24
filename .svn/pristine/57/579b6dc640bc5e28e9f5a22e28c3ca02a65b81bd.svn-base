using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain
{
    /// <summary>
    /// 退料费用明细
    /// </summary>
    [Serializable]
    public class MaterialReturnCostDtl : BaseDetail
    {
        private string costType;
        private MaterialReturnDetail master;
        /// <summary>
        /// 费用类型
        /// </summary>
        virtual public string CostType
        {
            get { return costType; }
            set { costType = value; }
        }
        /// <summary>
        /// 退料明细(GUID)
        /// </summary>
        virtual public MaterialReturnDetail Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
