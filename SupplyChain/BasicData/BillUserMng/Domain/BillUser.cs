using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.BasicData.BillUserMng.Domain
{
    /// <summary>
    /// 单据名称
    /// </summary>
    [Serializable]
    [Entity]
    public class BillUser
    {
        private long _Id = -1;
        private long _Version = -1;
        private string _Name;
        private string _Code;
        private Iesi.Collections.Generic.ISet<BillAction> _Actions = new HashedSet<BillAction>();
        private bool _IsAuto = false;
        /// <summary>
        /// 自动发送标志
        /// </summary>
        virtual public bool IsAuto
        {
            get { return _IsAuto; }
            set { _IsAuto = value; }
        }
        /// <summary>
        /// 单据环节
        /// </summary>
        virtual public Iesi.Collections.Generic.ISet<BillAction> Actions
        {
            get { return _Actions; }
            set { _Actions = value; }
        }
        virtual public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        /// <summary>
        /// 序号
        /// </summary>
        virtual public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        /// <summary>
        /// 单据名称
        /// </summary>
        virtual public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

    }
}
