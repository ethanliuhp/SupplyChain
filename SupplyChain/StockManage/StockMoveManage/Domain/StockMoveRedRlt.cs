using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain
{
    /// <summary>
    /// ת�ֵ���ϵ
    /// </summary>
    [Serializable]
    public class StockMoveRedRlt : BusEntityRelation
    {
        private StockMoveRed theStockMove;
        /// <summary>
        /// �������
        /// </summary>
        public override BusinessEntity BackwardBusEntity
        {
            get
            {
                return theStockMove;
            }
            set
            {
                theStockMove = value as StockMoveRed;
            }
        }
    }
}
