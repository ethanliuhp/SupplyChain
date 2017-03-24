using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.BasicData.BillUserMng.Domain
{
    /// <summary>
    /// 单据人员信息
    /// </summary>
    [Serializable]
    [Entity]
    public class BillPersonInfor
    {
        private long _Id=-1;
        private long _Version=-1;
        private string _Name;
        private string _Code;
        private PersonInfo _PersonInfor;
        private BillAction _BillAction;
        /// <summary>
        /// 单据环节
        /// </summary>
        virtual public BillAction BillAction
        {
            get { return _BillAction; }
            set { _BillAction = value; }
        }
        /// <summary>
        /// 人员
        /// </summary>
        virtual public PersonInfo PersonInfor
        {
            get { return _PersonInfor; }
            set { _PersonInfor = value; }
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
