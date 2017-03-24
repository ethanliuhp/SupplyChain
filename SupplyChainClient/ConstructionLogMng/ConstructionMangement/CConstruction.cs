using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ConstructionLogMng.ConstructionManagement
{
    public enum CConstructionMng_ExecType
    {
        /// <summary>
        /// 施工日志查询
        /// </summary>
        ConstructionQuery
    }

    public class CConstruction
    {
        private static IFramework framework = null;
        string mainViewName = "施工日志信息";
        private static VConstructionSearchList searchList;

        public CConstruction(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VConstructionSearchList(this);
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

            VConstruction vMainView = framework.GetMainView(mainViewName) as VConstruction;

            if (vMainView == null)
            {
                vMainView = new VConstruction();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VConstructionSearchCon searchCon = new VConstructionSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CConstructionMng_ExecType))
                {
                    CConstructionMng_ExecType execType = (CConstructionMng_ExecType)obj;
                    switch (execType)
                    {
                        case CConstructionMng_ExecType.ConstructionQuery:
                            IMainView mroqMv = framework.GetMainView("施工日志信息查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VConstructionQuery vmroq = new VConstructionQuery();
                            vmroq.ViewCaption = "施工日志信息查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
