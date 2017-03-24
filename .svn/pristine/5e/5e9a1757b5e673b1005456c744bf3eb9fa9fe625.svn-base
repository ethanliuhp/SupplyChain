using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain
{
    /// <summary>
    /// 资源组
    /// </summary>
    [Serializable]
    public class ResourceGroup
    {
        private string id;
        private long version;
        private string _resourceTypeGUID;
        private string _resourceTypeCode;
        private string _resourceTypeName;
        private string _resourceTypeSpec;
        private string diagramNumber;

        private string _resourceTypeQuality;
        private bool _isCateResource = false;
        private string _resourceCateId;
        private string _resourceCateSyscode;

        private string _description;
        private SubjectCostQuota _theCostQuota;


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
        /// 资源类型
        /// </summary>
        public virtual string ResourceTypeGUID
        {
            get { return _resourceTypeGUID; }
            set { _resourceTypeGUID = value; }
        }
        /// <summary>
        /// 资源类型代码
        /// </summary>
        public virtual string ResourceTypeCode
        {
            get { return _resourceTypeCode; }
            set { _resourceTypeCode = value; }
        }
        /// <summary>
        /// 资源类型名称
        /// </summary>
        public virtual string ResourceTypeName
        {
            get { return _resourceTypeName; }
            set { _resourceTypeName = value; }
        }
        /// <summary>
        /// 规格
        /// </summary>
        public virtual string ResourceTypeSpec
        {
            get { return _resourceTypeSpec; }
            set { _resourceTypeSpec = value; }
        }

        /// <summary>
        /// 图号
        /// </summary>
        virtual public string DiagramNumber
        {
            get { return diagramNumber; }
            set { diagramNumber = value; }
        }

        /// <summary>
        /// 材质
        /// </summary>
        public virtual string ResourceTypeQuality
        {
            get { return _resourceTypeQuality; }
            set { _resourceTypeQuality = value; }
        }
        /// <summary>
        /// 是否是分类资源
        /// </summary>
        public virtual bool IsCateResource
        {
            get { return _isCateResource; }
            set { _isCateResource = value; }
        }
        /// <summary>
        /// 资源分类Id
        /// </summary>
        public virtual string ResourceCateId
        {
            get { return _resourceCateId; }
            set { _resourceCateId = value; }
        }
        /// <summary>
        /// 资源分类层次码
        /// </summary>
        public virtual string ResourceCateSyscode
        {
            get { return _resourceCateSyscode; }
            set { _resourceCateSyscode = value; }
        }
        /// <summary>
        /// 说明
        /// </summary>
        public virtual string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// 所属资源耗用定额
        /// </summary>
        public virtual SubjectCostQuota TheCostQuota
        {
            get { return _theCostQuota; }
            set { _theCostQuota = value; }
        }
    }
}
