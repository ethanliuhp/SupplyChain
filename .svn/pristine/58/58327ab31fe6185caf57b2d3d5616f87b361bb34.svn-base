using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.ComponentModel;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    /// <summary>
    /// 与整体计划符合度
    /// </summary>
    public enum EnumWeekSchedulePlanConformity
    {
        [Description("按照计划执行")]
        按照计划执行,
        [Description("工期缩短")]
        工期缩短,
        [Description("工期延长")]
        工期延长
    }

    /// <summary>
    /// 周进度计划明细
    /// </summary>
    [Serializable]
    [Entity]
    public class WeekScheduleDetail
    {
        private string id;
        private GWBSTree _GWBSTree;
        private PBSTree _PBSTree;
        private string _GWBSTreeName;
        private string _PBSTreeName;
        private DateTime plannedBeginDate = new DateTime(1900, 1, 1);
        private DateTime plannedEndDate = new DateTime(1900, 1, 1);
        private decimal plannedWrokload;
        private decimal plannedDuration;
        private string taskCheckState;
        private decimal taskCompletedPercent;
        private int actualDuration;
        private DateTime actualBeginDate = new DateTime(1900, 1, 1);
        private DateTime actualEndDate = new DateTime(1900, 1, 1);
        private decimal actualWorklaod;
        private string completionAnalysis;
        private string planConformity;
        private string descript;
        private string mainTaskContent;
        private WeekScheduleMaster master;
        private EnumSummaryStatus summaryStatus;

        private string forwardBillMasterName;
        private string forwardBillMasterOwner;
        private string forwardBillDtlId;

        //private OBSService oBSService;
        private string supplierName;
        private int gWBSConfirmFlag = 0;
        private DateTime gWBSConfirmDate = new DateTime(1900, 1, 1);
        private int scheduleConfirmFlag = 0;
        private DateTime scheduleConfirmDate = new DateTime(1900, 1, 1);
        private SubContractProject subContractProject;
        private SupplierRelationInfo supplierRelationInfo;

        private string _GWBSTreeSysCode;
        private VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType _nodeType;

        private string projectId;
        private string projectName;
        private DateTime _createTime = DateTime.Now;
        private WeekScheduleDetail parentNode;
        private int _childCount;
        private int _rowIndex;
        private int level;
        private DocumentState state;
        private string sysCode;
        private int orderNo;
        private string scheduleUnit;

        private Iesi.Collections.Generic.ISet<WeekScheduleTask> details = new Iesi.Collections.Generic.HashedSet<WeekScheduleTask>();
        public virtual Iesi.Collections.Generic.ISet<WeekScheduleTask> Details
        {
            get { return details; }
            set { details = value; }
        }

        /// <summary>
        /// 项目ID
        /// </summary>
        public virtual string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
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
        /// 项目任务层次码
        /// </summary>
        public virtual string GWBSTreeSysCode
        {
            get { return _GWBSTreeSysCode; }
            set { _GWBSTreeSysCode = value; }
        }
        /// <summary>
        /// 节点类型
        /// </summary>
        public virtual VirtualMachine.Patterns.CategoryTreePattern.Domain.NodeType NodeType
        {
            get { return _nodeType; }
            set { _nodeType = value; }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual SupplierRelationInfo SupplierRelationInfo
        {
            get { return supplierRelationInfo; }
            set { supplierRelationInfo = value; }
        }

        /// <summary>
        /// 分包项目
        /// </summary>
        public virtual SubContractProject SubContractProject
        {
            get { return subContractProject; }
            set { subContractProject = value; }
        }

        /// <summary>
        /// 形象进度确认时间
        /// </summary>
        public virtual DateTime ScheduleConfirmDate
        {
            get { return scheduleConfirmDate; }
            set { scheduleConfirmDate = value; }
        }

        /// <summary>
        /// 形象进度确认标志 1己确认
        /// </summary>
        public virtual int ScheduleConfirmFlag
        {
            get { return scheduleConfirmFlag; }
            set { scheduleConfirmFlag = value; }
        }

        /// <summary>
        /// 工程量确认时间
        /// </summary>
        public virtual DateTime GWBSConfirmDate
        {
            get { return gWBSConfirmDate; }
            set { gWBSConfirmDate = value; }
        }

        /// <summary>
        /// 工程量确认标志 0未确认 1己确认
        /// </summary>
        public virtual int GWBSConfirmFlag
        {
            get { return gWBSConfirmFlag; }
            set { gWBSConfirmFlag = value; }
        }

        /// <summary>
        /// 承担者名称(供应商)
        /// </summary>
        public virtual string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }

        ///// <summary>
        ///// 服务OBS
        ///// </summary>
        //public virtual OBSService OBSService
        //{
        //    get { return oBSService; }
        //    set { oBSService = value; }
        //}

        /// <summary>
        /// 前驱单据主表名称
        /// </summary>
        public virtual string ForwardBillMasterName
        {
            get { return forwardBillMasterName; }
            set { forwardBillMasterName = value; }
        }
        /// <summary>
        /// 前驱单据责任人
        /// </summary>
        public virtual string ForwardBillMasterOwner
        {
            get { return forwardBillMasterOwner; }
            set { forwardBillMasterOwner = value; }
        }
        /// <summary>
        /// 滚动进度计划明细Id或工区周计划明细ID
        /// </summary>
        public virtual string ForwardBillDtlId
        {
            get { return forwardBillDtlId; }
            set { forwardBillDtlId = value; }
        }

        /// <summary>
        /// 汇总状态
        /// </summary>
        public virtual EnumSummaryStatus SummaryStatus
        {
            get { return summaryStatus; }
            set { summaryStatus = value; }
        }

        /// <summary>
        /// 周进度计划主表
        /// </summary>
        public virtual WeekScheduleMaster Master
        {
            get { return master; }
            set { master = value; }
        }

        /// <summary>
        /// 主要工作内容
        /// </summary>
        public virtual string MainTaskContent
        {
            get { return mainTaskContent; }
            set { mainTaskContent = value; }
        }

        /// <summary>
        /// 摘要
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 与整体计划符合度
        /// </summary>
        public virtual string PlanConformity
        {
            get { return planConformity; }
            set { planConformity = value; }

        }

        /// <summary>
        /// 完成情况分析
        /// </summary>
        public virtual string CompletionAnalysis
        {
            get { return completionAnalysis; }
            set { completionAnalysis = value; }
        }

        /// <summary>
        /// 实际完成工程量
        /// </summary>
        public virtual decimal ActualWorklaod
        {
            get { return actualWorklaod; }
            set { actualWorklaod = value; }
        }

        /// <summary>
        /// 实际结束日期
        /// </summary>
        public virtual DateTime ActualEndDate
        {
            get { return actualEndDate; }
            set { actualEndDate = value; }
        }

        /// <summary>
        /// 实际开始日期
        /// </summary>
        public virtual DateTime ActualBeginDate
        {
            get { return actualBeginDate; }
            set { actualBeginDate = value; }
        }

        /// <summary>
        /// 实际工期
        /// </summary>
        public virtual int ActualDuration
        {
            get { return actualDuration; }
            set { actualDuration = value; }
        }

        /// <summary>
        /// 累计工程形象进度（任务完成百分比）
        /// </summary>
        public virtual decimal TaskCompletedPercent
        {
            get { return taskCompletedPercent; }
            set { taskCompletedPercent = value; }
        }

        /// <summary>
        /// 任务检查状态
        /// </summary>
        public virtual string TaskCheckState
        {
            get { return taskCheckState; }
            set { taskCheckState = value; }
        }

        /// <summary>
        /// 计划工期（计划总天数）
        /// </summary>
        public virtual decimal PlannedDuration
        {
            get { return plannedDuration; }
            set { plannedDuration = value; }
        }

        /// <summary>
        /// 计划完成工程量
        /// </summary>
        public virtual decimal PlannedWrokload
        {
            get { return plannedWrokload; }
            set { plannedWrokload = value; }
        }

        /// <summary>
        /// 计划结束日期
        /// </summary>
        public virtual DateTime PlannedEndDate
        {
            get { return plannedEndDate; }
            set { plannedEndDate = value; }
        }

        /// <summary>
        /// 计划开始日期
        /// </summary>
        public virtual DateTime PlannedBeginDate
        {
            get { return plannedBeginDate; }
            set { plannedBeginDate = value; }
        }

        /// <summary>
        /// PBSTree名称
        /// </summary>
        public virtual string PBSTreeName
        {
            get { return _PBSTreeName; }
            set { _PBSTreeName = value; }
        }

        /// <summary>
        /// GWBSTree名称
        /// </summary>
        public virtual string GWBSTreeName
        {
            get { return _GWBSTreeName; }
            set { _GWBSTreeName = value; }
        }

        /// <summary>
        /// PBSTree
        /// </summary>
        public virtual PBSTree PBSTree
        {
            get { return _PBSTree; }
            set { _PBSTree = value; }
        }

        /// <summary>
        /// GWBSTree
        /// </summary>
        public virtual GWBSTree GWBSTree
        {
            get { return _GWBSTree; }
            set { _GWBSTree = value; }
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
        /// 父节点
        /// </summary>
        public virtual WeekScheduleDetail ParentNode
        {
            get { return parentNode; }
            set { parentNode = value; }
        }

        /// <summary>
        /// 下级节点数
        /// </summary>
        public virtual int ChildCount
        {
            get { return _childCount; }
            set { _childCount = value; }
        }

        /// <summary>
        /// 加载到Grid时的行号
        /// </summary>
        public virtual int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        /// <summary>
        /// 级数
        /// </summary>
        public virtual int Level
        {
            get { return level; }
            set { level = value; }
        }

        /// <summary>
        /// 状态
        /// 描述进度计划主表状态处于发布时各个进度计划节点是否有效
        /// </summary>
        public virtual DocumentState State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 计划明细层次码
        /// </summary>
        public virtual string SysCode
        {
            get { return sysCode; }
            set { sysCode = value; }
        }

        /// <summary>
        /// 排序号（按WBS排序）
        /// </summary>
        public virtual int OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }

        /// <summary>
        /// 工期计量单位
        /// </summary>
        public virtual string ScheduleUnit
        {
            get { return scheduleUnit; }
            set { scheduleUnit = value; }
        }

        private int _isFixed;

        ///<summary>
        ///是否合同履约节点
        ///</summary>
        public virtual int IsFixed
        {
            set { this._isFixed = value; }
            get { return this._isFixed; }
        }

        private bool _productionCuringNode;

        ///<summary>
        ///是否生产固定节点
        ///</summary>
        public virtual bool ProductionCuringNode
        {
            set { this._productionCuringNode = value; }
            get { return this._productionCuringNode; }
        }
        private string  _DelayDesc;

        ///<summary>
        ///前置节点
        ///</summary>
        public virtual string  DelayDesc
        {
            set { this._DelayDesc = value; }
            get { return this._DelayDesc; }
        }


        private Iesi.Collections.Generic.ISet<WeekScheduleRalation> _ralationDetails;

        ///<summary>
        ///推延关系表（前置节点）
        ///</summary>
        public virtual Iesi.Collections.Generic.ISet<WeekScheduleRalation> RalationDetails
        {
            set { this._ralationDetails = value; }
            get { return this._ralationDetails; }
        }


        private int? _wSDOrderNo;

        ///<summary>
        ///计划排序号（按计划本身排序）
        ///</summary>
        public virtual int? WSDOrderNo
        {
            set { this._wSDOrderNo = value; }
            get { return this._wSDOrderNo; }
        }

        private bool? _isExpand;

        ///<summary>
        ///是否展开
        ///</summary>
        public virtual bool? IsExpand
        {
            set { this._isExpand = value; }
            get { return this._isExpand; }
        }

        private bool _isSubmitCheck;

        ///<summary>
        ///
        ///</summary>
        public virtual bool IsSubmitCheck
        {
            set { this._isSubmitCheck = value; }
            get { return this._isSubmitCheck; }
        }

        private PersonInfo _submitPerson;

        ///<summary>
        ///提交人
        ///</summary>
        public virtual PersonInfo SubmitPerson
        {
            set { this._submitPerson = value; }
            get { return this._submitPerson; }
        }

        private string _submitPersonName;

        ///<summary>
        ///提交人姓名
        ///</summary>
        public virtual string SubmitPersonName
        {
            set { this._submitPersonName = value; }
            get { return this._submitPersonName; }
        }

        private DateTime _submitDate;

        ///<summary>
        ///提交人  
        ///</summary>
        public virtual DateTime SubmitDate
        {
            set { this._submitDate = value; }
            get { return this._submitDate; }
        }

        private PersonInfo _auditPerson;

        ///<summary>
        ///审核人   
        ///</summary>
        public virtual PersonInfo AuditPerson
        {
            set { this._auditPerson = value; }
            get { return this._auditPerson; }
        }

        private string _auditPersonName;

        ///<summary>
        ///审核人姓名
        ///</summary>
        public virtual string AuditPersonName
        {
            set { this._auditPersonName = value; }
            get { return this._auditPersonName; }
        }

        private DateTime _auditDate;

        ///<summary>
        ///审核时间
        ///</summary>
        public virtual DateTime AuditDate
        {
            set { this._auditDate = value; }
            get { return this._auditDate; }
        }




        private DateTime _temp_ActualBeginDate;

        ///<summary>
        ///临时字段 实际开始时间
        ///</summary>
        public virtual DateTime Temp_ActualBeginDate
        {
            set { this._temp_ActualBeginDate = value; }
            get { return this._temp_ActualBeginDate; }
        }

        private DateTime _temp_ActualEndDate;

        ///<summary>
        ///临时字段 实际结束时间
        ///</summary>
        public virtual DateTime Temp_ActualEndDate
        {
            set { this._temp_ActualEndDate = value; }
            get { return this._temp_ActualEndDate; }
        }

        private DateTime _tempPlannedBeginDate;

        ///<summary>
        ///临时字段 计划开始时间 
        ///</summary>
        public virtual DateTime Temp_PlannedBeginDate
        {
            set { this._tempPlannedBeginDate = value; }
            get { return this._tempPlannedBeginDate; }
        }

        private DateTime _tempPlannedEndDate;

        ///<summary>
        ///临时字段 计划结束时间
        ///</summary>
        public virtual DateTime Temp_PlannedEndDate
        {
            set { this._tempPlannedEndDate = value; }
            get { return this._tempPlannedEndDate; }
        }

        private decimal _tempPlannedDuration;

        ///<summary>
        ///临时字段 计划工期
        ///</summary>
        public virtual decimal Temp_PlannedDuration
        {
            set { this._tempPlannedDuration = value; }
            get { return this._tempPlannedDuration; }
        }
    }
}
