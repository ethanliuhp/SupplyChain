using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.CostManagement.EngineerChangeMng.Domain;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.CostManagement.Service
{
   

    /// <summary>
    /// 工程更改管理服务
    /// </summary>
    public interface IEngineerChangeSrv : IBaseService
    {
        IDao Dao { get; set; }

        #region 工程更改管理
        /// <summary>
        /// 通过ID查工程更改计划信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EngineerChangeMaster GetEngineerChangeById(string id);
        /// <summary>
        /// 通过Code查询工程更改计划信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        EngineerChangeMaster GetEngineerChangeByCode(string code);
        /// <summary>
        /// 查询工程更改计划信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetEngineerChange(ObjectQuery objeceQuery);
        /// <summary>
        /// 工程更改计划查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet EngineerChangeQuery(string condition);
        EngineerChangeMaster SaveEngineerChange(EngineerChangeMaster obj);
        #endregion

        
    }




}
