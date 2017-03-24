using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.SupplyManage.Service
{
   

    /// <summary>
    /// 采购计划服务
    /// </summary>
    public interface ISupplyPlanSrv : IBaseService
    {

        #region 采购计划
        /// <summary>
        /// 通过ID查询采购计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SupplyPlanMaster GetSupplyPlanById(string id);
        /// <summary>
        /// 通过Code查询采购计划信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        SupplyPlanMaster GetSupplyPlanByCode(string code);
        /// <summary>
        /// 查询采购计划信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetSupplyPlan(ObjectQuery objeceQuery);
        /// <summary>
        /// 采购计划查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet SupplyPlanQuery(string condition);
        //SupplyPlanMaster SaveSupplyPlan(SupplyPlanMaster obj);
        SupplyPlanMaster SaveSupplyPlan(SupplyPlanMaster obj, IList movedDtlList);
        DemandMasterPlanDetail GetSupplyDetailById(string supplyPlanDtlId);

        bool DeleteSupplyPlan(SupplyPlanMaster obj);
        /// <summary>

        #endregion

        
    }




}
