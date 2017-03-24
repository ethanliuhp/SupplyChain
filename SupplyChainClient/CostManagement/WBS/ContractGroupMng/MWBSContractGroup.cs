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
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.WBS.ContractGroupMng
{
    public class MWBSContractGroup
    {
        private static IContractGroupSrv mm;
        private static ILaborSporadicSrv laborSporadicSrv;
        public ILaborSporadicSrv LaborSporadicSrv
        {
            get { return laborSporadicSrv; }
            set { laborSporadicSrv = value; }
        }
        public MWBSContractGroup()
        {
            if (mm == null)
                mm = ConstMethod.GetService("ContractGroupSrv") as IContractGroupSrv;

            if (laborSporadicSrv == null)
                laborSporadicSrv = ConstMethod.GetService("LaborSporadicSrv") as ILaborSporadicSrv;
        }
       
        /// <summary>
        /// 获取契约组编号
        /// </summary>
        /// <returns></returns>
        public string GetContractGroupCode()
        {
            return mm.GetContractGroupCode();
        }

        /// <summary>
        /// 获取契约组明细编号
        /// </summary>
        /// <returns></returns>
        public string GetContractGroupDetailCode(string contractGroupCode, int detailNum)
        {
            return mm.GetContractGroupDetailCode(contractGroupCode, detailNum);
        }

        /// <summary>
        /// 保存或修改契约组集合
        /// </summary>
        /// <param name="list">契约组集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateContractGroup(IList list)
        {
            return mm.SaveOrUpdateContractGroup(list);
        }

        /// <summary>
        /// 保存或修改契约组
        /// </summary>
        /// <param name="group">契约组</param>
        /// <returns></returns>
        public ContractGroup SaveOrUpdateContractGroup(ContractGroup cg)
        {
            return mm.SaveOrUpdateContractGroup(cg);
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
        /// 删除契约组集合
        /// </summary>
        /// <param name="list">契约组集合</param>
        /// <returns></returns>
        public bool DeleteContractGroup(IList list)
        {
            return mm.DeleteContractGroup(list);
        }

        /// <summary>
        /// 契约组明细集合
        /// </summary>
        /// <param name="list">契约组明细集合</param>
        /// <returns></returns>
        public bool DeleteContractGroupDetail(IList list)
        {
            return mm.DeleteContractGroupDetail(list);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
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
    }
}
