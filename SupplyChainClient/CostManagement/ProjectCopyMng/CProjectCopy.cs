using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;
using Application.Business.Erp.SupplyChain.Client.CostManagement.PBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectCopyMng
{
    public enum CProjectCopy_ExecType
    {
        ProjectCopy
    }
    public class CProjectCopy
    {
        static IFramework framework;
        string mainViewName = "项目工程复制";
        public CProjectCopy(IFramework fw)
        {
            if (framework == null)
                framework = fw;
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
                Start();
            else
            {
                object o = args[0];
                CProjectCopy_ExecType executeType = (CProjectCopy_ExecType)o;

                switch (executeType)
                {
                    case CProjectCopy_ExecType.ProjectCopy:
                        string viewName = "项目工程复制";
                        IMainView mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MPBSTree mPBS = new MPBSTree();
                            MGWBSTree mWBS = new MGWBSTree();
                            VProjectCopy vmc = new VProjectCopy(mPBS,mWBS);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                }
            }

            return null;
        }
    }
}
