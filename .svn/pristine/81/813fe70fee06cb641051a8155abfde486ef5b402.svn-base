using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程WBS关联PBS
    /// </summary>
    [Serializable]
    public class GWBSRelaPBS
    {
        private string id;
        private long version;

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

        private PBSTree _thePBS;
        /// <summary>
        /// 关联的PBS对象
        /// </summary>
        public virtual PBSTree ThePBS
        {
            get { return _thePBS; }
            set { _thePBS = value; }
        }

        private string _PBSName;
        /// <summary>
        /// 关联的PBS对象名称
        /// </summary>
        public virtual string PBSName
        {
            get { return _PBSName; }
            set { _PBSName = value; }
        }

        private GWBSTree _theGWBSTree;
        /// <summary>
        /// 所属工程WBS
        /// </summary>
        public virtual GWBSTree TheGWBSTree
        {
            get { return _theGWBSTree; }
            set { _theGWBSTree = value; }
        }

    }
}
