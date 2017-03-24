using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Domain
{
    /// <summary>
    /// 目标责任书
    /// </summary>
    [Serializable]
    public class TargetRespBook : BaseMaster
    {
        //private string id;    //id
        private string safetyCivilizedSign;    //安全文明施工目标
        private decimal installationFreeRate;    //安装施工部分配合费比率
        private decimal costcontrolRewardtatio;    //成本控制奖励比率
        private decimal costcontrolTarget;    //成本控制目标
        private decimal cashRewardNodeNumber;    //兑现奖励节点数量
        private string riskPaymentState;    //风险抵押金缴纳状况
        private decimal riskRewardRatio;   //风险化解奖励比率
        private decimal riskDissolvesTarget;   //风险化解目标
        private string projectDate;    //工期
        private string periodmeaSuringUnit;    //工期计量单位
        private DateTime planEndDate;    //计划竣工时间
        private DateTime planBeginDate;    //计划开始时间
        private string prickleName;    //计量单位名称
        private StandardUnit pricePrickle;    //价格计量单位
        private string economicgoalEnginner;    //经济目标责任范围不含工程
        private string ensureLevel;   //确保工程质量等级
        private string signedWhether;    //是否签订
        private string documentName;   //文档名称 
        private string projectId;    //项目GUID 
        private string projectScale;    //项目规模
        private string projectManagerName;    //项目经理
        private PersonInfo projectManagerId;    //项目经理GUID
        private string projectName;    //项目名称
        private decimal serviceFeeRates;    //业务指定分包费用自提比率
        private decimal owneRawardsRatio;    //业主奖励自提比率
        private decimal responsibilityRatio;    //责任范围外分包工程利润自提比率
        private decimal responsibilityRewardTatio;    //责任上缴奖励比率
        private decimal responsibilityTurnedTarget;    //责任上缴目标
        private decimal state;    //状态
        private string handlePerson;   //责任人        
        private DateTime createDate;  //创建时间
        private Iesi.Collections.Generic.ISet<TargetProgressNode> nodeDetails = new Iesi.Collections.Generic.HashedSet<TargetProgressNode>();
        virtual public Iesi.Collections.Generic.ISet<TargetProgressNode> NodeDetails
        {
            get { return nodeDetails; }
            set { nodeDetails = value; }
        }
        private Iesi.Collections.Generic.ISet<IrpRiskDepositPayRecord> recordDetails = new Iesi.Collections.Generic.HashedSet<IrpRiskDepositPayRecord>();
        virtual public Iesi.Collections.Generic.ISet<IrpRiskDepositPayRecord> RecordDetails
        {
            get { return recordDetails; }
            set { recordDetails = value; }
        }

        virtual public void AddDetail(TargetProgressNode detail)
        {
            detail.Master = this;
            NodeDetails.Add(detail);
        }
        virtual public void AddRecordDetail(IrpRiskDepositPayRecord detail)
        {
            detail.Master = this;
            RecordDetails.Add(detail);
        }

        //virtual public string Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        /// <summary>
        /// 安全文明施工目标
        /// </summary>
        virtual public string SafetyCivilizedSign
        {
            get { return safetyCivilizedSign; }
            set { safetyCivilizedSign = value; }
        }
        /// <summary>
        /// 安装施工部分配合费比率
        /// </summary>
        virtual public decimal InstallationFreeRate
        {
            get { return installationFreeRate; }
            set { installationFreeRate = value; }
        }
        /// <summary>
        /// 成本控制奖励比率
        /// </summary>
        virtual public decimal CostcontrolRewardtatio
        {
            get { return costcontrolRewardtatio; }
            set { costcontrolRewardtatio = value; }
        }
        /// <summary>
        /// 成本控制目标
        /// </summary>
        virtual public decimal CostcontrolTarget
        {
            get { return costcontrolTarget; }
            set { costcontrolTarget = value; }
        }
        /// <summary>
        /// 兑现奖励节点数量
        /// </summary>
        virtual public decimal CashRewardNodeNumber
        {
            get { return cashRewardNodeNumber; }
            set { cashRewardNodeNumber = value; }
        }
        /// <summary>
        /// 风险抵押金缴纳状况
        /// </summary>
        virtual public string RiskPaymentState
        {
            get { return riskPaymentState; }
            set { riskPaymentState = value; }
        }
        /// <summary>
        /// 风险化解奖励比率
        /// </summary>
        virtual public decimal RiskrewardRatio
        {
            get { return riskRewardRatio; }
            set { riskRewardRatio = value; }
        }
        /// <summary>
        /// 风险化解目标
        /// </summary>
        virtual public decimal RiskDissolvesTarget
        {
            get { return riskDissolvesTarget; }
            set { riskDissolvesTarget = value; }
        }
        /// <summary>
        /// 工期
        /// </summary>
        virtual public string ProjectDate
        {
            get { return projectDate; }
            set { projectDate = value; }
        }
        /// <summary>
        /// 工期计量单位
        /// </summary>
        virtual public string PeriodmeaSuringUnit
        {
            get { return periodmeaSuringUnit; }
            set { periodmeaSuringUnit = value; }
        }
        /// <summary>
        /// 计划竣工时间
        /// </summary>
        virtual public DateTime PlanEndDate
        {
            get { return planEndDate; }
            set { planEndDate = value; }
        }
        /// <summary>
        /// 计划开始时间
        /// </summary>
        virtual public DateTime PlanBeginDate
        {
            get { return planBeginDate; }
            set { planBeginDate = value; }
        }
        /// <summary>
        /// 计量单位名称
        /// </summary>
        virtual public string PrickleName
        {
            get { return prickleName; }
            set { prickleName = value; }
        }
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public StandardUnit PricePrickle
        {
            get { return pricePrickle; }
            set { pricePrickle = value; }
        }
        /// <summary>
        /// 经济目标责任范围不含工程
        /// </summary>
        virtual public string EconomicgoalEnginner
        {
            get { return economicgoalEnginner; }
            set { economicgoalEnginner = value; }
        }
        /// <summary>
        /// 确保工程质量等级
        /// </summary>
        virtual public string EnsureLevel
        {
            get { return ensureLevel; }
            set { ensureLevel = value; }
        }
        /// <summary>
        /// 是否签订
        /// </summary>
        virtual public string SignedWhether
        {
            get { return signedWhether; }
            set { signedWhether = value; }
        }
        /// <summary>
        /// 文档名称
        /// </summary>
        virtual public string DocumentName
        {
            get { return documentName; }
            set { documentName = value; }
        }
        /// <summary>
        /// 项目GUID 
        /// </summary>
        virtual public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        /// <summary>
        /// 项目规模
        /// </summary>
        virtual public string ProjectScale
        {
            get { return projectScale; }
            set { projectScale = value; }
        }
        /// <summary>
        /// 项目经理
        /// </summary>
        virtual public string ProjectManagerName
        {
            get { return projectManagerName; }
            set { projectManagerName = value; }
        }
        /// <summary>
        /// 项目经理GUID
        /// </summary>
        virtual public PersonInfo ProjectManagerId
        {
            get { return projectManagerId; }
            set { projectManagerId = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        virtual public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        /// <summary>
        /// 业务指定分包费用自提比率
        /// </summary>
        virtual public decimal ServiceFeeRates
        {
            get { return serviceFeeRates; }
            set { serviceFeeRates = value; }
        }
        /// <summary>
        /// 业主奖励自提比率
        /// </summary>
        virtual public decimal OwneRawardsRatio
        {
            get { return owneRawardsRatio; }
            set { owneRawardsRatio = value; }
        }
        /// <summary>
        /// 责任范围外分包工程利润自提比率
        /// </summary>
        virtual public decimal ResponsibilityRatio
        {
            get { return responsibilityRatio; }
            set { responsibilityRatio = value; }
        }
        /// <summary>
        /// 责任上缴奖励比率
        /// </summary>
        virtual public decimal ResponsibilityRewardTatio
        {
            get { return responsibilityRewardTatio; }
            set { responsibilityRewardTatio = value; }
        }
        /// <summary>
        /// 责任上缴目标
        /// </summary>
        virtual public decimal ResponsibilityTurnedTarget
        {
            get { return responsibilityTurnedTarget; }
            set { responsibilityTurnedTarget = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        virtual public decimal State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// 责任人
        /// </summary>
        virtual public string HandlePerson
        {
            get { return handlePerson; }
            set { handlePerson = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
    }
}
