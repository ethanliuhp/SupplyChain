using System;
using System.Collections.Generic;
using System.Text;

using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.Indicator.IndicatorUse.DimUse
{
    public class CDataExplore
    {
        static IFramework framework;
        private string mainViewName = "数据浏览";
        private bool ifScore = false;

        public CDataExplore(IFramework fw,bool ifCalScore)
        {
            if (framework == null)
            {
                framework = fw;
            }
            ifScore = ifCalScore;
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
                VViewExplore mainView = new VViewExplore(ifScore);
                mainView.ShowDialog();
                /*framework.AddMainView(mainView);
                mainView.ViewCaption = caption;
                mainView.ViewName = caption;
                mainView.Start();*/
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
