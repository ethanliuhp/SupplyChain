using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain
{
    /// <summary>
    /// 滚动资源需求计划明细
    /// </summary>
    [Serializable]
    public class ResourceRequirePlanDetail : BaseDetail
    {
        private ResourceRequirePlanDetailState _state;
        private DateTime _stateUpdateTime;
        private decimal _plannedCostQuantity;
        private decimal _planInRequireQuantity;
        private decimal _planOutRequireQuantity;
        private decimal _firstOfferRequireQuantity;
        private decimal _dailyPlanPublishQuantity;
        private decimal _responsibilityCostQuantity;
        private decimal _monthPlanPublishQuantity;
        private decimal _supplyPlanPublishQuantity;
        private decimal _executedQuantity;
        private PlanRequireType _requireType;
        private StandardUnit _quantityUnitGUID;
        private string _quantityUnitName;
        private GWBSTree _theGWBSTaskGUID;
        private string _theGWBSTaskName;
        private string _theGWBSSysCode;
        private string _summary;
        private DateTime _createTime = DateTime.Now;
        private ResourceRequirePlan _theResourceRequirePlan;
        private MaterialCategory _resourceCategory;
        private string _resourceTypeClassification;
        private string theProjectGUID;
    
        private string technicalParameters;
        private string qualityStandards;

        /// <summary>
        /// 质量标准
        /// </summary>
        public virtual string QualityStandards
        {
            get { return qualityStandards; }
            set { qualityStandards = value; }
        }

        /// <summary>
        /// 归属项目
        /// </summary>
        public virtual string TheProjectGUID
        {
            get { return theProjectGUID; }
            set { theProjectGUID = value; }
        }
        private string theProjectName;
        /// <summary>
        /// 归属项目名称
        /// </summary>
        public virtual string TheProjectName
        {
            get { return theProjectName; }
            set { theProjectName = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual ResourceRequirePlanDetailState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 状态更新时间
        /// </summary>
        public virtual DateTime StateUpdateTime
        {
            get { return _stateUpdateTime; }
            set { _stateUpdateTime = value; }
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
        /// 甲供需求量
        /// </summary>
        public virtual decimal FirstOfferRequireQuantity
        {
            get { return _firstOfferRequireQuantity; }
            set { _firstOfferRequireQuantity = value; }
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
        /// 责任成本量
        /// </summary>
        public virtual decimal ResponsibilityCostQuantity
        {
            get { return _responsibilityCostQuantity; }
            set { _responsibilityCostQuantity = value; }
        }

        /// <summary>
        /// 月度计划发布累计量
        /// </summary>
        public virtual decimal MonthPlanPublishQuantity
        {
            get { return _monthPlanPublishQuantity; }
            set { _monthPlanPublishQuantity = value; }
        }

        /// <summary>
        /// 专业计划发布累计量
        /// </summary>
        public virtual decimal SupplyPlanPublishQuantity
        {
            get { return _supplyPlanPublishQuantity; }
            set { _supplyPlanPublishQuantity = value; }
        }
        /// <summary>
        /// 已执行累积量
        /// </summary>
        public virtual decimal ExecutedQuantity
        {
            get { return _executedQuantity; }
            set { _executedQuantity = value; }
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
        /// 摘要
        /// </summary>
        public virtual string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 所属滚动资源需求计划
        /// </summary>
        public virtual ResourceRequirePlan TheResourceRequirePlan
        {
            get { return _theResourceRequirePlan; }
            set { _theResourceRequirePlan = value; }
        }

        /// <summary>
        /// 资源类型分类
        /// </summary>
        public virtual MaterialCategory ResourceCategory
        {
            get { return _resourceCategory; }
            set { _resourceCategory = value; }
        }

        /// <summary>
        /// 资源类型分类名称
        /// </summary>
        public virtual string ResourceTypeClassification
        {
            get { return _resourceTypeClassification; }
            set { _resourceTypeClassification = value; }
        }

        /// <summary>
        /// 技术参数
        /// </summary>
        public virtual string TechnicalParameters
        {
            get { return technicalParameters; }
            set { technicalParameters = value; }
        }







        private DateTime? _planBeginApproachDate;
        /// <summary>
        /// 计划开始进场日期（已删除）
        /// </summary>
        public virtual DateTime? PlanBeginApproachDate
        {
            get { return _planBeginApproachDate; }
            set { _planBeginApproachDate = value; }
        }
        private DateTime? _planEndApproachDate;
        /// <summary>
        /// 计划结束进场日期（已删除）
        /// </summary>
        public virtual DateTime? PlanEndApproachDate
        {
            get { return _planEndApproachDate; }
            set { _planEndApproachDate = value; }
        }
        private decimal _responsibilityRequireQuantity;
        /// <summary>
        /// 责任需求量（已删除）
        /// </summary>
        public virtual decimal ResponsibilityRequireQuantity
        {
            get { return _responsibilityRequireQuantity; }
            set { _responsibilityRequireQuantity = value; }
        }
        private decimal _planRequireQuantity;
        /// <summary>
        /// 计划需求量（已删除）
        /// </summary>
        public virtual decimal PlanRequireQuantity
        {
            get { return _planRequireQuantity; }
            set { _planRequireQuantity = value; }
        }
        private string _buildResourceTypeGUID;
        /// <summary>
        /// 建筑资源类型GUID（已删除）
        /// </summary>
        public virtual string BuildResourceTypeGUID
        {
            get { return _buildResourceTypeGUID; }
            set { _buildResourceTypeGUID = value; }
        }
        private string _buildResourceTypeName;
        /// <summary>
        /// 建筑资源类型名称（已删除）
        /// </summary>
        public virtual string BuildResourceTypeName
        {
            get { return _buildResourceTypeName; }
            set { _buildResourceTypeName = value; }
        }
        private string _serviceType;
        /// <summary>
        /// 劳务类型（已删除）
        /// </summary>
        public virtual string ServiceType
        {
            get { return _serviceType; }
            set { _serviceType = value; }
        }
        private decimal _approachPlanPublishQuantity;
        /// <summary>
        /// 进场计划发布量累计（已删除）
        /// </summary>
        public virtual decimal ApproachPlanPublishQuantity
        {
            get { return _approachPlanPublishQuantity; }
            set { _approachPlanPublishQuantity = value; }
        }
        private decimal _approachPlanExecuteQuantity;
        /// <summary>
        /// 进场计划已执行数量（已删除）
        /// </summary>
        public virtual decimal ApproachPlanExecuteQuantity
        {
            get { return _approachPlanExecuteQuantity; }
            set { _approachPlanExecuteQuantity = value; }
        }
        private string _approachRequestDesc;
        /// <summary>
        /// 进场要求说明（已删除）
        /// </summary>
        public virtual string ApproachRequestDesc
        {
            get { return _approachRequestDesc; }
            set { _approachRequestDesc = value; }
        }
    }

    /// <summary>
    /// 滚动资源需求计划状态
    /// </summary>
    public enum ResourceRequirePlanDetailState
    {
        [Description("编制")]
        编制 = 1,
        [Description("发布")]
        发布 = 2,
        [Description("作废")]
        作废 = 3,
        [Description("执行完毕")]
        执行完毕 = 4
    }

    /// <summary>
    /// 资源需求计划中的资源类型
    /// </summary>
    public enum ResourceRequirePlanDetailResourceType
    {
        [Description("物资")]
        物资 = 1,
        [Description("劳务")]
        劳务 = 2
    }
    /// <summary>
    /// 资源需求计划中的劳务类型
    /// </summary>
    public enum ResourceRequirePlanDetailServiceType
    {
        [Description("主体")]
        主体 = 1,
        [Description("砌体")]
        砌体 = 2,
        [Description("门窗安装")]
        门窗安装 = 3,
        [Description("幕墙")]
        幕墙 = 4,
        [Description("防水")]
        防水 = 4
    }

    public enum PlanRequireType
    {
        [Description("计划内需求")]
        计划内需求 = 1,
        [Description("计划外需求")]
        计划外需求 = 2
    }
}
