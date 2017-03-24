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

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveInRedUI
{ 

    public class CStockMoveInRed
    {
        private static IFramework framework = null;
        string mvname = "调拨入库单红单";

        private static VStockMoveInRedSearchList searchListView;

        public CStockMoveInRed(IFramework fm)
        {
            if (framework == null)
            {
                framework = fm;
            }
            searchListView = new VStockMoveInRedSearchList(this);
        }

        public void Start(EnumStockExecType execType)
        {
            Find("空", execType);
        }

        public void Find(string name, EnumStockExecType execType)
        {
            if (execType == EnumStockExecType.土建)
            {
                mvname = "调拨入库单红单(土建)";
            } else if (execType == EnumStockExecType.安装)
            {
                mvname = "调拨入库单红单(安装)";
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

            VStockMoveInRed mainView = framework.GetMainView(mvname + "-空") as VStockMoveInRed;
            if (mainView == null)
            {
                mainView = new VStockMoveInRed();
                mainView.ExecType = execType;
                mainView.ViewName = mvname;

                //载入查询视图
                mainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vStockMove_ViewDeletedEvent);

                //分配辅助视图
                mainView.AssistViews.Add(searchListView);

                VStockMoveInRedSearchCon theVStockMoveSthCon = new VStockMoveInRedSearchCon(searchListView,execType);
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
            VStockMoveInRed mainView = sender as VStockMoveInRed;
            if (mainView != null)
            {
                searchListView.RemoveRow(mainView.CurBillMaster.Id);
            }

            IList lst = framework.GetMainViews(mainView.ViewName);
            if (lst.Count > 1)
                framework.CloseMainView(mainView);
        }

        public object Execute(params object[] args)
        {
            if (args.Length == 0)
            {
                //Start();
            } else
            {
                object o = args[0];
                if (o != null && o is EnumStockExecType)
                {
                    EnumStockExecType execType = (EnumStockExecType)o;
                    switch (execType)
                    { 
                        case EnumStockExecType.安装:
                        case EnumStockExecType.土建:
                            Start(execType);
                            break;
                    }
                }
            }
            return null;
        }

    }
}
