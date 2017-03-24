using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.Base.Domain
{
    /// <summary>
    /// 基础数据基类
    /// </summary>
    [Serializable]
    [Entity]
    public abstract class BaseBasicData
    {
        private long id = -1;
        private long version = -1;
        private string code;
        private string name;
        private string descript;
        private int state;
        private DateTime createDate;
        private DateTime auditDate;
        private PersonInfo createPerson;
        private PersonInfo auditPerson;
        private CustomerRelationInfo customer;
        private int auditState;

        /// <summary>
        /// 状态
        /// </summary>
        virtual public int State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// 审批状态
        /// </summary>
        virtual public int AuditState
        {
            get { return auditState; }
            set { auditState = value; }
        }
        /// <summary>
        /// ID
        /// </summary>
        virtual public long Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 版本
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
        /// 备注
        /// </summary>
        virtual public string Descript
        {
            get { return descript; }
            set { descript = value; }
        }
        public override string ToString()
        {
            return name;
        }
        /// <summary>
        /// 制单日期
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
        /// <summary>
        /// 审批日期
        /// </summary>
        virtual public DateTime AuditDate
        {
            get { return auditDate; }
            set { auditDate = value; }
        }
        /// <summary>
        /// 制单人
        /// </summary>
        virtual public PersonInfo CreatePerson
        {
            get { return createPerson; }
            set { createPerson = value; }
        }
        /// <summary>
        /// 审批人
        /// </summary>
        virtual public PersonInfo AuditPerson
        {
            get { return auditPerson; }
            set { auditPerson = value; }
        }
        /// <summary>
        /// 客户
        /// </summary>
        virtual public CustomerRelationInfo Customer
        {
            get { return customer; }
            set { customer = value; }
        }
    }
}
