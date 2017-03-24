using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    /// <summary>
    /// 财务综合账目明细
    /// </summary>
    [Serializable]
    [Entity]
   public class FinanceMultDataDetail:BaseDetail
    {
       private decimal subProjectPayout;
       private decimal personCost;
       private decimal materialCost;
       private decimal mechanicalCost;
       private decimal otherDirectCost;
       private decimal indirectCost;
       private decimal contractGrossProfit;
       private decimal materialRemain;
       private decimal tempDeviceRemain;
       private decimal lowValueConsumableRemain;
       private decimal exchangeMaterialRemain;
       private decimal financelCost;
       private decimal subBorrow;
       private decimal otherGatherMoney;
       private decimal otherPayoutMoney;
       private decimal profit;
       private decimal handInMoney;
       private decimal civilProjectBalance;
       private decimal setUpProjectBuild;
       private decimal civilAndSetUpBalance;
       private decimal civilAndSetUpPayout;
       private decimal setUpPayout;
       private decimal mainBusinessTax;
       private decimal busCostSure;
        /// <summary>
        /// 金额汇总
        /// </summary>
       public virtual decimal SumMoney
       {
           get
           {
               decimal dMoney = 0;
               dMoney += subProjectPayout;
               dMoney += personCost;
               dMoney += materialCost;
               dMoney += mechanicalCost;
               dMoney += otherDirectCost;
               dMoney += indirectCost;
               dMoney += contractGrossProfit;
               dMoney += materialRemain;
               dMoney += tempDeviceRemain;
               dMoney += lowValueConsumableRemain;
               dMoney += exchangeMaterialRemain;
               dMoney += financelCost;
               dMoney += subBorrow;
               dMoney += otherGatherMoney;
               dMoney += otherPayoutMoney;
               dMoney += profit;
               dMoney += handInMoney;
               dMoney += civilProjectBalance;
               dMoney += setUpProjectBuild;
               dMoney += civilAndSetUpBalance;
               dMoney += civilAndSetUpPayout;
               dMoney += setUpPayout;
               dMoney += mainBusinessTax;
               dMoney += BusCostSure;
               return dMoney;
           }
       }
       /// <summary>
       /// 分包项目支出
       /// </summary>
       public virtual decimal SubProjectPayout
       {
           get {return  subProjectPayout; }
           set { subProjectPayout = value; }
       }
       /// <summary>
       /// 人工费
       /// </summary>
       public virtual decimal PersonCost
       {
           get { return personCost; }
           set { personCost = value; }
       }
       /// <summary>
       /// 材料费
       /// </summary>
       public virtual decimal MaterialCost
       {
           get { return materialCost; }
           set { materialCost = value; }
       }
       /// <summary>
       /// 机械费
       /// </summary>
       public virtual decimal MechanicalCost
       {
           get { return mechanicalCost; }
           set { mechanicalCost = value; }
       }
       /// <summary>
       /// 其他直接费用
       /// </summary>
       public virtual decimal OtherDirectCost
       {
           get { return otherDirectCost; }
           set { otherDirectCost = value; }
       }
       /// <summary>
       /// 间接费用
       /// </summary>
       public virtual decimal IndirectCost
       {
           get { return indirectCost; }
           set { indirectCost = value; }
       }
       /// <summary>
       /// 合同毛利
       /// </summary>
       public virtual decimal ContractGrossProfit
       {
           get { return contractGrossProfit; }
           set { contractGrossProfit = value; }
       }
       /// <summary>
       /// 原材料余额
       /// </summary>
       public virtual decimal MaterialRemain
       {
           get { return materialRemain; }
           set { materialRemain = value; }
       }
       /// <summary>
       /// 临设余额
       /// </summary>
       public virtual decimal TempDeviceRemain
       {
           get { return tempDeviceRemain; }
           set { tempDeviceRemain = value; }
       }
       /// <summary>
       /// 低值易耗品余额
       /// </summary>
       public virtual decimal LowValueConsumableRemain
       {
           get { return lowValueConsumableRemain; }
           set { lowValueConsumableRemain = value; }
       }

       /// <summary>
       /// 周转材料余额
       /// </summary>
       public virtual decimal ExchangeMaterialRemain
       {
           get { return exchangeMaterialRemain; }
           set { exchangeMaterialRemain = value; }
       }
       /// <summary>
       /// 财务费用
       /// </summary>
       public virtual decimal FinancelCost
       {
           get { return financelCost; }
           set { financelCost = value; }
       }
       /// <summary>
       /// 局借款
       /// </summary>
       public virtual decimal SubBorrow
       {
           get { return subBorrow; }
           set { subBorrow = value; }
       }
       /// <summary>
       /// 其他应收款
       /// </summary>
       public virtual decimal OtherGatherMoney
       {
           get { return otherGatherMoney; }
           set { otherGatherMoney = value; }
       }
       /// <summary>
       /// 其他应付款
       /// </summary>
       public virtual decimal OtherPayoutMoney{
           set{otherPayoutMoney=value;}
           get{return otherPayoutMoney;}
       }
       /// <summary>
       /// 利润
       /// </summary>
       public virtual decimal Profit
       {
           get { return profit; }
           set { profit = value; }
       }
       /// <summary>
       /// 上交
       /// </summary>
       public virtual decimal HandInMoney
       {
           get { return handInMoney; }
           set { handInMoney = value; }
       }
       /// <summary>
       /// 土建工程结算
       /// </summary>
       public virtual decimal CivilProjectBalance
       {
           get { return civilProjectBalance; }
           set { civilProjectBalance = value; }
       }
       /// <summary>
       /// 安装工程施工
       /// </summary>
       public virtual decimal SetUpProjectBuild
       {
           get { return setUpProjectBuild; }
           set { setUpProjectBuild = value; }
       }
       /// <summary>
       /// 土建安装结算
       /// </summary>
       public virtual decimal CivilAndSetUpBalance
       {
           get { return civilAndSetUpBalance; }
           set { civilAndSetUpBalance = value; }
       }
       /// <summary>
       /// 土建安装付款
       /// </summary>
       public virtual decimal CivilAndSetUpPayout
       {
           get { return civilAndSetUpPayout; }
           set { civilAndSetUpPayout = value; }
       }
       /// <summary>
       /// 安装资金支出
       /// </summary>
       public virtual decimal SetUpPayout
       {
           get { return setUpPayout; }
           set { setUpPayout = value; }
       }
       /// <summary>
       /// 主营业务税费及其他
       /// </summary>
       public virtual decimal MainBusinessTax
       {
           get { return mainBusinessTax; }
           set { mainBusinessTax = value; }
       }
        /// <summary>
        /// 商务成本确认金额
        /// </summary>
       public virtual decimal BusCostSure
       {
           get { return busCostSure; }
            set {  busCostSure=value; }
       }

    }
}
