using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.ContractAdjustPriceManage.Domain;

namespace Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain
{
    /// <summary>
    /// 采购合同明细
    /// </summary>
    [Serializable]
    public class SupplyOrderDetail : BaseSupplyDetail
    {
        private decimal supplyPrice;
        private string grade;
        private decimal modifyPrice;
        private ContractAdjustPrice contractAdjustPrice;
        private string brand;
        private decimal confirmPrice;
        private decimal leftQuantity;
        private decimal rjMoney;
        private string technologyParameter;
        virtual public string TechnologyParameter
        {
            get { return technologyParameter; }
            set { technologyParameter = value; }
        }
        /// <summary>
        ///  认价总金额
        /// </summary>
        virtual public decimal RJMoney
        {
            get { return rjMoney; }
            set { rjMoney = value; }
        }

        private decimal quantityTemp;
        virtual public decimal QuantityTemp
        {
            get { return quantityTemp; }
            set { quantityTemp = value; }
        }


        /// <summary>
        /// 采购单价
        /// </summary>
        virtual public decimal SupplyPrice
        {
            get { return supplyPrice; }
            set { supplyPrice = value; }
        }
        /// <summary>
        /// 档次
        /// </summary>
        virtual public string Grade
        {
            get { return grade; }
            set { grade = value; }
        }
        /// <summary>
        /// 调后价格
        /// </summary>
        virtual public decimal ModifyPrice
        {
            get { return modifyPrice; }
            set { modifyPrice = value; }
        }
        /// <summary>
        /// 采购合同调价单ID
        /// </summary>
        virtual public ContractAdjustPrice ContractAdjustPrice
        {
            get { return contractAdjustPrice; }
            set { contractAdjustPrice = value; }
        }
        /// <summary>
        /// 品牌
        /// </summary>
        virtual public string Brand
        {
            get { return brand; }
            set { brand = value; }
        }
        /// <summary>
        /// 认价单价
        /// </summary>
        virtual public decimal ConfirmPrice
        {
            get { return confirmPrice; }
            set { confirmPrice = value; }
        }
        /// <summary>
        /// 剩余数量
        /// </summary>
        virtual public decimal LeftQuantity
        {
            get { return leftQuantity; }
            set { leftQuantity = value; }
        }
    }
}
