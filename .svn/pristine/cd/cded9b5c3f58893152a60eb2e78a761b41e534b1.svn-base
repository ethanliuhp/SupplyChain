using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.RectificationNoticeMng
{
    public enum CRectificationNotice_ExecType
    {
        /// <summary>
        /// 整改通知单查询
        /// </summary>
        RectificationNoticeQuery,
        RectificationNoticeSelector
    }

    public class CRectificationNoticeMng
    {
        private static IFramework framework = null;
        string mainViewName = "整改通知单";
        private static VRectificationNoticeSearchList searchList;

        public CRectificationNoticeMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VRectificationNoticeSearchList(this);
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

            VRectificationNoticeMng vMainView = framework.GetMainView(mainViewName + "-空") as VRectificationNoticeMng;

            if (vMainView == null)
            {
                vMainView = new VRectificationNoticeMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VRectificationNoticeSearchCon searchCon = new VRectificationNoticeSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CRectificationNotice_ExecType))
                {
                    CRectificationNotice_ExecType execType = (CRectificationNotice_ExecType)obj;
                    switch (execType)
                    {
                        case CRectificationNotice_ExecType.RectificationNoticeQuery:
                            IMainView mroqMv = framework.GetMainView("整改通知查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VRectificationNoticeQuery vmroq = new VRectificationNoticeQuery();
                            vmroq.ViewCaption = "整改通知查询";
                            framework.AddMainView(vmroq);
                            return null;
                        case CRectificationNotice_ExecType.RectificationNoticeSelector:
                            IMainView mroMv1=framework.GetMainView("整改单复核");
                            if (mroMv1 != null)
                            {
                                mroMv1.ViewShow();
                                return null;
                            }
                           VRectificationNoticeSelector mroMv2 = new VRectificationNoticeSelector();
                           mroMv2.ViewCaption = "整改单复核";
                           framework.AddMainView(mroMv2);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
