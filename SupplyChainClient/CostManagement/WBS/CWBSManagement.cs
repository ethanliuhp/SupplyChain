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
using Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.GWBS;
using Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS
{
    public enum OperationWBSType
    {
        施工任务类型 = 1,
        契约组维护 = 2,
        文件上传测试 = 3,
        文档对象管理 = 4,
        任务类型模版维护 = 5,
        初始化项目任务文档模板 = 6,
        现场生产管理 = 7,
        工程量提报查询 = 8,
        新现场生产管理 = 9,
        现场生产管理_现场=10
        ,质量验收检查 = 11
    }

    public class CWBSManagement
    {
        static IFramework framework;

        public CWBSManagement(IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length > 0)
            {
                object o = args[0];
                OperationWBSType executeType = (OperationWBSType)o;

                string viewName = "";
                IMainView mv = null;
                switch (executeType)
                {
                    case OperationWBSType.施工任务类型:
                        viewName = "施工任务类型";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MWBSManagement mmc = new MWBSManagement();
                            VWBSProjectTaskTypeTree vmc = new VWBSProjectTaskTypeTree(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationWBSType.契约组维护:
                        viewName = "契约组维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MWBSManagement mmc = new MWBSManagement();
                            VWBSProjectTaskTypeTree vmc = new VWBSProjectTaskTypeTree(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationWBSType.文件上传测试:
                        viewName = "文件上传测试";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VFileUpload vmc = new VFileUpload();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationWBSType.文档对象管理:
                        viewName = "文件对象管理";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MWBSManagement mmc = new MWBSManagement();
                            VDocumentManage vmc = new VDocumentManage(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationWBSType.任务类型模版维护:
                        viewName = "任务类型模版维护";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            //MWBSManagement mmc = new MWBSManagement();
                            VTemplateImport vmc = new VTemplateImport();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationWBSType.初始化项目任务文档模板:
                        viewName = "初始化项目任务文档模板";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VInitGWBSDocumentTemplate vmc = new VInitGWBSDocumentTemplate();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationWBSType.现场生产管理:
                        viewName = "现场生产管理";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                            MGWBSTree mmc = new MGWBSTree();
                            if (projectInfo.ProjectInfoState == EnumProjectInfoState.新项目)
                            {
                                VProductManageChangeNew vmc = new VProductManageChangeNew(mmc);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                            else
                            {
                                VProductManageChange vmc = new VProductManageChange(mmc);
                                vmc.ViewCaption = viewName;
                                framework.AddMainView(vmc);
                            }
                        }
                        break;
                    case OperationWBSType.新现场生产管理:
                        viewName = "新现场生产管理";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MGWBSTree mmc = new MGWBSTree();
                            VProductManageChangeNew vmc = new VProductManageChangeNew(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationWBSType.质量验收检查:
                        viewName = "质量验收检查";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MGWBSTree mmc = new MGWBSTree();
                            VQualityAcceptanceCheck vmc = new VQualityAcceptanceCheck(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationWBSType.现场生产管理_现场:
                        viewName = "现场生产管理(现场)";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MGWBSTree mmc = new MGWBSTree();
                            VProductManageChangeNow vmc = new VProductManageChangeNow(mmc);
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationWBSType.工程量提报查询:
                        viewName = "工程量提报查询";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VProductManagementQuery vmc = new VProductManagementQuery();
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
