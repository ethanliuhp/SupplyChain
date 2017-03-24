using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalContractMng.Domain
{
    /// <summary>
    /// 设备租赁合同单主表
    /// </summary>
    [Serializable]
    public class MaterialRentalContractMaster : BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;

        private StandardUnit priceUnit;
        private string priceUnitName;
        private DateTime startDate = ClientUtil.ToDateTime("1900-1-1");
        private DateTime endDate = ClientUtil.ToDateTime("1900-1-1");
        private decimal sumMoney;

        private string monthAccountBillId;
        private decimal processPayRate;
        private decimal completePayRate;
        private decimal warrantyPayRate;
        private string balanceStyle;

        //结算完成情况
        public virtual string BalanceStyle
        {
            get { return balanceStyle; }
            set { balanceStyle = value; }
        }

        /// <summary>
        /// 过程结算付款比例
        /// </summary>
        public virtual decimal ProcessPayRate
        {
            get { return processPayRate; }
            set { processPayRate = value; }
        }

        /// <summary>
        /// 完工结算付款比例
        /// </summary>
        public virtual decimal CompletePayRate
        {
            get { return completePayRate; }
            set { completePayRate = value; }
        }

        /// <summary>
        /// 质保期付款比例
        /// </summary>
        public virtual decimal WarrantyPayRate
        {
            get { return warrantyPayRate; }
            set { warrantyPayRate = value; }
        }

        /// <summary>
        /// 金额汇总
        /// </summary>
        virtual public decimal SumMoney
        {
            get
            {
                decimal temMoney = 0;
                //汇总
                foreach (MaterialRentalContractDetail var in Details)
                {
                    temMoney += var.SettleMoney;
                }
                sumMoney = temMoney;
                return sumMoney;
            }
            set
            {
                sumMoney = value;
            }
        }
        /// <summary>
        /// 月度成本核算GUID
        /// </summary>
        virtual public string MonthAccountBillId
        {
            get { return monthAccountBillId; }
            set { monthAccountBillId = value; }
        }
        /// <summary>
        /// 供应商
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        ///<summary>
        /// 价格单位
        ///</summary>
        virtual public StandardUnit PriceUnit
        {
            get { return priceUnit; }
            set { priceUnit = value; }
        }
        ///<summary>
        ///价格单位名称
        ///</summary>
        virtual public string PriceUnitName
        {
            get { return priceUnitName; }
            set { priceUnitName = value; }
        }
        ///<summary>
        ///起始时间
        ///</summary>
        virtual public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }
        ///<summary>
        ///终止时间
        ///</summary>
        virtual public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

    }
}
