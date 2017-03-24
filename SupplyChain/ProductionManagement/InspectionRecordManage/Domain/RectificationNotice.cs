using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.ProductionManagement.InspectionRecordManage.Domain
{
    /// <summary>
    /// 整改通知
    /// </summary>
    [Serializable]
    public class RectificationNotice
    {
        private string id;
        private InspectionRecord master;
        private SupplierRelationInfo subjectOrganization;
        private string subjectOrganizationName;
        private string existingProblem;
        private PersonInfo supjectOrgPerson;
        private string subjectOrgPersonName;
        private string rectificationRequirement;
        private DateTime rectificationNoticeDate = ClientUtil.ToDateTime("1900-1-1");
        private string rectificationMethod;
        private string rectificationConclusion;
        private DateTime rectificationConclusionDate= ClientUtil.ToDateTime("1900-1-1");

        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 主表GUID
        /// </summary>
        public virtual InspectionRecord Master
        {
            get { return master; }
            set { master = value; }
        }
        /// <summary>
        /// 受检组织
        /// </summary>
        public virtual SupplierRelationInfo SubjectOrganization
        {
            get { return subjectOrganization; }
            set { subjectOrganization = value; }
        }
        /// <summary>
        /// 受检组织名称
        /// </summary>
        public virtual string SubjectOrganizationName
        {
            get { return subjectOrganizationName; }
            set { subjectOrganizationName = value; }
        }
        /// <summary>
        /// 存在的问题
        /// </summary>
        public virtual string ExistingProblem
        {
            get { return existingProblem; }
            set { existingProblem = value; }
        }
        /// <summary>
        /// 受检组织负责人
        /// </summary>
        public virtual PersonInfo SupjectOrgPerson
        {
            get { return supjectOrgPerson; }
            set { supjectOrgPerson = value; }
        }
        /// <summary>
        /// 受检组织负责人名称
        /// </summary>
        public virtual string SupjectOrgPersonName
        {
            get { return subjectOrgPersonName; }
            set { subjectOrgPersonName = value; }
        }
        /// <summary>
        /// 整改要求
        /// </summary>
        public virtual string RectificationRequirement
        {
            get { return rectificationRequirement; }
            set { rectificationRequirement = value; }
        }
        /// <summary>
        /// 整改通知日期
        /// </summary>
        public virtual DateTime RectificationNoticeDate
        {
            get { return rectificationNoticeDate; }
            set { rectificationNoticeDate = value; }
        }
        /// <summary>
        /// 整改措施与结果说明
        /// </summary>
        public virtual string RectificationMethod
        {
            get { return rectificationMethod; }
            set { rectificationMethod = value; }
        }
        /// <summary>
        /// 整改结论
        /// </summary>
        public virtual string RectificationConclusion
        {
            get { return rectificationConclusion; }
            set { rectificationConclusion = value; }
        }
        /// <summary>
        /// 整改结论时间
        /// </summary>
        public virtual DateTime RectificationConclusionDate
        {
            get { return rectificationConclusionDate; }
            set { rectificationConclusionDate = value; }
        }
    }
}
