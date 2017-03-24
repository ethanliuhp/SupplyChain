using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using System.ComponentModel;
using Iesi.Collections;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程WBS明细
    /// </summary>
    [Serializable]
    public class GWBSDetail
    {
        private string id;
        private long version;

        private string _code;
        private string _name;
        private CostItem _theCostItem;
        //private string _costItemName;
        private string _summary;
        private string _contentDesc;
        private string _projectTaskTypeCode;
        private SupplierRelationInfo _bearOrgGUID;
        private string _bearOrgName;
        private decimal _responsibilitilyWorkAmount;
        private decimal _planWorkAmount;

        private decimal _finishedWorkAmount;
        private decimal _taskFinishedPercent;
        private decimal _accountWorkAmount;
        private decimal _subcontractBalanceWorkAmount;
        private decimal _completionBalanceWorkAmount;
        private StandardUnit _workAmountUnitGUID;
        private string _workAmountUnitName;
        private decimal _responsibilitilyPrice;
        private decimal _responsibilitilyTotalPrice;
        private decimal _planPrice;

        private decimal _planTotalPrice;
        private decimal _accountPrice;
        private decimal _accountTotalPrice;
        private decimal _subcontractBalancePrice;
        private decimal _subcontractBalanceTotalPrice;
        private decimal _completionBalancePrice;
        private decimal _completionBalanceTotalPrice;
        private StandardUnit _priceUnitGUID;
        private string _priceUnitName;
        private decimal _contractProjectQuantity;

        private decimal _contractPrice;
        private decimal _contractTotalPrice;
        private string _detailExecuteDesc;
        private string _contractGroupType;
        private GWBSDetailState _state;
        private DateTime? _currentStateTime;
        private GWBSTree _theGWBS;
        private string _contractGroupGUID;
        private string _contractGroupCode;
        private string _workPart;

        private string _workUseMaterial;
        private string _workMethod;

        private string _theProjectGUID;
        private string _theProjectName;
        private DateTime _createTime = DateTime.Now;
        private DateTime _updatedDate = DateTime.Now;

        private WeekScheduleDetail weekScheduleDetail;
        private int costingFlag = 0;
        private int produceConfirmFlag = 0;
        private int responseFlag = 0;
        private decimal progressConfirmed;
        private decimal quantityConfirmed;


        private string _NGUID;
        private decimal _addupAccQuantity;
        private decimal _addupAccFigureProgress;
        private decimal _subContractStepRate;
        private bool _subContractFeeFlag;
        private string _theGWBSSysCode;


        /// <summary>
        /// 名称GUID
        /// </summary>
        public virtual string NGUID
        {
            get { return _NGUID; }
            set { _NGUID = value; }
        }
        /// <summary>
        /// 累计核算工程量
        /// </summary>
        public virtual decimal AddupAccQuantity
        {
            get { return _addupAccQuantity; }
            set { _addupAccQuantity = value; }
        }
        /// <summary>
        /// 累计核算形象进度
        /// </summary>
        public virtual decimal AddupAccFigureProgress
        {
            get { return _addupAccFigureProgress; }
            set { _addupAccFigureProgress = value; }
        }
        /// <summary>
        /// 分包措施费率
        /// </summary>
        public virtual decimal SubContractStepRate
        {
            get { return _subContractStepRate; }
            set { _subContractStepRate = value; }
        }
        /// <summary>
        /// 分包取费标志
        /// </summary>
        public virtual bool SubContractFeeFlag
        {
            get { return _subContractFeeFlag; }
            set { _subContractFeeFlag = value; }
        }
        /// <summary>
        /// 所属GWBS节点层次码
        /// </summary>
        public virtual string TheGWBSSysCode
        {
            get { return _theGWBSSysCode; }
            set { _theGWBSSysCode = value; }
        }




        /// <summary>
        /// 累计工长确认工程量
        /// </summary>
        public virtual decimal QuantityConfirmed
        {
            get { return quantityConfirmed; }
            set { quantityConfirmed = value; }
        }

        /// <summary>
        /// 累积工长确认形象进度
        /// </summary>
        public virtual decimal ProgressConfirmed
        {
            get { return progressConfirmed; }
            set { progressConfirmed = value; }
        }

        /// <summary>
        /// 责任核算标志
        /// </summary>
        public virtual int ResponseFlag
        {
            get { return responseFlag; }
            set { responseFlag = value; }
        }

        /// <summary>
        /// 生产确认标志
        /// </summary>
        public virtual int ProduceConfirmFlag
        {
            get { return produceConfirmFlag; }
            set { produceConfirmFlag = value; }
        }

        /// <summary>
        /// 成本核算标志
        /// </summary>
        public virtual int CostingFlag
        {
            get { return costingFlag; }
            set { costingFlag = value; }
        }

        /// <summary>
        /// 执行进度计划明细
        /// 在工程确认单引用工程任务明细时 用来临时存放执行进度计划明细
        /// </summary>
        public virtual WeekScheduleDetail WeekScheduleDetail
        {
            get { return weekScheduleDetail; }
            set { weekScheduleDetail = value; }
        }

        /// <summary>
        /// 明细编码
        /// </summary>
        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        /// <summary>
        /// 任务明细名称
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 成本项
        /// </summary>
        public virtual CostItem TheCostItem
        {
            get { return _theCostItem; }
            set { _theCostItem = value; }
        }
        /// <summary>
        /// 成本项名称
        /// </summary>
        //public virtual string CostItemName
        //{
        //    get { return _costItemName; }
        //    set { _costItemName = value; }
        //}
        /// <summary>
        /// 摘要
        /// </summary>
        public virtual string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }
        /// <summary>
        /// 明细内容说明
        /// </summary>
        public virtual string ContentDesc
        {
            get { return _contentDesc; }
            set { _contentDesc = value; }
        }
        /// <summary>
        /// 工程任务类型代码
        /// </summary>
        public virtual string ProjectTaskTypeCode
        {
            get { return _projectTaskTypeCode; }
            set { _projectTaskTypeCode = value; }
        }
        /// <summary>
        /// 承担组织
        /// </summary>
        public virtual SupplierRelationInfo BearOrgGUID
        {
            get { return _bearOrgGUID; }
            set { _bearOrgGUID = value; }
        }
        /// <summary>
        /// 承担组织名称
        /// </summary>
        public virtual string BearOrgName
        {
            get { return _bearOrgName; }
            set { _bearOrgName = value; }
        }
        /// <summary>
        /// 责任工程量
        /// </summary>
        public virtual decimal ResponsibilitilyWorkAmount
        {
            get { return _responsibilitilyWorkAmount; }
            set { _responsibilitilyWorkAmount = value; }
        }
        /// <summary>
        /// 计划工程量
        /// </summary>
        public virtual decimal PlanWorkAmount
        {
            get { return _planWorkAmount; }
            set { _planWorkAmount = value; }
        }
        /// <summary>
        /// 完工工程量
        /// </summary>
        public virtual decimal FinishedWorkAmount
        {
            get { return _finishedWorkAmount; }
            set { _finishedWorkAmount = value; }
        }
        /// <summary>
        /// 任务完成百分比
        /// </summary>
        public virtual decimal TaskFinishedPercent
        {
            get { return _taskFinishedPercent; }
            set { _taskFinishedPercent = value; }
        }
        /// <summary>
        /// 核算工程量
        /// </summary>
        //public virtual decimal AccountWorkAmount
        //{
        //    get { return _accountWorkAmount; }
        //    set { _accountWorkAmount = value; }
        //}
        /// <summary>
        /// 分包结算工程量
        /// </summary>
        //public virtual decimal SubcontractBalanceWorkAmount
        //{
        //    get { return _subcontractBalanceWorkAmount; }
        //    set { _subcontractBalanceWorkAmount = value; }
        //}
        /// <summary>
        /// 竣工结算工程量
        /// </summary>
        //public virtual decimal CompletionBalanceWorkAmount
        //{
        //    get { return _completionBalanceWorkAmount; }
        //    set { _completionBalanceWorkAmount = value; }
        //}
        /// <summary>
        /// 工程量计量单位
        /// </summary>
        public virtual StandardUnit WorkAmountUnitGUID
        {
            get { return _workAmountUnitGUID; }
            set { _workAmountUnitGUID = value; }
        }
        /// <summary>
        /// 工程量计量单位名称
        /// </summary>
        public virtual string WorkAmountUnitName
        {
            get { return _workAmountUnitName; }
            set { _workAmountUnitName = value; }
        }
        /// <summary>
        /// 责任单价
        /// </summary>
        public virtual decimal ResponsibilitilyPrice
        {
            get { return _responsibilitilyPrice; }
            set { _responsibilitilyPrice = value; }
        }
        /// <summary>
        /// 责任合价
        /// </summary>
        public virtual decimal ResponsibilitilyTotalPrice
        {
            get { return _responsibilitilyTotalPrice; }
            set { _responsibilitilyTotalPrice = value; }
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
        /// <summary>
        /// 核算单价
        /// </summary>
        //public virtual decimal AccountPrice
        //{
        //    get { return _accountPrice; }
        //    set { _accountPrice = value; }
        //}
        /// <summary>
        /// 核算合价
        /// </summary>
        //public virtual decimal AccountTotalPrice
        //{
        //    get { return _accountTotalPrice; }
        //    set { _accountTotalPrice = value; }
        //}
        /// <summary>
        /// 分包结算单价
        /// </summary>
        //public virtual decimal SubcontractBalancePrice
        //{
        //    get { return _subcontractBalancePrice; }
        //    set { _subcontractBalancePrice = value; }
        //}
        /// <summary>
        /// 分包结算合价
        /// </summary>
        //public virtual decimal SubcontractBalanceTotalPrice
        //{
        //    get { return _subcontractBalanceTotalPrice; }
        //    set { _subcontractBalanceTotalPrice = value; }
        //}
        /// <summary>
        /// 竣工结算单价
        /// </summary>
        //public virtual decimal CompletionBalancePrice
        //{
        //    get { return _completionBalancePrice; }
        //    set { _completionBalancePrice = value; }
        //}
        /// <summary>
        /// 竣工结算合价
        /// </summary>
        //public virtual decimal CompletionBalanceTotalPrice
        //{
        //    get { return _completionBalanceTotalPrice; }
        //    set { _completionBalanceTotalPrice = value; }
        //}
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
        /// 合同工程量
        /// </summary>
        public virtual decimal ContractProjectQuantity
        {
            get { return _contractProjectQuantity; }
            set { _contractProjectQuantity = value; }
        }
        /// <summary>
        /// 合同单价
        /// </summary>
        public virtual decimal ContractPrice
        {
            get { return _contractPrice; }
            set { _contractPrice = value; }
        }
        /// <summary>
        /// 合同合价
        /// </summary>
        public virtual decimal ContractTotalPrice
        {
            get { return _contractTotalPrice; }
            set { _contractTotalPrice = value; }
        }
        /// <summary>
        /// 明细执行说明
        /// </summary>
        public virtual string DetailExecuteDesc
        {
            get { return _detailExecuteDesc; }
            set { _detailExecuteDesc = value; }
        }
        /// <summary>
        /// 契约组类型
        /// </summary>
        public virtual string ContractGroupType
        {
            get { return _contractGroupType; }
            set { _contractGroupType = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual GWBSDetailState State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// 当前状态时间
        /// </summary>
        public virtual DateTime? CurrentStateTime
        {
            get { return _currentStateTime; }
            set { _currentStateTime = value; }
        }
        /// <summary>
        /// 所属工程项目任务
        /// </summary>
        public virtual GWBSTree TheGWBS
        {
            get { return _theGWBS; }
            set { _theGWBS = value; }
        }
        /// <summary>
        /// 契约组
        /// </summary>
        public virtual string ContractGroupGUID
        {
            get { return _contractGroupGUID; }
            set { _contractGroupGUID = value; }
        }
        /// <summary>
        /// 契约组编号
        /// </summary>
        public virtual string ContractGroupCode
        {
            get { return _contractGroupCode; }
            set { _contractGroupCode = value; }
        }
        /// <summary>
        /// 部位
        /// </summary>
        public virtual string WorkPart
        {
            get { return _workPart; }
            set { _workPart = value; }
        }
        /// <summary>
        /// 材料
        /// </summary>
        public virtual string WorkUseMaterial
        {
            get { return _workUseMaterial; }
            set { _workUseMaterial = value; }
        }
        /// <summary>
        /// 做法
        /// </summary>
        public virtual string WorkMethod
        {
            get { return _workMethod; }
            set { _workMethod = value; }
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
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; }
        }

        private ISet<GWBSDetailCostSubject> _listCostSubjectDetails = new HashedSet<GWBSDetailCostSubject>();
        /// <summary>
        /// 分科目成本明细
        /// </summary>
        public virtual ISet<GWBSDetailCostSubject> ListCostSubjectDetails
        {
            get { return _listCostSubjectDetails; }
            set { _listCostSubjectDetails = value; }
        }
    }
    /// <summary>
    /// 工程WBS明细状态
    /// </summary>
    public enum GWBSDetailState
    {
        [Description("制定")]
        制定 = 1,
        [Description("有效")]
        有效 = 2,
        [Description("无效")]
        无效 = 3
    }
}
