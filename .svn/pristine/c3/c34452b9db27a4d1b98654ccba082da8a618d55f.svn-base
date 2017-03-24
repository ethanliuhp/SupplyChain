using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.ProjectTaskQuery
{
    public enum EnumProjectTask
    {
        WeekScheduleSearch
    }

    public class CProjectTaskQuery
    {
        private static IFramework framework = null;
        string mainViewName = "周计划任务查询";
        IList list = new ArrayList();
        string btnName;

        public CProjectTaskQuery(IFramework fm)
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
                if (obj != null && obj.GetType() == typeof(EnumProjectTask))
                {
                    EnumProjectTask execType = (EnumProjectTask)obj;
                    switch (execType)
                    {
                        case EnumProjectTask.WeekScheduleSearch:
                            IMainView mroqMv = framework.GetMainView("周计划任务查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VWeekScheduleSearch vscm = new VWeekScheduleSearch(btnName);
                            vscm.ViewCaption = "周计划任务查询";
                            framework.AddMainView(vscm);
                            return null;
                    }
                }
            }
            return null;
        }

    }
}
