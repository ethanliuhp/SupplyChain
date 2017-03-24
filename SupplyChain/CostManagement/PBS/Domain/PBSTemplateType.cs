using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain
{
    [Serializable]
    public class PBSTemplateType
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public virtual string Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 编号位数
        /// </summary>
        public virtual string CodeBit { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public virtual DateTime ModifyTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual PersonInfo CreatePerson { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public virtual string CreatePersonName { get; set; }
    }
}
