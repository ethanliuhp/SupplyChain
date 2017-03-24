using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Service
{
    /// <summary>
    /// 个人借款单
    /// </summary>
    public interface IDelimitIndividualBillSrv : IBaseService
    {
        #region 个人借款单
        /// <summary>
        /// 通过ID查询个人借款单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DelimitIndividualBill GetDelimitIndividualBillById(string id);
        /// <summary>
        /// 通过Code查询个人借款单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DelimitIndividualBill GetDelimitIndividualBillByCode(string code);
        /// <summary>
        /// 查询个人借款单信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetDelimitIndividualBill(ObjectQuery objeceQuery);
        /// <summary>
        /// 个人借款单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet DelimitIndividualBillQuery(string condition);
        DelimitIndividualBill SaveDelimitIndividualBill(DelimitIndividualBill obj);

        #endregion

        
    }




}
