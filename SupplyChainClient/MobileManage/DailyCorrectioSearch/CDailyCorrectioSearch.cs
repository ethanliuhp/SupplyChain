using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MobileManage.DailyCorrectioSearch
{
    public enum CDailyCorrectionSearch_ExecType
    {
        VDailyCorrectionSearch
    }
    public class CDailyCorrectioSearch
    {
        private static IFramework framework = null;
        string mainViewName = "整改单查询";

        public CDailyCorrectioSearch(IFramework fm)
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
                if (obj != null && obj.GetType() == typeof(CDailyCorrectionSearch_ExecType))
                {
                    CDailyCorrectionSearch_ExecType execType = (CDailyCorrectionSearch_ExecType)obj;
                    switch (execType)
                    {
                        case CDailyCorrectionSearch_ExecType.VDailyCorrectionSearch:
                            IMainView mroqMv = framework.GetMainView("整改单查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            IList list = new ArrayList();
                            int i = 0;
                            VDailyCorrectionSearch vgm = new VDailyCorrectionSearch("btnDaliyCorrectionSh");
                            vgm.ViewCaption = "整改单查询";
                            framework.AddMainView(vgm);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
