using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
//using Application.Business.Erp.SupplyChain.SaleManage.SaleBudget.Service;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic;
using Application.Business.Erp.SupplyChain.Client.StockMng.StockInMng.StockInUI;
using Application.Business.Erp.SupplyChain.StockManage.StockInManage.BasicDomain;
using Application.Business.Erp.SupplyChain.Client.StockMng;
using Application.Business.Erp.SupplyChain.Client.StockMng.StockQuery;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI
{
    public class CStockIn
    {
        private static IFramework framework = null;
        string mainViewName = "收料入库单";
        private static VStockInSearchList searchList;


        public CStockIn(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VStockInSearchList(this);
        }

        public void Start(EnumStockExecType execType)
        {
            Find("空", execType);
        }

        public void Find(string name,EnumStockExecType execType)
        {
            if (execType == EnumStockExecType.土建)
            {
                mainViewName = "收料入库单(土建)";
            } else if (execType == EnumStockExecType.安装)
            {
                mainViewName = "收料入库单(安装)";
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

            VStockIn mainView = framework.GetMainView(mainViewName + "-空") as VStockIn;

            if (mainView == null)
            {
                mainView = new VStockIn();
                mainView.ExcuteType = execType;
                mainView.ViewName = mainViewName;

                //载入查询视图
                mainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vSaleBudget_ViewDeletedEvent);

                //分配辅助视图
                mainView.AssistViews.Add(searchList);
                VStockInSearchCon theVStockInSthCon = new VStockInSearchCon(searchList,execType);
                mainView.AssistViews.Add(theVStockInSthCon);

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

            VStockIn vStockIn = mv as VStockIn;
            if (vStockIn != null)
                searchList.RemoveRow(vStockIn.CurBillMaster.Id);


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
                if (o != null && typeof(EnumStockExecType) == o.GetType())
                {
                    EnumStockExecType excuteType = (EnumStockExecType)o;
                    //MMaterial mmi = null;

                    switch (excuteType)
                    {
                        case EnumStockExecType.土建:
                        case EnumStockExecType.安装:
                            Start(excuteType);
                            break;
                        case EnumStockExecType.单据修改:
                            IMainView siqMv = framework.GetMainView("单据修改");
                            if (siqMv != null)
                            {
                                siqMv.ViewShow();
                                return null;
                            }
                            VUpdateStock vsriq = new VUpdateStock();
                            vsriq.ViewCaption = "单据修改";
                            framework.AddMainView(vsriq);
                            return null;
                        case EnumStockExecType.stateSearch:
                            IMainView mv = framework.GetMainView("收料入库单查询");
                            if (mv != null)
                            {
                                //如果当前视图已经存在，直接显示
                                mv.ViewShow();
                                return null;
                            }
                            VStockInQuery theVStockInStateSearch = new VStockInQuery("收料入库单查询");
                            theVStockInStateSearch.StockInManner = EnumStockInOutManner.收料入库;
                            theVStockInStateSearch.TheAuthMenu = theMenu;
                            theVStockInStateSearch.ViewCaption = "收料入库单查询";
                            framework.AddMainView(theVStockInStateSearch);
                            return null;
                        case EnumStockExecType.basicDataOptr:
                            IMainView bdoMv = framework.GetMainView("基础数据管理");
                            if (bdoMv != null)
                            {
                                bdoMv.ViewShow();
                                return null;
                            }
                            VBasicDataOptr vbdo = new VBasicDataOptr();
                            vbdo.ViewCaption = "基础数据管理";
                            framework.AddMainView(vbdo);
                            return null;
                        case EnumStockExecType.logDataQuery:
                            IMainView ldqMv = framework.GetMainView("日志查询");
                            if (ldqMv != null)
                            {
                                ldqMv.ViewShow();
                                return null;
                            }
                            VLogQuery vlq = new VLogQuery();
                            vlq.ViewCaption = "日志查询";
                            framework.AddMainView(vlq);
                            return null;
                        case EnumStockExecType.logStatReport:
                            IMainView rztjMv = framework.GetMainView("日志统计");
                            if (rztjMv != null)
                            {
                                rztjMv.ViewShow();
                                return null;
                            }
                            VLogStatReport vLogStat = new VLogStatReport();
                            vLogStat.ViewCaption = "日志统计";
                            framework.AddMainView(vLogStat);
                            return null;
                        case EnumStockExecType.StockRelationQuery:
                            IMainView srqMv = framework.GetMainView("库存查询");
                            if (srqMv != null)
                            {
                                srqMv.ViewShow();
                                return null;
                            }
                            VStockRelationQuery vsrq = new VStockRelationQuery();
                            vsrq.ViewCaption = "库存查询";
                            framework.AddMainView(vsrq);
                            return null;
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
