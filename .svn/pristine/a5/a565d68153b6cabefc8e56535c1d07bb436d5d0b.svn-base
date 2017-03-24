using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Iesi.Collections;
using Iesi.Collections.Generic;
using System.ComponentModel;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程WBS树
    /// </summary>
    [Entity]
    [Serializable]
    public class GWBSTree : CategoryNode
    {

        private ProjectTaskTypeTree _projectTaskTypeGUID;
        private string _projectTaskTypeName;
        private string _summary;
        private DocumentState _taskState = DocumentState.Edit;
        private DateTime? _taskStateTime = DateTime.Now;//缺省为创建事例的时间也就是编辑状态的时间
        private decimal _contractTotalPrice;
        private string _ownerGUID;
        private string _ownerName;
        private string _ownerOrgSysCode;
        private StandardUnit _priceAmountUnitGUID;
        private string _priceAmountUnitName;
        private decimal _responsibilityTotalPrice;
        private decimal _planTotalPrice;
        private DateTime? _taskPlanStartTime;
        private DateTime? _taskPlanEndTime;
        private string _checkRequire;
        private WBSNodeType _nodeType = WBSNodeType.WBS;
        private string _theProjectGUID;
        private string _theProjectName;
        private DateTime _updatedDate = DateTime.Now;

        private string _NGUID;
        private decimal _addUpFigureProgress;
        private bool _responsibleAccFlag;
        private bool _costAccFlag;
        private bool _productConfirmFlag;
        private bool _subContractFeeFlag;
        private int _checkBatchNumber;

        private OverOrUnderGroundFlagEnum _overOrUndergroundFlag = OverOrUnderGroundFlagEnum.不区分;

        private ISet<GWBSDetail> _Details = new HashedSet<GWBSDetail>();
        private ISet<GWBSRelaPBS> _listRelaPBS = new HashedSet<GWBSRelaPBS>();
        private ISet<ProjectDocumentVerify> _listDocVerify = new HashedSet<ProjectDocumentVerify>();
        private ISet<GWBSDetailImport> _listgwbsdetailimport = new HashedSet<GWBSDetailImport>();
        /// <summary>
        /// 施工任务明细导入表
        /// </summary>
        public virtual ISet<GWBSDetailImport> ListGWBSDetailImport
        {
            get { return _listgwbsdetailimport; }
            set { _listgwbsdetailimport = value; }
        }

        private bool _warehouseFlag = false;

        private AcceptanceCheckStateEnum _acceptanceCheckState;
        private SuperiorCheckStateEnum _superiorCheckState;
        private string _dailyCheckState;

        private string specialType;

        /// <summary>
        /// 专业分类
        /// </summary>
        virtual public string SpecialType
        {
            get { return specialType; }
            set { specialType = value; }
        }

        private int palnTime;
        /// <summary>
        /// 计划工期
        /// </summary>
        virtual public int PalnTime
        {
            get { return palnTime; }
            set { palnTime = value; }
        }
        private int realTime;
        /// <summary>
        /// 实际工期
        /// </summary>
        virtual public int RealTime
        {
            get { return realTime; }
            set { realTime = value; }
        }
        private string completeDescription;
        /// <summary>
        /// 完成情况说明
        /// </summary>
        virtual public string CompleteDescription
        {
            get { return completeDescription; }
            set { completeDescription = value; }
        }

        private DateTime? realStartDate;
        /// <summary>
        /// 实际开始时间
        /// </summary>
        virtual public DateTime? RealStartDate
        {
            get { return realStartDate; }
            set { realStartDate = value; }
        }
        private DateTime? realEndDate;
        /// <summary>
        /// 实际结束时间
        /// </summary>
        virtual public DateTime? RealEndDate
        {
            get { return realEndDate; }
            set { realEndDate = value; }
        }

        private string tempGUID;
        private string fullPath;
        /// <summary>
        /// 项目任务完整路径
        /// </summary>
        public virtual string FullPath
        {
            get { return fullPath; }
            set { fullPath = value; }
        }

        /// <summary>
        /// 临时GUID
        /// </summary>
        public virtual string TempGUID
        {
            get { return tempGUID; }
            set { tempGUID = value; }
        }

        /// <summary>
        /// 地上地下标志
        /// </summary>
        public virtual OverOrUnderGroundFlagEnum OverOrUndergroundFlag
        {
            get { return _overOrUndergroundFlag; }
            set { _overOrUndergroundFlag = value; }
        }
        /// <summary>
        /// 工程文档验证集
        /// </summary>
        public virtual ISet<ProjectDocumentVerify> ListDocVerify
        {
            get { return _listDocVerify; }
            set { _listDocVerify = value; }
        }
        /// <summary>
        /// 仓库标志
        /// </summary>
        public virtual bool WarehouseFlag
        {
            get { return _warehouseFlag; }
            set { _warehouseFlag = value; }
        }
        /// <summary>
        /// 验收检查状态
        /// </summary>
        public virtual AcceptanceCheckStateEnum AcceptanceCheckState
        {
            get { return _acceptanceCheckState; }
            set { _acceptanceCheckState = value; }
        }
        /// <summary>
        /// 监理检查状态
        /// </summary>
        public virtual SuperiorCheckStateEnum SuperiorCheckState
        {
            get { return _superiorCheckState; }
            set { _superiorCheckState = value; }
        }
        /// <summary>
        /// 日常检查状态
        /// </summary>
        public virtual string DailyCheckState
        {
            get { return _dailyCheckState; }
            set { _dailyCheckState = value; }
        }


        /// <summary>
        /// 名称GUID
        /// </summary>
        public virtual string NGUID
        {
            get { return _NGUID; }
            set { _NGUID = value; }
        }
        /// <summary>
        /// 累计工程形象进度
        /// </summary>
        public virtual decimal AddUpFigureProgress
        {
            get { return _addUpFigureProgress; }
            set { _addUpFigureProgress = value; }
        }
        /// <summary>
        /// 责任核算标志
        /// </summary>
        public virtual bool ResponsibleAccFlag
        {
            get { return _responsibleAccFlag; }
            set { _responsibleAccFlag = value; }
        }
        /// <summary>
        /// 成本核算标志
        /// </summary>
        public virtual bool CostAccFlag
        {
            get { return _costAccFlag; }
            set { _costAccFlag = value; }
        }
        /// <summary>
        /// 生产确认标志
        /// </summary>
        public virtual bool ProductConfirmFlag
        {
            get { return _productConfirmFlag; }
            set { _productConfirmFlag = value; }
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
        /// 检验批数目
        /// </summary>
        public virtual int CheckBatchNumber
        {
            get { return _checkBatchNumber; }
            set { _checkBatchNumber = value; }
        }


        /// <summary>
        /// 工程任务类型
        /// </summary>
        public virtual ProjectTaskTypeTree ProjectTaskTypeGUID
        {
            get { return _projectTaskTypeGUID; }
            set { _projectTaskTypeGUID = value; }
        }
        /// <summary>
        /// 工程任务类型名称
        /// </summary>
        public virtual string ProjectTaskTypeName
        {
            get { return _projectTaskTypeName; }
            set { _projectTaskTypeName = value; }
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
        /// 工程任务状态
        /// </summary>
        public virtual DocumentState TaskState
        {
            get { return _taskState; }
            set { _taskState = value; }
        }
        /// <summary>
        /// 工程任务状态时间
        /// </summary>
        public virtual DateTime? TaskStateTime
        {
            get { return _taskStateTime; }
            set { _taskStateTime = value; }
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
        /// 合同数量
        /// </summary>
        public virtual decimal ContractQuantity { get; set; }
        /// <summary>
        /// 责任人GUID
        /// </summary>
        public virtual string OwnerGUID
        {
            get { return _ownerGUID; }
            set { _ownerGUID = value; }
        }
        /// <summary>
        /// 责任人用户名
        /// </summary>
        public virtual string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }
        /// <summary>
        /// 责任人组织层次码
        /// </summary>
        public virtual string OwnerOrgSysCode
        {
            get { return _ownerOrgSysCode; }
            set { _ownerOrgSysCode = value; }
        }

        /// <summary>
        /// 价格计量单位
        /// </summary>
        public virtual StandardUnit PriceAmountUnitGUID
        {
            get { return _priceAmountUnitGUID; }
            set { _priceAmountUnitGUID = value; }
        }
        /// <summary>
        /// 价格计量单位名称
        /// </summary>
        public virtual string PriceAmountUnitName
        {
            get { return _priceAmountUnitName; }
            set { _priceAmountUnitName = value; }
        }
        /// <summary>
        /// 责任合价
        /// </summary>
        public virtual decimal ResponsibilityTotalPrice
        {
            get { return _responsibilityTotalPrice; }
            set { _responsibilityTotalPrice = value; }
        }
        /// <summary>
        /// 责任数量
        /// </summary>
        public virtual decimal ResponsibilityQuantity { get; set; }
        /// <summary>
        /// 计划合价
        /// </summary>
        public virtual decimal PlanTotalPrice
        {
            get { return _planTotalPrice; }
            set { _planTotalPrice = value; }
        }
        /// <summary>
        /// 计划数量
        /// </summary>
        public virtual decimal PlanQuantity { get; set; }

        /// <summary>
        /// 任务计划开始时间
        /// </summary>
        public virtual DateTime? TaskPlanStartTime
        {
            get { return _taskPlanStartTime; }
            set { _taskPlanStartTime = value; }
        }
        /// <summary>
        /// 任务计划结束时间
        /// </summary>
        public virtual DateTime? TaskPlanEndTime
        {
            get { return _taskPlanEndTime; }
            set { _taskPlanEndTime = value; }
        }
        /// <summary>
        /// 检查要求
        /// </summary>
        public virtual string CheckRequire
        {
            get { return _checkRequire; }
            set { _checkRequire = value; }
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
        /// 
        /// </summary>
        public virtual DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; }
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public virtual WBSNodeType NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }
        /// <summary>
        /// 工程WBS明细
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<GWBSDetail> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
        /// <summary>
        /// 关联PBS
        /// </summary>
        public virtual ISet<GWBSRelaPBS> ListRelaPBS
        {
            get { return _listRelaPBS; }
            set { _listRelaPBS = value; }
        }
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        public virtual GWBSTree Clone()
        {
            GWBSTree cat = new GWBSTree();
            cat.Name = this.Name;
            //cat.Code = this.Code;
            cat.OrderNo = this.OrderNo;

            cat.Describe = this.Describe;

            cat.OwnerGUID = this.OwnerGUID;
            cat.OwnerName = this.OwnerName;
            cat.OwnerOrgSysCode = this.OwnerOrgSysCode;

            cat.TheProjectGUID = this.TheProjectGUID;
            cat.TheProjectName = this.TheProjectName;

            //cat.BearOrgGUID = this.BearOrgGUID;
            //cat.BearOrgName = this.BearOrgName;

            cat.ProjectTaskTypeGUID = this.ProjectTaskTypeGUID;
            cat.ProjectTaskTypeName = this.ProjectTaskTypeName;

            //cat.ContractGroupGUID = this.ContractGroupGUID;
            //cat.ContractGroupCode = this.ContractGroupCode;

            foreach (GWBSRelaPBS rela in this.ListRelaPBS)
            {
                GWBSRelaPBS newRela = new GWBSRelaPBS();
                newRela.ThePBS = rela.ThePBS;
                newRela.PBSName = rela.PBSName;

                newRela.TheGWBSTree = cat;
                cat.ListRelaPBS.Add(newRela);
            }

            return cat;
        }


        ////移除的属性
        //移除的属性
        private bool _isAccountNode = false;
        /// <summary>
        /// 是否为核算节点(该属性已作废，判断是否为核算节点使用属性CostAccFlag)
        /// </summary>
        public virtual bool IsAccountNode
        {
            get { return _isAccountNode; }
            set { _isAccountNode = value; }
        }

        /// <summary>
        /// 是否已给子任务分配队伍
        /// </summary>
        public virtual bool IsSetup { get; set; }

        //private GWBSTreeManagementMode _managementMode;
        //private decimal _contractWorkAmount;
        //private decimal _contractPrice;
        //private StandardUnit _workAmountUnitGUID;
        //private string _workAmountUnitName;
        //private string _managementFeatureCode;
        //private decimal _projectDeptDefiniteTotalPrice;
        //private SupplierRelationInfo _bearOrgGUID;
        //private string _bearOrgName;
        //private ContractGroup _contractGroupGUID;
        //private string _contractGroupCode;

        ///// <summary>
        ///// 管理方式
        ///// </summary>
        //public virtual GWBSTreeManagementMode ManagementMode
        //{
        //    get { return _managementMode; }
        //    set { _managementMode = value; }
        //}
        ///// <summary>
        ///// 合同工程量
        ///// </summary>
        //public virtual decimal ContractWorkAmount
        //{
        //    get { return _contractWorkAmount; }
        //    set { _contractWorkAmount = value; }
        //}
        ///// <summary>
        ///// 合同单价
        ///// </summary>
        //public virtual decimal ContractPrice
        //{
        //    get { return _contractPrice; }
        //    set { _contractPrice = value; }
        //}
        ///// <summary>
        ///// 工程计量单位
        ///// </summary>
        //public virtual StandardUnit WorkAmountUnitGUID
        //{
        //    get { return _workAmountUnitGUID; }
        //    set { _workAmountUnitGUID = value; }
        //}
        ///// <summary>
        ///// 工程计量单位名称
        ///// </summary>
        //public virtual string WorkAmountUnitName
        //{
        //    get { return _workAmountUnitName; }
        //    set { _workAmountUnitName = value; }
        //}
        ///// <summary>
        ///// 管理特征代码
        ///// </summary>
        //public virtual string ManagementFeatureCode
        //{
        //    get { return _managementFeatureCode; }
        //    set { _managementFeatureCode = value; }
        //}
        ///// <summary>
        ///// 工程部门确定合价
        ///// </summary>
        //public virtual decimal ProjectDeptDefiniteTotalPrice
        //{
        //    get { return _projectDeptDefiniteTotalPrice; }
        //    set { _projectDeptDefiniteTotalPrice = value; }
        //}
        ///// <summary>
        ///// 承担组织
        ///// </summary>
        //public virtual SupplierRelationInfo BearOrgGUID
        //{
        //    get { return _bearOrgGUID; }
        //    set { _bearOrgGUID = value; }
        //}
        ///// <summary>
        ///// 承担组织名称
        ///// </summary>
        //public virtual string BearOrgName
        //{
        //    get { return _bearOrgName; }
        //    set { _bearOrgName = value; }
        //}
        ///// <summary>
        ///// 契约组
        ///// </summary>
        //public virtual ContractGroup ContractGroupGUID
        //{
        //    get { return _contractGroupGUID; }
        //    set { _contractGroupGUID = value; }
        //}
        ///// <summary>
        ///// 契约组代号
        ///// </summary>
        //public virtual string ContractGroupCode
        //{
        //    get { return _contractGroupCode; }
        //    set { _contractGroupCode = value; }
        //}

        private int _isFixed;

        ///<summary>
        ///是否固定节点
        ///</summary>
        public virtual int IsFixed
        {
            set { this._isFixed = value; }
            get { return this._isFixed; }
        }

        private DateTime? _startPlanBeginDate;

        ///<summary>
        ///初始计划开始时间
        ///</summary>
        public virtual DateTime? StartPlanBeginDate
        {
            set { this._startPlanBeginDate = value; }
            get { return this._startPlanBeginDate; }
        }

        private DateTime? _startPlanEndDate;

        ///<summary>
        ///初始计划结束时间
        ///</summary>
        public virtual DateTime? StartPlanEndDate
        {
            set { this._startPlanEndDate = value; }
            get { return this._startPlanEndDate; }
        }
        /// <summary>
        /// 是否为生产固化节点（1：是；0：不是）
        /// </summary>
        public virtual bool ProductionCuringNode { set; get; }

        private DateTime? _proCurBeginDate;

        ///<summary>
        ///生产固化节点开始时间
        ///</summary>
        public virtual DateTime? ProCurBeginDate
        {
            set { this._proCurBeginDate = value; }
            get { return this._proCurBeginDate; }
        }

        private DateTime? _proCurEndDate;

        ///<summary>
        ///
        ///</summary>
        public virtual DateTime? ProCurEndDate
        {
            set { this._proCurEndDate = value; }
            get { return this._proCurEndDate; }
        }
    }

    /// <summary>
    /// GWBS状态
    /// </summary>
    public enum GWBSTreeState
    {
        /// <summary>
        /// 编辑状态
        /// </summary>
        [Description("编制")]
        编制 = 1,
        /// <summary>
        /// 编制完成，提交审批
        /// </summary>
        [Description("提交")]
        提交 = 2,
        /// <summary>
        /// 审批通过，发布执行
        /// </summary>
        [Description("执行")]
        执行 = 3,
        /// <summary>
        /// {进度计划节点}上传了【任务实际完成时间】，并且所有下属{工程任务明细}_【任务完工百分比】均为100%；
        /// </summary>
        [Description("完工")]
        完工 = 4,
        /// <summary>
        /// 可作参考，不能继续使用；
        /// </summary>
        [Description("冻结")]
        冻结 = 5,
        /// <summary>
        /// 该工程项目任务已经无效。
        /// </summary>
        [Description("作废")]
        作废 = 6
    }

    public enum GWBSTreeManagementMode
    {
        [Description("自行完成")]
        自行完成 = 1,
        /// <summary>
        /// 企业内部部门对任务的收入、成本包干，如：安装。
        /// </summary>
        [Description("内部大包")]
        内部大包 = 2,
        /// <summary>
        /// 企业外部对任务的收入、成本包干，如一些专业分包。
        /// </summary>
        [Description("外部大包")]
        外部大包 = 3,
        /// <summary>
        /// 由企业外部承担工程任务的某些工作内容，如劳务分包。
        /// </summary>
        [Description("外部分包")]
        外部分包 = 4
    }

    /// <summary>
    /// 节点类型
    /// </summary>
    public enum WBSNodeType
    {
        WBS = 1,
        PBS = 2
    }

    /// <summary>
    /// 地上地下标志(描述GWBS节点属于地上、还是地下)
    /// </summary>
    public enum OverOrUnderGroundFlagEnum
    {
        /// <summary>
        /// （包括地上和地下，如单位工程节点等整体节点）
        /// </summary>
        [Description("不区分")]
        不区分 = 0,
        /// <summary>
        /// （该节点及其下属子节点属于地下）
        /// </summary>
        [Description("地下")]
        地下 = 1,
        /// <summary>
        /// （该节点及其下属子节点数据地上）
        /// </summary>
        [Description("地上")]
        地上 = 2
    }

    /// <summary>
    /// 验收检查状态（当【累积工程形象进度】=100% 并且【日常检查状态】均通过时，验收检查状态为2.已通过。）
    /// </summary>
    public enum AcceptanceCheckStateEnum
    {
        未通过 = 1,
        已通过 = 2
    }

    /// <summary>
    /// 监理检查状态（当该节点所有检验批均通过后，该节点的监理检查状态为2.已通过。）
    /// </summary>
    public enum SuperiorCheckStateEnum
    {
        未通过 = 1,
        已通过 = 2
    }
}
