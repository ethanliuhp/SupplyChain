using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using System.ComponentModel;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain
{
    /// <summary>
    /// 工程任务明细核算
    /// </summary>
    [Serializable]
    public class ProjectTaskDetailAccount
    {
        private string _id;
        private long _version;
        private string _theProjectGUID;
        private string _theProjectName;

        private GWBSTree _accountTaskNodeGUID;
        private string _accountTaskNodeName;
        private string _accountTaskNodeSyscode;
        private decimal _accountProjectAmount;
        private decimal _accountPrice;
        private decimal _accountTotalPrice;
        private ProjectTaskDetailAccountState _balanceState = ProjectTaskDetailAccountState.未结算;
        private GWBSDetail _projectTaskDtlGUID;
        private string _projectTaskDtlName;
        private StandardUnit _quantityUnitGUID;
        private string _quantityUnitName;
        private StandardUnit _priceUnitGUID;
        private string _priceUnitName;
        private ProjectTaskAccountBill _theAccountBill;
        private string _balanceDtlGUID;
        private string _remark;
        private ISet<ProjectTaskDetailAccountSubject> _Details = new HashedSet<ProjectTaskDetailAccountSubject>();

        private string _accTaskNodeSysCode;
        private decimal _contractQuantity;
        private decimal _contractPrice;
        private decimal _contractTotalPrice;
        private decimal _responsibleQuantity;
        private decimal _responsiblePrice;
        private decimal _responsibleTotalPrice;
        private decimal _planQuantity;
        private decimal _planPrice;
        private decimal _planTotalPrice;
        private decimal _confirmQuantity;
        private decimal _currAccFigureProgress;
        private decimal _currAccEV;
        private decimal _currContractIncomeQny;
        private decimal _currContractIncomeTotal;
        private decimal _currResponsibleCostQny;
        private decimal _currResponsibleCostTotal;
        private PersonInfo _responsiblePerson;
        private string _responsiblePersonName;

        /// <summary>
        /// 所属GWBS层次码
        /// </summary>
        public virtual string AccTaskNodeSysCode
        {
            get { return _accTaskNodeSysCode; }
            set { _accTaskNodeSysCode = value; }
        }
        /// <summary>
        /// 合同工程量
        /// </summary>
        public virtual decimal ContractQuantity
        {
            get { return _contractQuantity; }
            set { _contractQuantity = value; }
        }
        /// <summary>
        /// 合同单价
        /// </summary>
        public virtual decimal ContractPrice
        {
            get { return _contractPrice; }
            set { _contractPrice = value; }
        }
        /// <summary>
        /// 合同合价
        /// </summary>
        public virtual decimal ContractTotalPrice
        {
            get { return _contractTotalPrice; }
            set { _contractTotalPrice = value; }
        }
        /// <summary>
        /// 责任工程量
        /// </summary>
        public virtual decimal ResponsibleQuantity
        {
            get { return _responsibleQuantity; }
            set { _responsibleQuantity = value; }
        }
        /// <summary>
        /// 责任单价
        /// </summary>
        public virtual decimal ResponsiblePrice
        {
            get { return _responsiblePrice; }
            set { _responsiblePrice = value; }
        }
        /// <summary>
        /// 责任合价
        /// </summary>
        public virtual decimal ResponsibleTotalPrice
        {
            get { return _responsibleTotalPrice; }
            set { _responsibleTotalPrice = value; }
        }
        /// <summary>
        /// 计划工程量
        /// </summary>
        public virtual decimal PlanQuantity
        {
            get { return _planQuantity; }
            set { _planQuantity = value; }
        }
        /// <summary>
        /// 计划单价
        /// </summary>
        public virtual decimal PlanPrice
        {
            get { return _planPrice; }
            set { _planPrice = value; }
        }
        /// <summary>
        /// 计划合价
        /// </summary>
        public virtual decimal PlanTotalPrice
        {
            get { return _planTotalPrice; }
            set { _planTotalPrice = value; }
        }
        /// <summary>
        /// 确认工程量
        /// </summary>
        public virtual decimal ConfirmQuantity
        {
            get { return _confirmQuantity; }
            set { _confirmQuantity = value; }
        }
        /// <summary>
        /// 本次核算形象进度
        /// </summary>
        public virtual decimal CurrAccFigureProgress
        {
            get { return _currAccFigureProgress; }
            set { _currAccFigureProgress = value; }
        }
        /// <summary>
        /// 本次核算挣值
        /// </summary>
        public virtual decimal CurrAccEV
        {
            get { return _currAccEV; }
            set { _currAccEV = value; }
        }
        /// <summary>
        /// 本次合同收入实现量
        /// </summary>
        public virtual decimal CurrContractIncomeQny
        {
            get { return _currContractIncomeQny; }
            set { _currContractIncomeQny = value; }
        }
        /// <summary>
        /// 本次合同收入合价
        /// </summary>
        public virtual decimal CurrContractIncomeTotal
        {
            get { return _currContractIncomeTotal; }
            set { _currContractIncomeTotal = value; }
        }
        /// <summary>
        /// 本次责任成本实现量
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
        /// 责任人
        /// </summary>
        public virtual PersonInfo ResponsiblePerson
        {
            get { return _responsiblePerson; }
            set { _responsiblePerson = value; }
        }
        /// <summary>
        /// 责任人名字
        /// </summary>
        public virtual string ResponsiblePersonName
        {
            get { return _responsiblePersonName; }
            set { _responsiblePersonName = value; }
        }

        //修改的属性
        private SubContractProject _bearerGUID;
        private string _bearerName;
        private SupplierRelationInfo _bearerOrg;

        //移除的属性
        private CostItem _theCostItem;
        private string _costItemName;
        /// <summary>
        /// 成本项
        /// </summary>
        public virtual CostItem TheCostItem
        {
            get { return _theCostItem; }
            set { _theCostItem = value; }
        }
        /// <summary>
        /// 成本项名称
        /// </summary>
        public virtual string CostItemName
        {
            get { return _costItemName; }
            set { _costItemName = value; }
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
        /// 所属工程项目任务（核算任务节点）
        /// </summary>
        public virtual GWBSTree AccountTaskNodeGUID
        {
            get { return _accountTaskNodeGUID; }
            set { _accountTaskNodeGUID = value; }
        }
        /// <summary>
        /// 核算任务节点名称
        /// </summary>
        public virtual string AccountTaskNodeName
        {
            get { return _accountTaskNodeName; }
            set { _accountTaskNodeName = value; }
        }
        /// <summary>
        /// 核算任务节点层次码
        /// </summary>
        public virtual string AccountTaskNodeSyscode
        {
            get { return _accountTaskNodeSyscode; }
            set { _accountTaskNodeSyscode = value; }
        }

        /// <summary>
        /// 核算工程量
        /// </summary>
        public virtual decimal AccountProjectAmount
        {
            get { return _accountProjectAmount; }
            set { _accountProjectAmount = value; }
        }
        /// <summary>
        /// 核算单价
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
        /// 结算状态
        /// </summary>
        public virtual ProjectTaskDetailAccountState BalanceState
        {
            get { return _balanceState; }
            set { _balanceState = value; }
        }
        /// <summary>
        /// 工程任务明细
        /// </summary>
        public virtual GWBSDetail ProjectTaskDtlGUID
        {
            get { return _projectTaskDtlGUID; }
            set { _projectTaskDtlGUID = value; }
        }
        /// <summary>
        /// 工程任务明细名称
        /// </summary>
        public virtual string ProjectTaskDtlName
        {
            get { return _projectTaskDtlName; }
            set { _projectTaskDtlName = value; }
        }
        /// <summary>
        /// 分包项目
        /// </summary>
        public virtual SubContractProject BearerGUID
        {
            get { return _bearerGUID; }
            set { _bearerGUID = value; }
        }
        /// <summary>
        /// 承担组织
        /// </summary>
        public virtual SupplierRelationInfo BearerOrg
        {
            get { return _bearerOrg; }
            set { _bearerOrg = value; }
        }
        /// <summary>
        /// 承担者名称
        /// </summary>
        public virtual string BearerName
        {
            get { return _bearerName; }
            set { _bearerName = value; }
        }
        /// <summary>
        /// 数量单位
        /// </summary>
        public virtual StandardUnit QuantityUnitGUID
        {
            get { return _quantityUnitGUID; }
            set { _quantityUnitGUID = value; }
        }
        /// <summary>
        /// 数量单位名称
        /// </summary>
        public virtual string QuantityUnitName
        {
            get { return _quantityUnitName; }
            set { _quantityUnitName = value; }
        }
        /// <summary>
        /// 价格单位
        /// </summary>
        public virtual StandardUnit PriceUnitGUID
        {
            get { return _priceUnitGUID; }
            set { _priceUnitGUID = value; }
        }
        /// <summary>
        /// 价格单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return _priceUnitName; }
            set { _priceUnitName = value; }
        }
        /// <summary>
        /// 结算明细GUID
        /// </summary>
        public virtual string BalanceDtlGUID
        {
            get { return _balanceDtlGUID; }
            set { _balanceDtlGUID = value; }
        }
        /// <summary>
        /// 核算单
        /// </summary>
        public virtual ProjectTaskAccountBill TheAccountBill
        {
            get { return _theAccountBill; }
            set { _theAccountBill = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// 工程资源耗用核算（工程任务明细分科目核算）
        /// </summary>
        public virtual ISet<ProjectTaskDetailAccountSubject> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
    }
    /// <summary>
    /// 工程任务明细核算状态
    /// </summary>
    public enum ProjectTaskDetailAccountState
    {
        [Description("不需结算")]
        不需结算 = 1,
        [Description("未结算")]
        未结算 = 2,
        [Description("已结算")]
        已结算 = 3
    }
}
