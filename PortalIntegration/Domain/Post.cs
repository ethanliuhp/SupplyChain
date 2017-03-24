using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Application.Business.Erp.PortalIntegration.Domain
{
    /// <summary>
    /// 岗位
    /// </summary>
    [DataContract]
    public class Post
    {
        /// <summary>
        /// 岗位编码
        /// </summary>
        [DataMember]
        public string PosiCode;

        /// <summary>
        /// 岗位名称
        /// </summary>
        [DataMember]
        public string PosiName;

        /// <summary>
        /// 岗位所在机构编码
        /// </summary>
        [DataMember]
        public string OrgCode;

        /// <summary>
        /// 排序号
        /// </summary>
        [DataMember]
        public int OrderNo;
    }
}
