using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborDemandPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement
{
    public class MLaborDemandPlanMng
    {
        private ILaborDemandPlanSrv laborDemandPlanSrv;
        public ILaborDemandPlanSrv LaborDemandPlanSrv
        {
            get { return laborDemandPlanSrv; }
            set { laborDemandPlanSrv = value; }
        }
        public MLaborDemandPlanMng()
        {
            if (laborDemandPlanSrv == null)
            {
                laborDemandPlanSrv = StaticMethod.GetService("LaborDemandPlanSrv") as ILaborDemandPlanSrv;
            }
        }
        /// <summary>
        /// 保存劳务需求计划信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public LaborDemandPlanMaster SaveLaborDemandPlan(LaborDemandPlanMaster obj)
        {
            return laborDemandPlanSrv.SaveLaborDemandPlan(obj);
        }

        /// <summary>
        /// 保存废旧物资处理信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //public WasteMatProcessMaster saveWasteMatHandle(WasteMatProcessMaster obj, IList movedDtlList)
        //{
        //    return wasteMatSrv.saveWasteMatProcess(obj,movedDtlList);
        //}

    }
}
