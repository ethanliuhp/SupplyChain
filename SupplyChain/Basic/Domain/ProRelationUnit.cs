using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.Basic.Domain
{
    /// <summary>
    /// 工程相关单位
    /// </summary>
    [Serializable]
    public class ProRelationUnit
    {
        private string id;
        /// <summary>
        /// Id
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private int numbers;
        /// <summary>
        /// 序号
        /// </summary>
        virtual public int Numbers
        {
            get { return numbers; }
            set { numbers = value; }
        }
        private string unitType;
        /// <summary>
        /// 单位类型
        /// </summary>
        virtual public string UnitType
        {
            get { return unitType; }
            set { unitType = value; }
        }
        private string unitName;
        /// <summary>
        /// 单位名称
        /// </summary>
        virtual public string UnitName
        {
            get { return unitName; }
            set { unitName = value; }
        }
        private PersonInfo linkPerson;
        /// <summary>
        /// 联系人
        /// </summary>
        virtual public PersonInfo LinkPerson
        {
            get { return linkPerson; }
            set { linkPerson = value; }
        }
        private string linkPersonName;
        /// <summary>
        /// 联系人名称
        /// </summary>
        virtual public string LinkPersonName
        {
            get { return linkPersonName; }
            set { linkPersonName = value; }
        }
        private string linkPhone;
        /// <summary>
        /// 联系电话
        /// </summary>
        virtual public string LinkPhone
        {
            get { return linkPhone; }
            set { linkPhone = value; }
        }
        private CurrentProjectInfo projectId;
        /// <summary>
        /// 项目
        /// </summary>
        virtual public CurrentProjectInfo ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        //private string projectName;
        ///// <summary>
        ///// 项目名称
        ///// </summary>
        //public string ProjectName
        //{
        //    get { return projectName; }
        //    set { projectName = value; }
        //}


    }
}
