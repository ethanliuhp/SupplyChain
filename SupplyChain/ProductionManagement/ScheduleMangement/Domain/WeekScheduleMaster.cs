using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using System.ComponentModel;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    /// <summary>
    /// 周进度计划主表
    /// </summary>
    [Serializable]
    [Entity]
    public class WeekScheduleMaster
    {
        private string id;
        private string code;
        private string projectId;
        private string projectName;
        private string descript;
        private string completionAnalysis;
        private PersonInfo handlePerson;
        private string handlePersonName;
        private string handlePersonSyscode;
        private OperationOrgInfo handleOrg;
        private int handOrgLevel;
        private DateTime plannedBeginDate = new DateTime(1900, 1, 1);
        private DateTime plannedEndDate = new DateTime(1900, 1, 1);
        private DateTime createDate = DateTime.Now;
        private PersonInfo createPerson;
        private string createPersonName;
        private EnumExecScheduleType execScheduleType;
        private DocumentState docState;
        private string forwardBillId;
        private string forwardBillCode;
        private EnumSummaryStatus summaryStatus;

        private int accountYear;
        private int accountMonth;

        private DateTime realOperationDate = DateTime.Now;
        private DateTime submitDate = StringUtil.StrToDateTime("1900-01-01");
       

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

        private string _planName;
        /// <summary>
        /// 计划名称
        /// </summary>
        public virtual string PlanName
        {
            get { return _planName; }
            set { _planName = value; }
        }

        /// <summary>
        /// 汇总状态 为10表示为汇总生成
        /// </summary>
        public virtual EnumSummaryStatus SummaryStatus
        {
            get { return summaryStatus; }
            set { summaryStatus = value; }
        }

        /// <summary>
        /// 前驱单据名称或Code
        /// </summary>
        public virtual string ForwardBillCode
        {
            get { return forwardBillCode; }
            set { forwardBillCode = value; }
        }

        /// <summary>
        /// 前驱单据ID
        /// </summary>
        public virtual string ForwardBillId
        {
            get { return forwardBillId; }
            set { forwardBillId = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual DocumentState DocState
        {
            get { return docState; }
            set { docState = value; }
        }

        /// <summary>
        /// 执行进度计划类型
        /// </summary>
        public virtual EnumExecScheduleType ExecScheduleType
        {
            get { return execScheduleType; }
            set { execScheduleType = value; }
        }

        /// <summary>
        /// 制单人
        /// </summary>
        public virtual string CreatePersonName
        {
            get { return createPersonName; }
            set { createPersonName = value; }
        }

        /// <summary>
        /// 制单人
        /// </summary>
        public virtual PersonInfo CreatePerson
        {
            get { return createPerson; }
            set { createPerson = value; }
        }

        /// <summary>
        /// 业务日期
        /// </summary>
        public virtual DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
        /// <summary>
        /// 实际业务日期(制单时间)（服务器时间）
        /// </summary>
        virtual public DateTime RealOperationDate
        {
            get { return realOperationDate; }
            set { realOperationDate = value; }
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        virtual public DateTime SubmitDate
        {
            get { return submitDate; }
            set { submitDate = value; }
        }

        /// <summary>
        /// 单据号
        /// </summary>
        public virtual string Code
        {
            get { return code; }
            set { code = value; }
        }

        /// <summary>
        /// 计划结束日期
        /// </summary>
        public virtual DateTime PlannedEndDate
        {
            get { return plannedEndDate; }
            set { plannedEndDate = value; }
        }

        /// <summary>
        /// 计划开始日期
        /// </summary>
        public virtual DateTime PlannedBeginDate
        {
            get { return plannedBeginDate; }
            set { plannedBeginDate = value; }
        }

        /// <summary>
        /// 责任人名称
        /// </summary>
        public virtual string HandlePersonName
        {
            get { return handlePersonName; }
            set { handlePersonName = value; }
        }

        /// <summary>
        /// 责任人组织层次码
        /// </summary>
        public virtual string HandlePersonSyscode
        {
            get { return handlePersonSyscode; }
            set { handlePersonSyscode = value; }
        }

        /// <summary>
        /// 责任组织
        /// </summary>
        public virtual OperationOrgInfo HandleOrg
        {
            get { return handleOrg; }
            set { handleOrg = value; }
        }

        /// <summary>
        /// 责任人所属组织层级
        /// </summary>
        public virtual int HandOrgLevel
        {
            get { return handOrgLevel; }
            set { handOrgLevel = value; }
        }

        /// <summary>
        /// 责任人
        /// </summary>
        public virtual PersonInfo HandlePerson
        {
            get { return handlePerson; }
            set { handlePerson = value; }
        }

        /// <summary>
        /// 完全情况分析
        /// </summary>
        public virtual string CompletionAnalysis
        {
            get { return completionAnalysis; }
            set { completionAnalysis = value; }
        }

        /// <summary>
        /// 计划描述
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 所属项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }

        /// <summary>
        /// 所属项目ID
        /// </summary>
        public virtual string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }

        private Iesi.Collections.Generic.ISet<WeekScheduleDetail> details = new Iesi.Collections.Generic.HashedSet<WeekScheduleDetail>();
        public virtual Iesi.Collections.Generic.ISet<WeekScheduleDetail> Details
        {
            get { return details; }
            set { details = value; }
        }

        /// <summary>
        /// 添加明细
        /// </summary>
        /// <param name="detail"></param>
        public virtual void AddDetail(WeekScheduleDetail detail)
        {
            detail.Master = this;
            this.Details.Add(detail);
        }

        private decimal sumPlannedWorkload;

        /// <summary>
        /// 合计计划工程量
        /// </summary>
        public virtual decimal SumPlannedWorkload
        {
            get
            {
                decimal temp = 0M;
                foreach (WeekScheduleDetail detail in Details)
                {
                    temp += detail.PlannedWrokload;
                }
                return temp;
            }
        }

        private decimal sumActualWorkload;

        /// <summary>
        /// 合计实际工程量
        /// </summary>
        public virtual decimal SumActualWorkload
        {
            get
            {
                decimal temp = 0M;
                foreach (WeekScheduleDetail detail in Details)
                {
                    temp += detail.ActualWorklaod;
                }
                return temp;
            }
        }
    }

    /// <summary>
    /// 执行进度计划类型
    /// </summary>
    public enum EnumPlanType
    {
        [Description("周计划")]
        周计划 = 10,
        [Description("月计划")]
        月计划 = 20,
        [Description("季计划")]
        季计划 = 30,
        [Description("年计划")]
        年计划 = 32
    }

    public enum EnumExecScheduleType
    {
        [Description("周进度计划")]
        周进度计划 = 10,
        [Description("月度进度计划")]
        月度进度计划 = 20,
        [Description("季度进度计划")]
        季度进度计划 = 30,
        [Description("年度进度计划")]
        年度进度计划 = 32,
        [Description("总体进度计划")]
        总体进度计划 = 40
    }

    /// <summary>
    /// 执行进度计划 汇总状态
    /// </summary>
    public enum EnumSummaryStatus
    {
        /// <summary>
        /// 指未汇总的工区周计划
        /// </summary>
        [Description("未汇总")]
        未汇总 = 0,
        /// <summary>
        /// 指已汇总的工区周计划
        /// </summary>
        [Description("己汇总")]
        己汇总 = 1,
        /// <summary>
        /// 指项目周计划
        /// </summary>
        [Description("汇总生成")]
        汇总生成 = 10
    }
}
