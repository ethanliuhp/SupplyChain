using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using System.ComponentModel;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Component.Util;


namespace Application.Business.Erp.SupplyChain.ProductionManagement.ScheduleMangement.Domain
{
    /// <summary>
    /// 进度计划主表
    /// </summary>
    [Serializable]
    public class ProductionScheduleMaster
    {
        private string id;
        private string ganttChartId;
        private string networkChartId;
        private string scheduleVersion;
        private PersonInfo handlePerson;
        private string handlePersonName;
        private OperationOrgInfo operOrgInfo;
        private string operOrgInfoName;
        private string opgSysCode;
        private DocumentState docState;
        private DateTime createDate = DateTime.Now;
        private DateTime realOperationDate = DateTime.Now;
        private DateTime submitDate = StringUtil.StrToDateTime("1900-01-01");
        private string scheduleRootNodeId;
        private string scheduleCaliber;
        private EnumScheduleType scheduleType;
        private string projectId;
        private string projectName;
        private string scheduleName;
        private string descript;
        private string scheduleTypeDetail;

        /// <summary>
        /// 计划名称（基础数据中配置，原进度计划类型明细)
        /// </summary>
        public virtual string ScheduleTypeDetail
        {
            get { return scheduleTypeDetail; }
            set { scheduleTypeDetail = value; }
        }

        /// <summary>
        /// 说明
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 计划版本（原计划名称）
        /// </summary>
        public virtual string ScheduleName
        {
            get { return scheduleName; }
            set { scheduleName = value; }
        }

        private Iesi.Collections.Generic.ISet<ProductionScheduleDetail> details = new Iesi.Collections.Generic.HashedSet<ProductionScheduleDetail>();
        public virtual Iesi.Collections.Generic.ISet<ProductionScheduleDetail> Details
        {
            get { return details; }
            set { details = value; }
        }

        public virtual void AddDetail(ProductionScheduleDetail detail)
        {
            detail.Master = this;
            Details.Add(detail);
        }

        /// <summary>
        /// 获取进度计划根节点
        /// </summary>
        /// <returns></returns>
        public virtual ProductionScheduleDetail GetChildRootNode()
        {
            foreach (ProductionScheduleDetail dtl in Details)
            {
                if (dtl.ParentNode == null) return dtl;
            }
            return null;
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
        /// 进度计划类型
        /// </summary>
        public virtual EnumScheduleType ScheduleType
        {
            get { return scheduleType; }
            set { scheduleType = value; }
        }

        /// <summary>
        /// 进度计划口径
        /// </summary>
        public virtual string ScheduleCaliber
        {
            get { return scheduleCaliber; }
            set { scheduleCaliber = value; }
        }

        /// <summary>
        /// 进度计划节点ID
        /// </summary>
        public virtual string ScheduleRootNodeId
        {
            get { return scheduleRootNodeId; }
            set { scheduleRootNodeId = value; }
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
        public virtual DateTime RealOperationDate
        {
            get { return realOperationDate; }
            set { realOperationDate = value; }
        }

        /// <summary>
        /// 提交日期
        /// </summary>
        public virtual DateTime SubmitDate
        {
            get { return submitDate; }
            set { submitDate = value; }
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
        /// 组织层次码
        /// </summary>
        public virtual string OpgSysCode
        {
            get { return opgSysCode; }
            set { opgSysCode = value; }
        }

        /// <summary>
        /// 业务组织名称
        /// </summary>
        public virtual string OperOrgInfoName
        {
            get { return operOrgInfoName; }
            set { operOrgInfoName = value; }
        }

        /// <summary>
        /// 业务组织
        /// </summary>
        public virtual OperationOrgInfo OperOrgInfo
        {
            get { return operOrgInfo; }
            set { operOrgInfo = value; }
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
        /// 责任人
        /// </summary>
        public virtual PersonInfo HandlePerson
        {
            get { return handlePerson; }
            set { handlePerson = value; }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public virtual string ScheduleVersion
        {
            get { return scheduleVersion; }
            set { scheduleVersion = value; }
        }

        /// <summary>
        /// 网络图
        /// </summary>
        public virtual string NetworkChartId
        {
            get { return networkChartId; }
            set { networkChartId = value; }
        }

        /// <summary>
        /// 横道图
        /// </summary>
        public virtual string GanttChartId
        {
            get { return ganttChartId; }
            set { ganttChartId = value; }
        }

        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
    }

    /// <summary>
    /// 进度计划状态
    /// </summary>
    public enum EnumScheduleState
    {
        /// <summary>
        /// 编辑状态
        /// </summary>
        [Description("制定")]
        制定 = 0,
        /// <summary>
        /// 编辑完成，提交审批
        /// </summary>
        [Description("提交")]
        提交 = 3,
        /// <summary>
        /// 审批通过，发布生效
        /// </summary>
        [Description("发布")]
        发布 = 5,
        [Description("冻结")]
        冻结 = 6,
        [Description("作废")]
        作废 = 2
    }

    /// <summary>
    /// 进度计划节点状态
    /// </summary>
    public enum EnumScheduleDetailState
    {
        [Description("编辑")]
        编辑 = 10,
        [Description("有效")]
        有效 = 11,
        [Description("失效")]
        失效 = 12
    }

    /// <summary>
    /// 进度计划类型
    /// </summary>
    public enum EnumScheduleType
    {
        [Description("总进度计划")]
        总进度计划 = 20,
        [Description("总滚动进度计划")]
        总滚动进度计划 = 30,
        [Description("进度计划查询")]
        进度计划查询 = 40,
    }
}
