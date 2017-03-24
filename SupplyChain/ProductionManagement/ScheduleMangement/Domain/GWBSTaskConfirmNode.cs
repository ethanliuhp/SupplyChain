using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    /// <summary>
    /// 工程量确认 生产节点
    /// </summary>
    [Serializable]
    [Entity]
    public class GWBSTaskConfirmNode
    {
        private string id;
        private GWBSTaskConfirm _GWBSTaskConfirm;
        private string _GWBSTreeName;
        private decimal progress;
        private string _GWBSTree;

        /// <summary>
        /// 周计划所属的工程任务
        /// </summary>
        public virtual string GWBSTree
        {
            get { return _GWBSTree; }
            set { _GWBSTree = value; }
        }

        /// <summary>
        /// 确认累积工程形象进度 {周进度计划明细}_【确认累积工程形象进度】
        /// </summary>
        public virtual decimal Progress
        {
            get { return progress; }
            set { progress = value; }
        }

        /// <summary>
        /// 周进度计划明细}>所关联{工程项目任务}_【工程项目任务名称】
        /// </summary>
        public virtual string GWBSTreeName
        {
            get { return _GWBSTreeName; }
            set { _GWBSTreeName = value; }
        }

        /// <summary>
        /// 工程量确认明细
        /// </summary>
        public virtual GWBSTaskConfirm GWBSTaskConfirm
        {
            get { return _GWBSTaskConfirm; }
            set { _GWBSTaskConfirm = value; }
        }

        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
