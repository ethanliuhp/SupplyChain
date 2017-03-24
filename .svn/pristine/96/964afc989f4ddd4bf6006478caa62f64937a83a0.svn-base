using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain
{
    /// <summary>
    /// 版本
    /// </summary>
    [Serializable]
    public class Versions
    {
        private string id;
        private IfcChangeAction changeAction;
        private string ifcPersonAndOrganizationInfo;
        private string ifcPersonAndOrganizationId;
        private string ifcApplicationInfo;
        private string ifcApplicationId;
        private IfcState state = IfcState.可读写;
        private DateTime lastUpdateTime;
        private string lastUpdateApplicationInfo;
        private string lastUpdateApplicationId;
        private string lastUpdateUserInfo;
        private string lastUpdateUserId;
        private string snapValue;
        
        /// <summary>
        /// GUID
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
        
        /// <summary>
        /// 更改
        /// </summary>
        public virtual IfcChangeAction ChangeAction
        {
            get { return changeAction; }
            set { changeAction = value; }
        }
        
        /// <summary>
        /// 所有者信息
        /// </summary>
        public virtual string IfcPersonAndOrganizationInfo
        {
            get { return ifcPersonAndOrganizationInfo; }
            set { ifcPersonAndOrganizationInfo = value; }
        }
       
        /// <summary>
        /// 所有者Id
        /// </summary>
        public virtual string IfcPersonAndOrganizationId
        {
            get { return ifcPersonAndOrganizationId; }
            set { ifcPersonAndOrganizationId = value; }
        }
        
        /// <summary>
        /// 责任应用信息
        /// </summary>
        public virtual string IfcApplicationInfo
        {
            get { return ifcApplicationInfo; }
            set { ifcApplicationInfo = value; }
        }
        
        /// <summary>
        /// 责任应用Id
        /// </summary>
        public virtual string IfcApplicationId
        {
            get { return ifcApplicationId; }
            set { ifcApplicationId = value; }
        }
        
        /// <summary>
        /// 状态
        /// </summary>
        public virtual IfcState State
        {
            get { return state; }
            set { state = value; }
        }
        
        /// <summary>
        /// 最后更改时间
        /// </summary>
        public virtual DateTime LastUpdateTime
        {
            get { return lastUpdateTime; }
            set { lastUpdateTime = value; }
        }
        
        /// <summary>
        /// 最后修改的应用
        /// </summary>
        public virtual string LastUpdateApplicationInfo
        {
            get { return lastUpdateApplicationInfo; }
            set { lastUpdateApplicationInfo = value; }
        }

        /// <summary>
        /// 最后修改的应用Id
        /// </summary>
        public virtual string LastUpdateApplicationId
        {
            get { return lastUpdateApplicationId; }
            set { lastUpdateApplicationId = value; }
        }

        /// <summary>
        /// 最后修改用户
        /// </summary>
        public virtual string LastUpdateUserInfo
        {
            get { return lastUpdateUserInfo; }
            set { lastUpdateUserInfo = value; }
        }

        /// <summary>
        /// 最后修改用户Id
        /// </summary>
        public virtual string LastUpdateUserId
        {
            get { return lastUpdateUserId; }
            set { lastUpdateUserId = value; }
        }
        /// <summary>
        /// 临时值
        /// </summary>
        public virtual string SnapValue
        {
            get { return snapValue; }
            set { snapValue = value; }
        }

    }
    /// <summary>
    /// 更改
    /// </summary>
    public enum IfcChangeAction
    {
        [Description("无更改")]
        无更改 = 1,
        [Description("更改")]
        更改 = 2,
        [Description("增加")]
        增加 = 3,
        [Description("删除")]
        删除 = 4,
        [Description("无定义")]
        无定义 = 5
    }
    /// <summary>
    /// 状态
    /// </summary>
    public enum IfcState
    {
        [Description("可读写")]
        可读写 = 0,
        [Description("只读")]
        只读 = 1,
        [Description("不可被访问")]
        不可被访问 = 2,
        [Description("读写锁定")]
        读写锁定 = 3,
        [Description("读锁定")]
        读锁定 = 4
    }
}
