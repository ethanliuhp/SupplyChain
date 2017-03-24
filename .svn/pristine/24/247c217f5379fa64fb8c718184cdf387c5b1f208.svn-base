using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain
{
    /// <summary>
    /// 工程任务核算单
    /// </summary>
    [Serializable]
    public class ProjectTaskAccountBill : BaseMaster
    {
        //private string _theProjectGUID;
        //private string _theProjectName;

        private PersonInfo _accountPersonGUID;
        private string _accountPersonName;
        private string _accountPersonOrgSysCode;

        private GWBSTree _accountRange;
        private string _accountTaskName;
        private string _accountTaskSyscode;

        private DateTime _beginTime;
        private DateTime _endTime;

        private string _remark;
        //private ISet<ProjectTaskDetailAccount> _Details = new HashedSet<ProjectTaskDetailAccount>();
        ///// <summary>
        ///// 工程任务明细核算
        ///// </summary>
        //public virtual ISet<ProjectTaskDetailAccount> ListDetails
        //{
        //    get { return _Details; }
        //    set { _Details = value; }
        //}
        private ISet<ProjectTaskDetailAccountSummary> _listSummary = new HashedSet<ProjectTaskDetailAccountSummary>();

        private bool _monthAccountFlag;
        private string _monthAccountBill;
        private EnumConfirmBillType _frontConfirmBillType = EnumConfirmBillType.计划工单;

       
        private string subContractProjectID;
        /// <summary>
        /// 分包项目Id 临时属性
        /// </summary>
        public virtual string SubContractProjectID
        {
            get { return subContractProjectID; }
            set { subContractProjectID = value; }
        }

        /// <summary>
        /// 前驱确认单类型
        /// </summary>
        public virtual EnumConfirmBillType FrontConfirmBillType
        {
            get { return _frontConfirmBillType; }
            set { _frontConfirmBillType = value; }
        }

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
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        /// <summary>
        /// 工程任务明细核算汇总
        /// </summary>
        public virtual ISet<ProjectTaskDetailAccountSummary> ListSummary
        {
            get { return _listSummary; }
            set { _listSummary = value; }
        }

        private string _createBatchNo;
        /// <summary>
        /// 生成批次号
        /// </summary>
        public virtual string CreateBatchNo
        {
            get { return _createBatchNo; }
            set { _createBatchNo = value; }
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
