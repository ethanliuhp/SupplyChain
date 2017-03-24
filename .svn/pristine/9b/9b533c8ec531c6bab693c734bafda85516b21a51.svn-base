using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 月度资源耗用明细核算
    /// </summary>
    [Serializable]
    public class CostMonthAccDtlConsume
    {
        private string _id;
        private StandardUnit _rationUnitGUID;
        private string _rationUnitName;
        private CostAccountSubject _costingSubjectGUID;
        private string _costSubjectCode;
        private string _costSubjectSyscode;
        private string _costingSubjectName;
        private string _resourceTypeGUID;
        private string _resourceTypeName;
        private string _resourceTypeCode;
        private string _resourceTypeStuff;
        private string _resourceTypeSpec;
        private string _resourceSyscode;
        private string _accountTaskNodeGUID;
        private string _accountTaskNodeName;
        private string _accountTaskNodeSyscode;
        private int calType; //计算类型
        private CostMonthAccountDtl _theAccountDetail;
        //当期耗用
        private decimal currRealConsumeQuantity;
        private decimal currRealConsumePrice;
        private decimal currRealConsumeTotalPrice;
        private decimal currRealConsumePlanPrice;
        private decimal currRealConsumePlanQuantity;
        private decimal currRealConsumePlanTotalPrice;
        private decimal currIncomeQuantity;
        private decimal currIncomePrice;
        private decimal currIncomeTotalPrice;
        private decimal currResponsiConsumeQuantity;
        private decimal currResponsiConsumePrice;
        private decimal currResponsiConsumeTotalPrice;
        //累计耗用
        private decimal sumRealConsumeQuantity;
        private decimal sumRealConsumeTotalPrice;
        private decimal sumRealConsumePlanQuantity;
        private decimal sumRealConsumePlanTotalPrice;
        private decimal sumIncomeQuantity;
        private decimal sumIncomeTotalPrice;
        private decimal sumResponsiConsumeQuantity;
        private decimal sumResponsiConsumeTotalPrice;
        private string projectTaskDtlGUID;
        private string projectTaskDtlName; //传递数据用
        private string data1;
        private string data2;
        private string data3;

        private string diagramNumber;

        #region 记录预算体系中状态值

        private decimal budgetContractQuantity;
        private decimal budgetContractTotalPrice;
        private decimal budgetResponsibilitilyQuantity;
        private decimal budgetResponsibilitilyTotalPrice;
        private decimal budgetPlanQuantity;
        private decimal budgetPlanTotalPrice;

        /// <summary>
        /// 预算体系中合同数量
        /// </summary>
        public virtual decimal DudgetContractQuantity
        {
            get { return budgetContractQuantity; }
            set { budgetContractQuantity = value; }
        }

        /// <summary>
        /// 预算体系中合同合价
        /// </summary>
        public virtual decimal DudgetContractTotalPrice
        {
            get { return budgetContractTotalPrice; }
            set { budgetContractTotalPrice = value; }
        }

        /// <summary>
        /// 预算体系中责任数量
        /// </summary>
        public virtual decimal DudgetRespQuantity
        {
            get { return budgetResponsibilitilyQuantity; }
            set { budgetResponsibilitilyQuantity = value; }
        }

        /// <summary>
        /// 预算体系中责任合价
        /// </summary>
        public virtual decimal DudgetRespTotalPrice
        {
            get { return budgetResponsibilitilyTotalPrice; }
            set { budgetResponsibilitilyTotalPrice = value; }
        }

        /// <summary>
        /// 预算体系中计划数量
        /// </summary>
        public virtual decimal DudgetPlanQuantity
        {
            get { return budgetPlanQuantity; }
            set { budgetPlanQuantity = value; }
        }

        /// <summary>
        /// 预算体系中计划合价
        /// </summary>
        public virtual decimal DudgetPlanTotalPrice
        {
            get { return budgetPlanTotalPrice; }
            set { budgetPlanTotalPrice = value; }
        }

        #endregion

        /// <summary>
        /// 图号
        /// </summary>
        public virtual string DiagramNumber
        {
            get { return diagramNumber; }
            set { diagramNumber = value; }
        }

        /// <summary>
        /// 传递数据用
        /// </summary>
        public virtual string Data1
        {
            get { return data1; }
            set { data1 = value; }
        }

        /// <summary>
        /// 传递数据用
        /// </summary>
        public virtual string Data2
        {
            get { return data2; }
            set { data2 = value; }
        }

        /// <summary>
        /// 传递数据用
        /// </summary>
        public virtual string Data3
        {
            get { return data3; }
            set { data3 = value; }
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
        /// 定额计量单位GUID
        /// </summary>
        public virtual StandardUnit RationUnitGUID
        {
            get { return _rationUnitGUID; }
            set { _rationUnitGUID = value; }
        }

        /// <summary>
        /// 定额计量单位名称
        /// </summary>
        public virtual string RationUnitName
        {
            get { return _rationUnitName; }
            set { _rationUnitName = value; }
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
        /// 核算科目编码
        /// </summary>
        public virtual string CostSubjectCode
        {
            get { return _costSubjectCode; }
            set { _costSubjectCode = value; }
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
        /// 成本核算科目层次码
        /// </summary>
        public virtual string CostSubjectSyscode
        {
            get { return _costSubjectSyscode; }
            set { _costSubjectSyscode = value; }
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
        /// 资源类型编码
        /// </summary>
        public virtual string ResourceTypeCode
        {
            get { return _resourceTypeCode; }
            set { _resourceTypeCode = value; }
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
        /// 资源类型材质
        /// </summary>
        public virtual string ResourceTypeStuff
        {
            get { return _resourceTypeStuff; }
            set { _resourceTypeStuff = value; }
        }

        /// <summary>
        /// 资源类型规格
        /// </summary>
        public virtual string ResourceTypeSpec
        {
            get { return _resourceTypeSpec; }
            set { _resourceTypeSpec = value; }
        }

        /// <summary>
        /// 资源类型层次码
        /// </summary>
        public virtual string ResourceSyscode
        {
            get { return _resourceSyscode; }
            set { _resourceSyscode = value; }
        }

        /// <summary>
        /// 核算工程任务节点
        /// </summary>
        public virtual string AccountTaskNodeGUID
        {
            get { return _accountTaskNodeGUID; }
            set { _accountTaskNodeGUID = value; }
        }

        /// <summary>
        /// 核算工程任务节点名称
        /// </summary>
        public virtual string AccountTaskNodeName
        {
            get { return _accountTaskNodeName; }
            set { _accountTaskNodeName = value; }
        }

        /// <summary>
        /// 工程任务节点层次码
        /// </summary>
        public virtual string AccountTaskNodeSyscode
        {
            get { return _accountTaskNodeSyscode; }
            set { _accountTaskNodeSyscode = value; }
        }

        /// <summary>
        /// 计算类型
        /// </summary>
        public virtual int CalType
        {
            get { return calType; }
            set { calType = value; }
        }

        /// <summary>
        /// 工程任务明细核算
        /// </summary>
        public virtual CostMonthAccountDtl TheAccountDetail
        {
            get { return _theAccountDetail; }
            set { _theAccountDetail = value; }
        }

        /// <summary>
        /// 当期实际耗用数量
        /// </summary>
        public virtual decimal CurrRealConsumeQuantity
        {
            get { return currRealConsumeQuantity; }
            set { currRealConsumeQuantity = value; }
        }

        /// <summary>
        /// 当期实际耗用单价
        /// </summary>
        public virtual decimal CurrRealConsumePrice
        {
            get { return currRealConsumePrice; }
            set { currRealConsumePrice = value; }
        }

        /// <summary>
        /// 当期实际耗用合价
        /// </summary>
        public virtual decimal CurrRealConsumeTotalPrice
        {
            get { return currRealConsumeTotalPrice; }
            set { currRealConsumeTotalPrice = value; }
        }

        /// <summary>
        /// 当期实际耗用计划量
        /// </summary>
        public virtual decimal CurrRealConsumePlanQuantity
        {
            get { return currRealConsumePlanQuantity; }
            set { currRealConsumePlanQuantity = value; }
        }

        /// <summary>
        /// 当期实际耗用计划单价(数据传递用)
        /// </summary>
        public virtual decimal CurrRealConsumePlanPrice
        {
            get { return currRealConsumePlanPrice; }
            set { currRealConsumePlanPrice = value; }
        }

        /// <summary>
        /// 当期实际耗用计划合价
        /// </summary>
        public virtual decimal CurrRealConsumePlanTotalPrice
        {
            get { return currRealConsumePlanTotalPrice; }
            set { currRealConsumePlanTotalPrice = value; }
        }

        /// <summary>
        /// 当期实现合同收入量
        /// </summary>
        public virtual decimal CurrIncomeQuantity
        {
            get { return currIncomeQuantity; }
            set { currIncomeQuantity = value; }
        }

        /// <summary>
        /// 当期实现合同收入单价
        /// </summary>
        public virtual decimal CurrIncomePrice
        {
            get { return currIncomePrice; }
            set { currIncomePrice = value; }
        }

        /// <summary>
        /// 当期实现合同收入合价
        /// </summary>
        public virtual decimal CurrIncomeTotalPrice
        {
            get { return currIncomeTotalPrice; }
            set { currIncomeTotalPrice = value; }
        }

        /// <summary>
        /// 当期责任耗用数量
        /// </summary>
        public virtual decimal CurrResponsiConsumeQuantity
        {
            get { return currResponsiConsumeQuantity; }
            set { currResponsiConsumeQuantity = value; }
        }

        /// <summary>
        /// 当期责任耗用单价
        /// </summary>
        public virtual decimal CurrResponsiConsumePrice
        {
            get { return currResponsiConsumePrice; }
            set { currResponsiConsumePrice = value; }
        }

        /// <summary>
        /// 当期责任耗用合价
        /// </summary>
        public virtual decimal CurrResponsiConsumeTotalPrice
        {
            get { return currResponsiConsumeTotalPrice; }
            set { currResponsiConsumeTotalPrice = value; }
        }

        /// <summary>
        /// 累积实际耗用数量
        /// </summary>
        public virtual decimal SumRealConsumeQuantity
        {
            get { return sumRealConsumeQuantity; }
            set { sumRealConsumeQuantity = value; }
        }

        /// <summary>
        /// 累积实际耗用合价
        /// </summary>
        public virtual decimal SumRealConsumeTotalPrice
        {
            get { return sumRealConsumeTotalPrice; }
            set { sumRealConsumeTotalPrice = value; }
        }

        /// <summary>
        /// 累积实际耗用计划量
        /// </summary>
        public virtual decimal SumRealConsumePlanQuantity
        {
            get { return sumRealConsumePlanQuantity; }
            set { sumRealConsumePlanQuantity = value; }
        }

        /// <summary>
        /// 累积实际耗用计划合价
        /// </summary>
        public virtual decimal SumRealConsumePlanTotalPrice
        {
            get { return sumRealConsumePlanTotalPrice; }
            set { sumRealConsumePlanTotalPrice = value; }
        }

        /// <summary>
        /// 累积实现合同收入量
        /// </summary>
        public virtual decimal SumIncomeQuantity
        {
            get { return sumIncomeQuantity; }
            set { sumIncomeQuantity = value; }
        }

        /// <summary>
        /// 累积实现合同收入合价
        /// </summary>
        public virtual decimal SumIncomeTotalPrice
        {
            get { return sumIncomeTotalPrice; }
            set { sumIncomeTotalPrice = value; }
        }

        /// <summary>
        /// 累积责任耗用数量
        /// </summary>
        public virtual decimal SumResponsiConsumeQuantity
        {
            get { return sumResponsiConsumeQuantity; }
            set { sumResponsiConsumeQuantity = value; }
        }

        /// <summary>
        /// 累积责任耗用合价
        /// </summary>
        public virtual decimal SumResponsiConsumeTotalPrice
        {
            get { return sumResponsiConsumeTotalPrice; }
            set { sumResponsiConsumeTotalPrice = value; }
        }

        /// <summary>
        /// 工程任务明细GUID
        /// </summary>
        public virtual string ProjectTaskDtlGUID
        {
            get { return projectTaskDtlGUID; }
            set { projectTaskDtlGUID = value; }
        }

        /// <summary>
        /// 工程任务明细名称
        /// </summary>
        public virtual string ProjectTaskDtlName
        {
            get { return projectTaskDtlName; }
            set { projectTaskDtlName = value; }
        }

        private string _sourceType;

        /// <summary>
        /// 数据来源
        /// </summary>
        public virtual  string SourceType
        {
            get { return _sourceType; }
            set { _sourceType = value; }
        }

        private string _sourceId;

        /// <summary>
        /// 明细Id
        /// </summary>
        public virtual string SourceId
        {
            get { return _sourceId; }
            set { _sourceId = value; }
        }
    }
}
