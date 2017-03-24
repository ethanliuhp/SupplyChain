using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain
{
    /// <summary>
    /// 基本费用设置表
    /// </summary>
    [Serializable]
    public class OrderMasterCostSetItem : BaseDetail
    {
        private string matCostType;
        private int approachCalculation;
        private int exitCalculation;
        private int calculationMethod;
        private decimal costPrice;
        /// <summary>
        /// 料具费用类型
        /// </summary>
        virtual public string MatCostType
        {
            get { return matCostType; }
            set { matCostType = value; }
        }
        /// <summary>
        /// 进场计算 0:不算  1：算
        /// </summary>
        virtual public int ApproachCalculation
        {
            get { return approachCalculation; }
            set { approachCalculation = value; }
        }
        /// <summary>
        /// 退场计算 0:不算  1：算
        /// </summary>
        virtual public int ExitCalculation
        {
            get { return exitCalculation; }
            set { exitCalculation = value; }
        }
        /// <summary>
        /// 计算方式 0：非数量  1：数量
        /// </summary>
        virtual public int CalculationMethod
        {
            get { return calculationMethod; }
            set { calculationMethod = value; }
        }
        /// <summary>
        /// 费用单价（按车数计算使用）
        /// </summary>
        virtual public decimal CostPrice
        {
            get { return costPrice; }
            set { costPrice = value; }
        }
    }
}
