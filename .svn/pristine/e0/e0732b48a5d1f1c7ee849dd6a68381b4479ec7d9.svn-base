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
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Resource.MaterialResource.Domain;
using System.Drawing;
using VirtualMachine.Patterns.BusinessEssence.Domain;

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
        Hashtable MoveGWBSTree(GWBSTree pbs, GWBSTree toOrg);
        GWBSTree MoveGWBSTreeAndUpdateContract(GWBSTree pbs, GWBSTree toOrg, ContractGroup group);
        VirtualMachine.Patterns.CategoryTreePattern.Service.ICategoryNodeService NodeSrv { get; set; }

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        IList SaveGWBSTreeRootNode(IList list);
        GWBSTree SaveGWBSTree(GWBSTree childOrg);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="childOrg"></param>
        /// <param name="listVerify"></param>
        /// <returns></returns>
        GWBSTree SaveGWBSTree(GWBSTree childOrg, IList listVerify);
        IList GWBSTreeOrder(IList list);

        IList GetGWBSTreeRules(Type treeType);
        IList GetGWBSTreeRules(Type treeType, Type parentNodeType);
        bool InitRule();

        IList GetGWBSTrees(Type t, ObjectQuery oq);
        IList GetGWBSTreeAndJobs(Type t);
        GWBSTree GetGWBSTree(GWBSTree orgCat);
        IList GetGWBSTreeAndJobs();
        IList SaveGWBSTrees(IList lst);

        List<GWBSDetail> GetWBSDetail(string wbsId, string sqlWhere);

        GWBSTree InsertOrUpdateWBSTree(GWBSTree childNode);
        GWBSTree InsertOrUpdateWBSTree(GWBSTree childNode, IList listVerify);
        IList InsertOrUpdateWBSTrees(IList lst);
        IList InsertOrUpdateWBSTrees(IList lst, IList listVerify);

        long GetMaxOrderNo(CategoryNode node, ObjectQuery oq);
        /// <summary>
        /// 获取业务组织节点集合(返回有权限和无权限的集合)
        /// </summary>
        IList GetGWBSTreesByInstance(string projectId);
        
        DataTable GetGWBSTrees(string sProjectID, string sParentID, int iLevel);
        DataSet GetGWBSTrees(string sProjectID, string sName, DateTime dStartDate, DateTime dEndDate);
        DataSet GetGWBSTreesByWhere(string sWhere);
        DataSet GetGWBSTreesByInstanceSql(string projectId);
        DataSet GetGWBSTreesByInstanceSql(string projectId, string sParentID, string sName);
        void SaveBatchGWBSTreeName(IList lstGWBSTree);
        IList GetGWBSTreesByInstance(string projectId, string sSysCode, int iLevel);
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
        /// 对象查询并加载其完整路径
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList GetGWBSTreeAndFullPath(ObjectQuery oq);

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
        GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, IList listLedger);

        /// <summary>
        /// 保存或更新工程WBS集合,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点集合</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns></returns>
        IList SaveOrUpdateGWBSTree(IList list, IList listLedger);

        /// <summary>
        /// 保存或更新工程WBS集合,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点集合</param>
        /// <returns></returns>
        IList SaveOrUpdateGWBSTree(IList list);

        /// <summary>
        /// 保存复制的任务明细到任务节点
        /// </summary>
        /// <param name="wbsNode"></param>
        /// <param name="listDtl"></param>
        /// <returns></returns>
        IList SaveCopyDetailByNode(ContractGroup selectedContractGroup, GWBSTree wbsNode, List<GWBSDetail> listDtl);

        bool SaveCopyDetailByNode(ContractGroup selectedContractGroup, List<GWBSTree> listWBSNode, List<GWBSDetail> listDtl);

        /// <summary>
        /// 保存或修改工程WBS明细（1.任务明细，2.明细变更台账）
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>1.任务明细，2.明细变更台账</returns>
        IList SaveOrUpdateDetail(GWBSDetail dtl, IList listLedger);

        /// <summary>
        ///  变更工程WBS明细（1.任务明细，2.明细变更台账）
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>1.任务明细，2.明细变更台账</returns>
        IList ChangeTaskDetail(GWBSDetail dtl, IList listLedger);

        /// <summary>
        /// 保存或修改工程WBS明细集
        /// </summary>
        /// <param name="dtl">明细对象集合</param>
        /// <param name="dtl">明细耗用对象集合</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>返回对象集合（1.任务明细集，2.资源耗用集）</returns>
        IList SaveOrUpdateDetail(IList listDtl, IList listDtlUsage, IList listLedger);

        /// <summary>
        /// 保存或修改工程GWBS和GWBS明细
        /// </summary>
        /// <param name="listGWBSNode">项目任务集</param>
        /// <param name="listGWBSDtl">任务明细集</param>
        /// <param name="isReturnValue">是否需要返回值</param>
        /// <returns>返回对象集合（1.项目任务集,2.任务明细集）</returns>
        IList SaveOrUpdateDetail(IList listGWBSNode, IList listGWBSDtl, bool isReturnValue);

        /// <summary>
        /// 保存或修改工程WBS明细集
        /// </summary>
        /// <param name="dtl">明细对象集合</param>
        /// <param name="dtl">明细耗用对象集合</param>
        /// <returns>返回对象集合（1.任务明细集，2.资源耗用集）</returns>
        IList SaveOrUpdateDetailByCostEdit(IList listDtl, IList listLedger, IList listDtlUsage, List<string> listDeleteDtlUsages);

        /// <summary>
        /// 保存或修改任务明细和父对象
        /// </summary>
        /// <param name="dtl">明细对象</param>
        /// <param name="parentNode">父对象</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <returns>返回对象集合（1.任务明细，2.父节点）</returns>
        IList SaveOrUpdateDetail(GWBSDetail dtl, GWBSTree parentNode, IList listLedger);

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

        /// <summary>
        /// 保存或修改工程任务类型文档模版
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        ProTaskTypeDocumentStencil SaveOrUpDateDocStencil(ProTaskTypeDocumentStencil obj);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool Delete(IList list);
        /// <summary>
        /// 根据父节点层次码查找其子节点
        /// </summary>
        /// <param name="parentNodeSyscode"></param>
        /// <returns></returns>
        IList GetGWBSTreeByParentNodeSyscode(string parentNodeSyscode);

        /// <summary>
        /// 查询成本核算任务明细
        /// </summary>
        /// <param name="project"></param>
        /// <param name="gwbs"></param>
        /// <param name="taskType"></param>
        /// <param name="costItem"></param>
        /// <param name="accSubject"></param>
        /// <param name="mat"></param>
        /// <param name="usageName"></param>
        /// <returns>（1.任务明细集，2.任务明细资源耗用集）</returns>
        IList GetCostAccDtl(ObjectQuery oq, CostAccountSubject accSubject, MaterialCategory matCate, Material mat, string usageName);

        /// <summary>
        /// 获取分类树节点的完整路径
        /// </summary>
        /// <param name="cateEntityType"></param>
        /// <param name="nodeObj"></param>
        /// <returns></returns>
        string GetCategorTreeFullPath(Type cateEntityType, VirtualMachine.Patterns.CategoryTreePattern.Domain.CategoryNode nodeObj);

        /// <summary>
        /// 获取分类树节点的完整路径
        /// </summary>
        /// <param name="cateEntityType"></param>
        /// <param name="nodeName"></param>
        /// <param name="nodeSysCode"></param>
        /// <returns></returns>
        string GetCategorTreeFullPath(Type cateEntityType, string nodeName, string nodeSysCode);

        /// <summary>
        /// 计算分包取费任务明细的预算成本
        /// </summary>
        /// <param name="listDtl">分包取费明细集</param>
        /// <returns></returns>
        IList AccountSubContractFeeDtl(IList listDtl, IList listDtlUsage, DocumentState state);

        /// <summary>
        /// 计算分包取费任务明细的预算成本
        /// </summary>
        /// <param name="listDtl">分包取费明细集</param>
        /// <returns></returns>
        IList AccountSubContractFeeDtl(IList listDtl, IList listDtlUsage);

        /// <summary>
        /// 根据wbs任务类型得到工程文档验证
        /// </summary>
        /// <param name="taskTypeId"></param>
        /// <returns></returns>
        IList GetDocumentTemplatesByTaskType(GWBSTree wbs);

        /// <summary>
        /// 如果有多个对象，第一个对象为根节点，后续为子节点
        /// </summary>
        /// <param name="lst">树节点集合</param>
        /// <returns></returns>
        IList SaveGWBSTreeRootNode1(IList lst);
        /// <summary>
        /// 保存一个节点加上其子节点集合
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        IList SaveGWBSTrees1(IList lst);

        /// <summary>
        /// 校验发布的任务明细的成本项、主资源、图号不能重复
        /// </summary>
        /// <param name="listDtl"></param>
        /// <returns></returns>
        string CheckGWBSDetail(IList listDtl);

        /// <summary>
        /// 计算项目任务的合同合价、责任合价、计划合价
        /// </summary>
        /// <param name="task">项目任务</param>
        /// <returns></returns>
        GWBSTree AccountTotalPrice(GWBSTree task);

        /// <summary>
        /// 计算项目任务所有下级任务节点的合同合价、责任合价、计划合价
        /// </summary>
        /// <param name="task">项目任务</param>
        /// <returns></returns>
        GWBSTree AccountTotalPriceByChilds(GWBSTree task);

        /// <summary>
        /// 发布任务节点及其子节点
        /// </summary>
        /// <param name="taskId">父任务</param>
        /// <returns></returns>
        GWBSTree PublisthTaskNodeAndChilds(GWBSTree task);

        /// <summary>
        /// 作废任务节点及其子节点
        /// </summary>
        /// <param name="taskId">父任务</param>
        /// <returns></returns>
        GWBSTree InvalidTaskNodeAndChilds(GWBSTree task);

        List<GWBSDetail> AccountTaskDtlPrice(List<GWBSDetail> listTargetTaskDtl);

        /// <summary>
        /// 保存拆除的任务明细
        /// </summary>
        /// <param name="dtl"></param>
        /// <returns></returns>
        GWBSDetail SaveBackoutGWBSDetail(GWBSDetail dtl, string optTypeCode);

        /// <summary>
        /// 成本项分类划分区域的改变
        /// </summary>
        /// <param name="wbs"></param>
        /// <param name="cate"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        CostItemsZoning CostItemZoningChange(GWBSTree wbs, CostItemCategory cate, CurrentProjectInfo info);

        /// <summary>
        /// 删除成本项地域
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        void DeleteCostItemZoning(CostItemsZoning z);

        decimal GetProjectCostWarningInfo(string projectId, string projectSyscode, string projectType);

        #region 项目或公司、分公司预警及统计服务

        List<OperationOrg> GetOperationOrg();

        List<List<OperationOrg>> GetOperationOrgByUser(string userCode);

        List<string> GetOperationOrgSyscodeByUser(string userCode);

        /// <summary>
        /// 获取项目安全、质量、成本、工期预警指标信息（1.安全，2.质量，3.成本，4.工期）
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="projectSyscode"></param>
        /// <param name="projectType"></param>
        /// <returns></returns>
        IList GetProjectWarningTargetInfo(string projectId, string projectSyscode, string projectType);

        /// <summary>
        /// 获取起始日期开始的项目或公司/分公司各月合同收入、责任成本、实际成本信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="projectSyscode"></param>
        /// <returns></returns>
        DataTable[] GetProjectCostData(DateTime startDate, string projectSyscode);

        /// <summary>
        /// 获取起始日期开始的项目或公司/分公司当年和上一年年度合同收入、责任成本、实际成本信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="projectSyscode"></param>
        /// <returns></returns>
        DataTable[] GetProjectCostDataByYear(DateTime startDate, string projectSyscode);

        /// <summary>
        /// 获取起始日期开始的项目或公司/分公司各月利润信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="projectSyscode"></param>
        /// <returns></returns>
        DataSet GetProjectProfitData(DateTime startDate, string projectSyscode);

        /// <summary>
        /// 获取起始日期开始的项目或公司/分公司各月合同收入和收款信息
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="projectSyscode"></param>
        /// <returns></returns>
        DataTable[] GetProjectCollectionsummoneyData(DateTime startDate, string projectSyscode);

        Dictionary<CurrentProjectInfo, IList> QueryProjectWarnInfo(ObjectQuery oqProject);
        #endregion

        void Test(GWBSTree wbs);

        /// <summary>
        /// 根据项目ID和所需要的节点的层号的子节点 （大于当前层号）
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="iLevel"></param>
        /// <returns></returns>
        IList GetGWBSTreesByLevel(string projectId, int iLevel, string sParentSysCode);

        /// <summary>
        /// 查询 缺省WBS节点及其子节点上面的状态为发布的任务明细个数
        /// </summary>
        /// <param name="sysCode"></param>
        /// <returns></returns>
        int GetGWBSDetailLikeWBSSysCodeSql(string sysCode);
        int GetGWBSDetailLikeWBSSysCodeSql(string sysCode, string sProjectId);
        /// <summary>
        /// 导入任务明细检查这些定额编号在成本项里是否有重复
        /// </summary>
        /// <returns></returns>
        bool CheckQutaCodeIsRepeat(string strSql);

        /// <summary>
        /// 作废明细前的判断
        /// </summary>
        /// <param name="list"></param>
        /// <param name="wbs"></param>
        /// <returns></returns>
        string DeleteGWBSDetailBeforeOperat(List<GWBSDetail> list, GWBSTree wbs);

        /// <summary>
        /// 删除明细 连带相应生产明细一起删除
        /// </summary>
        /// <param name="wbs"></param>
        /// <param name="listInvalid"></param>
        /// <returns></returns>
        GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, List<GWBSDetail> listInvalid);

        /// <summary>
        /// 保存或更新GWBS,关联明细情况
        /// </summary>
        /// <param name="wbs">wbs节点</param>
        /// <param name="listLedger">明细台账集合</param>
        /// <param name="dtl">修改后的明细</param>
        /// <param name="theCostItemId">修改明细前的明细</param>
        /// <returns></returns>
        GWBSTree SaveOrUpdateGWBSTree(GWBSTree wbs, IList listLedger, GWBSDetail dtl, GWBSDetail beforeDtl);

        IList SaveBatchDetail(GWBSTree wbs, string sqlWhere);

        bool SaveAndWritebackScrollPlanState(GWBSTree wbs);

        List<GWBSDetailLedger> GWBSDtlLedgerQuery(string sqlCondition, string createStartTime, string createEndTime);

        /// <summary>
        /// 工程任务合价查询
        /// </summary>
        /// <param name="wbs"></param>
        /// <param name="projectInfo"></param>
        /// <returns></returns>
        DataTable SelectGWBSValence(GWBSTree wbs, CurrentProjectInfo projectInfo);
        /// <summary>
        /// 根据物资分类的syscode获取工程任务查询
        /// </summary>
        /// <param name="sSysCodes"></param>
        /// <param name="sProjectId"></param>
        /// <returns></returns>
        DataTable SelectGWBSValence(string sSysCodes, string sProjectId);
        /// <summary>
        /// 获取wbs明细上的最大code orderno 变更序号
        /// </summary>
        /// <param name="sGWBSTreeID">gwbs树id</param>
        /// <param name="sGWBSDetialID">需要变更明细id</param>
        /// <returns></returns>
        IList GetWBSDetailNum(string sGWBSTreeID, string sGWBSDetialID);
        /// <summary>
        /// 获取wbs明细上的最大code orderno 变更序号 服务器时间  变更明细序号
        /// </summary>
        /// <param name="sGWBSTreeID">gwbs树id</param>
        /// <param name="sGWBSDetialIDs">需要变更明细id集合</param>
        /// <returns></returns>
        IList GetWBSDetailNum(string sGWBSTreeID, string[] sGWBSDetialIDs);

        #region 工程成本批量维护
        DataSet GetGWBSDetailCostSubject(string sysCode);

        DataSet SelectGWBSResourceCost(GWBSTree wbs, CurrentProjectInfo projectInfo);

        DataSet GetSubjectCostQuotaByQuotaCode(string codes);

        DataSet GetSubjectCostQuotas(string id, string sysCode);
        #endregion

        bool UpdateGWBSAndScheduleOrderNO(List<GWBSTree> ilst);


        IList SaveOrUpdateShareRateInfo(IList ilistChanged, IList ilistDel);
       
    }
}
