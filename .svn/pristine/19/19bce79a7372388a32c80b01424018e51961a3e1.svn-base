using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement
{
    public class MInspectionLotMng
    {
        private IInspectionLotSrv inspectionLotSrv;


        public IInspectionLotSrv InspectionLotSrv
        {
            get { return inspectionLotSrv; }
            set { inspectionLotSrv = value; }
        }

        public MInspectionLotMng()
        {
            if (inspectionLotSrv == null)
            {
                inspectionLotSrv = StaticMethod.GetService("InspectionLotSrv") as IInspectionLotSrv;
            }
        }

        #region 检验批
        /// <summary>
        /// 保存检验批
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public InspectionLot SaveInspectionLotMaster(InspectionLot obj)
        {
            return inspectionLotSrv.SaveInspectionLot(obj);
        }

        #endregion
    }
}
