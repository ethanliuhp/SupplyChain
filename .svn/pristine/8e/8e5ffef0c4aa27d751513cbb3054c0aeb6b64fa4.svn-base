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
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Service
{
    public interface IProjectTaskAccountSrv
    {
        /// <summary>
        /// 获取工程任务核算单编号
        /// </summary>
        /// <returns></returns>
        string GetProjectTaskAccountCode();

        /// <summary>
        /// 获取工程任务核算单明细编号
        /// </summary>
        /// <returns></returns>
        string GetProjectTaskAccountDetailCode(string contractGroupCode, int detailNum);

        /// <summary>
        /// 保存或修改工程任务核算单集合
        /// </summary>
        /// <param name="list">工程任务核算单集合</param>
        /// <returns></returns>
        IList SaveOrUpdateProjectTaskAccount(IList list);

        /// <summary>
        /// 保存或修改工程任务核算单
        /// </summary>
        /// <param name="bill">工程任务核算单</param>
        /// <returns></returns>
        ProjectTaskAccountBill SaveOrUpdateProjectTaskAccount(ProjectTaskAccountBill bill);

        /// <summary>
        /// 删除工程任务核算单集合
        /// </summary>
        /// <param name="list">工程任务核算单集合</param>
        /// <returns></returns>
        bool DeleteProjectTaskAccount(IList list);

        /// <summary>
        /// 工程任务核算单明细集合
        /// </summary>
        /// <param name="list">工程任务核算单明细集合</param>
        /// <returns></returns>
        bool DeleteProjectTaskAccountDetail(IList list);

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
        /// 保存工程任务明细核算和汇总信息
        /// </summary>
        /// <param name="dtl">工程任务明细核算</param>
        /// <param name="sum">工程任务明细汇总</param>
        /// <returns></returns>
        bool SaveProjectTaskDetailAccountAndSummary(ProjectTaskDetailAccount dtl, ProjectTaskDetailAccountSummary sum);

        /// <summary>
        /// 保存核算明细并反写确认明细数据
        /// </summary>
        /// <param name="bill">核算单</param>
        /// <param name="listConfigDetail">确认明细集合</param>
        /// <returns></returns>
        ProjectTaskAccountBill SaveProjectTaskAndSetConfirmState(ProjectTaskAccountBill bill,IList listConfirmDetail);
    }
}
