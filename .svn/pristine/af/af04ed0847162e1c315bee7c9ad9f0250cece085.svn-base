using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;
using Iesi.Collections.Generic;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.Approval.AppProcessMng.Domain
{
    [Serializable]
    [Entity]
    public class AppStatus
    {
        private string id;
        private long version = -1;
        private AppTableSet appTableSet;
        private AppSolutionSet appSolutionSet;

        private SupplierRelationInfo supplierRelationInfo;
        private CustomerRelationInfo customerRelationInfo;

        private string billId;
        private string billCode;
        private string className;
        private DateTime createDate;
        private decimal quantity;
        private decimal money;
        private string remark;

        private PersonInfo createPerson;

        private ISet<AppStepsInfo> appStepsInfos = new HashedSet<AppStepsInfo>();

        private DateTime appOverDate;

        private string appResults;
        private long status;

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
        ///单据类型
        /// </summary>
        virtual public AppTableSet AppTableSet
        {
            get { return appTableSet; }
            set { appTableSet = value; }
        }

        /// <summary>
        ///方案
        /// </summary>
        virtual public AppSolutionSet AppSolutionSet
        {
            get { return appSolutionSet; }
            set { appSolutionSet = value; }
        }

        /// <summary>
        ///供应商
        /// </summary>
        virtual public SupplierRelationInfo SupplierRelationInfo
        {
            get { return supplierRelationInfo; }
            set { supplierRelationInfo = value; }
        }
        /// <summary>
        ///客户
        /// </summary>
        virtual public CustomerRelationInfo CustomerRelationInfo
        {
            get { return customerRelationInfo; }
            set { customerRelationInfo = value; }
        }
        /// <summary>
        /// 表单Id
        /// </summary>
        virtual public string BillId
        {
            get { return billId; }
            set { billId = value; }
        }

        /// <summary>
        /// 表单号
        /// </summary>
        virtual public string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }

        /// <summary>
        /// 表单名称
        /// </summary>
        virtual public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        /// <summary>
        /// 表单日期
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }


        /// <summary>
        /// 提交者
        /// </summary>
        virtual public PersonInfo CreatePerson
        {
            get { return createPerson; }
            set { createPerson = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        virtual public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// 金额
        /// </summary>
        virtual public decimal Money
        {
            get { return money; }
            set { money = value; }
        }

        /// <summary>
        /// 表单描述
        /// </summary>
        virtual public string Remarks
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 审批结束日期
        /// </summary>
        virtual public DateTime AppOverDate
        {
            get { return appOverDate; }
            set { appOverDate = value; }
        }


        /// <summary>
        ///审批流程步骤
        /// </summary>
        virtual public ISet<AppStepsInfo> AppStepsInfos
        {
            get { return appStepsInfos; }
            set { appStepsInfos = value; }
        }


        /// <summary>
        /// 审批结果
        /// </summary>
        virtual public string AppResults
        {
            get { return appResults; }
            set { appResults = value; }
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        virtual public long Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
