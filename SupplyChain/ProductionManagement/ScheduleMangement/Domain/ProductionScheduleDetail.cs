using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    /// <summary>
    /// 进度计划明细
    /// </summary>
    [Serializable]
    [Entity]
    public class ProductionScheduleDetail
    {
        private string id;
        private ProductionScheduleMaster master;
        private string scheduleUnit;
        private int plannedDuration;
        private int actualDuration;
        private string taskDescript;
        private string taskRequirements;
        private DateTime plannedBeginDate = new DateTime(1900, 1, 1);
        private DateTime plannedEndDate = new DateTime(1900, 1, 1);
        private DateTime actualBeginDate = new DateTime(1900, 1, 1);
        private DateTime actualEndDate = new DateTime(1900, 1, 1);
        private EnumScheduleDetailState state;
        private ProductionScheduleDetail parentNode;
        private string sysCode;
        private int level;
        private GWBSTree _GWBSTree;
        private string _GWBSTreeName;
        private string _GWBSTreeSysCode;
        private NodeType _GWBSNodeType;
        private bool isSelected = false;
        private int orderNo;
        private decimal _addupFigureProgress;
        private string _gwbsFullPath;
        private int _childCount;
        private int _rowIndex;

        /// <summary>
        /// 项目任务完成路径（临时存储）
        /// </summary>
        public virtual string GwbsFullPath
        {
            get { return _gwbsFullPath; }
            set { _gwbsFullPath = value; }
        }

        /// <summary>
        /// 任务累计形象进度
        /// </summary>
        public virtual decimal AddupFigureProgress
        {
            get { return _addupFigureProgress; }
            set { _addupFigureProgress = value; }
        }

        /// <summary>
        /// 排序号
        /// </summary>
        public virtual int OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        public virtual bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        /// <summary>
        /// 工程任务名称
        /// </summary>
        public virtual string GWBSTreeName
        {
            get { return _GWBSTreeName; }
            set { _GWBSTreeName = value; }
        }

        /// <summary>
        /// 工程项目任务层次码
        /// </summary>
        public virtual string GWBSTreeSysCode
        {
            get { return _GWBSTreeSysCode; }
            set { _GWBSTreeSysCode = value; }
        }

        /// <summary>
        /// 工程项目任务节点类型
        /// </summary>
        public virtual NodeType GWBSNodeType
        {
            get { return _GWBSNodeType; }
            set { _GWBSNodeType = value; }
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
        /// 工程WBS树(工程项目任务)
        /// </summary>
        public virtual GWBSTree GWBSTree
        {
            get { return _GWBSTree; }
            set { _GWBSTree = value; }
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
        /// 父节点
        /// </summary>
        public virtual ProductionScheduleDetail ParentNode
        {
            get { return parentNode; }
            set { parentNode = value; }
        }

        /// <summary>
        /// 状态
        /// 描述进度计划主表状态处于发布时各个进度计划节点是否有效
        /// </summary>
        public virtual EnumScheduleDetailState State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 任务实际结束日期
        /// </summary>
        public virtual DateTime ActualEndDate
        {
            get { return actualEndDate; }
            set { actualEndDate = value; }
        }

        /// <summary>
        /// 任务实际开始日期
        /// </summary>
        public virtual DateTime ActualBeginDate
        {
            get { return actualBeginDate; }
            set { actualBeginDate = value; }
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
        /// 任务要求
        /// </summary>
        public virtual string TaskRequirements
        {
            get { return taskRequirements; }
            set { taskRequirements = value; }
        }

        /// <summary>
        /// 任务描述
        /// </summary>
        public virtual string TaskDescript
        {
            get { return taskDescript; }
            set { taskDescript = value; }
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
        /// 计划工期
        /// </summary>
        public virtual int PlannedDuration
        {
            get { return plannedDuration; }
            set { plannedDuration = value; }
        }

        /// <summary>
        /// 工期计量单位
        /// </summary>
        public virtual string ScheduleUnit
        {
            get { return scheduleUnit; }
            set { scheduleUnit = value; }
        }

        /// <summary>
        /// 进度计划
        /// </summary>
        public virtual ProductionScheduleMaster Master
        {
            get { return master; }
            set { master = value; }
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
    }
}
