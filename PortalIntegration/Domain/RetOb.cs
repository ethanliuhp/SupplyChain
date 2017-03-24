using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Application.Business.Erp.PortalIntegration.Domain
{
    /// <summary>
    /// 返回对象
    /// </summary>
    [DataContract]
    public class RetOb
    {
        /// <summary>
        /// 1 成功；0 失败
        /// </summary>
        [DataMember]
        public int Out0=1;

        /// <summary>
        /// 失败原因
        /// </summary>
        [DataMember]
        public string Result;
    }
}
