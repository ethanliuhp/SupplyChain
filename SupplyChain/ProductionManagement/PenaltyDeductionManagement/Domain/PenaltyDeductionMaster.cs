using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using System.ComponentModel;
//using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain
{
    /// <summary>
    /// 专业罚款类型
    /// </summary>
    public enum PenaltyDeductionType
    {
        [Description("日常检查罚款")]
        日常检查罚款 = 1,
        [Description("暂扣款")]
        暂扣款 = 2,
        [Description("暂扣款红单")]
        暂扣款红单 = 3,
        [Description("代工扣款")]
        代工扣款 = 4
    }

    /// <summary>
    /// 罚扣款单主表
    /// </summary>
    [Serializable]
    public class PenaltyDeductionMaster : BaseMaster
    {
        private SubContractProject penaltyDeductionRant;
        private string penaltyDeductionRantName;
        private PenaltyDeductionType penaltyType;
        private string penaltyDeductionReason;
        private decimal sumQuantity;
        private string checkOrderId;
        /// <summary>
        /// 数量汇总
        /// </summary>
        public override decimal SumQuantity
        {
            get
            {
                decimal tmpQuantity = 0;
                //汇总
                foreach (PenaltyDeductionDetail var in Details)
                {
                    if (PenaltyType == PenaltyDeductionType.代工扣款)
                    {
                        tmpQuantity += var.AccountQuantity;
                    }
                    else
                    {
                        tmpQuantity += var.PenaltyQuantity;
                    }
                }
                sumQuantity = tmpQuantity;
                return sumQuantity;
            }
            set { sumQuantity = value; }
        }

        private decimal sumMoney;
        private string _OEMId;
        /// <summary>
        /// 金额汇总
        /// </summary>
        public override decimal SumMoney
        {
            get
            {
                decimal temMoney = 0;
                //汇总
                foreach (PenaltyDeductionDetail var in Details)
                {
                    if (PenaltyType == PenaltyDeductionType.代工扣款)
                    {
                        temMoney += var.AccountMoney;
                    }
                    else
                    {
                        temMoney += var.PenaltyMoney;
                    }
                }
                sumMoney = temMoney;
                return sumMoney;
            }
            set
            {
                sumMoney = value;
            }
        }

        /// <summary>
        /// 罚款原因
        /// </summary>
        virtual public string PenaltyDeductionReason
        {
            get { return penaltyDeductionReason; }
            set { penaltyDeductionReason = value; }
        }

        /// <summary>
        /// 专业罚款类型
        /// </summary>
        virtual public PenaltyDeductionType PenaltyType
        {
            get { return penaltyType; }
            set { penaltyType = value; }
        }

        /// <summary>
        /// 罚扣队伍
        /// </summary>
        virtual public SubContractProject PenaltyDeductionRant
        {
            get { return penaltyDeductionRant; }
            set { penaltyDeductionRant = value; }
        }

        /// <summary>
        /// 罚扣队伍名称
        /// </summary>
        virtual public string PenaltyDeductionRantName
        {
            get { return penaltyDeductionRantName; }
            set { penaltyDeductionRantName = value; }
        }
        /// <summary>
        /// 检查单ID
        /// </summary>
        public virtual string CheckOrderId
        {
            get { return checkOrderId; }
            set { checkOrderId = value; }
        }
        /// <summary>
        /// 检查单ID
        /// </summary>
        public virtual string OEMId
        {
            get { return _OEMId; }
            set { _OEMId = value; }
        }
        

        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
