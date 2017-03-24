using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain
{
    /// <summary>
    /// PBS结构类型规则（父）
    /// </summary>
    [Serializable]
    public class PBSStructTypeRuleMaster
    {
        private string id;
        private long version;
        private string _StructType;
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
        /// PBS结构类型
        /// </summary>
        public virtual string StructType
        {
            get { return _StructType; }
            set { _StructType = value; }
        }

        ISet<PBSStructTypeRuleDetail> _details = new HashedSet<PBSStructTypeRuleDetail>();
        /// <summary>
        /// 规则明细
        /// </summary>
        public virtual ISet<PBSStructTypeRuleDetail> Details
        {
            get { return _details; }
            set { _details = value; }
        }
    }
}
