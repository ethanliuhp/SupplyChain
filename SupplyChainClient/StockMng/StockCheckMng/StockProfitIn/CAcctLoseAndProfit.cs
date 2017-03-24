using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using VirtualMachine.Component;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.Client.StockManage.StockCheck.StockProfitIn
{
    public class CAcctLoseAndProfit
    {
        private static IFramework framework = null;
        string mvname = "账面盘盈盘亏单";

        private static VAcctLoseAndProfitList searchListView;

        public CAcctLoseAndProfit(IFramework fm)
        {
            if (framework == null)
            {
                framework = fm;
            }
            searchListView = new VAcctLoseAndProfitList(this);
        }

        public object Execute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object o = args[0];
            }

            return null;
        }

        public void Start()
        {
            Find("空");
        }

        public void Find(string name)
        {
            string captionName = mvname;

            if (name is string)
            {
                captionName = this.mvname + "-" + name;
            }

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VAcctLoseAndProfit vAcct = framework.GetMainView(mvname + "-空") as VAcctLoseAndProfit;

            if (vAcct == null)
            {
                vAcct = new VAcctLoseAndProfit();
                vAcct.ViewName = mvname;

                //载入查询视图
                vAcct.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(vAcct_ViewDeletedEvent);

                //分配辅助视图
                vAcct.AssistViews.Add(searchListView);

                VAcctLoseAndProfitCon theVAcctCon = new VAcctLoseAndProfitCon();
                theVAcctCon.theVList = searchListView;
                vAcct.AssistViews.Add(theVAcctCon);

                vAcct.theVList = searchListView;
                //载入框架
                framework.AddMainView(vAcct);
            }

            vAcct.ViewCaption = captionName;
            vAcct.ViewName = mvname;
            vAcct.Start(name);

            vAcct.ViewShow();            
        }

        public void vAcct_ViewDeletedEvent(object sender)
        {
            IMainView mv = sender as IMainView;

            VAcctLoseAndProfit vAcct = mv as VAcctLoseAndProfit;
            if (vAcct != null)
            {
                searchListView.RemoveRow(vAcct.theAcctLostAndProfit.Id);
            }

            IList lst = framework.GetMainViews(mv.ViewName);

            if (lst.Count > 1)
                framework.CloseMainView(mv);
        }
    }
}
