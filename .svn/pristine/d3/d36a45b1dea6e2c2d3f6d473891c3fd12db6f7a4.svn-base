using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using VirtualMachine.Core;
using NHibernate.Criterion;

using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.FinancialResource;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemCategoryMng
{
    public class MCostItemCategory
    {
        private static ICostItemCategorySrv model = null;
        public ICostItemSrv modelCostItem = null;

        public MCostItemCategory()
        {
            if (model == null)
                model = ConstMethod.GetService("CostItemCategorySrv") as ICostItemCategorySrv;
            if (modelCostItem == null)
                modelCostItem = ConstMethod.GetService("CostItemSrv") as ICostItemSrv;
        }

        //返回节点node所在树的当前层次的最大OrderNo
        public long GetMaxOrderNo(CategoryNode node)
        {
            return model.GetMaxOrderNo(node, null);
        }

        //保存组织集合
        public IList SaveCostItemCategorys(IList lst)
        {
            return model.SaveCostItemCategorys(lst);
        }

        public IDictionary MoveNode(CostItemCategory fromNode, CostItemCategory toNode)
        {
            CostItemCategory cat = model.MoveCostItemCategory(fromNode, toNode);
            IDictionary dic = new Hashtable();
            dic.Add(cat.ParentNode.Id.ToString(), cat.ParentNode);
            dic.Add(cat.Id.ToString(), cat);
            IList lstChildNodes = model.GetALLChildNodes(cat);
            foreach (CostItemCategory var in lstChildNodes)
            {
                dic.Add(var.Id.ToString(), var);
            }
            return dic;
        }
        /// <summary>
        /// 根据名称查找成本项分类
        /// </summary>
        public CostItemCategory GetOpeOrgByName(string name)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("Name", "%" + name + "%"));
            IList list = model.GetCostItemCategorys(typeof(CostItemCategory), oq);
            if (list.Count == 1)
                return list[0] as CostItemCategory;
            else
                return null;
        }

        /// <summary>
        /// 获取成本项分类节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetCostItemCategoryByInstance()
        {
            return model.GetCostItemCategoryByInstance();
        }

        public IList GetCostItemCategorys(Type t)
        {
            return model.GetCostItemCategorys(t);
        }
        public IList GetCostItemCategoryAndJobs(Type t)
        {
            return model.GetCostItemCategoryAndJobs(t);
        }


        public CostItemCategory GetCostItemCategoryById(string id)
        {
            //CostItemCategory org = mm.GetCostItemCategoryById(id);
            //return org;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = model.GetCostItemCategorys(typeof(CostItemCategory), oq);
            if (list.Count == 1)
                return list[0] as CostItemCategory;
            else
                return null;
        }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        public IList SaveCostItemCategoryRootNode(IList lst)
        {
            return model.SaveCostItemCategoryRootNode(lst);
        }

        public CostItemCategory SaveCostItemCategory(CostItemCategory childOrg)
        {
            return model.SaveCostItemCategory(childOrg);
        }

        public bool DeleteCostItemCategory(CostItemCategory childOrg)
        {
            return model.DeleteCostItemCategory(childOrg);
        }

        public bool DeleteCostItemCategory(IList lst)
        {
            return model.DeleteCostItemCategory(lst);
        }

        public bool InitRule()
        {
            return model.InitRule();
        }


        public IList GetCostItemCategoryRules(Type treeType)
        {
            return model.GetCostItemCategoryRules(treeType);
        }


        public IList GetCostItemCategoryRules(Type treeType, Type parentNodeType)
        {
            return model.GetCostItemCategoryRules(treeType, parentNodeType);
        }


        public string GetNextOPGCode()
        {
            return model.GetNextOPGCode();
        }
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return modelCostItem.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 保存成本项分类
        /// </summary>
        /// <param name="cateList"></param>
        /// <returns></returns>
        public bool SaveCostItemCate(List<CostItemCategory> cateList)
        {
            return model.SaveCostItemCate(cateList);
        }

        /// <summary>
        /// 保存成本项分类
        /// </summary>
        /// <param name="cateList"></param>
        /// <param name="defaultCate"></param>
        /// <returns></returns>
        public bool SaveCostItemCate(List<CostItemCategory> cateList, CostItemCategory defaultCate)
        {
            return model.SaveCostItemCate(cateList, defaultCate);
        }
    }
}
