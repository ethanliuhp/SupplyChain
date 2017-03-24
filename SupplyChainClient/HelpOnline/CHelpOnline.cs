using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.HelpOnline
{
    public enum CHelpOnline_ExexType
    {
        HelpOnline
    }

    public class CHelpOnline
    {
        private static IFramework framework = null;
        string mainViewName = "在线帮助";

        public CHelpOnline(IFramework fm)
        {
            if (framework == null)
                framework = fm;
        }

        public void Start()
        {
            Find("空", "空");
        }

        public void Find(string name, string Id)
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
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            }
            else
            {
                object obj = args[0];
                if (obj != null && obj.GetType() == typeof(CHelpOnline_ExexType))
                {
                    CHelpOnline_ExexType execType = (CHelpOnline_ExexType)obj;
                    switch (execType)
                    {
                        case CHelpOnline_ExexType.HelpOnline:
                            IMainView mroqMv = framework.GetMainView("在线帮助");
                            if (mroqMv != null)
                            {
                                mroqMv.ViewShow();
                                return null;
                            }
                            VHelpOnline vcq = new VHelpOnline();
                            vcq.ViewCaption = "在线帮助";
                            framework.AddMainView(vcq);
                            return null;
                        default:
                            return null;
                    }
                }
            }
            return null;
        }
    }
}
