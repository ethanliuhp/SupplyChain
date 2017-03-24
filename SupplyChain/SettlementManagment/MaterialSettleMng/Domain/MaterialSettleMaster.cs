using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain
{
    /// <summary>
    /// 物资耗用结算主表
    /// </summary>
    [Serializable]
    public class MaterialSettleMaster : BaseMaster
    {
        private int monthlyAccount;
        private string monthlySettlment;
        private string settleState;
        private string monthAccountBill;
        /// <summary>
        /// 物资实际耗用标志
        /// </summary>
        virtual public int MonthlyAccount
        {
            get { return monthlyAccount; }
            set { monthlyAccount = value; }
        }
        /// <summary>
        /// 物资实际耗用GUID
        /// </summary>
        virtual public string MonthlySettlment
        {
            get { return monthlySettlment; }
            set { monthlySettlment = value; }
        }

        /// <summary>
        /// 月度成本核算GUID
        /// </summary>
        virtual public string MonthAccountBill
        {
            get { return monthAccountBill; }
            set { monthAccountBill = value; }
        }
        /// <summary>
        /// 结算单类型
        /// </summary>
        virtual public string SettleState
        {
            get { return settleState; }
            set { settleState = value; }
        }
    }
}
