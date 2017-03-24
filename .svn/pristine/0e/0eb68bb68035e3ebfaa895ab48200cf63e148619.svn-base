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
    /// 临建摊销单服务
    /// </summary>
    public interface IOverlayAmortizeSrv : IBaseService
    {
        #region 临建摊销单
        /// <summary>
        /// 通过ID查询临建摊销单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OverlayAmortizeMaster GetOverlayAmortizeMasterById(string id);
        /// <summary>
        /// 通过Code查询临建摊销单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        OverlayAmortizeMaster GetOverlayAmortizeMasterByCode(string code);
        /// <summary>
        /// 查询临建摊销单信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetOverlayAmortizeMaster(ObjectQuery objeceQuery);
        /// <summary>
        /// 临建摊销单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet OverlayAmortizeMasterQuery(string condition);
        OverlayAmortizeMaster SaveOverlayAmortizeMaster(OverlayAmortizeMaster obj);

        OverlayAmortizeDetail GetOverlayAmortizeDetailById(string DemandDtlId);
        #endregion

        
    }




}
