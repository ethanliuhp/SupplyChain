using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireCollection.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain
{
    [Serializable]
    public class MatHireStockBlockMaster : BaseMaster
    {
        private DateTime blockStartTime;
        private DateTime blockFinishTime;
        private string stockReason;
        private string theme;
        private string contacter;
        //private int state;
        private string supplierName;
      private   EnumMatHireType matHireType;
        private SupplierRelationInfo theSupplierRelationInfo;
        private string operationorg;
        private MatHireOrderMaster contract;
        private string contractCode;
        int balState;
        int balYear;
        int balMonth;
        private string billCode;
        /// <summary>料具站手工录入单据号</summary>
        public virtual string BillCode
        {
            get { return billCode; }
            set { billCode = value; }
        }
        private string balRule;
        /// <summary>
        /// 料具合同
        /// </summary>
        public virtual MatHireOrderMaster Contract
        {
            get { return contract; }
            set { contract = value; }
        }
        /// <summary>
        /// 料具合同编号
        /// </summary>
        public virtual string ContractCode
        {
            get { return contractCode; }
            set { contractCode = value; }
        }
        public virtual string OperationOrg
        {
            get { return operationorg; }
            set { operationorg = value; }
        }
        private string operationorgname;
        public virtual string OperationOrgName
        {
            get { return operationorgname; }
            set { operationorgname = value; }
        }
        /// <summary>
        /// 封存开始时间
        /// </summary>
        public virtual DateTime BlockStartTime
        {
            get { return blockStartTime; }
            set { blockStartTime = value; }
        }
        /// <summary>
        /// 封存结束时间
        /// </summary>
        public virtual DateTime BlockFinishTime
        {
            get { return blockFinishTime; }
            set { blockFinishTime = value; }
        }
        /// <summary>
        /// 封存事由
        /// </summary>
        public virtual string StockReason
        {
            get { return stockReason; }
            set { stockReason = value; }
        }
        /// <summary>
        /// 主题
        /// </summary>
        public virtual string Theme
        {
            get { return theme; }
            set { theme = value; }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        public virtual string Contacter
        {
            get { return contacter; }
            set { contacter = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        //public virtual int State
        //{
        //    get { return state; }
        //    set { state = value; }
        //}
        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
      

        /// <summary>
        /// 出租方(供应商)
        /// </summary>
        public virtual SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }
        /// <summary>
        /// 料具类型
        /// </summary>
        public virtual EnumMatHireType MatHireType
        {
            get { return matHireType; }
            set { matHireType = value; }
        }
 
        /// <summary>
        /// 结算状态
        /// </summary>
        public virtual int BalState
        {
            get { return balState; }
            set { balState = value; }
        }
        /// <summary>
        /// 结算规则
        /// </summary>
        public virtual string BalRule
        {
            get { return balRule; }
            set { balRule = value; }
        }
        /// <summary>
        /// 结算年
        /// </summary>
        public virtual int BalYear
        {
            get { return balYear; }
            set { balYear = value; }
        }
        /// <summary>
        /// 结算月
        /// </summary>
        public virtual int BalMonth
        {
            get { return balMonth; }
            set { balMonth = value; }
        }
    }
}
