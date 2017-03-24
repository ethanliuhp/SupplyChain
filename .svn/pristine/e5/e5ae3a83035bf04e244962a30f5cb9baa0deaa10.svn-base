using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain
{
    /// <summary>
    /// 元素工程特性值
    /// </summary>
    [Serializable]
    public class ElementFeature
    {
        private string id;
        private long version;
        private string featureSet;
        private string featureName;
        private StandardUnit unit;
        private string unitName;
        private string lable;
        private string description;
        private string value;
        private string valueFormat;
        private Elements master;

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
        /// IFC特性集
        /// </summary>
        public virtual string FeatureSet
        {
            get { return featureSet; }
            set { featureSet = value; }
        }
        /// <summary>
        /// IFC特性值
        /// </summary>
        public virtual string FeatureName
        {
            get { return featureName; }
            set { featureName = value; }
        }

        /// <summary>
        /// 计量单位
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
        /// 特性标签
        /// </summary>
        public virtual string Lable
        {
            get { return lable; }
            set { lable = value; }
        }

        /// <summary>
        /// 特性描述
        /// </summary>
        public virtual string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// 特性值
        /// </summary>
        public virtual string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// 特性值格式
        /// </summary>
        public virtual string ValueFormat
        {
            get { return valueFormat; }
            set { valueFormat = value; }
        }
        /// <summary>
        /// 元素
        /// </summary>
        public virtual Elements Master
        {
            get { return master; }
            set { master = value; }
        }
    }
}
