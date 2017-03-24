using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 公司结算进展月报
    /// </summary>
    [Serializable]
    [Entity]
    public class SettlementProgressReportDtl : BaseDetail
    {
        private string thisMonth;       //本月工作
        private string settlementProgress;      //总体结算进度
        private string importantFactor;     //重要制约因素
        private string nextMonthPlan;   //下月工作计划

        virtual public string ThisMonth
        {
            get { return thisMonth; }
            set { thisMonth = value; }
        }

        virtual public string SettlementProgress
        {
            get { return settlementProgress; }
            set { settlementProgress = value; }
        }

        virtual public string ImportantFactor
        {
            get { return importantFactor; }
            set { importantFactor = value; }
        }

        virtual public string NextMonthPlan
        {
            get { return nextMonthPlan; }
            set { nextMonthPlan = value; }
        }
    }
}
