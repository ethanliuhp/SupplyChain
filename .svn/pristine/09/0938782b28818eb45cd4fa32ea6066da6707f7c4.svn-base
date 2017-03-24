using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRPServiceModel.Domain.Document
{
    /// <summary>
    /// 生成序列号
    /// </summary>
    [Serializable]
    public class GenerateSerialNumber
    {
        private string _id;
        private long _version = -1;
        public virtual string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        /// <summary>
        /// 版本（hibernate使用）
        /// </summary>
        public virtual long Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
            }
        }

        /// <summary>
        /// 生成对象编码（序列号）规则名称
        /// </summary>
        public virtual string RuleName { get; set; }
        /// <summary>
        /// 项目代码
        /// </summary>
        public virtual string ProjectCode { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName { get; set; }
        /// <summary>
        /// 代码长度
        /// </summary>
        public virtual int CodeLength { get; set; }
        /// <summary>
        /// 当前最大值
        /// </summary>
        public virtual long CurrMaxValue { get; set; }
        /// <summary>
        /// 业务对象类全名（带所属程序集）
        /// </summary>
        public virtual string AppClassName { get; set; }
    }
}
