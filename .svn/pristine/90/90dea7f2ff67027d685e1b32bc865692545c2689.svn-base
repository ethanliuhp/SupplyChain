using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public enum CGatheringMng_ExecType
    {
        Gathering,//收款单
        GatheringOther,//收款单(非工程款)
        GatheringQuery,//收款单查询
        收款台账=3,
        应收拖欠款台账 = 4,
        收付款保证金及押金台账 = 8

    }

    public class CGatheringMng
    {
        private static IFramework framework = null;
        string mainViewName = "收款单";
        private static VGatheringSearchList searchList;

        public CGatheringMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VGatheringSearchList(this);
        }

        public void Start(int gatheringType)
        {
            Find("空", "空", gatheringType);
        }
        //gatheringType,0:收款单(工程款),1:收款单(非工程款)
        public void Find(string name, string Id, int gatheringType)
        {
            if (gatheringType == 0)
            {
                mainViewName = "收款单(工程款)";
            }
            else
            {
                mainViewName = "收款单(非工程款)";
            }
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

            VGatheringMng vMainView = framework.GetMainView(mainViewName + "-空") as VGatheringMng;

            if (vMainView == null)
            {
                vMainView = new VGatheringMng(gatheringType);
                vMainView.ViewName = mainViewName;
                vMainView.GatheringType = gatheringType;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VGatheringSearchCon searchCon = new VGatheringSearchCon(searchList, gatheringType);

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
                Start(0);
            }
            else
            {
                object obj = args[0];
                CGatheringMng_ExecType executeType = (CGatheringMng_ExecType)obj;
                switch (executeType)
                {
                    case CGatheringMng_ExecType.Gathering:
                        Start(0);
                        break;
                    case CGatheringMng_ExecType.GatheringOther:
                        Start(1);
                        break;
                    case CGatheringMng_ExecType.GatheringQuery:
                        {
                            string viewName = "收款单查询";
                            IMainView mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VGatheringQuery vmc = new VGatheringQuery();
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    case CGatheringMng_ExecType.收款台账:
                        {
                            string viewName = "收款台账";
                            IMainView mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VGatheringMngReport vmc = new VGatheringMngReport(executeType);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    case CGatheringMng_ExecType.应收拖欠款台账:
                        {
                            string viewName = "应收拖欠款台账";
                            IMainView mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VMustNotGatheringReport vmc = new VMustNotGatheringReport(executeType);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                    case CGatheringMng_ExecType.收付款保证金及押金台账:
                        {
                            string viewName = "保证金/押金台账";
                            IMainView mv = framework.GetMainView(viewName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VGatherAndPayDepositReport vmc = new VGatherAndPayDepositReport(executeType);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            break;
                        }
                }

            }
            return null;
        }
    }
}
