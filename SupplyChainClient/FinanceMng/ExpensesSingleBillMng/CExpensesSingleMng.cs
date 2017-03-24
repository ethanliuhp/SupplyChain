using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesSingleBillMng
{
    public enum CExpensesSingleBillMng_ExecType
    {
        /// <summary>
        /// 费用报销查询
        /// </summary>
        ExpensesSingleBillQuery
    }
    
    public class CExpensesSingleBillMng
    {
        private static IFramework framework = null;
        string mainViewName = "费用报销单";
        private static VExpensesSingleSearchList searchList;

        public CExpensesSingleBillMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VExpensesSingleSearchList(this);
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

            VExpensesSingleMng vMainView = framework.GetMainView(mainViewName + "-空") as VExpensesSingleMng;

            if (vMainView == null)
            {
                vMainView = new VExpensesSingleMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VExpensesSingleSearchCon searchCon = new VExpensesSingleSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CExpensesSingleBillMng_ExecType))
                {
                    CExpensesSingleBillMng_ExecType execType = (CExpensesSingleBillMng_ExecType)obj;
                    switch (execType)
                    {
                        case CExpensesSingleBillMng_ExecType.ExpensesSingleBillQuery:
                            IMainView mroqMv = framework.GetMainView("费用报销查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VExpensesSingleQuery vmroq = new VExpensesSingleQuery();
                            vmroq.ViewCaption = "费用报销查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
