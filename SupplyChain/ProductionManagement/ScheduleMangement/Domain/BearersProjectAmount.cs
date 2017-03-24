using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    /// <summary>
    /// 承担者工程量集
    /// </summary>
    public class BearersProjectAmount
    {
        //【工程任务确认明细】、【分包合同】、【工程量】、【累积形象进度】
        /// <summary>
        /// 工程任务确认明细
        /// </summary>
        public GWBSTaskConfirm ProjectTaskConfirmDetail { get; set; }
        /// <summary>
        /// 分包合同（承担队伍）
        /// </summary>
        public SubContractProject ProjectSubContract { get; set; }
        /// <summary>
        /// 工程量（确认后）
        /// </summary>
        public decimal ProjectAmount { get; set; }
        /// <summary>
        /// 累积形象进度（确认后）
        /// </summary>
        public decimal ProgressAfterConfirm { get; set;}

    }
}
