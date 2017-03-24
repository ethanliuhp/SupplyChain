using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
//using Application.Business.Erp.SupplyChain.SaleManage.SaleBudget.Service;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockMoveManage.StockMoveOutRedUI
{

    public class CStockMoveOutRed
    {
        private static IFramework framework = null;
        string mainViewName = "调拨出库单红单";

        private static VStockMoveOutRedSearchList searchListView;


        public CStockMoveOutRed(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchListView = new VStockMoveOutRedSearchList(this);
        }

        public void Start(EnumStockExecType execType)
        {
            Find("空",execType);
        }

        public void Find(string name,EnumStockExecType execType)
        {
            if (execType == EnumStockExecType.安装)
            {
                mainViewName = "调拨出库单红单(安装)";
            } else if (execType == EnumStockExecType.土建)
            {
                mainViewName = "调拨出库单红单(土建)";
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

            VStockMoveOutRed mainView = framework.GetMainView(mainViewName + "-空") as VStockMoveOutRed;
            if (mainView == null)
            {
                mainView = new VStockMoveOutRed();
                mainView.ExecType = execType;
                mainView.ViewName = mainViewName;

                //载入查询视图
                mainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);

                //分配辅助视图
                mainView.AssistViews.Add(searchListView);
                VStockMoveOutRedSearchCon theVStockOutSthCon = new VStockMoveOutRedSearchCon(searchListView,execType);
                mainView.AssistViews.Add(theVStockOutSthCon);
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
            IMainView mv = sender as IMainView;

            VStockMoveOutRed billMaster = mv as VStockMoveOutRed;
            if (billMaster != null)
                searchListView.RemoveRow(billMaster.CurBillMaster.Id);

            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
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
                if (o != null && o.GetType() == typeof(EnumStockExecType))
                {
                    EnumStockExecType execType = (EnumStockExecType)o;
                    switch (execType)
                    { 
                        case EnumStockExecType.土建:
                        case EnumStockExecType.安装:
                            Start(execType);
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
