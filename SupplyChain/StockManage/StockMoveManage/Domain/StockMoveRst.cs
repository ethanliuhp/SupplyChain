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
    public class StockMoveRst : AuditResult
    {
        private StockMove theStockMove;
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
                theStockMove = value as StockMove;
            }
        }
    }
}
