using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Component.Util;
using System.ComponentModel;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;



namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    [Serializable]
    [Entity]
    public class AssignWorkerOrderMaster
    {
        private string _id;
        /// <summary></summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private Iesi.Collections.Generic.ISet<AssignWorkerOrderDetail> details = new Iesi.Collections.Generic.HashedSet<AssignWorkerOrderDetail>();
        public virtual Iesi.Collections.Generic.ISet<AssignWorkerOrderDetail> Details
        {
            get { return details; }
            set { details = value; }
        }

        private string _assignWorkerOrderName;
        /// <summary>派工单名称</summary>
        public virtual string Code
        {
            get { return _assignWorkerOrderName; }
            set { _assignWorkerOrderName = value; }
        }

        private string _assignWorkerOrderDescription;
        /// <summary>派工单描述</summary>
        public virtual string AssignWorkerOrderDescription
        {
            get { return _assignWorkerOrderDescription; }
            set { _assignWorkerOrderDescription = value; }
        }

        private string _weekSchedule;
        /// <summary>周进度计划</summary>
        public virtual string WeekSchedule
        {
            get { return _weekSchedule; }
            set { _weekSchedule = value; }
        }

        private string _weekScheduleName;
        /// <summary>周进度计划名称</summary>
        public virtual string WeekScheduleName
        {
            get { return _weekScheduleName; }
            set { _weekScheduleName = value; }
        }

        private string _assignTeam;
        /// <summary>派工队伍</summary>
        public virtual string AssignTeam
        {
            get { return _assignTeam; }
            set { _assignTeam = value; }
        }

        private string _assignTeamName;
        /// <summary>派工队伍名称</summary>
        public virtual string AssignTeamName
        {
            get { return _assignTeamName; }
            set { _assignTeamName = value; }
        }

        private string _projectId;
        /// <summary>项目</summary>
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

        private int _printCount;
        /// <summary>打印次数</summary>
        public virtual int PrintCount
        {
            get { return _printCount; }
            set { _printCount = value; }
        }

        private DateTime _createDate = new DateTime(1900, 1, 1);
        /// <summary>业务时间</summary>
        public virtual DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        private PersonInfo _createPerson;
        /// <summary>制单人</summary>
        public virtual PersonInfo CreatePerson
        {
            get { return _createPerson; }
            set { _createPerson = value; }
        }

        private string _createPersonName;
        /// <summary>制单人姓名</summary>
        public virtual string CreatePersonName
        {
            get { return _createPersonName; }
            set { _createPersonName = value; }
        }

        private DateTime _lastPrintTime = new DateTime(1900, 1, 1);
        /// <summary>最后打印时间</summary>
        public virtual DateTime LastPrintTime
        {
            get { return _lastPrintTime; }
            set { _lastPrintTime = value; }
        }

        private PersonInfo _lastPrintPerson;
        /// <summary>最后打印人</summary>
        public virtual PersonInfo LastPrintPerson
        {
            get { return _lastPrintPerson; }
            set { _lastPrintPerson = value; }
        }

        private string _lastPrintPersonName;
        /// <summary>最后打印人姓名</summary>
        public virtual string LastPrintPersonName
        {
            get { return _lastPrintPersonName; }
            set { _lastPrintPersonName = value; }
        }

        private string  _orgSysCode;

        ///<summary>
        ///
        ///</summary>
        public virtual string  OrgSysCode
        {
            set { this._orgSysCode = value; }
            get { return this._orgSysCode; }
        }

        private DocumentState _state;
        ///<summary>单据状态</summary>
        public virtual DocumentState DocState
        {
            set { this._state = value; }
            get { return this._state; }
        }

        private DateTime _submitDate;
        ///<summary>提交时间</summary>
        public virtual DateTime SubmitDate
        {
            set { this._submitDate = value; }
            get { return this._submitDate; }
        }

        private SendMessageState _msgState;
        ///<summary></summary>
        public virtual SendMessageState MsgState
        {
            set { this._msgState = value; }
            get { return this._msgState; }
        }

        private DateTime _msgDate;
        ///<summary>短信通知发起时间</summary>
        public virtual DateTime MsgDate
        {
            set { this._msgDate = value; }
            get { return this._msgDate; }
        }

        private PersonInfo _msgPerson;
        /// <summary>短信通知发起人</summary>
        public virtual PersonInfo MsgPerson
        {
            get { return _msgPerson; }
            set { _msgPerson = value; }
        }

        private string _msgPersonName;
        /// <summary>短信通知发起人姓名</summary>
        public virtual string MsgPersonName
        {
            get { return _msgPersonName; }
            set { _msgPersonName = value; }
        }

    }
    public enum SendMessageState
    {
        /// <summary>
        /// 尚未短信通知
        /// </summary>
        [Description("未通知")]
        未通知 = 0,
        /// <summary>
        /// 已经短信通知
        /// </summary>
        [Description("已通知")]
        已通知 = 1
    }
}
