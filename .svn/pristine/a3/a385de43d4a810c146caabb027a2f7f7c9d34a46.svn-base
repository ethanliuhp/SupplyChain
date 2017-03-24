using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain
{
    /// <summary>
    /// 计划类型
    /// </summary>
    public enum EnumPlanType
    {
        月度计划,
        节点计划
    }

    /// <summary>
    /// 月度需求计划主表
    /// </summary>
    [Serializable]
    public class MonthlyPlanMaster : BaseSupplyMaster
    {
        private string specialType;
        /// <summary>
        /// 专业分类
        /// </summary>
        virtual public string SpecialType
        {
            get { return specialType; }
            set { specialType = value; }
        }

        private string special;
        /// <summary>
        /// 专业 用于区分单据
        /// </summary>
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }

        private int year;
        private int month;
        /// <summary>
        /// 年份
        /// </summary>
        virtual public int Year
        {
            get { return year; }
            set { year = value; }
        }
        /// <summary>
        /// 月份
        /// </summary>
        virtual public int Month
        {
            get { return month; }
            set { month = value; }
        }

        private string name;
        /// <summary>
        /// 计划名称
        /// </summary>
        public virtual string PlanName
        {
            get { return name; }
            set { name = value; }
        }
        private string monthePlanType;
        /// <summary>
        /// 计划类型（区分月度和节点，取值“月度计划”或“节点计划”）
        /// </summary>
        virtual public string MonthePlanType
        {
            get { return monthePlanType; }
            set { monthePlanType = value; }
        }
    }

}
