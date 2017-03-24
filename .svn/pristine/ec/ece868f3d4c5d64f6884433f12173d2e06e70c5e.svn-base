using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Windows.Documents;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.MeetingSummary
{
    public enum CMeetingManage_ExexType
    {
        VMeetingManage,
        /// <summary>
        /// 会议纪要查询
        /// </summary>
        MeetingManageSearch
    }

    public class CMeetingManage
    {
        private static IFramework framework = null;
        string mainViewName = "会议纪要管理";
        private static VMeetingSearchList searchList;


        public CMeetingManage(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VMeetingSearchList(this);
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

            VMeetingManage vMainView = framework.GetMainView(mainViewName + "-空") as VMeetingManage;

            if (vMainView == null)
            {
                vMainView = new VMeetingManage();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VMeetingSearchCon searchCon = new VMeetingSearchCon(searchList);
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
                if (obj != null && obj.GetType() == typeof(CMeetingManage_ExexType))
                {
                    CMeetingManage_ExexType execType = (CMeetingManage_ExexType)obj;
                    switch (execType)
                    {
                        case CMeetingManage_ExexType.MeetingManageSearch:
                            IMainView mroqMv = framework.GetMainView("会议纪要查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VMeetingQuery vcq = new VMeetingQuery();
                            vcq.ViewCaption = "会议纪要查询";
                            framework.AddMainView(vcq);
                            return null;
                        default:
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
