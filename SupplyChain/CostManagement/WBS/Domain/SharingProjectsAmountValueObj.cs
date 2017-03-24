using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 分摊工程量值对象
    /// </summary>
    public class SharingProjectsAmountValueObj
    {
        //【叶节点路径】、【工程任务明细GUID】、【工程任务明细名称】、【主资源类型】、【成本项】、【已有工程量】、【本次分摊工程量】

        /// <summary>
        /// 叶节点路径
        /// </summary>
        public string LeafNodePath { get; set; }
        /// <summary>
        /// 节点
        /// </summary>
        public GWBSTree Node { get; set; }
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
        /// 成本项
        /// </summary>
        public CostItem CostItems { get; set; }
        /// <summary>
        /// 图号
        /// </summary>
        public string DiagramNumber { get; set; }
        /// <summary>
        /// 已有工程量
        /// </summary>
        public decimal ExistingProjectAmount { get; set; }
        /// <summary>
        /// 本次分摊工程量
        /// </summary>
        public decimal TheSharingSummaryAmount { get; set; }
        
        private bool showflag = true;
        /// <summary>
        /// 一个标记
        /// </summary>
        public bool Showflag
        {
            get { return showflag; }
            set { showflag = value; }
        }
        /// <summary>
        /// 临时记录 当前对象对应列表下标
        /// </summary>
        public int rowIndex { get; set; }
    }
}
