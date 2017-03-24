using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.Financial.InitialData.Domain
{
    /// <summary>
    /// 加锁对象
    /// </summary>
    [Serializable]
    [Entity]
    public class LockNo
    {
        private long id = -1;
        private long version = -1;
        private string code;

        /// <summary>
        /// ID
        /// </summary>
        virtual public long Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 版本号
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 号码
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }
    }
}
