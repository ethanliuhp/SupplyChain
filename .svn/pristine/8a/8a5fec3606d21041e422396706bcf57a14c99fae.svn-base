using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.SettlementManagement.ExpensesSettleMng.Domain
{
    /// <summary>
    /// 费用结算主表
    /// </summary>
    [Serializable]
    public class ExpensesSettleMaster : BaseMaster
    {
        private int monthlyAccount;
        private string monthlySettlment;

        /// <summary>
        /// 月度核算标志
        /// </summary>
        virtual public int MonthlyAccount
        {
            get { return monthlyAccount; }
            set { monthlyAccount = value; }
        }
        /// <summary>
        /// 月度核算单GUID
        /// </summary>
        virtual public string MonthlySettlment
        {
            get { return monthlySettlment; }
            set { monthlySettlment = value; }
        }
    }
}
