using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.GWBSConfirmUI
{
    public class CGWBSConfirm
    {
        private static IFramework framework = null;
        public string mainViewName = "工程量确认单维护";
        private static VGWBSConfirmSearchList searchList;

        public CGWBSConfirm(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VGWBSConfirmSearchList(this);
        }

        public void Start()
        {
            Find("空", null);
        }

        public void Find(string code, string id)
        {
            string captionName = mainViewName;
            if (code is string)
            {
                captionName = this.mainViewName + "-" + code;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VGWBSConfirm vMainView = framework.GetMainView(mainViewName + "-空") as VGWBSConfirm;

            if (vMainView == null)
            {
                vMainView = new VGWBSConfirm();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VGWBSConfirmSearchCon searchCon = new VGWBSConfirmSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(id);

            vMainView.ViewShow();
        }
        public void Find2(string code, string id)
        {
            string captionName = mainViewName;
            if (code is string)
            {
                captionName = this.mainViewName + "-" + code;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VGWBSConfirm2 vMainView = framework.GetMainView(mainViewName + "-空") as VGWBSConfirm2;

            if (vMainView == null)
            {
                vMainView = new VGWBSConfirm2();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VGWBSConfirmSearchCon searchCon = new VGWBSConfirmSearchCon(searchList);

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
                if (obj != null)
                {
                    EnumProductionMngExecType execType = (EnumProductionMngExecType)obj;
                    switch (execType)
                    {
                        case EnumProductionMngExecType.工程任务确认单查询:
                            IMainView ws = framework.GetMainView("工程量确认单查询");
                            if (ws != null)
                            {
                                ws.ViewShow();
                                return null;
                            }
                            VGWBSConfirmQueryByBill vwsq = new VGWBSConfirmQueryByBill();
                            vwsq.ViewCaption = "工程量确认单查询";
                            framework.AddMainView(vwsq);
                            return null;
                        case EnumProductionMngExecType.工程量确认单维护_非计划:
                            mainViewName = "工程量确认单维护_非计划";
                            Find2("空", null);
                            break;
                        //case EnumProductionMngExecType.工程任务确认单查询_主从:
                        //    IMainView ws1 = framework.GetMainView("工程量确认单查询(主从)");
                        //    if (ws1 != null)
                        //    {
                        //        ws1.ViewShow();
                        //        return null;
                        //    }
                        //    VGWBSConfirmQueryByBill vwsqb = new VGWBSConfirmQueryByBill();
                        //    vwsqb.ViewCaption = "工程量确认单查询(主从)";
                        //    framework.AddMainView(vwsqb);
                        //    return null;
                    }
                }
            }
            return null;
        }
    }
}
