using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Component.WinMVC.generic;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.GusetCompanyMng
{
    public enum GuestCompanyType
    {
        search
    }
    public class CGuestCompany
    {
        private static IFramework framework;
        public CGuestCompany(IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }
        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                VGuestCompanySearch theVGuestCompanySearch = new VGuestCompanySearch();
                theVGuestCompanySearch.ShowDialog();
            }
            else
            {
                GuestCompanyType theType = (GuestCompanyType)args[0];
                switch (theType)
                {
                    case GuestCompanyType.search:
                        VGuestCompanySearch theVGuestCompanySearch = new VGuestCompanySearch();
                        theVGuestCompanySearch.ShowDialog();
                        return theVGuestCompanySearch.Result;
                    default:
                        break;
                }
            }
            return null;
        }
    }
}
