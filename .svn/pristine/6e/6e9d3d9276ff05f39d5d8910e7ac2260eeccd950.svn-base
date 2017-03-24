using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Service
{
    /// <summary>
    /// 分包项目
    /// </summary>
    public interface IContractExcuteSrv : IBaseService
    {
        #region 分包项目
        /// <summary>
        /// 通过ID查询分包项目信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SubContractProject GetContractExcuteById(string id);
        /// <summary>
        /// 通过Code查询分包项目信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        SubContractProject GetContractExcuteByCode(string code);
        /// <summary>
        /// 查询分包项目信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetContractExcute(ObjectQuery objeceQuery);
        /// <summary>
        /// 分包项目查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet ContractExcuteQuery(string condition);
        SubContractProject SaveContractExcute(SubContractProject obj);
        IList GetContractDetailById(string ContractDtlId);
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        #endregion

        
    }




}
