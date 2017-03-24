using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Domain;

namespace Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Service
{
   

    /// <summary>
    /// 费用结算单
    /// </summary>
    public interface IExpensesSettleSrv : IBaseService
    {

        #region 费用结算
        /// <summary>
        /// 通过ID查询费用结算信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ExpensesSettleMaster GetExpensesSettleById(string id);
        /// <summary>
        /// 通过Code查询费用结算信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        ExpensesSettleMaster GetExpensesSettleByCode(string code);
        /// <summary>
        /// 查询费用结算信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetExpensesSettle(ObjectQuery objeceQuery);
        /// <summary>
        /// 费用结算查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        DataSet ExpensesSettleQuery(string condition);
        ExpensesSettleMaster SaveExpensesSettle(ExpensesSettleMaster obj);
        ExpensesSettleDetail GetExpensesSettleDetailById(string ExpensesSettleDtlId);
        IList GetExcel(DataSet ds);
        #endregion

        IList ObjectQuery(Type entityType, ObjectQuery oq);
    }




}
