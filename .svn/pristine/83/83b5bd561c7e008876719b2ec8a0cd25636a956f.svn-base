using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.DelimitIndividualBillMng
{
    public enum CDelimitIndividualBillMng_ExecType
    {
        /// <summary>
        /// 个人借款单查询
        /// </summary>
        DelimitIndividualBillQuery
    }
    
    public class CDelimitIndividualBillMng
    {
        private static IFramework framework = null;
        string mainViewName = "个人借款单";
        private static VDelimitIndividualBillSearchList searchList;

        public CDelimitIndividualBillMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VDelimitIndividualBillSearchList(this);
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

            VDelimitIndividualBillMng vMainView = framework.GetMainView(mainViewName + "-空") as VDelimitIndividualBillMng;

            if (vMainView == null)
            {
                vMainView = new VDelimitIndividualBillMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VDelimitIndividualBillSearchCon searchCon = new VDelimitIndividualBillSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CDelimitIndividualBillMng_ExecType))
                {
                    CDelimitIndividualBillMng_ExecType execType = (CDelimitIndividualBillMng_ExecType)obj;
                    switch (execType)
                    {
                        case CDelimitIndividualBillMng_ExecType.DelimitIndividualBillQuery:
                            IMainView mroqMv = framework.GetMainView("个人借款查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VDelimitIndividualBillQuery vmroq = new VDelimitIndividualBillQuery();
                            vmroq.ViewCaption = "个人借款查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
