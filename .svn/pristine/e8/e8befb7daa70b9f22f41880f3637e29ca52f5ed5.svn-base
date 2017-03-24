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
    /// 项目降低率
    /// </summary>
    [Serializable]
    public class ProgramReduceRate
    {
        private string id;
        private DateTime lastUpdateDate = DateTime.Now;
        private string state;
        private PersonInfo makePerson;
        private string makePersonName;
        private DateTime makeTime;
        private string projectId;
        private string projectName;
        private decimal rate;
        private string descript;
        private SupplierRelationInfo supplyer;
        private string supplyerName;
        private MaterialCategory materialCategory;
        private string materialCategoryName;
        private decimal rateMoney;

        /// <summary>
        /// 降低值
        /// </summary>
        virtual public decimal RateMoney
        {
            get { return rateMoney; }
            set { rateMoney = value; }
        }

        /// 材料类型
        /// </summary>
        virtual public MaterialCategory MaterialCategory
        {
            get { return materialCategory; }
            set { materialCategory = value; }
        }
        /// <summary>
        /// 材料类型名称
        /// </summary>
        virtual public string MaterialCategoryName
        {
            get { return materialCategoryName; }
            set { materialCategoryName = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        virtual public string State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// 供应商
        /// </summary>
        virtual public SupplierRelationInfo Supplyer
        {
            get { return supplyer; }
            set { supplyer = value; }
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        virtual public string SupplyerName
        {
            get { return supplyerName; }
            set { supplyerName = value; }
        }
        /// <summary>
        /// 编号
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 项目Id
        /// </summary>
        public virtual string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }
        /// <summary>
        /// 制定人
        /// </summary>
        public virtual PersonInfo MakePerson
        {
            get { return makePerson; }
            set { makePerson = value; }
        }
        /// <summary>
        /// 制定人名称
        /// </summary>
        public virtual string MakePersonName
        {
            get { return makePersonName; }
            set { makePersonName = value; }
        }
        /// <summary>
        /// 定制时间
        /// </summary>
        public virtual DateTime MakeTime
        {
            get { return makeTime; }
            set { makeTime = value; }
        }
        /// <summary>
        /// 降低率
        /// </summary>
        public virtual decimal Rate
        {
            get { return rate; }
            set { rate = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }
    }
}
