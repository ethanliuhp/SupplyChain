using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.AcceptanceInspectionMng.Service
{
   

    /// <summary>
    /// 验收检查记录
    /// </summary>
    public interface IAcceptanceInspectionSrv : IBaseService
    {

        #region 验收检查记录
        /// <summary>
        /// 通过ID查询验收检查记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AcceptanceInspection GetAcceptanceInspectionById(string id);
        /// <summary>
        /// 通过检验批GUID查询验收检查记录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IList GetAcceptanceInspectionByInsLotGUID(InspectionLot id);
        /// <summary>
        /// 通过Code查询验收检查记录信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        AcceptanceInspection GetAcceptanceInspectionByCode(string code);
        /// <summary>
        /// 查询验收检查记录信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetAcceptanceInspection(ObjectQuery objeceQuery);
        /// <summary>
        /// 验收检查记录查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet AcceptanceInspectionQuery(string condition);
        AcceptanceInspection SaveAcceptanceInspection(AcceptanceInspection obj);
        #endregion

        
    }




}
