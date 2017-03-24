using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionReport
{
    public enum CConstructionReportMng_ExecType
    {
        /// <summary>
        /// 日施工查询
        /// </summary>
        ConstructReportQuery
    }

    public class CConstructionReport
    {
        private static IFramework framework = null;
        string mainViewName = "日施工情况";
        private static VConstructionReportSearchList searchList;

        public CConstructionReport(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VConstructionReportSearchList(this);
        }
        
        public void Start()
        {
            Find("空","空");
        }
        
        public void Find(string name,string Id)
        {
            string captionName = mainViewName;
            if (name is string)
            {
                captionName = this.mainViewName + "-" + name;
            }
            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VConstructionReport vMainView = framework.GetMainView(mainViewName) as VConstructionReport;

            if (vMainView == null)
            {
                vMainView = new VConstructionReport();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VConstructionReportSearchCon searchCon = new VConstructionReportSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            } else
            {
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CConstructionReportMng_ExecType))
                {
                    CConstructionReportMng_ExecType execType = (CConstructionReportMng_ExecType)obj;
                    switch (execType)
                    {
                        case CConstructionReportMng_ExecType.ConstructReportQuery:
                            IMainView mroqMv = framework.GetMainView("日施工情况查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VConstructionReportQuery vmroq = new VConstructionReportQuery();
                            vmroq.ViewCaption = "日施工情况查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
