using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ApprovalMng.AppSolutionMng.Service;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.AppMng.AppTableSetUI
{
    public class MAppTableSet
    {
        private static IAppSrv service = null;
        public MAppTableSet()
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
        public IList GetObjects(ObjectQuery oq)
        {
            return service.GetDomainByCondition(typeof(AppTableSet), oq);
        }
        public bool Delete(IList lst)
        {
            return service.Delete(lst);
        }
    }
}