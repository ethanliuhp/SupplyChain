using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    public class IndirectCostMaster : BaseMaster
    {
        private int year;
        private int month;
        private DateTime endTime;
        private IndirectCostDetail borrowSymbol;
        private IndirectCostDetail profitSymbol;
        private IndirectCostDetail financeCostSymbol;
        private IndirectCostDetail handOnSymbol;
        private ISet<BaseDetail> indirectCost = new HashedSet<BaseDetail>();
        private ISet<BaseDetail> otherReceiveCost = new HashedSet<BaseDetail>();
        private ISet<BaseDetail> otherPayoutCost = new HashedSet<BaseDetail>();
        private ISet<BaseDetail> manageCost = new HashedSet<BaseDetail>();
        private int isSubCompany;
        /// <summary>
        /// 年份
        /// </summary>
        public virtual int Year
        {
            get { return year; }
            set { year = value; }
        }
        /// <summary>
        /// 月份
        /// </summary>
        public virtual int Month
        {
            get { return month; }
            set { month = value; }
        }
        /// <summary>
        /// 截至时间
        /// </summary>
        public virtual DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        /// <summary>
        /// 借款账面费用
        /// </summary>
        public virtual IndirectCostDetail BorrowSymbol
        {
            get {
                borrowSymbol = Details.OfType<IndirectCostDetail>().FirstOrDefault(item => (item.AccountSymbol == EnumAccountSymbol.借款标志));
                //if (borrowSymbol == null)
                //{
                //    borrowSymbol = new IndirectCostDetail() {Master=this,  AccountSymbol = EnumAccountSymbol.借款标志, CostType=EnumCostType.其他 };
                //    Details.Add(borrowSymbol);
                //}
                return borrowSymbol;
            }
            set {
                borrowSymbol = value; 
                IndirectCostDetail borrowSymbolTemp = Details.OfType<IndirectCostDetail>().FirstOrDefault(item => (item.AccountSymbol == EnumAccountSymbol.借款标志));
                if (borrowSymbolTemp != null)
                {
                    Details.Remove(borrowSymbolTemp);
                }
                borrowSymbol.Master = this;
                Details.Add(borrowSymbol);
                }
        }
        /// <summary>
        /// 利润账面费用
        /// </summary>
        public virtual IndirectCostDetail ProfitSymbol
        {
            get
            {
                profitSymbol = Details.OfType<IndirectCostDetail>().FirstOrDefault(item => (item.AccountSymbol == EnumAccountSymbol.利润标志));
                //if (profitSymbol == null)
                //{
                //    profitSymbol = new IndirectCostDetail() { Master = this, AccountSymbol = EnumAccountSymbol.利润标志, CostType = EnumCostType.其他 };
                //    Details.Add(profitSymbol);
                //}
                return profitSymbol;
            }
            set
            {
                profitSymbol = value;
                IndirectCostDetail profitSymbolTemp = Details.OfType<IndirectCostDetail>().FirstOrDefault(item => (item.AccountSymbol == EnumAccountSymbol.利润标志));
                if (profitSymbolTemp != null)
                {
                    Details.Remove(profitSymbolTemp);
                }
                profitSymbol.Master = this;
                Details.Add(profitSymbol);
            }
        }
        /// <summary>
        /// 财务账面费用
        /// </summary>
        public virtual IndirectCostDetail FinanceCostSymbol
        {
            get
            {
                financeCostSymbol = Details.OfType<IndirectCostDetail>().FirstOrDefault(item => (item.AccountSymbol == EnumAccountSymbol.财务费用标志));
                //if (financeCostSymbol == null)
                //{
                //    financeCostSymbol = new IndirectCostDetail() { Master = this, AccountSymbol = EnumAccountSymbol.财务费用标志, CostType = EnumCostType.其他 };
                //    Details.Add(financeCostSymbol);
                //}
                return financeCostSymbol;
            }
            set
            {
                financeCostSymbol = value;
                IndirectCostDetail financeCostSymbolTemp = Details.OfType<IndirectCostDetail>().FirstOrDefault(item => (item.AccountSymbol == EnumAccountSymbol.财务费用标志));
                if (financeCostSymbolTemp != null)
                {
                    Details.Remove(financeCostSymbolTemp);
                }
                financeCostSymbol.Master = this;
                Details.Add(financeCostSymbol);
            }
        }
        /// <summary>
        /// 上交账面费用
        /// </summary>
        public virtual IndirectCostDetail HandOnSymbol
        {
            get
            {
                handOnSymbol = Details.OfType<IndirectCostDetail>().FirstOrDefault(item => (item.AccountSymbol == EnumAccountSymbol.上交标志));
                //if (handOnSymbol == null)
                //{
                //    handOnSymbol = new IndirectCostDetail() { Master = this, AccountSymbol = EnumAccountSymbol.上交标志, CostType = EnumCostType.其他 };
                //    Details.Add(handOnSymbol);
                //}
                return handOnSymbol;
            }
            set
            {
                handOnSymbol = value;
                IndirectCostDetail handOnSymbolTemp = Details.OfType<IndirectCostDetail>().FirstOrDefault(item => (item.AccountSymbol == EnumAccountSymbol.上交标志));
                if (handOnSymbolTemp != null)
                {
                    Details.Remove(handOnSymbolTemp);
                }
                handOnSymbol.Master = this;
                Details.Add(handOnSymbol);
            }
        }
        /// <summary>
        /// 间接成本费用
        /// </summary>
        public virtual ISet<BaseDetail> IndirectCost
        {
            get {
                IEnumerable<IndirectCostDetail> indirectCostTemp = Details.OfType<IndirectCostDetail>().Where(item => item.CostType == EnumCostType.间接费用);
                if (indirectCost == null)
                {
                    indirectCost = new HashedSet<BaseDetail>();
                }
                indirectCost.Clear();
                foreach (IndirectCostDetail oDetail in indirectCostTemp)
                {
                    indirectCost.Add(oDetail);
                }
                return indirectCost; 
            }
            set
            {
                indirectCost = value;
                IEnumerable<IndirectCostDetail> indirectCostTemp = Details.OfType<IndirectCostDetail>().Where(item => item.CostType == EnumCostType.间接费用);
                foreach (IndirectCostDetail oDetail in indirectCostTemp)
                {
                    Details.Remove(oDetail);
                }
                foreach (IndirectCostDetail oDetail in indirectCost)
                {
                    Details.Add(oDetail);
                }
            }
        }
        /// <summary>
        /// 其他应收款
        /// </summary>
        public virtual ISet<BaseDetail> OtherReceiveCost
        {
            get {
                IEnumerable<IndirectCostDetail> otherReceiveCostTemp = Details.OfType<IndirectCostDetail>().Where(item => item.CostType == EnumCostType.其他应收);
                if (otherReceiveCost == null)
                {
                    otherReceiveCost = new HashedSet<BaseDetail>();
                }
                otherReceiveCost.Clear();
                foreach (IndirectCostDetail oDetail in otherReceiveCostTemp)
                {
                    otherReceiveCost.Add(oDetail);
                }
                return otherReceiveCost; 
            }
            set
            {
                otherReceiveCost = value;
                IEnumerable<IndirectCostDetail> otherReceiveCostTemp = Details.OfType<IndirectCostDetail>().Where(item => item.CostType == EnumCostType.其他应收);
                foreach (IndirectCostDetail oDetail in otherReceiveCostTemp)
                {
                    Details.Remove(oDetail);
                }
                foreach (IndirectCostDetail oDetail in otherReceiveCost)
                {
                    Details.Add(oDetail);
                }
            }
        }
        /// <summary>
        /// 其他应付款
        /// </summary>
        public virtual ISet<BaseDetail> OtherPayoutCost
        {
            get {
                IEnumerable<IndirectCostDetail>  otherPayoutCostTemp = Details.OfType<IndirectCostDetail>().Where(item => item.CostType == EnumCostType.其他应付);
                if (otherPayoutCost == null)
                {
                    otherPayoutCost = new HashedSet<BaseDetail>();
                }
                otherPayoutCost.Clear();
                foreach (IndirectCostDetail oDetail in otherPayoutCostTemp)
                {
                    otherPayoutCost.Add(oDetail);
                }
                return otherPayoutCost; 
            }
            set {
                otherPayoutCost = value;
                IEnumerable<IndirectCostDetail> otherPayoutCostTemp = Details.OfType<IndirectCostDetail>().Where(item => item.CostType == EnumCostType.其他应付);
                foreach (IndirectCostDetail oDetail in otherPayoutCostTemp)
                {
                    Details.Remove(oDetail);
                }
                foreach (IndirectCostDetail oDetail in otherPayoutCost)
                {
                    Details.Add(oDetail);
                }
             }
        }
       /// <summary>
       /// 管理费用
       /// </summary>
        public virtual ISet<BaseDetail> ManageCost
        {
            get
            {
                IEnumerable<IndirectCostDetail> manageCostTemp = Details.OfType<IndirectCostDetail>().Where(item => item.CostType == EnumCostType.管理费用);
                if (manageCost == null)
                {
                    manageCost = new HashedSet<BaseDetail>();
                }
                manageCost.Clear();
                foreach (IndirectCostDetail oDetail in manageCostTemp)
                {
                    manageCost.Add(oDetail);
                }
                return manageCost;
            }
            set {
                manageCost = value;
                IEnumerable<IndirectCostDetail> manageCostTemp = Details.OfType<IndirectCostDetail>().Where(item => item.CostType == EnumCostType.管理费用);
                foreach (IndirectCostDetail oDetail in manageCostTemp)
                {
                    Details.Remove(oDetail);
                }
                foreach (IndirectCostDetail oDetail in manageCost)
                {
                    Details.Add(oDetail);
                }
                 }
        }
        /// <summary>
        /// 是否是分公司
        /// </summary>
        public virtual int IsSubCompany
        {
            get { return isSubCompany; }
            set { isSubCompany = value; }
        }
    }
}
