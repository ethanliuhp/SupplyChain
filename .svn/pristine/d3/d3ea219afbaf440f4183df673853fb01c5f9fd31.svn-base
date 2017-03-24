using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain
{
    /// <summary>
    /// 资源需求计划单
    /// </summary>
    [Serializable]
    public class ResourceRequireReceipt : BaseMaster
    {
        private string _receiptName;
        private ResourceRequirePlan _resourceRequirePlan;
        private string _resourceRequirePlanName;

        private OperationOrgInfo _opgOrgInfo;
        private string _opgOrgInfoName;
        private string _ownerOrgSysCode;


        private DateTime _planRequireDateBegin;
        private DateTime _planRequireDateEnd;

        private WeekScheduleMaster _schedulingProduction;
        private string _schedulingProductionName;

        private MaterialCategory _resourceCategory;
        private string _resourceCategorySysCode;

        private string _resourceRequirePlanTypeWord;
        private string _resourceRequirePlanTypeCode;

        private ResourceRequirePlanState _state;

        /// <summary>
        /// 状态
        /// </summary>
        public virtual ResourceRequirePlanState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 资源需求计划单名称
        /// </summary>
        public virtual string ReceiptName
        {
            get { return _receiptName; }
            set { _receiptName = value; }
        }

        /// <summary>
        /// 滚动资源需求计划GUID
        /// </summary>
        public virtual ResourceRequirePlan ResourceRequirePlan
        {
            get { return _resourceRequirePlan; }
            set { _resourceRequirePlan = value; }
        }

        /// <summary>
        /// 滚动资源需求计划名称
        /// </summary>
        public virtual string ResourceRequirePlanName
        {
            get { return _resourceRequirePlanName; }
            set { _resourceRequirePlanName = value; }
        }

        /// <summary>
        /// 需求计划开始时间
        /// </summary>
        public virtual DateTime PlanRequireDateBegin
        {
            get { return _planRequireDateBegin; }
            set { _planRequireDateBegin = value; }
        }

        /// <summary>
        /// 核算组织
        /// </summary>
        public virtual OperationOrgInfo OpgOrgInfo
        {
            get { return _opgOrgInfo; }
            set { _opgOrgInfo = value; }
        }

        /// <summary>
        /// 核算组织名称
        /// </summary>
        public virtual string OpgOrgInfoName
        {
            get { return _opgOrgInfoName; }
            set { _opgOrgInfoName = value; }
        }

        /// <summary>
        /// 核算组织层次码
        /// </summary>
        public virtual string OwnerOrgSysCode
        {
            get { return _ownerOrgSysCode; }
            set { _ownerOrgSysCode = value; }
        }

        /// <summary>
        /// 需求计划结束时间
        /// </summary>
        public virtual DateTime PlanRequireDateEnd
        {
            get { return _planRequireDateEnd; }
            set { _planRequireDateEnd = value; }
        }

        /// <summary>
        /// 参照进度计划GUID
        /// </summary>
        public virtual WeekScheduleMaster SchedulingProduction
        {
            get { return _schedulingProduction; }
            set { _schedulingProduction = value; }
        }

        /// <summary>
        /// 参照进度计划名称
        /// </summary>
        public virtual string SchedulingProductionName
        {
            get { return _schedulingProductionName; }
            set { _schedulingProductionName = value; }
        }


        /// <summary>
        /// 资源分类代码
        /// </summary>
        public virtual MaterialCategory ResourceCategory
        {
            get { return _resourceCategory; }
            set { _resourceCategory = value; }
        }

        /// <summary>
        /// 资源分类代码层次码
        /// </summary>
        public virtual string ResourceCategorySysCode
        {
            get { return _resourceCategorySysCode; }
            set { _resourceCategorySysCode = value; }
        }

        /// <summary>
        /// 资源需求计划类型代码(名称)
        /// </summary>
        public virtual string ResourceRequirePlanTypeWord
        {
            get { return _resourceRequirePlanTypeWord; }
            set { _resourceRequirePlanTypeWord = value; }
        }

        /// <summary>
        /// 资源需求计划类型代码(编码)
        /// </summary>
        public virtual string ResourceRequirePlanTypeCode
        {
            get { return _resourceRequirePlanTypeCode; }
            set { _resourceRequirePlanTypeCode = value; }
        }

        private PlanType stageplantype;
            
        ///<summary>
        ///计划类型
        ///</summary>
        public virtual PlanType StagePlanType
        {
            set { this.stageplantype = value; }
            get { return this.stageplantype; }
        }

        private ResourceTpye materialtype;

        ///<summary>
        ///资源类型
        ///</summary>
        public virtual ResourceTpye MaterialType
        {
            set { this.materialtype = value; }
            get { return this.materialtype; }
        }


    }

    public enum PlanType
    {
        [Description("总体计划")]
        总体计划 = 1,

        [Description("月度计划")]
        月度计划 = 2,

        [Description("日常计划")]
        日常计划 = 3

    }
    public enum ResourceTpye
    {
        [Description("资源")]
        资源 = 1,

        [Description("分包资源")]
        分包资源 = 2,

        [Description("物资资源")]
        物资资源 = 3,

        [Description("机械资源")]
        机械资源 = 4

    }

}
