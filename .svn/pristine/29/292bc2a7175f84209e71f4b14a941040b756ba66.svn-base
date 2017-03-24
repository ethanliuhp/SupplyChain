using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.StockManage.DetectionReceiptManage.Domain;
using Application.Business.Erp.SupplyChain.StockManage.DetectionReceiptManage.Service;

namespace Application.Business.Erp.SupplyChain.Client.StockMng.DetectionReceiptMng
{
    public class MDetectionReceiptMng
    {
        private IDetectionReceiptSrv detectionReceiptSrv;

        public IDetectionReceiptSrv DetectionReceiptSrv
        {
            get { return detectionReceiptSrv; }
            set { detectionReceiptSrv = value; }
        }

        public MDetectionReceiptMng()
        {
            if (detectionReceiptSrv == null)
            {
                detectionReceiptSrv = StaticMethod.GetService("DetectionReceiptSrv") as IDetectionReceiptSrv;
            }
        }

        #region 检测回执单
        /// <summary>
        /// 保存检测回执单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DetectionReceiptMaster SaveDetectionReceiptMaster(DetectionReceiptMaster obj)
        {
            return detectionReceiptSrv.SaveDetectionReceipt(obj);
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
