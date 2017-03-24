using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using System.ComponentModel;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程任务明细分科目成本
    /// </summary>
    [Serializable]
    public class GWBSDetailCostSubject
    {
        private string id;
        private long version;
        private string _name;

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
        /// 成本名称
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private decimal _contractProjectAmount;
        /// <summary>
        /// 合同工程量
        /// </summary>
        public virtual decimal ContractProjectAmount
        {
            get { return _contractProjectAmount; }
            set { _contractProjectAmount = value; }
        }
        private decimal _contractPrice;
        /// <summary>
        /// 合同单价
        /// </summary>
        public virtual decimal ContractPrice
        {
            get { return _contractPrice; }
            set { _contractPrice = value; }
        }
        private decimal _contractTotalPrice;
        /// <summary>
        /// 合同收入合价
        /// </summary>
        public virtual decimal ContractTotalPrice
        {
            get { return _contractTotalPrice; }
            set { _contractTotalPrice = value; }
        }

        private decimal _responsibilitilyWorkAmount;
        /// <summary>
        /// 责任工程量
        /// </summary>
        public virtual decimal ResponsibilitilyWorkAmount
        {
            get { return _responsibilitilyWorkAmount; }
            set { _responsibilitilyWorkAmount = value; }
        }
        private decimal _responsibilitilyPrice;
        /// <summary>
        /// 责任单价
        /// </summary>
        public virtual decimal ResponsibilitilyPrice
        {
            get { return _responsibilitilyPrice; }
            set { _responsibilitilyPrice = value; }
        }
        private decimal _responsibilitilyTotalPrice;
        /// <summary>
        /// 责任合价
        /// </summary>
        public virtual decimal ResponsibilitilyTotalPrice
        {
            get { return _responsibilitilyTotalPrice; }
            set { _responsibilitilyTotalPrice = value; }
        }

        private decimal _planWorkAmount;
        private decimal _planPrice;
        private decimal _planTotalPrice;
        /// <summary>
        /// 计划工程量
        /// </summary>
        public virtual decimal PlanWorkAmount
        {
            get { return _planWorkAmount; }
            set { _planWorkAmount = value; }
        }
        /// <summary>
        /// 计划单价
        /// </summary>
        public virtual decimal PlanPrice
        {
            get { return _planPrice; }
            set { _planPrice = value; }
        }
        /// <summary>
        /// 计划合价
        /// </summary>
        public virtual decimal PlanTotalPrice
        {
            get { return _planTotalPrice; }
            set { _planTotalPrice = value; }
        }

        private decimal _addupAccountProjectAmount;
        /// <summary>
        /// 累计核算工程量
        /// </summary>
        public virtual decimal AddupAccountProjectAmount
        {
            get { return _addupAccountProjectAmount; }
            set { _addupAccountProjectAmount = value; }
        }
        private decimal _addupAccountCost;
        /// <summary>
        /// 累计核算成本
        /// </summary>
        public virtual decimal AddupAccountCost
        {
            get { return _addupAccountCost; }
            set { _addupAccountCost = value; }
        }
        private DateTime? _addupAccountCostEndTime;
        /// <summary>
        /// 累计核算成本截止时间
        /// </summary>
        public virtual DateTime? AddupAccountCostEndTime
        {
            get { return _addupAccountCostEndTime; }
            set { _addupAccountCostEndTime = value; }
        }

        private decimal _currentPeriodAccountProjectAmount;
        /// <summary>
        /// 当期核算工程量
        /// </summary>
        public virtual decimal CurrentPeriodAccountProjectAmount
        {
            get { return _currentPeriodAccountProjectAmount; }
            set { _currentPeriodAccountProjectAmount = value; }
        }
        private decimal _currentPeriodAccountCost;
        /// <summary>
        /// 当期核算成本
        /// </summary>
        public virtual decimal CurrentPeriodAccountCost
        {
            get { return _currentPeriodAccountCost; }
            set { _currentPeriodAccountCost = value; }
        }
        private DateTime? _currentPeriodAccountCostEndTime;
        /// <summary>
        /// 当期核算成本截止时间
        /// </summary>
        public virtual DateTime? CurrentPeriodAccountCostEndTime
        {
            get { return _currentPeriodAccountCostEndTime; }
            set { _currentPeriodAccountCostEndTime = value; }
        }

        private decimal _projectAmountWasta;
        /// <summary>
        /// 工程量损耗
        /// </summary>
        public virtual decimal ProjectAmountWasta
        {
            get { return _projectAmountWasta; }
            set { _projectAmountWasta = value; }
        }
        private decimal _addupBalanceProjectAmount;
        /// <summary>
        /// 累计结算工程量
        /// </summary>
        public virtual decimal AddupBalanceProjectAmount
        {
            get { return _addupBalanceProjectAmount; }
            set { _addupBalanceProjectAmount = value; }
        }
        private decimal _currentPeriodBalanceProjectAmount;
        /// <summary>
        /// 当期结算工程量
        /// </summary>
        public virtual decimal CurrentPeriodBalanceProjectAmount
        {
            get { return _currentPeriodBalanceProjectAmount; }
            set { _currentPeriodBalanceProjectAmount = value; }
        }
        private decimal _currentPeriodBalanceTotalPrice;
        /// <summary>
        /// 当期结算合价
        /// </summary>
        public virtual decimal CurrentPeriodBalanceTotalPrice
        {
            get { return _currentPeriodBalanceTotalPrice; }
            set { _currentPeriodBalanceTotalPrice = value; }
        }
        private decimal _addupBalanceTotalPrice;
        /// <summary>
        /// 累计结算合价
        /// </summary>
        public virtual decimal AddupBalanceTotalPrice
        {
            get { return _addupBalanceTotalPrice; }
            set { _addupBalanceTotalPrice = value; }
        }


        private StandardUnit _projectAmountUnitGUID;
        /// <summary>
        /// 定额工程量计量单位
        /// </summary>
        public virtual StandardUnit ProjectAmountUnitGUID
        {
            get { return _projectAmountUnitGUID; }
            set { _projectAmountUnitGUID = value; }
        }
        private string _projectAmountUnitName;
        /// <summary>
        /// 定额工程量计量单位名称
        /// </summary>
        public virtual string ProjectAmountUnitName
        {
            get { return _projectAmountUnitName; }
            set { _projectAmountUnitName = value; }
        }
        private StandardUnit _priceUnitGUID;
        /// <summary>
        /// 价格计量单位
        /// </summary>
        public virtual StandardUnit PriceUnitGUID
        {
            get { return _priceUnitGUID; }
            set { _priceUnitGUID = value; }
        }
        private string _priceUnitName;
        /// <summary>
        /// 价格计量单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return _priceUnitName; }
            set { _priceUnitName = value; }
        }

        private decimal _assessmentRate;
        /// <summary>
        /// 分摊比率
        /// </summary>
        public virtual decimal AssessmentRate
        {
            get { return _assessmentRate; }
            set { _assessmentRate = value; }
        }

        private GWBSDetail _theGWBSDetail;
        /// <summary>
        /// 工程任务明细
        /// </summary>
        public virtual GWBSDetail TheGWBSDetail
        {
            get { return _theGWBSDetail; }
            set { _theGWBSDetail = value; }
        }
        private string _resourceTypeGUID;
        /// <summary>
        /// 资源类型GUID
        /// </summary>
        public virtual string ResourceTypeGUID
        {
            get { return _resourceTypeGUID; }
            set { _resourceTypeGUID = value; }
        }
        private string _resourceTypeName;
        /// <summary>
        /// 资源类型名称
        /// </summary>
        public virtual string ResourceTypeName
        {
            get { return _resourceTypeName; }
            set { _resourceTypeName = value; }
        }
        private CostAccountSubject _costAccountSubjectGUID;
        /// <summary>
        /// 成本核算科目
        /// </summary>
        public virtual CostAccountSubject CostAccountSubjectGUID
        {
            get { return _costAccountSubjectGUID; }
            set { _costAccountSubjectGUID = value; }
        }
        private string _costAccountSubjectName;
        /// <summary>
        /// 成本核算科目名称
        /// </summary>
        public virtual string CostAccountSubjectName
        {
            get { return _costAccountSubjectName; }
            set { _costAccountSubjectName = value; }
        }
        private GWBSDetailCostSubjectState _state = GWBSDetailCostSubjectState.编制;
        /// <summary>
        /// 状态
        /// </summary>
        public virtual GWBSDetailCostSubjectState State
        {
            get { return _state; }
            set { _state = value; }
        }

        private DateTime _createTime = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }


        private string _theProjectGUID;
        private string _theProjectName;
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
    /// 工程任务明细分科目成本状态
    /// </summary>
    public enum GWBSDetailCostSubjectState
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
