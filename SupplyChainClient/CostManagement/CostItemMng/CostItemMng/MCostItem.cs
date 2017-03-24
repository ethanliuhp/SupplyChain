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
using System.Data;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.CostItemMng.CostItemMng
{
    public class MCostItem
    {
        private static ICostItemSrv modelCostItem;
        private static ICostItemCategorySrv modelCostItemCate;

        public MCostItem()
        {
            if (modelCostItem == null)
                modelCostItem = ConstMethod.GetService("CostItemSrv") as ICostItemSrv;

            if (modelCostItemCate == null)
                modelCostItemCate = ConstMethod.GetService("CostItemCategorySrv") as ICostItemCategorySrv;
        }

        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns></returns>
        public string GetCostItemCode()
        {
            return modelCostItem.GetCostItemCode();
        }

        /// <summary>
        /// 获取成本项明细编号
        /// </summary>
        /// <returns></returns>
        public string GetCostItemDetailCode(string CostItemCode, int detailNum)
        {
            return modelCostItem.GetCostItemDetailCode(CostItemCode, detailNum);
        }

        /// <summary>
        /// 保存或修改成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateCostItem(IList list)
        {
            return modelCostItem.SaveOrUpdateCostItem(list);
        }
        /// <summary>
        /// 保存成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        public IList Save(IList list)
        {
            return modelCostItem.Save(list);
        }
        /// <summary>
        /// 保存或修改成本项
        /// </summary>
        /// <param name="item">成本项</param>
        /// <returns></returns>
        public CostItem SaveOrUpdateCostItem(CostItem item)
        {
            return modelCostItem.SaveOrUpdateCostItem(item);
        }

        /// <summary>
        /// 删除成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        public bool DeleteCostItem(IList list)
        {
            return modelCostItem.DeleteCostItem(list);
        }

        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return modelCostItem.GetObjectById(entityType, Id);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return modelCostItem.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return modelCostItem.GetServerTime();
        }


        #region 成本项分类

        //返回节点node所在树的当前层次的最大OrderNo
        public long GetMaxOrderNo(CategoryNode node)
        {
            return modelCostItemCate.GetMaxOrderNo(node, null);
        }

        public DataSet SearchSQL(string sql)
        {
            return modelCostItemCate.SearchSQL(sql);
        }

        //保存组织集合
        public IList SaveCostItemCategorys(IList lst)
        {
            return modelCostItemCate.SaveCostItemCategorys(lst);
        }

        public IDictionary MoveNode(CostItemCategory fromNode, CostItemCategory toNode)
        {
            CostItemCategory cat = modelCostItemCate.MoveCostItemCategory(fromNode, toNode);
            IDictionary dic = new Hashtable();
            dic.Add(cat.ParentNode.Id.ToString(), cat.ParentNode);
            dic.Add(cat.Id.ToString(), cat);
            IList lstChildNodes = modelCostItemCate.GetALLChildNodes(cat);
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
            IList list = modelCostItemCate.GetCostItemCategorys(typeof(CostItemCategory), oq);
            if (list.Count == 1)
                return list[0] as CostItemCategory;
            else
                return null;
        }

        /// <summary>
        /// 获取成本项分类节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetCostItemCategoryByInstance(string projectId)
        {
            return modelCostItemCate.GetCostItemCategoryByInstance();
        }

        public IList GetCostItemCategorys(Type t)
        {
            return modelCostItemCate.GetCostItemCategorys(t);
        }
        public IList GetCostItemCategoryAndJobs(Type t)
        {
            return modelCostItemCate.GetCostItemCategoryAndJobs(t);
        }


        public CostItemCategory GetCostItemCategoryById(string id)
        {
            //CostItemCategory org = modelCostItemCate.GetCostItemCategoryById(id);
            //return org;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = modelCostItemCate.GetCostItemCategorys(typeof(CostItemCategory), oq);
            if (list.Count == 1)
                return list[0] as CostItemCategory;
            else
                return null;
        }


        public CostItemCategory SaveCostItemCategory(CostItemCategory childOrg)
        {
            return modelCostItemCate.SaveCostItemCategory(childOrg);
        }

        public bool DeleteCostItemCategory(CostItemCategory childOrg)
        {
            return modelCostItemCate.DeleteCostItemCategory(childOrg);
        }

        public bool DeleteCostItemCategory(IList lst)
        {
            return modelCostItemCate.DeleteCostItemCategory(lst);
        }

        public bool InitRule()
        {
            return modelCostItemCate.InitRule();
        }


        public IList GetCostItemCategoryRules(Type treeType)
        {
            return modelCostItemCate.GetCostItemCategoryRules(treeType);
        }


        public IList GetCostItemCategoryRules(Type treeType, Type parentNodeType)
        {
            return modelCostItemCate.GetCostItemCategoryRules(treeType, parentNodeType);
        }


        public string GetNextOPGCode()
        {
            return modelCostItemCate.GetNextOPGCode();
        }
        #endregion

        #region 成本定额

        /// <summary>
        /// 删除成本定额集合
        /// </summary>
        /// <param name="list">成本定额集合</param>
        /// <returns></returns>
        public bool DeleteCostItemQuota(IList list)
        {
            return modelCostItem.DeleteCostItemQuota(list);
        }

        /// <summary>
        /// 保存或修改成本定额
        /// </summary>
        /// <param name="list">成本定额集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateCostItemQuota(IList list)
        {
            return modelCostItem.SaveOrUpdateCostItemQuota(list);
        }

        #endregion
        
        #region 劳动力定额
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteCostWorkForce(IList list)
        {
            return modelCostItem.DeleteCostWorkForce(list);
        }

        public CostWorkForce SaveOrUpdateCostWorkForce(CostWorkForce obj)
        {
            return modelCostItem.SaveOrUpdateCostWorkForce(obj);
        }

        public IList SaveOrUpdateCostWorkForce(IList list)
        {
            return modelCostItem.SaveOrUpdateCostWorkForce(list);
        }
        #endregion
        /// <summary>
        /// 根据成本项分类Id获取最大成本项号
        /// </summary>
        /// <param name="cateId"></param>
        /// <returns></returns>
        public int GetMaxCostItemCodeByCate(string cateId)
        {
            return modelCostItem.GetMaxCostItemCodeByCate(cateId);
        }
    }
}
