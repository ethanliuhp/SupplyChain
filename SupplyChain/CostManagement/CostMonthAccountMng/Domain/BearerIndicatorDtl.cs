using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 项目分包指标统计表
    /// </summary>
    [Serializable]
    [Entity]
    public class BearerIndicatorDtl : BaseDetail
    {
        private string constructionTeam;    //施工队伍
        private string constructionContent; //合同施工内容
        private decimal currenContractMoney;     //本期合同金额
        private decimal currentSettleMoney;    //本期结算金额
        private decimal currentOuterSettleMoney;   //本期合同外结算金额
        private string isConrresponding;    //是否对应收入
        private decimal incomeMoney;    //本期收入金额
        private decimal currentSelfSignMoney;  //本期自签协议金额
        private decimal accrueSettleMoney; //累计结算金额
        private decimal accrueOuterSettleMoney; //累计合同外结算金额
        private decimal accrueSelfSignMoney;    //累计自签协议金额
        private decimal currentOemMoney;    //本期代工金额
        private decimal currentBeOemMoney;  //本期被代工金额
        private decimal accrueOemMoney; //累计代工金额
        private decimal accrueBeOemMoney;   //累计被代工金额
        private decimal currentHourlyMoney; //本期计时工金额
        private string currentHourlyRate; //本期计时工比例
        private decimal accrueHourlyMoney;  //累计计时工金额
        private string accrueHourlyRate;    //累计计时工比例
        private int orderNo;//序号
        /// <summary>
        /// 序号
        /// </summary>
        virtual public int OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
        private string flag;    //专业分包和劳务分包的区分标记  20160824 
        /// <summary>
        /// 施工队伍
        /// </summary>
        virtual public string ConstructionTeam
        {
            get { return constructionTeam; }
            set { constructionTeam = value; }
        }
        /// <summary>
        /// 合同施工内容
        /// </summary>
        virtual public string ConstructionContent
        {
            get { return constructionContent; }
            set { constructionContent = value; }
        }
        /// <summary>
        /// 本期合同金额
        /// </summary>
        virtual public decimal CurrenContractMoney
        {
            get { return currenContractMoney; }
            set { currenContractMoney = value; }
        }
        /// <summary>
        /// 本期结算金额
        /// </summary>
        virtual public decimal CurrentSettleMoney
        {
            get { return currentSettleMoney; }
            set { currentSettleMoney = value; }
        }
        /// <summary>
        /// 本期合同外结算金额
        /// </summary>
        virtual public decimal CurrentOuterSettleMoney
        {
            get { return currentOuterSettleMoney; }
            set { currentOuterSettleMoney = value; }
        }
        /// <summary>
        /// 是否对应收入
        /// </summary>
        virtual public string IsConrresponding
        {
            get { return isConrresponding; }
            set { isConrresponding = value; }
        }
        /// <summary>
        /// 本期收入金额
        /// </summary>
        virtual public decimal IncomeMoney
        {
            get { return incomeMoney; }
            set { incomeMoney = value; }
        }
        /// <summary>
        /// 本期自签协议金额
        /// </summary>
        virtual public decimal CurrentSelfSignMoney
        {
            get { return currentSelfSignMoney; }
            set { currentSelfSignMoney = value; }
        }
        /// <summary>
        /// 累计结算金额
        /// </summary>
        virtual public decimal AccrueSettleMoney
        {
            get { return accrueSettleMoney; }
            set { accrueSettleMoney = value; }
        }
        /// <summary>
        /// 累计合同外结算金额
        /// </summary>
        virtual public decimal AccrueOuterSettleMoney
        {
            get { return accrueOuterSettleMoney; }
            set { accrueOuterSettleMoney = value; }
        }
        /// <summary>
        /// 累计自签协议金额
        /// </summary>
        virtual public decimal AccrueSelfSignMoney
        {
            get { return accrueSelfSignMoney; }
            set { accrueSelfSignMoney = value; }
        }
        /// <summary>
        /// 本期代工金额
        /// </summary>
        virtual public decimal CurrentOemMoney
        {
            get { return currentOemMoney; }
            set { currentOemMoney = value; }
        }
        /// <summary>
        /// 本期被代工金额
        /// </summary>
        virtual public decimal CurrentBeOemMoney
        {
            get { return currentBeOemMoney; }
            set { currentBeOemMoney = value; }
        }
        /// <summary>
        /// 累计代工金额
        /// </summary>
        virtual public decimal AccrueOemMoney
        {
            get { return accrueOemMoney; }
            set { accrueOemMoney = value; }
        }
        /// <summary>
        /// 累计被代工金额
        /// </summary>
        virtual public decimal AccrueBeOemMoney
        {
            get { return accrueBeOemMoney; }
            set { accrueBeOemMoney = value; }
        }
        /// <summary>
        /// 本期计时工金额
        /// </summary>
        virtual public decimal CurrentHourlyMoney
        {
            get { return currentHourlyMoney; }
            set { currentHourlyMoney = value; }
        }
        /// <summary>
        /// 本期计时工比例
        /// </summary>
        virtual public string CurrentHourlyRate
        {
            get { return currentHourlyRate; }
            set { currentHourlyRate = value; }
        }
        /// <summary>
        /// 累计计时工金额
        /// </summary>
        virtual public decimal AccrueHourlyMoney
        {
            get { return accrueHourlyMoney; }
            set { accrueHourlyMoney = value; }
        }
        /// <summary>
        /// 累计计时工比例
        /// </summary>
        virtual public string AccrueHourlyRate
        {
            get { return accrueHourlyRate; }
            set { accrueHourlyRate = value; }
        }
        /// <summary>
        /// 专业分包和劳务分包的区分标记
        /// </summary>
        virtual public string FLAG
        {
            get { return flag; }
            set { flag = value; }
        }

    }
}
