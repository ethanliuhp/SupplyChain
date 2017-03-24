using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// ��ⵥ��ϸ��ϵ
    /// </summary>
    [Entity]
    [Serializable]
    public class StockInDtlRlt : BusEntityDetRelation
    {
        private StockIn theStockIn;
        private StockInDtl theStockInDtl;

        /// <summary>
        /// �����ϵ
        /// </summary>
        public override BusinessEntity BackwardMain
        {
            get
            {
                return theStockIn;
            }
            set
            {
                theStockIn = value as StockIn;
            }
        }
        /// <summary>
        /// ��ϸ��ϵ
        /// </summary>
        public override BusinessEntityDetails BackwardDetail
        {
            get
            {
                return theStockInDtl;
            }
            set
            {
                theStockInDtl = value as StockInDtl;
            }
        }
    }
}
