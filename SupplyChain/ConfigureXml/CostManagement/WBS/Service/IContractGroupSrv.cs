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
    public interface IContractGroupSrv
    {
        /// <summary>
        /// 获取契约组编号
        /// </summary>
        /// <returns></returns>
        string GetContractGroupCode();

        /// <summary>
        /// 获取契约组明细编号
        /// </summary>
        /// <returns></returns>
        string GetContractGroupDetailCode(string contractGroupCode, int detailNum);

        /// <summary>
        /// 保存或修改契约组集合
        /// </summary>
        /// <param name="list">契约组集合</param>
        /// <returns></returns>
        IList SaveOrUpdateContractGroup(IList list);
        /// <summary>
        /// 保存或修改契约组
        /// </summary>
        /// <param name="group">契约组</param>
        /// <returns></returns>
        ContractGroup SaveOrUpdateContractGroup(ContractGroup group);

        /// <summary>
        /// 删除契约组集合
        /// </summary>
        /// <param name="list">契约组集合</param>
        /// <returns></returns>
        bool DeleteContractGroup(IList list);

        /// <summary>
        /// 契约组明细集合
        /// </summary>
        /// <param name="list">契约组明细集合</param>
        /// <returns></returns>
        bool DeleteContractGroupDetail(IList list);

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
