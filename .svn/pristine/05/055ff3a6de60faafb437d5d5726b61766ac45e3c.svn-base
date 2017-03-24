using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.SupplyManage.MonthlyPlanManage.Domain
{
    /// <summary>
    /// 月度需求计划明细
    /// </summary>
    [Serializable]
    public class MonthlyPlanDetail : BaseSupplyDetail
    {
        private string materialType;
        private GWBSTree projectTask;
        private string projectTaskName;
        private string projectTaskSysCode;

        private decimal leftQuantity;
        private decimal needQuantity;
        private decimal realInQuantity;

        private string specialType;

        /// <summary>
        /// 专业分类
        /// </summary>
        virtual public string SpecialType
        {
            get { return specialType; }
            set { specialType = value; }
        }


        /// <summary>
        /// 需求数量
        /// </summary>
        virtual public decimal NeedQuantity
        {
            get { return needQuantity; }
            set { needQuantity = value; }
        }

        /// <summary>
        /// 实际进场量数量
        /// </summary>
        virtual public decimal RealInQuantity
        {
            get { return realInQuantity; }
            set { realInQuantity = value; }
        }

        /// <summary>
        /// 材料类别
        /// </summary>
        virtual public string MaterialType
        {
            get { return materialType; }
            set { materialType = value; }
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
        /// 剩余数量
        /// </summary>
        virtual public decimal LeftQuantity
        {
            get { return leftQuantity; }
            set { leftQuantity = value; }
        }
    }
}
