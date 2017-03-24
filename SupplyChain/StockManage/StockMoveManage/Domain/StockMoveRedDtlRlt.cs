using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain
{
    /// <summary>
    /// ת�ֵ���ϸ��ϵ
    /// </summary>
    [Serializable]
    public class StockMoveRedDtlRlt : BusEntityDetRelation
    {
        private StockMoveRed theStockMove;
        private StockMoveRedDtl theStockMoveDtl;

        /// <summary>
        /// �����ϵ
        /// </summary>
        public override BusinessEntity BackwardMain
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
        /// <summary>
        /// ��ϸ��ϵ
        /// </summary>
        public override BusinessEntityDetails BackwardDetail
        {
            get
            {
                return theStockMoveDtl;
            }
            set
            {
                theStockMoveDtl = value as StockMoveRedDtl;
            }
        }
    }
}
