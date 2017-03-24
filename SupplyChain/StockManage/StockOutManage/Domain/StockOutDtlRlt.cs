using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain
{
    /// <summary>
    /// ���ⵥ��ϸ��ϵ
    /// </summary>
    [Entity]
    [Serializable]
    public class StockOutDtlRlt : BusEntityDetRelation
    {
        private StockOut theStockOut;
        private StockOutDtl theStockOutDtl;

        /// <summary>
        /// �����ϵ
        /// </summary>
        public override BusinessEntity BackwardMain
        {
            get
            {
                return theStockOut;
            }
            set
            {
                theStockOut = value as StockOut;
            }
        }
        /// <summary>
        /// ��ϸ��ϵ
        /// </summary>
        public override BusinessEntityDetails BackwardDetail
        {
            get
            {
                return theStockOutDtl;
            }
            set
            {
                theStockOutDtl = value as StockOutDtl;
            }
        }
    }
}
