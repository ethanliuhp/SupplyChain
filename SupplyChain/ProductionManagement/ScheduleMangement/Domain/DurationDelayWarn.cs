using System;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Core.Attributes;
namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
   [Serializable]
   [Entity]
   public class DurationDelayWarn
   {
      private string _id;
      /// <summary></summary>
      public virtual string Id
      {
         get { return _id; }
         set { _id = value; }
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

      private GWBSTree _task;
      /// <summary>任务Id</summary>
      public virtual GWBSTree Task
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

      private int _planTime;
      /// <summary>计划工期</summary>
      public virtual int PlanTime
      {
         get { return _planTime; }
         set { _planTime = value; }
      }

      private decimal _planRate;
      /// <summary>计划进度</summary>
      public virtual decimal PlanRate
      {
         get { return _planRate; }
         set { _planRate = value; }
      }

      private DateTime _realBeginDate;
      /// <summary>实际开始日期</summary>
      public virtual DateTime RealBeginDate
      {
         get { return _realBeginDate; }
         set { _realBeginDate = value; }
      }

      private decimal _realRate;
      /// <summary>实际进度</summary>
      public virtual decimal RealRate
      {
         get { return _realRate; }
         set { _realRate = value; }
      }

      private int _delayDays;
      /// <summary>任务延误天数</summary>
      public virtual int DelayDays
      {
         get { return _delayDays; }
         set { _delayDays = value; }
      }

      private decimal _delayCosts;
      /// <summary>延误导致成本增加额</summary>
      public virtual decimal DelayCosts
      {
         get { return _delayCosts; }
         set { _delayCosts = value; }
      }

      private int _warnLevel;
      /// <summary>预警等级 0绿色 1蓝色 2黄色 3红色</summary>
      public virtual int WarnLevel
      {
         get { return _warnLevel; }
         set { _warnLevel = value; }
      }

      private string _costDetail;
      /// <summary>延误增加成本明细</summary>
      public virtual string CostDetail
      {
         get { return _costDetail; }
         set { _costDetail = value; }
      }

      private string _taskFullPath;
      /// <summary>任务全路径</summary>
      public virtual string TaskFullPath
      {
         get { return _taskFullPath; }
         set { _taskFullPath = value; }
      }

      private decimal _projectDelayDays;
      /// <summary>折算成项目延误天数</summary>
      public virtual decimal ProjectDelayDays
      {
         get { return _projectDelayDays; }
         set { _projectDelayDays = value; }
      }

      private DateTime _createDate;
      /// <summary>生成日期</summary>
      public virtual DateTime CreateDate
      {
         get { return _createDate; }
         set { _createDate = value; }
      }

      private string _orgSyscode;
      /// <summary>组织层次码</summary>
      public virtual string OrgSyscode
      {
         get { return _orgSyscode; }
         set { _orgSyscode = value; }
      }

      private DateTime _modifyTime;
      /// <summary>更新时间</summary>
      public virtual DateTime ModifyTime
      {
         get { return _modifyTime; }
         set { _modifyTime = value; }
      }

      private string _ownerOrg;
      /// <summary>归属组织</summary>
      public virtual string OwnerOrg
      {
          get { return _ownerOrg; }
          set { _ownerOrg = value; }
      }

      private bool  _isProjectDelay;

      ///<summary>
      ///标志，用来区分是否项目延期
      ///</summary>
      public virtual bool  IsProjectDelay
      {
          set { this._isProjectDelay = value; }
          get { return this._isProjectDelay; }
      }
   }
}
