using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.WeatherManage.Service
{
   

    /// <summary>
    /// 晴雨表信息服务
    /// </summary>
    public interface IWeatherSrv : IBaseService
    {

        #region 晴雨表信息
        /// <summary>
        /// 通过ID查询晴雨表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        WeatherInfo GetWeatherById(string id);
        /// <summary>
        /// 通过Code查询晴雨表信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        WeatherInfo GetWeatherByCode(string code);
        /// <summary>
        /// 查询晴雨表信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetWeather(ObjectQuery objectQuery);
        /// <summary>
        /// 晴雨表查询
        /// </summary>
        WeatherInfo SaveWeather(WeatherInfo obj);
        #endregion

        
    }




}
