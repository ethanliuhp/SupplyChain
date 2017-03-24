using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.DetectionReceiptManage.Domain
{
    /// <summary>
    /// 检测回执单主表
    /// </summary>
    public class DetectionReceiptMaster : BaseMaster
    {
        private decimal sumQuantity;
        /// <summary>
        /// 数量汇总
        /// </summary>
        virtual public decimal SumQuantity
        {
            get
            {
                decimal tmpQuantity = 0;
                //汇总
                foreach (DetectionReceiptDetail var in Details)
                {
                    tmpQuantity += var.DetectionQuantity;
                }
                sumQuantity = tmpQuantity;
                return sumQuantity;
            }
            set { sumQuantity = value; }
        }
    }
}
