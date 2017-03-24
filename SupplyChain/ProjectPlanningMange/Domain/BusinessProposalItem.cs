using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.ProjectPlanningMange.Domain
{
    /// <summary>
    /// 策划状态
    /// </summary>
    public enum EnumItemPlanningState
    {
        [Description("策划")]
        策划 = 0,
        [Description("实施")]
        实施 = 1,
        [Description("中止")]
        中止 = 2,
        [Description("完成")]
        完成 = 3,
    }
    /// <summary>
    /// 商务策划点
    /// </summary>
    [Serializable]
    public class BusinessProposalItem : BaseDetail
    {
        private string planningItemType;  //策划点类型
        private decimal planningPlannedCost;  //策划后成本
        private decimal planningIncome;  //策划后收入
        private string planningTheme;  //策划主题
        private DateTime planningImplementDate;  //策划结束时间
        private DateTime planningImplementStartDate;  //策划开始时间
        private EnumItemPlanningState planningState;  //策划状态 
        private DateTime refreshDate;  //更新时间  
        private DateTime planningDateEnd;  //计划完成时间  
        private DateTime planningDateStart;  //计划开始时间  
        private decimal benefitRegulation;  //效益增减     
        private decimal plannedCost;  //原计划成本
        private decimal formerProceeds;  //原收入
        private decimal state;
        private Iesi.Collections.Generic.ISet<IrpBusinessPlanManageLog> irpDetails = new Iesi.Collections.Generic.HashedSet<IrpBusinessPlanManageLog>();
        virtual public Iesi.Collections.Generic.ISet<IrpBusinessPlanManageLog> IrpDetails
        {
            get { return irpDetails; }
            set { irpDetails = value; }
        }
        virtual public void AddIrpDetail(IrpBusinessPlanManageLog detail)
        {
            detail.Master = this;
            IrpDetails.Add(detail);
        }

        /// <summary>
        /// 策划点类型
        /// </summary>
        virtual public string PlanningItemType
        {
            get { return planningItemType; }
            set { planningItemType = value; }
        }

        /// <summary>
        /// 策划后成本
        /// </summary>
        virtual public decimal PlanningPlannedCost
        {
            get { return planningPlannedCost; }
            set { planningPlannedCost = value; }
        }
        /// <summary>
        /// 策划后收入
        /// </summary>
        virtual public decimal PlanningIncome
        {
            get { return planningIncome; }
            set { planningIncome = value; }
        }
        /// <summary>
        /// 策划实施时间
        /// </summary>
        virtual public DateTime PlanningImplementDate
        {
            get { return planningImplementDate; }
            set { planningImplementDate = value; }
        }
        /// <summary>
        /// 策划开始时间
        /// </summary>
        virtual public DateTime PlanningImplementStartDate
        {
            get { return planningImplementStartDate; }
            set { planningImplementStartDate = value; }
        }
        /// <summary>
        /// 策划主题
        /// </summary>
        virtual public string PlanningTheme
        {
            get { return planningTheme; }
            set { planningTheme = value; }
        }
        /// <summary>
        /// 策划状态
        /// </summary>
        virtual public EnumItemPlanningState PlanningState
        {
            get { return planningState; }
            set { planningState = value; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        virtual public DateTime RefreshDate
        {
            get { return refreshDate; }
            set { refreshDate = value; }
        }
        /// <summary>
        /// 计划完成时间
        /// </summary>
        virtual public DateTime PlanningDateEnd
        {
            get { return planningDateEnd; }
            set { planningDateEnd = value; }
        }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        virtual public DateTime PlanningDateStart
        {
            get { return planningDateStart; }
            set { planningDateStart = value; }
        }
        /// <summary>
        /// 效益增减
        /// </summary>
        virtual public decimal BenefitRegulation
        {
            get { return benefitRegulation; }
            set { benefitRegulation = value; }
        }
        /// <summary>
        /// 原计划成本
        /// </summary>
        virtual public decimal PlannedCost
        {
            get { return plannedCost; }
            set { plannedCost = value; }
        }
        /// <summary>
        /// 原收入
        /// </summary>
        virtual public decimal FormerProceeds
        {
            get { return formerProceeds; }
            set { formerProceeds = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        virtual public decimal State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
