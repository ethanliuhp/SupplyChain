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

namespace Application.Business.Erp.SupplyChain.Client.TotalDemandPlan
{
    public class MTotalDemandPlanMng
    {
        private IResourceRequirePlanSrv resourceRequirePlanSrv;
        public IResourceRequirePlanSrv ResourceRequirePlanSrv
        {
            get { return resourceRequirePlanSrv; }
            set { resourceRequirePlanSrv = value; }
        }

        public MTotalDemandPlanMng()
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
    }
}
