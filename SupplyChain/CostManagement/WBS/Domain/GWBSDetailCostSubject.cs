using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using System.ComponentModel;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程资源耗用明细（工程任务明细分科目成本）
    /// </summary>
    [Serializable]
    public class GWBSDetailCostSubject
    {
        private string id;
        private long version;
        private string _name;

        private decimal _contractProjectAmount;
        private decimal _contractPrice;
        private decimal _contractQuotaQuantity;
        private decimal _contractQuantityPrice;
        private decimal _contractTotalPrice;

        private decimal _responsibilitilyWorkAmount;
        private decimal _responsibilitilyPrice;
        private decimal _responsibilitilyTotalPrice;
        private decimal _responsibleQuotaNum;
        private decimal _responsibleWorkPrice;

        private decimal _planWorkAmount;
        private decimal _planPrice;
        private decimal _planTotalPrice;
        private decimal _planQuotaNum;
        private decimal _planWorkPrice;

        private StandardUnit _priceUnitGUID;
        private string _priceUnitName;

        private StandardUnit _projectAmountUnitGUID;
        private string _projectAmountUnitName;

        private decimal _assessmentRate;

        private string _resourceTypeGUID;
        private string _resourceTypeCode;
        private string _resourceTypeName;
        private string _resourceTypeSpec;
        private string _resourceTypeQuality;
        private string _resourceCateSyscode;

        private CostAccountSubject _costAccountSubjectGUID;
        private string _costAccountSubjectName;
        private string _costAccountSubjectSyscode;

        private GWBSDetailCostSubjectState _state = GWBSDetailCostSubjectState.编制;
        private DateTime _createTime = DateTime.Now;
        private DateTime _updateTime = DateTime.Now;

        private GWBSDetail _theGWBSDetail;

        private string _theProjectGUID;
        private string _theProjectName;

        private GWBSTree theGWBSTree;
        private string theGWBSTreeName;
        private string theGWBSTreeSyscode;

        private bool _mainResTypeFlag;
        private bool _isCategoryResource = false;
        private SubjectCostQuota _resourceUsageQuota;

        private string diagramNumber;

        private string technicalParam;

        private decimal _contractBasePrice;
        private decimal _responsibleBasePrice;
        private decimal _planBasePrice;

        private decimal _contractPricePercent = 1;
        private decimal _responsiblePricePercent = 1;
        private decimal _planPricePercent = 1;

        /// <summary>
        /// 合同基准数量单价
        /// </summary>
        public virtual decimal ContractBasePrice
        {
            get { return _contractBasePrice; }
            set { _contractBasePrice = value; }
        }
        /// <summary>
        /// 责任基准数量单价
        /// </summary>
        public virtual decimal ResponsibleBasePrice
        {
            get { return _responsibleBasePrice; }
            set { _responsibleBasePrice = value; }
        }
        /// <summary>
        /// 计划基准数量单价
        /// </summary>
        public virtual decimal PlanBasePrice
        {
            get { return _planBasePrice; }
            set { _planBasePrice = value; }
        }

        /// <summary>
        /// 合同数量单价调整系数（在成本预算批量调整系数时使用）
        /// </summary>
        public virtual decimal ContractPricePercent
        {
            get { return _contractPricePercent; }
            set { _contractPricePercent = value; }
        }
        /// <summary>
        /// 责任数量单价调整系数（在成本预算批量调整系数时使用）
        /// </summary>
        public virtual decimal ResponsiblePricePercent
        {
            get { return _responsiblePricePercent; }
            set { _responsiblePricePercent = value; }
        }
        /// <summary>
        /// 计划数量单价调整系数（在成本预算批量调整系数时使用）
        /// </summary>
        public virtual decimal PlanPricePercent
        {
            get { return _planPricePercent; }
            set { _planPricePercent = value; }
        }



        /// <summary>
        /// 技术参数
        /// </summary>
        public virtual string TechnicalParam
        {
            get { return technicalParam; }
            set { technicalParam = value; }
        }

        /// <summary>
        /// 图号
        /// </summary>
        public virtual string DiagramNumber
        {
            get { return diagramNumber; }
            set { diagramNumber = value; }
        }
        /// <summary>
        /// 主资源标志
        /// </summary>
        public virtual bool MainResTypeFlag
        {
            get { return _mainResTypeFlag; }
            set { _mainResTypeFlag = value; }
        }
        /// <summary>
        /// 责任定额数量
        /// </summary>
        public virtual decimal ResponsibleQuotaNum
        {
            get { return _responsibleQuotaNum; }
            set { _responsibleQuotaNum = value; }
        }
        /// <summary>
        /// 责任工程量单价
        /// </summary>
        public virtual decimal ResponsibleWorkPrice
        {
            get { return _responsibleWorkPrice; }
            set { _responsibleWorkPrice = value; }
        }
        /// <summary>
        /// 计划定额数量
        /// </summary>
        public virtual decimal PlanQuotaNum
        {
            get { return _planQuotaNum; }
            set { _planQuotaNum = value; }
        }
        /// <summary>
        /// 计划工程量单价
        /// </summary>
        public virtual decimal PlanWorkPrice
        {
            get { return _planWorkPrice; }
            set { _planWorkPrice = value; }
        }
        /// <summary>
        /// 是否是分类资源
        /// </summary>
        public virtual bool IsCategoryResource
        {
            get { return _isCategoryResource; }
            set { _isCategoryResource = value; }
        }
        /// <summary>
        /// 资源耗用定额
        /// </summary>
        public virtual SubjectCostQuota ResourceUsageQuota
        {
            get { return _resourceUsageQuota; }
            set { _resourceUsageQuota = value; }
        }


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
        /// 耗用名称
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 资源合同工程(耗用)量
        /// </summary>
        public virtual decimal ContractProjectAmount
        {
            get { return _contractProjectAmount; }
            set { _contractProjectAmount = value; }
        }
        /// <summary>
        /// 资源合同工程量单价
        /// </summary>
        public virtual decimal ContractPrice
        {
            get { return _contractPrice; }
            set { _contractPrice = value; }
        }
        /// <summary>
        /// 资源合同定额数量
        /// </summary>
        public virtual decimal ContractQuotaQuantity
        {
            get { return _contractQuotaQuantity; }
            set { _contractQuotaQuantity = value; }
        }
        /// <summary>
        /// 合同数量单价
        /// </summary>
        public virtual decimal ContractQuantityPrice
        {
            get { return _contractQuantityPrice; }
            set { _contractQuantityPrice = value; }
        }
        /// <summary>
        /// 资源合同收入合价
        /// </summary>
        public virtual decimal ContractTotalPrice
        {
            get { return _contractTotalPrice; }
            set { _contractTotalPrice = value; }
        }
        /// <summary>
        /// 责任耗用数量
        /// </summary>
        public virtual decimal ResponsibilitilyWorkAmount
        {
            get { return _responsibilitilyWorkAmount; }
            set { _responsibilitilyWorkAmount = value; }
        }
        /// <summary>
        /// 责任数量单价
        /// </summary>
        public virtual decimal ResponsibilitilyPrice
        {
            get { return _responsibilitilyPrice; }
            set { _responsibilitilyPrice = value; }
        }
        /// <summary>
        /// 责任耗用合价
        /// </summary>
        public virtual decimal ResponsibilitilyTotalPrice
        {
            get { return _responsibilitilyTotalPrice; }
            set { _responsibilitilyTotalPrice = value; }
        }
        /// <summary>
        /// 计划耗用数量
        /// </summary>
        public virtual decimal PlanWorkAmount
        {
            get { return _planWorkAmount; }
            set { _planWorkAmount = value; }
        }
        /// <summary>
        /// 计划数量单价
        /// </summary>
        public virtual decimal PlanPrice
        {
            get { return _planPrice; }
            set { _planPrice = value; }
        }
        /// <summary>
        /// 计划耗用合价
        /// </summary>
        public virtual decimal PlanTotalPrice
        {
            get { return _planTotalPrice; }
            set { _planTotalPrice = value; }
        }
        /// <summary>
        /// 数量计量单位
        /// </summary>
        public virtual StandardUnit ProjectAmountUnitGUID
        {
            get { return _projectAmountUnitGUID; }
            set { _projectAmountUnitGUID = value; }
        }
        /// <summary>
        /// 数量计量单位名称
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
        /// 分摊比率
        /// </summary>
        public virtual decimal AssessmentRate
        {
            get { return _assessmentRate; }
            set { _assessmentRate = value; }
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
        /// 资源类型编码
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
        /// 资源类型规格
        /// </summary>
        public virtual string ResourceTypeSpec
        {
            get { return _resourceTypeSpec; }
            set { _resourceTypeSpec = value; }
        }
        /// <summary>
        /// 资源类型材质
        /// </summary>
        public virtual string ResourceTypeQuality
        {
            get { return _resourceTypeQuality; }
            set { _resourceTypeQuality = value; }
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
        /// 成本核算科目层次码
        /// </summary>
        public virtual string CostAccountSubjectSyscode
        {
            get { return _costAccountSubjectSyscode; }
            set { _costAccountSubjectSyscode = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual GWBSDetailCostSubjectState State
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
        /// 工程任务明细
        /// </summary>
        public virtual GWBSDetail TheGWBSDetail
        {
            get { return _theGWBSDetail; }
            set { _theGWBSDetail = value; }
        }

        /// <summary>
        /// 所属工程项目任务
        /// </summary>
        public virtual GWBSTree TheGWBSTree
        {
            get { return theGWBSTree; }
            set { theGWBSTree = value; }
        }
        /// <summary>
        /// 所属工程项目任务名称
        /// </summary>
        public virtual string TheGWBSTreeName
        {
            get { return theGWBSTreeName; }
            set { theGWBSTreeName = value; }
        }
        /// <summary>
        /// 所属工程项目任务层次码
        /// </summary>
        public virtual string TheGWBSTreeSyscode
        {
            get { return theGWBSTreeSyscode; }
            set { theGWBSTreeSyscode = value; }
        }

        private ISet<GWBSDtlCostSubRate> _listGWBSDtlCostSubRate = new HashedSet<GWBSDtlCostSubRate>();
        ///<summary>非实体成本取费费率</summary>
        public virtual ISet<GWBSDtlCostSubRate> ListGWBSDtlCostSubRate
        {
            set { this._listGWBSDtlCostSubRate = value; }
            get { return this._listGWBSDtlCostSubRate; }
        }


        //移除的属性
        private decimal _addupAccountProjectAmount;
        private decimal _addupAccountCost;
        private DateTime? _addupAccountCostEndTime;
        private decimal _currentPeriodAccountProjectAmount;
        private decimal _currentPeriodAccountCost;
        private DateTime? _currentPeriodAccountCostEndTime;
        private decimal _projectAmountWasta;
        private decimal _addupBalanceProjectAmount;
        private decimal _currentPeriodBalanceProjectAmount;
        private decimal _currentPeriodBalanceTotalPrice;
        private decimal _addupBalanceTotalPrice;

        /// <summary>
        /// 累计核算工程量
        /// </summary>
        public virtual decimal AddupAccountProjectAmount
        {
            get { return _addupAccountProjectAmount; }
            set { _addupAccountProjectAmount = value; }
        }
        /// <summary>
        /// 累计核算成本
        /// </summary>
        public virtual decimal AddupAccountCost
        {
            get { return _addupAccountCost; }
            set { _addupAccountCost = value; }
        }
        /// <summary>
        /// 累计核算成本截止时间
        /// </summary>
        public virtual DateTime? AddupAccountCostEndTime
        {
            get { return _addupAccountCostEndTime; }
            set { _addupAccountCostEndTime = value; }
        }
        /// <summary>
        /// 当期核算工程量
        /// </summary>
        public virtual decimal CurrentPeriodAccountProjectAmount
        {
            get { return _currentPeriodAccountProjectAmount; }
            set { _currentPeriodAccountProjectAmount = value; }
        }
        /// <summary>
        /// 当期核算成本
        /// </summary>
        public virtual decimal CurrentPeriodAccountCost
        {
            get { return _currentPeriodAccountCost; }
            set { _currentPeriodAccountCost = value; }
        }
        /// <summary>
        /// 当期核算成本截止时间
        /// </summary>
        public virtual DateTime? CurrentPeriodAccountCostEndTime
        {
            get { return _currentPeriodAccountCostEndTime; }
            set { _currentPeriodAccountCostEndTime = value; }
        }
        /// <summary>
        /// 工程量损耗
        /// </summary>
        public virtual decimal ProjectAmountWasta
        {
            get { return _projectAmountWasta; }
            set { _projectAmountWasta = value; }
        }
        /// <summary>
        /// 累计结算工程量
        /// </summary>
        public virtual decimal AddupBalanceProjectAmount
        {
            get { return _addupBalanceProjectAmount; }
            set { _addupBalanceProjectAmount = value; }
        }
        /// <summary>
        /// 当期结算工程量
        /// </summary>
        public virtual decimal CurrentPeriodBalanceProjectAmount
        {
            get { return _currentPeriodBalanceProjectAmount; }
            set { _currentPeriodBalanceProjectAmount = value; }
        }
        /// <summary>
        /// 当期结算合价
        /// </summary>
        public virtual decimal CurrentPeriodBalanceTotalPrice
        {
            get { return _currentPeriodBalanceTotalPrice; }
            set { _currentPeriodBalanceTotalPrice = value; }
        }
        /// <summary>
        /// 累计结算合价
        /// </summary>
        public virtual decimal AddupBalanceTotalPrice
        {
            get { return _addupBalanceTotalPrice; }
            set { _addupBalanceTotalPrice = value; }
        }

        /// <summary>
        /// 取费基准单价（人工+机械费单价）
        /// </summary>
        public virtual decimal LaborMachineBasePrice { get; set; }
        /// <summary>
        /// 非取费基准单价（材料费）
        /// </summary>
        public virtual decimal MaterialBasePrice { get; set; }
        /// <summary>
        /// 专业类型
        /// </summary>
        public virtual string ProfessionalType { get; set; }
        /// <summary>
        /// 前驱资源耗用ID
        /// </summary>
        public virtual string ForwardCostSubjectId { get; set; }
        public virtual GWBSDetailCostSubject Clone(GWBSDetail oGWBSDetail)
        {
            GWBSDetailCostSubject oTempCostSubject = this.MemberwiseClone() as GWBSDetailCostSubject;
            oTempCostSubject.id = null;
            oTempCostSubject.AddupAccountCost = 0;
            oTempCostSubject.AddupAccountProjectAmount = 0;
            oTempCostSubject.AddupBalanceProjectAmount = 0;
            oTempCostSubject.AddupBalanceTotalPrice = 0;
            oTempCostSubject.ContractProjectAmount = 0;
            oTempCostSubject.ContractQuotaQuantity = 0;
            oTempCostSubject.ContractTotalPrice = 0;
            oTempCostSubject.CurrentPeriodAccountCost = 0;
            oTempCostSubject.CurrentPeriodAccountProjectAmount = 0;
            oTempCostSubject.CurrentPeriodBalanceProjectAmount = 0;
            oTempCostSubject.CurrentPeriodBalanceTotalPrice = 0;
            oTempCostSubject.PlanTotalPrice = 0;
            oTempCostSubject.PlanWorkAmount = 0;
            oTempCostSubject.ProjectAmountWasta = 0;
            oTempCostSubject.ResponsibilitilyTotalPrice = 0;
            oTempCostSubject.ResponsibilitilyWorkAmount = 0;
            oTempCostSubject.ResponsibleQuotaNum = 0;
            //oTempCostSubject = this.MemberwiseClone() as GWBSDetailCostSubject;
            oTempCostSubject.PriceUnitGUID = this.PriceUnitGUID;
            oTempCostSubject.ProjectAmountUnitGUID = this.ProjectAmountUnitGUID;
            oTempCostSubject.CostAccountSubjectGUID = this.CostAccountSubjectGUID;
            oTempCostSubject.TheGWBSDetail = oGWBSDetail;
            oTempCostSubject.TheGWBSTree = this.TheGWBSTree;
            oTempCostSubject.ResourceUsageQuota = this.ResourceUsageQuota;
            oTempCostSubject.Version = 0;
            oTempCostSubject.ListGWBSDtlCostSubRate = new HashedSet<GWBSDtlCostSubRate>();
            foreach (GWBSDtlCostSubRate rate in this.ListGWBSDtlCostSubRate)
            {
                oTempCostSubject.ListGWBSDtlCostSubRate.Add(rate.Clone(oTempCostSubject));
            }

            return oTempCostSubject;
        }

        public virtual GWBSDetailCostSubject CloneByRate(GWBSTree theGWBSTree, GWBSDetail oGWBSDetail, decimal rate)
        {
            GWBSDetailCostSubject oTempCostSubject = this.MemberwiseClone() as GWBSDetailCostSubject;
            oTempCostSubject.ForwardCostSubjectId = this.Id;
            oTempCostSubject.Id = null;
            oTempCostSubject.AddupAccountCost = 0;
            oTempCostSubject.AddupAccountProjectAmount = 0;
            oTempCostSubject.AddupBalanceProjectAmount = 0;
            oTempCostSubject.AddupBalanceTotalPrice = 0;
            oTempCostSubject.ContractProjectAmount *= rate;// 0;
            oTempCostSubject.ContractQuotaQuantity *= rate;// 0;
            oTempCostSubject.ContractTotalPrice *= rate;// 0;
            oTempCostSubject.CurrentPeriodAccountCost = 0;// *= rate;// 0;
            oTempCostSubject.CurrentPeriodAccountProjectAmount = 0;// *= rate;// 0;
            oTempCostSubject.CurrentPeriodBalanceProjectAmount = 0;// *= rate;// 0;
            oTempCostSubject.CurrentPeriodBalanceTotalPrice = 0;// *= rate;// 0;
            oTempCostSubject.PlanTotalPrice *= rate;//0;
            oTempCostSubject.PlanWorkAmount *= rate;//0;
            //oTempCostSubject.PlanQuotaNum *= rate;//0;
            oTempCostSubject.ProjectAmountWasta = 0;// *= rate;// 0;
            oTempCostSubject.ResponsibilitilyTotalPrice *= rate;//0;
            oTempCostSubject.ResponsibilitilyWorkAmount *= rate;//0;
            //oTempCostSubject.ResponsibleQuotaNum *= rate;//0;
            oTempCostSubject.PriceUnitGUID = this.PriceUnitGUID;
            oTempCostSubject.ProjectAmountUnitGUID = this.ProjectAmountUnitGUID;
            oTempCostSubject.CostAccountSubjectGUID = this.CostAccountSubjectGUID;
            oTempCostSubject.TheGWBSDetail = oGWBSDetail;
            oTempCostSubject.TheGWBSTree = theGWBSTree;
            oTempCostSubject.ResourceUsageQuota = this.ResourceUsageQuota;
            oTempCostSubject.Version = 0;
            oTempCostSubject.ListGWBSDtlCostSubRate = new HashedSet<GWBSDtlCostSubRate>();
            foreach (GWBSDtlCostSubRate objRate in this.ListGWBSDtlCostSubRate)
            {
                oTempCostSubject.ListGWBSDtlCostSubRate.Add(objRate.Clone(oTempCostSubject));
            }

            return oTempCostSubject;
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
