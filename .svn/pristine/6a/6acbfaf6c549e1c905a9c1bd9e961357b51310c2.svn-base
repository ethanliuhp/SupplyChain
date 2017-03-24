using System;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    [Serializable]
    [Entity]
    public class WeekScheduleTask
    {
        private string id;

        /// <summary>
        /// GUID
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }

        private WeekScheduleDetail master;
        /// <summary>
        /// 主表
        /// </summary>
        virtual public WeekScheduleDetail Master
        {
            get { return master; }
            set { master = value; }
        }

        private string _projectId;

        /// <summary>项目Id</summary>
        public virtual string ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }

        private string _projectName;

        /// <summary>项目名称</summary>
        public virtual string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        private GWBSDetail _task;

        /// <summary>任务</summary>
        public virtual GWBSDetail Task
        {
            get { return _task; }
            set { _task = value; }
        }

        private string _taskName;

        /// <summary>任务名称</summary>
        public virtual string TaskName
        {
            get { return _taskName; }
            set { _taskName = value; }
        }

        private DateTime _planBeginDate;

        /// <summary>计划开始日期</summary>
        public virtual DateTime PlanBeginDate
        {
            get { return _planBeginDate; }
            set { _planBeginDate = value; }
        }

        private DateTime _planEndDate;

        /// <summary>计划结束日期</summary>
        public virtual DateTime PlanEndDate
        {
            get { return _planEndDate; }
            set { _planEndDate = value; }
        }

        private decimal _planTime;

        /// <summary>计划工期</summary>
        public virtual decimal PlanTime
        {
            get { return _planTime; }
            set { _planTime = value; }
        }

        private DateTime _realBeginDate;

        /// <summary>实际开始日期</summary>
        public virtual DateTime RealBeginDate
        {
            get { return _realBeginDate; }
            set { _realBeginDate = value; }
        }

        private DateTime _realEndDate;

        /// <summary>实际结束日期</summary>
        public virtual DateTime RealEndDate
        {
            get { return _realEndDate; }
            set { _realEndDate = value; }
        }

        private SubContractProject _subContractProject;

        /// <summary>分包队伍</summary>
        public virtual SubContractProject SubContractProject
        {
            get { return _subContractProject; }
            set { _subContractProject = value; }
        }

        private string _subContractProjectName;

        /// <summary>分包队伍名称</summary>
        public virtual string SubContractProjectName
        {
            get { return _subContractProjectName; }
            set { _subContractProjectName = value; }
        }

        private GWBSTree _gwbsTree;

        /// <summary></summary>
        public virtual GWBSTree GwbsTree
        {
            get { return _gwbsTree; }
            set { _gwbsTree = value; }
        }

        private string _gwbsName;

        /// <summary></summary>
        public virtual string GwbsName
        {
            get { return _gwbsName; }
            set { _gwbsName = value; }
        }

        private DateTime _createTime;

        /// <summary>创建时间</summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

    }
}
