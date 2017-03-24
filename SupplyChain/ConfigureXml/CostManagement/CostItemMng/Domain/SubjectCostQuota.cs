using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain
{
    /// <summary>
    /// 分科目成本定额
    /// </summary>
    [Serializable]
    public class SubjectCostQuota
    {
        private string id;
        private long version;
        private string code;
        private string _name;
        private decimal _quotaProjectAmount;
        private decimal _quotaPrice;
        private decimal _quotaMoney;
        private decimal _wastage;
        private StandardUnit _projectAmountUnitGUID;
        private string _projectAmountUnitName;
        private StandardUnit _priceUnitGUID;
        private string _priceUnitName;
        private string _quantityResponsibleOrgGUID;
        private string _quantityResponsibleOrgName;
        private string _priceResponsibleOrgGUID;
        private string _priceResponsibleOrgName;
        private decimal _assessmentRate;
        private CostItem _theCostItem;
        private string _resourceTypeGUID;
        private string _resourceTypeName;
        private CostAccountSubject _costAccountSubjectGUID;
        private string _costAccountSubjectName;
        private SubjectCostQuotaState _state = SubjectCostQuotaState.编制;
        private DateTime _createTime = DateTime.Now;
        private string _theProjectGUID;
        private string _theProjectName;

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
        /// 成本名称
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 定额工程量
        /// </summary>
        public virtual decimal QuotaProjectAmount
        {
            get { return _quotaProjectAmount; }
            set { _quotaProjectAmount = value; }
        }

        /// <summary>
        /// 定额单价
        /// </summary>
        public virtual decimal QuotaPrice
        {
            get { return _quotaPrice; }
            set { _quotaPrice = value; }
        }

        /// <summary>
        /// 定额金额
        /// </summary>
        public virtual decimal QuotaMoney
        {
            get { return _quotaMoney; }
            set { _quotaMoney = value; }
        }

        /// <summary>
        /// 损耗
        /// </summary>
        public virtual decimal Wastage
        {
            get { return _wastage; }
            set { _wastage = value; }
        }

        /// <summary>
        /// 定额工程量计量单位
        /// </summary>
        public virtual StandardUnit ProjectAmountUnitGUID
        {
            get { return _projectAmountUnitGUID; }
            set { _projectAmountUnitGUID = value; }
        }

        /// <summary>
        /// 定额工程量计量单位名称
        /// </summary>
        public virtual string ProjectAmountUnitName
        {
            get { return _projectAmountUnitName; }
            set { _projectAmountUnitName = value; }
        }

        /// <summary>
        /// 价格计量单位
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
        /// 数量责任组织
        /// </summary>
        public virtual string QuantityResponsibleOrgGUID
        {
            get { return _quantityResponsibleOrgGUID; }
            set { _quantityResponsibleOrgGUID = value; }
        }

        /// <summary>
        /// 数量责任组织名称
        /// </summary>
        public virtual string QuantityResponsibleOrgName
        {
            get { return _quantityResponsibleOrgName; }
            set { _quantityResponsibleOrgName = value; }
        }

        /// <summary>
        /// 单价责任组织GUID
        /// </summary>
        public virtual string PriceResponsibleOrgGUID
        {
            get { return _priceResponsibleOrgGUID; }
            set { _priceResponsibleOrgGUID = value; }
        }

        /// <summary>
        /// 单价责任组织名称
        /// </summary>
        public virtual string PriceResponsibleOrgName
        {
            get { return _priceResponsibleOrgName; }
            set { _priceResponsibleOrgName = value; }
        }

        /// <summary>
        /// 分摊比率
        /// </summary>
        public virtual decimal AssessmentRate
        {
            get { return _assessmentRate; }
            set { _assessmentRate = value; }
        }

        /// <summary>
        /// 所属成本项
        /// </summary>
        public virtual CostItem TheCostItem
        {
            get { return _theCostItem; }
            set { _theCostItem = value; }
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
        /// 成本核算科目
        /// </summary>
        public virtual CostAccountSubject CostAccountSubjectGUID
        {
            get { return _costAccountSubjectGUID; }
            set { _costAccountSubjectGUID = value; }
        }

        /// <summary>
        /// 成本核算科目名称
        /// </summary>
        public virtual string CostAccountSubjectName
        {
            get { return _costAccountSubjectName; }
            set { _costAccountSubjectName = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual SubjectCostQuotaState State
        {
            get { return _state; }
            set { _state = value; }
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
    }
    /// <summary>
    /// 分科目成本定额状态
    /// </summary>
    public enum SubjectCostQuotaState
    {
        [Description("编制")]
        编制 = 1,
        [Description("生效")]
        生效 = 2,
        [Description("冻结")]
        冻结 = 3,
        [Description("作废")]
        作废 = 4
    }
}
