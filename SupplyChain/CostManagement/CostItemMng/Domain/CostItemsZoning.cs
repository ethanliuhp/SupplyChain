using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain
{
    /// <summary>
    /// 成本项分类区域划分
    /// </summary>
    [Serializable]
    public class CostItemsZoning
    {
        private string id;
        private long version;
        private string gwbsId;
        private string gwbsSyscode;
        private string gwbsName;
        private string costItemsCategoryId;
        private string costItemsCateSyscode;
        private string costItemsCateName;
        private string projectId;

        /// <summary>
        /// id
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
        /// 工程任务id
        /// </summary>
        public virtual string GwbsId
        {
            get { return gwbsId; }
            set { gwbsId = value; }
        }
        /// <summary>
        /// 工程任务名称
        /// </summary>
        public virtual string GwbsName
        {
            get { return gwbsName; }
            set { gwbsName = value; }
        }
        /// <summary>
        /// 工程任务层次码
        /// </summary>
        public virtual string GwbsSyscode
        {
            get { return gwbsSyscode; }
            set { gwbsSyscode = value; }
        }
        /// <summary>
        /// 成本项分类id
        /// </summary>
        public virtual string CostItemsCategoryId
        {
            get { return costItemsCategoryId; }
            set { costItemsCategoryId = value; }
        }
        /// <summary>
        /// 成本项分类名称
        /// </summary>
        public virtual string CostItemsCateName
        {
            get { return costItemsCateName; }
            set { costItemsCateName = value; }
        }
        /// <summary>
        /// 成本项分类层次码
        /// </summary>
        public virtual string CostItemsCateSyscode
        {
            get { return costItemsCateSyscode; }
            set { costItemsCateSyscode = value; }
        }
        /// <summary>
        /// 项目Id
        /// </summary>
        public virtual string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }


    }
}
