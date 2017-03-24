using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockMoveManage.Domain
{
    /// <summary>
    /// ת�ֵ����
    /// </summary>
    [Serializable]
    public class StockMoveRedRst : AuditResult
    {
        private StockMoveRed theStockMove;
        /// <summary>
        /// �������
        /// </summary>
        public override BusinessEntity TheProcess
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
