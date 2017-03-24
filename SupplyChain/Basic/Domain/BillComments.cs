using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.Basic.Domain
{
    /// <summary>
    /// 单据跟帖表
    /// </summary>
    [Serializable]
    public class BillComments
    {
        private string id;
        private long version;
        private string theProjectGUID;
        private string theProjectName;
        private string billTypeName;
        private string billID;
        private string billName;
        private PersonInfo billHandlePerson;
        private string billHandlePersonName;
        private OperationOrgInfo theHandlePersonOrg;
        private string theHandlePersonOrgName;
        private string theHandlePersonOrgSyscode;
        private DateTime billCreateTime;
        private PersonInfo postPerson;
        private string postPersonName;
        private OperationOrgInfo postPersonOrg;
        private string postPersonOrgName;
        //private string postPersonOrgSyscode;
        private string postPersonJobName;
        private DateTime commentCommitTime;
        private string comment;

        /// <summary>
        /// Id
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        
        /// <summary>
        /// 
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        
        /// <summary>
        /// 所属项目
        /// </summary>
        virtual public string TheProjectGUID
        {
            get { return theProjectGUID; }
            set { theProjectGUID = value; }
        }
        
        /// <summary>
        /// 所属项目名称
        /// </summary>
        virtual public string TheProjectName
        {
            get { return theProjectName; }
            set { theProjectName = value; }
        }
        
        /// <summary>
        /// 单据类型名称
        /// </summary>
        virtual public string BillTypeName
        {
            get { return billTypeName; }
            set { billTypeName = value; }
        }
        
        /// <summary>
        /// 单据对象
        /// </summary>
        virtual public string BillID
        {
            get { return billID; }
            set { billID = value; }
        }
        
        /// <summary>
        /// 单据名称
        /// </summary>
        virtual public string BillName
        {
            get { return billName; }
            set { billName = value; }
        }
        
        /// <summary>
        /// 单据责任人
        /// </summary>
        virtual public PersonInfo BillHandlePerson
        {
            get { return billHandlePerson; }
            set { billHandlePerson = value; }
        }
        
        /// <summary>
        /// 单据责任人名称
        /// </summary>
        virtual public string BillHandlePersonName
        {
            get { return billHandlePersonName; }
            set { billHandlePersonName = value; }
        }
        
        /// <summary>
        /// 单据责任人所属组织
        /// </summary>
        virtual public OperationOrgInfo TheHandlePersonOrg
        {
            get { return theHandlePersonOrg; }
            set { theHandlePersonOrg = value; }
        }
        
        /// <summary>
        /// 单据责任人所属组织名称
        /// </summary>
        virtual public string TheHandlePersonOrgName
        {
            get { return theHandlePersonOrgName; }
            set { theHandlePersonOrgName = value; }
        }
        
        /// <summary>
        /// 单据责任人所属组织层次码
        /// </summary>
        virtual public string TheHandlePersonOrgSyscode
        {
            get { return theHandlePersonOrgSyscode; }
            set { theHandlePersonOrgSyscode = value; }
        }
        
        /// <summary>
        /// 单据编制时间
        /// </summary>
        virtual public DateTime BillCreateTime
        {
            get { return billCreateTime; }
            set { billCreateTime = value; }
        }
        
        /// <summary>
        /// 发帖人
        /// </summary>
        virtual public PersonInfo PostPerson
        {
            get { return postPerson; }
            set { postPerson = value; }
        }
        
        /// <summary>
        /// 发帖人名称
        /// </summary>
        virtual public string PostPersonName
        {
            get { return postPersonName; }
            set { postPersonName = value; }
        }
        
        /// <summary>
        /// 发帖人所属组织
        /// </summary>
        virtual public OperationOrgInfo PostPersonOrg
        {
            get { return postPersonOrg; }
            set { postPersonOrg = value; }
        }
        
        /// <summary>
        /// 发帖人所属组织名称
        /// </summary>
        virtual public string PostPersonOrgName
        {
            get { return postPersonOrgName; }
            set { postPersonOrgName = value; }
        }
        
        /// <summary>
        /// 发帖人所属组织层次码
        /// </summary>
        //virtual public string PostPersonOrgSyscode
        //{
        //    get { return thePostOrganizationSyscode; }
        //    set { thePostOrganizationSyscode = value; }
        //}
        
        /// <summary>
        /// 发帖人岗位名称
        /// </summary>
        virtual public string PostPersonJobName
        {
            get { return postPersonJobName; }
            set { postPersonJobName = value; }
        }
        
        /// <summary>
        /// 发帖时间
        /// </summary>
        virtual public DateTime CommentCommitTime
        {
            get { return commentCommitTime; }
            set { commentCommitTime = value; }
        }
        
        /// <summary>
        /// 评论内容
        /// </summary>
        virtual public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        
    }
}
