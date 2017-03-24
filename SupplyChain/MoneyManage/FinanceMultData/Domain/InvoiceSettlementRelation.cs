using System;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
   [Serializable]
   [Entity]
   public class InvoiceSettlementRelation
   {
      private string _id;
      /// <summary></summary>
      public virtual string Id
      {
         get { return _id; }
         set { _id = value; }
      }

      private PaymentInvoice _invoice;
      /// <summary>付款发票</summary>
      public virtual PaymentInvoice Invoice
      {
         get { return _invoice; }
         set { _invoice = value; }
      }

      private decimal _invoiceMoney;
      /// <summary>发票关联金额</summary>
      public virtual decimal InvoiceMoney
      {
         get { return _invoiceMoney; }
         set { _invoiceMoney = value; }
      }

      private string _settlement;
      /// <summary>结算单Id</summary>
      public virtual string Settlement
      {
         get { return _settlement; }
         set { _settlement = value; }
      }

      private string _settlementCode;
      /// <summary>结算单编号</summary>
      public virtual string SettlementCode
      {
          get { return _settlementCode; }
          set { _settlementCode = value; }
      }

      private decimal _settlementMoney;
      /// <summary>结算关联金额</summary>
      public virtual decimal SettlementMoney
      {
         get { return _settlementMoney; }
         set { _settlementMoney = value; }
      }

      private decimal _totalSettlementMoney;
      /// <summary>结算单总金额</summary>
      public virtual decimal TotalSettlementMoney
      {
          get { return _totalSettlementMoney; }
          set { _totalSettlementMoney = value; }
      }

      private int _relationIndex;
      /// <summary>关联顺序</summary>
      public virtual int RelationIndex
      {
          get { return _relationIndex; }
          set { _relationIndex = value; }
      }
   }
}
