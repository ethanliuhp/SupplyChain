using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
//using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain
{
    /// <summary>
    /// 罚扣款单明细
    /// </summary>
    [Serializable]
    public class PenaltyDeductionDetail : BaseDetail
    {
        private decimal penaltyQuantity;
        private decimal penaltyMoney;
        private string penaltySubject;
        private CostAccountSubject penaltySubjectGUID;
        private string penaltySysCode;
        private StandardUnit productUnit;
        private string productUnitName;
        private GWBSTree projectTask;
        private string projectTaskName;
        private string projectTaskSyscode;
        private GWBSDetail projectTaskDetail;
        private string projectDetailSysCode;
        private decimal accountPrice;
        private decimal accountQuantity;
        private decimal accountMoney;
        private EnumSettlementType accountState;
        private StandardUnit moneyUnit;
        private string moneyUnitName;
        private string taskDetailName;
        private string cause;
        private DateTime businessDate;
        private string _balanceDtlGUID;
        private string penaltyType;
        private Material resourceType;
        private string resourceTypeName;
        private string resourceTypeSpec;
        private string resourceTypeStuff;
        private string resourceSysCode;
        private LaborSporadicDetail laborDetailGUID;
        
        /// <summary>
        /// 关联的零星用工明细
        /// </summary>
        virtual public LaborSporadicDetail LaborDetailGUID
        {
            get { return laborDetailGUID; }
            set { laborDetailGUID = value; }
        }

        /// <summary>
        /// 科目层次码
        /// </summary>
        virtual public string PenaltySysCode
        {
            get { return penaltySysCode; }
            set { penaltySysCode = value; }
        }
        /// <summary>
        /// 工程任务明细层次码
        /// </summary>
        virtual public string ProjectDetailSysCode
        {
            get { return projectDetailSysCode; }
            set { projectDetailSysCode = value; }
        }


        /// <summary>
        /// 资源类型
        /// </summary>
        virtual public Material ResourceType
        {
            get { return resourceType; }
            set { resourceType = value; }
        }
        /// <summary>
        /// 资源类型名称
        /// </summary>
        virtual public string ResourceTypeName
        {
            get { return resourceTypeName; }
            set { resourceTypeName = value; }
        }

        /// <summary>
        /// 资源规格
        /// </summary>
        public virtual string ResourceTypeSpec
        {
            get { return resourceTypeSpec; }
            set { resourceTypeSpec = value; }
        }

        /// <summary>
        /// 资源材质
        /// </summary>
        public virtual string ResourceTypeStuff
        {
            get { return resourceTypeStuff; }
            set { resourceTypeStuff = value; }
        }

        /// <summary>
        /// 资源层次码
        /// </summary>
        public virtual string ResourceSysCode
        {
            get { return resourceSysCode; }
            set { resourceSysCode = value; }
        }

        /// <summary>
        /// 罚款类型
        /// </summary>
        virtual public string PenaltyType
        {
            get { return penaltyType; }
            set { penaltyType = value; }
        }

        /// <summary>
        /// 金额计量单位
        /// </summary>
        virtual public StandardUnit MoneyUnit
        {
            get { return moneyUnit; }
            set { moneyUnit = value; }
        }
        /// <summary>
        /// 金额计量单位名称
        /// </summary>
        virtual public string MoneyUnitName
        {
            get { return moneyUnitName; }
            set { moneyUnitName = value; }
        }
        /// <summary>
        /// 任务明细名称
        /// </summary>
        virtual public string TaskDetailName
        {
            get { return taskDetailName; }
            set { taskDetailName = value; }
        }
        /// <summary>
        /// 事由
        /// </summary>
        virtual public string Cause
        {
            get { return cause; }
            set { cause = value; }
        }
        /// <summary>
        /// 业务发生时间
        /// </summary>
        virtual public DateTime BusinessDate
        {
            get { return businessDate; }
            set { businessDate = value; }
        }
        /// <summary>
        /// 核算单价
        /// </summary>
        virtual public decimal AccountPrice
        {
            get { return accountPrice; }
            set { accountPrice = value; }
        }
        /// <summary>
        /// 核算工程量
        /// </summary>
        virtual public decimal AccountQuantity
        {
            get { return accountQuantity; }
            set { accountQuantity = value; }
        }
        /// <summary>
        /// 核算金额
        /// </summary>
        virtual public decimal AccountMoney
        {
            get { return accountMoney; }
            set { accountMoney = value; }
        }
        /// <summary>
        /// 结算状态
        /// </summary>
        virtual public EnumSettlementType AccountState
        {
            get { return accountState; }
            set { accountState = value; }
        }
        /// <summary>
        /// 罚扣工程量
        /// </summary>
        virtual public decimal PenaltyQuantity
        {
            get { return penaltyQuantity; }
            set { penaltyQuantity = value; }
        }
        /// <summary>
        /// 罚扣工程金额
        /// </summary>
        virtual public decimal PenaltyMoney
        {
            get { return penaltyMoney; }
            set { penaltyMoney = value; }
        }
        /// <summary>
        /// 罚扣款科目
        /// </summary>
        virtual public CostAccountSubject PenaltySubjectGUID
        {
            get { return penaltySubjectGUID; }
            set { penaltySubjectGUID = value; }
        }
        /// <summary>
        /// 罚扣款科目名称
        /// </summary>
        virtual public string PenaltySubject
        {
            get { return penaltySubject; }
            set { penaltySubject = value; }
        }
        /// <summary>
        /// 工程计量单位
        /// </summary>
        virtual public StandardUnit ProductUnit
        {
            get { return productUnit; }
            set { productUnit = value; }
        }
        /// <summary>
        /// 工程计量单位名称
        /// </summary>
        virtual public string ProductUnitName
        {
            get { return productUnitName; }
            set { productUnitName = value; }
        }
        /// <summary>
        /// 工程项目任务
        /// </summary>
        virtual public GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        /// <summary>
        /// 工程项目任务名称
        /// </summary>
        virtual public string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        /// <summary>
        /// 工程项目任务层次码
        /// </summary>
        virtual public string ProjectTaskSyscode
        {
            get { return projectTaskSyscode; }
            set { projectTaskSyscode = value; }
        }
        /// <summary>
        /// 工程项目任务明细
        /// </summary>
        virtual public GWBSDetail ProjectTaskDetail
        {
            get { return projectTaskDetail; }
            set { projectTaskDetail = value; }
        }

        /// <summary>
        /// 结算明细GUID
        /// </summary>
        public virtual string BalanceDtlGUID
        {
            get { return _balanceDtlGUID; }
            set { _balanceDtlGUID = value; }
        }
    }
}
