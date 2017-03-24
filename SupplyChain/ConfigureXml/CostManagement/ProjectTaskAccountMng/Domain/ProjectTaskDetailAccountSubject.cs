using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain
{
    /// <summary>
    /// 工程任务明细分科目核算
    /// </summary>
    [Serializable]
    public class ProjectTaskDetailAccountSubject
    {
        private string _id;
        private long _version;
        private string _theProjectGUID;
        private string _theProjectName;

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        public virtual long Version
        {
            get { return _version; }
            set { _version = value; }
        }
        /// <summary>
        /// 项目GUID
        /// </summary>
        public virtual string TheProjectGUID
        {
            get { return _theProjectGUID; }
            set { _theProjectGUID = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string TheProjectName
        {
            get { return _theProjectName; }
            set { _theProjectName = value; }
        }

        private string _costName;
        /// <summary>
        /// 成本名称
        /// </summary>
        public virtual string CostName
        {
            get { return _costName; }
            set { _costName = value; }
        }
        private string _bestaetigtCostSubjectGUID;
        /// <summary>
        /// 被确认工程任务明细分科目成本GUID
        /// </summary>
        public virtual string BestaetigtCostSubjectGUID
        {
            get { return _bestaetigtCostSubjectGUID; }
            set { _bestaetigtCostSubjectGUID = value; }
        }
        private string _bestaetigtCostSubjectName;
        /// <summary>
        /// 被确认工程任务明细分科目成本名称
        /// </summary>
        public virtual string BestaetigtCostSubjectName
        {
            get { return _bestaetigtCostSubjectName; }
            set { _bestaetigtCostSubjectName = value; }
        }
        private PersonInfo _contructResponsibleGUID;
        /// <summary>
        /// 施工责任人GUID
        /// </summary>
        public virtual PersonInfo ContructResponsibleGUID
        {
            get { return _contructResponsibleGUID; }
            set { _contructResponsibleGUID = value; }
        }
        private string _contructResponsibleName;
        /// <summary>
        /// 施工责任人名称
        /// </summary>
        public virtual string ContructResponsibleName
        {
            get { return _contructResponsibleName; }
            set { _contructResponsibleName = value; }
        }
        private string _contructResponsibleOrgSysCode;
        /// <summary>
        /// 施工责任人组织层次码
        /// </summary>
        public virtual string ContructResponsibleOrgSysCode
        {
            get { return _contructResponsibleOrgSysCode; }
            set { _contructResponsibleOrgSysCode = value; }
        }
        private decimal _accountQuantity;
        /// <summary>
        /// 核算数量
        /// </summary>
        public virtual decimal AccountQuantity
        {
            get { return _accountQuantity; }
            set { _accountQuantity = value; }
        }
        private decimal _accountTotalPrice;
        /// <summary>
        /// 核算合价
        /// </summary>
        public virtual decimal AccountTotalPrice
        {
            get { return _accountTotalPrice; }
            set { _accountTotalPrice = value; }
        }
        private SupplierRelationInfo _taskBearerGUID;
        /// <summary>
        /// 任务承担者GUID
        /// </summary>
        public virtual SupplierRelationInfo TaskBearerGUID
        {
            get { return _taskBearerGUID; }
            set { _taskBearerGUID = value; }
        }
        private string _taskBearerName;
        /// <summary>
        /// 任务承担者名称
        /// </summary>
        public virtual string TaskBearerName
        {
            get { return _taskBearerName; }
            set { _taskBearerName = value; }
        }
        private decimal _accountPrice;
        /// <summary>
        /// 核算单价
        /// </summary>
        public virtual decimal AccountPrice
        {
            get { return _accountPrice; }
            set { _accountPrice = value; }
        }
        private StandardUnit _quantityUnitGUID;
        /// <summary>
        /// 数量计量单位GUID
        /// </summary>
        public virtual StandardUnit QuantityUnitGUID
        {
            get { return _quantityUnitGUID; }
            set { _quantityUnitGUID = value; }
        }
        private string _quantityUnitName;
        /// <summary>
        /// 数量计量单位名称
        /// </summary>
        public virtual string QuantityUnitName
        {
            get { return _quantityUnitName; }
            set { _quantityUnitName = value; }
        }
        private StandardUnit _priceUnitGUID;
        /// <summary>
        /// 单价计量单位GUID
        /// </summary>
        public virtual StandardUnit PriceUnitGUID
        {
            get { return _priceUnitGUID; }
            set { _priceUnitGUID = value; }
        }
        private string _priceUnitName;
        /// <summary>
        /// 单价计量单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return _priceUnitName; }
            set { _priceUnitName = value; }
        }
        private CostAccountSubject _costingSubjectGUID;
        /// <summary>
        /// 成本核算科目
        /// </summary>
        public virtual CostAccountSubject CostingSubjectGUID
        {
            get { return _costingSubjectGUID; }
            set { _costingSubjectGUID = value; }
        }
        private string _costingSubjectName;
        /// <summary>
        /// 核算科目名称
        /// </summary>
        public virtual string CostingSubjectName
        {
            get { return _costingSubjectName; }
            set { _costingSubjectName = value; }
        }
        private string _resourceTypeGUID;
        /// <summary>
        /// 资源类型GUID
        /// </summary>
        public virtual string ResourceTypeGUID
        {
            get { return _resourceTypeGUID; }
            set { _resourceTypeGUID = value; }
        }
        private string _resourceTypeName;
        /// <summary>
        /// 资源类型名称
        /// </summary>
        public virtual string ResourceTypeName
        {
            get { return _resourceTypeName; }
            set { _resourceTypeName = value; }
        }

        private ProjectTaskDetailAccount _theAccountDetail;
        /// <summary>
        /// 工程任务明细核算
        /// </summary>
        public virtual ProjectTaskDetailAccount TheAccountDetail
        {
            get { return _theAccountDetail; }
            set { _theAccountDetail = value; }
        }
    }
}
