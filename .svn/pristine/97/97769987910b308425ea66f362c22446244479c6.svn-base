using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.WeekScheduleUI;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.WeekSchedules
{
    public enum EnumWeekPlanMngExecType
    {
        周进度计划,
    }
    public class CWeekPlanEntry
    {
       private static IFramework framework = null;
        string mainViewName = "周计划任务查询";
        private EnumExecScheduleType execScheduleType;

        public CWeekPlanEntry(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(EnumWeekPlanMngExecType))
                {
                    EnumWeekPlanMngExecType execType = (EnumWeekPlanMngExecType)obj;
                    switch (execType)
                    {
                        case EnumWeekPlanMngExecType.周进度计划:
                            IMainView mroqMv = framework.GetMainView("周进度计划");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VWeekPlanEntry vscm = new VWeekPlanEntry(execScheduleType);
                            vscm.ViewCaption = "周进度计划";
                            framework.AddMainView(vscm);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
