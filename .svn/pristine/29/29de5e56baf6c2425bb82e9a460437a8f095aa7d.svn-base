using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Core;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Service
{
   public  interface IAccountTitleTreeSvr
    {
       AccountTitleTree GetAccountTitleTreeById(string id);
      IList GetALLChildNodes(AccountTitleTree pbs);
      IList GetCostAccountSubjects(Type t, ObjectQuery oq);
      IList GetOpeOrgAndJobsByInstance();

      IList GetAccountTitleTreeByQuery(ObjectQuery oQuery);
      IList GetAccountTitleTreeAndJobs();
      IList GetAccountTitleTreeAndJobs(Type t);
      AccountTitleTree GetAccountTitleTree(AccountTitleTree orgCat);
      long GetMaxOrderNo(CategoryNode node, ObjectQuery oq);
      IList GetAccountTitleTreeByInstance();
      IList GetAccountTitleTreeByInstance(string sRootCode);
      IList GetAccountTitleTrees(Type t);
      CategoryTree InitTree(string treeName, Type treeType, StandardPerson aPerson);
      CategoryTree InitTree(string treeName, Type treeType);
      IList InvalidateAccountTitleTree(AccountTitleTree pbs);
      AccountTitleTree MoveAccountTitleTree(AccountTitleTree pbs, AccountTitleTree toPbs);
      bool DeleteAccountTitleTree(IList list);
      bool DeleteAccountTitleTree(AccountTitleTree pbs);
      IList SaveCostAccountSubjects(IList lst);
      AccountTitleTree SaveAccountTitleTree(AccountTitleTree childNode);
      IList SaveAccountTitleTreeRootNode(IList lst);
      IList SaveAccountTitleTrees(IList lst);
      string GetCode(Type type);
      IDictionary MoveNode(AccountTitleTree fromNode, AccountTitleTree toNode);
       /// <summary>
       /// 获取财务账面维护中的四大基本会计科目(费用类别)
       /// </summary>
       /// <returns></returns>
      Hashtable GetBasicAccountTitleTree();
    }
}
