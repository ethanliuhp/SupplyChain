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
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    public interface IResourceRequirePlanSrv
    {
        /// <summary>
        /// 保存或修改资源需求计划集合
        /// </summary>
        /// <param name="list">资源需求计划集合</param>
        /// <returns></returns>
        IList SaveOrUpdateResourceRequirePlan(IList list);

        /// <summary>
        /// 保存或修改资源需求计划
        /// </summary>
        /// <param name="group">资源需求计划</param>
        /// <returns></returns>
        ResourceRequirePlan SaveOrUpdateResourceRequirePlan(ResourceRequirePlan plan);

        /// <summary>
        /// 删除资源需求计划集合
        /// </summary>
        /// <param name="list">资源需求计划集合</param>
        /// <returns></returns>
        bool DeleteResourceRequirePlan(IList list);

        /// <summary>
        /// 保存或修改滚动资源需求计划和计划明细
        /// </summary>
        /// <param name="plan">滚动资源需求计划</param>
        /// <param name="list">滚动资源需求计划明细集合</param>
        /// <param name="isPublish">删除</param>
        /// <param name="deleteList">是否发布</param>
        /// <returns></returns>
        Hashtable SaveOrUpdateResourcePlanAndDetail(ResourceRequirePlan plan, IList list, bool isPublish,IList deleteList);

        /// <summary>
        /// 保存或修改计划明细
        /// </summary>
        /// <param name="group">计划明细</param>
        /// <returns></returns>
        ResourceRequirePlanDetail SaveOrUpdateResourcePlanDetail(ResourceRequirePlanDetail dtl);

        /// <summary>
        /// 保存或修改计划明细集合
        /// </summary>
        /// <param name="group">计划明细集合</param>
        /// <returns></returns>
        IList SaveOrUpdateResourcePlanDetail(IList list);

        /// <summary>
        /// 资源需求计划明细集合
        /// </summary>
        /// <param name="list">资源需求计划明细集合</param>
        /// <returns></returns>
        bool DeleteResourceRequirePlanDetail(IList list);

        /// <summary>
        /// 保存或修改资源需求计划单
        /// </summary>
        /// <param name="rrr"></param>
        /// <returns></returns>
        ResourceRequireReceipt SaveOrUpdateResourceRequireReceipt(ResourceRequireReceipt rrr);

        /// <summary>
        /// 删除资源需求计划单
        /// </summary>
        /// <param name="rrr"></param>
        /// <returns></returns>
        bool DeleteResourceRequireReceipt(ResourceRequireReceipt rrr);
        /// <summary>
        /// 删除资源需求计划单集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DeleteResourceRequireReceiptList(IList list);
        /// <summary>
        /// 删除资源需求计划单明细
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [TransManager]
        bool DeleteResourceRequireReceiptDtlList(IList list);
        /// <summary>
        /// 保存或修改资源需求计划单明细集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        IList SaveOrUpdateResourceRequireReceiptDetail(IList list);

         /// <summary>
        /// 保存或修改资源需求计划单和其明细集合
        /// </summary>
        /// <param name="rrr"></param>
        /// <param name="rrrd"></param>
        /// <returns></returns>
        ResourceRequireReceipt SaveResourceRequireReceiptAndDetail(ResourceRequireReceipt rrr, IList list);

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

        DataSet SearchSQL(string sql);

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        string GetCode(Type type, string projectId);

        /// <summary>
        /// 根据料具资源需求计划生成物资资源需求计划
        /// </summary>
        /// <param name="planType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        bool GenerateSupplyResourcePlanBak(RemandPlanType planType, string id);

        /// <summary>
        /// 根据期间需求计划生成物资资源需求计划
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList GenerateSupplyResourcePlan(string id);

        Hashtable GetFirstMatInfo();
        /// <summary>
        /// 通过滚动需求计划查询所属滚动计划的所有总量需求计划明细
        /// </summary>
        /// <param name="strPlan"></param>
        /// <returns></returns>
        Hashtable GetTotalReceipt(string strPlan);
        /// <summary>
        /// 通过滚动需求计划查询滚动计划明细
        /// </summary>
        /// <param name="strPlan"></param>
        /// <returns></returns>
        Hashtable GetTotalPlan(string strPlan);

        IList GetResReceipt(ObjectQuery objectQuery);
        ResourceRequireReceipt SaveResReceipt(ResourceRequireReceipt obj);
        bool DeleteResReceipt(ResourceRequireReceipt obj);
        bool DeleteResReceiptDetail(ResourceRequireReceiptDetail obj);
        ResourceRequireReceiptDetail SaveResReceiptDetail(ResourceRequireReceiptDetail obj);
        IList ObjectQuery(ObjectQuery oq);
        ResourceRequireReceipt GetResourceRequireReceipt(ObjectQuery oq);
        Hashtable GetResPlanDetail(ObjectQuery oq);
        //IList GetResPlanDetail(ObjectQuery oq);
       
        /// <summary>
        /// 计算GWBS滚动资源需求数据
        /// </summary>
        /// <param name="rrp">所依据的{滚动资源需求计划}</param>
        /// <param name="wbs">指定的{工程项目任务}节点</param>
        /// <param name="mat">指定的资源类型</param>
        /// <param name="diagramNumber">指定资源类型的图号</param>
        /// <returns></returns>
        ResourceRequirePlanDetail CalculateGWBSRollingResourceDemandData(ResourceRequirePlan rrp, GWBSTree wbs, Material mat, string diagramNumber, PlanRequireType requireType);
        /// <summary>
        /// 需求总量分摊系数
        /// </summary>
        /// <param name="rrpd">被分摊滚动需求计划明细对象</param>
        /// <param name="wbs">需要计算分摊系数的GWBS节点，该节点是<滚动资源需求计划明细>_【所属工程项目任务】的子节点</param>
        /// <returns></returns>
        decimal DemandTotalSharingCoefficient(ResourceRequirePlanDetail rrpd, GWBSTree wbs);

        /// <summary>
        /// 获取预算资源需求量
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="projectInfo"></param>
        /// <param name="wbs"></param>
        /// <returns></returns>
        IList GetBudgetResourcesDemand(ResourceRequirePlan plan, CurrentProjectInfo projectInfo,GWBSTree wbs);

        IList GetBudgetResourcesDemand(ResourceRequireReceipt plan,CurrentProjectInfo projectInfo, GWBSTree wbs, DateTime beginDate, DateTime endDate,ResourceTpye rt,PlanType st);
 
        IList GWBSPlanValue(WeekScheduleMaster schedulePlan, GWBSTree wbs);
        /// <summary>
        /// 生成期间需求计划单明细
        /// </summary>
        /// <param name="resPlan"></param>
        /// <param name="resReceipt"></param>
        /// <param name="ht"></param>
        /// <returns></returns>
        IList GetPeriodResPlanDetail(ResourceRequirePlan resPlan, ResourceRequireReceipt resReceipt, List<GWBSTree> ht, int a);

        /// <summary>
        /// 修改滚动资源需求计划明细状态
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        ResourceRequirePlan modifyPlanDetailState(ResourceRequirePlan plan);

        IList GenerateSupplyResourcePlanNew(string id);
    }
}
