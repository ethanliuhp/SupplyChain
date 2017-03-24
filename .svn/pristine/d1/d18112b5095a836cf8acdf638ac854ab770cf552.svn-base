using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Iesi.Collections.Generic;
using System.Collections;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    //收款明细
    [Serializable]
    [Entity]
    public class GatheringDetail : BaseDetail
    {
        private decimal gatheringMoney;
        private decimal waterElecMoney;
        private decimal penaltyMoney;
        private decimal workerMoney;
        private decimal concreteMoney;
        private decimal agreementMoney;
        private decimal otherMoney;
        private decimal otherItemMoney;
        private AcceptanceBill acceptBillID;
        
        /// <summary>
        /// 收款金额
        /// </summary>
        virtual public decimal GatheringMoney
        {
            get { return gatheringMoney; }
            set { gatheringMoney = value; }
        }
        /// <summary>
        /// 代扣水电费
        /// </summary>
        virtual public decimal WaterElecMoney
        {
            get { return waterElecMoney; }
            set { waterElecMoney = value; }
        }

        /// <summary>
        /// 代扣罚款
        /// </summary>
        virtual public decimal PenaltyMoney
        {
            get { return penaltyMoney; }
            set { penaltyMoney = value; }
        }

        /// <summary>
        /// 代扣农民工保障金
        /// </summary>
        virtual public decimal WorkerMoney
        {
            get { return workerMoney; }
            set { workerMoney = value; }
        }

        /// <summary>
        /// 代扣散装水泥押金
        /// </summary>
        virtual public decimal ConcreteMoney
        {
            get { return concreteMoney; }
            set { concreteMoney = value; }
        }

        /// <summary>
        /// 代扣履约保证金
        /// </summary>
        virtual public decimal AgreementMoney
        {
            get { return agreementMoney; }
            set { agreementMoney = value; }
        }

        /// <summary>
        /// 代扣其他费用
        /// </summary>
        virtual public decimal OtherMoney
        {
            get { return otherMoney; }
            set { otherMoney = value; }
        }
        /// <summary>
        /// 代扣其他款项
        /// </summary>
        virtual public decimal OtherItemMoney
        {
            get { return otherItemMoney; }
            set { otherItemMoney = value; }
        }

        /// <summary>
        /// 关联票据ID
        /// </summary>
        virtual public AcceptanceBill AcceptBillID
        {
            get { return acceptBillID; }
            set { acceptBillID = value; }
        }
        
    }
}
