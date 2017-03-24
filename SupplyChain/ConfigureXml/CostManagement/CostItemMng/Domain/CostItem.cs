using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

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
        private string _projectUnitGUID;
        private string _projectUnitName;
        private CostItemPricingType _pricingType;
        private decimal _price;
        private decimal _priceNumber;
        private decimal _subContractPrice;
        private string _priceUnitGUID;
        private string _priceUnitName;
        private string _summary;
        private CostItemApplyLeve _applyLevel;
        private string _baseCostItemCateFilter;
        private string _baseCostSubjectCateFilter1;
        private string _baseCostSubjectCateFilter2;
        private string _baseCostSubjectCateFilter3;
        private decimal _pricingRate;
        private string _partGUID;
        private string _partName;
        private string _resourceTypeGUID;
        private string _resourceTypeName;
        private string _methodGUID;
        private string _methodName;
        private string _territoryGUID;
        private string _territoryName;
        private CostItemState _itemState;
        private DateTime _createTime = DateTime.Now;
        private DateTime _updateTime = DateTime.Now;
        private string _theProjectGUID;
        private string _theProjectName;
        private Iesi.Collections.Generic.ISet<SubjectCostQuota> _listQuotas = new Iesi.Collections.Generic.HashedSet<SubjectCostQuota>();


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
        /// 工程量计量单位GUID
        /// </summary>
        public virtual string ProjectUnitGUID
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
        /// 定价类型
        /// </summary>
        public virtual CostItemPricingType PricingType
        {
            get { return _pricingType; }
            set { _pricingType = value; }
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
        /// 价格计量单位GUID
        /// </summary>
        public virtual string PriceUnitGUID
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
        /// 基数成本项分类过滤
        /// </summary>
        public virtual string BaseCostItemCateFilter
        {
            get { return _baseCostItemCateFilter; }
            set { _baseCostItemCateFilter = value; }
        }

        /// <summary>
        /// 基数成本科目分类过滤1
        /// </summary>
        public virtual string BaseCostSubjectCateFilter1
        {
            get { return _baseCostSubjectCateFilter1; }
            set { _baseCostSubjectCateFilter1 = value; }
        }

        /// <summary>
        /// 基数成本科目分类过滤2
        /// </summary>
        public virtual string BaseCostSubjectCateFilter2
        {
            get { return _baseCostSubjectCateFilter2; }
            set { _baseCostSubjectCateFilter2 = value; }
        }

        /// <summary>
        /// 基数成本科目分类过滤3
        /// </summary>
        public virtual string BaseCostSubjectCateFilter3
        {
            get { return _baseCostSubjectCateFilter3; }
            set { _baseCostSubjectCateFilter3 = value; }
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
}
