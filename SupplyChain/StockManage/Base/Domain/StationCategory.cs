using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.Base.Domain
{
    /// <summary>
    /// ��λ��
    /// </summary>
    [Serializable]
    public class StationCategory : CategoryNode
    {
        private int stationKind;
        private string address;

        private decimal capability;
        private decimal usableCapability;
        private decimal usedPrice;
        private PersonInfo author;
        private OperationOrgInfo operOrgInfo;
        private int businessType;
        private DateTime updateDate;
        private PersonInfo modifyPerson;
        private string opgSysCode;
        //private int nodeType;

        private CurrentProjectInfo projectId;
        private string projectName;

        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public virtual string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        /// <summary>
        /// ��ĿID
        /// </summary>
        public virtual CurrentProjectInfo ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        //public virtual int NodeType
        //{
        //    get { return nodeType; }
        //    set { nodeType = value; }
        //}

        /// <summary>
        /// �ֿ�ҵ������(0 ���� ,1 ������)
        /// </summary>
        virtual public int BusinessType
        {
            get { return businessType; }
            set { businessType = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        virtual public new PersonInfo Author
        {
            get
            {
                return author;
            }
            set
            {
                author = value;
            }
        }

        /// <summary>
        /// ��λ����(0 �ֿ� ,1 ��λ)
        /// </summary>
        virtual public int StationKind
        {
            get { return stationKind; }
            set { stationKind = value; }
        }
        /// <summary>
        /// ҵ����֯
        /// </summary>
        virtual public OperationOrgInfo OperOrgInfo
        {
            get { return operOrgInfo; }
            set { operOrgInfo = value; }
        }
        /// <summary>
        /// ��λʹ�ü�
        /// </summary>
        virtual public decimal UsedPrice
        {
            get { return usedPrice; }
            set { usedPrice = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        virtual public decimal UsableCapability
        {
            get { return usableCapability; }
            set { usableCapability = value; }
        }
        /// <summary>
        /// �����
        /// </summary>
        virtual public decimal Capability
        {
            get { return capability; }
            set { capability = value; }
        }
        /// <summary>
        /// ����λ��
        /// </summary>
        virtual public string Address
        {
            get { return address; }
            set { address = value; }
        }
        virtual public StationCategory Clone()
        {
            StationCategory theStationCategory = new StationCategory();
            theStationCategory.Address = this.Address;
            theStationCategory.Author = this.Author;
            theStationCategory.Capability = this.Capability;
            theStationCategory.CategoryNodeType = this.CategoryNodeType;
            theStationCategory.Code = this.Code;
            theStationCategory.CreateDate = DateTime.Today;
            theStationCategory.Describe = this.Describe;
            theStationCategory.Name = this.Name;
            theStationCategory.State = 1;
            theStationCategory.StationKind = this.StationKind;
            theStationCategory.TheTree = this.TheTree;
            theStationCategory.UsableCapability = this.UsableCapability;
            theStationCategory.UsedPrice = this.UsedPrice;
            return theStationCategory;
        }
        /// <summary>
        /// �޸�����
        /// </summary>
        virtual public DateTime UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value; }
        }
        /// <summary>
        /// ����޸���
        /// </summary>
        virtual public PersonInfo ModifyPerson
        {
            get { return modifyPerson; }
            set { modifyPerson = value; }
        }
        /// <summary>
        /// �����ˡ��޸�������ҵ���Ų����
        /// </summary>
        virtual public string OpgSysCode
        {
            get { return opgSysCode; }
            set { opgSysCode = value; }
        }
    }
}
