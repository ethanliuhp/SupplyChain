using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalContractMng.Domain
{
    /// <summary>
    /// 设备来源
    /// </summary>
    
    public enum MaterialResource
    {
        内部租赁,
        外部租赁
    }
    /// <summary>
    /// 设备租赁合同明细
    /// </summary>
    [Serializable]
    public class MaterialRentalContractDetail : BaseDetail
    {
        private StandardUnit priceUnit;
        private string priceUnitName;
        private StandardUnit dateUnit;
        private string dateUnitName;
        private StandardUnit quantityUnit;
        private string quantityUnitName;
        private string materialSource;

        //private Iesi.Collections.Generic.ISet<MaterialSubjectDetail> materialSubjectDetails = new Iesi.Collections.Generic.HashedSet<MaterialSubjectDetail>();
        //virtual public Iesi.Collections.Generic.ISet<MaterialSubjectDetail> MaterialSubjectDetails
        //{
        //    get { return materialSubjectDetails; }
        //    set { materialSubjectDetails = value; }
        //}

        ///// <summary>
        ///// 增加费用明细
        ///// </summary>
        ///// <param name="detail"></param>
        //virtual public void AddSubjectDetail(MaterialSubjectDetail detail)
        //{
        //    detail.MasterCost = this;
        //    MaterialSubjectDetails.Add(detail);
        //}
        ///// <summary>
        ///// 设备来源
        ///// </summary>
        //virtual public string MaterialSource
        //{
        //    get { return materialSource; }
        //    set { materialSource = value; }
        //}
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
        /// 时间单位
        ///</summary>
        virtual public StandardUnit DateUnit
        {
            get { return dateUnit; }
            set { dateUnit = value; }
        }
        ///<summary>
        ///时间单位名称
        ///</summary>
        virtual public string DateUnitName
        {
            get { return dateUnitName; }
            set { dateUnitName = value; }
        }
        ///<summary>
        /// 数量单位
        ///</summary>
        virtual public StandardUnit QuantityUnit
        {
            get { return quantityUnit; }
            set { quantityUnit = value; }
        }
        ///<summary>
        ///数量单位名称
        ///</summary>
        virtual public string QuantityUnitName
        {
            get { return quantityUnitName; }
            set { quantityUnitName = value; }
        }
        private decimal settleMoney;
        ///<summary>
        ///结算合价
        ///</summary>
        virtual public decimal SettleMoney
        {
            get { return settleMoney; }
            set { settleMoney = value; }
        }
        private DateTime startSettleDate = ClientUtil.ToDateTime("1900-1-1");
        ///<summary>
        ///租赁开始时间
        ///</summary>
        virtual public DateTime StartSettleDate
        {
            get { return startSettleDate; }
            set { startSettleDate = value; }
        }
        private DateTime endSettleDate = ClientUtil.ToDateTime("1900-1-1");
        ///<summary>
        ///租赁结束时间
        ///</summary>
        virtual public DateTime EndSettleDate
        {
            get { return endSettleDate; }
            set { endSettleDate = value; }
        }
        private decimal settleDate;
        ///<summary>
        ///租赁时间
        ///</summary>
        virtual public decimal SettleDate
        {
            get { return settleDate; }
            set { settleDate = value; }
        }





    }
}
