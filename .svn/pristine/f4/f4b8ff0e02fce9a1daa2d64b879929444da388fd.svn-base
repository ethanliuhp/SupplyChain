using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain
{
    /// <summary>
    /// 费用明细设置表
    /// </summary>
    [Serializable]
    public class BasicDtlCostSet : BaseDetail
    {
        private string costType;
        private string workContent;
        private string approachExpression;
        private string exitExpression;
        private string setType;
        private MaterialRentalOrderDetail master;

        virtual public MaterialRentalOrderDetail Master
        {
            get { return master; }
            set { master = value; }
        }
        /// <summary>
        /// 费用类型（价格类型）
        /// </summary>
        virtual public string CostType
        {
            get { return costType; }
            set { costType = value; }
        }
        /// <summary>
        /// 工作内容
        /// </summary>
        virtual public string WorkContent
        {
            get { return workContent; }
            set { workContent = value; }
        }
        /// <summary>
        /// 进场公式
        /// </summary>
        virtual public string ApproachExpression
        {
            get { return approachExpression; }
            set { approachExpression = value; }
        }
        /// <summary>
        /// 退场公式
        /// </summary>
        virtual public string ExitExpression
        {
            get { return exitExpression; }
            set { exitExpression = value; }
        }
        /// <summary>
        /// 设置类型  
        /// </summary>
        virtual public string SetType
        {
            get { return setType; }
            set { setType = value; }
        }
    }
}
