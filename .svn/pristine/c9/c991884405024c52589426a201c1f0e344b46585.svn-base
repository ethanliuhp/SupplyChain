using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;

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
    /// OBS管理负责人
    /// </summary>
    public class OBSManage : BaseMaster
    {
        private GWBSTree projectTask;
        private string projectTaskName;
        private AppStepsSet appRole;
        private string appRoleName;
        private DateTime beginDate = ClientUtil.ToDateTime("1900-1-1");
        private DateTime endDate = ClientUtil.ToDateTime("1900-1-1");
        private int handleType;

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
        ///管理岗位
        ///</summary>
        virtual public AppStepsSet AppRole
        {
            get { return appRole; }
            set { appRole = value; }
        }
        ///<summary>
        ///管理岗位名称
        ///</summary>
        virtual public string AppRoleName
        {
            get { return appRoleName; }
            set { appRoleName = value; }
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
