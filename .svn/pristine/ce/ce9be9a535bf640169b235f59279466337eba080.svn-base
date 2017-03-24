using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialCollectionMng.Domain
{
    /// <summary>
    /// 收料费用明细
    /// </summary>
    [Serializable]
    public class MaterialCostDtl : BaseDetail
    {
        private string costType;
        private MaterialCollectionDetail master;

        /// <summary>
        /// 费用类型
        /// </summary>
        virtual public string CostType
        {
            get { return costType; }
            set { costType = value; }
        }
        /// <summary>
        /// 收料明细（GUID）
        /// </summary>
        virtual public MaterialCollectionDetail Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
