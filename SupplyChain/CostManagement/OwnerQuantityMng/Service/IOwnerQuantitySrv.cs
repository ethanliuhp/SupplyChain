using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Service
{
    /// <summary>
    /// 业主报量
    /// </summary>
    public interface IOwnerQuantitySrv : IBaseService
    {
        #region 业主报量
        /// <summary>
        /// 通过ID查询业主报量信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OwnerQuantityMaster GetOwnerQuantityById(string id);
        /// <summary>
        /// 通过Code查询业主报量信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        OwnerQuantityMaster GetOwnerQuantityByCode(string code);
        /// <summary>
        /// 查询业主报量信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetOwnerQuantity(ObjectQuery objeceQuery);
        IList GetOwner(ObjectQuery objectQuery);
        DataSet OwnerQuantity(string condition);
        /// <summary>
        /// 业主报量查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet OwnerQuantityQuery(string condition);
        DataSet OwnerQuantitySearch(string projectId, string qwbsGUID);
        OwnerQuantityMaster SaveOwnerQuantity(OwnerQuantityMaster obj);
        OwnerQuantity SaveOwner(OwnerQuantity obj);
        OwnerQuantityMaster UpdateOwnerQuantity(OwnerQuantityMaster obj);
        #endregion   
    }
}
