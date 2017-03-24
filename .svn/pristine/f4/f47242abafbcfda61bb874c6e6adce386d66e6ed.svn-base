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
        private string _costSubjectSyscode;
        private string _costingSubjectName;
        private string _resourceTypeGUID;
        private string _resourceTypeName;
        private string _resourceSyscode;
        private CostMonthAccountDtl _theAccountDetail;
        //当期耗用
        private decimal currRealConsumeQuantity;
        private decimal currRealConsumePrice;
        private decimal currRealConsumeTotalPrice;
        private decimal currRealConsumePlanQuantity;
        private decimal currRealConsumePlanTotalPrice;
        private decimal currIncomeQuantity;
        private decimal currIncomeTotalPrice;
        private decimal currResponsiConsumeQuantity;
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
        /// 资源类型名称
        /// </summary>
        public virtual string ResourceTypeName
        {
            get { return _resourceTypeName; }
            set { _resourceTypeName = value; }
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
    }
}
