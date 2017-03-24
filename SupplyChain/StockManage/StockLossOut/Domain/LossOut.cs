using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.StockManage.StockOutManage.BasicDomain;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.StockManage.StockLossOut.Domain
{
    /// <summary>
    /// 盘亏出库单
    /// </summary>
    [Serializable]
    public class LossOut :BaseMaster
    {
        private StationCategory theStationCategory;
        private int isTally;
        private string special;

        /// <summary>
        /// 专业 区分单据
        /// </summary>
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }

        /// <summary>
        /// 是否记账
        /// </summary>
        virtual public int IsTally
        {
            get { return isTally; }
            set { isTally = value; }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        virtual public StationCategory TheStationCategory
        {
            get { return theStationCategory; }
            set { theStationCategory = value; }
        }

    }
}
