using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireStockIn.BasicDomain
{
    /// <summary>
    /// 出入库方式
    /// </summary>
    public enum EnumStockInOutManner
    {
        收料入库 = 10,
        调拨入库 = 11,
        盘盈入库 = 12,
        领料出库 = 20,
        调拨出库 = 21,
        盘亏出库 = 22,
    }

    /// <summary>
    /// 入库单冲红类型
    /// </summary>
    public enum EnumForRedType
    {
        冲数量 = 0,
        冲单价 = 1,
    }

    /// <summary>
    /// 入库单基类
    /// </summary>
    [Serializable]
    public abstract class MatHireBasicStockIn : BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private StationCategory theStationCategory;
        private int isTally = 0;
        private string supplyOrderCode;
        private decimal sumMoney;
        private decimal sumQuantity;
        private string theSupplierName;
        private EnumStockInOutManner stockInManner;
        private string contractNo;
        private decimal sumConfirmMoney;
        private string special;
        private MaterialCategory materialCategory;
        private string matCatName;
        private string professionCategory;
        private int theStockInOutKind;

        /// <summary>
        /// 入库类型
        /// </summary>
        public virtual int TheStockInOutKind
        {
            get { return theStockInOutKind; }
            set { theStockInOutKind = value; }
        }
        /// <summary>
        /// 专业分类
        /// </summary>
        public virtual string ProfessionCategory
        {
            get { return professionCategory; }
            set { professionCategory = value; }
        }

        /// <summary>
        /// 物资分类名称
        /// </summary>
        public virtual string MatCatName
        {
            get { return matCatName; }
            set { matCatName = value; }
        }

        /// <summary>
        /// 物资分类
        /// </summary>
        public virtual MaterialCategory MaterialCategory
        {
            get { return materialCategory; }
            set { materialCategory = value; }
        }

        /// <summary>
        /// 专业 用于区分单据
        /// </summary>
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual string ContractNo
        {
            get { return contractNo; }
            set { contractNo = value; }
        }

        /// <summary>
        /// 入库方式
        /// </summary>
        public virtual EnumStockInOutManner StockInManner
        {
            get { return stockInManner; }
            set { stockInManner = value; }
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string TheSupplierName
        {
            get { return theSupplierName; }
            set { theSupplierName = value; }
        }

        /// <summary>
        /// 数量合计
        /// </summary>
        virtual public decimal SumQuantity
        {
            get
            {
                decimal tmpQuantity = 0;
                //汇总
                foreach (MatHireBasicStockInDtl var in Details)
                {
                    tmpQuantity += var.Quantity;
                }
                sumQuantity = tmpQuantity;
                return sumQuantity;
            }
            set { sumQuantity = value; }
        }

        public virtual decimal SumConfirmMoney
        {
            get
            {
                decimal tmpMoney = 0;
                foreach (MatHireBasicStockInDtl var in Details)
                {
                    tmpMoney += var.ConfirmMoney;
                }
                sumConfirmMoney = tmpMoney;
                return sumConfirmMoney;
            }
            set { sumConfirmMoney = value; }
        }

        /// <summary>
        /// 金额总计
        /// </summary>
        virtual public decimal SumMoney
        {
            get
            {
                decimal tmpMoney = 0;
                //汇总
                foreach (MatHireBasicStockInDtl var in Details)
                {
                    tmpMoney += var.Money;
                }
                sumMoney = tmpMoney;
                return sumMoney;
            }
            set
            {
                sumMoney = value;
            }
        }

        /// <summary>
        /// 采购订单Code
        /// </summary>
        virtual public string SupplyOrderCode
        {
            get { return supplyOrderCode; }
            set { supplyOrderCode = value; }
        }

        /// <summary>
        /// 是否记账
        /// </summary>
        virtual public int IsTally
        {
            get { return isTally; }
            set { isTally = value; }
        }
        /// <summary>
        /// 仓库
        /// </summary>
        virtual public StationCategory TheStationCategory
        {
            get { return theStationCategory; }
            set { theStationCategory = value; }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }

        private string concreteBalId;

        /// <summary>
        /// 商品砼结算主表ID
        /// </summary>
        virtual public string ConcreteBalID
        {
            get { return concreteBalId; }
            set { concreteBalId = value; }
        }
    }
}
