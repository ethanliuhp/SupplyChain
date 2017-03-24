using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using System;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppPlatformUI
{
    public class MAppStatusQuery
    {
        private static IAppSrv service = null;
        public MAppStatusQuery()
        {
            if (service == null)
            {
                service = StaticMethod.GetService("RefAppSrv") as IAppSrv;
            }
        }
        public IList Save(IList lst)
        {
            return service.Save(lst);
        }
        public IList GetObjects(Type t,ObjectQuery oq)
        {
            return service.GetObjects(t, oq);
        }
        public bool Delete(IList lst)
        {
            return service.Delete(lst);
        }

        public IList GetOpeOrgsByInstance()
        {
            return service.GetOpeOrgsByInstance();
        }

        public object GetDomain(Type t, ObjectQuery l)
        {
            return service.GetDomain(t, l);
        }
    }
}