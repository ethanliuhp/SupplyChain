using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain
{
    /// <summary>
    /// 供应商类型
    /// </summary>
    public enum EnumSupplierType
    {
        公司 = 0,
        分公司 = 1,
        外单位 = 2,
        其他 = 100
    }
    /// <summary>
    /// 料具租赁合同主表
    /// </summary>
    [Serializable]
   public  class MatHireOrderMaster: BaseMaster
    {
        private string attachFilePath;
        private string originalContractNo;
        private string receiveLocation;
        private string receiveType;
        private DateTime receiveDate = new DateTime(1900, 1, 1);
        private EnumSupplierType supplierType;
        private string supplierName;
        private string supplierID;
        private SupplierRelationInfo theSupplierRelationInfo;
        private IList tempData;
        private string billCode;
        /// <summary>料具站手工录入单据号</summary>
        public virtual string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }
        private Iesi.Collections.Generic.ISet<OrderMasterCostSetItem> basiCostSets = new Iesi.Collections.Generic.HashedSet<OrderMasterCostSetItem>();
        virtual public IList TempData { get { return tempData; } set { tempData = value; } }
        virtual public Iesi.Collections.Generic.ISet<OrderMasterCostSetItem> BasiCostSets
        {
            get { return basiCostSets; }
            set { basiCostSets = value; }
        }

        /// <summary>
        /// 增加费用设置表明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddBasicCostSet(OrderMasterCostSetItem detail)
        {
            detail.Master = this;
            BasiCostSets.Add(detail);
        }
        private string balRule;

        /// <summary>
        /// 结算规则
        /// </summary>
        public virtual string BalRule
        {
            get { return balRule; }
            set { balRule = value; }
        }
        /// <summary>
        /// 供应商类型
        /// </summary>
        public virtual EnumSupplierType SupplierType
        {
            get { return supplierType; }
            set { supplierType = value; }
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public virtual string SupplierID
        {
            get { return supplierID; }
            set { supplierID = value; }
        }

        /// <summary>
        /// 出租方(供应商)
        /// </summary>
        public virtual SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }

        /// <summary>
        /// 交货日期
        /// </summary>
        public virtual DateTime ReceiveDate
        {
            get { return receiveDate; }
            set { receiveDate = value; }
        }

        /// <summary>
        /// 交货方式
        /// </summary>
        public virtual string ReceiveType
        {
            get { return receiveType; }
            set { receiveType = value; }
        }

        /// <summary>
        /// 交货地点
        /// </summary>
        public virtual string ReceiveLocation
        {
            get { return receiveLocation; }
            set { receiveLocation = value; }
        }

        /// <summary>
        /// 原始合同号
        /// </summary>
        public virtual string OriginalContractNo
        {
            get { return originalContractNo; }
            set { originalContractNo = value; }
        }

        /// <summary>
        /// 附件路径
        /// </summary>
        public virtual string AttachFilePath
        {
            get { return attachFilePath; }
            set { attachFilePath = value; }
        }
       
    }
}
