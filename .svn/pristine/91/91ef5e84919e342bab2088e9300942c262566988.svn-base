using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialCost
{
    public enum CSpecialCostOrder_ExecType
    {
        /// <summary>
        /// 专项费用管理查询
        /// </summary>
        SpecialCostQuery
    }

    public class CSpecialCost
    {
        private static IFramework framework = null;
        string mainViewName = "专项费用管理单";
        private static VSpecialCostSearchList searchList;

        public CSpecialCost(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VSpecialCostSearchList(this);
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

            VSpecialCost vMainView = framework.GetMainView(mainViewName + "-空") as VSpecialCost;

            if (vMainView == null)
            {
                vMainView = new VSpecialCost();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VSpecialCostSearchCon searchCon = new VSpecialCostSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CSpecialCostOrder_ExecType))
                {
                    CSpecialCostOrder_ExecType execType = (CSpecialCostOrder_ExecType)obj;
                    switch (execType)
                    {
                        case CSpecialCostOrder_ExecType.SpecialCostQuery:
                            IMainView mroqMv = framework.GetMainView("专项费用管理查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VSpecialCostQuery vmroq = new VSpecialCostQuery();
                            vmroq.ViewCaption = "专项费用管理查询";
                            framework.AddMainView(vmroq);
                            return null;
                       
                    }
                }
            }
            return null;
        }
    }
}
