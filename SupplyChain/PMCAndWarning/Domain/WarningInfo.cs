using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using System.ComponentModel;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.PMCAndWarning.Domain
{
    /// <summary>
    /// 预警信息
    /// </summary>
    [Serializable]
    public class WarningInfo
    {
        private string id;
        private long version;
        private string _projectId;
        private string _projectName;
        private string _projectSyscode;
        
        private WarningTarget _theTarget;
        private string _theTargetName;
        private string _theWarningObjectTypeName;
        private string _theWarningObjectId;
        private WarningInfoStateEnum _state;
        private DateTime? _submitTime;
        private DateTime? _failureTime;
        private WarningLevelEnum _level;
        private string _warningContent;
        private PersonInfo _owner;
        private string _ownerName;
        private string _ownerOrgSysCode;

        /// <summary>
        /// 项目Id
        /// </summary>
        public virtual string ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }
        /// <summary>
        /// 工程项目任务层次码
        /// </summary>
        public virtual string ProjectSyscode
        {
            get { return _projectSyscode; }
            set { _projectSyscode = value; }
        }
        /// <summary>
        /// 预警指标
        /// </summary>
        public virtual WarningTarget TheTarget
        {
            get { return _theTarget; }
            set { _theTarget = value; }
        }
        /// <summary>
        /// 预警指标名称
        /// </summary>
        public virtual string TheTargetName
        {
            get { return _theTargetName; }
            set { _theTargetName = value; }
        }
        /// <summary>
        /// 预警对象类型名称（存typeof(object).Name）
        /// </summary>
        public virtual string TheWarningObjectTypeName
        {
            get { return _theWarningObjectTypeName; }
            set { _theWarningObjectTypeName = value; }
        }
        /// <summary>
        /// 预警对象ID
        /// </summary>
        public virtual string TheWarningObjectId
        {
            get { return _theWarningObjectId; }
            set { _theWarningObjectId = value; }
        }
        /// <summary>
        /// 预警状态
        /// </summary>
        public virtual WarningInfoStateEnum State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// 预警信息提报时间
        /// </summary>
        public virtual DateTime? SubmitTime
        {
            get { return _submitTime; }
            set { _submitTime = value; }
        }
        /// <summary>
        /// 预警信息失效时间
        /// </summary>
        public virtual DateTime? FailureTime
        {
            get { return _failureTime; }
            set { _failureTime = value; }
        }
        /// <summary>
        /// 预警级别
        /// </summary>
        public virtual WarningLevelEnum Level
        {
            get { return _level; }
            set { _level = value; }
        }
        /// <summary>
        /// 预警信息
        /// </summary>
        public virtual string WarningContent
        {
            get { return _warningContent; }
            set { _warningContent = value; }
        }
        /// <summary>
        /// 责任人
        /// </summary>
        public virtual PersonInfo Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        /// <summary>
        /// 责任人名称
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
    }
    /// <summary>
    /// 预警状态
    /// </summary>
    public enum WarningInfoStateEnum
    {
        [Description("有效")]
        有效 = 1,
        [Description("无效")]
        无效 = 2
    }
    /// <summary>
    /// 预警级别
    /// </summary>
    public enum WarningLevelEnum
    {
        [Description("无预警")]
        无预警 = 0,
        [Description("低级别")]
        低级别 = 1,
        [Description("中级别")]
        中级别 = 2,
        [Description("高级别")]
        高级别 = 3
    }
}
