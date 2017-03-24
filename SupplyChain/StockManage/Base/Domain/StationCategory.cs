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
    /// 仓位树
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
        /// 项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        /// <summary>
        /// 项目ID
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
        /// 仓库业务类型(0 销售 ,1 厂商银)
        /// </summary>
        virtual public int BusinessType
        {
            get { return businessType; }
            set { businessType = value; }
        }
        /// <summary>
        /// 创建人
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
        /// 仓位类型(0 仓库 ,1 仓位)
        /// </summary>
        virtual public int StationKind
        {
            get { return stationKind; }
            set { stationKind = value; }
        }
        /// <summary>
        /// 业务组织
        /// </summary>
        virtual public OperationOrgInfo OperOrgInfo
        {
            get { return operOrgInfo; }
            set { operOrgInfo = value; }
        }
        /// <summary>
        /// 单位使用价
        /// </summary>
        virtual public decimal UsedPrice
        {
            get { return usedPrice; }
            set { usedPrice = value; }
        }
        /// <summary>
        /// 可用容量
        /// </summary>
        virtual public decimal UsableCapability
        {
            get { return usableCapability; }
            set { usableCapability = value; }
        }
        /// <summary>
        /// 额定容量
        /// </summary>
        virtual public decimal Capability
        {
            get { return capability; }
            set { capability = value; }
        }
        /// <summary>
        /// 所在位置
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
        /// 修改日期
        /// </summary>
        virtual public DateTime UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value; }
        }
        /// <summary>
        /// 最后修改人
        /// </summary>
        virtual public PersonInfo ModifyPerson
        {
            get { return modifyPerson; }
            set { modifyPerson = value; }
        }
        /// <summary>
        /// 创建人、修改人所属业务部门层次码
        /// </summary>
        virtual public string OpgSysCode
        {
            get { return opgSysCode; }
            set { opgSysCode = value; }
        }
    }
}
