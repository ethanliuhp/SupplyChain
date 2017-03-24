using System;
using System.Collections.Generic;
using System.Text;

using VirtualMachine.Component.WinMVC.core;
using VirtualMachine.Component.WinMVC.generic;

using VirtualMachine.Component.WinControls.Controls;
using Application.Resource.MaterialResource.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Business.Erp.ResourceManager.Client.Main;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using IFramework = VirtualMachine.Component.WinMVC.generic.IFramework;
using Application.Business.Erp.SupplyChain.Client.FileUpload;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.QWBS
{
    public enum OperationQWBS
    {
        清单WBS管理
    }

    public class CQWBSManagement
    {
        static IFramework framework;

        public CQWBSManagement(IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length > 0)
            {
                object o = args[0];
                OperationQWBS executeType = (OperationQWBS)o;

                string viewName = "";
                IMainView mv = null;
                switch (executeType)
                {
                    case OperationQWBS.清单WBS管理:
                        viewName = "清单WBS管理";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MQWBSManagement mmc = new MQWBSManagement();
                            VQWBSTree vmc = new VQWBSTree(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    default:
                        break;
                }
            }

            return null;
        }
    }
}
