using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.OBS.Domain
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public enum EnumTaskType
    {
        综合管理任务 = 0,
        安全任务 = 1
    }
    /// <summary>
    /// 管理OBS
    /// </summary>
    [Serializable]
    public class OBSManage : BaseMaster
    {
        private GWBSTree projectTask;
        private string projectTaskName;
        private string projectTaskSysCode;
        private DateTime beginDate = ClientUtil.ToDateTime("1900-1-1");
        private DateTime endDate = ClientUtil.ToDateTime("1900-1-1");
        private int handleType;
        private string mngState;
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
        /// 状态
        ///</summary>
        virtual public string MngState
        {
            get { return mngState; }
            set { mngState = value; }
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
        ///<summary>
        ///责任类型
        ///</summary>
        virtual public int HandleType
        {
            get { return handleType; }
            set { handleType = value; }
        }
    }
}
