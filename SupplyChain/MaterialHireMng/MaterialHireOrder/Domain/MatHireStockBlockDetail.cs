using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain
{
    [Serializable]
    public class MatHireStockBlockDetail : BaseDetail
    {
        private string stockId;
        private string materialType;
        private decimal materialLength;
        private CostAccountSubject subjectGUID;
        private string subjectName;
        private string subjectSysCode;
        private SupplierRelationInfo borrowUnit;
        private string borrowUnitName;
        private decimal beforeStockQty = 0;
        /// <summary>
        /// 收料前库存数
        /// </summary>
        virtual public decimal BeforeStockQty
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
        /// 库存ID
        /// </summary>
        public virtual string StockId
        {
            get { return stockId; }
            set { stockId = value; }
        }

        /// <summary>
        /// 碗扣型号
        /// </summary>
        public  virtual string MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
        }
        /// <summary>
        /// 长度
        /// </summary>
        public virtual decimal MaterialLength
        {
            get { return materialLength; }
            set { materialLength = value; }
        }
        /// <summary>
        /// 实际数量 长度*数量
        /// </summary>
        public virtual decimal RealQuantity
        {
            get { return MaterialLength * Quantity; }
        }

    }
}
