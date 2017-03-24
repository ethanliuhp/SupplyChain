using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRPServiceModel.Domain.Basic;

namespace IRPServiceModel.Domain.PaymentOrder
{
    
    /// <summary>
    /// 付款单明细
    /// </summary>
    [Serializable]
    public class PaymentOrderDetial : BasicDetailBill
    {
       
        private string paymentItemName;
        private string describe;
        private decimal money;
        private PaymentOrderMaster master;
        
       /// <summary>
       /// 付款项名称
       /// </summary>
       public virtual string PaymentItemName
       {
           get { return paymentItemName; }
           set { paymentItemName = value; }
       }
       /// <summary>
       /// 描述
       /// </summary>
       public virtual string Describe
       {
           get { return describe; }
           set { describe = value; }
       }
       /// <summary>
       /// 金额
       /// </summary>
       public virtual decimal Money
       {
           get { return money; }
           set { money = value; }
       }
       public virtual PaymentOrderMaster Master
       {
           get { return master; }
           set { master = value; }
       }
    }
}
