using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialHireTransportCost;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.MaterialHireMng.MaterialTransportCost
{
   
     public enum CMaterialHireTransportCost_ExecType
    {
        运输费=0,
         运输费查询=1
    }

    public class CMaterialHireTransportCost
    {
        private static IFramework framework = null;
        string mainViewName = "运输单";
        private static VMaterialHireTransportCostSearchList searchList;

        public CMaterialHireTransportCost(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMaterialHireTransportCostSearchList(this);
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

            VMaterialHireTransportCost vMainView = framework.GetMainView(mainViewName + "-空") as VMaterialHireTransportCost;

            if (vMainView == null)
            {
                vMainView = new VMaterialHireTransportCost();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VMaterialHireTransportCostSearchCon searchCon = new VMaterialHireTransportCostSearchCon(searchList);

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
            object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CMaterialHireTransportCost_ExecType))
                {
                    CMaterialHireTransportCost_ExecType execType = (CMaterialHireTransportCost_ExecType)obj;
                    switch (execType)
                    {
                        case CMaterialHireTransportCost_ExecType.运输费:
                            {
                            Start();
                            break;
                            }
                        case CMaterialHireTransportCost_ExecType.运输费查询:
                            {
                                IMainView mroqMv = framework.GetMainView("运输费查询");
                                if (mroqMv != null)
                                {
                                    mroqMv.ViewShow();
                                    return null;
                                }
                                VMaterialHireTransportCostQuery vmcq = new VMaterialHireTransportCostQuery();
                                vmcq.ViewCaption = "运输费查询";
                                framework.AddMainView(vmcq);
                                break;
                            }
                    }
                }
                return null;
        }
    }
}

 
