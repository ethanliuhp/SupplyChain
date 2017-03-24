using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain
{
    /// <summary>
    /// 工程资源耗用核算（工程任务明细分科目核算）
    /// </summary>
    [Serializable]
    public class ProjectTaskDetailAccountSubject
    {
        private string _id;
        private long _version;
        private string _theProjectGUID;
        private string _theProjectName;
        private string _costName;
        private decimal _accountQuantity;
        private decimal _accountTotalPrice;
        private decimal _accountPrice;
        private StandardUnit _quantityUnitGUID;
        private string _quantityUnitName;
        private StandardUnit _priceUnitGUID;
        private string _priceUnitName;
        private CostAccountSubject _costingSubjectGUID;
        private string _costingSubjectName;
        private string _resourceTypeGUID;
        private string _resourceTypeName;
        private string _resourceTypeQuality;
        private string _resourceTypeSpec;
        private ProjectTaskDetailAccount _theAccountDetail;


        private GWBSDetailCostSubject _bestaetigtCostSubjectGUID;
        private string _bestaetigtCostSubjectName;

        private MonthAccountFlag _monthAccFlag;

        private decimal _resContractQuantity;
        private decimal _contractQuantityPrice;
        private decimal _contractIncomeTotal;
        private decimal _contractQuotaNum;
        private decimal _contractProjectAmountPrice;

        private decimal _responsibleQuantity;
        private decimal _responsibleQnyPrice;
        private decimal _responsibleWorkQnyPrice;
        private decimal _responsibleUsageQny;
        private decimal _responsibleUsageTotal;

        private decimal _planQuantity;
        private decimal _planQnyPrice;
        private decimal _planWorkQnyPrice;
        private decimal _planUsageQny;
        private decimal _planUsageTotal;

        private decimal _accWorkQnyPrice;
        private decimal _accUsageQny;
        private decimal _currContractIncomeQny;
        private decimal _currContractIncomeTotal;
        private decimal _currResponsibleCostQny;
        private decimal _currResponsibleCostTotal;

        /// <summary>
        /// 月度核算成功标志
        /// </summary>
        public virtual MonthAccountFlag MonthAccFlag
        {
            get { return _monthAccFlag; }
            set { _monthAccFlag = value; }
        }

        /// <summary>
        /// 合同定额数量
        /// </summary>
        public virtual decimal ContractQuotaNum
        {
            get { return _contractQuotaNum; }
            set { _contractQuotaNum = value; }
        }
        /// <summary>
        /// 合同数量单价
        /// </summary>
        public virtual decimal ContractQuantityPrice
        {
            get { return _contractQuantityPrice; }
            set { _contractQuantityPrice = value; }
        }
        /// <summary>
        /// 合同工程量单价
        /// </summary>
        public virtual decimal ContractProjectAmountPrice
        {
            get { return _contractProjectAmountPrice; }
            set { _contractProjectAmountPrice = value; }
        }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public virtual decimal ResContractQuantity
        {
            get { return _resContractQuantity; }
            set { _resContractQuantity = value; }
        }
        /// <summary>
        /// 合同收入合价
        /// </summary>
        public virtual decimal ContractIncomeTotal
        {
            get { return _contractIncomeTotal; }
            set { _contractIncomeTotal = value; }
        }
        /// <summary>
        /// 责任定额数量
        /// </summary>
        public virtual decimal ResponsibleQuantity
        {
            get { return _responsibleQuantity; }
            set { _responsibleQuantity = value; }
        }
        /// <summary>
        /// 责任数量单价
        /// </summary>
        public virtual decimal ResponsibleQnyPrice
        {
            get { return _responsibleQnyPrice; }
            set { _responsibleQnyPrice = value; }
        }
        /// <summary>
        /// 责任工程量单价
        /// </summary>
        public virtual decimal ResponsibleWorkQnyPrice
        {
            get { return _responsibleWorkQnyPrice; }
            set { _responsibleWorkQnyPrice = value; }
        }
        /// <summary>
        /// 责任耗用数量
        /// </summary>
        public virtual decimal ResponsibleUsageQny
        {
            get { return _responsibleUsageQny; }
            set { _responsibleUsageQny = value; }
        }
        /// <summary>
        /// 责任耗用合价
        /// </summary>
        public virtual decimal ResponsibleUsageTotal
        {
            get { return _responsibleUsageTotal; }
            set { _responsibleUsageTotal = value; }
        }
        /// <summary>
        /// 计划定额数量
        /// </summary>
        public virtual decimal PlanQuantity
        {
            get { return _planQuantity; }
            set { _planQuantity = value; }
        }
        /// <summary>
        /// 计划数量单价
        /// </summary>
        public virtual decimal PlanQnyPrice
        {
            get { return _planQnyPrice; }
            set { _planQnyPrice = value; }
        }
        /// <summary>
        /// 计划工程量单价
        /// </summary>
        public virtual decimal PlanWorkQnyPrice
        {
            get { return _planWorkQnyPrice; }
            set { _planWorkQnyPrice = value; }
        }
        /// <summary>
        /// 计划耗用数量
        /// </summary>
        public virtual decimal PlanUsageQny
        {
            get { return _planUsageQny; }
            set { _planUsageQny = value; }
        }
        /// <summary>
        /// 计划耗用合价
        /// </summary>
        public virtual decimal PlanUsageTotal
        {
            get { return _planUsageTotal; }
            set { _planUsageTotal = value; }
        }
        /// <summary>
        /// 核算工程量单价
        /// </summary>
        public virtual decimal AccWorkQnyPrice
        {
            get { return _accWorkQnyPrice; }
            set { _accWorkQnyPrice = value; }
        }
        /// <summary>
        /// 核算消耗数量
        /// </summary>
        public virtual decimal AccUsageQny
        {
            get { return _accUsageQny; }
            set { _accUsageQny = value; }
        }
        /// <summary>
        /// 本次实现合同收入量
        /// </summary>
        public virtual decimal CurrContractIncomeQny
        {
            get { return _currContractIncomeQny; }
            set { _currContractIncomeQny = value; }
        }
        /// <summary>
        /// 本次实现合同收入合价
        /// </summary>
        public virtual decimal CurrContractIncomeTotal
        {
            get { return _currContractIncomeTotal; }
            set { _currContractIncomeTotal = value; }
        }
        /// <summary>
        /// 本次责任成本数量
        /// </summary>
        public virtual decimal CurrResponsibleCostQny
        {
            get { return _currResponsibleCostQny; }
            set { _currResponsibleCostQny = value; }
        }
        /// <summary>
        /// 本次责任成本合价
        /// </summary>
        public virtual decimal CurrResponsibleCostTotal
        {
            get { return _currResponsibleCostTotal; }
            set { _currResponsibleCostTotal = value; }
        }


        /// <summary>
        /// 业务前驱（工程资源耗用明细）
        /// </summary>
        public virtual GWBSDetailCostSubject BestaetigtCostSubjectGUID
        {
            get { return _bestaetigtCostSubjectGUID; }
            set { _bestaetigtCostSubjectGUID = value; }
        }
        /// <summary>
        /// 被确认工程任务明细分科目成本（工程资源耗用明细）名称
        /// </summary>
        public virtual string BestaetigtCostSubjectName
        {
            get { return _bestaetigtCostSubjectName; }
            set { _bestaetigtCostSubjectName = value; }
        }


        //删除属性
        private PersonInfo _contructResponsibleGUID;
        private string _contructResponsibleName;
        private string _contructResponsibleOrgSysCode;
        private SupplierRelationInfo _taskBearerGUID;
        private string _taskBearerName;
        /// <summary>
        /// 施工责任人GUID
        /// </summary>
        public virtual PersonInfo ContructResponsibleGUID
        {
            get { return _contructResponsibleGUID; }
            set { _contructResponsibleGUID = value; }
        }
        /// <summary>
        /// 施工责任人名称
        /// </summary>
        public virtual string ContructResponsibleName
        {
            get { return _contructResponsibleName; }
            set { _contructResponsibleName = value; }
        }
        /// <summary>
        /// 施工责任人组织层次码
        /// </summary>
        public virtual string ContructResponsibleOrgSysCode
        {
            get { return _contructResponsibleOrgSysCode; }
            set { _contructResponsibleOrgSysCode = value; }
        }
        /// <summary>
        /// 任务承担者GUID
        /// </summary>
        public virtual SupplierRelationInfo TaskBearerGUID
        {
            get { return _taskBearerGUID; }
            set { _taskBearerGUID = value; }
        }
        /// <summary>
        /// 任务承担者名称
        /// </summary>
        public virtual string TaskBearerName
        {
            get { return _taskBearerName; }
            set { _taskBearerName = value; }
        }


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
        /// <summary>
        /// 成本名称
        /// </summary>
        public virtual string CostName
        {
            get { return _costName; }
            set { _costName = value; }
        }
        /// <summary>
        /// 核算定额数量
        /// </summary>
        public virtual decimal AccountQuantity
        {
            get { return _accountQuantity; }
            set { _accountQuantity = value; }
        }
        /// <summary>
        /// 核算数量单价
        /// </summary>
        public virtual decimal AccountPrice
        {
            get { return _accountPrice; }
            set { _accountPrice = value; }
        }
        /// <summary>
        /// 核算合价
        /// </summary>
        public virtual decimal AccountTotalPrice
        {
            get { return _accountTotalPrice; }
            set { _accountTotalPrice = value; }
        }
        /// <summary>
        /// 数量计量单位GUID
        /// </summary>
        public virtual StandardUnit QuantityUnitGUID
        {
            get { return _quantityUnitGUID; }
            set { _quantityUnitGUID = value; }
        }
        /// <summary>
        /// 数量计量单位名称
        /// </summary>
        public virtual string QuantityUnitName
        {
            get { return _quantityUnitName; }
            set { _quantityUnitName = value; }
        }
        /// <summary>
        /// 单价计量单位GUID
        /// </summary>
        public virtual StandardUnit PriceUnitGUID
        {
            get { return _priceUnitGUID; }
            set { _priceUnitGUID = value; }
        }
        /// <summary>
        /// 单价计量单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return _priceUnitName; }
            set { _priceUnitName = value; }
        }
        /// <summary>
        /// 成本核算科目
        /// </summary>
        public virtual CostAccountSubject CostingSubjectGUID
        {
            get { return _costingSubjectGUID; }
            set { _costingSubjectGUID = value; }
        }
        /// <summary>
        /// 核算科目名称
        /// </summary>
        public virtual string CostingSubjectName
        {
            get { return _costingSubjectName; }
            set { _costingSubjectName = value; }
        }
        /// <summary>
        /// 资源类型GUID
        /// </summary>
        public virtual string ResourceTypeGUID
        {
            get { return _resourceTypeGUID; }
            set { _resourceTypeGUID = value; }
        }
        /// <summary>
        /// 资源类型名称
        /// </summary>
        public virtual string ResourceTypeName
        {
            get { return _resourceTypeName; }
            set { _resourceTypeName = value; }
        }
        /// <summary>
        /// 材质
        /// </summary>
        public virtual string ResourceTypeQuality
        {
            get { return _resourceTypeQuality; }
            set { _resourceTypeQuality = value; }
        }
        /// <summary>
        /// 规格
        /// </summary>
        public virtual string ResourceTypeSpec
        {
            get { return _resourceTypeSpec; }
            set { _resourceTypeSpec = value; }
        }
        /// <summary>
        /// 工程任务明细核算
        /// </summary>
        public virtual ProjectTaskDetailAccount TheAccountDetail
        {
            get { return _theAccountDetail; }
            set { _theAccountDetail = value; }
        }
    }

    /// <summary>
    /// 月度核算成功标志
    /// </summary>
    public enum MonthAccountFlag
    {
        [Description("未结算")]
        未结算 = 0,
        [Description("成功")]
        成功 = 1,
        [Description("结算失败")]
        结算失败 = 2
    }
}
