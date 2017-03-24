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

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng
{
    public enum OperationCostItemType
    {
        成本项维护 = 1,
        成本项导入 = 2
    }

    public class CCostItem
    {
        static IFramework framework;

        public CCostItem(IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public void Start()
        {
            string viewName = "成本项维护";
            if (!ConstObject.FrameWorkNewFlag)
            {
                IMainView mv = framework.GetMainView(viewName);
                if (mv != null)
                    mv.ViewShow();
                else
                {
                    MCostItem mmc = new MCostItem();
                    VCostItem vmc = new VCostItem(mmc);
                    vmc.ViewCaption = viewName;
                    framework.AddMainView(vmc);
                    vmc.Start();
                }
            }
            else
            {
                MCostItem mmc = new MCostItem();
                VCostItem vmc = new VCostItem(mmc);
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
                OperationCostItemType executeType = (OperationCostItemType)o;

                switch (executeType)
                {
                    case OperationCostItemType.成本项维护:
                        string viewName = "成本项维护";
                        IMainView mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MCostItem mmc = new MCostItem();
                            VCostItem vmc = new VCostItem(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationCostItemType.成本项导入:
                        viewName = "成本项导入";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VCostItemImport vmc = new VCostItemImport(new MCostItem());
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
