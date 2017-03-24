using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng
{
    public enum EnumMonthlyType
    {
        /// <summary>
        /// 月度需求计划查询
        /// </summary>
        monthlySearch,
        /// <summary>
        /// 土建 单据
        /// </summary>
        土建,
        /// <summary>
        /// 安装 单据
        /// </summary>
        安装,

    }
    public class MMonthlyPlanMng
    {
        private IMonthlyPlanSrv monthlyPlanSrv;
        public IMonthlyPlanSrv MonthlyPlanSrv
        {
            get { return monthlyPlanSrv; }
            set { monthlyPlanSrv = value; }
        }

        public MMonthlyPlanMng()
        {
            if (monthlyPlanSrv == null)
            {
                monthlyPlanSrv = StaticMethod.GetService("MonthlyPlanSrv") as IMonthlyPlanSrv;
            }
        }

        #region 月度需求计划
        /// <summary>
        /// 保存月度需求计划信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MonthlyPlanMaster SaveMonthlyPlanMng(MonthlyPlanMaster obj)
        {
            return monthlyPlanSrv.SaveMonthlyPlan(obj);
        }
        /// <summary>
        /// 保存月度需求计划信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MonthlyPlanMaster SaveMonthlyPlanMng(MonthlyPlanMaster obj, IList movedDtlList)
        {
            return monthlyPlanSrv.SaveMonthlyPlan(obj, movedDtlList);
        }

        #endregion
    }
}
