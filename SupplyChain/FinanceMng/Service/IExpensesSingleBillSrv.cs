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
    /// 费用报销单
    /// </summary>
    public interface IExpensesSingleBillSrv : IBaseService
    {
        #region 费用报销
        /// <summary>
        /// 通过ID查询费用报销信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ExpensesSingleBill GetExpensesSingleBillById(string id);
        /// <summary>
        /// 通过Code查询费用报销单信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ExpensesSingleBill GetExpensesSingleBillByCode(string code);
        /// <summary>
        /// 查询费用报销单信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetExpensesSingleBill(ObjectQuery objeceQuery);
        /// <summary>
        /// 费用报销单查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet ExpensesSingleBillQuery(string condition);
        ExpensesSingleBill SaveExpensesSingleBill(ExpensesSingleBill obj);

        #endregion

        
    }




}
