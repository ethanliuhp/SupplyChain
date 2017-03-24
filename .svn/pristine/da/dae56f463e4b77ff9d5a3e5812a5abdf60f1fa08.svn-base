using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Domain
{
    /// <summary>
    /// 费用结算明细
    /// </summary>
    [Serializable]
    public class ExpensesSettleDetail : BaseDetail
    {
        private string costName;
        /// <summary>
        /// 费用名称
        /// </summary>
        virtual public string CostName
        {
            get { return costName; }
            set { costName = value; }
        }
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
        private string projectTaskSysCode;
        /// <summary>
        /// 工程任务层次码
        /// </summary>
        virtual public string ProjectTaskSysCode
        {
            get { return projectTaskSysCode; }
            set { projectTaskSysCode = value; }
        }
        private CostAccountSubject accountCostSubject;
        /// <summary>
        /// 核算科目
        /// </summary>
        virtual public CostAccountSubject AccountCostSubject
        {
            get { return accountCostSubject; }
            set { accountCostSubject = value; }
        }
        private string accountCostName;
        /// <summary>
        /// 核算科目名称
        /// </summary>
        virtual public string AccountCostName
        {
            get { return accountCostName; }
            set { accountCostName = value; }
        }
        private string accountCostSysCode;
        /// <summary>
        /// 核算科目层次码
        /// </summary>
        virtual public string AccountCostSysCode
        {
            get { return accountCostSysCode; }
            set { accountCostSysCode = value; }
        }
        private decimal price;
        /// <summary>
        /// 价格
        /// </summary>
        virtual public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        private StandardUnit priceUnit;
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public StandardUnit  PriceUnit
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
        
        private decimal quantity;
        /// <summary>
        /// 数量
        /// </summary>
        virtual public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
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
        private MaterialCategory materialCategory;
        private string materialCategoryName;
        /// 资源类型
        /// </summary>
        virtual public MaterialCategory MaterialCategory
        {
            get { return materialCategory; }
            set { materialCategory = value; }
        }
        /// <summary>
        /// 资源类型名称
        /// </summary>
        virtual public string MaterialCategoryName
        {
            get { return materialCategoryName; }
            set { materialCategoryName = value; }
        }
        private string monthlySettlment;
        /// <summary>
        /// 月度核算明细GUID
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
        private string materialSysCode;
        /// <summary>
        /// 物资类型层次码
        /// </summary>
        virtual public string MaterialSysCode
        {
            get { return materialSysCode; }
            set { materialSysCode = value; }
        }
    }




}
