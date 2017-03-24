using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain
{
    /// <summary>
    /// 劳务分包价格明细
    /// </summary>
    [Serializable]
    public class LaborSubContractPriceItem
    {
        private string id;
        private long version; 
        private string descript;
        private string constractType;
        private SubContractProject subConProject;  
        private decimal unitPrice;
        private StandardUnit projectAmountUnit; 
        private string projectAmountName;   
        private string jobContent;    
        private StandardUnit priceUnit;    
        private string priceUnitName;
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
        /// 承包方式
        /// </summary>
        virtual public string ConstractType
        {
            get { return constractType; }
            set { constractType = value; }
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
        /// 单价
        /// </summary>
        virtual public decimal UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }
        /// <summary>
        /// 工程量GUID
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
        /// 工作内容
        /// </summary>
        virtual public string JobContent
        {
            get { return jobContent; }
            set { jobContent = value; }
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
        /// 分包项目
        /// </summary>
        public virtual SubContractProject TheProject
        {
            get { return _theProject; }
            set { _theProject = value; }
        }
    }
}
