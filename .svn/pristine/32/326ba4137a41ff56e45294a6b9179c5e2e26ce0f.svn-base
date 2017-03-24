using System;
using System.Collections.Generic;
using System.Text;

using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public class CViewDefTechEco
    {
        static IFramework framework;
        private string mainViewName = "技经指标模板定义";

        public CViewDefTechEco(IFramework fw)
        {
            if (framework == null)
            {
                framework = fw;
            }
        }

        private void Find()
        {
            string caption = mainViewName;
            IMainView mv = framework.GetMainView(caption);
            if (mv != null)
            {
                mv.ViewShow();
            }
            else
            {
                VViewDefTechEco mainView = new VViewDefTechEco();
                framework.AddMainView(mainView);
                mainView.ViewCaption = caption;
                mainView.ViewName = caption;
                mainView.Start();
            }
        }

        private void Start()
        {
            Find();
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            return null;
        }
    }
}
