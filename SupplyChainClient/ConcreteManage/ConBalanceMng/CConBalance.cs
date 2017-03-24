using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng
{
    public enum CConBalance_ExceType
    {
        /// <summary>
        /// 商品砼结算查询
        /// </summary>
        ConBalanceQuery
    }
    public class CConBalance
    {
        private static IFramework framework = null;
        string mainViewName = "商品砼结算单";
        private static VConBalanceSearchList searchList;

        public CConBalance(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VConBalanceSearchList(this);
        }

        public void Start()
        {
            Find("空", "空");
        }
        public void Find(string name, string id)
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
            VConBalance vMainView = framework.GetMainView(mainViewName + "-空") as VConBalance;

            if (vMainView == null)
            {
                vMainView = new VConBalance();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VConBalSearchCon searchCon = new VConBalSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(id);

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
                if (obj != null && obj.GetType() == typeof(CConBalance_ExceType))
                {
                    CConBalance_ExceType execType = (CConBalance_ExceType)obj;
                    switch (execType)
                    {
                        case CConBalance_ExceType.ConBalanceQuery:
                            IMainView mroqMv = framework.GetMainView("商品砼结算单统计查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VConBalanceQuery vccq = new VConBalanceQuery();
                            vccq.ViewCaption = "商品砼结算单统计查询";
                            framework.AddMainView(vccq);
                            return null;
                    }
                }
            }
            return null;
        }

    }
}
