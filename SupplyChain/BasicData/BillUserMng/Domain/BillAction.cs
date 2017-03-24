using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.BasicData.BaleNoMng.Domain;

namespace Application.Business.Erp.SupplyChain.BasicData.BillUserMng.Domain
{
    /// <summary>
    /// 单据环节
    /// </summary>
    [Serializable]
    [Entity]
    public class BillAction
    {
        private long _Id=-1;
        private long _Version=-1;
        private string _Name;
        private string _Code;
        private Iesi.Collections.Generic.ISet<BillPersonInfor> _Persons = new HashedSet<BillPersonInfor>();
        private BillUser _BillUser;
        private string _Answer;
        /// <summary>
        /// 留言
        /// </summary>
        virtual public string Answer
        {
            get { return _Answer; }
            set { _Answer = value; }
        }
        /// <summary>
        /// 单据名称
        /// </summary>
        virtual public BillUser BillUser
        {
            get { return _BillUser; }
            set { _BillUser = value; }
        }
        /// <summary>
        /// 人员信息
        /// </summary>
        virtual public Iesi.Collections.Generic.ISet<BillPersonInfor> Persons
        {
            get { return _Persons; }
            set { _Persons = value; }
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
