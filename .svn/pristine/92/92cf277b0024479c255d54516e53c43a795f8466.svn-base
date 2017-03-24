using System;
using System.Collections;
using System.Text;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.Financial.BasicAccount.AssisAccount.Domain
{
    /// <summary>
    /// 台账类别
    /// </summary>
    [Serializable]
    [Entity]
    public class DeskAccount
    {
        private string id;
        private long version = -1;
        private string code;
        private string name;
        private IList details = new ArrayList();
        private int state = 1;
        private string belongCode;

        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
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
        /// 编码
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        virtual public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 台账项目
        /// </summary>
        virtual public IList Details
        {
            get { return details; }
            set { details = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        virtual public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 归属编码
        /// </summary>
        virtual public string BelongCode
        {
            get { return belongCode; }
            set { belongCode = value; }
        }
    }
}
