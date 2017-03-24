using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;


namespace Application.Business.Erp.SupplyChain.Indicator.IndicatorDefine.Domain
{
    /// <summary>
    /// 指标定义
    /// </summary>
    [Serializable]
    [Entity]
    public class IndicatorDefinition
    {
        private string id;

        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private long version = -1;

        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        private string code;//指标编码

        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string name;//指标名称

        virtual public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string unitId;//计量单位

        virtual public string UnitId
        {
            get { return unitId; }
            set { unitId = value; }
        }
        private string description;//备注

        virtual public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private DateTime createdDate;
        private PersonInfo author;
        private OperationOrgInfo theOpeOrg;
        private int state = 1;
        private IndicatorCategory category;

        /// <summary>
        /// 指标分类
        /// </summary>
        virtual public IndicatorCategory Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        virtual public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 创建部门
        /// </summary>
        virtual public OperationOrgInfo TheOpeOrg
        {
            get { return theOpeOrg; }
            set { theOpeOrg = value; }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        virtual public PersonInfo Author
        {
            get { return author; }
            set { author = value; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        virtual public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }
    }
}
