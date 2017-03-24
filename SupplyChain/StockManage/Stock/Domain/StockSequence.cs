using System;
using System.Collections.Generic;
using System.Text;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Domain
{
    /// <summary>
    /// ���ʱ���
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
        /// �ֿ�
        /// </summary>
        virtual public StationCategory TheStationCategory
        {
            get { return theStationCategory; }
            set { theStationCategory = value; }
        }

        /// <summary>
        /// �����
        /// </summary>
        virtual public long FiscalMonth
        {
            get { return fiscalMonth; }
            set { fiscalMonth = value; }
        }

        /// <summary>
        /// �����
        /// </summary>
        virtual public long FiscalYear
        {
            get { return fiscalYear; }
            set { fiscalYear = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// �Ƽ���λ
        /// </summary>
        virtual public PieceUnitMaterial ThePieceUnit
        {
            get { return thePieceUnit; }
            set { thePieceUnit = value; }
        }

        /// <summary>
        /// �Ƽ�����
        /// </summary>
        virtual public decimal PieceQuantity
        {
            get { return pieceQuantity; }
            set { pieceQuantity = value; }
        }


        /// <summary>
        /// ����������λ
        /// </summary>
        virtual public StandardUnit TheStandardUnit
        {
            get { return theStandardUnit; }
            set { theStandardUnit = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        virtual public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// ����ϵ
        /// </summary>
        virtual public StockRelation TheStockRelation
        {
            get { return theStockRelation; }
            set { theStockRelation = value; }
        }

        /// <summary>
        /// ��ϸ����
        /// </summary>
        virtual public string BillDtlType
        {
            get { return billDtlType; }
            set { billDtlType = value; }
        }
        /// <summary>
        /// �ϵ���ϸ
        /// </summary>
        virtual public object BillDtl
        {
            get { return billDtl; }
            set { billDtl = value; }
        }
        /// <summary>
        /// �ϵ�
        /// </summary>
        virtual public object Bill
        {
            get { return bill; }
            set { bill = value; }
        }
        /// <summary>
        /// �ϵ�����
        /// </summary>
        virtual public string BillType
        {
            get { return billType; }
            set { billType = value; }
        }


        /// <summary>
        /// �汾
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// ���
        /// </summary>
        virtual public long Id
        {
            get { return id; }
            set { id = value; }
        }

    }
}
