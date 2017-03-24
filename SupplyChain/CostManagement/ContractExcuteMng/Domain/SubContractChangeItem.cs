using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain
{
    /// <summary>
    /// 分包合同变更项
    /// </summary>
    [Serializable]
    public class SubContractChangeItem
    {
        private string id;
        private long version;
        private string _projectId;
        private string _projectName;
        private string _changeDesc;
        private decimal _changeMoney;
        private StandardUnit _priceUnit;
        private string _priceUnitName;
        private string _contractType;
        private string _contractCode;
        private string _contractName;
        private ContractGroup _theContractGroup;
        private SubContractProject _theProject;

        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
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
        /// 所属项目ID
        /// </summary>
        public virtual string ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }
        /// <summary>
        /// 所属项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }
        /// <summary>
        /// 变更说明
        /// </summary>
        public virtual string ChangeDesc
        {
            get { return _changeDesc; }
            set { _changeDesc = value; }
        }
        /// <summary>
        /// 变更金额
        /// </summary>
        public virtual decimal ChangeMoney
        {
            get { return _changeMoney; }
            set { _changeMoney = value; }
        }
        /// <summary>
        /// 价格计量单位
        /// </summary>
        public virtual StandardUnit PriceUnit
        {
            get { return _priceUnit; }
            set { _priceUnit = value; }
        }
        /// <summary>
        /// 价格计量单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return _priceUnitName; }
            set { _priceUnitName = value; }
        }
        /// <summary>
        /// 契约类型
        /// </summary>
        public virtual string ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
        }
        /// <summary>
        /// 契约编号
        /// </summary>
        public virtual string ContractCode
        {
            get { return _contractCode; }
            set { _contractCode = value; }
        }
        /// <summary>
        /// 契约名称
        /// </summary>
        public virtual string ContractName
        {
            get { return _contractName; }
            set { _contractName = value; }
        }
        /// <summary>
        /// 依据契约
        /// </summary>
        public virtual ContractGroup TheContractGroup
        {
            get { return _theContractGroup; }
            set { _theContractGroup = value; }
        }
        /// <summary>
        /// 分包项目
        /// </summary>
        public virtual SubContractProject TheProject
        {
            get { return _theProject; }
            set { _theProject = value; }
        }
    }
}
