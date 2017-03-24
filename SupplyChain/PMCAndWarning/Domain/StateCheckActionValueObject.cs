using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.PMCAndWarning.Domain
{
    /// <summary>
    /// 状态检查值对象
    /// </summary>
    [Serializable]
    public class StateCheckActionValueObject
    {
        /// <summary>
        /// 所属项目ID
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 所属项目代码
        /// </summary>
        public string ProjectSysCode { get; set; }
        /// <summary>
        /// 所属项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 进度计划节点
        /// </summary>
        public ProductionScheduleDetail SchedulePlanNode { get; set; }

        /// <summary>
        /// 计划开始时间
        /// </summary>
        public DateTime? PlanBeginTime { get; set; }
        /// <summary>
        /// 计划结束时间
        /// </summary>
        public DateTime? PlanEndTime { get; set; }
        /// <summary>
        /// 计划工期
        /// </summary>
        public decimal PlanDuration { get; set; }

        /// <summary>
        /// 实际开始时间
        /// </summary>
        public string FactBeginTime { get; set; }
        /// <summary>
        /// 实际结束时间
        /// </summary>
        public string FactEndTime { get; set; }
        /// <summary>
        /// 实际工期
        /// </summary>
        public string FactDuration { get; set; }

        /// <summary>
        /// 预计实际开始时间
        /// </summary>
        public DateTime? PrecastFactEndTime { get; set; }
        /// <summary>
        /// 预计工期
        /// </summary>
        public decimal PrecastDuration { get; set; }
        /// <summary>
        /// 工期状态
        /// </summary>
        public DurationStateEnum DurationState { get; set; }
        /// <summary>
        /// 预警级别
        /// </summary>
        public WarningLevelEnum Level { get; set; }
        /// <summary>
        /// 预警内容
        /// </summary>
        public string WarningContent { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public PersonInfo Owner { get; set; }
        /// <summary>
        /// 责任人名称
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// 责任人组织层次码
        /// </summary>
        public string OwnerOrgSysCode { get; set; }

    }
    /// <summary>
    /// 工期状态枚举
    /// </summary>
    public enum DurationStateEnum
    {
        未开工 = 1,
        未完工 = 2,
        已完工 = 3
    }

    /// <summary>
    /// 资料状态检查值对象
    /// </summary>
    [Serializable]
    public class StateCheckActionValueObjectOnMeans
    {
        /// <summary>
        /// 所属项目ID
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 所属项目代码
        /// </summary>
        public string ProjectSysCode { get; set; }
        /// <summary>
        /// 所属项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目任务
        /// </summary>
        public GWBSTree TaskNode { get; set; }
        /// <summary>
        /// 项目任务名称
        /// </summary>
        public string TaskNodeName { get; set; }
        /// <summary>
        /// 项目任务层次码
        /// </summary>
        public string TaskNodeSysCode { get; set; }
        /// <summary>
        /// 文档名称
        /// </summary>
        public string DocumentName { get; set; }
        /// <summary>
        /// 提交状态
        /// </summary>
        public ProjectDocumentSubmitState submitState { get; set; }

        /// <summary>
        /// 工期状态
        /// </summary>
        public DurationStateEnum DurationState { get; set; }
        /// <summary>
        /// 预警级别
        /// </summary>
        public WarningLevelEnum Level { get; set; }
        /// <summary>
        /// 预警内容
        /// </summary>
        public string WarningContent { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public PersonInfo Owner { get; set; }
        /// <summary>
        /// 责任人名称
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// 责任人组织层次码
        /// </summary>
        public string OwnerOrgSysCode { get; set; }

    }

    /// <summary>
    /// 物资预警状态检查值对象
    /// </summary>
    [Serializable]
    public class StateCheckActionValueObjectOnWZ
    {
        /// <summary>
        /// 所属项目ID
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 所属项目代码
        /// </summary>
        public string ProjectSysCode { get; set; }
        /// <summary>
        /// 所属项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 预警级别
        /// </summary>
        public WarningLevelEnum Level { get; set; }
        /// <summary>
        /// 预警内容
        /// </summary>
        public string WarningContent { get; set; }
        /// <summary>
        /// 责任人
        /// </summary>
        public PersonInfo Owner { get; set; }
        /// <summary>
        /// 责任人名称
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// 责任人组织层次码
        /// </summary>
        public string OwnerOrgSysCode { get; set; }
    }
}
