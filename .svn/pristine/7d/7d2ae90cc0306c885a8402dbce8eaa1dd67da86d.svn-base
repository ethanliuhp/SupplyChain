using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireRelation
{
    /// <summary>
    /// 库存关系
    /// </summary>
    [Serializable]
    [Entity]
    public class MatHireStockRelation
    {
        private string id;
        private int accountYear;
        private int accountMonth;
        private DocumentState state;
        private string stationCategory;
        private string projectId;
        private string projectName;
        private string operOrgInfo;
        private string operOrgInfoName;
        private string stockInId;
        private string stockInDtlId;
        private decimal price;
        private decimal quantity;
        private decimal money;
        private decimal remainQuantity;
        private decimal remainMoney;
        private string seqId;
        private DateTime seqCreateDate = new DateTime(1900, 1, 1);
        private string material;
        private string materialName;
        private string materialCode;
        private string materialSpec;
        private decimal idleQuantity;
        private string descript;
        private string special;
        private string professionCategory;
        private SupplierRelationInfo supplierRelationInfo;
        private string supplierName;
        private string diagramNumber;
        private decimal confirmprice;
        private string materialStuff;
        private string matStandardUnit;
        private string matStandardUnitName;

        public virtual string MatStandardUnit
        {
            get { return matStandardUnit; }
            set { matStandardUnit = value; }
        }
        public virtual string MatStandardUnitName
        {
            get { return matStandardUnitName; }
            set { matStandardUnitName = value; }
        }

        public virtual string MaterialStuff
        {
            get { return materialStuff; }
            set { materialStuff = value; }
        }
        /// <summary>
        /// 图号
        /// </summary>
        public virtual string DiagramNumber
        {
            get { return diagramNumber; }
            set { diagramNumber = value; }
        }
        /// <summary>
        /// 认价单价
        /// </summary>
        public virtual decimal Confirmprice
        {
            get { return confirmprice; }
            set { confirmprice = value; }
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
        /// 供应商
        /// </summary>
        public virtual SupplierRelationInfo SupplierRelationInfo
        {
            get { return supplierRelationInfo; }
            set { supplierRelationInfo = value; }
        }

        /// <summary>
        /// 安装 专业分类
        /// </summary>
        public virtual string ProfessionCategory
        {
            get { return professionCategory; }
            set { professionCategory = value; }
        }

        /// <summary>
        /// 专业 区分单据
        /// </summary>
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 闲置数量
        /// </summary>
        public virtual decimal IdleQuantity
        {
            get { return idleQuantity; }
            set { idleQuantity = value; }
        }

        /// <summary>
        /// 规格型号
        /// </summary>
        public virtual string MaterialSpec
        {
            get { return materialSpec; }
            set { materialSpec = value; }
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
        /// 物资名称
        /// </summary>
        public virtual string MaterialName
        {
            get { return materialName; }
            set { materialName = value; }
        }

        /// <summary>
        /// 物料Id
        /// </summary>
        public virtual string Material
        {
            get { return material; }
            set { material = value; }
        }

        /// <summary>
        /// 时序表生成日期
        /// </summary>
        public virtual DateTime SeqCreateDate
        {
            get { return seqCreateDate; }
            set { seqCreateDate = value; }
        }

        /// <summary>
        /// 时序表ID
        /// </summary>
        public virtual string SeqId
        {
            get { return seqId; }
            set { seqId = value; }
        }

        /// <summary>
        /// 剩余金额
        /// </summary>
        public virtual decimal RemainMoney
        {
            get { return remainMoney; }
            set { remainMoney = value; }
        }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public virtual decimal RemainQuantity
        {
            get { return remainQuantity; }
            set { remainQuantity = value; }
        }

        /// <summary>
        /// 金额
        /// </summary>
        public virtual decimal Money
        {
            get { return money; }
            set { money = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        /// <summary>
        /// 入库单明细ID
        /// </summary>
        public virtual string StockInDtlId
        {
            get { return stockInDtlId; }
            set { stockInDtlId = value; }
        }

        /// <summary>
        /// 入库单ID
        /// </summary>
        public virtual string StockInId
        {
            get { return stockInId; }
            set { stockInId = value; }
        }

        /// <summary>
        /// 业务组织名称
        /// </summary>
        public virtual string OperOrgInfoName
        {
            get { return operOrgInfoName; }
            set { operOrgInfoName = value; }
        }

        /// <summary>
        /// 业务组织
        /// </summary>
        public virtual string OperOrgInfo
        {
            get { return operOrgInfo; }
            set { operOrgInfo = value; }
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        /// <summary>
        /// 项目ID
        /// </summary>
        public virtual string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public virtual string StationCategory
        {
            get { return stationCategory; }
            set { stationCategory = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual DocumentState State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 会计月
        /// </summary>
        public virtual int AccountMonth
        {
            get { return accountMonth; }
            set { accountMonth = value; }
        }

        /// <summary>
        /// 会计年
        /// </summary>
        public virtual int AccountYear
        {
            get { return accountYear; }
            set { accountYear = value; }
        }

        /// <summary>
        /// 序号
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }


    }
}
