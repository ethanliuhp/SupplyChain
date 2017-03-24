using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
//using Application.Business.Erp.SupplyChain.SaleManage.SaleBudget.Service;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.StockMng.StockQuery;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInRedUI
{
    public class CStockInRed
    {
        private static IFramework framework = null;
        string mainViewName = "收料入库红单";
        private static VStockInRedSearchList searchListView;

        public CStockInRed(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchListView = new VStockInRedSearchList(this);
        }

        public void Start(EnumStockExecType execType)
        {
            Find("空", execType);
        }

        public void Find(string name,EnumStockExecType execType)
        {
            if (execType == EnumStockExecType.土建)
            {
                mainViewName = "收料入库红单(土建)";
            } else if (execType == EnumStockExecType.安装)
            {
                mainViewName = "收料入库红单(安装)";
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

            VStockInRed mainView = framework.GetMainView(mainViewName + "-空") as VStockInRed;

            if (mainView == null)
            {
                mainView = new VStockInRed();
                mainView.ExecType = execType;
                mainView.ViewName = mainViewName;

                //载入查询视图
                mainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);

                //分配辅助视图
                mainView.AssistViews.Add(searchListView);
                VStockInRedSearchCon theVStockOutRedSthCon = new VStockInRedSearchCon(searchListView,execType);
                mainView.AssistViews.Add(theVStockOutRedSthCon);
                //载入框架
                framework.AddMainView(mainView);
            }
            mainView.ViewCaption = captionName;
            mainView.ViewName = mainViewName;
            mainView.Start(name);
            mainView.ViewShow();
        }

        void vSaleBudget_ViewDeletedEvent(object sender)
        {
            VStockInRed mainView = sender as VStockInRed;
            if (mainView != null)
                searchListView.RemoveRow(mainView.CurBillMaster.Id);

            IList lst = framework.GetMainViews(mainView.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mainView);
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                //Start();
            }
            else
            {
                object o = args[0];              
                if (o!=null&&o.GetType() == typeof(EnumStockExecType))
                {
                    EnumStockExecType excuteType = (EnumStockExecType)o;

                    switch (excuteType)
                    {
                        case EnumStockExecType.仓库收发台账:
                            IMainView mv1 = framework.GetMainView("仓库收发台账");
                            if (mv1 != null)
                            {
                                mv1.ViewShow();
                                return null;
                            }
                            VStockSequenceQuery vssq = new VStockSequenceQuery();
                            vssq.ViewCaption = "仓库收发台账";
                            framework.AddMainView(vssq);
                            return null;
                        case EnumStockExecType.安装:
                        case EnumStockExecType.土建:
                            Start(excuteType);
                            break;
                    }
                }
                else if (o.GetType() == typeof(string))
                {
                    //Find(o.ToString());
                }

            }

            return null;
        }
    }
}
