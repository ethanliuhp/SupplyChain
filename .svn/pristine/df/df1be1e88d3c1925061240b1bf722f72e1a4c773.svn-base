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
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Service
{
    public interface IPBSTreeSrv
    {
        bool DeletePBSTree(PBSTree pbs);
        bool DeletePBSTree(IList lst);
        System.Collections.IList GetALLChildNodes(PBSTree pbs);
        PBSTree GetPBSTreeById(string id);
        System.Collections.IList GetPBSTrees(Type t);
        System.Collections.IList InvalidatePBSTree(PBSTree pbs);
        Hashtable MovePBSTree(PBSTree pbs, PBSTree toOrg);
        VirtualMachine.Patterns.CategoryTreePattern.Service.ICategoryNodeService NodeSrv { get; set; }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        IList SavePBSTreeRootNode(IList list);

        PBSTree SavePBSTree(PBSTree childOrg);
        IList GetPBSTreeRules(Type treeType);
        IList GetPBSTreeRules(Type treeType, Type parentNodeType);
        bool InitRule();

        IList GetPBSTrees(Type t, ObjectQuery oq);
        IList GetPBSTreeAndJobs(Type t);
        PBSTree GetPBSTree(PBSTree orgCat);
        IList GetPBSTreeAndJobs();
        IList SavePBSTrees(IList lst);
        IList PBSTreeOrder(IList list);
        PBSTree InsertOrUpdatePBSTree(PBSTree childOrg);
        IList InsertOrUpdatePBSTrees(IList lst);

        long GetMaxOrderNo(CategoryNode node);

        /// <summary>
        /// 获取PBS节点集合(返回有权限和无权限的集合)
        /// </summary>
        IList GetPBSTreesByInstance(string projectId);

        /// <summary>
        /// 获取PBS节点集合(返回有权限和无权限的集合)
        /// </summary>
        DataTable GetPBSTreesInstanceBySql(string projectId);

        /// <summary>
        /// 获取PBS节点集合
        /// </summary>
        IList GetPBSTreesByInstance(string projectId, IList listParentPBS);

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
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetCode(Type type);

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);


        /// <summary>
        /// 保存 修改 特性集 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        IList SaveOrUpdateFeatureSet(IList list);

        bool Delete(IList list);

        bool Delete(object obj);

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        bool DeleteElement(Elements element);

        object Save(object obj);

        IList Save(IList obj);

        PBSTree SavePBSTree(PBSTree pbs, string parentSysCode);

        void SaveBatchPBSTreeName(IList lstPBSTree);

        DataSet GetPBSTreesByInstanceSql(string projectId, string sParentID, string sName);

        DataSet GetCostBudget(string projectid, string sysCode);
    }
}
