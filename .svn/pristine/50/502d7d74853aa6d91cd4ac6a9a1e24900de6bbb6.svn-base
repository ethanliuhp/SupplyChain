using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.PBS.Domain
{
    [Serializable]
    [Entity]
    public class PBSTemplate
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public virtual string Id { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public virtual string Code { get; set; }
        /// <summary>
        /// 节点层次码
        /// </summary>
        public virtual int Level { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        public virtual string ParentId { get; set; }
        /// <summary>
        /// 节点描述
        /// </summary>
        public virtual string Description { get; set; }
        /// <summary>
        /// 层次码
        /// </summary>
        public virtual string SysCode { get; set; }
        /// <summary>
        /// 节点创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }
        /// <summary>
        /// 节点修改时间
        /// </summary>
        public virtual DateTime ModifyTime { get; set; }
        /// <summary>
        /// 节点全路径
        /// </summary>
        public virtual string FullPath { get; set; }
        /// <summary>
        /// 节点创建人
        /// </summary>
        public virtual PersonInfo CreatePerson { get; set; }
        /// <summary>
        /// 节点创建人姓名
        /// </summary>
        public virtual string CreatePersonName { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        public virtual int Sort { get; set; }
        /// <summary>
        /// 结构类型
        /// </summary>
        public virtual PBSTemplateType Type { get; set; }
        /// <summary>
        /// 结构类型编码
        /// </summary>
        public virtual string TypeCode { get; set; }
        /// <summary>
        /// 结构类型名称
        /// </summary>
        public virtual string TypeName { get; set; }
        /// <summary>
        /// 结构类型编号位数
        /// </summary>
        public virtual string TypeBit { get; set; }

    }
}
