using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Basic.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    [Serializable]
   public  class CostReporterConfig
    {
        private string id;
       
        private CurrentProjectInfo project;
        private string  materialCategoryID;
        private string projectName;
        private string categoryType;
        private string categoryCode;
        private string categoryName;
        private string displayName;
        private int  orderNo;
        private int tLevel;
        Int64 version;
        private string path;
        /// <summary>
        /// 物资分类路径
        /// </summary>
        public virtual string Path
        {
            get { return path; }
            set { path = value; }
        }
        public virtual Int64 Version
        {
            get { return version; }
            set { version = value; }
        }
        
        /// <summary>
        ///id
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 项目id
        /// </summary>
        public virtual CurrentProjectInfo Project
        {
            get { return project; }
            set { project = value; }
        }

        /// <summary>
        /// 项目
        /// </summary>
        public virtual string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        public virtual string  MaterialCategoryID
        {
            get { return materialCategoryID; }
            set { materialCategoryID = value; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public virtual string CategoryType
        {
            get { return categoryType; }
            set { categoryType = value; }
        }
        /// <summary>
        /// 资源分类编码
        /// </summary>
        public virtual string MaterialCategoryCode
        {
            get { return categoryCode; }
            set { categoryCode = value; }
        }
        /// <summary>
        /// 资源分类名称
        /// </summary>
        public virtual string MaterialCategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }
        /// <summary>
        /// 序号
        /// </summary>
        public virtual int OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        /// <summary>
        /// 分类树级别
        /// </summary>
        public virtual int TLevel
        {
            get { return tLevel; }
            set { tLevel = value; }
        }

    }
}
