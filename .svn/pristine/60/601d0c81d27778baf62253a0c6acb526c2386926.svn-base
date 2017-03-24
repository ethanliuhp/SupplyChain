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
using Application.Business.Erp.SupplyChain.Client.StockMng.StockQuery;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockOutManage.StockOutUI
{

    public class CStockOut
    {
        private static IFramework framework = null;
        string mainViewName = "领料出库单";

        private static VStockOutSearchList searchListView;


        public CStockOut(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchListView = new VStockOutSearchList(this);
        }

        public void Start(EnumStockExecType execType)
        {
            Find("空", execType);
        }

        public void Find(string name,EnumStockExecType execType)
        {
            if (execType == EnumStockExecType.土建)
            {
                mainViewName = "领料出库单(土建)";
            } else if(execType == EnumStockExecType.安装)
            { 
                mainViewName = "领料出库单(安装)";
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

            VStockOut mainView = framework.GetMainView(mainViewName + "-空") as VStockOut;
            if (mainView == null)
            {
                mainView = new VStockOut();
                mainView.ExecType = execType;
                mainView.ViewName = mainViewName;

                //载入查询视图
                mainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);

                //分配辅助视图
                mainView.AssistViews.Add(searchListView);
                VStockOutSearchCon theVStockOutSthCon = new VStockOutSearchCon(searchListView,execType);
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

            VStockOut vStockOut = mv as VStockOut;
            if (vStockOut != null)
                searchListView.RemoveRow(vStockOut.CurBillMaster.Id);

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
                AuthManagerLib.AuthMng.MenusMng.Domain.Menus theMenu = null;
                if (args.Length > 1)
                {
                    theMenu = args[1] as AuthManagerLib.AuthMng.MenusMng.Domain.Menus;
                }
                if (o != null && o.GetType() == typeof(EnumStockExecType))
                {
                    EnumStockExecType excuteType = (EnumStockExecType)o;
                    switch (excuteType)
                    {
                        case EnumStockExecType.StockOutQuery:
                            IMainView vsoqMV = framework.GetMainView("领料出库查询");
                            if (vsoqMV != null)
                            {
                                vsoqMV.ViewShow();
                                return null;
                            }
                            VStockOutQuery vsoq = new VStockOutQuery("领料出库查询");
                            vsoq.TheAuthMenu = theMenu;
                            vsoq.StockInOutManner = EnumStockInOutManner.领料出库;
                            vsoq.ViewCaption = "领料出库查询";
                            framework.AddMainView(vsoq);
                            vsoq.ViewShow();
                            return null;
                        case EnumStockExecType.辅材数据比例统计:
                            IMainView vsSporadic = framework.GetMainView("辅材数据比例统计");
                            if (vsSporadic != null)
                            {
                                vsSporadic.ViewShow();
                                return null;
                            }
                            VStockSporadicQuery vssq = new VStockSporadicQuery();
                            vssq.ViewCaption = "辅材数据比例统计";
                            framework.AddMainView(vssq);
                            vssq.ViewShow();
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
