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

namespace Application.Business.Erp.SupplyChain.Client.FinanceMng.IncomeSettlementMng
{
    public class MIncomeSettlementMng
    {
        private IIncomeSettlementSrv incomeSettlementSrv;

        public IIncomeSettlementSrv IncomeSettlementSrv
        {
            get { return incomeSettlementSrv; }
            set { incomeSettlementSrv = value; }
        }

        public MIncomeSettlementMng()
        {
            if (incomeSettlementSrv == null)
            {
                incomeSettlementSrv = StaticMethod.GetService("IncomeSettlementSrv") as IIncomeSettlementSrv;
            }
        }

        #region 当期收益结算单
        /// <summary>
        /// 保存当期收益结算单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IncomeSettlementMaster SaveIncomeSettlementMaster(IncomeSettlementMaster obj)
        {
            return incomeSettlementSrv.SaveIncomeSettlementMaster(obj);
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
