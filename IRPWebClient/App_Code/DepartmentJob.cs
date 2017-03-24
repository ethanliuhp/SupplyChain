using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cscec.Organization
{
    public class DepartmentJob
    {
        /// <summary>
        /// 岗位ID
        /// </summary>
        public string ObjectID { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 岗位编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 所属部门ID
        /// </summary>
        public string DepartmentID { get; set; }
    }
}
