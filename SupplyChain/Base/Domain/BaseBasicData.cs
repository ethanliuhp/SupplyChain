using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.Base.Domain
{
    /// <summary>
    /// �������ݻ���
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
        /// ״̬
        /// </summary>
        virtual public int State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// ����״̬
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
        /// �汾
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        virtual public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// ��ע
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
        /// �Ƶ�����
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        virtual public DateTime AuditDate
        {
            get { return auditDate; }
            set { auditDate = value; }
        }
        /// <summary>
        /// �Ƶ���
        /// </summary>
        virtual public PersonInfo CreatePerson
        {
            get { return createPerson; }
            set { createPerson = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        virtual public PersonInfo AuditPerson
        {
            get { return auditPerson; }
            set { auditPerson = value; }
        }
        /// <summary>
        /// �ͻ�
        /// </summary>
        virtual public CustomerRelationInfo Customer
        {
            get { return customer; }
            set { customer = value; }
        }
    }
}
