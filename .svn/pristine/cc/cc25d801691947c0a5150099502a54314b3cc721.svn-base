using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.StockManage.StockInventory.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.StockManage.StockInventory.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Service;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.StockCheckMng.StockInventoryMng
{
    public class MStockInventoryMng
    {
        private IStockInventorySrv stockInventorySrv;

        public IStockInventorySrv StockInventorySrv
        {
            get { return stockInventorySrv; }
            set { stockInventorySrv = value; }
        }
        public IStockRelationSrv stockRelationSrv;
        public MStockInventoryMng()
        {
            if (stockInventorySrv == null)
            {
                stockInventorySrv = StaticMethod.GetService("StockInventorySrv") as IStockInventorySrv;
            }
            if (stockRelationSrv == null)
                stockRelationSrv = StaticMethod.GetService("StockRelationSrv") as IStockRelationSrv;
        }

        #region 月度盘点单
        /// <summary>
        /// 保存月度盘点单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public StockInventoryMaster SaveStockInventoryMaster(StockInventoryMaster obj, IList movedDtlList)
        {
            return stockInventorySrv.SaveStockInventory(obj, movedDtlList);
        }

        #endregion

        //#region 料具收料
        //public MaterialCollectionMaster SaveMaterialCollection(MaterialCollectionMaster obj, IList list)
        //{
        //    //return matMngSrv.SaveMaterialCollectionMaster(obj, list);
        //}
        //#endregion
    }
}
