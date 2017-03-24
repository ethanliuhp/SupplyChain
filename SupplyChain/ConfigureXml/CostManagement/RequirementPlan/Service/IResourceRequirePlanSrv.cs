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
    }
}
