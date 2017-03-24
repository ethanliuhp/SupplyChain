using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.AcceptanceInspectionMng
{
    public enum CAcceptanceInspectionMng_ExecType
    {
        /// <summary>
        /// 验收检查记录查询
        /// </summary>
        AcceptanceInspectionQuery,
        Acceptance
    }
    
    public class CAcceptanceInspectionMng
    {
        private static IFramework framework = null;
        string mainViewName = "验收检查记录";
        private static VAcceptanceInspectionSearchList searchList;

        public CAcceptanceInspectionMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VAcceptanceInspectionSearchList(this);
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

            VAcceptanceInspection vMainView = framework.GetMainView(mainViewName + "-空") as VAcceptanceInspection;

            if (vMainView == null)
            {
                vMainView = new VAcceptanceInspection();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VAcceptanceInspectionSearchCon searchCon = new VAcceptanceInspectionSearchCon(searchList);

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
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CAcceptanceInspectionMng_ExecType))
                {
                    CAcceptanceInspectionMng_ExecType execType = (CAcceptanceInspectionMng_ExecType)obj;
                    switch (execType)
                    {
                        case CAcceptanceInspectionMng_ExecType.AcceptanceInspectionQuery:
                            IMainView mroqMv = framework.GetMainView("验收检查记录查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VAcceptanceInspectionQuery vmroq = new VAcceptanceInspectionQuery();
                            vmroq.ViewCaption = "验收检查记录查询";
                            framework.AddMainView(vmroq);
                            return null;
                        case CAcceptanceInspectionMng_ExecType.Acceptance:
                            IMainView mroqMv1 = framework.GetMainView("验收检查");
                            if (mroqMv1 != null)
                            {
                                mroqMv1.ViewShow();
                                return null;
                            }
                            VAcceptance vmroq1 = new VAcceptance();
                            vmroq1.ViewCaption = "验收检查";
                            framework.AddMainView(vmroq1);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
