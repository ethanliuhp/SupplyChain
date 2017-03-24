using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Domain
{
    /// <summary>
    /// 费用报销单
    /// </summary>
    [Serializable]
    public class ExpensesSingleBill : BaseMaster
    {
        private DateTime expensesDate;
        private string expensesPerson;
        private string expensesType;
        private string purpose;
        private Double totalMoney;
        private Double money;

        /// <summary>
        /// 报销日期
        /// </summary>
        virtual public DateTime ExpensesDate
        {
            get { return expensesDate; }
            set { expensesDate = value; }
        }
        /// <summary>
        /// 报销人
        /// </summary>
        virtual public string ExpensesPerson
        {
            get { return expensesPerson; }
            set { expensesPerson = value; }
        }
        /// <summary>
        /// 费用类型
        /// </summary>
        virtual public string ExpensesType
        {
            get { return expensesType; }
            set { expensesType = value; }
        }
        /// <summary>
        /// 用途
        /// </summary>
        virtual public string Purpose
        {
            get { return purpose; }
            set { purpose = value; }
        }
        /// <summary>
        /// 报销金额
        /// </summary>
        virtual public Double TotalMoney
        {
            get { return totalMoney; }
            set { totalMoney = value; }
        }
        /// <summary>
        /// 冲借款金额
        /// </summary>
        virtual public Double Money
        {
            get { return money; }
            set { money = value; }
        }
    }
}
