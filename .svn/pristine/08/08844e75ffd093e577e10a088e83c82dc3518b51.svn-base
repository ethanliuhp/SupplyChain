using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.SupplyManage.Service
{
   

    /// <summary>
    /// 需求总计划服务
    /// </summary>
    public interface IDemandPlanSrv : IBaseService
    {
        #region 需求总计划
        /// <summary>
        /// 通过ID查询需求总计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DemandMasterPlanMaster GetDemandMasterPlanById(string id);
        /// <summary>
        /// 通过Code查询需求总计划信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DemandMasterPlanMaster GetDemandMasterPlanByCode(string code);
        /// <summary>
        /// 查询需求总计划信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetDemandMasterPlan(ObjectQuery objeceQuery);
        /// <summary>
        /// 需求总计划查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet DemandMstPlanQuery(string condition);
        DemandMasterPlanMaster SaveDemandMasterPlan(DemandMasterPlanMaster obj);
        DemandMasterPlanMaster SaveDemandMasterPlan(DemandMasterPlanMaster obj, IList movedDtlList);
        DemandMasterPlanDetail GetDemandDetailById(string DemandDtlId);

        /// <summary>
        /// 需求总计划查询(公司)
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        DataSet DemandMstPlanQuery4Company(string condition);
        IList GetDemandDetailPlan(ObjectQuery oq);
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        DataSet Stkstockindtl_RealInQuantity(string condition);
        #endregion
        IList QuerySupplyProjectInfo();
        IList QuerySupplyCostInfo(string condition, string startDate, string endDate);
    }
}
