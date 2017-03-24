using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Iesi.Collections;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain
{
    [Serializable]
    [Entity]
    public class AppSolutionSet
    {
        private string id;
        private long version = -1;
        private string solutionName;
        private string description;
        private AppTableSet parentId;
        private ISet<AppStepsSet> appStepsSets = new HashedSet<AppStepsSet>();
        private string conditions;
        private int status;

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
        /// 方案名称
        /// </summary>
        virtual public string SolutionName
        {
            get { return solutionName; }
            set { solutionName = value; }
        }
        /// <summary>
        /// 方案描述
        /// </summary>
        virtual public string Description
        {
            get { return description; }
            set { description = value; }
        }
        /// <summary>
        ///单据类型
        /// </summary>
        virtual public AppTableSet ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        /// <summary>
        ///审批流程步骤
        /// </summary>
        virtual public ISet<AppStepsSet> AppStepsSets
        {
            get { return appStepsSets; }
            set { appStepsSets = value; }
        }

        /// <summary>
        /// 执行条件
        /// </summary>
        virtual public string Conditions
        {
            get { return conditions; }
            set { conditions = value; }
        }

        /// <summary>
        /// 默认方案
        /// </summary>
        virtual public int Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
