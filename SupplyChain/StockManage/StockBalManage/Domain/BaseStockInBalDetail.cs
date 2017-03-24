using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockBalManage.Domain
{
    [Serializable]
    public class BaseStockInBalDetail:BaseDetail
    {
        private decimal stockInPrice;
        private decimal stockInMoney;
        private decimal costMoney;
        private string professionalCategory;
        private decimal quantityTemp;
        private decimal confirmPrice;
        private decimal confirmMoney;
        /// <summary>
        /// 认价单价
        /// </summary>
        public virtual decimal ConfirmPrice
        {
            get { return confirmPrice; }
            set { confirmPrice = value; }
        }
        /// <summary>
        /// 认价金额
        /// </summary>
        public virtual decimal ConfirmMoney
        {
            get { return confirmMoney; }
            set { confirmMoney = value; }
        }
        /// <summary>
        /// 临时数量
        /// </summary>
        public virtual decimal QuantityTemp
        {
            get { return quantityTemp; }
            set { quantityTemp = value; }
        }

        /// <summary>
        /// 专业分类
        /// </summary>
        public virtual string ProfessionalCategory
        {
            get { return professionalCategory; }
            set { professionalCategory = value; }
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
        /// 采购金额
        /// </summary>
        public virtual decimal StockInMoney
        {
            get { return stockInMoney; }
            set { stockInMoney = value; }
        }

        /// <summary>
        /// 采购单价
        /// </summary>
        public virtual decimal StockInPrice
        {
            get { return stockInPrice; }
            set { stockInPrice = value; }
        }
    }
}
