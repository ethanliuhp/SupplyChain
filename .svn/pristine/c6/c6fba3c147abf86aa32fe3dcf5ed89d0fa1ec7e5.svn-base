using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.SystemAspect.Security.SysAuthentication.Domain;

namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorUse.Domain
{
    /// <summary>
    /// 视图显示明细Domain
    /// </summary>
    [Serializable]
    [Entity]
    public class ViewDetail
    {
        private string id;
        private long version = -1;
        private PersonInfo author;

        private Hashtable htDimMx=new Hashtable();
        private ViewMain main;
        private SysRole theJob;

        /// <summary>
        /// 业务组织
        /// </summary>
        virtual public PersonInfo Author
        {
            get { return author; }
            set { author = value; }
        }
        
        /// <summary>
        /// 岗位
        /// </summary>
        virtual public SysRole TheJob
        {
            get { return theJob; }
            set { theJob = value; }
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
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }

        /// <summary>
        /// 展现的维度格式和值的结构
        /// </summary>
        virtual public Hashtable HtDimMx
        {
            get { return htDimMx; }
            set { htDimMx = value; }
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
