using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.StockMng.Report;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng
{
    public enum CStockInventoryMng_ExecType
    {
        /// <summary>
        /// 月度盘点单查询
        /// </summary>
        StockInventoryQuery,
        安装,
        土建,
        仓库报表
    }
    
    public class CStockInventoryMng
    {
        private static IFramework framework = null;
        string mainViewName = "月度盘点单";
        private static VStockInventorySearchList searchList;

        public CStockInventoryMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VStockInventorySearchList(this);
        }

        public void Start(CStockInventoryMng_ExecType ExecType)
        {
            Find("空", "空", ExecType);
        }

        public void Find(string name, string Id, CStockInventoryMng_ExecType ExecType)
        {
            string captionName = mainViewName;
            if (ExecType == CStockInventoryMng_ExecType.安装)
            {
                this.mainViewName += "(安装)";
            }
            else if (ExecType == CStockInventoryMng_ExecType.土建)
            {
                this.mainViewName += "(土建)";
            }
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

            VStockInventory vMainView = framework.GetMainView(mainViewName + "-空") as VStockInventory;

            if (vMainView == null)
            {
                vMainView = new VStockInventory(  ExecType);
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VStockInventorySearchCon searchCon = new VStockInventorySearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        public object Excute(params object[] args )
        {
            if (args.Length == 0)
            {
               // Start();
                
               // Start(CStockInventoryMng_ExecType.土建);
            } else
            {
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CStockInventoryMng_ExecType))
                {
                    CStockInventoryMng_ExecType execType = (CStockInventoryMng_ExecType)obj;
                    switch (execType)
                    {
                        case CStockInventoryMng_ExecType.StockInventoryQuery:
                            {
                                IMainView mroqMv = framework.GetMainView("月度盘点查询");
                                if (mroqMv != null)
                                {
                                    mroqMv.ViewShow();
                                    return null;
                                }
                                VStockInventoryQuery vmroq = new VStockInventoryQuery();
                                vmroq.ViewCaption = "月度盘点查询";
                                framework.AddMainView(vmroq);
                                return null;
                            }
                        case CStockInventoryMng_ExecType.安装:
                            {
                                
                                Start(CStockInventoryMng_ExecType.安装);
                                return null;
                            }
                        case CStockInventoryMng_ExecType.土建 :
                            {
                                
                                Start(CStockInventoryMng_ExecType.土建);
                                return null;
                            }
                        case CStockInventoryMng_ExecType.仓库报表:
                            {
                                IMainView mroqMv = framework.GetMainView("仓库报表(安装)");
                                if (mroqMv != null)
                                {
                                    mroqMv.ViewShow();
                                    return null;
                                }
                                VStockReport vmroq = new VStockReport();
                                vmroq.ViewCaption = "仓库报表(安装)";
                                framework.AddMainView(vmroq);
                                return null;
                            }
                    }
                }
            }
            return null;
        }
         
    }
}
