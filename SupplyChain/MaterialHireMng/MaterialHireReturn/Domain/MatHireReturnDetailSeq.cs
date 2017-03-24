﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;
 
namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireReturn.Domain
{
    /// <summary>
    /// 退料时序表
    /// </summary>
    [Serializable]
    public class MatHireReturnDetailSeq
    {
        private string id;
        private string matCollDtlId;
        private string matReturnDtlId;
        private string matLedgerId;
        private decimal returnQuantity;
        private DateTime returnDate;
        private MatHireReturnDetail matReturnDtlMaster;
        private MatHireCollectionDetail matCollDtlMaster;
        private long version;
        private string seqType;
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
        /// 碗扣型号
        /// </summary>
        public virtual string MaterialType
        {
            get { return materialType; }
            set {   materialType=value; }
        }
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 时序类型(退料产生、收料负数产生)
        /// </summary>
        virtual public string SeqType
        {
            get { return seqType; }
            set { seqType = value; }
        }
        /// <summary>
        /// 收料明细GUID
        /// </summary>
        virtual public string MatCollDtlId
        {
            get { return matCollDtlId; }
            set { matCollDtlId = value; }
        }
        /// <summary>
        /// 退料明细GUID(退料负数产生的收料)
        /// </summary>
        virtual public string MatReturnDtlId
        {
            get { return matReturnDtlId; }
            set { matReturnDtlId = value; }
        }
        /// <summary>
        /// 台账GUID
        /// </summary>
        virtual public string MatLedgerId
        {
            get { return matLedgerId; }
            set { matLedgerId = value; }
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
        /// 退料明细GUID（父ID）
        /// </summary>
        virtual public MatHireReturnDetail MatReturnDtlMaster
        {
            get { return matReturnDtlMaster; }
            set { matReturnDtlMaster = value; }
        }
        /// <summary>
        /// 收料明细GUID(收料负数产生的退料时序(父ID))
        /// </summary>
        virtual public MatHireCollectionDetail MatCollDtlMaster
        {
            get { return matCollDtlMaster; }
            set { matCollDtlMaster = value; }
        }
        /// <summary>
        /// 退料日期
        /// </summary>
        virtual public DateTime ReturnDate
        {
            get { return returnDate; }
            set { returnDate = value; }
        }

        private string matCollCode;
        private decimal matCollDtlQty;
        private string matReturnCode;
        private decimal matRerturnDtlQty;

        /// <summary>
        /// 收料单号
        /// </summary>
        virtual public string MatCollCode
        {
            get { return matCollCode; }
            set { matCollCode = value; }
        }
        /// <summary>
        /// 收料明细数量
        /// </summary>
        virtual public decimal MatCollDtlQty
        {
            get { return matCollDtlQty; }
            set { matCollDtlQty = value; }
        }
        /// <summary>
        /// 退料单号
        /// </summary>
        virtual public string MatReturnCode
        {
            get { return matReturnCode; }
            set { matReturnCode = value; }
        }
        /// <summary>
        /// 退料明细（真实数量）
        /// </summary>
        virtual public decimal MatReturnDtlQty
        {
            get { return matRerturnDtlQty; }
            set { matRerturnDtlQty = value; }
        }
    }
}