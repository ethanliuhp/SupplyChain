using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ConcreteManage.ConCheckingMng
{
    public enum CConCheck_ExceType
    {
        /// <summary>
        /// 商品砼对账单查询
        /// </summary>
        ConPouringNoteQuery
    }
    public class CConCheck
    {
        private static IFramework framework = null;
        string mainViewName = "商品砼对账单";
        private static VConCheckSearchList searchList;

        public CConCheck(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VConCheckSearchList(this);
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
            VConCheck vMainView = framework.GetMainView(mainViewName + "-空") as VConCheck;

            if (vMainView == null)
            {
                vMainView = new VConCheck();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VConCheckSearchCon searchCon = new VConCheckSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CConCheck_ExceType))
                {
                    CConCheck_ExceType execType = (CConCheck_ExceType)obj;
                    switch (execType)
                    {
                        case CConCheck_ExceType.ConPouringNoteQuery:
                            IMainView mroqMv = framework.GetMainView("对账单统计查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VConCheckQuery vccq = new VConCheckQuery();
                            vccq.ViewCaption = "对账单统计查询";
                            framework.AddMainView(vccq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
