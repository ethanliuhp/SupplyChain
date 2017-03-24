using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 视图分发Domain
    /// </summary>
    [Serializable]
    [Entity]
    public class ViewDistribute
    {
        private string id;
        private long version = -1;
        private string distributeSerial;
        private DateTime distributeDate;
        private int stateCode;
        private string stateName;
        private DateTime writeDate;
        private OperationOrg theOpeOrg;
        private OperationJob theJob;
        private PersonInfo author;
        private string viewName;
        private ViewMain main;

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
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 分发序列号
        /// </summary>
        virtual public string DistributeSerial
        {
            get { return distributeSerial; }
            set { distributeSerial = value; }
        }

        /// <summary>
        /// 分发日期
        /// </summary>
        virtual public DateTime DistributeDate
        {
            get { return distributeDate; }
            set { distributeDate = value; }
        }

        /// <summary>
        /// 状态编码
        /// </summary>
        virtual public int StateCode
        {
            get { return stateCode; }
            set { stateCode = value; }
        }

        /// <summary>
        /// 视图名称
        /// </summary>
        virtual public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }

        /// <summary>
        /// 状态名称
        /// </summary>
        virtual public string StateName
        {
            get { return stateName; }
            set { stateName = value; }
        }

        /// <summary>
        /// 录入日期
        /// </summary>
        virtual public DateTime WriteDate
        {
            get { return writeDate; }
            set { writeDate = value; }
        }

        /// <summary>
        /// 填写人
        /// </summary>
        virtual public PersonInfo Author
        {
            get { return author; }
            set { author = value; }
        }

        /// <summary>
        /// 分发岗位
        /// </summary>
        virtual public OperationJob TheJob
        {
            get { return theJob; }
            set { theJob = value; }
        }

        /// <summary>
        /// 分发部门
        /// </summary>
        virtual public OperationOrg TheOpeOrg
        {
            get { return theOpeOrg; }
            set { theOpeOrg = value; }
        }

        /// <summary>
        /// 视图主表
        /// </summary>
        virtual public ViewMain Main
        {
            get { return main; }
            set { main = value; }
        }

    }
}
