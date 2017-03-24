using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Service;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;
using NHibernate.Criterion;
using VirtualMachine.SystemAspect.Security;
using NHibernate;
using System.Runtime.Remoting.Messaging;
using System.Data;
using VirtualMachine.Core.DataAccess;
using Application.Resource.FinancialResource;
using Application.Resource.PersonAndOrganization.OrganizationResource.Service;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.CommonClass.Domain;
using Iesi.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    /// <summary>
    /// 工程任务类型服务
    /// </summary>
    public class ResourceRequirePlanSrv : IResourceRequirePlanSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        /// <summary>
        /// 保存或修改资源需求计划集合
        /// </summary>
        /// <param name="list">资源需求计划集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateResourceRequirePlan(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 保存或修改资源需求计划
        /// </summary>
        /// <param name="group">资源需求计划</param>
        /// <returns></returns>
        [TransManager]
        public ResourceRequirePlan SaveOrUpdateResourceRequirePlan(ResourceRequirePlan plan)
        {
            dao.SaveOrUpdate(plan);
            return plan;
        }

        /// <summary>
        /// 删除资源需求计划集合
        /// </summary>
        /// <param name="list">资源需求计划集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteResourceRequirePlan(IList list)
        {
            foreach (ResourceRequirePlan cg in list)
            {
                if (!string.IsNullOrEmpty(cg.Id))
                {
                    object obj = dao.Get(typeof(ResourceRequirePlan), cg.Id);
                    if (obj != null)
                        dao.Delete(obj);
                }
            }
            return true;
        }

        /// <summary>
        /// 保存或修改计划明细
        /// </summary>
        /// <param name="dtl">计划明细</param>
        /// <returns></returns>
        [TransManager]
        public ResourceRequirePlanDetail SaveOrUpdateResourcePlanDetail(ResourceRequirePlanDetail dtl)
        {
            dao.SaveOrUpdate(dtl);

            return dtl;
        }

        /// <summary>
        /// 保存或修改计划明细集合
        /// </summary>
        /// <param name="list">计划明细集合</param>
        /// <returns></returns>
        [TransManager]
        public IList SaveOrUpdateResourcePlanDetail(IList list)
        {
            dao.SaveOrUpdate(list);

            return list;
        }

        /// <summary>
        /// 资源需求计划明细集合
        /// </summary>
        /// <param name="list">资源需求计划明细集合</param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteResourceRequirePlanDetail(IList list)
        {
            foreach (ResourceRequirePlanDetail dtl in list)
            {
                if (!string.IsNullOrEmpty(dtl.Id))
                {
                    ResourceRequirePlanDetail tempDtl = dao.Get(typeof(ResourceRequirePlanDetail), dtl.Id) as ResourceRequirePlanDetail;
                    if (tempDtl != null)
                        dao.Delete(tempDtl);
                }
            }
            return true;
        }

        /// <summary>
        /// 根据对象类型和GUID获取对象
        /// </summary>
        /// <param name="entityType">对象类型</param>
        /// <param name="Id">GUID</param>
        /// <returns></returns>
        public Object GetObjectById(Type entityType, string Id)
        {
            return dao.Get(entityType, Id);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return dao.ObjectQuery(entityType, oq);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return DateTime.Now;
        }
    }
}
