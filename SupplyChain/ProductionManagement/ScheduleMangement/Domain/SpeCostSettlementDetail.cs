using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    [Serializable]
    [Entity]
    public class SpeCostSettlementDetail :BaseDetail
    {
        private GWBSTree engTask;
        private string engTaskName;
        private string engTaskSyscode;
        private string priceUnitName; 
        private StandardUnit priceUom;         
        private decimal settlementMoney;                       
        private decimal payMentFees;
        private decimal otherAccruals;
        private decimal manageAcer;
        private decimal electAcer;
        private decimal manageMoney;
        private decimal electMoney;

        /// <summary>
        /// 管理费比例
        /// </summary>
        virtual public decimal ManageAcer
        {
            get { return manageAcer; }
            set { manageAcer = value; }
        }

        /// <summary>
        /// 水电费比例
        /// </summary>
        virtual public decimal ElectAcer
        {
            get { return electAcer; }
            set { electAcer = value; }
        }
        /// <summary>
        /// 应缴管理费
        /// </summary>
        virtual public decimal ManageMoney
        {
            get { return manageMoney; }
            set { manageMoney = value; }
        }
        /// <summary>
        /// 代缴水电费
        /// </summary>
        virtual public decimal ElectMoney
        {
            get { return electMoney; }
            set { electMoney = value; }
        }
        /// <summary>
        /// 应缴规费
        /// </summary>
        virtual public decimal PayMentFees
        {
            get { return payMentFees; }
            set { payMentFees = value; }
        }
        
        /// <summary>
        /// 其他应计费用
        /// </summary>
        virtual public decimal OtherAccruals
        {
            get { return otherAccruals; }
            set { otherAccruals = value; }
        }
        /// <summary>
        /// 工程任务
        /// </summary>
        virtual public GWBSTree EngTask
        {
            get { return engTask; }
            set { engTask = value; }
        }
         /// <summary>
        /// 工程任务名称
         /// </summary>
        virtual public string EngTaskName
        {
            get { return engTaskName; }
            set { engTaskName = value; }
        }
        /// <summary>
        /// 工程任务层次码
        /// </summary>
        virtual public string EngTaskSyscode
        {
            get { return engTaskSyscode; }
            set { engTaskSyscode = value; }
        }
         /// <summary>
        /// 价格单位名称
         /// </summary>
        virtual public string PriceUnitName
        {
            get { return priceUnitName; }
            set { priceUnitName = value; }
        }
         /// <summary>
        /// 价格计量单位
         /// </summary>
        virtual public StandardUnit PriceUom
        {
            get { return priceUom; }
            set { priceUom = value; }
        }
         /// <summary>
        /// 结算金额
         /// </summary>
        virtual public decimal SettlementMoney
        {
            get { return settlementMoney; }
            set { settlementMoney = value; }
        } 
            
    }
}
