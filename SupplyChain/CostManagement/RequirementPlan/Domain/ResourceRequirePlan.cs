using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Iesi.Collections;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain
{
    /// <summary>
    /// 滚动资源需求计划
    /// </summary>
    [Serializable]
    public class ResourceRequirePlan : BaseMaster
    {
        private string _requirePlanVersion;
        private string _type;
        private string _ownerOrgSysCode;
        private ResourceRequirePlanState _state;
        private Iesi.Collections.Generic.ISet<ResourceRequirePlanDetail> _details = new Iesi.Collections.Generic.HashedSet<ResourceRequirePlanDetail>();
        private GWBSTree theGWBSTreeGUID;
        private string theGWBSTreeName;
        private string theGWBSTreeSyscode;
        
        /// <summary>
        /// 明细集合
        /// </summary>
        virtual public Iesi.Collections.Generic.ISet<ResourceRequirePlanDetail> Details
        {
            get { return _details; }
            set { _details = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual ResourceRequirePlanState State
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 责任人层次码
        /// </summary>
        public virtual string OwnerOrgSysCode
        {
            get { return _ownerOrgSysCode; }
            set { _ownerOrgSysCode = value; }
        }

        /// <summary>
        /// 需求计划名称(版本)
        /// </summary>
        virtual public string RequirePlanVersion
        {
            get { return _requirePlanVersion; }
            set { _requirePlanVersion = value; }
        }

        /// <summary>
        /// 计划类型
        /// </summary>
        public virtual string PlanType
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 选择的根节点
        /// </summary>
        virtual public GWBSTree TheGWBSTreeGUID
        {
            get { return theGWBSTreeGUID; }
            set { theGWBSTreeGUID = value; }
        }
        /// <summary>
        /// 选择的根节点名称
        /// </summary>
        virtual public string TheGWBSTreeName
        {
            get { return theGWBSTreeName; }
            set { theGWBSTreeName = value; }
        }
        /// <summary>
        /// 选择的根节点的层次码
        /// </summary>
        virtual public string TheGWBSTreeSyscode
        {
            get { return theGWBSTreeSyscode; }
            set { theGWBSTreeSyscode = value; }
        }

    }
    /// <summary>
    /// 资源需求计划类型
    /// </summary>
    public enum ResourceRequirePlanType
    {
        [Description("总体需求计划")]
        总体需求计划 = 1,
        [Description("滚动需求计划")]
        滚动需求计划 = 2
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
