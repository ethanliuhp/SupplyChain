using System;
using System.Collections.Generic;
using System.Text;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Domain
{
    /// <summary>
    /// 库存时序表
    /// </summary>
    [Serializable]
    [Entity]
    public class StockSequence
    {
        private long id;
        private long version;
        private string billType;
        private object bill;
        private string billDtlType;
        private object billDtl;
        private StockRelation theStockRelation;
        private decimal quantity;
        private StandardUnit theStandardUnit;
        private decimal pieceQuantity;
        private PieceUnitMaterial thePieceUnit;
        private DateTime createDate=DateTime.Today;
        private long fiscalYear;
        private long fiscalMonth;
        private StationCategory theStationCategory;
        /// <summary>
        /// 仓库
        /// </summary>
        virtual public StationCategory TheStationCategory
        {
            get { return theStationCategory; }
            set { theStationCategory = value; }
        }

        /// <summary>
        /// 会计月
        /// </summary>
        virtual public long FiscalMonth
        {
            get { return fiscalMonth; }
            set { fiscalMonth = value; }
        }

        /// <summary>
        /// 会计年
        /// </summary>
        virtual public long FiscalYear
        {
            get { return fiscalYear; }
            set { fiscalYear = value; }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// 计件单位
        /// </summary>
        virtual public PieceUnitMaterial ThePieceUnit
        {
            get { return thePieceUnit; }
            set { thePieceUnit = value; }
        }

        /// <summary>
        /// 计件数量
        /// </summary>
        virtual public decimal PieceQuantity
        {
            get { return pieceQuantity; }
            set { pieceQuantity = value; }
        }


        /// <summary>
        /// 基本计量单位
        /// </summary>
        virtual public StandardUnit TheStandardUnit
        {
            get { return theStandardUnit; }
            set { theStandardUnit = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        virtual public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// 库存关系
        /// </summary>
        virtual public StockRelation TheStockRelation
        {
            get { return theStockRelation; }
            set { theStockRelation = value; }
        }

        /// <summary>
        /// 明细类型
        /// </summary>
        virtual public string BillDtlType
        {
            get { return billDtlType; }
            set { billDtlType = value; }
        }
        /// <summary>
        /// 料单明细
        /// </summary>
        virtual public object BillDtl
        {
            get { return billDtl; }
            set { billDtl = value; }
        }
        /// <summary>
        /// 料单
        /// </summary>
        virtual public object Bill
        {
            get { return bill; }
            set { bill = value; }
        }
        /// <summary>
        /// 料单类型
        /// </summary>
        virtual public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }


        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 序号
        /// </summary>
        virtual public long Id
        {
            get { return id; }
            set { id = value; }
        }

    }
}
