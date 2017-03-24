using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireTransportCost.Domain
{
    /// <summary>
    /// 运输费主表
    /// </summary>
    [Serializable]
    public class MatHireTranCostMaster : BaseMaster
    {
        MatHireOrderMaster contract;
        string contractCode;
        SupplierRelationInfo theSupplierRelationInfo;
        string supplierName;
        string projectName;
        string projectId;
        DateTime transportTime;
        int balState;
        int balYear;
        int balMonth;
        decimal sumMoney;
        private string billCode;
        /// <summary>料具站手工录入单据号</summary>
        public virtual string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }
        /// <summary>
        /// 合同
        /// </summary>
        public virtual MatHireOrderMaster Contract
        {
            get { return contract; }
            set { contract = value; }
        }
        /// <summary>
        /// 合同编码
        /// </summary>
        public virtual string ContractCode
        {
            get { return contractCode; }
            set { contractCode = value; }
        }
        /// <summary>
        /// 出租方/供应商
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }
        /// <summary> 出租方/供应商名称 </summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        /// <summary> 租赁方项目名称 </summary>
        virtual public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        /// <summary> 租赁方项目ID </summary>
        virtual public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        /// <summary> 运输时间 </summary>
        virtual public DateTime TransportTime
        {
            get { return transportTime; }
            set { transportTime = value; }
        }
        virtual public decimal SumMoney
        {
            get
            {
                return sumMoney;
            }
            set { sumMoney = value; }
        }
        virtual public int BalState
        {
            get { return balState; }
            set { balState = value; }
        }
        virtual public int BalYear
        {
            get { return balYear; }
            set { balYear = value; }
        }
        virtual public int BalMonth
        {
            get { return balMonth; }
            set { balMonth = value; }
        }
    }
}
