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
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Service
{
    public interface IGWBSDetailCostSubjectSrv
    {
        /// <summary>
        /// 保存或修改明细分科目成本
        /// </summary>
        /// <param name="list">分科目成本集合</param>
        /// <returns></returns>
        IList SaveOrUpdateCostSubject(IList list);

        /// <summary>
        /// 删除明细分科目成本集合
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DeleteCostSubject(IList list);
    }
}
