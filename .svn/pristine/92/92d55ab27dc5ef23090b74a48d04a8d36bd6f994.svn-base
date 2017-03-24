using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange
{
    public enum EnumWebSitePlan
    {
        /// <summary>
        /// 专业策划查询
        /// </summary>
        WebSitePlanSearch,
    }
    public class CWebSitePlan
    {
        private static IFramework framework = null;
        string mainViewName = "专业策划维护";
        private static VWebSitePlanSearchList searchList;

        public CWebSitePlan(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VWebSitePlanSearchList(this);
        }

        public void Start()
        {
            Find("空", "空");
        }

        public void Find(string name, string Id)
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

            VWebSitePlan vMainView = framework.GetMainView(mainViewName + "-空") as VWebSitePlan;

            if (vMainView == null)
            {
                vMainView = new VWebSitePlan();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VWebSitePlanSearchCon searchCon = new VWebSitePlanSearchCon(searchList);
                //VWasteMaterialHandleSearchList searchCon = new VWasteMaterialHandleSearchList(searchList);
                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            //vMainView.Start(Id);

            vMainView.ViewShow();
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(EnumWebSitePlan))
                {
                    EnumWebSitePlan execType = new EnumWebSitePlan();
                    switch (execType)
                    {
                        case EnumWebSitePlan.WebSitePlanSearch:
                            IMainView mroqMv = framework.GetMainView("专业策划查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VWebSitePlanQuery vcdq = new VWebSitePlanQuery();
                            vcdq.ViewCaption = "专业策划查询";
                            framework.AddMainView(vcdq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
