using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
   

    /// <summary>
    /// 劳务需求计划服务
    /// </summary>
    public interface ILaborDemandPlanSrv : IBaseService
    {

        #region 劳务需求计划
        /// <summary>
        /// 通过ID查询劳务需求计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LaborDemandPlanMaster GetLaborDemandPlanById(string id);
        /// <summary>
        /// 通过Code查询劳务需求计划信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        LaborDemandPlanMaster GetLaborDemandPlanByCode(string code);
        /// <summary>
        /// 查询劳务需求计划信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetLaborDemandPlan(ObjectQuery objeceQuery);
        /// <summary>
        /// 劳务需求计划查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet LaborDemandPlanQuery(string condition);
        LaborDemandPlanMaster AddLaborDemandPlan(LaborDemandPlanMaster obj);
        LaborDemandPlanMaster SaveLaborDemandPlan(LaborDemandPlanMaster obj);

        IList GetWorkTypeByParentId(string id);
        #endregion

        
    }




}
