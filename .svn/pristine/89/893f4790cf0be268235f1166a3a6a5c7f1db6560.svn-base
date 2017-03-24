using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.SupplyManage.ContractAdjustPriceManage.Domain
{
    /// <summary>
    /// 采购合同调价单
    /// </summary>
    [Serializable]
    public class ContractAdjustPrice : BaseSupplyMaster
    {
        private string billType;
        private decimal modifyPrice;
        private string modifyPriceReason;
        private decimal prePrice;
        private string supplyOrderCode;
        private string contractNum;
        private SupplyOrderDetail supplyOrderDetail;
        private DateTime availabilityDate;

        private Material materialResource;
        private string materialName;
        private string materialStuff;
        private string materialSpec;
        private string materialCode;
        private string theSupplierRelationInfo;
        private string supplierName;

        /// <summary>
        /// 供应商
        /// </summary>
        virtual public string TheSupplierRelationInfo
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
        /// <summary>
        /// 单据类型
        /// </summary>
        virtual public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }
        /// <summary>
        /// 调后价格
        /// </summary>
        virtual public decimal ModifyPrice
        {
            get { return modifyPrice; }
            set { modifyPrice = value; }
        }
        /// <summary>
        ///调价原因 
        /// </summary>
        virtual public string ModifyPriceReason
        {
            get { return modifyPriceReason; }
            set { modifyPriceReason = value; }
        }
        /// <summary>
        /// 调前价格
        /// </summary>
        virtual public decimal PrePrice
        {
            get { return prePrice; }
            set { prePrice = value; }
        }
        /// <summary>
        /// 采购合同单据号
        /// </summary>
        virtual public string SupplyOrderCode
        {
            get { return supplyOrderCode; }
            set { supplyOrderCode = value; }
        }
        /// <summary>
        /// 合同号
        /// </summary>
        virtual public string ContractNum
        {
            get { return contractNum; }
            set { contractNum = value; }
        }
        /// <summary>
        /// 采购合同明细GUID
        /// </summary>
        virtual public SupplyOrderDetail SupplyOrderDetail
        {
            get { return supplyOrderDetail; }
            set { supplyOrderDetail = value; }
        }
        /// <summary>
        /// 生效日期
        /// </summary>
        virtual public DateTime AvailabilityDate
        {
            get { return availabilityDate; }
            set { availabilityDate = value; }
        }
        /// <summary>
        /// 物料
        /// </summary>        
        virtual public Material MaterialResource
        {
            get { return materialResource; }
            set { materialResource = value; }
        }
        /// <summary>
        /// 物料编码
        /// </summary>
        virtual public string MaterialCode
        {
            get { return materialCode; }
            set { materialCode = value; }
        }
        /// <summary>
        /// 物料名称
        /// </summary>
        virtual public string MaterialName
        {
            get { return materialName; }
            set { materialName = value; }
        }
        /// <summary>
        /// 规格
        /// </summary>
        virtual public string MaterialSpec
        {
            get { return materialSpec; }
            set { materialSpec = value; }
        }
        /// <summary>
        /// 材质
        /// </summary>
        virtual public string MaterialStuff
        {
            get { return materialStuff; }
            set { materialStuff = value; }
        }
    }
}
