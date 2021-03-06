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
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
using System.Data;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.PBS
{
    public class MPBSTree
    {
        private static IPBSTreeSrv model;
        private static IProObjectRelaDocumentSrv modelDoc;
        #region 创建这个属性，免得调用多这一层太麻烦

        public IPBSTreeSrv Service { get; set; }
        #endregion
        public MPBSTree()
        {
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            Service = model;
        }

        //返回节点node所在树的当前层次的最大OrderNo
        public long GetMaxOrderNo(CategoryNode node)
        {
            return model.GetMaxOrderNo(node);
        }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        public IList SavePBSTreeRootNode(IList list)
        {
            return model.SavePBSTreeRootNode(list);
        }

        /// <summary>
        /// 保存PBS树节点集合
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public IList SavePBSTrees(IList lst)
        {
            return model.SavePBSTrees(lst);
        }
        public IList PBSTreeOrder(IList list)
        {
            return model.PBSTreeOrder(list);
        }
        /// <summary>
        /// 插入（部分属性固定）或修改PBS树节点
        /// </summary>
        /// <param name="childNode">树节点</param>
        /// <returns></returns>
        public PBSTree InsertOrUpdatePBSTree(PBSTree childNode)
        {
            return model.InsertOrUpdatePBSTree(childNode);
        }

        /// <summary> 
        /// 插入（部分属性固定）或修改PBS节点集合
        /// </summary>
        public IList InsertOrUpdatePBSTrees(IList lst)
        {
            return model.InsertOrUpdatePBSTrees(lst);
        }

        public Hashtable MoveNode(PBSTree fromNode, PBSTree toNode)
        {
            return model.MovePBSTree(fromNode, toNode);
        }

        /// <summary>
        /// 根据名称查找业务组织
        /// </summary>
        public PBSTree GetOpeOrgByName(string name)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Like("Name", "%" + name + "%"));
            IList list = model.GetPBSTrees(typeof(PBSTree), oq);
            if (list.Count == 1)
                return list[0] as PBSTree;
            else
                return null;
        }

        /// <summary>
        /// 获取业务组织节点集合(返回有权限和无权限的集合)
        /// </summary>
        public IList GetPBSTreesByInstance(string projectId)
        {
            return model.GetPBSTreesByInstance(projectId);
        }

        /// <summary>
        /// 获取PBS节点集合(返回有权限和无权限的集合)
        /// </summary>
        public DataTable GetPBSTreesInstanceBySql(string projectId)
        {
            return model.GetPBSTreesInstanceBySql(projectId);
        }

        /// <summary>
        /// 获取PBS节点集合
        /// </summary>
        public IList GetPBSTreesByInstance(string projectId, IList listParentPBS)
        {
            return model.GetPBSTreesByInstance(projectId, listParentPBS);
        }

        public IList GetPBSTrees(Type t)
        {
            return model.GetPBSTrees(t);
        }
        public IList GetPBSTreeAndJobs(Type t)
        {
            return model.GetPBSTreeAndJobs(t);
        }


        public PBSTree GetPBSTreeById(string id)
        {
            //PBSTree org = mm.GetPBSTreeById(id);
            //return org;
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", id));
            IList list = model.GetPBSTrees(typeof(PBSTree), oq);
            if (list.Count == 1)
                return list[0] as PBSTree;
            else
                return null;
        }


        public PBSTree SavePBSTree(PBSTree childOrg)
        {
            return model.SavePBSTree(childOrg);
        }

        public bool DeletePBSTree(PBSTree childOrg)
        {
            return model.DeletePBSTree(childOrg);
        }

        public bool DeletePBSTree(IList lst)
        {
            return model.DeletePBSTree(lst);
        }

        public bool InitRule()
        {
            return model.InitRule();
        }


        public IList GetPBSTreeRules(Type treeType)
        {
            return model.GetPBSTreeRules(treeType);
        }


        public IList GetPBSTreeRules(Type treeType, Type parentNodeType)
        {
            return model.GetPBSTreeRules(treeType, parentNodeType);
        }


        public string GetNextOPGCode()
        {
            return model.GetNextOPGCode();
        }

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type)
        {
            return model.GetCode(type);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return model.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 保存或修改对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        public IList SaveOrUpdate(IList list)
        {
            return modelDoc.SaveOrUpdate(list);
        }

        /// <summary>
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
        /// <returns></returns>
        public ProObjectRelaDocument SaveOrUpdateProObjRelaDoc(ProObjectRelaDocument item)
        {
            return modelDoc.SaveOrUpdate(item);
        }

        /// <summary>
        /// 删除工程对象关联文档对象集合
        /// </summary>
        /// <param name="list">工程对象关联文档对象集合</param>
        /// <returns></returns>
        public bool DeleteProObjRelaDoc(IList list)
        {
            return modelDoc.DeleteProObjRelaDoc(list);
        }


        /// <summary>
        /// 保存 修改 特性集 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList SaveOrUpdateFeatureSet(IList list)
        {
            return model.SaveOrUpdateFeatureSet(list);
        }

        public bool Delete(IList list)
        {
            return model.Delete(list);
        }
        public bool Delete(object obj)
        {
            return model.Delete(obj);
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public bool DeleteElement(Elements element)
        {
            return model.DeleteElement(element);
        }

        public void SaveBatchPBSTreeName(IList lstPBSTree)
        {
            model.SaveBatchPBSTreeName(lstPBSTree);
        }
        public DataSet GetPBSTreesByInstanceSql(string projectId, string sParentID, string sName)
        {
            return model.GetPBSTreesByInstanceSql(projectId, sParentID, sName);
        }

        #region 基础设施修改

        public bool IsLeafNode(PBSTree pbs)
        {
            var children = model.GetALLChildNodes(pbs);
            return children.Count <= 0;
        }
        #endregion
    }
}
