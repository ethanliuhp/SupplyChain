using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain
{
    /// <summary>
    ///退料非数量费用 
    /// </summary>
    [Serializable]
    public class MatHireReturnNotQtyCost : BaseDetail
    {
        private string costType;
        private MatHireReturnMaster master;

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
        virtual public MatHireReturnMaster Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
