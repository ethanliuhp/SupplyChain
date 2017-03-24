using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockOutManage.Domain
{
    /// <summary>
    /// ���ⵥ�����ϵ
    /// </summary>
    [Entity]
    [Serializable]
    public class StockOutRlt : BusEntityRelation
    {
        private StockOut theStockOut;
        /// <summary>
        /// �������
        /// </summary>
        public override BusinessEntity BackwardBusEntity
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
    }
}
