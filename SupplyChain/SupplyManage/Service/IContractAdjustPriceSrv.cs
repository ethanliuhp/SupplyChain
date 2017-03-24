using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.SupplyManage.ContractAdjustPriceManage.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using System.Collections;
using System.Data;

namespace Application.Business.Erp.SupplyChain.SupplyManage.Service
{
   

    /// <summary>
    /// 合同调价单
    /// </summary>
    public interface IContractAdjustPriceSrv : IBaseService
    {
        #region 合同调价单
        /// <summary>
        /// 通过ID查询合同调价信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ContractAdjustPrice GetContractAdjustPriceById(string id);
        /// <summary>
        /// 通过Code查询合同调价信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ContractAdjustPrice GetContractAdjustPriceByCode(string code);
        /// <summary>
        /// 查询合同调价信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetContractAdjustPrice(ObjectQuery objeceQuery);
        /// <summary>
        /// 合同调价查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet ContractAdjustPriceQuery(string condition);
        DataSet SelectContractAdjustPrice(string ContractNo,string MaterialCode);
        ContractAdjustPrice SaveContractAdjustPrice(ContractAdjustPrice obj);
        //ContractAdjustPrice UpdateContractAdjustPrice(ContractAdjustPrice obj);

        //int SubmitContractAdjustPrice(string forwordId, Decimal ContractPrice, string ID);
        SupplyOrderDetail GetSupplyOrderDetail(string id);
        ContractAdjustPrice saveaa(ContractAdjustPrice master, SupplyOrderDetail supplyOrderDetail);
        #endregion

        
    }




}
