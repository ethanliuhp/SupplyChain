using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.FinnaceMng.OverlayAmortizeMng;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.OverlayAmortizeMng
{
    public enum COverlayAmortizeMng_ExecType
    {
        /// <summary>
        /// 临建摊销单查询
        /// </summary>
        OverlayAmortizeQuery
    }
    
    public class COverlayAmortizeMng
    {
        private static IFramework framework = null;
        string mainViewName = "临建摊销单";
        private static VOverlayAmortizeSearchList searchList;

        public COverlayAmortizeMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VOverlayAmortizeSearchList(this);
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

            VOverlayAmortizeMng vMainView = framework.GetMainView(mainViewName + "-空") as VOverlayAmortizeMng;

            if (vMainView == null)
            {
                vMainView = new VOverlayAmortizeMng();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VOverlayAmortizeSearchCon searchCon = new VOverlayAmortizeSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(COverlayAmortizeMng_ExecType))
                {
                    COverlayAmortizeMng_ExecType execType = (COverlayAmortizeMng_ExecType)obj;
                    switch (execType)
                    {
                        case COverlayAmortizeMng_ExecType.OverlayAmortizeQuery:
                            IMainView mroqMv = framework.GetMainView("临建摊销查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VOverlayAmortizeQuery vmroq = new VOverlayAmortizeQuery();
                            vmroq.ViewCaption = "临建摊销查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
