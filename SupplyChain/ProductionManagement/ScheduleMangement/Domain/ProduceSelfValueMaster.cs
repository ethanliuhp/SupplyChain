using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    /// <summary>
    /// 自行产值主表
    /// </summary>
    [Serializable]
    [Entity]
    public class ProduceSelfValueMaster : BaseMaster
    {
        private string id;
        private string schedualGUID;
        private DateTime? beginDate;
        private DateTime? endDate;
        private ProduceSelfValueMasterAccountType accountType = ProduceSelfValueMasterAccountType.无意义;
        private DateTime? planDate;
        private DateTime? realDate;
        private int accountYear;
        private int accountMonth;
        private ProduceSelfValueMasterState state;
        private string operOrgSysCode;

        /// <summary>
        /// 核算组织层次码
        /// </summary>
        public virtual string OperOrgSysCode
        {
            get { return operOrgSysCode; }
            set { operOrgSysCode = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual ProduceSelfValueMasterState State
        {
            get { return state; }
            set { state = value; }
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
        /// 核算类型
        /// </summary>
        public virtual ProduceSelfValueMasterAccountType AccountType
        {
            get { return accountType; }
            set { accountType = value; }
        }

        /// <summary>
        /// 进度计划GUID
        /// </summary>
        public virtual string SchedualGUID
        {
            get { return schedualGUID; }
            set { schedualGUID = value; }
        }

        /// <summary>
        /// 核算期间开始时间
        /// </summary>
        public virtual DateTime? BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }

        /// <summary>
        /// 核算期间结束时间
        /// </summary>
        public virtual DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        /// <summary>
        /// 计划产值生成时间
        /// </summary>
        public virtual DateTime? PlanDate
        {
            get { return planDate; }
            set { planDate = value; }
        }

        /// <summary>
        /// 实际产值生成时间
        /// </summary>
        public virtual DateTime? RealDate
        {
            get { return realDate; }
            set { realDate = value; }
        }

        /// <summary>
        /// 会计年
        /// </summary>
        public virtual int AccountYear
        {
            get { return accountYear; }
            set { accountYear = value; }
        }
        
        /// <summary>
        /// 会计月
        /// </summary>
        public virtual int AccountMonth
        {
            get { return accountMonth; }
            set { accountMonth = value; }
        }
    }

    /// <summary>
    /// 核算类型
    /// </summary>
    public enum ProduceSelfValueMasterAccountType
    {
        [Description("无意义")]
        无意义 = 1,
        [Description("月度核算")]
        月度核算 = 2,
        [Description("季度核算")]
        季度核算 = 3
    }
        /// <summary>
    /// 状态
    /// </summary>
    public enum ProduceSelfValueMasterState
    {
        [Description("计划产值已生成")]
        计划产值已生成 = 1,
        [Description("实际产值已生成")]
        实际产值已生成 = 2
    }
}
