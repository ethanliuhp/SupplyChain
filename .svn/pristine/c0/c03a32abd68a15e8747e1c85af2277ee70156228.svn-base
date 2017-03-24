using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    /// <summary>
    /// 自行产值明细表
    /// </summary>
    [Serializable]
    [Entity]
    public class ProduceSelfValueDetail : BaseDetail
    {
        private string id;
        private GWBSTree _GWBSTree;
        private string _GWBSTreeName;
        private string _GWBSTreeSysCode;
        private decimal planValue;
        private decimal realValue;
        private decimal planProgress;
        private decimal realProgress;

        /// <summary>
        /// 实际形象进度
        /// </summary>
        public virtual decimal RealProgress
        {
            get { return realProgress; }
            set { realProgress = value; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 计划产值
        /// </summary>
        public virtual decimal PlanValue
        {
            get { return planValue; }
            set { planValue = value; }
        }

        /// <summary>
        /// 实际产值
        /// </summary>
        public virtual decimal RealValue
        {
            get { return realValue; }
            set { realValue = value; }
        }

        /// <summary>
        /// 计划形象进度
        /// </summary>
        public virtual decimal PlanProgress
        {
            get { return planProgress; }
            set { planProgress = value; }
        }

        /// <summary>
        /// 工程任务名称
        /// </summary>
        public virtual string GWBSTreeName
        {
            get { return _GWBSTreeName; }
            set { _GWBSTreeName = value; }
        }

        /// <summary>
        /// 工程任务层次码
        /// </summary>
        public virtual string GWBSTreeSysCode
        {
            get { return _GWBSTreeSysCode; }
            set { _GWBSTreeSysCode = value; }
        }

        /// <summary>
        /// 工程任务
        /// </summary>
        public virtual GWBSTree GWBSTree
        {
            get { return _GWBSTree; }
            set { _GWBSTree = value; }
        }

        /// <summary>
        /// 会计年，临时使用，不做map
        /// </summary>
        public virtual int Kjn { get; set; }
        /// <summary>
        /// 会计月，临时使用，不做map
        /// </summary>
        public virtual int Kjy { get; set; }
    }
}

