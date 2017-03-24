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
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

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
        IList SaveOrUpdate(IList lst);
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
        ProjectTaskAccountBill SaveProjectTaskAndSetConfirmState(ProjectTaskAccountBill bill, IList listConfirmDetail);

        /// <summary>
        /// 保存核算单并反写确认明细数据（包括虚拟工单）
        /// </summary>
        /// <param name="bill">核算单</param>
        /// <param name="listConfigDetail">确认明细集合</param>
        /// <returns></returns>
        ProjectTaskAccountBill SaveAccBillAndSetCfmStateByVirCfmBill(ProjectTaskAccountBill bill, IList listConfirmDetail);

        /// <summary>
        /// 生成工程任务核算单
        /// </summary>
        /// <param name="curBillMaster"></param>
        /// <returns>1.核算单，2.要回写数据的确认单明细，3.异常/提示消息</returns>
        IList GenAccountBill(ProjectTaskAccountBill curBillMaster);

        /// <summary>
        /// 生成工程任务核算单(包括虚拟工单)
        /// </summary>
        /// <param name="curBillMaster"></param>
        /// <returns>1.核算单，2.要回写数据的确认单明细，3.异常/提示消息</returns>
        IList GenAccountBillByVirConfirmBill(ProjectTaskAccountBill curBillMaster);

        /// <summary>
        /// 根据审批通过的核算单回写前驱核算任务明细的核算工程量和形象进度
        /// </summary>
        /// <param name="accBillId"></param>
        void WriteBackAccTaskDtlQnyAndProgress(string accBillId);

        /// <summary>
        /// 删除工程任务耗用核算
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool DeleteProjectTaskAccountDtlUsage(IList list);

        void RollbackAccTaskDtlQnyAndProgress(IList billDetails);
        /// <summary>
        /// 是否需要工程量确认
        /// </summary>
        /// <param name="sProjectId"></param>
        /// <returns></returns>
        bool IsNeedProjectTaskAccount(string sProjectId);
    }
}
