using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.PMCAndWarning.Domain
{
    /// <summary>
    /// 预警信息推送方式
    /// </summary>
    [Serializable]
    public class WarningPushMode
    {
        private string id;
        private long version;
        private WarningTarget _theTarget;
        private OperationRole _userRole;
        private string _userRoleName;
        private WarningLevelEnum _warningLevel;
        private WarningPushModeEnum _pushMode;

        /// <summary>
        /// 预警指标
        /// </summary>
        public virtual WarningTarget TheTarget
        {
            get { return _theTarget; }
            set { _theTarget = value; }
        }
        /// <summary>
        /// 用户角色
        /// </summary>
        public virtual OperationRole UserRole
        {
            get { return _userRole; }
            set { _userRole = value; }
        }
        /// <summary>
        /// 用户角色名称
        /// </summary>
        public virtual string UserRoleName
        {
            get { return _userRoleName; }
            set { _userRoleName = value; }
        }
        /// <summary>
        /// 预警级别
        /// </summary>
        public virtual WarningLevelEnum WarningLevel
        {
            get { return _warningLevel; }
            set { _warningLevel = value; }
        }
        /// <summary>
        /// 推送方式
        /// </summary>
        public virtual WarningPushModeEnum PushMode
        {
            get { return _pushMode; }
            set { _pushMode = value; }
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
    /// 预警信息推送方式
    /// </summary>
    public enum WarningPushModeEnum
    {
        [Description("不推送")]
        不推送 = 0,
        /// <summary>
        /// 登录时在起始页显示，用户登录时查询{预警信息}并在起始页上显示
        /// </summary>
        [Description("登录显示")]
        登录显示 = 1,
        /// <summary>
        /// 推送到具有该角色用户的起始页
        /// </summary>
        [Description("起始页推送")]
        起始页推送 = 2,
        /// <summary>
        /// 为具有该角色用户发短信
        /// </summary>
        [Description("短信")]
        短信 = 3
    }
}
