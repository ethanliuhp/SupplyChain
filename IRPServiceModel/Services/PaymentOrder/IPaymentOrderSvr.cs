using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRPServiceModel.Domain.PaymentOrder;
using System.Collections;
using VirtualMachine.Core;

namespace IRPServiceModel.Services.PaymentOrder
{
  public  interface IPaymentOrderSvr
    {
      PaymentOrderMaster GetPaymentOrderById(string strId);
      /// <summary>
      /// 查询
      /// </summary>
      /// <param name="oQuery"></param>
      /// <returns></returns>
      IList<PaymentOrderMaster> Query(ObjectQuery oQuery);
      /// <summary>
      /// 删除对象集合
      /// </summary>
      /// <param name="list"></param>
      /// <returns></returns>
      bool Delete(IList lst);
      /// <summary>
      /// 删除对象
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      bool Delete(object master);
    
      bool SaveOrUpdate(object obj);
      bool SaveOrUpdate(IList lst);
      /// <summary>
      ///获取明细
      /// </summary>
      /// <param name="strID"></param>
      /// <returns></returns>
      PaymentOrderDetial GetPaymentOrderDetailById(string strID);
      /// <summary>
      /// 查询明细
      /// </summary>
      /// <param name="oQuery"></param>
      /// <returns></returns>
      IList<PaymentOrderDetial> QueryDetial(ObjectQuery oQuery);
    }
}
