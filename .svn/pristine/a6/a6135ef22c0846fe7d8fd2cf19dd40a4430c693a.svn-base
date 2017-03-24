using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng
{
    public enum EnumDailyType
    {
        /// <summary>
        /// 日常需求计划查询
        /// </summary>
        dailySearch,
        /// <summary>
        /// 土建 单据
        /// </summary>
        土建,
        /// <summary>
        /// 安装 单据
        /// </summary>
        安装,

    }
    public class MDailyPlanMng
    {
        private IDailyPlanSrv dailyPlanSrv;

        public IDailyPlanSrv DailyPlanSrv
        {
            get { return dailyPlanSrv; }
            set { dailyPlanSrv = value; }
        }

        public MDailyPlanMng()
        {
            if (dailyPlanSrv == null)
            {
                dailyPlanSrv = StaticMethod.GetService("DailyPlanSrv") as IDailyPlanSrv;
            }
        }

        #region 日常需求计划
        /// <summary>
        /// 保存日常需求计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DailyPlanMaster SaveDailyPlanMaster(DailyPlanMaster obj)
        {
            return dailyPlanSrv.SaveDailyPlan(obj);
        }

        /// <summary>
        /// 保存日常需求计划
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DailyPlanMaster SaveDailyPlanMaster(DailyPlanMaster obj, IList movedDtlList)
        {
            return dailyPlanSrv.SaveDailyPlan(obj, movedDtlList);
        }
        #endregion
    }
}
