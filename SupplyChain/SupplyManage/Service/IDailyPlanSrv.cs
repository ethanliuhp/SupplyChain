using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.SupplyManage.Service
{
   

    /// <summary>
    /// 需求总计划服务
    /// </summary>
    public interface IDailyPlanSrv : IBaseService
    {

        #region 日常需求计划
        /// <summary>
        /// 通过ID查询日常需求计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DailyPlanMaster GetDailyPlanById(string id);
        DailyPlanDetail GetDailyPlanDetail(string id);
        /// <summary>
        /// 通过Code查询日常需求计划信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DailyPlanMaster GetDailyPlanByCode(string code);
        /// <summary>
        /// 查询日常需求计划信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetDailyPlan(ObjectQuery objeceQuery);
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        /// <summary>
        /// 日常需求计划查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet DailyPlanQuery(string condition);
        DailyPlanMaster SaveDailyPlan(DailyPlanMaster obj);
        DailyPlanMaster SaveDailyPlan(DailyPlanMaster obj, IList movedDtlList);
         /// <summary>
        /// 查询日常需求计划某一物资数量
        /// </summary>
        /// <param name="sProjectID"></param>
        /// <param name="sMaterialID"></param>
        /// <returns></returns>
        decimal DailyPlanQuantity(string sProjectID, string sMaterialID,string  sSpecial,string  sSpecialType);
        DataSet DailyAudit(string condition);
        #endregion

        
    }




}
