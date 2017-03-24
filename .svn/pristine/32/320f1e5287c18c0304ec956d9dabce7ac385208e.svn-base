using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;

using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    [Serializable]
    [Entity]
    public class ViewWriteInfo
    {
        private string id;
        private long version = -1;
        private ViewMain main;
        private string timeDimId;
        private string ywzzDimId;
        private string state;
        private string remark;
        private string standby1;
        private string standby2;
        private string standby3;
        private string standby4;
        private string standby5;
        private DateTime createDate;
        private OperationOrgInfo theOpeOrg;
        private SysRole theJob;
        private PersonInfo author;

        /// <summary>
        /// 时间维度值ID
        /// </summary>
        virtual public string TimeDimId
        {
            get { return timeDimId; }
            set { timeDimId = value; }
        }

        /// <summary>
        /// 业务组织维度值ID
        /// </summary>
        virtual public string YwzzDimId
        {
            get { return ywzzDimId; }
            set { ywzzDimId = value; }
        }

        /// <summary>
        /// 视图主表
        /// </summary>
        virtual public ViewMain Main
        {
            get { return main; }
            set { main = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        virtual public string State
        {
            get { return state; }
            set { state = value; }
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
        /// 备用字段1
        /// </summary>
        virtual public string Standby1
        {
            get { return standby1; }
            set { standby1 = value; }
        }

        /// <summary>
        /// 备用字段2
        /// </summary>
        virtual public string Standby2
        {
            get { return standby2; }
            set { standby2 = value; }
        }

        /// <summary>
        /// 备用字段3
        /// </summary>
        virtual public string Standby3
        {
            get { return standby3; }
            set { standby3 = value; }
        }

        /// <summary>
        /// 备用字段4
        /// </summary>
        virtual public string Standby4
        {
            get { return standby4; }
            set { standby4 = value; }
        }

        /// <summary>
        /// 备用字段5
        /// </summary>
        virtual public string Standby5
        {
            get { return standby5; }
            set { standby5 = value; }
        }


        /// <summary>
        /// 录入人
        /// </summary>
        virtual public PersonInfo Author
        {
            get { return author; }
            set { author = value; }
        }

        /// <summary>
        /// 录入岗位
        /// </summary>
        virtual public SysRole TheJob
        {
            get { return theJob; }
            set { theJob = value; }
        }

        /// <summary>
        /// 录入部门
        /// </summary>
        virtual public OperationOrgInfo TheOpeOrg
        {
            get { return theOpeOrg; }
            set { theOpeOrg = value; }
        }

        /// <summary>
        /// 录入时间
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

    }
}
