using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.PMCAndWarning.Domain
{
    /// <summary>
    /// 状态检查动作
    /// </summary>
    [Serializable]
    public class StateCheckAction
    {
        private string id;
        private long version;
        private string _actionName;
        private string _actionDesc;
        private StateCheckTriggerMode _triggerMode;
        private decimal _triggerTerm1;
        private string _triggerTerm2;
        private int _triggerTerm3;
        private string _startMethod;

        private Iesi.Collections.Generic.ISet<WarningTarget> _listTargets = new Iesi.Collections.Generic.HashedSet<WarningTarget>();


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
        /// <summary>
        /// 状态检查动作名称
        /// </summary>
        public virtual string ActionName
        {
            get { return _actionName; }
            set { _actionName = value; }
        }
        /// <summary>
        /// 状态检查动作说明
        /// </summary>
        public virtual string ActionDesc
        {
            get { return _actionDesc; }
            set { _actionDesc = value; }
        }
        /// <summary>
        /// 状态检查动作的触发方式
        /// </summary>
        public virtual StateCheckTriggerMode TriggerMode
        {
            get { return _triggerMode; }
            set { _triggerMode = value; }
        }
        /// <summary>
        /// 触发条件1(定时轮询时的时间触发条件,单位：小时)
        /// </summary>
        public virtual decimal TriggerTerm1
        {
            get { return _triggerTerm1; }
            set { _triggerTerm1 = value; }
        }
        /// <summary>
        /// 触发条件2(值为时分秒，用于定点执行)
        /// </summary>
        public virtual string TriggerTerm2
        {
            get { return _triggerTerm2; }
            set { _triggerTerm2 = value; }
        }
        /// <summary>
        /// 触发条件3
        /// </summary>
        public virtual int TriggerTerm3
        {
            get { return _triggerTerm3; }
            set { _triggerTerm3 = value; }
        }
        /// <summary>
        /// 启动方式
        /// </summary>
        public virtual string StartMethod
        {
            get { return _startMethod; }
            set { _startMethod = value; }
        }
        /// <summary>
        /// 预警指标
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<WarningTarget> ListTargets
        {
            get { return _listTargets; }
            set { _listTargets = value; }
        }

        /// <summary>
        /// 临时使用不做MAP
        /// </summary>
        public virtual string TempID
        {
            get;
            set;
        }

    }
    /// <summary>
    /// 状态检查的触发方式
    /// </summary>
    public enum StateCheckTriggerMode
    {
        [Description("定时")]
        定时 = 1,
        [Description("定点")]
        定点 = 2
    }
}
