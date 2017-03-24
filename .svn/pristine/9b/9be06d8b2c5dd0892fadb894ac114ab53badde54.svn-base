using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.PumpingPoundsMng
{
    public enum CPumpingPounds_ExceType
    {
        /// <summary>
        /// 抽磅单统计查询
        /// </summary>
        PumpingPoundsQuery
    }
    public class CPumpingPounds
    {
        private static IFramework framework = null;
        string mainViewName = "抽磅单";
        private static VPumpingPoundSearchList searchList;

        public CPumpingPounds(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VPumpingPoundSearchList(this);
        }

        public void Start()
        {
            Find("空", "空");
        }
        public void Find(string name, string id)
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
            VPumpingPound vMainView = framework.GetMainView(mainViewName + "-空") as VPumpingPound;

            if (vMainView == null)
            {
                vMainView = new VPumpingPound();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VPumpingPoundSearchCon searchCon = new VPumpingPoundSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(id);

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
                if (obj != null && obj.GetType() == typeof(CPumpingPounds_ExceType))
                {
                    CPumpingPounds_ExceType execType = (CPumpingPounds_ExceType)obj;
                    switch (execType)
                    {
                        case CPumpingPounds_ExceType.PumpingPoundsQuery:
                            IMainView mroqMv = framework.GetMainView("抽磅单统计查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VPumpingPoundQuery vppq = new VPumpingPoundQuery();
                            vppq.ViewCaption = "抽磅单统计查询";
                            framework.AddMainView(vppq);
                            return null;
                    }
                }
            }
            return null;
        }

    }
}
