using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.MaterialResource.Domain;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain
{
    /// <summary>
    /// 元素
    /// </summary>
    [Serializable]
    public class Elements
    {
        private string id;
        private long version;
        private string name;
        private string code;
        private StandardUnit unit;
        private string unitName;
        private string description;
        private decimal workAmount;
        private string type;
        private string resources;
        private string resourcesName;       
        private ISet<ElementFeature> _Details = new HashedSet<ElementFeature>();
        private string pbsId;
        private string pbsName;

        public virtual ISet<ElementFeature> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

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
        /// 名称
        /// </summary>
        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        /// <summary>
        /// 代码
        /// </summary>
        public virtual string Code
        {
            get { return code; }
            set { code = value; }
        }
        
        /// <summary>
        /// 工程量计量单位
        /// </summary>
        public virtual StandardUnit Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        
        /// <summary>
        /// 单位名称
        /// </summary>
        public virtual string UnitName
        {
            get { return unitName; }
            set { unitName = value; }
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
        /// 元素工程量
        /// </summary>
        public virtual decimal WorkAmount
        {
            get { return workAmount; }
            set { workAmount = value; }
        }
        
        /// <summary>
        /// 元素类型
        /// </summary>
        public virtual string Type
        {
            get { return type; }
            set { type = value; }
        }
        
        /// <summary>
        /// 元素资源
        /// </summary>
        public virtual string Resources
        {
            get { return resources; }
            set { resources = value; }
        }

        /// <summary>
        /// 元素资源名称
        /// </summary>
        public virtual string ResourcesName
        {
            get { return resourcesName; }
            set { resourcesName = value; }
        }

        /// <summary>
        /// pbs节点id
        /// </summary>
        public virtual string PbsId
        {
            get { return pbsId; }
            set { pbsId = value; }
        }

       /// <summary>
       /// pbs节点名称
       /// </summary>
        public virtual string PbsName
        {
            get { return pbsName; }
            set { pbsName = value; }
        }
    }
}
