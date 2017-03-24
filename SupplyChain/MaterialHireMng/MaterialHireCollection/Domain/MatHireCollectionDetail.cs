using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain
{
    /// <summary>
    /// 料具收料单明细
    /// </summary>

    [Serializable]
  public   class MatHireCollectionDetail : BaseDetail
    {
        private string balRule;
        private decimal rentalPrice;
        private decimal borrowQuantity;
        private SupplierRelationInfo borrowUnit;
        private string borrowUnitName;
        private int tempDtlId;
        private decimal leftQuantity;
        private DateTime matCollDate;
        //private decimal realQuantity;
        private CostAccountSubject subjectGUID;
        private string subjectName;
        private string subjectSysCode;
        private decimal materialLength = 0;
        private string materialType;
        private decimal beforeStockQty = 0;
        /// <summary>
        /// 收料前库存数
        /// </summary>
       virtual  public decimal BeforeStockQty
        {
            get { return beforeStockQty; }
            set { beforeStockQty = value; }
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

        private Iesi.Collections.Generic.ISet<MatHireCollectionCostDtl> matCostDtls = new Iesi.Collections.Generic.HashedSet<MatHireCollectionCostDtl>();
        virtual public Iesi.Collections.Generic.ISet<MatHireCollectionCostDtl> MatCostDtls
        {
            get { return matCostDtls; }
            set { matCostDtls = value; }
        }
        /// <summary>
        /// 增加费用明细设置
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatCostDtl(MatHireCollectionCostDtl detail)
        {
            detail.Master = this;
            MatCostDtls.Add(detail);
        }

        /// <summary>
        /// 临时明细ID，不做MAP
        /// </summary>
        public virtual int TempDtlId
        {
            get { return tempDtlId; }
            set { tempDtlId = value; }
        }
        /// <summary>
        /// 结算规则
        /// </summary>
        public virtual string BalRule
        {
            get { return balRule; }
            set { balRule = value; }
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
        /// 借用数量
        /// </summary>
        virtual public decimal BorrowQuantity
        {
            get { return borrowQuantity; }
            set { borrowQuantity = value; }
        }
        /// <summary>
        /// 借用单位
        /// </summary>
        virtual public SupplierRelationInfo BorrowUnit
        {
            get { return borrowUnit; }
            set { borrowUnit = value; }
        }
        /// <summary>
        /// 借用单位名称
        /// </summary>
        virtual public string BorrowUnitName
        {
            get { return borrowUnitName; }
            set { borrowUnitName = value; }
        }

        /// <summary>
        /// 剩余数量(临时字段)
        /// </summary>
        virtual public decimal LeftQuantuity
        {
            get { return leftQuantity; }
            set { leftQuantity = value; }
        }
        /// <summary>
        /// 收料日期
        /// </summary>
        virtual public DateTime MatCollDate
        {
            get { return matCollDate; }
            set { matCollDate = value; }
        }

        private Iesi.Collections.Generic.ISet<MatHireReturnDetailSeq> materialReturnDetailSeqs = new Iesi.Collections.Generic.HashedSet<MatHireReturnDetailSeq>();
        virtual public Iesi.Collections.Generic.ISet<MatHireReturnDetailSeq> MaterialReturnDetailSeqs
        {
            get { return materialReturnDetailSeqs; }
            set { materialReturnDetailSeqs = value; }
        }

        /// <summary>
        /// 增加退料时序
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatReturnDtlSeq(MatHireReturnDetailSeq detail)
        {
            detail.MatCollDtlMaster = this;
            MaterialReturnDetailSeqs.Add(detail);
        }
        /// <summary>
        /// 碗口型号
        /// </summary>
        virtual public string  MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
        }
        /// <summary>
        /// 物资长度
        /// </summary>
        virtual public decimal MaterialLength
        {
            get { return materialLength; }
            set { materialLength = value; }
        }
        /// <summary>
        /// 不存储 实际录入数量 Quantity*MaterialLength
        /// </summary>
        virtual public decimal RealQuantity
        {
            get { return Quantity*MaterialLength; }
          
        }
        /// <summary>
        /// 不存储
        /// </summary>
        virtual public decimal RealTempData
        {
            get { return ClientUtil.ToDecimal( TempData) * MaterialLength; }

        }
    }
}
