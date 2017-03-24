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
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.RequirementPlan
{
    public class MRollingDemandPlan
    {
        //private static IResourceRequirePlanSrv mm;

        private IResourceRequirePlanSrv mm;
        public IResourceRequirePlanSrv Mm
        {
            get { return mm; }
            set { mm = value; }
        }

        public MRollingDemandPlan()
        {
            if (mm == null)
                mm = ConstMethod.GetService("ResourceRequirePlanSrv") as IResourceRequirePlanSrv;
        }

        /// <summary>
        /// 保存或修改资源需求计划集合
        /// </summary>
        /// <param name="list">资源需求计划集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateResourceRequirePlan(IList list)
        {
            return mm.SaveOrUpdateResourceRequirePlan(list);
        }

        /// <summary>
        /// 保存或修改资源需求计划
        /// </summary>
        /// <param name="group">资源需求计划</param>
        /// <returns></returns>
        public ResourceRequirePlan SaveOrUpdateResourceRequirePlan(ResourceRequirePlan cg)
        {
            return mm.SaveOrUpdateResourceRequirePlan(cg);
        }

        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return mm.GetObjectById(entityType, Id);
        }

        /// <summary>
        /// 删除资源需求计划集合
        /// </summary>
        /// <param name="list">资源需求计划集合</param>
        /// <returns></returns>
        public bool DeleteResourceRequirePlan(IList list)
        {
            return mm.DeleteResourceRequirePlan(list);
        }

        /// <summary>
        /// 保存或修改资源需求计划单
        /// </summary>
        /// <param name="rrr"></param>
        /// <returns></returns>
        public ResourceRequireReceipt SaveOrUpdateResourceRequireReceipt(ResourceRequireReceipt rrr)
        {
            return mm.SaveOrUpdateResourceRequireReceipt(rrr);
        }

        /// <summary>
        /// 保存或修改滚动资源需求计划和计划明细
        /// </summary>
        /// <param name="plan">滚动资源需求计划</param>
        /// <param name="list">滚动资源需求计划明细集合</param>
        /// <param name="isPublish">删除</param>
        /// <param name="deleteList">是否发布</param>
        /// <returns></returns>
        public Hashtable SaveOrUpdateResourcePlanAndDetail(ResourceRequirePlan plan, IList list, bool isPublish,IList deleteList)
        {
            return mm.SaveOrUpdateResourcePlanAndDetail(plan, list, isPublish,deleteList);
        }
        /// <summary>
        /// 保存或修改计划明细
        /// </summary>
        /// <param name="group">计划明细</param>
        /// <returns></returns>
        public ResourceRequirePlanDetail SaveOrUpdateResourcePlanDetail(ResourceRequirePlanDetail dtl)
        {
            return mm.SaveOrUpdateResourcePlanDetail(dtl);
        }

        /// <summary>
        /// 保存或修改计划明细集合
        /// </summary>
        /// <param name="list">计划明细集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateResourcePlanDetail(IList list)
        {
            return mm.SaveOrUpdateResourcePlanDetail(list);
        }

        /// <summary>
        /// 资源需求计划明细集合
        /// </summary>
        /// <param name="list">资源需求计划明细集合</param>
        /// <returns></returns>
        public bool DeleteResourceRequirePlanDetail(IList list)
        {
            return mm.DeleteResourceRequirePlanDetail(list);
        }

        /// <summary>
        /// 删除资源需求计划单
        /// </summary>
        /// <param name="rrr"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteResourceRequireReceipt(ResourceRequireReceipt rrr)
        {
            return mm.DeleteResourceRequireReceipt(rrr);
        }
        /// <summary>
        /// 删除资源需求计划单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteResourceRequireReceiptList(IList list)
        {
            return mm.DeleteResourceRequireReceiptList(list);
        }
        /// <summary>
        /// 保存或修改资源需求计划单明细集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public IList SaveOrUpdateResourceRequireReceiptDetail(IList list)
        {
            return mm.SaveOrUpdateResourceRequireReceiptDetail(list);
        }

         /// <summary>
        /// 保存或修改资源需求计划单和其明细集合
        /// </summary>
        /// <param name="rrr"></param>
        /// <param name="rrrd"></param>
        /// <returns></returns>
        public ResourceRequireReceipt SaveResourceRequireReceiptAndDetail(ResourceRequireReceipt rrr, IList list)
        {
            return mm.SaveResourceRequireReceiptAndDetail(rrr, list);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            oq.AddFetchMode("MaterialGUID", NHibernate.FetchMode.Eager);
            return mm.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return mm.GetServerTime();
        }

        public DataSet SearchSQL(string sql)
        {
            return mm.SearchSQL(sql);
        }

        /// <summary>
        /// 获取对象编码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCode(Type type, string projectId)
        {
            return mm.GetCode(type, projectId);
        }

        /// <summary>
        /// 根据料具资源需求计划生成物资资源需求计划
        /// </summary>
        /// <param name="planType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool GenerateSupplyResourcePlanBak(RemandPlanType planType, string id)
        {
            return mm.GenerateSupplyResourcePlanBak(planType, id);
        }

       /// <summary>
        /// 获取预算资源需求量
       /// </summary>
       /// <param name="plan"></param>
       /// <param name="projectInfo"></param>
       /// <returns></returns>
        public IList GetBudgetResourcesDemand(ResourceRequirePlan plan, CurrentProjectInfo projectInfo,GWBSTree wbs)
        {
            return mm.GetBudgetResourcesDemand(plan, projectInfo,wbs);
        }

        public IList GetBudgetResourcesDemand(ResourceRequireReceipt plan ,CurrentProjectInfo projectInfo, GWBSTree wbs, DateTime beginDate, DateTime endDate,ResourceTpye rt ,PlanType st)
        {
            return mm.GetBudgetResourcesDemand(plan,projectInfo, wbs, beginDate, endDate,rt,st);
        }

        public IList GenerateSupplyResourcePlan(string id)
        {
            return mm.GenerateSupplyResourcePlan(id);
        }

        public IList GenerateSupplyResourcePlanNew(string id)
        {
            return mm.GenerateSupplyResourcePlanNew(id);
        }
    }
}
