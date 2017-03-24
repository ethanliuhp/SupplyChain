using System;
using System.Collections.Generic;
using System.Text;
using Application.Resource.FinancialResource.Service;
using Application.Resource.FinancialResource.RelateClass;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng
{
    class MVoucherType
    {
        private static IVouTypeComSrv VouTypeComSrv = null;
        public MVoucherType()
        {
            if (VouTypeComSrv == null)
            {
                VouTypeComSrv = StaticMethod.GetService("VouTypeComSrv") as IVouTypeComSrv;
            }
        }
        public VoucherTypeInfo Save(VoucherTypeInfo obj)
        {
            if (obj.Id == "")
                return VouTypeComSrv.Save(obj) as VoucherTypeInfo;
            else
                return VouTypeComSrv.Update(obj) as VoucherTypeInfo;
        }
        public bool Delete(VoucherTypeInfo obj)
        {
            return VouTypeComSrv.Delete(obj);
        }
        public IList getobj()
        {
            return VouTypeComSrv.GetObjects(typeof(VoucherTypeInfo));
        }

        public IList getVouType(ObjectQuery oq)
        {
            return VouTypeComSrv.GetObjects(typeof(VoucherTypeInfo), oq);
        }
    }
}
