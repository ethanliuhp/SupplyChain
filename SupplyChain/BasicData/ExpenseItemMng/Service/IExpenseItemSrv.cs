using System;
using System.Collections;
using Application.Business.Erp.SupplyChain.Base.Service;
namespace Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Service
{
   public interface IExpenseItemSrv :IBaseService
    {
        bool DeleteItem(object obj);
        System.Collections.IList GetDomain(Type objType, VirtualMachine.Core.ObjectQuery objectQuery);
        System.Collections.IList GetDomainByCondition(Type objType, VirtualMachine.Core.ObjectQuery objectQuery);
        bool InitDateExpItem(object obj);
        object Save(VirtualMachine.Patterns.BusinessEssence.Domain.BusinessEntity obj);
        object Update(VirtualMachine.Patterns.BusinessEssence.Domain.BusinessEntity obj);
    }
}
