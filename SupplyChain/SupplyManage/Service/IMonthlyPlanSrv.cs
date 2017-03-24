using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.SupplyManage.Service
{
   

    /// <summary>
    /// 月度需求计划服务
    /// </summary>
    public interface IMonthlyPlanSrv : IBaseService
    {
        #region 月度需求计划
        /// <summary>
        /// 通过ID查询月度需求计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MonthlyPlanMaster GetMonthlyPlanById(string id);
        /// <summary>
        /// 通过Code查询月度需求计划信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MonthlyPlanMaster GetMonthlyPlanByCode(string code);
        /// <summary>
        /// 查询月度需求计划信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetMonthlyPlan(ObjectQuery objeceQuery);
        /// <summary>
        /// 月度需求计划查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet MonthlyPlanQuery(string condition);
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        MonthlyPlanMaster SaveMonthlyPlan(MonthlyPlanMaster obj);
        MonthlyPlanMaster SaveMonthlyPlan(MonthlyPlanMaster obj, IList movedDtlList);
        DataSet MonthAudit(string billId);
        #endregion

        
    }




}
