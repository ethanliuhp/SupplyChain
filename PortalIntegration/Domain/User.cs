using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Application.Business.Erp.PortalIntegration.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    [DataContract]
    public class User
    {
        /// <summary>
        /// 用户编码/登录名
        /// </summary>
        [DataMember]
        public string UserCode;

        /// <summary>
        /// 用户名称
        /// </summary>
        [DataMember]
        public string UserName;

        /// <summary>
        /// 密码
        /// </summary>
        [DataMember]
        public string Password;

    }
}
