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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng
{
    public enum OperationContractGroupType
    {
        合约商务信息维护 = 1
    }

    public class CWBSContractGroup
    {
        static IFramework framework;

        public CWBSContractGroup(IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length > 0)
            {
                object o = args[0];
                OperationContractGroupType executeType = (OperationContractGroupType)o;

                string viewName = "";
                IMainView mv = null;
                switch (executeType)
                {
                    case OperationContractGroupType.合约商务信息维护:
                        viewName = "合约商务信息维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MWBSContractGroup mmc = new MWBSContractGroup();
                            VWBSContractGroup vmc = new VWBSContractGroup(mmc);
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
