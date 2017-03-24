﻿using System;
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
        private string _code;
        private DateTime _createTime = DateTime.Now;
        private PersonInfo _accountPersonGUID;
        private string _accountPersonName;
        private string _accountPersonOrgSysCode;
        private GWBSTree _accountRange;
        private string _accountTaskName;
        private string _accountTaskSyscode;
        private DateTime _beginTime;
        private DateTime _endTime;
        private DateTime _billingTime = DateTime.Now;
        private ProjectTaskAccountBillState _state;
        private string _remark;
        private ISet<ProjectTaskDetailAccount> _Details = new HashedSet<ProjectTaskDetailAccount>();
        private ISet<ProjectTaskDetailAccountSummary> _listSummary = new HashedSet<ProjectTaskDetailAccountSummary>();

        private bool _monthAccountFlag;
        private string _monthAccountBill;

        /// <summary>
        /// 月度核算标志
        /// </summary>
        public virtual bool MonthAccountFlag
        {
            get { return _monthAccountFlag; }
            set { _monthAccountFlag = value; }
        }
        /// <summary>
        /// 月度核算单GUID
        /// </summary>
        public virtual string MonthAccountBill
        {
            get { return _monthAccountBill; }
            set { _monthAccountBill = value; }
        }


        //移除的属性
        private CurrencyType _currency;
        private decimal _exchangeRate;
        private string _theOrgName;
        /// <summary>
        /// 币种
        /// </summary>
        public virtual CurrencyType Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }
        /// <summary>
        /// 汇率
        /// </summary>
        public virtual decimal ExchangeRate
        {
            get { return _exchangeRate; }
            set { _exchangeRate = value; }
        }
        /// <summary>
        /// 组织名称
        /// </summary>
        public virtual string TheOrgName
        {
            get { return _theOrgName; }
            set { _theOrgName = value; }
        }


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

        /// <summary>
        /// 编号
        /// </summary>
        public virtual string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        /// <summary>
        /// 核算人GUID
        /// </summary>
        public virtual PersonInfo AccountPersonGUID
        {
            get { return _accountPersonGUID; }
            set { _accountPersonGUID = value; }
        }
        /// <summary>
        /// 核算人名称
        /// </summary>
        public virtual string AccountPersonName
        {
            get { return _accountPersonName; }
            set { _accountPersonName = value; }
        }
        /// <summary>
        /// 核算人组织层次码
        /// </summary>
        public virtual string AccountPersonOrgSysCode
        {
            get { return _accountPersonOrgSysCode; }
            set { _accountPersonOrgSysCode = value; }
        }
        /// <summary>
        /// 核算范围,核算的GWBS树节点,此处存GUID
        /// </summary>
        public virtual GWBSTree AccountRange
        {
            get { return _accountRange; }
            set { _accountRange = value; }
        }
        /// <summary>
        /// 核算的GWBS树节点名称
        /// </summary>
        public virtual string AccountTaskName
        {
            get { return _accountTaskName; }
            set { _accountTaskName = value; }
        }
        /// <summary>
        /// 核算的GWBS树节点层次码
        /// </summary>
        public virtual string AccountTaskSyscode
        {
            get { return _accountTaskSyscode; }
            set { _accountTaskSyscode = value; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime BeginTime
        {
            get { return _beginTime; }
            set { _beginTime = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
        /// <summary>
        /// 开单时间
        /// </summary>
        public virtual DateTime BillingTime
        {
            get { return _billingTime; }
            set { _billingTime = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public virtual ProjectTaskAccountBillState State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// 工程任务明细核算
        /// </summary>
        public virtual ISet<ProjectTaskDetailAccount> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
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