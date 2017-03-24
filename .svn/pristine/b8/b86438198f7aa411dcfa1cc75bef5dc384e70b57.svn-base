using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain
{
    [Serializable]
    [Entity]
    public class AppRoleSet
    {
        private string id;
        private long version = -1;
        private AppStepsSet parentId;
        private OperationRole appRole;
        private string remark;
        private string roleName;

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
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }


        /// <summary>
        /// 审批步骤
        /// </summary>
        virtual public AppStepsSet ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        /// <summary>
        /// 审批角色
        /// </summary>
        virtual public OperationRole AppRole
        {
            get { return appRole; }
            set { appRole = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
    }
}
