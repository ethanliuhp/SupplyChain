using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockBalMng.StockInBalUI
{
    public class CStockInBal
    {
        private static IFramework framework = null;
        string mainViewName = "验收结算单";
        private static VStockInBalSearchList searchListView;

        public CStockInBal(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchListView = new VStockInBalSearchList(this);
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

            VStockInBal mainView = framework.GetMainView(mainViewName + "-空") as VStockInBal;

            if (mainView == null)
            {
                mainView = new VStockInBal();
                mainView.ViewName = mainViewName;

                //载入查询视图
                mainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);

                //分配辅助视图
                mainView.AssistViews.Add(searchListView);
                VStockInBalSearchCon theVStockOutRedSthCon = new VStockInBalSearchCon(searchListView);
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
            VStockInBal mainView = sender as VStockInBal;
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
                Start();
            }
            else
            {
                object o = args[0];
                AuthManagerLib.AuthMng.MenusMng.Domain.Menus theMenu = null;
                if (args.Length > 1)
                {
                    theMenu = args[1] as AuthManagerLib.AuthMng.MenusMng.Domain.Menus;
                }
                if (o!=null&&o.GetType() == typeof(EnumStockExecType))
                {
                    EnumStockExecType excuteType = (EnumStockExecType)o;

                    switch (excuteType)
                    {
                        case EnumStockExecType.StockInBalQuery:
                            IMainView sibq = framework.GetMainView("验收结算单查询");
                            if (sibq != null)
                            {
                                sibq.ViewShow();
                                return null;
                            }
                            VStockInBalQuery vsibq = new VStockInBalQuery();
                            vsibq.TheAuthMenu = theMenu;
                            vsibq.ViewCaption = "验收结算单查询";
                            framework.AddMainView(vsibq);
                            return null;
                    }
                }
                else if (o.GetType() == typeof(string))
                {
                    Find(o.ToString());
                }

            }

            return null;
        }
    }
}
