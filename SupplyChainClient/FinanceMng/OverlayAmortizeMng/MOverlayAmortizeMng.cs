using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.SupplyChain.FinanceMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.OverlayAmortizeMng
{
    public class MOverlayAmortizeMng
    {
        private IOverlayAmortizeSrv overlayAmortizeSrv;

        public IOverlayAmortizeSrv OverlayAmortizeSrv
        {
            get { return overlayAmortizeSrv; }
            set { overlayAmortizeSrv = value; }
        }

        public MOverlayAmortizeMng()
        {
            if (overlayAmortizeSrv == null)
            {
                overlayAmortizeSrv = StaticMethod.GetService("OverlayAmortizeSrv") as IOverlayAmortizeSrv;
            }
        }

        #region 临建摊销单
        /// <summary>
        /// 保存临建摊销单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public OverlayAmortizeMaster SaveOverlayAmortizeMaster(OverlayAmortizeMaster obj)
        {
            return overlayAmortizeSrv.SaveOverlayAmortizeMaster(obj);
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
