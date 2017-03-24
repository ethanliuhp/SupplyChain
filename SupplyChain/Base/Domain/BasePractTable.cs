using System;
using System.Collections.Generic;
using System.Text;

using Application.Resource.MaterialResource.Domain;
using Application.Resource.BasicData.Domain;

namespace Application.Business.Erp.SupplyChain.Base.Domain
{
    /// <summary>
    /// 码单表基类
    /// </summary>
    public abstract class BasePractTable
    {
        private long id = -1;
        private long version = -1;
        private string _BaleNo;
        private string _CoilNo;
        private QuanlityGrade _QlyGrade;
        private decimal _PieceQty;
        private decimal _Quantity;
        private StandardUnit _Unit;
        private MeterMannerMat _MeterManner;
        private PieceUnitMaterial _PieceUnit;
        private string descript;
        /// <summary>
        /// 计件单位
        /// </summary>
        virtual public PieceUnitMaterial PieceUnit
        {
            get { return _PieceUnit; }
            set { _PieceUnit = value; }
        }

        /// <summary>
        /// 计量方式
        /// </summary>
        virtual public MeterMannerMat MeterManner
        {
            get { return _MeterManner; }
            set { _MeterManner = value; }
        }

        /// <summary>
        /// 计量单位
        /// </summary>
        virtual public StandardUnit Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        /// <summary>
        /// 重量
        /// </summary>
        virtual public decimal Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        virtual public decimal PieceQty
        {
            get { return _PieceQty; }
            set { _PieceQty = value; }
        }

        /// <summary>
        /// 质量等级
        /// </summary>
        virtual public QuanlityGrade QlyGrade
        {
            get { return _QlyGrade; }
            set { _QlyGrade = value; }
        }
        /// <summary>
        /// 钢卷号
        /// </summary>
        virtual public string CoilNo
        {
            get { return _CoilNo; }
            set { _CoilNo = value; }
        }

        /// <summary>
        /// 捆包号
        /// </summary>
        virtual public string BaleNo
        {
            get { return _BaleNo; }
            set { _BaleNo = value; }
        }        
        /// <summary>
        /// ID
        /// </summary>
        virtual public long Id
        {
            get { return id; }
            set { id = value; }
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
        /// 备注
        /// </summary>
        virtual public string Descript
        {
            get { return descript; }
            set { descript = value; }
        }
      
    }
}
