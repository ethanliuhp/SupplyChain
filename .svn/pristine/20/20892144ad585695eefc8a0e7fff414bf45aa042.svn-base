using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.BasicData.UnitMng.Domain;

namespace Application.Business.Erp.SupplyChain.BasicData.Service
{
   

    /// <summary>
    /// 计量单位服务
    /// </summary>
    public interface IUnitSrv : IBaseService
    {

        #region 计量单位
        /// <summary>
        /// 通过ID查询计量单位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UnitMaster GetUnitById(string id);

        /// <summary>
        /// 通过BillTypeName查询计量单位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UnitMaster GetUnitBillTypeNameById(string BillTypeName);
        /// <summary>
        /// 通过Code查询计量单位信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        UnitMaster GetUnitByCode(string code);
        /// <summary>
        /// 查询计量单位信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetUnit(ObjectQuery objeceQuery);
        /// <summary>
        /// 计量单位查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet UnitQuery(string condition);
        UnitMaster SaveUnit(UnitMaster obj);

        #endregion

        
    }




}
