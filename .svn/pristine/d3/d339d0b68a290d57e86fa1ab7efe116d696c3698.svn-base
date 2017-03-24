using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
  [Serializable]
  [Entity]
  public  class SpecialCostDetail:BaseDetail
   {
       //private string  id;                                                          //guid
       private decimal currentPlanProgress;                             //当期计划工程形象进度
       private decimal currentPlanIncome;                               //当期计划收入
       private decimal currentRealProgress;                            //当期实际核算形象进度
       private decimal currentRealIncome;                               //当期实际收入
       private decimal currentRealPay;                                    //当期实际支出
       private int accountingYear;                                   //会计年
       private DateTime accountingStartDate;                          //会计期间开始时间
       private DateTime accountingEndDate;                           //会计期间结束时间
       private string accountingStyle;                                      //会计期类型
       private int accountingMonth;                                //会计月
       private string progressPlanId;                                      //进度计划GUID
       //private string mngCostId;                                            //专项管理费用GUID
     //    /// <summary>
     //   /// guid
     //   /// </summary>
     //virtual   public string Id
     //   {
     //     get { return id; }
     //     set { id = value; }
     //   }
       /// <summary>
       /// 当期计划工程形象进度
       /// </summary>
       virtual public decimal CurrentPlanProgress
       {
           get { return currentPlanProgress; }
           set { currentPlanProgress = value; }
       }       
         /// <summary>
       /// 当期计划收入
         /// </summary>
       virtual public decimal CurrentPlanIncome
       {
           get { return currentPlanIncome; }
           set { currentPlanIncome = value; }
       } 
         /// <summary>
       /// 当期实际核算形象进度
         /// </summary>
       virtual public decimal CurrentRealProgress
       {
           get { return currentRealProgress; }
           set { currentRealProgress = value; }
       } 
         /// <summary>
       /// 当期实际收入
         /// </summary>
       virtual public decimal CurrentRealIncome
       {
           get { return currentRealIncome; }
           set { currentRealIncome = value; }
       }  
         /// <summary>
       /// 当期实际支出
         /// </summary>
       virtual public decimal CurrentRealPay
       {
           get { return currentRealPay; }
           set { currentRealPay = value; }
       }
         /// <summary>
       /// 会计年
         /// </summary>
       virtual public int AccountingYear
       {
           get { return accountingYear; }
           set { accountingYear = value; }
       }
  
         /// <summary>
       /// 会计期间开始时间
         /// </summary>
       virtual public DateTime AccountingStartDate
       {
           get { return accountingStartDate; }
           set { accountingStartDate = value; }
       }
         /// <summary>
       /// 会计期间结束时间
         /// </summary>
       virtual public DateTime AccountingEndDate
       {
           get { return accountingEndDate; }
           set { accountingEndDate = value; }
       } 
         /// <summary>
       /// 会计期类型
         /// </summary>
       virtual public string AccountingStyle
       {
           get { return accountingStyle; }
           set { accountingStyle = value; }
       }
    
         /// <summary>
       /// 会计月
         /// </summary>
       virtual public int AccountingMonth
       {
           get { return accountingMonth; }
           set { accountingMonth = value; }
       }
         /// <summary>
       /// 进度计划GUID
         /// </summary>
       virtual public string ProgressPlanId
       {
           get { return progressPlanId; }
           set { progressPlanId = value; }
       }
       //  /// <summary>
       ///// 专项管理费用GUID
       //  /// </summary>
       //virtual public string MngCostId
       //{
       //    get { return mngCostId; }
       //    set { mngCostId = value; }
       //}
    }
}
