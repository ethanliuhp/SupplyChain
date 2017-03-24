using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Domain;

namespace Application.Business.Erp.SupplyChain.ConstructionLogManage.PersonManagement.Service
{
   

    /// <summary>
    /// 管理人员日志信息服务
    /// </summary>
    public interface IPersonManageSrv : IBaseService
    {

        #region 管理人员日志信息
        /// <summary>
        /// 通过ID查询管理人员日志信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PersonManage GetPersonManageById(string id);
        /// <summary>
        /// 通过Code查询管理人员日志信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        PersonManage GetPersonManageByCode(string code);
        /// <summary>
        /// 查询管理人员日志信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetPersonManage(ObjectQuery objectQuery);
        /// <summary>
        /// 管理人员日志查询
        /// </summary>
        PersonManage SavePersonManage(PersonManage obj);
        /// <summary>
        /// 查询晴雨表信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetWeather(ObjectQuery objectQuery);
        #endregion

        
    }




}
