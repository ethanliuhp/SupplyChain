using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.ResourceManager.Client.Main;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppTableSetUI
{
    public class CAppTableSet
    {
        private static VirtualMachine.Component.WinMVC.generic.IFramework  framework;
        public CAppTableSet(VirtualMachine.Component.WinMVC.generic.IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length != 0)
            {
                IMainView mv = framework.GetMainView("����������");
                if (mv != null)
                {
                    //�����ǰ��ͼ�Ѿ����ڣ�ֱ����ʾ
                    mv.ViewShow();
                    return null;
                }
                VAppTableSet theVAppTableSet = new VAppTableSet();
                theVAppTableSet.ViewCaption = "����������";
                framework.AddMainView(theVAppTableSet);
                return null;
            }
            else
            {
                object o = args[0];
            }
            return null;
        
        }
    }
}

