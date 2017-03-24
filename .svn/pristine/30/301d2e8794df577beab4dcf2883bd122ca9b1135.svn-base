using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.MaterialResource.Domain;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 工程任务明细台账
    /// </summary>
    [Serializable]
    public class GWBSDetailLedger
    {
        private string id;
        private long version;

        private string _projectTaskID;
        private string _projectTaskName;
        private string _theProjectTaskSysCode;
        private string _projectTaskDtlID;
        private string _projectTaskDtlName;
        private decimal _contractWorkAmount;
        private decimal _contractPrice;
        private decimal _contractTotalPrice;
        private decimal _responsibleWorkAmount;
        private decimal _responsiblePrice;
        private decimal _responsibleTotalPrice;
        private decimal _planWorkAmount;
        private decimal _planPrice;
        private decimal _planTotalPrice;
        private StandardUnit _workAmountUnit;
        private string _workAmountUnitName;
        private StandardUnit _priceUnit;
        private string _priceUnitName;
        private string _theProjectGUID;
        private string _theProjectName;
        private DateTime _createTime = DateTime.Now;
        private ContractGroup _theContractGroup;
        private string temp;
        private string temp1;
        private string temp2;
        private string temp3;
        private decimal temp4;
        private decimal temp5;

        private ContractIncomeChangeModeEnum _contractChangeMode = ContractIncomeChangeModeEnum.合同初始值;
        private PlanCostChangeModeEnum _planCostChangeMode = PlanCostChangeModeEnum.计划成本初始值;
        private ResponsibleCostChangeModeEnum _responsibleCostChangeMode = ResponsibleCostChangeModeEnum.责任成本初始值;

        /// <summary>
        /// 合同收入变更方式
        /// </summary>
        public virtual ContractIncomeChangeModeEnum ContractChangeMode
        {
            get { return _contractChangeMode; }
            set { _contractChangeMode = value; }
        }
        /// <summary>
        /// 计划成本变更方式
        /// </summary>
        public virtual PlanCostChangeModeEnum PlanCostChangeMode
        {
            get { return _planCostChangeMode; }
            set { _planCostChangeMode = value; }
        }
        /// <summary>
        /// 责任成本变更方式
        /// </summary>
        public virtual ResponsibleCostChangeModeEnum ResponsibleCostChangeMode
        {
            get { return _responsibleCostChangeMode; }
            set { _responsibleCostChangeMode = value; }
        }


        /// <summary>
        /// 临时预计效益
        /// </summary>
        virtual public decimal Temp5
        {
            get { return temp5; }
            set { temp5 = value; }
        }
        /// <summary>
        /// 临时预计效益率
        /// </summary>
        virtual public decimal Temp4
        {
            get { return temp4; }
            set { temp4 = value; }
        }
        /// <summary>
        /// 临时数据契约组编号
        /// </summary>
        virtual public string Temp3
        {
            get { return temp3; }
            set { temp3 = value; }
        }
        /// <summary>
        /// 临时数据成本项
        /// </summary>
        virtual public string Temp2
        {
            get { return temp2; }
            set { temp2 = value; }
        }
        /// <summary>
        /// 临时数据契约组类型
        /// </summary>
        virtual public string Temp1
        {
            get { return temp1; }
            set { temp1 = value; }
        }
        /// <summary>
        /// 临时数据成本项
        /// </summary>
        virtual public string Temp
        {
            get { return temp; }
            set { temp = value; }
        }
        /// <summary>
        /// 临时数据项目任务的完整路径
        /// </summary>
        virtual public string Temp_WBSFullPath { set; get; }
        /// <summary>
        /// 临时数据工程任务明细
        /// </summary>
        virtual public GWBSDetail Temp_WBSDtl { set; get; }

        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 工程项目任务
        /// </summary>
        public virtual string ProjectTaskID
        {
            get { return _projectTaskID; }
            set { _projectTaskID = value; }
        }
        /// <summary>
        /// 工程任务名称
        /// </summary>
        public virtual string ProjectTaskName
        {
            get { return _projectTaskName; }
            set { _projectTaskName = value; }
        }
        /// <summary>
        /// 所属工程WBS结构层次码
        /// </summary>
        public virtual string TheProjectTaskSysCode
        {
            get { return _theProjectTaskSysCode; }
            set { _theProjectTaskSysCode = value; }
        }
        /// <summary>
        /// 工程任务明细
        /// </summary>
        public virtual string ProjectTaskDtlID
        {
            get { return _projectTaskDtlID; }
            set { _projectTaskDtlID = value; }
        }
        /// <summary>
        /// 工程任务明细名称
        /// </summary>
        public virtual string ProjectTaskDtlName
        {
            get { return _projectTaskDtlName; }
            set { _projectTaskDtlName = value; }
        }

        /// <summary>
        /// 合同工程量
        /// </summary>
        public virtual decimal ContractWorkAmount
        {
            get { return _contractWorkAmount; }
            set { _contractWorkAmount = value; }
        }

        /// <summary>
        /// 合同单价
        /// </summary>
        public virtual decimal ContractPrice
        {
            get { return _contractPrice; }
            set { _contractPrice = value; }
        }

        /// <summary>
        /// 合同合价
        /// </summary>
        public virtual decimal ContractTotalPrice
        {
            get { return _contractTotalPrice; }
            set { _contractTotalPrice = value; }
        }

        /// <summary>
        /// 责任工程量
        /// </summary>
        public virtual decimal ResponsibleWorkAmount
        {
            get { return _responsibleWorkAmount; }
            set { _responsibleWorkAmount = value; }
        }

        /// <summary>
        /// 责任单价
        /// </summary>
        public virtual decimal ResponsiblePrice
        {
            get { return _responsiblePrice; }
            set { _responsiblePrice = value; }
        }

        /// <summary>
        /// 责任合价
        /// </summary>
        public virtual decimal ResponsibleTotalPrice
        {
            get { return _responsibleTotalPrice; }
            set { _responsibleTotalPrice = value; }
        }

        /// <summary>
        /// 计划工程量
        /// </summary>
        public virtual decimal PlanWorkAmount
        {
            get { return _planWorkAmount; }
            set { _planWorkAmount = value; }
        }

        /// <summary>
        /// 计划单价
        /// </summary>
        public virtual decimal PlanPrice
        {
            get { return _planPrice; }
            set { _planPrice = value; }
        }

        /// <summary>
        /// 计划合价
        /// </summary>
        public virtual decimal PlanTotalPrice
        {
            get { return _planTotalPrice; }
            set { _planTotalPrice = value; }
        }

        /// <summary>
        /// 工程量计量单位
        /// </summary>
        public virtual StandardUnit WorkAmountUnit
        {
            get { return _workAmountUnit; }
            set { _workAmountUnit = value; }
        }

        /// <summary>
        /// 工程量计量单位名称
        /// </summary>
        public virtual string WorkAmountUnitName
        {
            get { return _workAmountUnitName; }
            set { _workAmountUnitName = value; }
        }

        /// <summary>
        /// 价格计量单位
        /// </summary>
        public virtual StandardUnit PriceUnit
        {
            get { return _priceUnit; }
            set { _priceUnit = value; }
        }

        /// <summary>
        /// 价格计量单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return _priceUnitName; }
            set { _priceUnitName = value; }
        }

        /// <summary>
        /// 所属项目
        /// </summary>
        public virtual string TheProjectGUID
        {
            get { return _theProjectGUID; }
            set { _theProjectGUID = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string TheProjectName
        {
            get { return _theProjectName; }
            set { _theProjectName = value; }
        }
        /// <summary>
        /// 创建(登记)时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        /// <summary>
        /// 依据契约组
        /// </summary>
        public virtual ContractGroup TheContractGroup
        {
            get { return _theContractGroup; }
            set { _theContractGroup = value; }
        }
    }
    /// <summary>
    /// 合同收入变更方式
    /// </summary>
    public enum ContractIncomeChangeModeEnum
    {
        [Description("合同初始值")]
        合同初始值 = 0,
        [Description("合同收入无变化")]
        合同收入无变化 = 1,
        [Description("合同收入工程量变化")]
        合同收入工程量变化 = 2,
        [Description("合同单价变化")]
        合同单价变化 = 3
    }
    /// <summary>
    /// 责任成本变更方式
    /// </summary>
    public enum ResponsibleCostChangeModeEnum
    {
        [Description("责任成本初始值")]
        责任成本初始值 = 0,
        [Description("责任成本无变化")]
        责任成本无变化 = 1,
        [Description("责任工程量变化")]
        责任工程量变化 = 2,
        [Description("责任单价变化")]
        责任单价变化 = 3
    }
    /// <summary>
    /// 计划成本变更方式
    /// </summary>
    public enum PlanCostChangeModeEnum
    {
        [Description("计划成本初始值")]
        计划成本初始值 = 0,
        [Description("计划成本无变化")]
        计划成本无变化 = 1,
        [Description("计划工程量变化")]
        计划工程量变化 = 2,
        [Description("计划单价变化")]
        计划单价变化 = 3
    }
}
