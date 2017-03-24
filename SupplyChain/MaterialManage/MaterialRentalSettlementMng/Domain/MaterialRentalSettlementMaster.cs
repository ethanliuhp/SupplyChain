using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain
{
    /// <summary>
    /// 设备租赁结算单主表
    /// </summary>
    [Serializable]
    public class MaterialRentalSettlementMaster : BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;

        private StandardUnit priceUnit;
        private string priceUnitName;
        private DateTime startDate = ClientUtil.ToDateTime("1900-1-1");
        private DateTime endDate = ClientUtil.ToDateTime("1900-1-1");
        private decimal sumMoney;

        private string monthAccountBillId;

        /// <summary>
        /// 金额汇总
        /// </summary>
        virtual public decimal SumMoney
        {
            get
            {
                decimal temMoney = 0;
                //汇总
                foreach (MaterialRentalSettlementDetail var in Details)
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
