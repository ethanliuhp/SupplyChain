using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.FinanceMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.ExpensesRowBillMng
{
    public class MExpensesRowBillMng
    {
        private IExpensesRowBillSrv expensesRowBillSrv;

        public IExpensesRowBillSrv ExpensesRowBillSrv
        {
            get { return expensesRowBillSrv; }
            set { expensesRowBillSrv = value; }
        }

        public MExpensesRowBillMng()
        {
            if (expensesRowBillSrv == null)
            {
                expensesRowBillSrv = StaticMethod.GetService("ExpensesRowBillSrv") as IExpensesRowBillSrv;
            }
        }

        //#region 需求总计划
        ///// <summary>
        ///// 保存需求总计划
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        //public DelimitIndividualBill SaveDelimitIndividualBill(DelimitIndividualBill obj)
        //{
        //    return delimitIndividualBillSrv.SaveDelimitIndividualBill(obj);
        //}

        //#endregion

        //#region 料具收料
        //public MaterialCollectionMaster SaveMaterialCollection(MaterialCollectionMaster obj, IList list)
        //{
        //    //return matMngSrv.SaveMaterialCollectionMaster(obj, list);
        //}
        //#endregion
    }
}
