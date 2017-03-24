using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.Service
{
   

    /// <summary>
    /// 检验批服务
    /// </summary>
    public interface IInspectionLotSrv : IBaseService
    {

        #region 检验批
        /// <summary>
        /// 通过ID查询检验批信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        InspectionLot GetInspectionLotById(string id);
        /// <summary>
        /// 通过Code查询检验批信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        InspectionLot GetInspectionLotByCode(string code);
        /// <summary>
        /// 查询检验批信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetInspectionLot(ObjectQuery objeceQuery);
        /// <summary>
        /// 检验批查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet InspectionLotQuery(string condition);
        InspectionLot SaveInspectionLot(InspectionLot obj);
        #endregion

        
    }




}
