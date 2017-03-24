using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Iesi.Collections;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain
{
    [Serializable]
    [Entity]
    public class AppStepsSet
    {
        private string id;
        private long version = -1;
        private long stepOrder;
        private string stepsName;
        private AppSolutionSet parentId;
        private AppTableSet appTableSet;
        private int appRelations;
        private ISet<AppRoleSet> appRoleSets = new HashedSet<AppRoleSet>();
        private string remark;

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
        virtual public long StepOrder
        {
            get { return stepOrder; }
            set { stepOrder = value; }
        }
        /// <summary>
        /// 审批步骤名称
        /// </summary>
        virtual public string StepsName
        {
            get { return stepsName; }
            set { stepsName = value; }
        }
        /// <summary>
        ///审批方案
        /// </summary>
        virtual public AppSolutionSet ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }
        /// <summary>
        ///单据类型
        /// </summary>
        virtual public AppTableSet AppTableSet
        {
            get { return appTableSet; }
            set { appTableSet = value; }
        }

        /// <summary>
        /// 审批关系 0 或 1 并
        /// </summary>
        virtual public int AppRelations
        {
            get { return appRelations; }
            set { appRelations = value; }
        }
        /// <summary>
        /// 审批岗位
        /// </summary>
        virtual public ISet<AppRoleSet> AppRoleSets
        {
            get { return appRoleSets; }
            set { appRoleSets = value; }
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
