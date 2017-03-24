using System;
using System.Collections;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core;
namespace Application.Business.Erp.SupplyChain.Base.Service
{
    public interface IBaseService
    {
        bool DeleteObject(object obj);
        bool DeleteByDao(object obj);
        //bool DeleteValide(object obj);
        System.Collections.IList FindAll(Type obj);
        object GetDomain(Type domain, string id, VirtualMachine.Core.ObjectQuery objectQuery);
        IList GetDomainByCondition(Type objType, VirtualMachine.Core.ObjectQuery objectQuery);
        IList GetDetailList(Type objType, ObjectQuery objectQuery);
        object Save(VirtualMachine.Patterns.BusinessEssence.Domain.BusinessEntity obj);
        object SaveByDao(object obj);
        System.Collections.IList SaveByDao(System.Collections.IList objList);
        System.Collections.IList SaveOrUpdateByDao(System.Collections.IList objList);
        //bool SaveValide(object obj);
        object Update(VirtualMachine.Patterns.BusinessEssence.Domain.BusinessEntity obj);
        object UpdateByDao(object obj);
        System.Collections.IList UpdateByDao(System.Collections.IList objList);
        //bool UpdateValide(object obj);
        object SaveOrUpdateByDao(object obj);
        bool DeleteByDao(IList obj);

        IList GetForwardMasterBills(LinkRule linkRule, ObjectQuery objectQuery, bool isEagerDetail);
        IList GetForwardMasterBills(LinkRule linkRule, ObjectQuery objectQuery, bool isEagerDetail, bool onlyExecBills);
        IList GetForwardDetailBills(LinkRule linkRule, ObjectQuery objectQuery, string masterID);
        IList GetForwardTypes(Type objType);
        string GetIdByCode(Type objType, string code);
        LinkRule GetLinkRule(Type type, string id);
        IList GetForwardDetailBills(LinkRule linkRule, ObjectQuery objectQuery);
        IList GetForwardDetailBills(LinkRule linkRule, ObjectQuery objectQuery, bool onlyExecBills);
        IList GetObjects(Type aType, ObjectQuery aObjectQuery);
    }
}
