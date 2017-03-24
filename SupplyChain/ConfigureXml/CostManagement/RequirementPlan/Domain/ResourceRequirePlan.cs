using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Iesi.Collections;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain
{
    /// <summary>
    /// 资源需求计划
    /// </summary>
    [Serializable]
    public class ResourceRequirePlan
    {
        private string _id;
        private long _version;
        private string _code;
        private string _ownerGUID;
        private string _ownerName;
        private string _ownerOrgSysCode;
        private string _requirePlanVersion;
        private ResourceRequirePlanState _state;
        private DateTime _createTime;

        private string _type;

        private string _theProjectGUID;
        private string _theProjectName;

        private ISet<ResourceRequirePlanDetail> _details = new HashedSet<ResourceRequirePlanDetail>();


        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return _version; }
            set { _version = value; }
        }
        /// <summary>
        /// Code
        /// </summary>
        virtual public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 责任人GUID
        /// </summary>
        virtual public string OwnerGUID
        {
            get { return _ownerGUID; }
            set { _ownerGUID = value; }
        }

        /// <summary>
        /// 责任人名
        /// </summary>
        virtual public string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }

        /// <summary>
        /// 责任人组织层次码
        /// </summary>
        virtual public string OwnerOrgSysCode
        {
            get { return _ownerOrgSysCode; }
            set { _ownerOrgSysCode = value; }
        }

        /// <summary>
        /// 需求计划版本
        /// </summary>
        virtual public string RequirePlanVersion
        {
            get { return _requirePlanVersion; }
            set { _requirePlanVersion = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        virtual public ResourceRequirePlanState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        virtual public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual string PlanType
        {
            get { return _type; }
            set { _type = value; }
        }

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

        /// <summary>
        /// 资源需求计划明细
        /// </summary>
        public virtual ISet<ResourceRequirePlanDetail> Details
        {
            get { return _details; }
            set { _details = value; }
        }

    }
    /// <summary>
    /// 资源需求计划状态
    /// </summary>
    public enum ResourceRequirePlanState
    {
        /// <summary>
        /// 编辑状态
        /// </summary>
        [Description("制定")]
        制定 = 1,
        /// <summary>
        /// 提交
        /// </summary>
        [Description("提交")]
        提交 = 2,
        /// <summary>
        /// 发布
        /// </summary>
        [Description("发布")]
        发布 = 3,
        /// <summary>
        /// 冻结
        /// </summary>
        [Description("冻结")]
        冻结 = 4,
        /// <summary>
        /// 作废
        /// </summary>
        [Description("作废")]
        作废 = 5
    }
}
