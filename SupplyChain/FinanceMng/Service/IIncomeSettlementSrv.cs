using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Service
{
   

    /// <summary>
    /// 当期收益结算单服务
    /// </summary>
    public interface IIncomeSettlementSrv : IBaseService
    {
        #region 当期收益结算单
        /// <summary>
        /// 通过ID查询当期收益结算单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IncomeSettlementMaster GetIncomeSettlementMasterById(string id);
        /// <summary>
        /// 通过Code查询当期收益结算单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        IncomeSettlementMaster GetIncomeSettlementMasterByCode(string code);
        /// <summary>
        /// 查询当期收益结算单信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetIncomeSettlementMaster(ObjectQuery objeceQuery);
        /// <summary>
        /// 当期收益结算单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet IncomeSettlementMasterQuery(string condition);
        IncomeSettlementMaster SaveIncomeSettlementMaster(IncomeSettlementMaster obj);

        IncomeSettlementDetail GetIncomeSettlementDetailById(string DemandDtlId);
        #endregion       
    }




}
