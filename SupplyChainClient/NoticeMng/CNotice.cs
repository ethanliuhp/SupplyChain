using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Client.NoticeMng
{

    public enum EdiNoticeExcType
    {
        search
    }
    public class CNotice
               
    {
        private static VirtualMachine.Component.WinMVC.generic.IFramework  framework;
        public CNotice(VirtualMachine.Component.WinMVC.generic.IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                VNotice theVEdiDept = new VNotice();
                theVEdiDept.ShowDialog();
            }
           
            return null;
        }
    }
}

