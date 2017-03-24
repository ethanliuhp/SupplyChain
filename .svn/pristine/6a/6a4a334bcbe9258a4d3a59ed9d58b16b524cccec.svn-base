using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SettlementManagement.ExpensesSettleMng
{
    public enum CExpensesSettleMng_ExecType
    {
        /// <summary>
        /// 费用结算查询
        /// </summary>
        ExpensesSettleQuery
    }
    
    public class CExpensesSettleMng
    {
        private static IFramework framework = null;
        string mainViewName = "费用结算单维护";
        private static VExpensesSettleSearchList searchList;

        public CExpensesSettleMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VExpensesSettleSearchList(this);
        }

        public void Start()
        {
            Find("空", "空");
        }

        public void Find(string name, string Id)
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

            VExpensesSettleMng vMainView = framework.GetMainView(mainViewName + "-空") as VExpensesSettleMng;

            if (vMainView == null)
            {
                vMainView = new VExpensesSettleMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VExpensesSettleSearchCon searchCon = new VExpensesSettleSearchCon(searchList);

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
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CExpensesSettleMng_ExecType))
                {
                    CExpensesSettleMng_ExecType execType = (CExpensesSettleMng_ExecType)obj;
                    switch (execType)
                    {
                        case CExpensesSettleMng_ExecType.ExpensesSettleQuery:
                            IMainView mroqMv = framework.GetMainView("费用结算单查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VExpensesSettleQuery vmroq = new VExpensesSettleQuery();
                            vmroq.ViewCaption = "费用结算单查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
