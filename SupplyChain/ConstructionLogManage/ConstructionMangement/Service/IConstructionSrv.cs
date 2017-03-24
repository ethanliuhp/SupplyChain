using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Domain;
using Application.Business.Erp.SupplyChain.Basic.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.ConstructionManagement.Service
{
   

    /// <summary>
    /// 施工日志信息服务
    /// </summary>
    public interface IConstructionSrv : IBaseService
    {

        #region 施工日志日志信息
        /// <summary>
        /// 通过ID查询施工日志信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ConstructionManage GetConstructionById(string id);
        /// <summary>
        /// 通过Code查询施工日志信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ConstructionManage GetConstructionByCode(string code);
        /// <summary>
        /// 查询施工日志信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetConstruction(ObjectQuery objectQuery);

        IList GetConstructionList(CurrentProjectInfo projectInfo, DateTime strDate);
        /// <summary>
        /// 施工日志查询
        /// </summary>
        ConstructionManage SaveConstruction(ConstructionManage obj);
        /// <summary>
        /// 查询晴雨表信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetWeather(ObjectQuery objectQuery);
        #endregion

        
    }




}
