using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// ���쵥�����ϵ
    /// </summary>
    [Entity]
    [Serializable]
    public class StockInRedRlt : BusEntityRelation
    {
        private StockInRed theStockInRed;
        /// <summary>
        /// ��������ϵ
        /// </summary>
        public override BusinessEntity BackwardBusEntity
        {
            get
            {
                return theStockInRed;
            }
            set
            {
                theStockInRed = value as StockInRed;
            }
        }
    }
}
