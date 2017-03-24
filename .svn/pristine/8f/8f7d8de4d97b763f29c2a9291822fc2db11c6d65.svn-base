using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng
{
    public enum VoucherTypeExcType
    {
        search
    }
    class CVoucherType
    {
        private static VirtualMachine.Component.WinMVC.generic.IFramework framework;
        public CVoucherType(VirtualMachine.Component.WinMVC.generic.IFramework fw)
        {
            if (framework == null)
                framework = fw;
        }

        public object Excute(params object[] args)
        {
            if (args.Length == 0)
            {
                VVoucherType theVVoucherType = new VVoucherType();
                theVVoucherType.ShowDialog();
            }
            else
            {
                VoucherTypeExcType theVoucherTypeExcType = (VoucherTypeExcType)args[0];
                switch (theVoucherTypeExcType)
                {
                    case VoucherTypeExcType.search:
                        VVoucherTypeSearch theVVoucherTypeSearch = new VVoucherTypeSearch();
                        theVVoucherTypeSearch.ShowDialog();
                        //return theVVoucherTypeSearch.Result;
                        break;
                    default:
                        break;
                }
            }
            return null;
        }
    }
}
