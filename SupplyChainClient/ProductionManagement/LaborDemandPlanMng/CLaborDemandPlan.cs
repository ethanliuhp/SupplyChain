using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.LaborDemandPlanMng
{
    public enum CLaborDemandPlan_ExecType
    {
        /// <summary>
        /// 劳务需求计划
        /// </summary>
        LaborDemandPlanQuery
    }

    public class CLaborDemandPlan
    {
        private static IFramework framework = null;
        string mainViewName = "劳务需求计划";
        private static VLaborDemandPlanSearchList searchList;

        public CLaborDemandPlan(IFramework fm)
        {
            if (framework == null)
                framework = fm;
            searchList = new VLaborDemandPlanSearchList(this);
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

            VLaborDemandPlan vMainView = framework.GetMainView(mainViewName + "-空") as VLaborDemandPlan;

            if (vMainView == null)
            {
                vMainView = new VLaborDemandPlan();
                vMainView.ViewName = mainViewName;

                //载入查询视图
                //分配辅助视图
                vMainView.AssistViews.Add(searchList);
                VLaborDemandPlanSearchCon searchCon = new VLaborDemandPlanSearchCon(searchList);

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
                if (obj != null && obj.GetType() == typeof(CLaborDemandPlan_ExecType))
                {
                    CLaborDemandPlan_ExecType execType = (CLaborDemandPlan_ExecType)obj;
                    switch (execType)
                    {
                        case CLaborDemandPlan_ExecType.LaborDemandPlanQuery:
                            IMainView mroqMv = framework.GetMainView("劳务需求计划查询");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VLaborDemandPlanQuery vmroq = new VLaborDemandPlanQuery();
                            vmroq.ViewCaption = "劳务需求计划查询";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
