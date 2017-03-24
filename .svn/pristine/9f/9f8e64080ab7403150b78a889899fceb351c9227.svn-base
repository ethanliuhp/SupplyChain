using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesRowBillMng
{
    public enum CExpensesRowBillMng_ExecType
    {
        /// <summary>
        /// 费用划账查询
        /// </summary>
        ExpensesRowBillQuery
    }
    
    public class CExpensesRowBillMng
    {
        private static IFramework framework = null;
        string mainViewName = "费用划账单";
        private static VExpensesRowBillSearchList searchList;

        public CExpensesRowBillMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VExpensesRowBillSearchList(this);
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

            VExpensesRowBillMng vMainView = framework.GetMainView(mainViewName + "-空") as VExpensesRowBillMng;

            if (vMainView == null)
            {
                vMainView = new VExpensesRowBillMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VExpensesRowBillSearchCon searchCon = new VExpensesRowBillSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CExpensesRowBillMng_ExecType))
                {
                    CExpensesRowBillMng_ExecType execType = (CExpensesRowBillMng_ExecType)obj;
                    switch (execType)
                    {
                        case CExpensesRowBillMng_ExecType.ExpensesRowBillQuery:
                            IMainView mroqMv = framework.GetMainView("费用划账查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VExpensesRowBillQuery vmroq = new VExpensesRowBillQuery();
                            vmroq.ViewCaption = "费用划账查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
