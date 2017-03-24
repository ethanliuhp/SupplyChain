using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.ResourceManager.Client.Main;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI
{
    public enum CAppSolutionSet_ExecType
    {
        /// <summary>
        /// ������������
        /// </summary>
        AppSolutionSet,
        /// <summary>
        /// �������Զ���
        /// </summary>
        AppPropertySet
    }

    public class CAppSolutionSet
    {
        private static VirtualMachine.Component.WinMVC.generic.IFramework framework;
        public CAppSolutionSet(VirtualMachine.Component.WinMVC.generic.IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length != 0)
            {
                object obj = args[1];
                if (obj != null && obj.GetType() == typeof(CAppSolutionSet_ExecType))
                {
                    CAppSolutionSet_ExecType execType = (CAppSolutionSet_ExecType)obj;

                    switch (execType)
                    {
                        case CAppSolutionSet_ExecType.AppSolutionSet:
                            IMainView mv = framework.GetMainView("������������");
                            if (mv != null)
                            {
                                //�����ǰ��ͼ�Ѿ����ڣ�ֱ����ʾ
                                mv.ViewShow();
                                return null;
                            }
                            VAppSolutionSet theVAppSolutionSet = new VAppSolutionSet();
                            theVAppSolutionSet.ViewCaption = "������������";
                            framework.AddMainView(theVAppSolutionSet);
                            return null;

                        case CAppSolutionSet_ExecType.AppPropertySet:
                            IMainView mainView1 = framework.GetMainView("�������Զ���");
                            if (mainView1 != null)
                            {
                                //�����ǰ��ͼ�Ѿ����ڣ�ֱ����ʾ
                                mainView1.ViewShow();
                                return null;
                            }
                            VAppPropertySet theVAppPropertySet = new VAppPropertySet();
                            theVAppPropertySet.ViewCaption = "�������Զ���";
                            framework.AddMainView(theVAppPropertySet);
                            return null;
                    }
                }
            }
            else
            {
                object o = args[0];
            }
            return null;

        }
    }
}

