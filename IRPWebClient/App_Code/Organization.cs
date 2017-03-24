using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cscec.Organization
{
    public class Organization
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public string ObjectID { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 组织编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 组织级别
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 上级组织ID
        /// </summary>
        public string ParentID { get; set; }
    }
}
