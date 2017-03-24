using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.ComponentModel;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain
{
    /// <summary>
    /// 文档对象管理
    /// </summary>
    [Serializable]
    public class DocumentManagement : CategoryNode
    {
        private GWBSTree projectTask;
        private string projectTaskName;
        private string documentGUID;
        private string documentName;
        private string documentDeacript;
        private DateTime submitDate;
        private PersonInfo docHandlePerson;
        private string  docHandlePersonName;
        private string docURL;
        private string projectGUID;
        private string projectName;

        /// <summary>
        /// 工程任务对象GUID
        /// </summary>
        public virtual GWBSTree ProjectTask
        {
            get { return projectTask; }
            set { projectTask = value; }
        }
        /// <summary>
        /// 工程任务对象类型名称
        /// </summary>
        public virtual string ProjectTaskName
        {
            get { return projectTaskName; }
            set { projectTaskName = value; }
        }
        /// <summary>
        /// 文档对象GUID
        /// </summary>
        public virtual string DocumentGUID
        {
            get { return documentGUID ; }
            set { documentGUID  = value; }
        }
        /// <summary>
        /// 文档描述
        /// </summary>
        public virtual string DocumentDeacript
        {
            get { return documentDeacript; }
            set { documentDeacript = value; }
        }
        /// <summary>
        /// 文档名称
        /// </summary>
        public virtual string DocumentName
        {
            get { return documentName; }
            set { documentName = value; }
        }
        /// <summary>
        /// 提交时间
        /// </summary>
        public virtual DateTime SubmitDate
        {
            get { return submitDate; }
            set { submitDate = value; }
        }
        /// <summary>
        /// 文档负责人
        /// </summary>
        public virtual PersonInfo DocHandlePerson
        {
            get { return docHandlePerson; }
            set { docHandlePerson = value; }
        }
        /// <summary>
        /// 文档负责人
        /// </summary>
        public virtual string DocHandlePersonName
        {
            get { return docHandlePersonName; }
            set { docHandlePersonName = value; }
        }
        /// <summary>
        /// 文档URL
        /// </summary>
        public virtual string DocURL
        {
            get { return docURL; }
            set { docURL = value; }
        }
        /// <summary>
        /// 项目GUID
        /// </summary>
        public virtual string ProjectGUID
        {
            get { return projectGUID; }
            set { projectGUID = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
    }
}
