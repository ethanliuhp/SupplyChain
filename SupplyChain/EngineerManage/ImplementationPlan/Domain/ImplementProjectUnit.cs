using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.ImplementationPlan.Domain
{
    [Serializable]
    public class ImplementProjectUnit
    {
        private string id; 
        private string guidName;
        private decimal unitStyle;
        private string unitName;
        private string implementationBook;
        private string entityGuid;
        private string serialNO;
        private ImplementationMaintain master;
        /// <summary>
        /// 主表
        /// </summary>
        virtual public ImplementationMaintain Master
        {
            get { return master; }
            set { master = value; }
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
        /// GUID名称
        /// </summary>
        virtual public string GuidName
        {
            get { return guidName; }
            set { guidName = value; }
        }
        /// <summary>
        /// 单位类型
        /// </summary>
        virtual public decimal UnitStyle
        {
            get { return unitStyle; }
            set { unitStyle = value; }
        }
        /// <summary>
        /// 单位名称
        /// </summary>
        virtual public string UnitName
        {
            get { return unitName; }
            set { unitName = value; }
        }
        /// <summary>
        /// 实施策划书
        /// </summary>
        virtual public string ImplementationBook
        {
            get { return implementationBook; }
            set { implementationBook = value; }
        }
          /// <summary>
        /// 实体GUID
        /// </summary>
        virtual public string EntityGuid
        {
            get { return entityGuid; }
            set { entityGuid = value; }
        }
        /// <summary>
        /// 序号
        /// </summary>
        virtual public string SerialNO
        {
            get { return serialNO; }
            set { serialNO = value; }
        }
    }
}
