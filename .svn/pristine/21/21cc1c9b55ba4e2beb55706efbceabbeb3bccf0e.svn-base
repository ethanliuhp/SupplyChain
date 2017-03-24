using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimDefine;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse
{
    public class CDimensionCategory
    {
        static IFramework framework;
        public CDimensionCategory(IFramework fw)
        {
            if (framework == null)
            {
                framework = fw;
            }
        }

        public void Start()
        {
            string viewName = "Œ¨∂»Œ¨ª§";
            IMainView mv = framework.GetMainView(viewName);
            if (mv != null)
                mv.ViewShow();
            else
            {
                VDimensionDefine view = new VDimensionDefine();
                view.ViewCaption = viewName;
                view.ViewName = viewName;
                framework.AddMainView(view);
                view.Start();
            }
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
                Start();
            return null;
        }
    }
}
