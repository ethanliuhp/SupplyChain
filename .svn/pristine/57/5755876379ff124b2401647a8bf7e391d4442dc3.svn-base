using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.RelateClass;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain
{
    /// <summary>
    /// 物资耗用结算明细
    /// </summary>
    [Entity]
    [Serializable]
    public class MaterialSettleDetail : BaseDetail
    {
        private GWBSTree projectTask;
        /// <summary>
        /// 工程任务
        /// </summary>
        virtual public GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        private string projectTaskName;
        /// <summary>
        /// 工程任务名称
        /// </summary>
        virtual public string  ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        private string projectTaskCode;
        /// <summary>
        /// 工程任务层次码
        /// </summary>
        virtual public string ProjectTaskCode
        {
            get { return projectTaskCode; }
            set { projectTaskCode = value; }
        }
        private CostAccountSubject accountCostSubject;
        /// <summary>
        /// 成本核算科目
        /// </summary>
        virtual public CostAccountSubject AccountCostSubject
        {
            get { return accountCostSubject; }
            set { accountCostSubject = value; }
        }
        private string accountCostName;
        /// <summary>
        /// 成本核算科目名称
        /// </summary>
        virtual public string AccountCostName
        {
            get { return accountCostName; }
            set { accountCostName = value; }
        }
        private string accountCostCode;
        /// <summary>
        /// 成本核算科目层次码
        /// </summary>
        virtual public string AccountCostCode
        {
            get { return accountCostCode; }
            set { accountCostCode = value; }
        }
        private StandardUnit priceUnit;
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public StandardUnit PriceUnit
        {
            get { return priceUnit; }
            set { priceUnit = value; }
        }
        private string priceUnitName;
        /// <summary>
        /// 价格单位名称
        /// </summary>
        virtual public string PriceUnitName
        {
            get { return priceUnitName; }
            set { priceUnitName = value; }
        }
        private StandardUnit quantityUnit;
        /// <summary>
        /// 数量计量单位
        /// </summary>
        virtual public StandardUnit QuantityUnit
        {
            get { return quantityUnit; }
            set { quantityUnit = value; }
        }
        private string quantityUnitName;
        /// <summary>
        /// 数量单位名称
        /// </summary>
        virtual public string QuantityUnitName
        {
            get { return quantityUnitName; }
            set { quantityUnitName = value; }
        }
        
        private string monthlySettlment;
        /// <summary>
        /// 月度核算单GUID
        /// </summary>
        virtual public string MonthlySettlment
        {
            get { return monthlySettlment; }
            set { monthlySettlment = value; }
        }
        private int monthlyCostSuccess;
        /// <summary>
        /// 月度核算成功标志
        /// </summary>
        virtual public int MonthlyCostSuccess
        {
            get { return monthlyCostSuccess; }
            set { monthlyCostSuccess = value; }
        }
       
    }
}
