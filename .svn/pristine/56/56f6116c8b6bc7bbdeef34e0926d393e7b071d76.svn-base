using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    [Serializable]
    [Entity]
    public class SpecialCostMaster : BaseMaster
    {
        private string costType;                                      //费用类型
        private GWBSTree engTaskId;                                    //工程任务GUID   
        private string engTaskName;                             //工程任务名称
        private string engTaskSyscode;
        private decimal contractProfit;                             //合同利润率
        private decimal contractTotalIncome;                   //合同总收入
        private decimal contractTotalIPay;                          //合同总支出
        private decimal accountingProgress;                       //累计核算形象进度
        private decimal realIncome;                                    //累计实际收入
        private decimal realPay;                                          //累计实际支出
        private DateTime submitDate;                                  //提交时间       

        private PersonInfo handlePerson;
        private string handlePersonName;
        /// <summary>
        /// 经手人(责任人)名称
        /// </summary>
        public virtual string HandlePersonName
        {
            get { return handlePersonName; }
            set { handlePersonName = value; }
        }
        /// <summary>
        /// 业务经手人（采购员，销售员）
        /// </summary>
        virtual public PersonInfo HandlePerson
        {
            get { return handlePerson; }
            set { handlePerson = value; }
        }

        /// <summary>
        /// 费用类型
        /// </summary>
        virtual public string CostType
        {
            get { return costType; }
            set { costType = value; }
        }
        /// <summary>
        /// 工程任务GUID   
        /// </summary>
        virtual public GWBSTree EngTaskId
        {
            get { return engTaskId; }
            set { engTaskId = value; }
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
        /// 合同利润率
        /// </summary>
        virtual public decimal ContractProfit
        {
            get { return contractProfit; }
            set { contractProfit = value; }
        }
        /// <summary>
        /// 合同总收入
        /// </summary>
        virtual public decimal ContractTotalIncome
        {
            get { return contractTotalIncome; }
            set { contractTotalIncome = value; }
        }
        /// <summary>
        /// 合同总支出
        /// </summary>
        virtual public decimal ContractTotalIPay
        {
            get { return contractTotalIPay; }
            set { contractTotalIPay = value; }
        }
        /// <summary>
        /// 累计核算形象进度
        /// </summary>
        virtual public decimal AccountingProgress
        {
            get { return accountingProgress; }
            set { accountingProgress = value; }
        }
        /// <summary>
        /// 累计实际收入
        /// </summary>
        virtual public decimal RealIncome
        {
            get { return realIncome; }
            set { realIncome = value; }
        }
        /// <summary>
        /// 累计实际支出
        /// </summary>
        virtual public decimal RealPay
        {
            get { return realPay; }
            set { realPay = value; }
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        virtual public DateTime SubmitDate
        {
            get { return submitDate; }
            set { submitDate = value; }
        }
    }
}
