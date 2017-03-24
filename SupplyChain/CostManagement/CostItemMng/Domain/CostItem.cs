using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain
{
    /// <summary>
    /// 成本项
    /// </summary>
    [Serializable]
    public class CostItem
    {
        private string id;
        private long version;
        private string code;
        private string _name;
        private string _quotaCode;
        private string _costDesc;
        private CostItemCategory _theCostItemCategory;
        private string _theCostItemCateSyscode;
        private StandardUnit _projectUnitGUID;
        private string _projectUnitName;
        private decimal _price;
        private StandardUnit _priceUnitGUID;
        private string _priceUnitName;
        private string _summary;
        private CostItemApplyLeve _applyLevel;

        private CostItemCategory _cateFilter1;
        private string _cateFilterName1;
        private string _cateFilterSysCode1;

        private CostItemCategory _cateFilter2;
        private string _cateFilterName2;
        private string _cateFilterSysCode2;

        private CostAccountSubject _subjectCateFilter1;
        private string _subjectCateFilterName1;
        private string _subjectCateFilterSyscode1;

        private CostAccountSubject _subjectCateFilter2;
        private string _subjectCateFilterName2;
        private string _subjectCateFilterSyscode2;

        private CostAccountSubject _subjectCateFilter3;
        private string _subjectCateFilterName3;
        private string _subjectCateFilterSyscode3;

        private decimal _pricingRate;
        private CostItemState _itemState = CostItemState.制定;
        private DateTime _createTime = DateTime.Now;
        private DateTime _updateTime = DateTime.Now;
        private string _theProjectGUID;
        private string _theProjectName;
        private Iesi.Collections.Generic.ISet<SubjectCostQuota> _listQuotas = new Iesi.Collections.Generic.HashedSet<SubjectCostQuota>();
        private Iesi.Collections.Generic.ISet<CostWorkForce> _listCostWorkForce = new Iesi.Collections.Generic.HashedSet<CostWorkForce>();
        private BasicDataOptr _managementMode;
        private string _managementModeName;
        private CostItemContentType _contentType;
        private string _costItemVersion;
        private CostItemType costItemType = CostItemType.标准;

        /// <summary>
        /// 临时字段，不做map
        /// </summary>
        public virtual decimal Temp1 { get; set; }


        /// <summary>
        /// 管理模式
        /// </summary>
        public virtual BasicDataOptr ManagementMode
        {
            get { return _managementMode; }
            set { _managementMode = value; }
        }
        /// <summary>
        /// 管理模式名称
        /// </summary>
        public virtual string ManagementModeName
        {
            get { return _managementModeName; }
            set { _managementModeName = value; }
        }
        /// <summary>
        /// 内容类型
        /// </summary>
        public virtual CostItemContentType ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }
        /// <summary>
        /// 成本项版本
        /// </summary>
        public virtual string CostItemVersion
        {
            get { return _costItemVersion; }
            set { _costItemVersion = value; }
        }



        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 编码
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }
        /// <summary>
        /// 成本项名称
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 对应定额编号
        /// </summary>
        public virtual string QuotaCode
        {
            get { return _quotaCode; }
            set { _quotaCode = value; }
        }
        /// <summary>
        /// 成本项说明
        /// </summary>
        public virtual string CostDesc
        {
            get { return _costDesc; }
            set { _costDesc = value; }
        }
        /// <summary>
        /// 所属成本项分类
        /// </summary>
        public virtual CostItemCategory TheCostItemCategory
        {
            get { return _theCostItemCategory; }
            set { _theCostItemCategory = value; }
        }
        /// <summary>
        /// 所属分类层次码
        /// </summary>
        public virtual string TheCostItemCateSyscode
        {
            get { return _theCostItemCateSyscode; }
            set { _theCostItemCateSyscode = value; }
        }
        /// <summary>
        /// 工程量计量单位GUID
        /// </summary>
        public virtual StandardUnit ProjectUnitGUID
        {
            get { return _projectUnitGUID; }
            set { _projectUnitGUID = value; }
        }
        /// <summary>
        /// 工程量计量单位名称
        /// </summary>
        public virtual string ProjectUnitName
        {
            get { return _projectUnitName; }
            set { _projectUnitName = value; }
        }
        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal Price
        {
            get { return _price; }
            set { _price = value; }
        }
        /// <summary>
        /// 价格计量单位GUID
        /// </summary>
        public virtual StandardUnit PriceUnitGUID
        {
            get { return _priceUnitGUID; }
            set { _priceUnitGUID = value; }
        }
        /// <summary>
        /// 价格计量单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return _priceUnitName; }
            set { _priceUnitName = value; }
        }
        /// <summary>
        /// 摘要
        /// </summary>
        public virtual string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }
        /// <summary>
        /// 使用级别
        /// </summary>
        public virtual CostItemApplyLeve ApplyLevel
        {
            get { return _applyLevel; }
            set { _applyLevel = value; }
        }

        /// <summary>
        /// 基数成本项分类过滤一
        /// </summary>
        public virtual CostItemCategory CateFilter1
        {
            get { return _cateFilter1; }
            set { _cateFilter1 = value; }
        }
        /// <summary>
        /// 基数成本项分类过滤名称一
        /// </summary>
        public virtual string CateFilterName1
        {
            get { return _cateFilterName1; }
            set { _cateFilterName1 = value; }
        }
        /// <summary>
        /// 基数成本项分类过滤层次码一
        /// </summary>
        public virtual string CateFilterSysCode1
        {
            get { return _cateFilterSysCode1; }
            set { _cateFilterSysCode1 = value; }
        }

        /// <summary>
        /// 基数成本项分类过滤二
        /// </summary>
        public virtual CostItemCategory CateFilter2
        {
            get { return _cateFilter2; }
            set { _cateFilter2 = value; }
        }
        /// <summary>
        /// 基数成本项分类过滤名称二
        /// </summary>
        public virtual string CateFilterName2
        {
            get { return _cateFilterName2; }
            set { _cateFilterName2 = value; }
        }
        /// <summary>
        /// 基数成本项分类过滤层次码二
        /// </summary>
        public virtual string CateFilterSysCode2
        {
            get { return _cateFilterSysCode2; }
            set { _cateFilterSysCode2 = value; }
        }

        /// <summary>
        /// 基数成本科目分类过滤1
        /// </summary>
        public virtual CostAccountSubject SubjectCateFilter1
        {
            get { return _subjectCateFilter1; }
            set { _subjectCateFilter1 = value; }
        }
        /// <summary>
        /// 基数成本科目分类过滤名称1
        /// </summary>
        public virtual string SubjectCateFilterName1
        {
            get { return _subjectCateFilterName1; }
            set { _subjectCateFilterName1 = value; }
        }
        /// <summary>
        /// 基数成本科目分类过滤层次码1
        /// </summary>
        public virtual string SubjectCateFilterSyscode1
        {
            get { return _subjectCateFilterSyscode1; }
            set { _subjectCateFilterSyscode1 = value; }
        }

        /// <summary>
        /// 基数成本科目分类过滤2
        /// </summary>
        public virtual CostAccountSubject SubjectCateFilter2
        {
            get { return _subjectCateFilter2; }
            set { _subjectCateFilter2 = value; }
        }
        /// <summary>
        /// 基数成本科目分类过滤名称2
        /// </summary>
        public virtual string SubjectCateFilterName2
        {
            get { return _subjectCateFilterName2; }
            set { _subjectCateFilterName2 = value; }
        }
        /// <summary>
        /// 基数成本科目分类过滤层次码2
        /// </summary>
        public virtual string SubjectCateFilterSyscode2
        {
            get { return _subjectCateFilterSyscode2; }
            set { _subjectCateFilterSyscode2 = value; }
        }

        /// <summary>
        /// 基数成本科目分类过滤3
        /// </summary>
        public virtual CostAccountSubject SubjectCateFilter3
        {
            get { return _subjectCateFilter3; }
            set { _subjectCateFilter3 = value; }
        }
        /// <summary>
        /// 基数成本科目分类过滤名称3
        /// </summary>
        public virtual string SubjectCateFilterName3
        {
            get { return _subjectCateFilterName3; }
            set { _subjectCateFilterName3 = value; }
        }
        /// <summary>
        /// 基数成本科目分类过滤层次码3
        /// </summary>
        public virtual string SubjectCateFilterSyscode3
        {
            get { return _subjectCateFilterSyscode3; }
            set { _subjectCateFilterSyscode3 = value; }
        }

        /// <summary>
        /// 计价费率
        /// </summary>
        public virtual decimal PricingRate
        {
            get { return _pricingRate; }
            set { _pricingRate = value; }
        }
        /// <summary>
        /// 成本项状态
        /// </summary>
        public virtual CostItemState ItemState
        {
            get { return _itemState; }
            set { _itemState = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }
        /// <summary>
        /// 所属项目
        /// </summary>
        public virtual string TheProjectGUID
        {
            get { return _theProjectGUID; }
            set { _theProjectGUID = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string TheProjectName
        {
            get { return _theProjectName; }
            set { _theProjectName = value; }
        }
        /// <summary>
        /// 分科目成本定额
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<SubjectCostQuota> ListQuotas
        {
            get { return _listQuotas; }
            set { _listQuotas = value; }
        }

        /// <summary>
        /// 分科目成本定额
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<CostWorkForce> ListCostWorkForce
        {
            get { return _listCostWorkForce; }
            set { _listCostWorkForce = value; }
        }

        //移除的属性
        private CostItemPricingType _pricingType;
        private decimal _priceNumber;
        private decimal _subContractPrice;
        private string _partGUID;
        private string _partName;
        private string _resourceTypeGUID;
        private string _resourceTypeName;
        private string _methodGUID;
        private string _methodName;
        private string _territoryGUID;
        private string _territoryName;

        /// <summary>
        /// 定价类型
        /// </summary>
        public virtual CostItemPricingType PricingType
        {
            get { return _pricingType; }
            set { _pricingType = value; }
        }
        /// <summary>
        /// 单价所含数量
        /// </summary>
        public virtual decimal PriceNumber
        {
            get { return _priceNumber; }
            set { _priceNumber = value; }
        }
        /// <summary>
        /// 分包单价
        /// </summary>
        public virtual decimal SubContractPrice
        {
            get { return _subContractPrice; }
            set { _subContractPrice = value; }
        }
        /// <summary>
        /// 部位GUID
        /// </summary>
        public virtual string PartGUID
        {
            get { return _partGUID; }
            set { _partGUID = value; }
        }
        /// <summary>
        /// 部位名称
        /// </summary>
        public virtual string PartName
        {
            get { return _partName; }
            set { _partName = value; }
        }
        /// <summary>
        /// 资源类型GUID
        /// </summary>
        public virtual string ResourceTypeGUID
        {
            get { return _resourceTypeGUID; }
            set { _resourceTypeGUID = value; }
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
        /// 做法GUID
        /// </summary>
        public virtual string MethodGUID
        {
            get { return _methodGUID; }
            set { _methodGUID = value; }
        }
        /// <summary>
        /// 做法名称
        /// </summary>
        public virtual string MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }
        /// <summary>
        /// 地域GUID
        /// </summary>
        public virtual string TerritoryGUID
        {
            get { return _territoryGUID; }
            set { _territoryGUID = value; }
        }
        /// <summary>
        /// 地域名称
        /// </summary>
        public virtual string TerritoryName
        {
            get { return _territoryName; }
            set { _territoryName = value; }
        }
        /// <summary>
        /// 成本项类型
        /// </summary>
        public virtual CostItemType CostItemType
        {
            get { return costItemType; }
            set { costItemType = value; }
        }

        /// <summary>
        /// 是否是常用项【1：是；0：否】（便于在工程成本批量维护中使用）
        /// </summary>
        public virtual bool IsCommonlyUsed { get; set; }
    }
    /// <summary>
    /// 定价类型
    /// </summary>
    public enum CostItemPricingType
    {
        [Description("固定价格")]
        固定价格 = 1,
        [Description("费率价格")]
        费率价格 = 2
    }
    /// <summary>
    /// 使用级别
    /// </summary>
    public enum CostItemApplyLeve
    {
        [Description("项目部")]
        项目部 = 1,
        [Description("公司")]
        公司 = 2
    }
    /// <summary>
    /// 成本项状态
    /// </summary>
    public enum CostItemState
    {
        [Description("制定")]
        制定 = 1,
        [Description("发布")]
        发布 = 2,
        [Description("作废")]
        作废 = 3
    }
    /// <summary>
    /// 内容类型
    /// </summary>
    public enum CostItemContentType
    {
        [Description("工程实体")]
        工程实体 = 0,
        [Description("非工程实体")]
        非工程实体 = 1,
        [Description("分包取费")]
        分包取费 = 2
    }
    /// <summary>
    /// 成本项用于拷贝操作的类型
    /// </summary>
    public enum CostItemCopyTypeEnum
    {
        拆除 = 1
    }
    /// <summary>
    /// 成本向类型
    /// </summary>
    public enum CostItemType
    {
        标准 = 0,
        拆除 = 1
    }
}
