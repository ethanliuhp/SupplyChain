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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Core.Expression;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    public interface IGWBSTreeSrv
    {
        bool DeleteGWBSTree(GWBSTree pbs);
        bool DeleteGWBSTree(IList lst);
        System.Collections.IList GetALLChildNodes(GWBSTree pbs);
        GWBSTree GetGWBSTreeById(string id);
        System.Collections.IList GetGWBSTrees(Type t);
        System.Collections.IList InvalidateGWBSTree(GWBSTree pbs);
        GWBSTree MoveGWBSTree(GWBSTree pbs, GWBSTree toOrg);
        GWBSTree MoveGWBSTreeAndUpdateContract(GWBSTree pbs, GWBSTree toOrg, ContractGroup group);
        VirtualMachine.Patterns.CategoryTreePattern.Service.ICategoryNodeService NodeSrv { get; set; }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        IList SaveGWBSTreeRootNode(IList list);
        GWBSTree SaveGWBSTree(GWBSTree childOrg);

        IList GetGWBSTreeRules(Type treeType);
        IList GetGWBSTreeRules(Type treeType, Type parentNodeType);
        bool InitRule();

        IList GetGWBSTrees(Type t, ObjectQuery oq);
        IList GetGWBSTreeAndJobs(Type t);
        GWBSTree GetGWBSTree(GWBSTree orgCat);
        IList GetGWBSTreeAndJobs();
        IList SaveGWBSTrees(IList lst);

        GWBSTree InsertOrUpdateWBSTree(GWBSTree childNode);
        IList InsertOrUpdateWBSTrees(IList lst);

        long GetMaxOrderNo(CategoryNode node, ObjectQuery oq);
        /// <summary>
        /// 获取业务组织节点集合(返回有权限和无权限的集合)
        /// </summary>
        IList GetGWBSTreesByInstance(string projectId);
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
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        Object GetObjectById(Type entityType, string Id);

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        DateTime GetServerTime();

        /// <summary>
        /// 保存或更新GWBS,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns></returns>
        GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, IList<GWBSDetailLedger> listLedger);

        /// <summary>
        /// 保存或更新工程WBS集合,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点集合</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns></returns>
        IList SaveOrUpdateGWBSTree(IList list, IList<GWBSDetailLedger> listLedger);

        /// <summary>
        /// 保存或更新工程WBS集合,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点集合</param>
        /// <returns></returns>
        IList SaveOrUpdateGWBSTree(IList list);

        /// <summary>
        /// 保存或修改工程WBS明细
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <returns></returns>
        GWBSDetail SaveOrUpdateDetail(GWBSDetail dtl);

        /// <summary>
        /// 保存或修改任务明细和父对象
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="parentNode">父对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns></returns>
        GWBSDetail SaveOrUpdateDetail(GWBSDetail dtl, ref GWBSTree parentNode, IList listLedger);

        /// <summary>
        /// 保存或修改工程WBS明细集合
        /// </summary>
        /// <param name="list">明细集合</param>
        /// <returns></returns>
        IList SaveOrUpdateDetail(IList list);
        /// <summary>
        /// 删除工程WBS明细集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DeleteDetail(IList list);

        /// <summary>
        /// 保存或修改明细分科目成本
        /// </summary>
        /// <param name="list">分科目成本集合</param>
        /// <returns></returns>
        IList SaveOrUpdateCostSubject(IList list);

        /// <summary>
        /// 删除明细分科目成本集合
        /// </summary>
        /// <param name="list">分科目成本集合</param>
        /// <returns></returns>
        bool DeleteCostSubject(IList list);

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetCode(Type type);
    }
}
