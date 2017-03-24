using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
//using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain
{
    /// <summary>
    /// 派工类型
    /// </summary>
    public enum LaborType
    {
        计时派工,
        零星用工,
        代工
    }

    /// <summary>
    /// 零星用工主表
    /// </summary>
    [Serializable]
    public class LaborSporadicMaster : BaseMaster
    {
        private SubContractProject bearTeam;
        private string bearTeamName;
        private string deductionLabor;//代工扣款单
        private decimal sumPredictProjectNum;
        private decimal sumRealProjectNum;
        private string laborState;
        private string penaltyDeductionMaster;
        private PersonInfo accountPerson;
        private string accountPersonName;
        private int isCreate = 0;
        /// <summary>
        /// 是否是计划
        /// </summary>
        public virtual bool IsPlan { set; get; }
        /// <summary>
        /// 是否已经复核
        /// </summary>
        public virtual int IsCreate
        {
            get { return isCreate; }
            set { isCreate = value; }
        }
        //private Iesi.Collections.Generic.ISet<LaborSporadicDetail> details = new Iesi.Collections.Generic.HashedSet<LaborSporadicDetail>();
        //virtual public Iesi.Collections.Generic.ISet<LaborSporadicDetail> Details
        //{
        //    get { return details; }
        //    set { details = value; }
        //}
        /// <summary>
        /// 核算人
        /// </summary>
        public virtual PersonInfo AccountPerson
        {
            get { return accountPerson; }
            set { accountPerson = value; }
        }
        /// <summary>
        /// 核算人名称
        /// </summary>
        public virtual string AccountPersonName
        {
            get { return accountPersonName; }
            set { accountPersonName = value; }
        }

        /// <summary>
        /// 罚扣款单GUID
        /// </summary>
        public virtual string PenaltyDeductionMaster
        {
            get { return penaltyDeductionMaster; }
            set { penaltyDeductionMaster = value; }
        }
        /// <summary>
        /// 派工状态
        /// </summary>
        virtual public string LaborState
        {
            get { return laborState; }
            set { laborState = value; }
        }


        /// <summary>
        /// 计划工程总量
        /// </summary>
        virtual public decimal SumPredictProjectNum
        {
            get
            {
                decimal tmpPreQuantity = 0;
                //汇总
                foreach (LaborSporadicDetail var in Details)
                {
                    tmpPreQuantity += var.PredictLaborNum;
                }
                sumPredictProjectNum = tmpPreQuantity;
                return sumPredictProjectNum;
            }
            set { sumPredictProjectNum = value; }
        }

        /// <summary>
        /// 实际工程总量
        /// </summary>
        virtual public decimal SumRealProjectNum
        {
            get
            {
                decimal tmpRealQuantity = 0;
                //汇总
                foreach (LaborSporadicDetail var in Details)
                {
                    tmpRealQuantity += var.RealLaborNum;
                }
                sumRealProjectNum = tmpRealQuantity;
                return sumRealProjectNum;
            }
            set { sumRealProjectNum = value; }
        }

        ///// <summary>
        ///// 用工类型
        ///// </summary>
        //virtual public LaborType LaborState
        //{
        //    get { return laborState; }
        //    set { laborState = value; }
        //}

        /// <summary>
        /// 承担队伍
        /// </summary>
        virtual public SubContractProject BearTeam
        {
            get { return bearTeam; }
            set { bearTeam = value; }
        }
        /// <summary>
        /// 承担队伍名称
        /// </summary>
        virtual public string BearTeamName
        {
            get { return bearTeamName; }
            set { bearTeamName = value; }
        }

        /// <summary>
        /// 代工扣款单
        /// </summary>
        virtual public string DeductionLabor
        {
            get { return deductionLabor; }
            set { deductionLabor = value; }
        }

        private string _monthlySettlment;

        /// <summary>
        /// 月度成本核算单号
        /// </summary>
        public virtual string MonthlySettlment
        {
            get { return _monthlySettlment; }
            set { _monthlySettlment = value; }
        }       
    }
}
