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
using Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Service
{
    public interface ISubContractBalanceBillSrv
    {

        /// <summary>
        /// 保存或修改分包结算单集合
        /// </summary>
        /// <param name="list">分包结算单集合</param>
        /// <returns></returns>
        IList SaveOrUpdateSubContractBalanceBill(IList list);

        /// <summary>
        /// 保存或修改分包结算单
        /// </summary>
        /// <param name="bill">分包结算单</param>
        /// <returns></returns>
        SubContractBalanceBill SaveOrUpdateSubContractBalanceBill(SubContractBalanceBill bill);

        /// <summary>
        /// 保存分包结算单
        /// </summary>
        /// <param name="bill">分包结算单</param>
        /// <param name="listAccountDetail">结算的核算明细</param>
        /// <param name="listPenalty">结算的罚扣款单</param>
        /// <param name="listLaborSporadic">结算的零星用工单</param>
        /// <returns></returns>
        SubContractBalanceBill SaveSubContractBalanceBill(SubContractBalanceBill bill, IList listAccountDetail, IList listPenalty, IList listLaborSporadic);

        /// <summary>
        /// 删除分包结算单集合
        /// </summary>
        /// <param name="list">分包结算单集合</param>
        /// <returns></returns>
        bool DeleteSubContractBalanceBill(IList list);

        /// <summary>
        /// 分包结算单明细集合
        /// </summary>
        /// <param name="list">分包结算单明细集合</param>
        /// <returns></returns>
        bool DeleteSubContractBalanceBillDetail(IList list);

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
