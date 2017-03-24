using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionReport.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionReport.Service
{
   

    /// <summary>
    /// 日施工情况
    /// </summary>
    public interface IConstructionReportSrv : IBaseService
    {

        #region 日施工情况
        /// <summary>
        /// 通过ID查询日施工情况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ConstructReport GetConstructReportById(string id);
        /// <summary>
        /// 通过Code查询日施工情况
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ConstructReport GetConstructReportByCode(string code);
        /// <summary>
        /// 查询日施工情况
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetConstructReport(ObjectQuery objectQuery);

        IList GetConstructReportList(CurrentProjectInfo projectInfo,DateTime dt);
        /// <summary>
        /// 日施工情况查询
        /// </summary>
        ConstructReport SaveConstructReport(ConstructReport obj);
        /// <summary>
        /// 查询晴雨表信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetWeather(ObjectQuery objectQuery);
        #endregion

        
    }




}
