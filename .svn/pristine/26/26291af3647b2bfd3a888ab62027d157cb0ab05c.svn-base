using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain
{
    /// <summary>
    /// 工程任务核算单
    /// </summary>
    [Serializable]
    public class ProjectTaskAccountBill
    {
        private string _id;
        private long _version;
        private string _theProjectGUID;
        private string _theProjectName;

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        public virtual long Version
        {
            get { return _version; }
            set { _version = value; }
        }
        /// <summary>
        /// 项目GUID
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

        private string _code;
        /// <summary>
        /// 编号
        /// </summary>
        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        private DateTime _createTime = DateTime.Now;
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        private PersonInfo _accountPersonGUID;
        /// <summary>
        /// 核算人GUID
        /// </summary>
        public virtual PersonInfo AccountPersonGUID
        {
            get { return _accountPersonGUID; }
            set { _accountPersonGUID = value; }
        }
        private string _accountPersonName;
        /// <summary>
        /// 核算人名称
        /// </summary>
        public virtual string AccountPersonName
        {
            get { return _accountPersonName; }
            set { _accountPersonName = value; }
        }
        private string _accountPersonOrgSysCode;
        /// <summary>
        /// 核算人组织层次码
        /// </summary>
        public virtual string AccountPersonOrgSysCode
        {
            get { return _accountPersonOrgSysCode; }
            set { _accountPersonOrgSysCode = value; }
        }
        private string _theOrgName;
        /// <summary>
        /// 组织名称
        /// </summary>
        public virtual string TheOrgName
        {
            get { return _theOrgName; }
            set { _theOrgName = value; }
        }
        private GWBSTree _accountRange;
        /// <summary>
        /// 核算范围,核算的GWBS树节点,此处存GUID
        /// </summary>
        public virtual GWBSTree AccountRange
        {
            get { return _accountRange; }
            set { _accountRange = value; }
        }
        private string _accountTaskName;
        /// <summary>
        /// 核算的GWBS树节点名称
        /// </summary>
        public virtual string AccountTaskName
        {
            get { return _accountTaskName; }
            set { _accountTaskName = value; }
        }

        private DateTime _beginTime;
        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime BeginTime
        {
            get { return _beginTime; }
            set { _beginTime = value; }
        }
        private DateTime _endTime;
        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
        private DateTime _billingTime = DateTime.Now;
        /// <summary>
        /// 开单时间
        /// </summary>
        public virtual DateTime BillingTime
        {
            get { return _billingTime; }
            set { _billingTime = value; }
        }

        private ProjectTaskAccountBillState _state;
        /// <summary>
        /// 状态
        /// </summary>
        public virtual ProjectTaskAccountBillState State
        {
            get { return _state; }
            set { _state = value; }
        }
        private CurrencyType _currency;
        /// <summary>
        /// 币种
        /// </summary>
        public virtual CurrencyType Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }
        private decimal _exchangeRate;
        /// <summary>
        /// 汇率
        /// </summary>
        public virtual decimal ExchangeRate
        {
            get { return _exchangeRate; }
            set { _exchangeRate = value; }
        }
        private string _remark;
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }


        ISet<ProjectTaskDetailAccount> _Details = new HashedSet<ProjectTaskDetailAccount>();
        /// <summary>
        /// 工程任务明细核算
        /// </summary>
        public virtual ISet<ProjectTaskDetailAccount> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        ISet<ProjectTaskDetailAccountSummary> _listSummary = new HashedSet<ProjectTaskDetailAccountSummary>();
        /// <summary>
        /// 工程任务明细核算汇总
        /// </summary>
        public virtual ISet<ProjectTaskDetailAccountSummary> ListSummary
        {
            get { return _listSummary; }
            set { _listSummary = value; }
        }

    }
    /// <summary>
    /// 工程任务核算单状态
    /// </summary>
    public enum ProjectTaskAccountBillState
    {
        [Description("制定")]
        制定 = 0,
        /// <summary>
        /// 提交审批
        /// </summary>
        [Description("提交")]
        提交 = 1,
        /// <summary>
        /// 通过审批
        /// </summary>
        [Description("发布")]
        发布 = 2,
        [Description("冻结")]
        冻结 = 3,
        [Description("作废")]
        作废 = 4
    }
    /// <summary>
    /// 币种
    /// </summary>
    public enum CurrencyType
    {
        [Description("人民币")]
        人民币 = 0,
        [Description("美元")]
        美元 = 1
    }
}
