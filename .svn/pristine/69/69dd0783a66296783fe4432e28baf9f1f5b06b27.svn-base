using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Basic.Domain
{
    /// <summary>
    /// 项目机械费责任成本参数
    /// </summary>
    [Serializable]
    public class MachineCostParame
    {
        public virtual string Id { get; set; }
        /// <summary>
        /// 核算科目id
        /// </summary>
        public virtual string SubjectId { get; set; }
        /// <summary>
        /// 核算科目code
        /// </summary>
        public virtual string SubjectCode { get; set; }
        /// <summary>
        /// 核算科目名称
        /// </summary>
        public virtual string SubjectName { get; set; }
        /// <summary>
        /// 工期
        /// </summary>
        public virtual int Duration { get; set; }
        /// <summary>
        /// 实际进场日期
        /// </summary>
        public virtual DateTime ActualentryDate { get; set; }
        /// <summary>
        /// 实际出场日期
        /// </summary>
        public virtual DateTime ActualoutDate { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public virtual string ProjectId { get; set; }
        /// <summary>
        /// 主表，工程项目信息
        /// </summary>
        public virtual CurrentProjectInfo Resconfig { get; set; }
    }
}
