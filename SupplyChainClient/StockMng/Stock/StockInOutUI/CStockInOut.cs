using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.StockMng.Report;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.Stock.StockInOutUI
{
    public class CStockInOut
    {
        private static IFramework framework = null;
        string mainViewName = "物资实际耗用结算";

        public CStockInOut(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        public void Start()
        {
            Find("空");
        }

        public void Find(string name)
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

            VStockInOut vSaleBudget = framework.GetMainView(mainViewName + "-空") as VStockInOut;

            if (vSaleBudget == null)
            {
                vSaleBudget = new VStockInOut();
                vSaleBudget.ViewName = mainViewName;

                //载入查询视图
                vSaleBudget.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);

                //分配辅助视图
                //vSaleBudget.AssistViews.Add(searchListView);
                //vSaleBudget.theVStockInList = searchListView;
                //载入框架
                framework.AddMainView(vSaleBudget);
            }

            vSaleBudget.ViewCaption = captionName;
            vSaleBudget.ViewName = mainViewName;
            vSaleBudget.Start(name);

            vSaleBudget.ViewShow();
        }

        void vSaleBudget_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object o = args[0];
                if (o != null && typeof(EnumStockExecType) == o.GetType())
                {
                    EnumStockExecType execType = (EnumStockExecType)o;
                    switch (execType)
                    {
                        case EnumStockExecType.仓库收发存月报:
                            IMainView mv = framework.GetMainView("仓库收发存月报");
                            if (mv != null)
                            {
                                mv.ViewShow();
                                return null;
                            }
                            VStockInOutQuery vsioq = new VStockInOutQuery();
                            vsioq.ViewCaption = "仓库收发存月报";
                            framework.AddMainView(vsioq);
                            return null;
                        case EnumStockExecType.仓库收发存报表:
                            IMainView mv2 = framework.GetMainView("仓库收发存报表");
                            if (mv2 != null)
                            {
                                mv2.ViewShow();
                                return null;
                            }
                            WZReport report = new WZReport();
                            report.ViewCaption = "仓库收发存报表";
                            framework.AddMainView(report);
                            return null;
                        case EnumStockExecType.料具租赁:
                            IMainView mv3 = framework.GetMainView("料具租赁月报表");
                            if (mv3 != null)
                            {
                                mv3.ViewShow();
                                return null;
                            }
                            LJReport ljreport = new LJReport();
                            ljreport.ViewCaption = "料具租赁月报表";
                            framework.AddMainView(ljreport);
                            return null;
                        case EnumStockExecType.成本对比分析表:
                            IMainView mv4 = framework.GetMainView("成本对比分析表");
                            if (mv4 != null)
                            {
                                mv4.ViewShow();
                                return null;
                            }
                            VWZTJReport ljreport1 = new VWZTJReport();
                            ljreport1.ViewCaption = "成本对比分析表";
                            framework.AddMainView(ljreport1);
                            return null;
                    }
                }
                //
            }
            return null;
        }
    }
}
