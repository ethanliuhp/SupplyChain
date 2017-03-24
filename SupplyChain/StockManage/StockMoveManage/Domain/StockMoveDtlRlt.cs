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
    public class StockMoveDtlRlt : BusEntityDetRelation
    {
        private StockMove theStockMove;
        private StockMoveDtl theStockMoveDtl;

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
                theStockMove = value as StockMove;
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
                theStockMoveDtl = value as StockMoveDtl;
            }
        }
    }
}
