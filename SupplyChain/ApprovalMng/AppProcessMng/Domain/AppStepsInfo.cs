using System;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain
{

    [Serializable]
    [Entity]
    public class AppStepsInfo
    {
        private string id;
        private long version = -1;

        private long stepOrder;
        private string stepsName;
        private string billId;
        private AppTableSet appTableSet;
        private AppStepsSet appStepsSet;
        private int appRelations;
        private OperationRole appRole;
        private PersonInfo auditPerson;
        private DateTime appDate;
        private long appStatus;
        private string appComments;
        private long appQtyByRole;
        private DateTime billAppDate;
        private int state;
        private string roleName;
        private string tempLogData;
        private DataDomain tempData;

        /// <summary>
        /// 临时日志字段
        /// </summary>
        public virtual string TempLogData
        {
            get { return tempLogData; }
            set { tempLogData = value; }
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        public virtual string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        /// <summary>
        /// Id
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public virtual long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 审批步骤
        /// </summary>
        public virtual long StepOrder
        {
            get { return stepOrder; }
            set { stepOrder = value; }
        }

        /// <summary>
        /// 审批步骤名称
        /// </summary>
        public virtual string StepsName
        {
            get { return stepsName; }
            set { stepsName = value; }
        }

        /// <summary>
        ///审批状态
        /// </summary>
        public virtual string BillId
        {
            get { return billId; }
            set { billId = value; }
        }

        /// <summary>
        ///单据类型
        /// </summary>
        public virtual AppTableSet AppTableSet
        {
            get { return appTableSet; }
            set { appTableSet = value; }
        }

        /// <summary>
        ///单据类型
        /// </summary>
        public virtual AppStepsSet AppStepsSet
        {
            get { return appStepsSet; }
            set { appStepsSet = value; }
        }

        /// <summary>
        /// 审批关系 0 或 1 并
        /// </summary>
        public virtual int AppRelations
        {
            get { return appRelations; }
            set { appRelations = value; }
        }

        /// <summary>
        /// 审批角色
        /// </summary>
        public virtual OperationRole AppRole
        {
            get { return appRole; }
            set { appRole = value; }
        }

        /// <summary>
        /// 审批人
        /// </summary>
        public virtual PersonInfo AuditPerson
        {
            get { return auditPerson; }
            set { auditPerson = value; }
        }


        /// <summary>
        /// 审批日期
        /// </summary>
        public virtual DateTime AppDate
        {
            get { return appDate; }
            set { appDate = value; }
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        public virtual long AppStatus
        {
            get { return appStatus; }
            set { appStatus = value; }
        }

        /// <summary>
        /// 审批意见
        /// </summary>
        public virtual string AppComments
        {
            get { return appComments; }
            set { appComments = value; }
        }

        /// <summary>
        /// 审批岗位数量
        /// </summary>
        public virtual long AppQtyByRole
        {
            get { return appQtyByRole; }
            set { appQtyByRole = value; }
        }

        /// <summary>
        /// 单据的审批日期(时间戳：单据第一步审批产生的审批AppMasterData的审批日期)
        /// </summary>
        public virtual DateTime BillAppDate
        {
            get { return billAppDate; }
            set { billAppDate = value; }
        }

        /// <summary>
        /// 状态 1：有效  2：无效
        /// </summary>
        public virtual int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 临时数据，非持久化
        /// </summary>
        public virtual DataDomain TempData
        {
            get { return tempData; }
            set { tempData = value; }
        }
    }
}
