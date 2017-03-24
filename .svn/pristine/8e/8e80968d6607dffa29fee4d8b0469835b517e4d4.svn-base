using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.SettlementManagement.ExpensesSettleMng
{
    public class MExpensesSettleMng
    {
        private IExpensesSettleSrv expensesSettleSrv;

        public IExpensesSettleSrv ExpensesSettleSrv
        {
            get { return expensesSettleSrv; }
            set { expensesSettleSrv = value; }
        }

        public MExpensesSettleMng()
        {
            if (expensesSettleSrv == null)
            {
                expensesSettleSrv = StaticMethod.GetService("ExpensesSettleSrv") as IExpensesSettleSrv;
            }
        }

        #region 费用结算单
        /// <summary>
        /// 保存费用结算单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ExpensesSettleMaster SaveExpensesSettleMaster(ExpensesSettleMaster obj)
        {
            return expensesSettleSrv.SaveExpensesSettle(obj);
        }
        #endregion
    }
}
