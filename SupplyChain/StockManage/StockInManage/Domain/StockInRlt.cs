using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockInManage.Domain
{
    /// <summary>
    /// ��ⵥ�����ϵ
    /// </summary>
    [Entity]
    [Serializable]
    public class StockInRlt : BusEntityRelation
    {
        private StockIn theStockIn;
        /// <summary>
        /// �������
        /// </summary>
        public override BusinessEntity BackwardBusEntity
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
    }
}