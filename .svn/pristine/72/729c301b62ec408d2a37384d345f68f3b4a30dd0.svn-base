using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;

namespace Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Service
{
   

    /// <summary>
    /// 物资耗用结算单
    /// </summary>
    public interface IMaterialSettleSrv : IBaseService
    {
        #region 物资耗用结算单
        /// <summary>
        /// 通过ID查询物资耗用结算单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MaterialSettleMaster GetMaterialSettleById(string id);
        /// <summary>
        /// 通过Code查询物资耗用结算单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        MaterialSettleMaster GetMaterialSettleByCode(string code);
        /// <summary>
        /// 查询物资耗用结算单信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetMaterialSettle(ObjectQuery objeceQuery);
        /// <summary>
        /// 物资耗用结算单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet MaterialSettleQuery(string condition);
        MaterialSettleMaster SaveMaterialSettle(MaterialSettleMaster obj);
        MaterialSettleDetail GetMaterialSettleDetailById(string MaterialSettleDtlId);
        IList GetExcel(DataSet ds);
        #endregion

        
    }




}
