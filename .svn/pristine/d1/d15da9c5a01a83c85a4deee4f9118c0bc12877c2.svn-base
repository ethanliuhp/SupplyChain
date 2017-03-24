using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using VirtualMachine.Core;
using VirtualMachine.Core.Expression;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service;
using Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Domain;
using Application.Business.Erp.SupplyChain.BasicData.ExpenseItemMng.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.BasicData.ExpenseItemMng
{
    public class MExpenseItem
    {
        public IExpenseItemSrv saleBudgeSrv = null;
        public IList expenseItemList=new ArrayList();

        public MExpenseItem()
        {
            if (saleBudgeSrv == null)
            {
                saleBudgeSrv = StaticMethod.GetService("ExpenseItemSrv") as IExpenseItemSrv;
            }
        }

        public void New()
        {
        }

        public IList Save()
        {
            return saleBudgeSrv.SaveOrUpdateByDao(expenseItemList);
        }

        public void FindAll()
        {
           // expenseItemList = saleBudgeSrv.FindAll(typeof(ExpenseItem));
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("AccountTitle", NHibernate.FetchMode.Eager);
            expenseItemList = saleBudgeSrv.GetDomain(typeof(ExpenseItem), oq);
        }

        public void Delete(ExpenseItem item)
        {
            saleBudgeSrv.DeleteItem(item);
        }
    }
}
