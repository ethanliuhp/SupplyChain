using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.FinanceMng.Domain;
using Application.Business.Erp.SupplyChain.FinanceMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.MaterialsettlementMng
{
    public class MMaterialSettlementMng
    {
        private IMaterialSettlementSrv materialSettlementSrv;

        public IMaterialSettlementSrv MaterialSettlementSrv
        {
            get { return materialSettlementSrv; }
            set { materialSettlementSrv = value; }
        }

        public MMaterialSettlementMng()
        {
            if (materialSettlementSrv == null)
            {
                materialSettlementSrv = StaticMethod.GetService("MaterialSettlementSrv") as IMaterialSettlementSrv;
            }
        }

        #region 材料结算
        /// <summary>
        /// 保存材料结算
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MaterialSettlementMaster SaveMaterialSettlementMaster(MaterialSettlementMaster obj)
        {
            return materialSettlementSrv.SaveMaterialSettlementMaster(obj);
        }

        #endregion
    }
}
