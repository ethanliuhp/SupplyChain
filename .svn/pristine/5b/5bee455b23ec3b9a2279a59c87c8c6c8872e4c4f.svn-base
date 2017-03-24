using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;

namespace Application.Business.Erp.SupplyChain.SupplyManage.DailyPlanManage.Domain
{
    /// <summary>
    /// 日常需求计划明细
    /// </summary>
    [Serializable]
    public class DailyPlanDetail : BaseSupplyDetail
    {
        private GWBSTree projectTask;
        private string projectTaskName;
        private string projectTaskSysCode;

        private DateTime? approachDate;
        private string manufacturer;
        private decimal leftQuantity;
        private decimal sendBackQuantity;
        private SupplyOrderDetail supplyOrderDetail;
        private decimal supplyQuantity;

        /// <summary>
        /// 采购计划总量
        /// </summary>
        public virtual decimal SupplyQuantity
        {
            get { return supplyQuantity; }
            set { supplyQuantity = value; }
        }
        /// <summary>
        /// 采购合同明细
        /// 收料入库引用日常需求计划时使用
        /// </summary>
        public virtual SupplyOrderDetail SupplyOrderDetail
        {
            get { return supplyOrderDetail; }
            set { supplyOrderDetail = value; }
        }

        /// <summary>
        /// 工程项目任务
        /// </summary>
        virtual public GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        /// <summary>
        /// 工程项目任务名称
        /// </summary>
        virtual public string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        /// <summary>
        /// 项目任务层次码
        /// </summary>
        public virtual string ProjectTaskSysCode
        {
            get { return projectTaskSysCode; }
            set { projectTaskSysCode = value; }
        }

        /// <summary>
        /// 进场时间
        /// </summary>
        virtual public DateTime? ApproachDate
        {
            get { return approachDate; }
            set { approachDate = value; }
        }

        /// <summary>
        /// 剩余数量
        /// </summary>
        virtual public decimal LeftQuantity
        {
            get { return leftQuantity; }
            set { ;leftQuantity = value; }
        }
        /// <summary>
        /// 退减数量
        /// </summary>
        virtual public decimal SendBackQuantity
        {
            get { return sendBackQuantity; }
            set { sendBackQuantity = value; }
        }
    }
}
