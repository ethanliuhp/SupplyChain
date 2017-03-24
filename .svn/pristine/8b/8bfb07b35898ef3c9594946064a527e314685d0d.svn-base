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
    /// 材料结算单服务
    /// </summary>
    public interface IMaterialSettlementSrv : IBaseService
    {
        #region 材料结算单
        /// <summary>
        /// 通过ID查询材料结算单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MaterialSettlementMaster GetMaterialSettlementMasterById(string id);
        /// <summary>
        /// 通过Code查询材料结算单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MaterialSettlementMaster GetMaterialSettlementMasterByCode(string code);
        /// <summary>
        /// 查询材料结算单信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetMaterialSettlementMaster(ObjectQuery objeceQuery);
        /// <summary>
        /// 材料结算单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet MaterialSettlementMasterQuery(string condition);
        MaterialSettlementMaster SaveMaterialSettlementMaster(MaterialSettlementMaster obj);

        MaterialSettlementDetail GetMaterialSettlementDetailById(string DemandDtlId);
        #endregion

        
    }




}
