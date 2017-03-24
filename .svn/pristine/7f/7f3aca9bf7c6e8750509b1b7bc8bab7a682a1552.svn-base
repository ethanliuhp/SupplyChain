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

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockLossOut
{
    public enum LossOutExcType
    {
        forwardSearch,
        stateSearch,
        multiSelect,
        LossOutQuery//xl
    }

    public class CLossOut
    {
        private static IFramework framework = null;
        string mvname = "盘亏单";

        private static VLossOutList searchListView;

        public CLossOut(IFramework fm)
        {
            if (framework == null)
            {
                framework = fm;
            }
            searchListView = new VLossOutList(this);
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
                        default:
                            break;
                    }
                }
                else if (o is LossOutExcType)
                {
                    LossOutExcType lossType = (LossOutExcType)o;
                    switch (lossType)
                    {
                        case LossOutExcType.LossOutQuery:
                            //IMainView mroqMv = framework.GetMainView("月度盘点查询");
                            //if (mroqMv != null)
                            //{
                            //    mroqMv.ViewShow();
                            //    return null;
                            //}
                            //VStockInventoryQuery vmroq = new VStockInventoryQuery();
                            //vmroq.ViewCaption = "月度盘点查询";
                            //framework.AddMainView(vmroq);
                            string sName = "盘亏单查询";
                            IMainView mv = framework.GetMainView(sName);
                            if (mv != null)
                            {
                                mv.ViewShow();
                            }
                            else
                            {
                                VLossOutQuery query = new VLossOutQuery();
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
                mvname = "盘亏单(土建)";
            } else if (execType == EnumStockExecType.安装)
            {
                mvname = "盘亏单(安装)";
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

            VLossOut vLossOut = framework.GetMainView(mvname + "-空") as VLossOut;

            if (vLossOut == null)
            {
                vLossOut = new VLossOut(execType);
                vLossOut.ExecType = execType;
                vLossOut.ViewName = mvname;

                //载入查询视图
                vLossOut.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vLossOut_ViewDeletedEvent);

                //分配辅助视图
                vLossOut.AssistViews.Add(searchListView);

                VLossOutSthCon theVLossOutSthCon = new VLossOutSthCon(execType);
                theVLossOutSthCon.theVLossOutList = searchListView;
                vLossOut.AssistViews.Add(theVLossOutSthCon);

                vLossOut.theVLossOutList = searchListView;
                //载入框架
                framework.AddMainView(vLossOut);
            }

            vLossOut.ViewCaption = captionName;
            vLossOut.ViewName = mvname;
            vLossOut.Start(name);

            vLossOut.ViewShow();            
        }

        public void vLossOut_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VLossOut vLossOut = mv as VLossOut;
            if (vLossOut != null)
            {
                searchListView.RemoveRow(vLossOut.theLossOut.Id);
            }

            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }
    }
}
