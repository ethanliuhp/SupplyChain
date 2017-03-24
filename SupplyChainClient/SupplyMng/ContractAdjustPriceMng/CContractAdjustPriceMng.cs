using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng.ContractAdjustPriceMng
{
    public enum CContractAdjustPrice_ExecType
    {
        /// <summary>
        /// 采购合同调价
        /// </summary>
        ContractAdjustPriceQuery,

        /// <summary>
        /// 采购合同调价查询
        /// </summary>
        ContractAdujustPriceQueryNew
    }
    
    public class CContractAdjustPriceMng
    {
        private static IFramework framework = null;
        string mainViewName = "采购合同调价";
        public CContractAdjustPriceMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        //public void Start()
        //{
        //    Find("空");
        //}

        //public void Find(string name)
        //{
        //    string captionName = mainViewName;
        //    if (name is string)
        //    {
        //        captionName = this.mainViewName + "-" + name;
        //    }

        //    IMainView mv = framework.GetMainView(captionName);

        //    if (mv != null)
        //    {
        //        //如果当前视图已经存在，直接显示
        //        mv.ViewShow();
        //        return;
        //    }

        //    VContractAdjustPrice vMainView = framework.GetMainView(mainViewName + "-空") as VContractAdjustPrice;

        //    if (vMainView == null)
        //    {
        //        vMainView = new VContractAdjustPrice();
        //        vMainView.ViewName = mainViewName;

        //        //载入查询视图
        //        //分配辅助视图
        //        //vMainView.AssistViews.Add(searchList);
        //        //VSupplyOrderSearchCon searchCon = new VSupplyOrderSearchCon(searchList);

        //        //vMainView.AssistViews.Add(searchCon);

        //        //载入框架
        //        framework.AddMainView(vMainView);
        //    }
        //    vMainView.ViewCaption = captionName;
        //    vMainView.ViewName = mainViewName;
        //    vMainView.Start(name);
        //    vMainView.ViewShow();
        //}

        public object Excute(params object[] args)
        {
            //if (args.Length == 0)
            //{
            //    Start();
            //} else
            //{
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CContractAdjustPrice_ExecType))
                {
                    CContractAdjustPrice_ExecType execType = (CContractAdjustPrice_ExecType)obj;
                    switch (execType)
                    {
                        case CContractAdjustPrice_ExecType.ContractAdjustPriceQuery:
                            IMainView mroqMv = framework.GetMainView("采购合同调价");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VContractAdjustPriceQuery vmroq = new VContractAdjustPriceQuery();
                            vmroq.ViewCaption = "采购合同调价";
                            framework.AddMainView(vmroq);
                            return null;
                        case CContractAdjustPrice_ExecType.ContractAdujustPriceQueryNew:
                            IMainView mcapView = framework.GetMainView("采购合同调价查询");
                            if (mcapView != null)
                            {
                                mcapView.ViewShow();
                                return null;
                            }
                            VContractAdjustPriceQueryNew vmcapView = new VContractAdjustPriceQueryNew();
                            vmcapView.ViewCaption = "采购合同调价查询";
                            framework.AddMainView(vmcapView);
                            return null;

                    }
                }
            //}
            return null;
        }
    }
}
