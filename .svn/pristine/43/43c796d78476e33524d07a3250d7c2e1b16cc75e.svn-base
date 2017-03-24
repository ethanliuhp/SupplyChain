using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialManage.Service;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement.ScheduleMangement.SpecialCostMng.SpecialAccount
{
    public class MSpecialAccount
    {
        private ISpecialCostSrv specialCostSrv;
        public ISpecialCostSrv SpecialCostSrv
        {
            get { return specialCostSrv; }
            set { specialCostSrv = value; }
        }
        public MSpecialAccount()
        {
            if (specialCostSrv == null)
            {
                specialCostSrv = StaticMethod.GetService("SpecialCostSrv") as ISpecialCostSrv;
            }
        }
        /// <summary>
        /// 保存专项费用管理信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SpeCostSettlementMaster SaveSpecialAccount(SpeCostSettlementMaster obj)
        {
            return specialCostSrv.SaveSpecialAccount(obj);
        }

    }
}
