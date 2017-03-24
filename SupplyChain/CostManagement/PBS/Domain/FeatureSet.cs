using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain
{
    public class FeatureSet
    {
        private string id;
        private string name;
        private string versionId;
        private string markId;
        private string description;
        private string propertySetNam;
        private ISet<Feature> details = new HashedSet<Feature>();

        /// <summary>
        /// id
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
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
        /// 版本id
        /// </summary>
        public virtual string VersionId
        {
            get { return versionId; }
            set { versionId = value; }
        }
        
        /// <summary>
        /// 标识
        /// </summary>
        public virtual string MarkId
        {
            get { return markId; }
            set { markId = value; }
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
        /// IFC属性集名称
        /// </summary>
        public virtual string PropertySetNam
        {
            get { return propertySetNam; }
            set { propertySetNam = value; }
        }

        public virtual ISet<Feature> Details
        {
            get { return details; }
            set { details = value; }
        }


    }
}
