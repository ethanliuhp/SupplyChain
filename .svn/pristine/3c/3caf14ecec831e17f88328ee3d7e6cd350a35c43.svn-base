using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain
{
   
    /// <summary>
    /// OBS管理负责人
    /// </summary>
    [Serializable]
    public class OBSPerson : BaseMaster
    {
        private GWBSTree projectTask;
        private string projectTaskName;
        private string projectTaskSysCode;
        private StandardPerson managePerson;
        private string personName;
        private OperationRole personRole;
        private string roleName;
        private DateTime beginDate = ClientUtil.ToDateTime("1900-1-1");
        private DateTime endDate = ClientUtil.ToDateTime("1900-1-1");
        private string personStates;
        private OperationOrg orpJob;
        ///<summary>
        ///组织
        ///</summary>
        virtual public OperationOrg OrpJob
        {
            get { return orpJob; }
            set { orpJob = value; }
        }
        private string orgJobName;
        ///<summary>
        ///组织名称
        ///</summary>
        virtual public string OrgJobName
        {
            get { return orgJobName; }
            set { orgJobName = value; }
        }
        private string orgJobSysCode;
        ///<summary>
        ///组织层次码
        ///</summary>
        virtual public string OrgJobSysCode
        {
            get { return orgJobSysCode; }
            set { orgJobSysCode = value; }
        }
        ///<summary>
        ///状态
        ///</summary>
        virtual public string PersonStates
        {
            get { return personStates; }
            set { personStates = value; }
        }
        ///<summary>
        /// 角色名称
        ///</summary>
        virtual public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }
        ///<summary>
        /// 角色
        ///</summary>
        virtual public OperationRole PersonRole
        {
            get { return personRole; }
            set { personRole = value; }
        }
        ///<summary>
        /// 人员名称
        ///</summary>
        virtual public string PersonName
        {
            get { return personName; }
            set { personName = value; }
        }
        ///<summary>
        /// 人员
        ///</summary>
        virtual public StandardPerson ManagePerson
        {
            get { return managePerson; }
            set { managePerson = value; }
        }

        ///<summary>
        ///工程项目任务
        ///</summary>
        virtual public GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        ///<summary>
        ///工程项目任务名称
        ///</summary>
        virtual public string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        ///<summary>
        ///工程项目任务层次码
        ///</summary>
        virtual public string ProjectTaskSysCode
        {
            get { return projectTaskSysCode; }
            set { projectTaskSysCode = value; }
        }
       
        ///<summary>
        ///责任开始时间
        ///</summary>
        virtual public DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }
        ///<summary>
        ///责任结束时间
        ///</summary>
        virtual public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
      
    }
}
