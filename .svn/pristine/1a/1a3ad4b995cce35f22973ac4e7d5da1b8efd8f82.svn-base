using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.SupplyManage.ContractAdjustPriceManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.Client.StockMng;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng
{
    public class MContractAdjustPriceMng
    {
        private IContractAdjustPriceSrv contractAdjustPriceSrv;

        public IContractAdjustPriceSrv ContractAdjustPriceSrv
        {
            get { return contractAdjustPriceSrv; }
            set { contractAdjustPriceSrv = value; }
        }

        public MContractAdjustPriceMng()
        {
            if (contractAdjustPriceSrv == null)
            {
                contractAdjustPriceSrv = StaticMethod.GetService("ContractAdjustPriceSrv") as IContractAdjustPriceSrv;
            }
        }

        #region 采购合同调价
        /// <summary>
        /// 保存采购合同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ContractAdjustPrice SaveContractAdjustPrice(ContractAdjustPrice obj)
        {
            return contractAdjustPriceSrv.SaveContractAdjustPrice(obj);
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
