using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain
{
    /// <summary>
    /// 现金流量项目
    /// </summary>
    [Serializable]
    public class CashFlowItems
    {
        private long id = -1;
        private long version = -1;
        private string code;
        private string name;
        private int cashFlowDire;
        private AccountTitle objAccTitle;
        private int state = 1;
        private string belongCode;        

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
        /// 现金流动方向 -1 流出 1 流入
        /// </summary>
        virtual public int CashFlowDire
        {
            get { return cashFlowDire; }
            set { cashFlowDire = value; }
        }

        /// <summary>
        /// 对应科目
        /// </summary>
        virtual public AccountTitle ObjAccTitle
        {
            get { return objAccTitle; }
            set { objAccTitle = value; }
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
