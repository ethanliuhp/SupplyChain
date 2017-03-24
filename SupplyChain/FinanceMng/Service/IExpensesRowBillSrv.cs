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
    /// 费用划账单
    /// </summary>
    public interface IExpensesRowBillSrv : IBaseService
    {
        #region 费用划账单
        /// <summary>
        /// 通过ID查询费用划账单信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ExpensesRowBill GetExpensesRowBillById(string id);
        /// <summary>
        /// 通过Code查询费用划账单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ExpensesRowBill GetExpensesRowBillByCode(string code);
        /// <summary>
        /// 查询费用划账单信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetExpensesRowBill(ObjectQuery objeceQuery);
        /// <summary>
        /// 费用划账单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet ExpensesRowBillQuery(string condition);
        ExpensesRowBill SaveExpensesRowBill(ExpensesRowBill obj);

        #endregion

        
    }




}
