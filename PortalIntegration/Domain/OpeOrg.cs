using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Application.Business.Erp.PortalIntegration.Domain
{
    /// <summary>
    /// 组织
    /// </summary>
    [DataContract]
    public class OpeOrg
    {
        /// <summary>
        /// 机构编码
        /// </summary>
        [DataMember]
        public string OrgCode;

        /// <summary>
        /// 机构名称
        /// </summary>
        [DataMember]
        public string OrgName;

        /// <summary>
        /// 机构类型
        /// </summary>
        [DataMember]
        public string OrgType;

        /// <summary>
        /// 父组织机构编码
        /// </summary>
        [DataMember]
        public string ParentOrgCode;
    }
}
