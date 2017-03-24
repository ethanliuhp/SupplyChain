using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.MaterialResource.Domain;

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

        private GWBSTree _theProjectTask;
        private string _theProjectTaskSysCode;
        private GWBSDetail _theProjectTaskDtl;
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
        public virtual GWBSTree TheProjectTask
        {
            get { return _theProjectTask; }
            set { _theProjectTask = value; }
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
        public virtual GWBSDetail TheProjectTaskDtl
        {
            get { return _theProjectTaskDtl; }
            set { _theProjectTaskDtl = value; }
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
}
