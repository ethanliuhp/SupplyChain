using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;
namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
   [Serializable]
   [Entity]
   public class OfficeFundPlanPayDetail : BaseDetail
   {
      private string _costDetail;
      /// <summary>费用明细</summary>
      virtual public string CostDetail
      {
         get { return _costDetail; }
         set { _costDetail = value; }
      }

      private decimal _planDeclarePayment;
      /// <summary>计划申报支出</summary>
      virtual public decimal PlanDeclarePayment
      {
         get { return _planDeclarePayment; }
         set { _planDeclarePayment = value; }
      }

      private decimal _orderNumber;
      /// <summary>序号</summary>
      virtual public decimal OrderNumber
      {
         get { return _orderNumber; }
         set { _orderNumber = value; }
      }

      private string _payScope;
      /// <summary>支出范围</summary>
      virtual public string PayScope
      {
         get { return _payScope; }
         set { _payScope = value; }
      }

      private decimal _quota;

      /// <summary>分配额</summary>
      public virtual decimal Quota
      {
          get { return _quota; }
          set { _quota = value; }
      }
   }
}
