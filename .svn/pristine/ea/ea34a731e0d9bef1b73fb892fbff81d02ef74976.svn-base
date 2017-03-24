using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProjectPlanningMange.ConstructionDesignManage
{
    public enum EnumContructionDesign
    {
        /// <summary>
        /// 施工组织设计查询
        /// </summary>
        ContructionDesignSearch,
    }
    public class CContructionDesign
    {
        private static IFramework framework = null;
        string mainViewName = "施工组织设计维护";
        private static VConstructionDesignSearchList searchList;

        public CContructionDesign(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VConstructionDesignSearchList(this);
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

            VConstructionDesign vMainView = framework.GetMainView(mainViewName + "-空") as VConstructionDesign;

            if (vMainView == null)
            {
                vMainView = new VConstructionDesign();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VConstructionDesignSearchCon searchCon = new VConstructionDesignSearchCon(searchList);
                //VWasteMaterialHandleSearchList searchCon = new VWasteMaterialHandleSearchList(searchList);

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
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(EnumContructionDesign))
                {
                    EnumContructionDesign execType = new EnumContructionDesign();
                    switch (execType)
                    {
                        case EnumContructionDesign.ContructionDesignSearch:
                            IMainView mroqMv = framework.GetMainView("施工组织设计查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VConstructionDesignQuery vcdq = new VConstructionDesignQuery();
                            vcdq.ViewCaption = "施工组织设计查询";
                            framework.AddMainView(vcdq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
