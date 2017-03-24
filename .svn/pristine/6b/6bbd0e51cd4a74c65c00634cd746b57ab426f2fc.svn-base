using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount
{
    public enum CSpecialAccount_ExecType
    {
        /// <summary>
        /// 专项费用结算查询
        /// </summary>
        SpecialAccountQuery
    }

    public class CSpecialAccount
    {
        private static IFramework framework = null;
        string mainViewName = "专项费用结算单";
        private static VSpecialAccountSearchList searchList;

        public CSpecialAccount(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VSpecialAccountSearchList(this);
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

            VSpecialAccount vMainView = framework.GetMainView(mainViewName + "-空") as VSpecialAccount;

            if (vMainView == null)
            {
                vMainView = new VSpecialAccount();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VSpecialAccountSearchCon searchCon = new VSpecialAccountSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CSpecialAccount_ExecType))
                {
                    CSpecialAccount_ExecType execType = (CSpecialAccount_ExecType)obj;
                    switch (execType)
                    {
                        case CSpecialAccount_ExecType.SpecialAccountQuery:
                            IMainView mroqMv = framework.GetMainView("专项费用结算查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VSpecialAccountQuery vmroq = new VSpecialAccountQuery();
                            vmroq.ViewCaption = "专项费用结算查询";
                            framework.AddMainView(vmroq);
                            return null;
                       
                    }
                }
            }
            return null;
        }
    }
}
