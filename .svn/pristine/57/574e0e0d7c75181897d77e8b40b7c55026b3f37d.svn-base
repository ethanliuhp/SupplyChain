using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRPServiceModel.Domain.PaymentOrder;
using NHibernate.Criterion;
using System.Collections;
using VirtualMachine.Core;

namespace IRPServiceModel.Services.PaymentOrder
{
   public  class PaymentOrderSvr : IPaymentOrderSvr
    {
       private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }
        public PaymentOrderMaster GetPaymentOrderById(string strId)
        {
            //PaymentOrderMaster oPaymentOrderMaster;
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", strId));
            oQuery.AddFetchMode("Payee", NHibernate.FetchMode.Eager);
            oQuery.AddFetchMode("Details", NHibernate.FetchMode.Eager);
            IList<PaymentOrderMaster> lst = Query(oQuery);
            return (lst != null && lst.Count > 0) ? lst[0]  : null;
        }
        public IList<PaymentOrderMaster> Query(ObjectQuery oQuery)
        {
            IList<PaymentOrderMaster> lstResult = new List<PaymentOrderMaster>();
            IList lst = Dao.ObjectQuery(typeof(PaymentOrderMaster), oQuery);
            foreach (PaymentOrderMaster item in lst)
            {
                lstResult.Add(item);
            }
            return lstResult;
        }
        public IList< PaymentOrderDetial> QueryDetial(ObjectQuery oQuery)
        {
            IList<PaymentOrderDetial> lstResult = new List<PaymentOrderDetial>();
            IList lst = Dao.ObjectQuery(typeof(PaymentOrderDetial), oQuery);
            foreach (PaymentOrderDetial item in lst)
            {
                lstResult.Add(item);
            }
            return lstResult;
        }
        [TransManager]
        public bool Delete(IList list)
        {
            return dao.Delete(list);
        }
        
        [TransManager]
        public bool Delete(object obj)
        {
            return dao.Delete(obj);
        }
       
        [TransManager]
        public bool SaveOrUpdate(object obj)
        {
            return dao.SaveOrUpdate(obj);
        }
        [TransManager]
        public bool SaveOrUpdate(IList lst)
        {
            return dao.SaveOrUpdate(lst);
        }
        public PaymentOrderDetial GetPaymentOrderDetailById(string strID)
        {
            ObjectQuery oQuery = new ObjectQuery();
            oQuery.AddCriterion(Expression.Eq("Id", strID));
            oQuery.AddFetchMode("Master", NHibernate.FetchMode.Eager);
            IList lst = dao.ObjectQuery(typeof(PaymentOrderDetial),oQuery);
            return lst == null || lst.Count == 0 ? null : lst[0] as PaymentOrderDetial;
        }
    }
}
