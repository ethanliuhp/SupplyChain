using System;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.BasicData.SystemParamMng.Domain
{

    /// <summary>
    /// 系统参数
    /// </summary>
    [Serializable]
    [Entity]
    public class SystemParam
    {
        private long _id = -1;
        private long _version = -1;
        private string _ModuleName;
        private string _OptionStyle;
        private string _OptionDescribe;
        private int _state;
        private int _maymodify;

        /// <summary>
        /// 可修改 系统使用后可修改
        /// </summary>
        virtual public int MayModify
        {
            get { return _maymodify; }
            set { _maymodify = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        virtual public int State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// ID
        /// </summary>
        virtual public long Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// 选项类型
        /// </summary>
        virtual public string ModuleName
        {
            get { return _ModuleName; }
            set { _ModuleName = value; }
        }
        /// <summary>
        /// 选项类型
        /// </summary>
        virtual public string OptionStyle
        {
            get { return _OptionStyle; }
            set { _OptionStyle = value; }
        }
        /// <summary>
        /// 选项描述
        /// </summary>
        virtual public string OptionDescribe
        {
            get { return _OptionDescribe; }
            set { _OptionDescribe = value; }
        }
    }
}
