using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain
{
    /// <summary>
    /// �̿����ⵥ��ϸ
    /// </summary>
    [Serializable]
    public class LossOutDtl : BasicStockOutDtl
    {
        private StockRelation theStockRelation;
        /// <summary>
        /// ��ϵʵ��
        /// </summary>
        virtual public StockRelation TheStockRelation
        {
            get { return theStockRelation; }
            set { theStockRelation = value; }
        }
    }
}
