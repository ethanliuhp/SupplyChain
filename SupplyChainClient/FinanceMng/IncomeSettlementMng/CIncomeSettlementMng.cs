using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.FinnaceMng.IncomeSettlementMng;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.IncomeSettlementMng
{
    public enum CIncomeSettlementMng_ExecType
    {
        /// <summary>
        /// 当期收益结算单查询
        /// </summary>
        IncomeSettlementQuery
    }
    
    public class CIncomeSettlementMng
    {
        private static IFramework framework = null;
        string mainViewName = "当期收益结算单";
        private static VIncomeSettlementSearchList searchList;

        public CIncomeSettlementMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VIncomeSettlementSearchList(this);
        }

        public void Start()
        {
            Find("空","空");
        }

        public void Find(string name,string Id)
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

            VIncomeSettlementMng vMainView = framework.GetMainView(mainViewName + "-空") as VIncomeSettlementMng;

            if (vMainView == null)
            {
                vMainView = new VIncomeSettlementMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VIncomeSettlementSearchCon searchCon = new VIncomeSettlementSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(Id);

            vMainView.ViewShow();
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            } else
            {
                object obj=args[0];
                if (obj != null && obj.GetType() == typeof(CIncomeSettlementMng_ExecType))
                {
                    CIncomeSettlementMng_ExecType execType = (CIncomeSettlementMng_ExecType)obj;
                    switch (execType)
                    {
                        case CIncomeSettlementMng_ExecType.IncomeSettlementQuery:
                            IMainView mroqMv = framework.GetMainView("当前收益结算单查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VIncomeSettlementQuery vmroq = new VIncomeSettlementQuery();
                            vmroq.ViewCaption = "当前收益结算单查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
