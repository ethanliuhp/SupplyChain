using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Service;
using Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.ResourceManager.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ProjectTaskAccountMng
{
    public class MProjectTaskAccount
    {
        private static IProjectTaskAccountSrv mm;

        public MProjectTaskAccount()
        {
            if (mm == null)
                mm = ConstMethod.GetService("ProjectTaskAccountSrv") as IProjectTaskAccountSrv;
        }

        /// <summary>
        /// 获取工程任务核算单编号
        /// </summary>
        /// <returns></returns>
        public string GetProjectTaskAccountCode()
        {
            return mm.GetProjectTaskAccountCode();
        }
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return mm.ObjectQuery(entityType, oq);
        }
        public IList SaveOrUpdate(IList lst)
        {
            return mm.SaveOrUpdate(lst);
        }
        /// <summary>
        /// 获取工程任务核算单明细编号
        /// </summary>
        /// <returns></returns>
        public string GetProjectTaskAccountDetailCode(string contractGroupCode, int detailNum)
        {
            return mm.GetProjectTaskAccountDetailCode(contractGroupCode, detailNum);
        }

        /// <summary>
        /// 保存或修改工程任务核算单集合
        /// </summary>
        /// <param name="list">工程任务核算单集合</param>
        /// <returns></returns>
        public IList SaveOrUpdateProjectTaskAccount(IList list)
        {
            return mm.SaveOrUpdateProjectTaskAccount(list);
        }

        /// <summary>
        /// 保存或修改工程任务核算单
        /// </summary>
        /// <param name="bill">工程任务核算单</param>
        /// <returns></returns>
        public ProjectTaskAccountBill SaveOrUpdateProjectTaskAccount(ProjectTaskAccountBill bill)
        {
            return mm.SaveOrUpdateProjectTaskAccount(bill);
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
        /// 删除工程任务核算单集合
        /// </summary>
        /// <param name="list">工程任务核算单集合</param>
        /// <returns></returns>
        public bool DeleteProjectTaskAccount(IList list)
        {
            return mm.DeleteProjectTaskAccount(list);
        }

        /// <summary>
        /// 工程任务核算单明细集合
        /// </summary>
        /// <param name="list">工程任务核算单明细集合</param>
        /// <returns></returns>
        public bool DeleteProjectTaskAccountDetail(IList list)
        {
            return mm.DeleteProjectTaskAccountDetail(list);
        }

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerTime()
        {
            return mm.GetServerTime();
        }

        /// <summary>
        /// 保存工程任务明细核算和汇总信息
        /// </summary>
        /// <param name="dtl">工程任务明细核算</param>
        /// <param name="sum">工程任务明细汇总</param>
        /// <returns></returns>
        public bool SaveProjectTaskDetailAccountAndSummary(ProjectTaskDetailAccount dtl, ProjectTaskDetailAccountSummary sum)
        {
            return mm.SaveProjectTaskDetailAccountAndSummary(dtl, sum);
        }

        /// <summary>
        /// 保存核算明细并反写确认明细数据
        /// </summary>
        /// <param name="bill">核算单</param>
        /// <param name="listConfigDetail">确认明细集合</param>
        /// <returns></returns>
        public ProjectTaskAccountBill SaveProjectTaskAndSetConfirmState(ProjectTaskAccountBill bill, IList listConfirmDetail)
        {
            return mm.SaveProjectTaskAndSetConfirmState(bill, listConfirmDetail);
        }

        /// <summary>
        /// 生成工程任务核算单
        /// </summary>
        /// <param name="curBillMaster"></param>
        /// <returns>1.核算单，2.要回写数据的确认单明细，3.异常/提示消息</returns>
        public IList GenAccountBill(ProjectTaskAccountBill curBillMaster)
        {
            return mm.GenAccountBill(curBillMaster);
        }

        /// <summary>
        /// 生成工程任务核算单(包括虚拟工单)
        /// </summary>
        /// <param name="curBillMaster"></param>
        /// <returns>1.核算单，2.要回写数据的确认单明细，3.异常/提示消息</returns>
        public IList GenAccountBillByVirConfirmBill(ProjectTaskAccountBill curBillMaster)
        {
            return mm.GenAccountBillByVirConfirmBill(curBillMaster);
        }

        /// <summary>
        /// 保存核算单并反写确认明细数据（包括虚拟工单）
        /// </summary>
        /// <param name="bill">核算单</param>
        /// <param name="listConfigDetail">确认明细集合</param>
        /// <returns></returns>
        public ProjectTaskAccountBill SaveAccBillAndSetCfmStateByVirCfmBill(ProjectTaskAccountBill bill, IList listConfirmDetail)
        {
            return mm.SaveAccBillAndSetCfmStateByVirCfmBill(bill, listConfirmDetail);
        }

        /// <summary>
        /// 删除工程任务耗用核算
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool DeleteProjectTaskAccountDtlUsage(IList list)
        {
            return mm.DeleteProjectTaskAccountDtlUsage(list);
        }
    }
}
