using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Service
{
    public interface ICostItemCategorySrv
    {
        bool DeleteCostItemCategory(CostItemCategory pbs);
        bool DeleteCostItemCategory(IList lst);
        System.Collections.IList GetALLChildNodes(CostItemCategory pbs);
        CostItemCategory GetCostItemCategoryById(string id);
        System.Collections.IList GetCostItemCategorys(Type t);
        System.Collections.IList InvalidateCostItemCategory(CostItemCategory pbs);
        CostItemCategory MoveCostItemCategory(CostItemCategory pbs, CostItemCategory toOrg);
        VirtualMachine.Patterns.CategoryTreePattern.Service.ICategoryNodeService NodeSrv { get; set; }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        IList SaveCostItemCategoryRootNode(IList list);
        CostItemCategory SaveCostItemCategory(CostItemCategory childOrg);
        IList GetCostItemCategoryRules(Type treeType);
        IList GetCostItemCategoryRules(Type treeType, Type parentNodeType);
        bool InitRule();

        IList GetCostItemCategorys(Type t, ObjectQuery oq);
        IList GetCostItemCategoryAndJobs(Type t);
        CostItemCategory GetCostItemCategory(CostItemCategory orgCat);
        IList GetCostItemCategoryAndJobs();
        IList SaveCostItemCategorys(IList lst);
        long GetMaxOrderNo(CategoryNode node, ObjectQuery oq);
        /// <summary>
        /// 获取业务组织节点集合(返回有权限和无权限的集合)
        /// </summary>
        IList GetCostItemCategoryByInstance();
        /// <summary>
        /// 获取业务组织及岗位节点集合(返回有权限和无权限的集合)
        /// </summary>
        IList GetOpeOrgAndJobsByInstance();
        CategoryTree InitTree(string treeName, Type treeType);
        CategoryTree InitTree(string treeName, Type treeType, StandardPerson aPerson);
        int SaveSQL(string sql);
        DataSet SearchSQL(string sql);

        string GetNextOPGCode();

        /// <summary>
        /// 保存成本项分类
        /// </summary>
        /// <param name="cateList"></param>
        /// <returns></returns>
        bool SaveCostItemCate(List<CostItemCategory> cateList);

        /// <summary>
        /// 保存成本项分类
        /// </summary>
        /// <param name="cateList"></param>
        /// <param name="defaultCate"></param>
        /// <returns></returns>
        bool SaveCostItemCate(List<CostItemCategory> cateList, CostItemCategory defaultCate);

    }
}
