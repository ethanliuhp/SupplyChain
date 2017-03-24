using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain
{
    [Serializable]
    public class BaseStockInBalMaster:BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private string theSupplierName;
        private string invoiceCode;
        private decimal costMoney;
        private MaterialCategory _MaterialCategory;
        private string _MaterialCategoryName;
        private string _ProfessionCategory;
        private long theStockInOutKind;

        public virtual long TheStockInOutKind
        {
            get { return theStockInOutKind; }
            set { theStockInOutKind = value; }
        }
        /// <summary>
        /// 专业分类
        /// </summary>
        public virtual string ProfessionCategory
        {
            set { _ProfessionCategory = value; }
            get { return _ProfessionCategory; }
        }
        /// <summary>
        /// 物资分类
        /// </summary>
        public virtual MaterialCategory MaterialCategory
        {
            get { return _MaterialCategory; }
            set { _MaterialCategory = value; }
        }
        /// <summary>
        /// 物资分类名称
        /// </summary>
        public virtual string MaterialCategoryName
        {
            set { _MaterialCategoryName = value; }
            get { return _MaterialCategoryName; }
        }

        /// <summary>
        /// 力资运费
        /// </summary>
        public virtual decimal CostMoney
        {
            get { return costMoney; }
            set { costMoney = value; }
        }

        /// <summary>
        /// 发票号
        /// </summary>
        public virtual string InvoiceCode
        {
            get { return invoiceCode; }
            set { invoiceCode = value; }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string TheSupplierName
        {
            get { return theSupplierName; }
            set { theSupplierName = value; }
        }
    }
}
