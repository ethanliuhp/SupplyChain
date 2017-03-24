using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain
{
    /// <summary>
    /// 资源需求计划明细
    /// </summary>
    [Serializable]
    public class ResourceRequireReceiptDetail : BaseDetail
    {
        private ResourceRequireReceiptDetailState _state;        
        private decimal _plannedCostQuantity;
        private decimal _firstOfferRequireQuantity;
        private decimal _responsibilityCostQuantity;
        private decimal _planInRequireQuantity;
        private decimal _planOutRequireQuantity;
        private decimal _dailyPlanPublishQuantity;
        private decimal _supplyPlanPublishQuantity;
        private decimal _costQuantity;
        private decimal _periodQuantity;
        private StandardUnit _quantityUnitGUID;
        private string _quantityUnitName;
        private string _approachRequestDesc;
        private GWBSTree _theGWBSTaskGUID;
        private string _theGWBSTaskName;
        private string _theGWBSSysCode;
        private ResourceRequireReceipt _theResReceipt;
        private MaterialCategory _materialCategory;
        private string _materialCategoryName;
        private string technicalParameters;
        private PlanRequireType _requireType;
        private DateTime _approachDate;
        private string qualityStandards;
        private SubContractProject usedRank;
        private string usedRankName;

       /// <summary>
        /// 专业计划发布累计量
        /// </summary>
        public virtual decimal SupplyPlanPublishQuantity
        {
            get { return _supplyPlanPublishQuantity; }
            set { _supplyPlanPublishQuantity = value; }
        }

        /// <summary>
        /// 使用队伍ID
        /// </summary>
        virtual public SubContractProject UsedRank
        {
            get { return usedRank; }
            set { usedRank = value; }
        }
        /// <summary>
        /// 使用队伍名称
        /// </summary>
        virtual public string UsedRankName
        {
            get { return usedRankName; }
            set { usedRankName = value; }
        }

        /// <summary>
        /// 质量标准
        /// </summary>
        public virtual string QualityStandards
        {
            get { return qualityStandards; }
            set { qualityStandards = value; }
        }

        /// <summary>
        /// 进场时间
        /// </summary>
        public virtual DateTime ApproachDate
        {
            get { return _approachDate; }
            set { _approachDate = value; }
        }

        /// <summary>
        /// 需求类型
        /// </summary>
        public virtual PlanRequireType RequireType
        {
            get { return _requireType; }
            set { _requireType = value; }
        }

        /// <summary>
        /// 技术参数
        /// </summary>
        public virtual string TechnicalParameters
        {
            get { return technicalParameters; }
            set { technicalParameters = value; }
        }

        /// <summary>
        /// 资源需求计划单
        /// </summary>
        public virtual ResourceRequireReceipt TheResReceipt
        {
            get { return _theResReceipt; }
            set { _theResReceipt = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual ResourceRequireReceiptDetailState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 计划成本量
        /// </summary>
        public virtual decimal PlannedCostQuantity
        {
            get { return _plannedCostQuantity; }
            set { _plannedCostQuantity = value; }
        }

        /// <summary>
        /// 甲供需求量
        /// </summary>
        public virtual decimal FirstOfferRequireQuantity
        {
            get { return _firstOfferRequireQuantity; }
            set { _firstOfferRequireQuantity = value; }
        }

        /// <summary>
        /// 责任成本量
        /// </summary>
        public virtual decimal ResponsibilityCostQuantity
        {
            get { return _responsibilityCostQuantity; }
            set { _responsibilityCostQuantity = value; }
        }
        
        /// <summary>
        /// 计划内需求总量
        /// </summary>
        public virtual decimal PlanInRequireQuantity
        {
            get { return _planInRequireQuantity; }
            set { _planInRequireQuantity = value; }
        }

        /// <summary>
        /// 计划外需求总量
        /// </summary>
        public virtual decimal PlanOutRequireQuantity
        {
            get { return _planOutRequireQuantity; }
            set { _planOutRequireQuantity = value; }
        }
        
        /// <summary>
        /// 日常计划发布累计量
        /// </summary>
        public virtual decimal DailyPlanPublishQuantity
        {
            get { return _dailyPlanPublishQuantity; }
            set { _dailyPlanPublishQuantity = value; }
        }
        
        /// <summary>
        /// 期间需求量
        /// </summary>
        public virtual decimal PeriodQuantity
        {
            get { return _periodQuantity; }
            set { _periodQuantity = value; }
        }
        
        /// <summary>
        /// 已执行累积量
        /// </summary>
        public virtual decimal CostQuantity
        {
            get { return _costQuantity; }
            set { _costQuantity = value; }
        }
        
        /// <summary>
        /// 数量计量单位GUID
        /// </summary>
        public virtual StandardUnit QuantityUnitGUID
        {
            get { return _quantityUnitGUID; }
            set { _quantityUnitGUID = value; }
        }
        
        /// <summary>
        /// 数量计量单位名称
        /// </summary>
        public virtual string QuantityUnitName
        {
            get { return _quantityUnitName; }
            set { _quantityUnitName = value; }
        }
        
        /// <summary>
        /// 进场要求说明
        /// </summary>
        public virtual string ApproachRequestDesc
        {
            get { return _approachRequestDesc; }
            set { _approachRequestDesc = value; }
        }
        
        /// <summary>
        /// 所属工程项目任务GUID
        /// </summary>
        public virtual GWBSTree TheGWBSTaskGUID
        {
            get { return _theGWBSTaskGUID; }
            set { _theGWBSTaskGUID = value; }
        }
        
        /// <summary>
        /// 所属工程项目任务名称
        /// </summary>
        public virtual string TheGWBSTaskName
        {
            get { return _theGWBSTaskName; }
            set { _theGWBSTaskName = value; }
        }
        
        /// <summary>
        /// 所属工程项目任务层次码
        /// </summary>
        public virtual string TheGWBSSysCode
        {
            get { return _theGWBSSysCode; }
            set { _theGWBSSysCode = value; }
        }

        
        /// <summary>
        /// 资源类型分类
        /// </summary>
        public virtual MaterialCategory MaterialCategory
        {
            get { return _materialCategory; }
            set { _materialCategory = value; }
        }

        /// <summary>
        /// 资源类型分类名称
        /// </summary>
        public virtual string MaterialCategoryName
        {
            get { return _materialCategoryName; }
            set { _materialCategoryName = value; }
        }
    }

    /// <summary>
    /// 资源需求计划状态
    /// </summary>
    public enum ResourceRequireReceiptDetailState
    {
        [Description("有效")]
        有效 = 1,
        [Description("作废")]
        作废 = 2,
        [Description("执行完毕")]
        执行完毕 = 3
    }
}
