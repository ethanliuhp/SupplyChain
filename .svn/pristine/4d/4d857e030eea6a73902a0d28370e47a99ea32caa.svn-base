using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.BasicData
{
    public class CBasicData
    {
        static IFramework framework;
        public CBasicData(IFramework fw)
        {
            if (framework == null)
            {
                framework = fw;
            }
        }

        public void Start()
        {
            string viewName = "基础数据维护";
            IMainView mv = framework.GetMainView(viewName);
            if (mv != null)
                mv.ViewShow();
            else
            {
                VBasicData vBasicData = new VBasicData();
                vBasicData.ViewCaption = viewName;
                vBasicData.ViewName = viewName;
                framework.AddMainView(vBasicData);
                vBasicData.Start();
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
