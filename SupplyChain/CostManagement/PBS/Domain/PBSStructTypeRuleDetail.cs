using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain
{
    /// <summary>
    /// PBS结构类型规则（明细）
    /// </summary>
    [Serializable]
    public class PBSStructTypeRuleDetail
    {
        private string id;
        private long version;
        private string _StructType;
        private PBSStructTypeRuleMaster _theParent;


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
        /// <summary>
        /// 父对象
        /// </summary>
        public virtual PBSStructTypeRuleMaster TheParent
        {
            get { return _theParent; }
            set { _theParent = value; }
        }
    }
}
