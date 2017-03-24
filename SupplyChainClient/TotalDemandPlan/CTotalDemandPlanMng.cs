using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.TotalDemandPlan;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using System.ComponentModel;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Client.SupplyMng.DemandMasterPlanMng;

namespace Application.Business.Erp.SupplyChain.Client.TotalDemandPlan
{
    public enum CDemandPlanMng_ExecType
    {
        /// <summary>
        /// 总量需求计划
        /// </summary>
        TotalDemandPlanQuery,
        TotalDemandAnalysis
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

    public class CTotalDemandPlanMng
    {
        private static IFramework framework = null;
        string mainViewName = "总量需求计划单";

        public CTotalDemandPlanMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        public object Excute(params object[] args)
        {
            if (args.Length > 0)
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CDemandPlanMng_ExecType))
                {
                    CDemandPlanMng_ExecType execType = (CDemandPlanMng_ExecType)obj;
                    string viewName = "";
                    IMainView mv = null;
                    switch (execType)
                    {
                        case CDemandPlanMng_ExecType.TotalDemandPlanQuery:
                            viewName = "总量需求计划单";
                            mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                //OperationCode OprCode = EnumUtil<OperationCode>.FromDescription(OperationCode.新增);
                                int OprCode = (int)OperationCode.新增;
                                ResourceRequirePlan ResourcePlan = new ResourceRequirePlan();
                                ResourceRequireReceipt ResReceipt = new ResourceRequireReceipt();
                                VTotalDemandPlanQuery vmroq = new VTotalDemandPlanQuery(OprCode, ResourcePlan, ResReceipt);
                                vmroq.ViewCaption = viewName;
                                framework.AddMainView(vmroq);
                            }
                            break;
                        case CDemandPlanMng_ExecType.TotalDemandAnalysis:
                            viewName = "需求总量分析单";
                            mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                ResourceRequirePlan ResourcePlan = new ResourceRequirePlan();
                                VTotalDemandAnalysis vmroq = new VTotalDemandAnalysis(ResourcePlan);
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
