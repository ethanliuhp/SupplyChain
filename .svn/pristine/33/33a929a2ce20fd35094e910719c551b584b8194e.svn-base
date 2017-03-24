using System;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Core.Attributes;
namespace Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain
{
   [Serializable]
   [Entity]
   public class ApproveBill
   {
      private string _id;
      /// <summary></summary>
      public virtual string Id
      {
         get { return _id; }
         set { _id = value; }
      }

      private string _projectId;
      /// <summary></summary>
      public virtual string ProjectId
      {
          get { return _projectId; }
          set { _projectId = value; }
      }

      private string _projectName;
      /// <summary></summary>
      public virtual string ProjectName
      {
          get { return _projectName; }
          set { _projectName = value; }
      }

      private long _version;
      /// <summary></summary>
      public virtual long Version
      {
          get { return _version; }
          set { _version = value; }
      }

      private string _billId;
      /// <summary>单据Id</summary>
      public virtual string BillId
      {
         get { return _billId; }
         set { _billId = value; }
      }

      private string _billCode;
      /// <summary>单据编号</summary>
      public virtual string BillCode
      {
         get { return _billCode; }
         set { _billCode = value; }
      }

      private string _billSysCode;
      /// <summary>单据SysCode</summary>
      public virtual string BillSysCode
      {
         get { return _billSysCode; }
         set { _billSysCode = value; }
      }

      private DateTime _billCreateDate;
      /// <summary>单据创建日期</summary>
      public virtual DateTime BillCreateDate
      {
         get { return _billCreateDate; }
         set { _billCreateDate = value; }
      }

      private PersonInfo _billCreatePerson;
      /// <summary>单据创建人</summary>
      public virtual PersonInfo BillCreatePerson
      {
         get { return _billCreatePerson; }
         set { _billCreatePerson = value; }
      }

      private string _billCreatePersonName;
      /// <summary>单据创建人姓名</summary>
      public virtual string BillCreatePersonName
      {
         get { return _billCreatePersonName; }
         set { _billCreatePersonName = value; }
      }

      private AppTableSet _appTableSet;
      /// <summary>审批表单定义</summary>
      public virtual AppTableSet AppTableDefine
      {
         get { return _appTableSet; }
         set { _appTableSet = value; }
      }

      private AppSolutionSet _appSolution;
      /// <summary>审批方案定义</summary>
      public virtual AppSolutionSet AppSolution
      {
         get { return _appSolution; }
         set { _appSolution = value; }
      }

      private string _appSolutionName;
      /// <summary>审批方案定义名称</summary>
      public virtual string AppSolutionName
      {
         get { return _appSolutionName; }
         set { _appSolutionName = value; }
      }

      private bool _isDone;
      /// <summary>是否审批完成</summary>
      public virtual bool IsDone
      {
         get { return _isDone; }
         set { _isDone = value; }
      }

      private DateTime _lastModifTime;
      /// <summary>最后一次修改时间</summary>
      public virtual DateTime LastModifTime
      {
         get { return _lastModifTime; }
         set { _lastModifTime = value; }
      }

      private string _lastModifyBy;
      /// <summary>最后一次修改人</summary>
      public virtual string LastModifyBy
      {
         get { return _lastModifyBy; }
         set { _lastModifyBy = value; }
      }

      private int _nextStep;
      /// <summary>审批下一步</summary>
      public virtual int NextStep
      {
          get { return _nextStep; }
          set { _nextStep = value; }
      }

      private string _approveJob;
      /// <summary>审批人岗位（非持久化）</summary>
      public virtual string ApproveJob
      {
          get { return _approveJob; }
          set { _approveJob = value; }
      }

   }
}
