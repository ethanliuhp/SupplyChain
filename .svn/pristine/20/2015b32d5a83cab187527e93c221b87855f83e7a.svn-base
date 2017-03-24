using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using System;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppSolutionSetUI
{
    public class MAppSolutionSet
    {
        private static IAppSrv service = null;
        public MAppSolutionSet()
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
      public   AppSolutionSet DeleteAppStep(IList lstDel, AppSolutionSet oAppSolutionSet)
        {
            return service.DeleteAppStep(lstDel, oAppSolutionSet);
        }
    }
}