using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 计划工程量值对象
    /// </summary>
    public class PlanProjectsAmountValueObj
    {
        //【工程任务明细GUID】、【工程任务明细名称】、【主资源类型】、【成本项】、【计划工程量】、【分摊汇总量】、【本次分摊汇总量】

        /// <summary>
        /// 工程任务明细GUID
        /// </summary>
        public GWBSDetail ProjectTaskDetailGUID { get; set; }
        /// <summary>
        /// 工程任务明细名称
        /// </summary>
        public string ProjectTaskDetaikName { get; set; }
        /// <summary>
        /// 主资源类型ID
        /// </summary>
        public string MainResourceTypeId { get; set; }
        /// <summary>
        /// 主资源类型名称
        /// </summary>
        public string MainResourceTypeName { get; set; }

        /// <summary>
        /// 主资源类型规格
        /// </summary>
        public string MainResourceTypeSpec { get; set; }
        /// <summary>
        /// 成本项
        /// </summary>
        public CostItem CostItems { get; set; }
        /// <summary>
        /// 图号
        /// </summary>
        public string DiagramNumber { get; set; }
        /// <summary>
        /// 计划工程量
        /// </summary>
        public decimal PlanProjectAmount { get; set; }
        /// <summary>
        /// 分摊汇总量
        /// </summary>
        public decimal SharingSummaryAmount { get; set; }
        /// <summary>
        /// 旧分摊汇总量
        /// </summary>
        public decimal OldSharingSummaryAmount { get; set; }
        /// <summary>
        /// 本次分摊汇总量
        /// </summary>
        public decimal TheSharingSummaryAmount { get; set; }
    }
}
