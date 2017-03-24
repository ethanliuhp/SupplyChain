﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.Core;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Service
{
    public interface ICostItemSrv
    {
        /// <summary>
        /// 获取编号
        /// </summary>
        /// <returns></returns>
        string GetCostItemCode();

        /// <summary>
        /// 获取明细编号
        /// </summary>
        /// <returns></returns>
        string GetCostItemDetailCode(string CostItemCode, int detailNum);

        /// <summary>
        /// 保存或修改成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        IList SaveOrUpdateCostItem(IList list);
        /// <summary>
        /// 保存或修改成本项
        /// </summary>
        /// <param name="item">成本项</param>
        /// <returns></returns>
        CostItem SaveOrUpdateCostItem(CostItem item);

        /// <summary>
        /// 删除成本项集合
        /// </summary>
        /// <param name="list">成本项集合</param>
        /// <returns></returns>
        bool DeleteCostItem(IList list);

        /// <summary>
        /// 删除成本定额集合
        /// </summary>
        /// <param name="list">成本定额集合</param>
        /// <returns></returns>
        bool DeleteCostItemQuota(IList list);

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

        /// <summary>
        /// 保存或修改成本定额
        /// </summary>
        /// <param name="list">成本定额集合</param>
        /// <returns></returns>
        IList SaveOrUpdateCostItemQuota(IList list);
    }
}
