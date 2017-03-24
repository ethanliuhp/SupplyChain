using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.FinanceMultData
{
    public class CFactoringData
    {
        public CFactoringData(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VFactoringDataSearchList(this);
        }
        private static IFramework framework = null;
        private static VFactoringDataSearchList searchList;
        string mainViewName = "保理单";
        public object Excute(params object[] args)
        {
            if (args != null && args.Length>0)
            {
                Report();
            }
            else
            {
                Start();
            }
            return null;
        }

        private void Start()
        {
            Find("空","空");
        }

        public void Find(string name,string id)
        {
            string captionName = mainViewName;                          
            if (name is string)
            {
                captionName = mainViewName + "-" + name;                // 视图标题 
            }
            IMainView mv = framework.GetMainView(captionName);          // 在主窗体中获取当前视图
            if (mv != null)
            {
                // 如果当前视图存在，则直接显示
                mv.ViewShow();
                return;
            }
            // 视图不存在
            VFactoringData vMainView = new VFactoringData();
           
            // 载入查询视图
            vMainView.ViewDeletedEvent += new Application.Business.Erp.SupplyChain.Client.Basic.Template.ViewDeletedHandle(a =>
            {
                var vFactoringData = a as VFactoringData;
                if (vFactoringData != null) searchList.RemoveRow(vMainView.Master.Id);
                IList list = framework.GetMainViews(vMainView.ViewName);
                if (list.Count > 1)
                {
                    framework.CloseMainView(vMainView);
                }
            });
            // 分配辅助视图
            vMainView.AssistViews.Add(searchList);
            VFactoringDataSearchCon searchCon = new VFactoringDataSearchCon(searchList);
            vMainView.AssistViews.Add(searchCon);
            // 载入框架
            framework.AddMainView(vMainView);
            vMainView.ViewCaption = captionName;
            vMainView.ViewName = mainViewName;
            vMainView.Start(id);
        }
        
        /// <summary>
        /// 保理台帐
        /// </summary>
        private void Report()
        {
            var viewName = "保理台账";
            IMainView mv = framework.GetMainView(viewName);
            if (mv != null)
            {
                mv.ViewShow();
            }
            else
            {
                VFactoringDataReport report = new VFactoringDataReport();
                report.ViewCaption = viewName;
                framework.AddMainView(report);
            }
        }

    }
}
