using System;
using System.Collections.Generic;
using System.Text;
using Application.Business.Erp.SupplyChain.StockManage.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Domain
{
    /// <summary>
    /// 库存量
    /// </summary>
    [Serializable]
    [Entity]
    public class StockQuantity
    {
        private long id=-1;
        private long version=-1;
        private StationCategory theStaCat;
        private ManageState theMngState;
        private decimal useableQuantity;
        private decimal quantity;
        /// <summary>
        /// 数量
        /// </summary>
        virtual public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        virtual public decimal UseableQuantity
        {
            get { return useableQuantity; }
            set { useableQuantity = value; }
        }
        /// <summary>
        /// 管理实例
        /// </summary>
        virtual public ManageState TheMngState
        {
            get { return theMngState; }
            set { theMngState = value; }
        }
        /// <summary>
        /// 库存
        /// </summary>
        virtual public StationCategory TheStaCat
        {
            get { return theStaCat; }
            set { theStaCat = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// Id
        /// </summary>
        virtual public long Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
