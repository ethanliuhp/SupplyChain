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
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public enum OperationPBSTreeType
    {
        施工部位划分维护 = 1,
        特性集维护 = 2,
        施工部位模板维护 = 3,
        施工部位结构类型维护 = 4,
        PBS节点维护 = 5
    }

    public class CPBSTree
    {
        static IFramework framework;
        static IPBSTreeSrv mm = null;

        public CPBSTree(IFramework fw)
        {
            if (framework == null)
                framework = fw;

            if (mm == null)
                mm = ConstMethod.GetService(typeof(IPBSTreeSrv)) as IPBSTreeSrv;
        }

        public void Start()
        {
            string viewName = "施工部位划分维护";
            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
            if (!ConstObject.FrameWorkNewFlag)
            {
                IMainView mv = framework.GetMainView(viewName);
                if (mv != null)
                    mv.ViewShow();
                else
                {
                    MPBSTree mmc = new MPBSTree();
                    if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
                    {
                        VPBSBuilding vmc = new VPBSBuilding(mmc);
                        vmc.ViewCaption = viewName;
                        framework.AddMainView(vmc);
                        vmc.SelectPBSTemplate();
                    }
                    else
                    {
                        VPBSTree vmc = new VPBSTree(mmc);
                        vmc.ViewCaption = viewName;
                        framework.AddMainView(vmc);
                        vmc.Start();
                    }
                    
                }
            }
            else
            {
                MPBSTree mmc = new MPBSTree();
                VPBSTree vmc = new VPBSTree(mmc);
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
                OperationPBSTreeType executeType = (OperationPBSTreeType)o;
                string viewName = "";
                IMainView mv = null;
                switch (executeType)
                {
                    case OperationPBSTreeType.施工部位划分维护:
                        viewName = "施工部位划分维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MPBSTree mmc = new MPBSTree();
                            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                            if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
                            {
                                VPBSBuilding vmc = new VPBSBuilding(mmc);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                                vmc.SelectPBSTemplate();
                            }
                            else
                            {
                                VPBSTreeBakNew vmc = new VPBSTreeBakNew(mmc);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                        }
                        break;
                    case OperationPBSTreeType.施工部位模板维护:
                        viewName = "施工部位模板维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MPBSTree mmc = new MPBSTree();
                            VPBSTemplate vmc = new VPBSTemplate(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationPBSTreeType.施工部位结构类型维护:
                        viewName = "施工部位结构类型维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MPBSTree mmc = new MPBSTree();
                            VTemplateType vmc = new VTemplateType(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationPBSTreeType.特性集维护:
                        viewName = "特性集维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VFeatureSet vmc = new VFeatureSet();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationPBSTreeType.PBS节点维护:
                        viewName = "PBS节点维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MPBSTree mmc = new MPBSTree();
                            VPBSBuilding vmc = new VPBSBuilding(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                            vmc.SelectPBSTemplate();
                        }
                        break;
                }
            }

            return null;
        }
    }
}
