using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.ComponentModel;
using Iesi.Collections;
using Iesi.Collections.Generic;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain
{
    /// <summary>
    /// 分包结算明细
    /// </summary>
    [Serializable]
    [Entity]
    public class SubContractBalanceDetail
    {
        private string _id;
        private long _version;

        private GWBSTree _balanceTask;
        private string _balanceTaskName;
        private GWBSDetail _balanceTaskDtl;
        private string _balanceTaskDtlName;
        private StandardUnit _priceUnit;
        private string _priceUnitName;
        private decimal _balancePrice;
        private decimal _balanceTotalPrice;
        private decimal _balacneQuantity;
        private FrontBillType _fontBillType;
        private string _frontBillGUID;
        private StandardUnit _quantityUnit;
        private string _quantityUnitName;

        private string _projectId;
        private string _projectName;

        private SubContractBalanceBill _theBalanceBill;

        private ISet<SubContractBalanceSubjectDtl> _Details = new HashedSet<SubContractBalanceSubjectDtl>();


        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return _version; }
            set { _version = value; }
        }
        /// <summary>
        /// 结算任务
        /// </summary>
        public virtual GWBSTree BalanceTask
        {
            get { return _balanceTask; }
            set { _balanceTask = value; }
        }
        /// <summary>
        /// 结算任务名称
        /// </summary>
        public virtual string BalanceTaskName
        {
            get { return _balanceTaskName; }
            set { _balanceTaskName = value; }
        }
        /// <summary>
        /// 结算任务明细
        /// </summary>
        public virtual GWBSDetail BalanceTaskDtl
        {
            get { return _balanceTaskDtl; }
            set { _balanceTaskDtl = value; }
        }
        /// <summary>
        /// 结算任务明细名称
        /// </summary>
        public virtual string BalanceTaskDtlName
        {
            get { return _balanceTaskDtlName; }
            set { _balanceTaskDtlName = value; }
        }
        /// <summary>
        /// 单价单位
        /// </summary>
        public virtual StandardUnit PriceUnit
        {
            get { return _priceUnit; }
            set { _priceUnit = value; }
        }
        /// <summary>
        /// 单价单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return _priceUnitName; }
            set { _priceUnitName = value; }
        }
        /// <summary>
        /// 结算单价
        /// </summary>
        public virtual decimal BalancePrice
        {
            get { return _balancePrice; }
            set { _balancePrice = value; }
        }
        /// <summary>
        /// 结算合价
        /// </summary>
        public virtual decimal BalanceTotalPrice
        {
            get { return _balanceTotalPrice; }
            set { _balanceTotalPrice = value; }
        }
        /// <summary>
        /// 结算数量
        /// </summary>
        public virtual decimal BalacneQuantity
        {
            get { return _balacneQuantity; }
            set { _balacneQuantity = value; }
        }
        /// <summary>
        /// 前驱单据类型
        /// </summary>
        public virtual FrontBillType FontBillType
        {
            get { return _fontBillType; }
            set { _fontBillType = value; }
        }
        /// <summary>
        /// 前驱单据GUID
        /// </summary>
        public virtual string FrontBillGUID
        {
            get { return _frontBillGUID; }
            set { _frontBillGUID = value; }
        }
        /// <summary>
        /// 数量单位
        /// </summary>
        public virtual StandardUnit QuantityUnit
        {
            get { return _quantityUnit; }
            set { _quantityUnit = value; }
        }
        /// <summary>
        /// 数量单位名称
        /// </summary>
        public virtual string QuantityUnitName
        {
            get { return _quantityUnitName; }
            set { _quantityUnitName = value; }
        }
        /// <summary>
        /// 结算单
        /// </summary>
        public virtual SubContractBalanceBill TheBalanceBill
        {
            get { return _theBalanceBill; }
            set { _theBalanceBill = value; }
        }

        /// <summary>
        /// 所属项目ID
        /// </summary>
        public virtual string ProjectId
        {
            get { return _projectId; }
            set { _projectId = value; }
        }
        /// <summary>
        /// 所属项目名称
        /// </summary>
        public virtual string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        /// <summary>
        /// 分包结算分科目明细
        /// </summary>
        public virtual ISet<SubContractBalanceSubjectDtl> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
    }
    /// <summary>
    /// 前驱但据类型
    /// </summary>
    public enum FrontBillType
    {
        [Description("工程任务核算明细")]
        工程任务核算明细 = 0,
        [Description("零星用工明细")]
        零星用工明细 = 1,
        [Description("罚扣款单明细")]
        罚扣款单明细 = 2
    }
}
