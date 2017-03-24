using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.SupplyManage.DemandMasterPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng
{
    public enum EnumDemandType
    {
        /// <summary>
        /// 需求总计划查询
        /// </summary>
        demandSearch,
        /// <summary>
        /// 公司需求总计划查询
        /// </summary>
        companyDemandSearch,
        companySupplyPlan,
        /// <summary>
        /// 土建 单据
        /// </summary>
        土建,
        /// <summary>
        /// 安装 单据
        /// </summary>
        安装,

    }
    public class MDemandMasterPlanMng
    {
        private IDemandPlanSrv demandPlanSrv;

        public IDemandPlanSrv DemandPlanSrv
        {
            get { return demandPlanSrv; }
            set { demandPlanSrv = value; }
        }

        public MDemandMasterPlanMng()
        {
            if (demandPlanSrv == null)
            {
                demandPlanSrv = StaticMethod.GetService("DemandPlanSrv") as IDemandPlanSrv;
            }
        }

        #region 需求总计划
        /// <summary>
        /// 保存需求总计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DemandMasterPlanMaster SaveDemandMasterPlanMaster(DemandMasterPlanMaster obj, IList movedDtlList)
        {
            return demandPlanSrv.SaveDemandMasterPlan(obj, movedDtlList);
        }


        /// <summary>
        /// 保存需求总计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DemandMasterPlanMaster SaveDemandMasterPlanMaster(DemandMasterPlanMaster obj)
        {
            return demandPlanSrv.SaveDemandMasterPlan(obj);
        }
        #endregion
    }
}
