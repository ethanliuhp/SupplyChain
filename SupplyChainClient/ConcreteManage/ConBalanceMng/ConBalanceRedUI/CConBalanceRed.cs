using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConBalanceMng.ConBalanceRedUI
{

    public class CConBalanceRed
    {
        private static IFramework framework = null;
        string mainViewName = "商品砼结算红单";
        private static VConBalanceRedSearchList searchList;

        public CConBalanceRed(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VConBalanceRedSearchList(this);
        }

        public void Start()
        {
            Find("空");
        }
        public void Find(string name)
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
            VConBalanceRed vMainView = framework.GetMainView(mainViewName + "-空") as VConBalanceRed;

            if (vMainView == null)
            {
                vMainView = new VConBalanceRed();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VConBalRedSearchCon searchCon = new VConBalRedSearchCon(searchList);

                vMainView.AssistViews.Add(searchCon);

                //载入框架
                framework.AddMainView(vMainView);
            }

            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(name);

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

                    }
                }
            }
            return null;
        }
    }
}
