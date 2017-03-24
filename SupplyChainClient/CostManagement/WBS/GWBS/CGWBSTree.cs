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
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS
{
    public enum OperationGWBSType
    {
        施工任务划分维护 = 1,
        工程成本维护 = 2,
        工程WBS明细编辑 = 3,
        签证变更台账查询 = 4,
        项目任务综合查询 = 5,
        工程成本维护综合查询 = 6,
        施工任务划分维护_新 = 7,
        工程成本批量维护 = 8
    }

    public class CWBSTree
    {
        static IFramework framework;

        public CWBSTree(IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length > 0)
            {
                object o = args[0];
                OperationGWBSType executeType = (OperationGWBSType)o;

                string viewName = "";
                IMainView mv = null;
                switch (executeType)
                {
                    case OperationGWBSType.施工任务划分维护:
                        viewName = "施工任务划分维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MGWBSTree mmc = new MGWBSTree();
                            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                            if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
                            {
                                VGWBSTree_New vmc = new VGWBSTree_New(mmc);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            else
                            {
                                VGWBSTree vmc = new VGWBSTree(mmc);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                        }
                        break;
                    case OperationGWBSType.施工任务划分维护_新:
                        viewName = "施工任务划分维护_新";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MGWBSTree mmc = new MGWBSTree();
                            VGWBSTree_New vmc = new VGWBSTree_New(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationGWBSType.工程成本维护:
                        viewName = "工程成本维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MGWBSTree mmc = new MGWBSTree();
                            VGWBSTreeDetail vmc = new VGWBSTreeDetail(mmc);
                            //VSelectGWBSValence vmc = new VSelectGWBSValence();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationGWBSType.工程成本批量维护:
                        viewName = "工程成本批量维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MGWBSTree mmc = new MGWBSTree();
                            VGWBSAddByBatch vmc = new VGWBSAddByBatch();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationGWBSType.工程WBS明细编辑:
                        viewName = "工程WBS明细编辑";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VGWBSTreeDetailEdit vmc = new VGWBSTreeDetailEdit();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationGWBSType.签证变更台账查询:
                        viewName = "签证变更台账查询";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VGWBSDetailLedgerQuery2 vmc = new VGWBSDetailLedgerQuery2();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationGWBSType.项目任务综合查询:
                        viewName = "项目任务综合查询";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VGWBSNodeIntegratedQuery vmc = new VGWBSNodeIntegratedQuery();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationGWBSType.工程成本维护综合查询:
                        viewName = "工程成本维护综合查询";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VSelectGWBSValence vmc = new VSelectGWBSValence();
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
