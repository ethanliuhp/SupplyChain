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
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Service;

namespace Application.Business.Erp.SupplyChain.Client.SettlementManagement.MaterialSettleMng
{
    public enum EnumMaterialSettleType
    {
        /// <summary>
        /// 物资耗用查询
        /// </summary>
        materialSettleQuery,
        /// <summary>
        /// 料具租赁查询
        /// </summary>
        materialQuery,
        /// <summary>
        /// 物资耗用结算单维护 单据
        /// </summary>
        物资耗用结算单维护,
        /// <summary>
        /// 料具结算单维护 单据
        /// </summary>
        料具结算单维护
    }
    public class MMaterialSettleMng
    {
        
        private IMaterialSettleSrv materialSettleSrv;

        public IMaterialSettleSrv MaterialSettleSrv
        {
            get { return materialSettleSrv; }
            set { materialSettleSrv = value; }
        }

        public MMaterialSettleMng()
        {
            if (materialSettleSrv == null)
            {
                materialSettleSrv = StaticMethod.GetService("MaterialSettleSrv") as IMaterialSettleSrv;
            }
        }

        #region 物资耗用结算单
        /// <summary>
        /// 保存物资耗用结算单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MaterialSettleMaster SaveMaterialSettleMaster(MaterialSettleMaster obj)
        {
            return materialSettleSrv.SaveMaterialSettle(obj);
        }

        #endregion
    }
}
