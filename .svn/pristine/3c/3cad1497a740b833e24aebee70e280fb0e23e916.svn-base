using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.ComponentModel;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{

    /// <summary>
    ///  推延规则
    /// </summary>
    public enum EnumDelayRule
    {
        [Description("FS(完成-开始)")]
        FS = 1,
        [Description("SS(开始-开始)")]
        SS = 2,
        [Description("FF(完成-完成)")]
        FF = 3
    }

    /// <summary>
    /// 推延关系表（前置节点）
    /// </summary>
    [Serializable]
    [Entity]
    public class WeekScheduleRalation
    {
        private string _id;
        /// <summary>主键</summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private WeekScheduleDetail _master;
        /// <summary>进度计划明细</summary>
        public virtual WeekScheduleDetail Master
        {
            get { return _master; }
            set { _master = value; }
        }

        private WeekScheduleDetail _frontWeekScheduleDetail;
        /// <summary>前置进度计划明细</summary>
        public virtual WeekScheduleDetail FrontWeekScheduleDetail
        {
            get { return _frontWeekScheduleDetail; }
            set { _frontWeekScheduleDetail = value; }
        }

        private EnumDelayRule _delayRule;
        /// <summary>推延规则(1=FS,2=SS,3=FF)</summary>
        public virtual EnumDelayRule DelayRule
        {
            get { return _delayRule; }
            set { _delayRule = value; }
        }

        private int _delayDays;
        /// <summary></summary>
        public virtual int DelayDays
        {
            get { return _delayDays; }
            set { _delayDays = value; }
        }


        #region 扩展字段
        /// <summary>
        /// 前置任务节点行号
        /// </summary>
        public virtual int PreNodeRowIndex { set; get; }
        /// <summary>
        /// 前置任务节点名称
        /// </summary>
        public virtual int PreNodeName { set; get; }
        #endregion

    }
}
