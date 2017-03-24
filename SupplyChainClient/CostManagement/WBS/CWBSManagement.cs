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
        ʩ���������� = 1,
        ��Լ��ά�� = 2,
        �ļ��ϴ����� = 3,
        �ĵ�������� = 4,
        ��������ģ��ά�� = 5,
        ��ʼ����Ŀ�����ĵ�ģ�� = 6,
        �ֳ��������� = 7,
        �������ᱨ��ѯ = 8,
        ���ֳ��������� = 9,
        �ֳ���������_�ֳ�=10
        ,�������ռ�� = 11
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
                    case OperationWBSType.ʩ����������:
                        viewName = "ʩ����������";
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
                    case OperationWBSType.��Լ��ά��:
                        viewName = "��Լ��ά��";
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
                    case OperationWBSType.�ļ��ϴ�����:
                        viewName = "�ļ��ϴ�����";
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
                    case OperationWBSType.�ĵ��������:
                        viewName = "�ļ��������";
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
                    case OperationWBSType.��������ģ��ά��:
                        viewName = "��������ģ��ά��";
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
                    case OperationWBSType.��ʼ����Ŀ�����ĵ�ģ��:
                        viewName = "��ʼ����Ŀ�����ĵ�ģ��";
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
                    case OperationWBSType.�ֳ���������:
                        viewName = "�ֳ���������";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                            MGWBSTree mmc = new MGWBSTree();
                            if (projectInfo.ProjectInfoState == EnumProjectInfoState.����Ŀ)
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
                    case OperationWBSType.���ֳ���������:
                        viewName = "���ֳ���������";
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
                    case OperationWBSType.�������ռ��:
                        viewName = "�������ռ��";
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
                    case OperationWBSType.�ֳ���������_�ֳ�:
                        viewName = "�ֳ���������(�ֳ�)";
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
                    case OperationWBSType.�������ᱨ��ѯ:
                        viewName = "�������ᱨ��ѯ";
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
