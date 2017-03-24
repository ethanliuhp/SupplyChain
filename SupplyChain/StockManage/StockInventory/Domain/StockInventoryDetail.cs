using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInventory.Domain
{
    /// <summary>
    /// 月度盘点明细
    /// </summary>
    [Entity]
    [Serializable]
    public class StockInventoryDetail : BaseDetail
    {
        private string specialType;
        private decimal stockQuantity;
        private decimal inventoryQuantity;
        private string materialClassify;
        private decimal price;
        private decimal confirmprice;
        private decimal money;
        private decimal confirmmoney;
        private string diagramNumber;
        /// <summary>
        /// 图号
        /// </summary>
        public virtual string DiagramNumber
        {
            get { return diagramNumber; }
            set { diagramNumber = value; }
        }

    //    <property name="Price"/>
    //<property name="Money"/>
    //<property name="ConfirmPrice"/>
    //<property name="ConfirmMoney"/>
    //    <property name="SubjectGuid"/>
    //<property name="SubjectName"/>
    //<property name="SubjectSysCode"/>
        private string subjectGuid;
        private string subjectName;
        private string subjectSysCode;
        /// <summary>
        /// 核算节点ID
        /// </summary>
        public virtual string SubjectGuid
        {
            get { return subjectGuid; }
            set { subjectGuid = value; }
        }
        /// <summary>
        /// 核算节点名称
        /// </summary>
        public virtual string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }
        /// <summary>
        /// 核算节点的syscode
        /// </summary>
        public virtual string SubjectSysCode
        {
            get { return subjectSysCode; }
            set { subjectSysCode = value; }
        }
        virtual public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        virtual public decimal Money
        {
            get { return money; }
            set { money = value; }
        }
        virtual public decimal ConfirmPrice
        {
            get { return confirmprice; }
            set { confirmprice = value; }
        }
        virtual public decimal ConfirmMoney
        {
            get { return confirmmoney; }
            set { confirmmoney = value; }
        }
        /// <summary>
        /// 专业分类
        /// </summary>
        virtual public string SpecialType
        {
            get { return specialType; }
            set { specialType = value; }
        }
        /// <summary>
        /// 库存数量
        /// </summary>
        virtual public decimal StockQuantity
        {
            get { return stockQuantity; }
            set { stockQuantity = value; }
        }
        /// <summary>
        /// 盘点数量
        /// </summary>
        virtual public decimal InventoryQuantity
        {
            get { return inventoryQuantity; }
            set { inventoryQuantity = value; }
        }
        /// <summary>
        /// 物资分类层次
        /// </summary>
        virtual public string MaterialClassify
        {
            get { return materialClassify; }
            set { materialClassify = value; }
        }

    }
}
