using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.StockMng.StockInMng.StockInUI;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveUI
{ 

    public class CStockMoveIn
    {
        private static IFramework framework = null;
        string mvname = "调拨入库单";

        private static VStockMoveSearchList searchListView;

        public CStockMoveIn(IFramework fm)
        {
            if (framework == null)
            {
                framework = fm;
            }
            searchListView = new VStockMoveSearchList(this);
        }

        public void Start(EnumStockExecType execType)
        {
            Find("空",execType);
        }

        public void Find(string name,EnumStockExecType execType)
        {            
            if (execType == EnumStockExecType.土建)
            {
                mvname = "调拨入库单(土建)";
            } else if(execType == EnumStockExecType.安装)
            {
                mvname = "调拨入库单(安装)";
            }
            string captionName = mvname;
            if (name is string)
            {
                captionName = this.mvname + "-" + name;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VStockMoveIn mainView = framework.GetMainView(mvname + "-空") as VStockMoveIn;
            if (mainView == null)
            {
                mainView = new VStockMoveIn();
                mainView.ExecType = execType;
                mainView.ViewName = mvname;

                //载入查询视图
                mainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vStockMove_ViewDeletedEvent);

                //分配辅助视图
                mainView.AssistViews.Add(searchListView);

                VStockMoveSearchCon theVStockMoveSthCon = new VStockMoveSearchCon(searchListView,execType);
                mainView.AssistViews.Add(theVStockMoveSthCon);
                //载入框架
                framework.AddMainView(mainView);
            }
            mainView.ViewCaption = captionName;
            mainView.ViewName = mvname;
            mainView.Start(name);
            mainView.ViewShow();
        }

        public void vStockMove_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VStockMoveIn vStockMove = mv as VStockMoveIn;
            if (vStockMove != null)
            {
                searchListView.RemoveRow(vStockMove.CurBillMaster.Id);
            }

            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }

        public object Execute(params object[] args)
        {
            if (args.Length == 0)
            {
                //Start();
            } else
            {
                object o = args[0];
                AuthManagerLib.AuthMng.MenusMng.Domain.Menus theMenu = null;
                if (args.Length > 1)
                {
                    theMenu = args[1] as AuthManagerLib.AuthMng.MenusMng.Domain.Menus;
                }
                if (o != null && o is EnumStockExecType)
                {
                    EnumStockExecType execType = (EnumStockExecType)o;
                    switch (execType)
                    { 
                        case EnumStockExecType.StockMoveInQuery:
                            IMainView mv = framework.GetMainView("调拨入库单查询");
                            if (mv != null)
                            {
                                mv.ViewShow();
                                return null;
                            }
                            VStockInQuery vsiq = new VStockInQuery("调拨入库单查询");
                            vsiq.TheAuthMenu = theMenu;
                            vsiq.StockInManner = EnumStockInOutManner.调拨入库;
                            vsiq.ViewCaption = "调拨入库单查询";
                            framework.AddMainView(vsiq);
                            return null;
                        case EnumStockExecType.土建:
                        case EnumStockExecType.安装:
                            Start(execType);
                            break;
                    }
                }
            }
            return null;
        }

    }
}
