using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using System.Collections;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.MaterialResource.Service;

namespace Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Service
{
    public class ExpenseItemSrv : BaseService, Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Service.IExpenseItemSrv
    {

        public override object Update(BusinessEntity obj)
        {
            return base.Update(obj);
        }
        public override object Save(BusinessEntity obj)
        {
            return base.Save(obj);
        }
      
        public override System.Collections.IList GetDomainByCondition(Type objType, VirtualMachine.Core.ObjectQuery objectQuery)
        {
            IList list = null;
            list=base.GetDomainByCondition(objType, objectQuery);
            return list;
        }

        public System.Collections.IList GetDomain(Type objType, VirtualMachine.Core.ObjectQuery objectQuery)
        {
            return base.GetDomainByCondition(objType, objectQuery);
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="obj">要删除的对象</param>
        /// <returns>True 删除成功，False 删除失败</returns>
        virtual public bool DeleteItem(Object obj)
        {
            Dao.Delete(obj);
            return true;
        }

        virtual public bool InitDateExpItem(Object obj)
        {
            Dao.Delete(obj);
            return true;
        }

    }
}
