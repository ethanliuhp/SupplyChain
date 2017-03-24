using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain
{
    /// <summary>
    /// 工程对象关联文档
    /// </summary>
    [Serializable]
    public class ProObjectRelaDocument
    {
        private string id;
        private long version;

        private string _proObjectName;
        private string _proObjectGUID;
        private PersonInfo _documentOwner;
        private string _documentOwnerName;
        private string _documentGUID;
        private string _documentName;
        private string _documentCateCode;
        private string _documentCateName;
        private string _documentDesc;
        private string _fileURL;
        private DateTime _submitTime = DateTime.Now;

        private string _theProjectGUID;
        private string _theProjectName;

        private string _projectDocumentVerifyID;
        private string _documentCode;
        private string _documentWorkflowName;
        private string _organizationSyscode;
        private string _proObjectCode;

        /// <summary>
        /// ID
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
        /// 工程对象名称
        /// </summary>
        public virtual string ProObjectName
        {
            get { return _proObjectName; }
            set { _proObjectName = value; }
        }
        /// <summary>
        /// 工程对象GUID
        /// </summary>
        public virtual string ProObjectGUID
        {
            get { return _proObjectGUID; }
            set { _proObjectGUID = value; }
        }
        /// <summary>
        /// 文档责任人
        /// </summary>
        public virtual PersonInfo DocumentOwner
        {
            get { return _documentOwner; }
            set { _documentOwner = value; }
        }
        /// <summary>
        /// 文档责任人姓名
        /// </summary>
        public virtual string DocumentOwnerName
        {
            get { return _documentOwnerName; }
            set { _documentOwnerName = value; }
        }
        /// <summary>
        /// 文档GUID
        /// </summary>
        public virtual string DocumentGUID
        {
            get { return _documentGUID; }
            set { _documentGUID = value; }
        }
        /// <summary>
        /// 文档名称
        /// </summary>
        public virtual string DocumentName
        {
            get { return _documentName; }
            set { _documentName = value; }
        }
        /// <summary>
        /// 文档分类编码
        /// </summary>
        public virtual string DocumentCateCode
        {
            get { return _documentCateCode; }
            set { _documentCateCode = value; }
        }
        /// <summary>
        /// 文档分类名称
        /// </summary>
        virtual public string DocumentCateName
        {
            get { return _documentCateName; }
            set { _documentCateName = value; }
        }
        /// <summary>
        /// 文档说明
        /// </summary>
        public virtual string DocumentDesc
        {
            get { return _documentDesc; }
            set { _documentDesc = value; }
        }
        /// <summary>
        /// 文件URL
        /// </summary>
        public virtual string FileURL
        {
            get { return _fileURL; }
            set { _fileURL = value; }
        }
        /// <summary>
        /// 文档提交时间
        /// </summary>
        public virtual DateTime SubmitTime
        {
            get { return _submitTime; }
            set { _submitTime = value; }
        }
        /// <summary>
        /// 项目GUID
        /// </summary>
        public virtual string TheProjectGUID
        {
            get { return _theProjectGUID; }
            set { _theProjectGUID = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string TheProjectName
        {
            get { return _theProjectName; }
            set { _theProjectName = value; }
        }
        /// <summary>
        /// 项目代码
        /// </summary>
        virtual public string TheProjectCode
        {
            get { return _proObjectCode; }
            set { _proObjectCode = value; }
        }
        /// <summary>
        /// 工程文档验证GUID
        /// </summary>
        virtual public string ProjectDocumentVerifyID
        {
            get { return _projectDocumentVerifyID; }
            set { _projectDocumentVerifyID = value; }
        }

        /// <summary>
        /// 文档代码
        /// </summary>
        virtual public string DocumentCode
        {
            get { return _documentCode; }
            set { _documentCode = value; }
        }

        /// <summary>
        /// 文档工作流名称
        /// </summary>
        virtual public string DocumentWorkflowName
        {
            get { return _documentWorkflowName; }
            set { _documentWorkflowName = value; }
        }

        /// <summary>
        /// 所属组织层次码 
        /// </summary>
        virtual public string OrganizationSyscode
        {
            get { return _organizationSyscode; }
            set { _organizationSyscode = value; }
        }
    }
}