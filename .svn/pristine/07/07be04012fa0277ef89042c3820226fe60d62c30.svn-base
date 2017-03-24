using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// GWBS中关联PBS和任务类型组合规则（父）
    /// </summary>
    [Serializable]
    public class PBSRelaTaskTypeRuleMaster
    {
        private string id;
        private long version;
        private string _PBSType;
        private string _taskType;

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        public virtual long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// PBS类型
        /// </summary>
        public virtual string PBSType
        {
            get { return _PBSType; }
            set { _PBSType = value; }
        }
        /// <summary>
        /// 任务类型
        /// </summary>
        public virtual string TaskType
        {
            get { return _taskType; }
            set { _taskType = value; }
        }

        ISet<PBSRelaTaskTypeRuleDetail> _details = new HashedSet<PBSRelaTaskTypeRuleDetail>();
        /// <summary>
        /// 规则明细
        /// </summary>
        public virtual ISet<PBSRelaTaskTypeRuleDetail> Details
        {
            get { return _details; }
            set { _details = value; }
        }
    }
}
