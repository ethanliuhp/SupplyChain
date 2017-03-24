using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain
{
    /// <summary>
    /// 资源需求计划明细
    /// </summary>
    [Serializable]
    public class ResourceRequirePlanDetail
    {
        private string _id;
        private long _version;
        private string _summary;
        private ResourceRequirePlanDetailState _state;
        private DateTime _stateUpdateTime;
        private decimal _firstOfferRequireQuantity;

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        public virtual long Version
        {
            get { return _version; }
            set { _version = value; }
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
        /// 甲供需求量
        /// </summary>
        public virtual decimal FirstOfferRequireQuantity
        {
            get { return _firstOfferRequireQuantity; }
            set { _firstOfferRequireQuantity = value; }
        }

        private decimal _responsibilityRequireQuantity;
        /// <summary>
        /// 责任需求量
        /// </summary>
        public virtual decimal ResponsibilityRequireQuantity
        {
            get { return _responsibilityRequireQuantity; }
            set { _responsibilityRequireQuantity = value; }
        }
        private decimal _planRequireQuantity;
        /// <summary>
        /// 计划需求量
        /// </summary>
        public virtual decimal PlanRequireQuantity
        {
            get { return _planRequireQuantity; }
            set { _planRequireQuantity = value; }
        }
        private decimal _monthPlanPublishQuantity;
        /// <summary>
        /// 月度计划发布累计量
        /// </summary>
        public virtual decimal MonthPlanPublishQuantity
        {
            get { return _monthPlanPublishQuantity; }
            set { _monthPlanPublishQuantity = value; }
        }
        private decimal _approachPlanPublishQuantity;
        /// <summary>
        /// 进场计划发布量累计
        /// </summary>
        public virtual decimal ApproachPlanPublishQuantity
        {
            get { return _approachPlanPublishQuantity; }
            set { _approachPlanPublishQuantity = value; }
        }
        private decimal _approachPlanExecuteQuantity;
        /// <summary>
        /// 进场计划已执行数量
        /// </summary>
        public virtual decimal ApproachPlanExecuteQuantity
        {
            get { return _approachPlanExecuteQuantity; }
            set { _approachPlanExecuteQuantity = value; }
        }
        private string _quantityUnitGUID;
        /// <summary>
        /// 数量计量单位GUID
        /// </summary>
        public virtual string QuantityUnitGUID
        {
            get { return _quantityUnitGUID; }
            set { _quantityUnitGUID = value; }
        }
        private string _quantityUnitName;
        /// <summary>
        /// 数量计量单位名称
        /// </summary>
        public virtual string QuantityUnitName
        {
            get { return _quantityUnitName; }
            set { _quantityUnitName = value; }
        }
        private DateTime? _planBeginApproachDate;
        /// <summary>
        /// 计划开始进场日期
        /// </summary>
        public virtual DateTime? PlanBeginApproachDate
        {
            get { return _planBeginApproachDate; }
            set { _planBeginApproachDate = value; }
        }
        private DateTime? _planEndApproachDate;
        /// <summary>
        /// 计划结束进场日期
        /// </summary>
        public virtual DateTime? PlanEndApproachDate
        {
            get { return _planEndApproachDate; }
            set { _planEndApproachDate = value; }
        }
        private string _approachRequestDesc;
        /// <summary>
        /// 进场要求说明
        /// </summary>
        public virtual string ApproachRequestDesc
        {
            get { return _approachRequestDesc; }
            set { _approachRequestDesc = value; }
        }
        private GWBSTree _theGWBSTaskGUID;
        /// <summary>
        /// 所属工程项目任务GUID
        /// </summary>
        public virtual GWBSTree TheGWBSTaskGUID
        {
            get { return _theGWBSTaskGUID; }
            set { _theGWBSTaskGUID = value; }
        }
        private string _theGWBSTaskName;
        /// <summary>
        /// 所属工程项目任务名称
        /// </summary>
        public virtual string TheGWBSTaskName
        {
            get { return _theGWBSTaskName; }
            set { _theGWBSTaskName = value; }
        }
        private string _buildResourceTypeGUID;
        /// <summary>
        /// 建筑资源类型GUID
        /// </summary>
        public virtual string BuildResourceTypeGUID
        {
            get { return _buildResourceTypeGUID; }
            set { _buildResourceTypeGUID = value; }
        }
        private string _buildResourceTypeName;
        /// <summary>
        /// 建筑资源类型名称
        /// </summary>
        public virtual string BuildResourceTypeName
        {
            get { return _buildResourceTypeName; }
            set { _buildResourceTypeName = value; }
        }

        private string _serviceType;
        /// <summary>
        /// 劳务类型
        /// </summary>
        public virtual string ServiceType
        {
            get { return _serviceType; }
            set { _serviceType = value; }
        }

        private Material _materialGUID;
        /// <summary>
        /// 资源对象(物料)GUID
        /// </summary>
        public virtual Material MaterialGUID
        {
            get { return _materialGUID; }
            set { _materialGUID = value; }
        }
        private string _materialCode;
        /// <summary>
        /// 资源对象(物料)编码
        /// </summary>
        public virtual string MaterialCode
        {
            get { return _materialCode; }
            set { _materialCode = value; }
        }
        private string _materialName;
        /// <summary>
        /// 资源对象(物料)名称
        /// </summary>
        public virtual string MaterialName
        {
            get { return _materialName; }
            set { _materialName = value; }
        }
        private string _materialSpec;
        /// <summary>
        /// 资源对象(物料)规格型号
        /// </summary>
        public virtual string MaterialSpec
        {
            get { return _materialSpec; }
            set { _materialSpec = value; }
        }


        private DateTime _createTime = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        private string _theProjectGUID;
        private string _theProjectName;
        /// <summary>
        /// 项目GUID
        /// </summary>
        public virtual string TheProjectGUID
        {
            get { return _theProjectGUID; }
            set { _theProjectGUID = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string TheProjectName
        {
            get { return _theProjectName; }
            set { _theProjectName = value; }
        }


        private ResourceRequirePlan _theResourceRequirePlan;
        /// <summary>
        /// 所属资源需求计划
        /// </summary>
        public virtual ResourceRequirePlan TheResourceRequirePlan
        {
            get { return _theResourceRequirePlan; }
            set { _theResourceRequirePlan = value; }
        }
    }

    /// <summary>
    /// 资源需求计划状态
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
}
