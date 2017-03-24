using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.FinanceMng.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;

namespace Application.Business.Erp.SupplyChain.Client.MoneyManage.IndirectCost
{
   public  class MIndirectCost
    {
       private static IIndirectCostSvr indirectCostSvr;
       private static  IAccountTitleTreeSvr accountTitleTreeSvr;
       private static IOperationOrgService operationOrgService;
       public  IIndirectCostSvr IndirectCostSvr
       {
           get
           {
               if (indirectCostSvr == null)
               {
                   indirectCostSvr = StaticMethod.GetService("IndirectCostSvr") as IIndirectCostSvr;
               }
               return indirectCostSvr;
           }
       }
       /// <summary>
       /// 会计科目
       /// </summary>
       public IAccountTitleTreeSvr AccountTitleTreeSvr
       {
           get
           {
               if (accountTitleTreeSvr == null)
               {
                   accountTitleTreeSvr = StaticMethod.GetService("AccountTitleTreeSvr") as IAccountTitleTreeSvr;
               }
               return accountTitleTreeSvr;
           }
       }
       public IOperationOrgService OperationOrgService
       {
           get
           {
               if (operationOrgService == null)
               {
                   operationOrgService = StaticMethod.GetService("OperationOrgService") as IOperationOrgService;
               }
               return operationOrgService;
           }
       }
    }
}
