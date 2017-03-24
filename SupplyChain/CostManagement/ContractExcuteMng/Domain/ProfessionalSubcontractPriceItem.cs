using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain
{
    /// <summary>
    /// 专业分包价格明细
    /// </summary>
    [Serializable]
    public class ProfessionalSubcontractPriceItem
    {
        private string id;   
        private long version;
        private string descript;
        private SubContractProject subConProject;
        private decimal projectAmount;
        private StandardUnit projectAmountUnit;
        private string projectAmountName;
        private string jobType;
        private string jobContent;    
        private string productModel;       
        private StandardUnit priceUnit;   
        private string priceUnitName;
        private decimal provisionalPrice;
        private decimal provisionalTotalPrice;
        private string qualityLevel;
        private SubContractProject _theProject;

        /// <summary>
        /// Id
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Descript
        {
            get { return descript; }
            set { descript = value; }
        }
        /// <summary>
        /// 分包项目GUID
        /// </summary>
        virtual public SubContractProject SubConProject
        {
            get { return subConProject; }
            set { subConProject = value; }
        }
        /// <summary>
        /// 工程量
        /// </summary>
        virtual public decimal ProjectAmount
        {
            get { return projectAmount; }
            set { projectAmount = value; }
        }
        /// <summary>
        /// 工程量单位GIUD
        /// </summary>
        virtual public StandardUnit ProjectAmountUnit
        {
            get { return projectAmountUnit; }
            set { projectAmountUnit = value; }
        }
        /// <summary>
        /// 工程量单位
        /// </summary>
        virtual public string ProjectAmountName
        {
            get { return projectAmountName; }
            set { projectAmountName = value; }
        }
        /// <summary>
        /// 工作类型
        /// </summary>
        virtual public string JobType
        {
            get { return jobType; }
            set { jobType = value; }
        }
        /// <summary>
        /// 工作内容
        /// </summary>
        virtual public string JobContent
        {
            get { return jobContent; }
            set { jobContent = value; }
        }
        /// <summary>
        /// 规格型号
        /// </summary>
        virtual public string ProductModel
        {
            get { return productModel; }
            set { productModel = value; }
        }
        /// <summary>
        /// 价格单位GUID
        /// </summary>
        virtual public StandardUnit PriceUnit
        {
            get { return priceUnit; }
            set { priceUnit = value; }
        }
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public string PriceUnitName
        {
            get { return priceUnitName; }
            set { priceUnitName = value; }
        }
        /// <summary>
        /// 暂定单价
        /// </summary>
        virtual public decimal ProvisionalPrice
        {
            get { return provisionalPrice; }
            set { provisionalPrice = value; }
        }
        /// <summary>
        /// 暂定总价
        /// </summary>
        virtual public decimal ProvisionalTotalPrice
        {
            get { return provisionalTotalPrice; }
            set { provisionalTotalPrice = value; }
        }
        /// <summary>
        /// 质量标准
        /// </summary>
        virtual public string QualityLevel
        {
            get { return qualityLevel; }
            set { qualityLevel = value; }
        }
        /// <summary>
        /// 分包项目
        /// </summary>
        public virtual SubContractProject TheProject
        {
            get { return _theProject; }
            set { _theProject = value; }
        }
    }
}
