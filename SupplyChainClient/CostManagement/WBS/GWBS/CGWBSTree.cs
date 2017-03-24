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
        ʩ�����񻮷�ά�� = 1,
        ���̳ɱ�ά�� = 2,
        ����WBS��ϸ�༭ = 3,
        ǩ֤���̨�˲�ѯ = 4,
        ��Ŀ�����ۺϲ�ѯ = 5,
        ���̳ɱ�ά���ۺϲ�ѯ = 6,
        ʩ�����񻮷�ά��_�� = 7,
        ���̳ɱ�����ά�� = 8
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
                    case OperationGWBSType.ʩ�����񻮷�ά��:
                        viewName = "ʩ�����񻮷�ά��";
                        mv = framework.GetMainView(viewName);
                        if (mv != null)
                        {
                            mv.ViewShow();
                        }
                        else
                        {
                            MGWBSTree mmc = new MGWBSTree();
                            CurrentProjectInfo projectInfo = Application.Business.Erp.SupplyChain.Client.Basic.CommonClass.StaticMethod.GetProjectInfo();
                            if (projectInfo.ProjectInfoState == EnumProjectInfoState.����Ŀ)
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
                    case OperationGWBSType.ʩ�����񻮷�ά��_��:
                        viewName = "ʩ�����񻮷�ά��_��";
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
                    case OperationGWBSType.���̳ɱ�ά��:
                        viewName = "���̳ɱ�ά��";
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
                    case OperationGWBSType.���̳ɱ�����ά��:
                        viewName = "���̳ɱ�����ά��";
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
                    case OperationGWBSType.����WBS��ϸ�༭:
                        viewName = "����WBS��ϸ�༭";
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
                    case OperationGWBSType.ǩ֤���̨�˲�ѯ:
                        viewName = "ǩ֤���̨�˲�ѯ";
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
                    case OperationGWBSType.��Ŀ�����ۺϲ�ѯ:
                        viewName = "��Ŀ�����ۺϲ�ѯ";
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
                    case OperationGWBSType.���̳ɱ�ά���ۺϲ�ѯ:
                        viewName = "���̳ɱ�ά���ۺϲ�ѯ";
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
