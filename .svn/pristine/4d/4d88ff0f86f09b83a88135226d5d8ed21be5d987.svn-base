using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    /// <summary>
    /// 财务综合账目主表
    /// </summary>
    [Serializable]
    [Entity]
    public class FinanceMultDataMaster : BaseMaster
    {
        private int year;
        private int month;
        private AccountType accountType;
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
        /// 账面类型
        /// </summary>
        public virtual AccountType AccountType
        {
            get { return  accountType; }
            set { accountType = value; }
        }
         
    }
    /// <summary>
    /// 账面类型
    /// </summary>
    public enum AccountType
    {
        公司 = 0, 项目 = 1, 分公司 = 2

    }
}
