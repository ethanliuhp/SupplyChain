using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.ProjectPlanningMange.Domain
{
    /// <summary>
    /// 商务策划书管理日志
    /// </summary>
    [Serializable]
    public class IrpBusinessPlanManageLog
    {
        private string id;
        private long version;
        private string implementResult;  //实施结果     
        private string implementCondition;  //实施情况    
        private PersonInfo implementPerson;  //实施人GUID    
        private string implementPersonName;  //实施人姓名
        private DateTime implementDate;  //实施时间
        private BusinessProposalItem master;

        virtual public BusinessProposalItem Master
        {
            get { return master; }
            set { master = value; }
        }
        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public virtual long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 实施结果
        /// </summary>
        virtual public string ImplementResult
        {
            get { return implementResult; }
            set { implementResult = value; }
        } 
        /// <summary>
        /// 实施情况
        /// </summary>
        virtual public string ImplementCondition
        {
            get { return implementCondition; }
            set { implementCondition = value; }
        } 
        /// <summary>
        /// 实施人GUID 
        /// </summary>
        virtual public PersonInfo ImplementPerson
        {
            get { return implementPerson; }
            set { implementPerson = value; }
        }
        /// <summary>
        /// 实施人姓名
        /// </summary>
        virtual public string ImplementPersonName
        {
            get { return implementPersonName; }
            set { implementPersonName = value; }
        }
        /// <summary>
        /// 实施时间
        /// </summary>
        virtual public DateTime ImplementDate
        {
            get { return implementDate; }
            set { implementDate = value; }
        }
    }
}
