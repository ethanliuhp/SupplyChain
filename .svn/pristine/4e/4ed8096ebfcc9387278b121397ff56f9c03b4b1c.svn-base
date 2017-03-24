using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Service;
using Application.Business.Erp.SupplyChain.CostManagement.RequirementPlan.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.Client.PeriodRequirePlan
{
    public class MPeriodRequirePlanMng
    {
        private IResourceRequirePlanSrv resourceRequirePlanSrv;
        public IResourceRequirePlanSrv ResourceRequirePlanSrv
        {
            get { return resourceRequirePlanSrv; }
            set { resourceRequirePlanSrv = value; }
        }

        public MPeriodRequirePlanMng()
        {
            if (resourceRequirePlanSrv == null)
            {
                resourceRequirePlanSrv = StaticMethod.GetService("ResourceRequirePlanSrv") as IResourceRequirePlanSrv;
            }
        }

        /// 保存总量需求计划单信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ResourceRequireReceipt SaveResourceReceipt(ResourceRequireReceipt obj)
        {
            return resourceRequirePlanSrv.SaveResReceipt(obj);
        }


        /// <summary>
        /// 根据期间需求计划生成物资资源需求计划
        /// </summary>
        /// <param name="planType"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList GenerateSupplyResourcePlan(string id)
        {
            return resourceRequirePlanSrv.GenerateSupplyResourcePlan(id);
        }

        /// <summary>
        /// 生成期间需求计划单明细
        /// </summary>
        /// <param name="resPlan"></param>
        /// <param name="resReceipt"></param>
        /// <param name="ht"></param>
        /// <param name="a"></param>
        /// <returns></returns>
        public IList GetPeriodResPlanDetail(ResourceRequirePlan resPlan, ResourceRequireReceipt resReceipt, List<GWBSTree> ht, int a)
        {
            return resourceRequirePlanSrv.GetPeriodResPlanDetail(resPlan, resReceipt, ht, a);
        }
    }
}
