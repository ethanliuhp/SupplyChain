using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    
    [Serializable]
    [Entity]
    public class ProjectBalanceOfPayment : BaseMaster
    {
        private string subCompanyName;
        private decimal contractTotal;
        private string contractContent;
        private string projectType;
        private string ownerType;
        private string projectState;
        private decimal finanBalanceTotal;
        private decimal finanBalanceCurrent;
        private decimal ownerSure;
        private decimal ownerSureTJ;
        private DateTime ownerSureLastTime = DateTime.MinValue;
        private string ownerSureYearMonth;
        private decimal cbCostPaymentTotal;
        private decimal tjCostPaymentTotal;
        private string   cbSureRate;
        private string   tjSureRate;
        private decimal contractGatheringRate;
        private decimal contractGathering;
        private decimal mustNotGathering;
        private string delayTime;
        private decimal mainBusinessTotal;
        private decimal mainBusinessCurrYear;
        private decimal mainBusinessCurrMonth;
        private decimal  mainBusinessGatheringTotalRate;
        private decimal  mainBusinessGatheringCurrYearRate;
        private decimal gatheringMoneyTotalRate;
        private decimal gatheringMoneyCurrYearRate;
        private decimal cbProjectGatheringTotal;
        private decimal cbProjectGatheringCurrYear;
        private decimal cbProjectGatheringCurrMonth;
        private decimal cbProjectPaymentTotal;
        private decimal cbProjectPaymentCurrYear;
        private decimal cbProjectPaymentCurrMonth;
        private decimal cbMoneyRemainTotal;
        private decimal cbMoneyRemainCurrYear;
        private decimal cbMoneyRemainCurrMonth;

        private decimal tjProjectGatheringTotal;
        private decimal tjProjectGatheringCurrYear;
        private decimal tjProjectGatheringCurrMonth;
        private decimal tjProjectPaymentTotal;
        private decimal tjProjectPaymentCurrYear;
        private decimal tjProjectPaymentCurrMonth;
        private decimal tjMoneyRemainTotal;
        private decimal tjMoneyRemainCurrYear;
        private decimal tjMoneyRemainCurrMonth;
        private string warnCause;
        private string   warnMoneyRemain;
        private string  warnMoneyFlow;
        private string  warnMustNotGathering;
        private decimal finanYearEndMustGathering;
        private decimal finanYearEndNotSureMoney;
        private decimal finanYearEndTotal;
        private decimal finanYearBeginMustGathering;
        private decimal finanYearBeginNotSureMoney;
        private decimal finanYearBeginTotal;
        private decimal finanIncreaseMustGathering;
        private decimal finanIncreaseNotSureMoney;
        private decimal finanIncreaseTotal;
        private decimal mustPayment;
        private decimal otherMustPayment;
        private decimal cbHandUpRate;
        /// <summary>
        /// 分公司名称
        /// </summary>
        public virtual string SubCompanyName
        {
            get { return subCompanyName; }
            set {  subCompanyName=value; }
        }
        /// <summary>
        /// 合同额
        /// </summary>
        public virtual decimal ContractTotal
        {
            get { return contractTotal; }
            set { contractTotal = value; }
        }
        /// <summary>
        /// 合同条款
        /// </summary>
        public virtual string ContractContent
        {
            get { return contractContent; }
            set { contractContent = value; }
        }
        /// <summary>
        /// 工程类型
        /// </summary>
        public virtual string ProjectType
        {
            get { return projectType; }
            set { projectType = value; }
        }
        /// <summary>
        /// 业主类型
        /// </summary>
        public virtual string OwnerType {
            get { return ownerType; }
            set { ownerType = value; }
        }
        /// <summary>
        /// 工程状态
        /// </summary>
        public virtual string ProjectState
        {
            get { return projectState; }
            set { projectState = value; }
        }
        /// <summary>
        /// 工程结算   财务列报结算  累计数 
        /// </summary>
        public virtual decimal FinanBalanceTotal
        {
            get { return finanBalanceTotal; }
            set { finanBalanceTotal = value; }
        }
        /// <summary>
        /// 工程结算   财务列报结算  本年数
        /// </summary>
        public virtual decimal FinanBalanceCurrent
        {
            get { return finanBalanceCurrent; }
            set { finanBalanceCurrent = value; }
        }
        /// <summary>
        /// 工程结算 实际结算  业主实际确认结算额 												 
        /// </summary>
        public virtual decimal OwnerSure
        {
            get { return ownerSure; }
            set { ownerSure = value; }
        }
        /// <summary>
        ///  工程结算 实际结算  业主实际确认土建结算额 		
        /// </summary>
        public virtual decimal OwnerSureTJ
        {
            get { return ownerSureTJ; }
            set { ownerSureTJ = value; }
        }
        /// <summary>
        ///工程结算 实际结算  最后一次业主审量签字时间
        /// </summary>
        public virtual DateTime OwnerSureLastTime
        {
            get { return ownerSureLastTime; }
            set { ownerSureLastTime = value; }
        }
        /// <summary>
        /// 工程结算 实际结算  业主审量确认到*年*月
        /// </summary>
        public virtual string OwnerSureYearMonth
        {
            get { return ownerSureYearMonth; }
            set { ownerSureYearMonth = value; }
        }
        /// <summary>
        /// 工程结算  总包累计成本支出 
        /// </summary>
        public virtual decimal CBCostPaymentTotal
        {
            get { return cbCostPaymentTotal; }
            set { cbCostPaymentTotal = value; }
        }
        /// <summary>
        /// 工程结算  土建累计成本支出
        /// </summary>
        public virtual decimal TJCostPaymentTotal
        {
            get { return tjCostPaymentTotal; }
            set { tjCostPaymentTotal = value; }
        }
        /// <summary>
        /// 工程结算 总包确权率
        /// </summary>
        public virtual string CBSureRate
        {
            get { return CBCostPaymentTotal == 0 ? "-" : Math.Round((OwnerSure / CBCostPaymentTotal) * 100, 2).ToString() ; ; }
            set { cbSureRate = value; }
        }
        /// <summary>
        /// 工程结算 土建确权率
        /// </summary>
        public virtual string  TJSureRate
        {
            get { return TJCostPaymentTotal == 0 ? "-" : Math.Round((OwnerSureTJ / TJCostPaymentTotal) * 100, 2).ToString() ; ; }
            set { tjSureRate = value; }
        }
        /// <summary>
        ///  应收欠款  当前工程状态合同收款率 
        /// </summary>
        public virtual decimal ContractGatheringRate
        {
            get { return contractGatheringRate; }
            set { contractGatheringRate = value; }
        }
        /// <summary>
        /// 应收欠款   按合同应收款 
        /// </summary>
        public virtual decimal ContractGathering
        {
            get { return contractGathering; }
            set { contractGathering = value; }
        }
        /// <summary>
        ///  应收欠款  应收未回收款 
        /// </summary>
        public virtual decimal MustNotGathering
        {
            get
            {
                mustNotGathering = ContractGathering - CBProjectGatheringTotal;
                return mustNotGathering;
            }
            set { mustNotGathering = value; }
        }
        /// <summary>
        /// 应收欠款  应收欠款时间
        /// </summary>
        public virtual string DelayTime
        {
            get { return delayTime; }
            set { delayTime = value; }
        }
        /// <summary>
        ///  主营业务收入 累计数
        /// </summary>
        public virtual decimal MainBusinessTotal
        {
            get { return mainBusinessTotal; }
            set { mainBusinessTotal = value; }
        }
        /// <summary>
        ///  主营业务收入 本年数
        /// </summary>
        public virtual decimal MainBusinessCurrYear
        {
            get { return mainBusinessCurrYear; }
            set { mainBusinessCurrYear = value; }
        }
        /// <summary>
        ///  主营业务收入 本月数
        /// </summary>
        public virtual decimal MainBusinessCurrMonth
        {
            get { return mainBusinessCurrMonth; }
            set { mainBusinessCurrMonth = value; }
        }
        /// <summary>
        /// 主营业务收入回款率 累计回款率
        /// </summary>
        public virtual decimal MainBusinessGatheringTotalRate
        {
            get { return MainBusinessTotal == 0 ? decimal.MaxValue : Math.Round((CBProjectGatheringTotal / MainBusinessTotal)*100, 2); }
            set { mainBusinessGatheringTotalRate = value; }
        }
        /// <summary>
        /// 主营业务收入回款率 本年回款率
        /// </summary>
        public virtual decimal  MainBusinessGatheringCurrYearRate
        {
            get { return MainBusinessCurrYear == 0 ? decimal.MaxValue : Math.Round((CBProjectGatheringCurrYear / MainBusinessCurrYear) * 100, 2); }
            set {  mainBusinessGatheringCurrYearRate=value; }
        }
        /// <summary>
        /// 收现率	累计收现率
        /// </summary>
        public virtual decimal GatheringMoneyTotalRate
        {
            get { return gatheringMoneyTotalRate; }
            set { gatheringMoneyTotalRate = value; }
        }
        /// <summary>
        /// 收现率	本年收现率
        /// </summary>r
        public virtual decimal GatheringMoneyCurrYearRate
        {
            get { return gatheringMoneyCurrYearRate; }
            set { gatheringMoneyCurrYearRate = value; }
        }
        /// <summary>
        /// 总包收工程款 累计数		
        /// </summary>
        public virtual decimal CBProjectGatheringTotal
        {
            get { return cbProjectGatheringTotal; }
            set { cbProjectGatheringTotal = value; }
        }
        /// <summary>
        /// 总包收工程款 本年数		
        /// </summary>
        public virtual decimal CBProjectGatheringCurrYear
        {
            get { return cbProjectGatheringCurrYear; }
            set { cbProjectGatheringCurrYear = value; }
        }
        /// <summary>
        /// 总包收工程款 本月数		
        /// </summary>
        public virtual decimal CBProjectGatheringCurrMonth
        {
            get { return cbProjectGatheringCurrMonth; }
            set { cbProjectGatheringCurrMonth = value; }
        }
        /// <summary>
        /// 总包资金支出 累计数
        /// </summary>
        public virtual decimal CBProjectPaymentTotal
        {
            get { return cbProjectPaymentTotal; }
            set { cbProjectPaymentTotal = value; }
        }
        /// <summary>
        /// 总包资金支出 本年数
        /// </summary>
        public virtual decimal CBProjectPaymentCurrYear
        {
            get { return cbProjectPaymentCurrYear; }
            set { cbProjectPaymentCurrYear = value; }
        }
        /// <summary>
        /// 总包资金支出 本月数
        /// </summary>
        public virtual decimal CBProjectPaymentCurrMonth
        {
            get { return cbProjectPaymentCurrMonth; }
            set { cbProjectPaymentCurrMonth = value; }
        }
        /// <summary>
        /// 土建收工程款 累计数		
        /// </summary>
        public virtual decimal TJProjectGatheringTotal
        {
            get { return tjProjectGatheringTotal; }
            set { tjProjectGatheringTotal = value; }
        }
        /// <summary>
        /// 土建收工程款 本年数		
        /// </summary>
        public virtual decimal TJProjectGatheringCurrYear
        {
            get { return tjProjectGatheringCurrYear; }
            set { tjProjectGatheringCurrYear = value; }
        }
        /// <summary>
        /// 土建收工程款 本月数		
        /// </summary>
        public virtual decimal TJProjectGatheringCurrMonth
        {
            get { return tjProjectGatheringCurrMonth; }
            set { tjProjectGatheringCurrMonth = value; }
        }
        /// <summary>
        /// 土建资金支出 累计数
        /// </summary>
        public virtual decimal TJProjectPaymentTotal
        {
            get { return tjProjectPaymentTotal; }
            set { tjProjectPaymentTotal = value; }
        }
        /// <summary>
        /// 土建资金支出 本年数
        /// </summary>
        public virtual decimal TJProjectPaymentCurrYear
        {
            get { return tjProjectPaymentCurrYear; }
            set { tjProjectPaymentCurrYear = value; }
        }
        /// <summary>
        /// 土建资金支出 本月数
        /// </summary>
        public virtual decimal TJProjectPaymentCurrMonth
        {
            get { return tjProjectPaymentCurrMonth; }
            set { tjProjectPaymentCurrMonth = value; }
        }
        /// <summary>
        /// 总包资金净额 累计
        /// </summary>
        public virtual decimal CBMoneyRemainTotal
        {
            get {//总包收工程款-总包资金支出
                cbMoneyRemainTotal = CBProjectGatheringTotal - cbProjectPaymentTotal;
                return cbMoneyRemainTotal; }
            set { cbMoneyRemainTotal = value; }
        }
        /// <summary>
        /// 总包资金净额 年累计
        /// </summary>
        public virtual decimal CBMoneyRemainCurrYear
        {
            get
            {
                cbMoneyRemainCurrYear = CBProjectGatheringCurrYear - CBProjectPaymentCurrYear;
                return cbMoneyRemainCurrYear; }
            set { cbMoneyRemainCurrYear = value; }
        }
        /// <summary>
        /// 总包资金净额 月累计
        /// </summary>
        public virtual decimal CBMoneyRemainCurrMonth
        {
            get
            {
                cbMoneyRemainCurrMonth = CBProjectGatheringCurrMonth - CBProjectPaymentCurrMonth;
                return cbMoneyRemainCurrMonth; }
            set { cbMoneyRemainCurrMonth = value; }
        }
        /// <summary>
        /// 土建资金净额 累计
        /// </summary>
        public virtual decimal TJMoneyRemainTotal
        {
            ////土建收工程款-土建资金支出
            get
            {
                tjMoneyRemainTotal = TJProjectGatheringTotal - TJProjectPaymentTotal;
                return tjMoneyRemainTotal;
            }
            set { tjMoneyRemainTotal = value; }
        }
        /// <summary>
        /// 土建资金净额 年累计
        /// </summary>
        public virtual decimal TJMoneyRemainCurrYear
        {
            get
            {
                tjMoneyRemainCurrYear = TJProjectGatheringCurrYear - TJProjectPaymentCurrYear;
                return tjMoneyRemainCurrYear; }
            set { tjMoneyRemainCurrYear = value; }
        }
        /// <summary>
        /// 土建资金净额 月累计
        /// </summary>
        public virtual decimal TJMoneyRemainCurrMonth
        {
            get
            {
                tjMoneyRemainCurrMonth = TJProjectGatheringCurrMonth - TJProjectPaymentCurrMonth;
                return tjMoneyRemainCurrMonth;
            }
            set { tjMoneyRemainCurrMonth = value; }
        }
        /// <summary>
        /// 风险分析预警 负现金流项目原因分析
        /// </summary>
        public virtual string WarnCause
        {
            get { return warnCause; }
            set { warnCause = value; }
        }
        /// <summary>
        /// 风险分析预警 现金存量预警
        /// </summary>
        public virtual string  WarnMoneyRemain
        {
            get {
                //黄色预警  -5000000>=资金净额>-10000000
                //橙色预警 -10000000>=资金净额>-20000000
                //红色预警 -20000000>=资金净额
                //warnMoneyRemain = CBMoneyRemainTotal < -2000 ? "红色预警" :
                //    (CBMoneyRemainTotal < -1000 ? "橙色预警" :
                //    (CBMoneyRemainTotal < -500 ? "黄色预警" : "-"));
                if (CBMoneyRemainTotal <= -5000000 && CBMoneyRemainTotal >-10000000)
                {
                    warnMoneyRemain = "黄色预警";
                }
                else if (CBMoneyRemainTotal <= -10000000 && CBMoneyRemainTotal > -20000000)
                {
                    warnMoneyRemain = "橙色预警";
                }
                else if (CBMoneyRemainTotal <= -20000000)
                {
                    warnMoneyRemain = "红色预警";
                }
                else
                {
                    warnMoneyRemain = "-";
                }
                return warnMoneyRemain; }
            set { warnMoneyRemain = value; }
        }
        /// <summary>
        /// 风险分析预警 收现率预警
        /// </summary>
        public virtual string WarnMoneyFlow
        {
            
            get {//=IF(Y6<(R6-15%),"红色预警",IF(Y6<(R6-10%),"橙色预警",IF(Y6<R6,"黄色预警","-")))
                //Y主营业务收入累计回款率  R当前工程状态合同收款率
                //累计收现率与合同约定合同回款率
                //黄色预警     合同回款率-收现率<10
                //橙色预警     10<=合同回款率-收现率<15
                //红色预警     15<=合同回款率-收现率
                decimal dTemp = ContractGatheringRate - MainBusinessGatheringTotalRate;
                if (dTemp>0 && dTemp < 10)
                {
                    warnMoneyFlow = "黄色预警";
                }
                else if (dTemp >= 10 && dTemp < 15)
                {
                    warnMoneyFlow = "橙色预警";
                }
                else if (dTemp >= 15)
                {
                    warnMoneyFlow = "红色预警";
                }
                else
                {
                    warnMoneyFlow = "-";
                }
                //warnMoneyFlow= MainBusinessGatheringTotalRate<(contractGatheringRate-15)?"红色预警":
                //    (MainBusinessGatheringTotalRate<(contractGatheringRate-10)?"橙色预警":
                //    (MainBusinessGatheringTotalRate<contractGatheringRate?"黄色预警":"-"));
                return warnMoneyFlow; 
            }
            set { warnMoneyFlow = value; }
        }
        /// <summary>
        /// 风险分析预警 应收欠款预警
        /// </summary>
        public virtual string WarnMustNotGathering
        {//=IF(T6>1000,IF(U6="3个月及以上","红色预警",IF(U6="2个月","橙色预警",IF(U6="1个月","黄色预警","-"))),"-")
            //T:应收未回收款  U:应收欠款时间
            get
            {
                //"1个月以内", "1个月", "2个月", "3个月及以上
                //黄色预警  一个月 1000万<=应收欠款<2000万
                //橙色预警  一个月 2000万<=应收欠款
                //橙色预警  二个月 1000万<=应收欠款<2000万
                //红色预警  二个月  2000万<=应收欠款
                //红色预警  三个月  500万<=应收欠款
                if (string.Equals(DelayTime, "1个月"))//一个月   不包括一个月以内
                {
                    if (MustNotGathering >= 10000000 && MustNotGathering < 20000000)
                    {
                        warnMustNotGathering = "黄色预警";
                    }
                    else if (MustNotGathering >= 20000000)
                    {
                        warnMustNotGathering = "橙色预警";
                    }
                    else
                    {
                        warnMustNotGathering = "-";
                    }
                }
                else if (string.Equals(DelayTime, "2个月"))
                {
                    if (MustNotGathering >= 10000000 && MustNotGathering < 20000000)
                    {
                        warnMustNotGathering = "橙色预警";
                    }
                    else if (MustNotGathering >= 20000000)
                    {
                        warnMustNotGathering = "红色预警";
                    }
                    else
                    {
                        warnMustNotGathering = "-";
                    }
                }
                else if (string.Equals(DelayTime, "3个月及以上"))
                {
                    if (MustNotGathering >= 5000000)
                    {
                        warnMustNotGathering = "红色预警";
                    }
                    else
                    {
                        warnMustNotGathering = "-";
                    }
                }
                else
                {
                    warnMustNotGathering = "-";
                }
                return warnMustNotGathering;
            }
            set { warnMustNotGathering = value; }
        }
        /// <summary>
        /// 财务列报期末应收款 应收账款
        /// </summary>
        public virtual decimal FinanYearEndMustGathering
        {
            //H6-AA6>=0?H6-AA6:0  H:财务列报结算累计数 AA:总包工程收款累计数
            get
            {
                decimal dTemp = FinanBalanceTotal - CBProjectGatheringTotal;
                finanYearEndMustGathering = dTemp >= 0 ? dTemp : 0;
                return finanYearEndMustGathering;
            }
            set { finanYearEndMustGathering = value; }
        }
        /// <summary>
        ///财务列报期末应收款 完工未确认款
        /// </summary>
        public virtual decimal FinanYearEndNotSureMoney
        {
            //V6-H6>=0?V6-H6:0
            //V主营业务收入累计数  H:财务列报结算累计数
            get {
                decimal dTemp = MainBusinessTotal-FinanBalanceTotal;
                finanYearEndNotSureMoney = dTemp >= 0 ? dTemp : 0;
                return finanYearEndNotSureMoney; }
            set { finanYearEndNotSureMoney = value; }
        }
        /// <summary>
        ///财务列报期末应收款 合计
        /// </summary>
        public virtual decimal FinanYearEndTotal
        {
            //AY+AZ  AY:财务列报期末应收款 应收账款 AZ:财务列报期末应收款 完工未确认款
            get
            {
                finanYearEndTotal = FinanYearEndMustGathering + FinanYearEndNotSureMoney;
                return finanYearEndTotal; }
            set { finanYearEndTotal = value; }
        }
        /// <summary>
        /// 财务列报年初应收款 应收账款
        /// </summary>
        public virtual decimal FinanYearBeginMustGathering
        {//(H6-I6)-(AA6-AB6):(财务列报结算累计数-财务列报结算本年数)-(总包工程收款累计数累计数-总包工程收款累计数本年数)
            get { decimal dTemp=(FinanBalanceTotal-FinanBalanceCurrent)-(CBProjectGatheringTotal-CBProjectGatheringCurrYear);
            finanYearBeginMustGathering = dTemp >= 0 ? dTemp : 0;
                return finanYearBeginMustGathering; }
            set { finanYearBeginMustGathering = value; }
        }
        /// <summary>
        ///财务列报年初应收款 完工未确认款
        /// </summary>
        public virtual decimal FinanYearBeginNotSureMoney
        {//(V-W)-(H-I)=(主营业务收入累计数-主营业务收入本年数)-(财务列报结算累计数-财务列报结算本年数)
           
            get { 
                decimal dTemp=(MainBusinessTotal-MainBusinessCurrYear)-(FinanBalanceTotal-FinanBalanceCurrent);
                finanYearBeginNotSureMoney = dTemp >= 0 ? dTemp : 0;
                return finanYearBeginNotSureMoney; }
            set { finanYearBeginNotSureMoney = value; }
        }
        /// <summary>
        ///财务列报年初应收款 合计
        /// </summary>
        public virtual decimal FinanYearBeginTotal
        {//BD+BE =财务列报年初应收款应收账款+财务列报年初应收款 完工未确认款
            get
            {
                finanYearBeginTotal = FinanYearBeginMustGathering + FinanYearBeginNotSureMoney;
                return finanYearBeginTotal;
            }
            set { finanYearBeginTotal = value; }
        }
        /// <summary>
        /// 财务列报应收款增长额 应收账款
        /// </summary>
        public virtual decimal FinanIncreaseMustGathering
        {
            //AY-BD 财务列报期末应收款 应收账款-财务列报年初应收款 应收账款
            get
            {
                finanIncreaseMustGathering = FinanYearEndMustGathering - FinanYearBeginMustGathering;
                return finanIncreaseMustGathering; }
            set { finanIncreaseMustGathering = value; }
        }
        /// <summary>
        ///财务列报应收款增长额 完工未确认款
        /// </summary>
        public virtual decimal FinanIncreaseNotSureMoney
        {//AZ-BE 财务列报期末应收款 完工未确认款-财务列报年初应收款 完工未确认款
            get
            {
                finanIncreaseNotSureMoney = FinanYearEndNotSureMoney - finanYearBeginNotSureMoney;
                return finanIncreaseNotSureMoney; }
            set { finanIncreaseNotSureMoney = value; }
        }
        /// <summary>
        ///财务列报应收款增长额 合计
        /// </summary>
        public virtual decimal FinanIncreaseTotal
        {//财务列报应收款增长额 应收账款+财务列报应收款增长额 完工未确认款
            get
            {
                finanIncreaseTotal = FinanIncreaseMustGathering + FinanIncreaseNotSureMoney;
                return finanIncreaseTotal;
            }
            set { finanIncreaseTotal = value; }
        }
        /// <summary>
        /// 应付款项 应付账款	
        /// </summary>
        public virtual decimal MustPayment
        {
            get { return mustPayment; }
            set { mustPayment = value; }
        }
        /// <summary>
        /// 应付款项 其他应付款
        /// </summary>
        public virtual decimal OtherMustPayment
        {
            get { return otherMustPayment; }
            set { otherMustPayment = value; }
        }
        /// <summary>
        /// 责任上缴比例
        /// </summary>
        public virtual decimal CBHandUpRate
        {
            get { return cbHandUpRate; }
            set { cbHandUpRate = value; }
        }
        
    }
}
