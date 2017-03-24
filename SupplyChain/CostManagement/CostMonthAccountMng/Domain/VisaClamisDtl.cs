using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 签证索赔情况表
    /// </summary>
    [Serializable]
    [Entity]
    public class VisaClamisDtl : BaseDetail
    {
        private int  ybsqzfs;    //已报送签证份数
        private int   yqrqzfs; //已确认签证份数
        private decimal qzljbsje;     //签证累计报送金额(万元)
        private decimal ljqrsr;    //累计确认收入（万元）(其中业主累计确认签证)
        private decimal ljsjsr;   //累计实际成本（万元）(其中业主累计确认签证)
        private string  ljqrqzbfsyl;    //累计确认签证部分的效益率(%)(其中业主累计确认签证)
        private decimal ljbsje;    //累计报送金额（万元) (其中业主未确认签证)
        private decimal ljsjcb;  // 累计实际成本 (其中业主未确认签证)
        private decimal qzljsjcb; //签证累计实际成本（C1+C2）
        private string qzxyl; //签证效益率(%)（S/(C1+C2)-1）
        private decimal jzljzxsgcz;    //截止5月10日累计自行施工产值
        private string qzzedczgxl;    //签证总额对产值贡献率（%）
        private string qzxyczgxl;  //签证效益产值贡献率（%）
        private int projectDeclareCount;//立项申报情况		立项申报个数
        private decimal projectRiskSolution;//立项申报情况		风险化解（万元）
        private decimal projectAddBenefit;// 立项申报情况		增加效益（万元）
        private int rewardDeclareCount; //奖励申报情况		奖励申报个数
        private decimal rewardRiskSolution;//奖励申报情况		风险化解（万元）
        private decimal rewardAddBenefit;//奖励申报情况		增加效益（万元）
        private decimal outPutMoney;//已发放奖励金额（万元）
        private string selfMoneyRate;//已实施策划占自行产值比例（%）
        /// <summary>
        /// 奖励申报情况		立项申报个数
        /// </summary>
        public  virtual int ProjectDeclareCount
        {
            get { return projectDeclareCount; }
            set { projectDeclareCount = value; }
        }
        /// <summary>
        ///奖励申报情况		 风险化解（万元）
        /// </summary>
        public  virtual decimal ProjectRiskSolution
        {
            get { return projectRiskSolution; }
            set { projectRiskSolution = value; }
        }
        /// <summary>
        /// 立项申报情况		增加效益（万元）
        /// </summary>
        public virtual decimal ProjectAddBenefit
        {
            get { return projectAddBenefit; }
            set { projectAddBenefit = value; }
        }
        /// <summary>
        /// 奖励申报情况		奖励申报个数
        /// </summary>
        public virtual int RewardDeclareCount
        {
            get { return rewardDeclareCount; }
            set { rewardDeclareCount = value; }
        }
        /// <summary>
        /// 奖励申报情况		风险化解（万元）
        /// </summary>
        public virtual decimal RewardRiskSolution
        {
            get { return rewardRiskSolution; }
            set { rewardRiskSolution = value; }
        }
        /// <summary>
        /// 奖励申报情况		增加效益（万元）
        /// </summary>
        public virtual decimal RewardAddBenefit
        {
            get { return rewardAddBenefit; }
            set { rewardAddBenefit = value; }
        }
        /// <summary>
        /// 已发放奖励金额（万元）
        /// </summary>
        public virtual decimal OutPutMoney
        {
            get { return outPutMoney; }
            set { outPutMoney = value; }
        }
        /// <summary>
        /// 已实施策划占自行产值比例（%）
        /// </summary>
        public virtual string SelfMoneyRate
        {
            get { return selfMoneyRate; }
            set { selfMoneyRate = value; }
        }
        virtual public int YBSQZFS
        {
            get { return ybsqzfs; }
            set { ybsqzfs = value; }
        }

        virtual public int YQRQZFS
        {
            get { return yqrqzfs; }
            set { yqrqzfs = value; }
        }

        virtual public decimal QZLJBSJE
        {
            get { return qzljbsje; }
            set { qzljbsje = value; }
        }

        virtual public decimal LJQRSR
        {
            get { return ljqrsr; }
            set { ljqrsr = value; }
        }

        virtual public decimal LSSJSR
        {
            get { return ljsjsr; }
            set { ljsjsr = value; }
        }

        virtual public string LJQRQZBFSYL
        {
            get { return ljqrqzbfsyl; }
            set { ljqrqzbfsyl = value; }
        }

        virtual public decimal LJBSJE
        {
            get { return ljbsje; }
            set { ljbsje = value; }
        }

        virtual public decimal LJSJCB
        {
            get { return ljsjcb; }
            set { ljsjcb = value; }
        }

        virtual public decimal QZLJSJCB
        {
            get { return qzljsjcb; }
            set { qzljsjcb = value; }
        }

        virtual public string QZXYL
        {
            get { return qzxyl; }
            set { qzxyl = value; }
        }

        virtual public decimal JZLJZXSGCZ
        {
            get { return jzljzxsgcz; }
            set { jzljzxsgcz = value; }
        }

        virtual public string QZZEDCZGXL
        {
            get { return qzzedczgxl; }
            set { qzzedczgxl = value; }
        }
        virtual public string QZXYCZGXL
        {
            get { return qzxyczgxl; }
            set { qzxyczgxl = value; }
        }
       

    }
}
