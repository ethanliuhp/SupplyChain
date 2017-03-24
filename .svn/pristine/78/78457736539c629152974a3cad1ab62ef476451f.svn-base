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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    public interface IProjectTaskTypeSrv
    {
        bool DeleteProjectTaskTypeTree(ProjectTaskTypeTree pbs);
        bool DeleteProjectTaskTypeTree(IList lst);
        System.Collections.IList GetALLChildNodes(ProjectTaskTypeTree pbs);
        ProjectTaskTypeTree GetProjectTaskTypeTreeById(string id);
        System.Collections.IList GetProjectTaskTypeTrees(Type t);
        System.Collections.IList InvalidateProjectTaskTypeTree(ProjectTaskTypeTree pbs);
        ProjectTaskTypeTree MoveProjectTaskTypeTree(ProjectTaskTypeTree pbs, ProjectTaskTypeTree toOrg);
        VirtualMachine.Patterns.CategoryTreePattern.Service.ICategoryNodeService NodeSrv { get; set; }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        IList SaveProjectTaskTypeRootNode(IList list);
        ProjectTaskTypeTree SaveProjectTaskTypeTree(ProjectTaskTypeTree childOrg);
        IList GetProjectTaskTypeTreeRules(Type treeType);
        IList GetProjectTaskTypeTreeRules(Type treeType, Type parentNodeType);
        bool InitRule();

        IList GetProjectTaskTypeTrees(Type t, ObjectQuery oq);
        IList GetProjectTaskTypeTreeAndJobs(Type t);
        ProjectTaskTypeTree GetProjectTaskTypeTree(ProjectTaskTypeTree orgCat);
        IList GetProjectTaskTypeTreeAndJobs();
        IList SaveProjectTaskTypeTrees(IList lst);

        ProjectTaskTypeTree InsertOrUpdateTaskTypeTree(ProjectTaskTypeTree childNode);
        IList InsertOrUpdateTaskTypeTrees(IList lst);

        long GetMaxOrderNo(CategoryNode node);
        /// <summary>
        /// 获取工程任务类型节点集合(返回有权限和无权限的集合)
        /// </summary>
        IList GetProjectTaskTypeByInstance();
        /// <summary>
        /// 获取工程任务类型节点集合
        /// </summary>
        IList GetProjectTaskTypeByInstance(IList listParentTaskType);
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
        /// 发布本节点及子节点
        /// </summary>
        /// <param name="sNodeSysCode"></param>
        /// <returns></returns>
        IList PublisthTaskNodeAndChilds(string sNodeSysCode);
       /// <summary>
       /// 作废本节点及子节点
       /// </summary>
       /// <param name="sNodeSysCode"></param>
       /// <returns></returns>
        IList InvalidTaskNodeAndChilds(string sNodeSysCode);

        #region  工程任务类型文档模版
        /// <summary>
        /// 保存或修改工程任务类型文档模版
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ProTaskTypeDocumentStencil SaveOrUpDateDocStencil(ProTaskTypeDocumentStencil obj);
        /// <summary>
        /// 批量保存工程任务类型文档模版
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        IList SaveDaoStencilList(IList objs);
        /// <summary>
        /// 删除工程任务类型文档模版
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        bool DeleteDocStencilList(IList objs);

        #endregion

        bool UpdateProjectTaskTypeTreeOrderNO(List<ProjectTaskTypeTree> ilst);
    }
}
