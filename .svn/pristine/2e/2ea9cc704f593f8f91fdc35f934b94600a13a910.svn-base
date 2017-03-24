using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.ResourceManager.Client.ResourceUI.PersonAndOrganization.HumanResource;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.PersonInforMng
{
    public enum PersonInforType
    {
        search
    }
    public class CPersonInfor
    {
        private static VirtualMachine.Component.WinMVC.generic.IFramework framework;
        public CPersonInfor(VirtualMachine.Component.WinMVC.generic.IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                VPersonSearch theVPersonSearch = new VPersonSearch();
                theVPersonSearch.ShowDialog();
            }
            else
            {
                PersonInforType thePIT = (PersonInforType)args[0];
                switch (thePIT)
                {
                    case PersonInforType.search:
                        VPersonSearch theVPersonSearch = new VPersonSearch();
                        theVPersonSearch.ShowDialog();
                        return theVPersonSearch.Result;
                    default:
                        break;
                }
            }
            return null;
        }
    }
}
