using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;

namespace IRPServiceModel.Domain.Document
{
    /// <summary>
    /// 文档分类
    /// </summary>
    [Serializable]
    public class DocumentCategory : CategoryNode
    {
        private string _ownerGUID;
        private string _ownerName;
        private string _ownerOrgSysCode;
        private string _theProjectGUID;
        private string _theProjectCode;
        private string _theProjectName;
        private DateTime _updatedDate = DateTime.Now;

        /// <summary>
        /// 责任人
        /// </summary>
        public virtual string OwnerGUID
        {
            get { return _ownerGUID; }
            set { _ownerGUID = value; }
        }
        /// <summary>
        /// 责任人姓名
        /// </summary>
        public virtual string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }
        /// <summary>
        /// 责任人组织层次码
        /// </summary>
        public virtual string OwnerOrgSysCode
        {
            get { return _ownerOrgSysCode; }
            set { _ownerOrgSysCode = value; }
        }
        /// <summary>
        /// 所属项目ID
        /// </summary>
        public virtual string ProjectId
        {
            get { return _theProjectGUID; }
            set { _theProjectGUID = value; }
        }
        /// <summary>
        /// 项目代码
        /// </summary>
        public virtual string ProjectCode
        {
            get { return _theProjectCode; }
            set { _theProjectCode = value; }
        }
        /// <summary>
        /// 所属项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return _theProjectName; }
            set { _theProjectName = value; }
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; }
        }

        /// <summary>
        /// 临时属性（不做MAP）
        /// </summary>
        public virtual string Temp1
        {
            get;
            set;
        }
    }
}
