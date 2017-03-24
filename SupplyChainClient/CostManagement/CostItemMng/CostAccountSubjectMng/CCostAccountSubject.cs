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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostAccountSubjectMng
{
    public enum OperationCostAccountSubjectType
    {
        �ɱ������Ŀ = 1
    }

    public class CCostItemCategory
    {
        static IFramework framework;

        public CCostItemCategory(IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public void Start()
        {
            string viewName = "�ɱ������Ŀ";
            if (!ConstObject.FrameWorkNewFlag)
            {
                IMainView mv = framework.GetMainView(viewName);
                if (mv != null)
                    mv.ViewShow();
                else
                {
                    MCostAccountSubject mmc = new MCostAccountSubject();
                    VCostAccountSubject vmc = new VCostAccountSubject(mmc);
                    vmc.ViewCaption = viewName;
                    framework.AddMainView(vmc);
                    vmc.Start();
                }
            }
            else
            {
                MCostAccountSubject mmc = new MCostAccountSubject();
                VCostAccountSubject vmc = new VCostAccountSubject(mmc);
                vmc.ViewCaption = viewName;
                AppDomain.CurrentDomain.SetData("mvv", vmc);
                vmc.Start();
            }
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
                Start();
            else
            {
                object o = args[0];
                OperationCostAccountSubjectType executeType = (OperationCostAccountSubjectType)o;

                switch (executeType)
                {
                    case OperationCostAccountSubjectType.�ɱ������Ŀ:
                        string viewName = "�ɱ������Ŀ";
                        IMainView mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MCostAccountSubject mmc = new MCostAccountSubject();
                            VCostAccountSubject vmc = new VCostAccountSubject(mmc);
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
