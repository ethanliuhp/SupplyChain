using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using System.ComponentModel;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng;
using Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan;

namespace Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan
{
    public enum CPeriodRequirePlanMng_ExecType
    {
        /// <summary>
        /// 期间需求计划单维护
        /// </summary>
        期间需求计划单维护 = 1
    }
    public enum OperationCode
    {
        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        新增 = 1,
        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        修改 = 2,
        /// <summary>
        /// 显示
        /// </summary>
        [Description("显示")]
        显示 = 3
    }

    public class CPeriodRequirePlanMng
    {
        private static IFramework framework = null;
        string mainViewName = "期间需求计划单维护";

        public CPeriodRequirePlanMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        public object Excute(params object[] args)
        {
            if (args.Length > 0)
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CPeriodRequirePlanMng_ExecType))
                {
                    CPeriodRequirePlanMng_ExecType execType = (CPeriodRequirePlanMng_ExecType)obj;
                    string viewName = "";
                    IMainView mv = null;
                    switch (execType)
                    {
                        case CPeriodRequirePlanMng_ExecType.期间需求计划单维护:
                            viewName = "期间需求计划单维护";
                            mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                int OprCode = (int)OperationCode.新增;
                                ResourceRequirePlan ResourcePlan = new ResourceRequirePlan();
                                ResourceRequireReceipt ResReceipt = new ResourceRequireReceipt();
                                VPeriodRequirePlan vmroq = new VPeriodRequirePlan(OprCode, ResourcePlan, ResReceipt);
                                vmroq.ViewCaption = viewName;
                                framework.AddMainView(vmroq);
                            }
                            break;
                    }
                }
            }
            return null;
        }
    }
}
