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

namespace Application.Business.Erp.SupplyChain.Client.PMCAndWarning
{
    public enum OperationPMCAndWarningType
    {
        ״̬��鶯��ά�� = 1,
        Ԥ����Ϣ���ͷ�ʽ���� = 2,
        Ԥ���������̨ = 3,
        Ԥ����Ϣ��ѯ = 4
    }

    public class CPMCAndWarning
    {
        static IFramework framework;

        public CPMCAndWarning(IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length > 0)
            {
                object o = args[0];
                OperationPMCAndWarningType executeType = (OperationPMCAndWarningType)o;

                string viewName = "";
                IMainView mv = null;
                switch (executeType)
                {
                    case OperationPMCAndWarningType.״̬��鶯��ά��:
                        viewName = "״̬��鶯��ά��";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VImportCheckAction vmc = new VImportCheckAction();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationPMCAndWarningType.Ԥ����Ϣ���ͷ�ʽ����:
                        viewName = "Ԥ����Ϣ���ͷ�ʽ����";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VWarningPushModeMng vmc = new VWarningPushModeMng();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationPMCAndWarningType.Ԥ���������̨:
                        viewName = "Ԥ���������̨";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VWaringServerControl vmc = new VWaringServerControl();
                            vmc.ViewCaption = viewName;
                            framework.AddMainView(vmc);
                        }
                        break;
                    case OperationPMCAndWarningType.Ԥ����Ϣ��ѯ:
                        viewName = "Ԥ����Ϣ��ѯ";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            VWarningInfoMng vmc = new VWarningInfoMng();
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
