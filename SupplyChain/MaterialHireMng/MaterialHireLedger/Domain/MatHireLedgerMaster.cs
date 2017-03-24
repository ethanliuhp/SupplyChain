using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireLedger.Domain
{
    /// <summary>
    ///料具租赁台账
    /// </summary>

    [Serializable]
    public class MatHireLedgerMaster : BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;
        private string billId;
        private string billDetailId;
        private string billCode;
        private decimal balanceQuantity;
        private int washType;
        private decimal leftQuantity;
        private decimal collectionQuantity;
        private decimal returnQuantity;
        private DateTime systemDate;
        private string oldContractNum;
        private decimal rentalPrice;

        private Material materialResource;
        private string materialCode;
        private string materialName;
        private string materialSpec;

        private StandardUnit matStandardUnit;
        private string matStandardUnitName;

        private GWBSTree usedPart;
        private string usedPartName;
        private string usedPartSysCode;
        private SupplierRelationInfo theRank;
        private string theRankName;

        private decimal tempQuantity;

        private CostAccountSubject subjectGUID;
        private string subjectName;
        private string subjectSysCode;
        private decimal materialLength;
        private EnumMatHireType matHireType;
        private string materialType;
        /// <summary>
        /// 发料类型  普通料具 钢管 碗扣
        /// </summary>
        public virtual EnumMatHireType MatHireType
        {
            get { return matHireType; }
            set { matHireType = value; }
        }
        /// <summary>
        /// 物资长度
        /// </summary>
        public virtual decimal MaterialLength
        {
            get { return materialLength; }
            set { materialLength = value; }
        }

        /// <summary>
        /// 核算科目
        /// </summary>
        virtual public CostAccountSubject SubjectGUID
        {
            get { return subjectGUID; }
            set { subjectGUID = value; }
        }
        /// <summary>
        /// 核算科目名称
        /// </summary>
        virtual public string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }
        /// <summary>
        /// 核算科目层次码
        /// </summary>
        virtual public string SubjectSysCode
        {
            get { return subjectSysCode; }
            set { subjectSysCode = value; }
        }

        /// <summary>
        /// 临时数量，不做MAP
        /// </summary>
        virtual public decimal TempQuantity
        {
            get { return tempQuantity; }
            set { tempQuantity = value; }
        }

        /// <summary>
        /// 出租方/供应商
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }
        /// <summary>
        /// 出租方/供应商名称
        /// </summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        /// <summary>
        /// 单据明细ID(收退料)
        /// </summary>
        virtual public string BillDetailId
        {
            get { return billDetailId; }
            set { billDetailId = value; }
        }
        /// <summary>
        /// 收退料单GUID
        /// </summary>
        virtual public string BillId
        {
            get { return billId; }
            set { billId = value; }
        }
        /// <summary>
        /// 收退料单号
        /// </summary>
        virtual public string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }
        /// <summary>
        /// 结算数量
        /// </summary>
        virtual public decimal BalanceQuantity
        {
            get { return balanceQuantity; }
            set { balanceQuantity = value; }
        }
        /// <summary>
        /// 流转类型 0:收料; 1:退料
        /// </summary>
        virtual public int WashType
        {
            get { return washType; }
            set { washType = value; }
        }
        /// <summary>
        /// 剩余数量
        /// </summary>
        virtual public decimal LeftQuantity
        {
            get { return leftQuantity; }
            set { leftQuantity = value; }
        }
        /// <summary>
        /// 收料数量
        /// </summary>
        virtual public decimal CollectionQuantity
        {
            get { return collectionQuantity; }
            set { collectionQuantity = value; }
        }
        /// <summary>
        /// 退料数量
        /// </summary>
        virtual public decimal ReturnQuantity
        {
            get { return returnQuantity; }
            set { returnQuantity = value; }
        }
        /// <summary>
        /// 系统日期
        /// </summary>
        virtual public DateTime SystemDate
        {
            get { return systemDate; }
            set { systemDate = value; }
        }
        /// <summary>
        /// 原始合同号
        /// </summary>
        virtual public string OldContractNum
        {
            get { return oldContractNum; }
            set { oldContractNum = value; }
        }
        /// <summary>
        /// 租赁单价
        /// </summary>
        virtual public decimal RentalPrice
        {
            get { return rentalPrice; }
            set { rentalPrice = value; }
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
        /// 物资规格
        /// </summary>
        public virtual string MaterialSpec
        {
            get { return materialSpec; }
            set { materialSpec = value; }
        }

        /// <summary>
        /// 物资名称
        /// </summary>
        public virtual string MaterialName
        {
            get { return materialName; }
            set { materialName = value; }
        }

        /// <summary>
        /// 物资编码
        /// </summary>
        public virtual string MaterialCode
        {
            get { return materialCode; }
            set { materialCode = value; }
        }

        /// <summary>
        /// 计量单位
        /// </summary>
        virtual public StandardUnit MatStandardUnit
        {
            get { return matStandardUnit; }
            set { matStandardUnit = value; }
        }

        /// <summary>
        /// 计量单位名称
        /// </summary>
        public virtual string MatStandardUnitName
        {
            get { return matStandardUnitName; }
            set { matStandardUnitName = value; }
        }

        /// <summary> 
        /// 使用部位
        /// </summary>
        virtual public GWBSTree UsedPart
        {
            get { return usedPart; }
            set { usedPart = value; }
        }
        /// <summary>
        /// 使用部位名称
        /// </summary>
        virtual public string UsedPartName
        {
            get { return usedPartName; }
            set { usedPartName = value; }
        }
        /// <summary>
        /// 使用部位层次码
        /// </summary>
        virtual public string UsedPartSysCode
        {
            get { return usedPartSysCode; }
            set { usedPartSysCode = value; }
        }
        /// <summary>
        /// 使用队伍
        /// </summary>
        virtual public SupplierRelationInfo TheRank
        {
            get { return theRank; }
            set { theRank = value; }
        }
        /// <summary>
        /// 使用队伍名称
        /// </summary>
        virtual public string TheRankName
        {
            get { return theRankName; }
            set { theRankName = value; }
        }
        virtual public string MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
        }
    }
}
