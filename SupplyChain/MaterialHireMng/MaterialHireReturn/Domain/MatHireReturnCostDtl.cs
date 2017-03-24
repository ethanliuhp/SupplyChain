using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain
{
    /// <summary>
    /// 退料费用明细
    /// </summary>
    [Serializable]
    public class MatHireReturnCostDtl : BaseDetail
    {
        private string costType;
        private MatHireReturnDetail master;
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
        virtual public MatHireReturnDetail Master
        {
            get { return master; }
            set { master = value; }
        }

        private decimal constValue = 0;
        private string expression;

        /// <summary>
        /// 理论值
        /// </summary>
        public virtual decimal ConstValue
        {
            get { return constValue; }
            set { constValue = value; }
        }
        /// <summary>
        /// 公式
        /// </summary>
        public virtual string Expression
        {
            get { return expression; }
            set { expression = value; }
        }
    }
}
