using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialReturnMng.Domain
{
    /// <summary>
    ///退料非数量费用 
    /// </summary>
    [Serializable]
    public class MaterialReturnNotQtyCost : BaseDetail
    {
        private string costType;
        private MaterialReturnMaster master;

        /// <summary>
        /// 费用类型
        /// </summary>
        virtual public string CostType
        {
            get { return costType; }
            set { costType = value; }
        }
        /// <summary>
        /// 退料主表(GUID)
        /// </summary>
        virtual public MaterialReturnMaster Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
