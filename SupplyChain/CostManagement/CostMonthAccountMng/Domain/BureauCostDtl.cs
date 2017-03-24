using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 局成本报表
    /// </summary>
    [Serializable]
    [Entity]
    public class BureauCostDtl : BaseDetail
    {
        private string projectType; //工程类别
        private string projectScale;    //工程规模
        private string location;    //所在区域
        private string ownerName;   //业主名称
        private string generalContractName;   //总承包单位名称
        private string ownerProperty;   //业主性质
        private string isStractegicCustomer;    //是否战略客户
        private string projectSite; //工程地点
        private decimal constructionAcreage; //建筑面积
        private decimal constructionHeight;  //建筑高度
        private decimal totalContractAmount; //合同总金额
        private decimal selfContractAmount;  //其中自行合同额
        private decimal generalManageCost;  //其中总包管理费金额
        private string manageCostCalculateRate; //总包管理费计取比例
        private string settlementType;  //结算方式
        private DateTime settlementTime;    //结算时间
        private string invokeSituation; //可调用情况
        private string imprestRate; //预付款比例
        private string paymentMode; //付款方式
        private string paymentRate;    //支付比例
        private string paymentForm; //支付形式
        private DateTime maintenancePayTime; //保修金支付时间
        private string managementMode;  //管理模式
        private decimal targetScopeMoney;   //目标合同内金额
        private DateTime dutyAgreementTime; //目标责任书签订日期
        private string checkNodeSetting;    //考核节点设置
        private string foreBidEarningRate;   //标前盈利率(含税)
        private decimal calculateCost;   //测算(施工图预算)成本
        private string calculateBraekevenRate;  //测算(预算)盈亏比例
        private decimal targetCost;  //目标成本
        private string  handInRate;  //上交比例
        private string targetRemark;    //目标责任书备注
        private decimal shouldPaydeposit; //应交抵押金
        private decimal actualPayDeposit;    //实际已交抵押金
        private string quarterFulfillTimes;  //季度开工累计兑现次数
        private decimal quarterShouldPayMoney;     //季度应发金额
        private decimal quarterActualPayMoney;     //季度实际已发金额
        private decimal quarter2015Money;   //季度其中2015发放金额
        private string nodeFulfillTimes;    //节点开工累计兑现次数
        private decimal nodeShouldPayMoney; //节点应发金额
        private decimal nodeActualPayMoney; //节点实际已发金额
        private decimal node2015Money;  //节点其中2015发放金额
        private decimal generalCompleteOutput;   //总包完成产值
        private decimal selefCompleteOutput; //其中自行完成产值
        private decimal expectConfirmAmount;    //预计确认金额
        private DateTime expectConfirmDate;     //预计确认金额计量日期
        private decimal confirmedAmount;    //其中已确认金额
        private DateTime confirmedDate; //已确认金额计量日期
        private decimal estimateAmount; //其中预估金额
        private decimal projectActualCost;   //项目实际成本(自行施工、含税)
        private decimal projectProfit;       //工程盈利
        private decimal gatheringAmountAtRate;      //按合同比例应收款(自行施工)
        private decimal selfAcceptAmount;       //自行施工已收款
        private decimal selfImprest;    //自行施工预付款
        private decimal selfProgressAmount; //自行施工进度款
        private decimal selfOthers; //自行施工其他
        private decimal paymentAtRate;  //按合同比例应付款
        private decimal payedAmount;    //已付款
        private string reportedQuantity;       //报送份数
        private decimal reportedAmount;     //报送金额
        private decimal correspondingCost;   //报送金额对应成本
        private string ownerConfirmQuantity;   //业主确认份数
        private decimal ownerConfirmAmount; //业主确认金额
        private decimal planReduceAmount;   //策划降本金额
        private decimal planIncreaseAmount; //策划增收金额
        private decimal actualReduceAmount; //实际降本金额
        private decimal actualIncreaseAmount;   //实际增收金额
        private decimal accumulatePaidBonus;    //开工累计已发放商务策划奖金额
        private decimal paidBonus2015;  //2015年已发放商务策划奖金额
        private decimal rebarDetailingAmount;   //钢筋翻样量
        private DateTime contractStartDate;  //合同开工日期
        private DateTime contractEndDate;   //合同竣工日期
        private DateTime actualStartDate;   //实际开工日期
        private DateTime expectEendDate;    //预计竣工日期
        private string isPerforming;    //工期是否履约
        private string delayDays;   //工期滞后天数
        private decimal causeIncreaseCost;    //造成成本增加金额
        private string durationCaption; //工期履约情况说明
        private string ownerSignDays;   //业主签认天数
        private decimal confirmExpense; //确认费用金额
        private decimal confrontForfeit;    //按合同将面临处罚金额
        private string programType;     //项目类别
        private string programState;    //项目状态

        virtual public string ProjectType
        {
            get { return projectType; }
            set { projectType = value; }
        }

        virtual public string ProjectScale
        {
            get { return projectScale; }
            set { projectScale = value; }
        }

        virtual public string Location
        {
            get { return location; }
            set { location = value; }
        }

        virtual public string OwnerName
        {
            get { return ownerName; }
            set { ownerName = value; }
        }

        virtual public string GeneralContractName
        {
            get { return generalContractName; }
            set { generalContractName = value; }
        }

        virtual public string OwnerProperty
        {
            get { return ownerProperty; }
            set { ownerProperty = value; }
        }

        virtual public string IsStractegicCustomer
        {
            get { return isStractegicCustomer; }
            set { isStractegicCustomer = value; }
        }

        virtual public string ProjectSite
        {
            get { return projectSite; }
            set { projectSite = value; }
        }

        virtual public decimal ConstructionAcreage
        {
            get { return constructionAcreage; }
            set { constructionAcreage = value; }
        }

        virtual public decimal ConstructionHeight
        {
            get { return constructionHeight; }
            set { constructionHeight = value; }
        }

        virtual public decimal TotalContractAmount
        {
            get { return totalContractAmount; }
            set { totalContractAmount = value; }
        }

        virtual public decimal SelfContractAmount
        {
            get { return selfContractAmount; }
            set { selfContractAmount = value; }
        }

        virtual public decimal GeneralManageCost
        {
            get { return generalManageCost; }
            set { generalManageCost = value; }
        }

        virtual public string ManageCostCalculateRate
        {
            get { return manageCostCalculateRate; }
            set { manageCostCalculateRate = value; }
        }

        virtual public string SettlementType
        {
            get { return settlementType; }
            set { settlementType = value; }
        }

        virtual public System.DateTime SettlementTime
        {
            get { return settlementTime; }
            set { settlementTime = value; }
        }

        virtual public string InvokeSituation
        {
            get { return invokeSituation; }
            set { invokeSituation = value; }
        }

        virtual public string ImprestRate
        {
            get { return imprestRate; }
            set { imprestRate = value; }
        }

        virtual public string PaymentMode
        {
            get { return paymentMode; }
            set { paymentMode = value; }
        }

        virtual public string PaymentRate
        {
            get { return paymentRate; }
            set { paymentRate = value; }
        }

        virtual public string PaymentForm
        {
            get { return paymentForm; }
            set { paymentForm = value; }
        }

        virtual public System.DateTime MaintenancePayTime
        {
            get { return maintenancePayTime; }
            set { maintenancePayTime = value; }
        }

        virtual public string ManagementMode
        {
            get { return managementMode; }
            set { managementMode = value; }
        }

        virtual public decimal TargetScopeMoney
        {
            get { return targetScopeMoney; }
            set { targetScopeMoney = value; }
        }

        virtual public System.DateTime DutyAgreementTime
        {
            get { return dutyAgreementTime; }
            set { dutyAgreementTime = value; }
        }

        virtual public string CheckNodeSetting
        {
            get { return checkNodeSetting; }
            set { checkNodeSetting = value; }
        }

        virtual public string ForeBidEarningRate
        {
            get { return foreBidEarningRate; }
            set { foreBidEarningRate = value; }
        }

        virtual public decimal CalculateCost
        {
            get { return calculateCost; }
            set { calculateCost = value; }
        }

        virtual public string CalculateBraekevenRate
        {
            get { return calculateBraekevenRate; }
            set { calculateBraekevenRate = value; }
        }

        virtual public decimal TargetCost
        {
            get { return targetCost; }
            set { targetCost = value; }
        }

        virtual public string  HandInRate
        {
            get { return handInRate; }
            set { handInRate = value; }
        }

        virtual public string TargetRemark
        {
            get { return targetRemark; }
            set { targetRemark = value; }
        }

        virtual public decimal ShouldPaydeposit
        {
            get { return shouldPaydeposit; }
            set { shouldPaydeposit = value; }
        }

        virtual public decimal ActualPayDeposit
        {
            get { return actualPayDeposit; }
            set { actualPayDeposit = value; }
        }

        virtual public string QuarterFulfillTimes
        {
            get { return quarterFulfillTimes; }
            set { quarterFulfillTimes = value; }
        }

        virtual public decimal QuarterShouldPayMoney
        {
            get { return quarterShouldPayMoney; }
            set { quarterShouldPayMoney = value; }
        }

        virtual public decimal QuarterActualPayMoney
        {
            get { return quarterActualPayMoney; }
            set { quarterActualPayMoney = value; }
        }

        virtual public decimal Quarter2015Money
        {
            get { return quarter2015Money; }
            set { quarter2015Money = value; }
        }

        virtual public string NodeFulfillTimes
        {
            get { return nodeFulfillTimes; }
            set { nodeFulfillTimes = value; }
        }

        virtual public decimal NodeShouldPayMoney
        {
            get { return nodeShouldPayMoney; }
            set { nodeShouldPayMoney = value; }
        }

        virtual public decimal NodeActualPayMoney
        {
            get { return nodeActualPayMoney; }
            set { nodeActualPayMoney = value; }
        }

        virtual public decimal Node2015Money
        {
            get { return node2015Money; }
            set { node2015Money = value; }
        }

        virtual public decimal GeneralCompleteOutput
        {
            get { return generalCompleteOutput; }
            set { generalCompleteOutput = value; }
        }

        virtual public decimal SelefCompleteOutput
        {
            get { return selefCompleteOutput; }
            set { selefCompleteOutput = value; }
        }

        virtual public decimal ExpectConfirmAmount
        {
            get { return expectConfirmAmount; }
            set { expectConfirmAmount = value; }
        }

        virtual public System.DateTime ExpectConfirmDate
        {
            get { return expectConfirmDate; }
            set { expectConfirmDate = value; }
        }

        virtual public decimal ConfirmedAmount
        {
            get { return confirmedAmount; }
            set { confirmedAmount = value; }
        }

        virtual public System.DateTime ConfirmedDate
        {
            get { return confirmedDate; }
            set { confirmedDate = value; }
        }

        virtual public decimal EstimateAmount
        {
            get { return estimateAmount; }
            set { estimateAmount = value; }
        }

        virtual public decimal ProjectActualCost
        {
            get { return projectActualCost; }
            set { projectActualCost = value; }
        }

        virtual public decimal ProjectProfit
        {
            get { return projectProfit; }
            set { projectProfit = value; }
        }

        virtual public decimal GatheringAmountAtRate
        {
            get { return gatheringAmountAtRate; }
            set { gatheringAmountAtRate = value; }
        }

        virtual public decimal SelfAcceptAmount
        {
            get { return selfAcceptAmount; }
            set { selfAcceptAmount = value; }
        }

        virtual public decimal SelfImprest
        {
            get { return selfImprest; }
            set { selfImprest = value; }
        }

        virtual public decimal SelfProgressAmount
        {
            get { return selfProgressAmount; }
            set { selfProgressAmount = value; }
        }

        virtual public decimal SelfOthers
        {
            get { return selfOthers; }
            set { selfOthers = value; }
        }

        virtual public decimal PaymentAtRate
        {
            get { return paymentAtRate; }
            set { paymentAtRate = value; }
        }

        virtual public decimal PayedAmount
        {
            get { return payedAmount; }
            set { payedAmount = value; }
        }

        virtual public string ReportedQuantity
        {
            get { return reportedQuantity; }
            set { reportedQuantity = value; }
        }

        virtual public decimal ReportedAmount
        {
            get { return reportedAmount; }
            set { reportedAmount = value; }
        }

        virtual public decimal CorrespondingCost
        {
            get { return correspondingCost; }
            set { correspondingCost = value; }
        }

        virtual public string OwnerConfirmQuantity
        {
            get { return ownerConfirmQuantity; }
            set { ownerConfirmQuantity = value; }
        }

        virtual public decimal OwnerConfirmAmount
        {
            get { return ownerConfirmAmount; }
            set { ownerConfirmAmount = value; }
        }

        virtual public decimal PlanReduceAmount
        {
            get { return planReduceAmount; }
            set { planReduceAmount = value; }
        }

        virtual public decimal PlanIncreaseAmount
        {
            get { return planIncreaseAmount; }
            set { planIncreaseAmount = value; }
        }

        virtual public decimal ActualReduceAmount
        {
            get { return actualReduceAmount; }
            set { actualReduceAmount = value; }
        }

        virtual public decimal ActualIncreaseAmount
        {
            get { return actualIncreaseAmount; }
            set { actualIncreaseAmount = value; }
        }

        virtual public decimal AccumulatePaidBonus
        {
            get { return accumulatePaidBonus; }
            set { accumulatePaidBonus = value; }
        }

        virtual public decimal PaidBonus2015
        {
            get { return paidBonus2015; }
            set { paidBonus2015 = value; }
        }

        virtual public decimal RebarDetailingAmount
        {
            get { return rebarDetailingAmount; }
            set { rebarDetailingAmount = value; }
        }

        virtual public System.DateTime ContractStartDate
        {
            get { return contractStartDate; }
            set { contractStartDate = value; }
        }

        virtual public System.DateTime ContractEndDate
        {
            get { return contractEndDate; }
            set { contractEndDate = value; }
        }

        virtual public System.DateTime ActualStartDate
        {
            get { return actualStartDate; }
            set { actualStartDate = value; }
        }

        virtual public System.DateTime ExpectEendDate
        {
            get { return expectEendDate; }
            set { expectEendDate = value; }
        }

        virtual public string IsPerforming
        {
            get { return isPerforming; }
            set { isPerforming = value; }
        }

        virtual public string DelayDays
        {
            get { return delayDays; }
            set { delayDays = value; }
        }

        virtual public decimal CauseIncreaseCost
        {
            get { return causeIncreaseCost; }
            set { causeIncreaseCost = value; }
        }

        virtual public string DurationCaption
        {
            get { return durationCaption; }
            set { durationCaption = value; }
        }

        virtual public string OwnerSignDays
        {
            get { return ownerSignDays; }
            set { ownerSignDays = value; }
        }

        virtual public decimal ConfirmExpense
        {
            get { return confirmExpense; }
            set { confirmExpense = value; }
        }

        virtual public decimal ConfrontForfeit
        {
            get { return confrontForfeit; }
            set { confrontForfeit = value; }
        }

        virtual public string ProgramType
        {
            get { return programType; }
            set { programType = value; }
        }

        virtual public string ProgramState
        {
            get { return programState; }
            set { programState = value; }
        }
    }
}
