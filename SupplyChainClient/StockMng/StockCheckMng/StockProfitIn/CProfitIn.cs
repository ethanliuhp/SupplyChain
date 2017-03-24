using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn
{
    public enum ProfitInExcType
    {
        forwardSearch,
        stateSearch,
        multiSelect,
        ProfitInQuery//xl
    }

    public class CProfitIn
    {
        private static IFramework framework = null;
        string mvname = "盘盈单";

        private static VProfitInList searchListView;

        public CProfitIn(IFramework fm)
        {
            if (framework == null)
            {
                framework = fm;
            }
            searchListView = new VProfitInList(this);
        }

        public object Execute(params object[] args)
        {
            if (args.Length == 0)
            {
                //Start();
            }
            else
            {
                object o = args[0];
                if (o is EnumStockExecType)
                {
                    EnumStockExecType excuteType = (EnumStockExecType)o;
                    switch (excuteType)
                    {
                        case EnumStockExecType.安装:
                        case EnumStockExecType.土建:
                            Start(excuteType);
                            break;


                    }
                }
                else if (o is ProfitInExcType)
                {
                    ProfitInExcType excuteType = (ProfitInExcType)o;
                    switch (excuteType)
                    {
                        case ProfitInExcType .ProfitInQuery :
                            //IMainView mroqMv = framework.GetMainView("月度盘点查询");
                            //if (mroqMv != null)
                            //{
                            //    mroqMv.ViewShow();
                            //    return null;
                            //}
                            //VStockInventoryQuery vmroq = new VStockInventoryQuery();
                            //vmroq.ViewCaption = "月度盘点查询";
                            //framework.AddMainView(vmroq);
                            string sName = "盘盈单查询";
                            IMainView mv = framework.GetMainView(sName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VProfitInQuery query = new VProfitInQuery();
                                query.ViewCaption = sName;
                                framework.AddMainView(query);
                            }
                            break;
                    }
                }
            }

            return null;
        }

        public void Start(EnumStockExecType execType)
        {
            Find("空", execType);
        }

        public void Find(string name, EnumStockExecType execType)
        {
            if (execType == EnumStockExecType.土建)
            {
                mvname = "盘盈单(土建)";
            } else if (execType == EnumStockExecType.安装)
            {
                mvname = "盘盈单(安装)";
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

            VProfitIn vProfitIn = framework.GetMainView(mvname + "-空") as VProfitIn;

            if (vProfitIn == null)
            {
                vProfitIn = new VProfitIn(execType);
               // vProfitIn.ExecType = execType;
                vProfitIn.ViewName = mvname;

                //载入查询视图
                vProfitIn.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vProfitIn_ViewDeletedEvent);

                //分配辅助视图
                vProfitIn.AssistViews.Add(searchListView);

                VProfitInSthCon theVProfitInSthCon = new VProfitInSthCon(execType);
                theVProfitInSthCon.theVProfitInList = searchListView;
                vProfitIn.AssistViews.Add(theVProfitInSthCon);
                //载入框架
                framework.AddMainView(vProfitIn);
            }

            vProfitIn.ViewCaption = captionName;
            vProfitIn.ViewName = mvname;
            vProfitIn.Start(name);

            vProfitIn.ViewShow();            
        }

        public void vProfitIn_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VProfitIn vProfitIn = mv as VProfitIn;
            if (vProfitIn != null)
            {
                searchListView.RemoveRow(vProfitIn.theProfitIn.Id);
            }

            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }
    }
}
