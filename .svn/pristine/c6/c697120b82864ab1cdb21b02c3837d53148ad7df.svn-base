using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.InspectionLotMng.Domain
{
    /// <summary>
    /// 检验批明细
    /// </summary>
    [Serializable]
    [Entity]
    public class InspectionLotDetail : BaseDetail
    {

        private GWBSTree projectId;
        private string projectName;
        private string weekScheduleId;
        private string weekScheduleName;

        /// <summary>
        /// 项目Id
        /// </summary>
        public virtual GWBSTree ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        /// <summary>
        /// 周进度计划明细ID
        /// </summary>
        public virtual string WeekScheduleId
        {
            get { return weekScheduleId; }
            set { weekScheduleId = value; }
        }

        /// <summary>
        /// 周进度计划明细名称
        /// </summary>
        public virtual string WeekScheduleName
        {
            get { return weekScheduleName; }
            set { weekScheduleName = value; }
        }
    }
}
