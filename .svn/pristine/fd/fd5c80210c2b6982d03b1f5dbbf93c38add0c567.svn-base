using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Approval.AppSolutionSetMng.Domain;
using Spring.Collections;
using Iesi.Collections.Generic;

namespace Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain
{
    [Serializable]
    [Entity]
    public class AppTableSet
    {
        private string id;
        private long version = -1;
        private string tableName;
        private string className;
        private string detailClassName;
        private string physicsName;
        private string detailPhysicsName;
        private string statusName;
        private string statusValueAgr;
        private string statusValueDis;
        private string queryCode;
        private string execCode;
        private string masterNameSpace;
        private string detailNameSpace;
        private string remark;
        private int status;
        private string projectId;
        private string projectName;

        /// <summary>
        /// 归属项目ID
        /// </summary>
        virtual public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        /// <summary>
        /// 归属项目名称
        /// </summary>
        virtual public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
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
        /// 表单名称
        /// </summary>
        virtual public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }


        /// <summary>
        /// 表单类名
        /// </summary>
        virtual public string ClassName
        {
            get { return className; }
            set { className = value; }
        }

        /// <summary>
        /// 物理名称
        /// </summary>
        virtual public string PhysicsName
        {
            get { return physicsName; }
            set { physicsName = value; }
        }
        /// <summary>
        /// 表单明细类名
        /// </summary>
        virtual public string DetailClassName
        {
            get { return detailClassName; }
            set { detailClassName = value; }
        }
        /// <summary>
        /// 明细物理名称
        /// </summary>
        virtual public string DetailPhysicsName
        {
            get { return detailPhysicsName; }
            set { detailPhysicsName = value; }
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        virtual public string StatusName
        {
            get { return statusName; }
            set { statusName = value; }
        }

        /// <summary>
        /// 审批通过状态值
        /// </summary>
        virtual public string StatusValueAgr
        {
            get { return statusValueAgr; }
            set { statusValueAgr = value; }
        }

        /// <summary>
        /// 审批不通过状态值
        /// </summary>
        virtual public string StatusValueDis
        {
            get { return statusValueDis; }
            set { statusValueDis = value; }
        }

        /// <summary>
        /// 执行代码
        /// </summary>
        virtual public string ExecCode
        {
            get { return execCode; }
            set { execCode = value; }
        }


        /// <summary>
        /// 查询代码
        /// </summary>
        virtual public string QueryCode
        {
            get { return queryCode; }
            set { queryCode = value; }
        }

        /// <summary>
        /// 主表类命名空间
        /// </summary>
        virtual public string MasterNameSpace
        {
            get { return masterNameSpace; }
            set { masterNameSpace = value; }
        }
        /// <summary>
        /// 明细类命名空间
        /// </summary>
        virtual public string DetailNameSpace
        {
            get { return detailNameSpace; }
            set { detailNameSpace = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        /// <summary>
        /// 状态 是否启用
        /// </summary>
        virtual public int Status
        {
            get { return status; }
            set { status = value; }
        }
    }
}
