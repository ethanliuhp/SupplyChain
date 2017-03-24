using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.BasicData.BillUserMng.Domain
{
    /// <summary>
    /// ��������
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
        /// �Զ����ͱ�־
        /// </summary>
        virtual public bool IsAuto
        {
            get { return _IsAuto; }
            set { _IsAuto = value; }
        }
        /// <summary>
        /// ���ݻ���
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
        /// �汾
        /// </summary>
        virtual public long Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        /// <summary>
        /// ���
        /// </summary>
        virtual public long Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        virtual public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

    }
}
