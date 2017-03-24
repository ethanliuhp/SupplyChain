using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain
{
    /// <summary>
    /// PBS节点关系
    /// </summary>
    [Serializable]
    public class PBSNodeRelation
    {
        private string id;
        private long version;
        private string relationType;
        private string description;
        private PBSTree targetPBS;
        private string targetPBSName;
        private string targetPBSSyscode;
        private PBSTree sourcePBS;
        private string sourcePBSName;
        private string sourcePBSSyscode;

        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }


        public virtual long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 关系类型
        /// </summary>
        public virtual string RelationType
        {
            get { return relationType; }
            set { relationType = value; }
        }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 目标PBS节点
        /// </summary>
        public virtual PBSTree TargetPBS
        {
            get { return targetPBS; }
            set { targetPBS = value; }
        }

        /// <summary>
        /// 目标PBS节点名称
        /// </summary>
        public virtual string TargetPBSName
        {
            get { return targetPBSName; }
            set { targetPBSName = value; }
        }

        /// <summary>
        /// 目标PBS节点层次码
        /// </summary>
        public virtual string TargetPBSSyscode
        {
            get { return targetPBSSyscode; }
            set { targetPBSSyscode = value; }
        }

        /// <summary>
        /// 源PBS节点
        /// </summary>
        public virtual PBSTree SourcePBS
        {
            get { return sourcePBS; }
            set { sourcePBS = value; }
        }

        /// <summary>
        /// 源PBS节点名称
        /// </summary>
        public virtual string SourcePBSName
        {
            get { return sourcePBSName; }
            set { sourcePBSName = value; }
        }

        /// <summary>
        /// 源PBS节点层次码
        /// </summary>
        public virtual string SourcePBSSyscode
        {
            get { return sourcePBSSyscode; }
            set { sourcePBSSyscode = value; }
        }
    }
}
