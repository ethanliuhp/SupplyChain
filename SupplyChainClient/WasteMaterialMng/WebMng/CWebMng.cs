using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.WebMng
{
    public class CWebMng
    {
        private static IFramework framework = null;
        string mainViewName = "文档管理";
        
        public CWebMng(IFramework fm)
        {
            if (framework == null) framework = fm;
        }

        public void Start()
        {
            Find("");
            //Find("空");
        }

        public void Find(string name)
        {
            string captionName = mainViewName;
            //if (name is string)
            //{
            //    captionName = this.mainViewName + "-" + name;
            //}

            IMainView mv = framework.GetMainView(captionName);

            if (mv != null)
            {
                //如果当前视图已经存在，直接显示
                mv.ViewShow();
                return;
            }

            VWebPageInvokeTest vwpit = framework.GetMainView(captionName) as VWebPageInvokeTest;

            if (vwpit == null)
            {
                vwpit = new VWebPageInvokeTest();
                vwpit.ViewName = mainViewName;
                //载入框架
                framework.AddMainView(vwpit);
            }

            vwpit.ViewCaption = captionName;
            vwpit.ViewName = mainViewName;
            //vwpit.Start(name);
            vwpit.LoadPage();
            vwpit.ViewShow();
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                Start();
            } else
            {
                string o = args[0].ToString();
                if (o.Equals("WorkFlowHistory", StringComparison.OrdinalIgnoreCase))
                {
                    string captionName="工作流历史";                    
                    IMainView mv = framework.GetMainView(captionName);
                    if (mv != null)
                    {
                        ((WorkFlowHistoryPage)mv).LoadPage();
                        mv.ViewShow();
                        return null;
                    }
                    WorkFlowHistoryPage wfhp = new WorkFlowHistoryPage();
                    wfhp.ViewName = captionName;
                    wfhp.ViewCaption = captionName;
                    framework.AddMainView(wfhp);
                    wfhp.LoadPage();
                    wfhp.ViewShow();
                    return null;
                }
                else if (o.Equals("IRP同步", StringComparison.OrdinalIgnoreCase))
                {
                    ReloadIrpXML reloadIrpXML = new ReloadIrpXML();
                    reloadIrpXML.ShowDialog();
                    return null;
                }
            }
            return null;
        }
    }
}
