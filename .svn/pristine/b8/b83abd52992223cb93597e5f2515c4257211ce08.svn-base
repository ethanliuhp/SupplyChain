using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;
using System.Collections;
using NHibernate.Criterion;
using NHibernate;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.Service
{
    public class CashFlowItemSrv : ICashFlowItemSrv 
    {
        private IDao dao;
        virtual public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        [TransManager]
        virtual public bool SaveItems(CashFlowItems Items)
        {
            bool isok = false;
            try
            {
                dao.Save(Items);
                
            }
            catch
            {
                throw;
            }
            return isok;
        }
        [TransManager]
        virtual public bool UpdateItems(CashFlowItems Items)
        {
            bool isok = false;
            try
            {
                dao.Update(Items);
                
            }
            catch
            {
                throw;
            }
            return isok;
        }
        [TransManager]
        virtual public bool DeleteItems(long itemId)
        {
            try
            {
                CashFlowItems itemsId = null;
                itemsId = dao.Get(typeof(CashFlowItems), itemId) as CashFlowItems;
                itemsId.State = 0;
                dao.Update(itemsId);
                return true;
            }
            catch
            {
                throw;
            }
        }
        virtual public IList ShowItems(ObjectQuery oq)
        {
            if (oq == null)
            {
                oq = new ObjectQuery();
                oq.AddFetchMode("ObjAccTitle", FetchMode.Eager);
                oq.AddCriterion (Expression.Eq ("State",1));
            }
            return dao.ObjectQuery(typeof (CashFlowItems ),oq);
        }
        virtual public CashFlowItems GetItems(long itemId)
        {
            CashFlowItems item = null;
            item = dao.Get(typeof(CashFlowItems), itemId) as CashFlowItems;
            return item;
        }

        virtual public IList SearchItems(string code, string name)
        {
            try
            {
                ObjectQuery oq = new ObjectQuery();
                if (code != string.Empty)
                {
                    oq.AddCriterion(Expression.Like("Code", "%" + code + "%"));
                }
                if (name != string.Empty)
                {
                    oq.AddCriterion(Expression.Like("Name", "%" + name + "%"));
                }
                oq.AddFetchMode("ObjAccTitle", FetchMode.Eager);
                oq.AddCriterion(Expression.Eq("State", 1));
                return dao.ObjectQuery(typeof(CashFlowItems), oq);
            }
            catch
            {
                throw;
            }

        }
    }
}
