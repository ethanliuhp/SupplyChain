using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.UnitMng
{
    public enum CUnitMng_ExecType
    {
        /// <summary>
        /// 计量单位查询
        /// </summary>
        UnitMng
    }
    
    public class CUnitMng
    {
        private static IFramework framework = null;
        string mainViewName = "计量单位配置";

        public CUnitMng(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        //public void Start()
        //{
        //    Find("空");
        //}

        //public void Find(string name)
        //{
        //    string captionName = mainViewName;
        //    if (name is string)
        //    {
        //        captionName = this.mainViewName + "-" + name;
        //    }

        //    IMainView mv = framework.GetMainView(captionName);

        //    if (mv != null)
        //    {
        //        //如果当前视图已经存在，直接显示
        //        mv.ViewShow();
        //        return;
        //    }

        //    VUnitMng vMainView = framework.GetMainView(mainViewName + "-空") as VUnitMng;

        //    if (vMainView == null)
        //    {
        //        vMainView = new VUnitMng();
        //        vMainView.ViewName = mainViewName;

        //        //载入查询视图
        //        //分配辅助视图
        //        //vMainView.AssistViews.Add(searchList);
        //        //VDemandMasterPlanSearchCon searchCon = new VDemandMasterPlanSearchCon(searchList);

        //        //vMainView.AssistViews.Add(searchCon);

        //        //载入框架
        //        framework.AddMainView(vMainView);
        //    }

        //    vMainView.ViewCaption = captionName;
        //    vMainView.ViewName = mainViewName;
        //    vMainView.Start(name);

        //    vMainView.ViewShow();
        //}

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                //Start();
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CUnitMng_ExecType))
                {
                    CUnitMng_ExecType execType = (CUnitMng_ExecType)obj;
                    switch (execType)
                    {
                        case CUnitMng_ExecType.UnitMng:
                            IMainView mroqMv = framework.GetMainView("计量单位配置");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            //MGWBSTree mmc = new MGWBSTree();
                            VUnitMng vmroq = new VUnitMng();
                            vmroq.ViewCaption = "计量单位配置";
                            framework.AddMainView(vmroq);
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
