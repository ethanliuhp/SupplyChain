using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Iesi.Collections;
using Iesi.Collections.Generic;
using System.ComponentModel;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程WBS树
    /// </summary>
    [Entity]
    [Serializable]
    public class GWBSTreeDetailRelationship
    {
        /// <summary>
        /// 主键
        /// </summary>
        public virtual string Id { set; get; }
        /// <summary>
        /// 源任务节点ID
        /// </summary>
        public virtual string SourseGWBSTreeID { set; get; }
        /// <summary>
        /// 源任务节点明细ID
        /// </summary>
        public virtual string SourseGWBSDetailID { set; get; }
        /// <summary>
        /// 目标任务节点ID
        /// </summary>
        public virtual string TargetGWBSTreeID { set; get; }
        /// <summary>
        /// 目标任务节点明细ID
        /// </summary>
        public virtual string TargetGWBSDetailID { set; get; }

        /// <summary>
        /// 目标任务节点分摊所占比例
        /// </summary>
        public virtual decimal Rate { set; get; }

        #region 扩展字段
        /// <summary>
        /// 目标任务节点明细
        /// </summary>
        public virtual GWBSDetail TargetGWBSDetail { set; get; }
        #endregion

    }
}