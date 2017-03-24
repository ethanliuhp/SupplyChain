using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 成本检查指标
    /// </summary>
    [Serializable]
    [Entity]
    public class CostCheckIndicatorDtl : BaseDetail
    {
        private string tenderCalculatePoint;    //投标测算点数
        private string liabilityPaid;   //责任上缴
        private decimal selfPlanMoney;//自营预计总产值（万元）
        private string quarterBenefitRate;  //上季度效益率
        private string constructionContractIncome;  //建造合同收入(财务口径数据)
        private decimal income;  //收入
        private decimal cost;   //成本
        private decimal benefitAmount;  //效益额
        private string benefitRate; //效益率
        private decimal dutyCost;   //责任成本
        private string overCostReduceRate;  //超成本降低率
        private decimal siteFunds;  //现场经费
        private string totalOutputValueAccount; //占累计自营产值比例
        private decimal occurredMoney;  //已发生金额
        private decimal expectOccurredMoney;    //预计尚需发生金额
        private decimal totalMoney; //合计金额
        private string expectOutputValueAccount; //占自营预计总产值比例
        private string concreteDrawingBudget;   //商品砼图纸预算量
        private string concreteConsumption; //商品砼消耗量
        private string concreteSaveRate;    //商品砼节约率(%)((预算量-消耗量)/预算量)
        private string rebarDetailingAmount;    //钢筋翻样量
        private string rebarConsumption; //钢筋消耗量
        private string rebarSaveRate;   //钢筋节约率(%)((翻样量-消耗量)/翻样量)
        private string wasteRebarAmount; //处理废钢筋量
        private string scrapRate;   //废材率(处理废钢筋量/消耗量)
        private string rightReportPoint;    //确权报量节点
        private DateTime projectSubmitTime; //项目报送时间
        private DateTime ownerConfirmTime;  //业主确认时间
        private decimal ownerRightOutput;    //累计业主确权自行产值
        private decimal contractorRightOutput;   //累计业主确权总包产值
        private decimal projectSelfPayment;    //累计项目自行支出
        private decimal projcetContractorPayment;   //累计项目总包支出
        private string selfOutputRightRate; //累计自行产值确权率
        private string contractorOutputRightRate;   //累计总包产值确权率
        private string outPutRightRate;//产值确认率
        private decimal receivableAccount;  //应收款
        private decimal actualAccount;  //实际收款
        private string overallBusinessPlan; //整体商务策划
        private string responsibilitySigh;  //责任状签订
        private decimal receivableRiskMortgage; //应收风险抵押金
        private decimal actualRiskMortgage; //实收风险抵押金
        private string riskMortgageRate;    //风险抵押金比例
        private decimal occurredHourlyAccount;  //已发生计时工金额
        private string proportionOfHourlyWork;  //计时工占劳务费比例
        private decimal occurredOEMAccount;    //已发生代工金额
        private decimal deductedAccount;   //已扣款金额
        private decimal outerContractAccount;   //合同外费用
        private decimal selfSignedAccount;   //自签协议费用
        private string proportionOfOuterContractAccount;  //合同外费用占分包结算比例
        /// <summary>
        /// 投标测算点数
        /// </summary>
        virtual public string TenderCalculatePoint
        {
            get { return tenderCalculatePoint; }
            set { tenderCalculatePoint = value; }
        }
        /// <summary>
        /// 责任上缴
        /// </summary>
        virtual public string LiabilityPaid
        {
            get { return liabilityPaid; }
            set { liabilityPaid = value; }
        }
        /// <summary>
        /// 自营预计总产值（万元）
        /// </summary>
        virtual public decimal SelfPlanMoney
        {
            get { return selfPlanMoney; }
            set { selfPlanMoney = value; }
        }
        /// <summary>
        /// 上季度效益率
        /// </summary>
        virtual public string QuarterBenefitRate
        {
            get { return quarterBenefitRate; }
            set { quarterBenefitRate = value; }
        }
        /// <summary>
        /// 建造合同收入(财务口径数据)
        /// </summary>
        virtual public string ConstructionContractIncome
        {
            get { return constructionContractIncome; }
            set { constructionContractIncome = value; }
        }
        /// <summary>
        /// 收入
        /// </summary>
        virtual public decimal Income
        {
            get { return income; }
            set { income = value; }
        }
        /// <summary>
        /// 成本
        /// </summary>
        virtual public decimal Cost
        {
            get { return cost; }
            set { cost = value; }
        }
        /// <summary>
        /// 效益额
        /// </summary>
        virtual public decimal BenefitAmount
        {
            get { return benefitAmount; }
            set { benefitAmount = value; }
        }
        /// <summary>
        /// 效益率
        /// </summary>
        virtual public string BenefitRate
        {
            get { return benefitRate; }
            set { benefitRate = value; }
        }
        /// <summary>
        /// 责任成本
        /// </summary>
        virtual public decimal DutyCost
        {
            get { return dutyCost; }
            set { dutyCost = value; }
        }
        /// <summary>
        /// 超成本降低率
        /// </summary>
        virtual public string OverCostReduceRate
        {
            get { return overCostReduceRate; }
            set { overCostReduceRate = value; }
        }
        /// <summary>
        /// 现场经费
        /// </summary>
        virtual public decimal SiteFunds
        {
            get { return siteFunds; }
            set { siteFunds = value; }
        }
        /// <summary>
        /// 占累计自营产值比例
        /// </summary>
        virtual public string TotalOutputValueAccount
        {
            get { return totalOutputValueAccount; }
            set { totalOutputValueAccount = value; }
        }
        /// <summary>
        /// 已发生金额
        /// </summary>
        virtual public decimal OccurredMoney
        {
            get { return occurredMoney; }
            set { occurredMoney = value; }
        }
        /// <summary>
        /// 预计尚需发生金额
        /// </summary>
        virtual public decimal ExpectOccurredMoney
        {
            get { return expectOccurredMoney; }
            set { expectOccurredMoney = value; }
        }
        /// <summary>
        /// 合计金额
        /// </summary>
        virtual public decimal TotalMoney
        {
            get { return totalMoney; }
            set { totalMoney = value; }
        }
        /// <summary>
        /// 占自营预计总产值比例
        /// </summary>
        virtual public string ExpectOutputValueAccount
        {
            get { return expectOutputValueAccount; }
            set { expectOutputValueAccount = value; }
        }
        /// <summary>
        /// 商品砼图纸预算量
        /// </summary>
        virtual public string ConcreteDrawingBudget
        {
            get { return concreteDrawingBudget; }
            set { concreteDrawingBudget = value; }
        }
        /// <summary>
        /// 商品砼消耗量
        /// </summary>
        virtual public string ConcreteConsumption
        {
            get { return concreteConsumption; }
            set { concreteConsumption = value; }
        }
        /// <summary>
        /// 商品砼节约率(%)((预算量-消耗量)/预算量)
        /// </summary>
        virtual public string ConcreteSaveRate
        {
            get { return concreteSaveRate; }
            set { concreteSaveRate = value; }
        }
        /// <summary>
        /// 钢筋翻样量
        /// </summary>
        virtual public string RebarDetailingAmount
        {
            get { return rebarDetailingAmount; }
            set { rebarDetailingAmount = value; }
        }
        /// <summary>
        /// 钢筋消耗量
        /// </summary>
        virtual public string RebarConsumption
        {
            get { return rebarConsumption; }
            set { rebarConsumption = value; }
        }
        /// <summary>
        /// 钢筋节约率(%)((翻样量-消耗量)/翻样量)
        /// </summary>
        virtual public string RebarSaveRate
        {
            get { return rebarSaveRate; }
            set { rebarSaveRate = value; }
        }
        /// <summary>
        /// 处理废钢筋量
        /// </summary>
        virtual public string WasteRebarAmount
        {
            get { return wasteRebarAmount; }
            set { wasteRebarAmount = value; }
        }
        /// <summary>
        /// 废材率(处理废钢筋量/消耗量)
        /// </summary>
        virtual public string ScrapRate
        {
            get { return scrapRate; }
            set { scrapRate = value; }
        }
        /// <summary>
        /// 确权报量节点
        /// </summary>
        virtual public string RightReportPoint
        {
            get { return rightReportPoint; }
            set { rightReportPoint = value; }
        }
        /// <summary>
        /// 项目报送时间
        /// </summary>
        virtual public DateTime ProjectSubmitTime
        {
            get { return projectSubmitTime; }
            set { projectSubmitTime = value; }
        }
        /// <summary>
        /// 业主确认时间
        /// </summary>
        virtual public DateTime OwnerConfirmTime
        {
            get { return ownerConfirmTime; }
            set { ownerConfirmTime = value; }
        }
        /// <summary>
        /// 累计业主确权自行产值
        /// </summary>
        virtual public decimal OwnerRightOutput
        {
            get { return ownerRightOutput; }
            set { ownerRightOutput = value; }
        }
        /// <summary>
        /// 累计业主确权总包产值
        /// </summary>
        virtual public decimal ContractorRightOutput
        {
            get { return contractorRightOutput; }
            set { contractorRightOutput = value; }
        }
        /// <summary>
        /// 产值确认率
        /// </summary>
        virtual public string OutPutRightRate
        {
            get { return outPutRightRate; }
             set {  outPutRightRate=value; }
        }
        /// <summary>
        /// 累计项目自行支出
        /// </summary>
        virtual public decimal ProjectSelfPayment
        {
            get { return projectSelfPayment; }
            set { projectSelfPayment = value; }
        }
        /// <summary>
        /// 累计项目总包支出
        /// </summary>
        virtual public decimal ProjcetContractorPayment
        {
            get { return projcetContractorPayment; }
            set { projcetContractorPayment = value; }
        }
        /// <summary>
        /// 累计自行产值确权率
        /// </summary>
        virtual public string SelfOutputRightRate
        {
            get { return selfOutputRightRate; }
            set { selfOutputRightRate = value; }
        }
        /// <summary>
        /// 累计总包产值确权率
        /// </summary>
        virtual public string ContractorOutputRightRate
        {
            get { return contractorOutputRightRate; }
            set { contractorOutputRightRate = value; }
        }
        /// <summary>
        /// 应收款
        /// </summary>
        virtual public decimal ReceivableAccount
        {
            get { return receivableAccount; }
            set { receivableAccount = value; }
        }
        /// <summary>
        /// 实际收款
        /// </summary>
        virtual public decimal ActualAccount
        {
            get { return actualAccount; }
            set { actualAccount = value; }
        }
        /// <summary>
        /// 整体商务策划
        /// </summary>
        virtual public string OverallBusinessPlan
        {
            get { return overallBusinessPlan; }
            set { overallBusinessPlan = value; }
        }
        /// <summary>
        /// 责任状签订
        /// </summary>
        virtual public string ResponsibilitySigh
        {
            get { return responsibilitySigh; }
            set { responsibilitySigh = value; }
        }
        /// <summary>
        /// 应收风险抵押金
        /// </summary>
        virtual public decimal ReceivableRiskMortgage
        {
            get { return receivableRiskMortgage; }
            set { receivableRiskMortgage = value; }
        }
        /// <summary>
        /// 实收风险抵押金
        /// </summary>
        virtual public decimal ActualRiskMortgage
        {
            get { return actualRiskMortgage; }
            set { actualRiskMortgage = value; }
        }
        /// <summary>
        /// 风险抵押金比例
        /// </summary>
        virtual public string RiskMortgageRate
        {
            get { return riskMortgageRate; }
            set { riskMortgageRate = value; }
        }
        /// <summary>
        /// 已发生计时工金额
        /// </summary>
        virtual public decimal OccurredHourlyAccount
        {
            get { return occurredHourlyAccount; }
            set { occurredHourlyAccount = value; }
        }
        /// <summary>
        /// 计时工占劳务费比例
        /// </summary>
        virtual public string ProportionOfHourlyWork
        {
            get { return proportionOfHourlyWork; }
            set { proportionOfHourlyWork = value; }
        }
        /// <summary>
        /// 已发生代工金额
        /// </summary>
        virtual public decimal OccurredOEMAccount
        {
            get { return occurredOEMAccount; }
            set { occurredOEMAccount = value; }
        }
        /// <summary>
        /// 已扣款金额
        /// </summary>
        virtual public decimal DeductedAccount
        {
            get { return deductedAccount; }
            set { deductedAccount = value; }
        }
        /// <summary>
        /// 合同外费用
        /// </summary>
        virtual public decimal OuterContractAccount
        {
            get { return outerContractAccount; }
            set { outerContractAccount = value; }
        }
        /// <summary>
        /// 自签协议费用
        /// </summary>
        virtual public decimal SelfSignedAccount
        {
            get { return selfSignedAccount; }
            set { selfSignedAccount = value; }
        }
        /// <summary>
        /// 合同外费用占分包结算比例
        /// </summary>
        virtual public string ProportionOfOuterContractAccount
        {
            get { return proportionOfOuterContractAccount; }
            set { proportionOfOuterContractAccount = value; }
        }
    }
}
