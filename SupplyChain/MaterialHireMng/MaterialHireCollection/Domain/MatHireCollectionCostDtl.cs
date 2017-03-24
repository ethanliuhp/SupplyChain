using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain
{
    /// <summary>
    /// 收料费用明细
    /// </summary>
    [Serializable]
   public  class MatHireCollectionCostDtl : BaseDetail
    {
        private string costType;
        private MatHireCollectionDetail master;

        //Quantity Price  Money ConstValue Expression MatStandardUnitName
        private decimal constValue=0;
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
        virtual public MatHireCollectionDetail Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
