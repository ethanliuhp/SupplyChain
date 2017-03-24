using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialCollectionMng.Domain
{
    /// <summary>
    /// 料具非数量费用
    /// </summary>
    [Serializable]
    public class MaterialNotQtyCost : BaseDetail
    {
        private string costType;
        private MaterialCollectionMaster master;

        /// <summary>
        /// 费用类型
        /// </summary>
        virtual public string CostType
        {
            get { return costType; }
            set { costType = value; }
        }
        /// <summary>
        /// 收料主表GUID
        /// </summary>
        virtual public MaterialCollectionMaster Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
