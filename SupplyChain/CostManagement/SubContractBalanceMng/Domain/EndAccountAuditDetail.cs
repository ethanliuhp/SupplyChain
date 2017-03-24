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
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain
{
    /// <summary>
    /// 分包终结结算及审核
    /// </summary>
    [Serializable]
    [Entity]
    public class EndAccountAuditDetail : BaseDetail
    {
        private string _id;
        private long _version;

        private GWBSTree _balanceTask;
        private string _balanceTaskName;
        private string _balanceTaskSyscode;
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
        private EndAccountAuditBill _master;
        //private ISet<SubContractBalanceSubjectDtl> _Details = new HashedSet<SubContractBalanceSubjectDtl>();

        private decimal _confirmQuantity;
        //private decimal _accountQuantity;
        //private decimal _accountPrice;
        private string _balanceBase;
        private string _remarks;
        private string _useDescript;//用工说明
        private PersonInfo _handlePerson;
        private string _handlePersonName;
        //private decimal _planWorkAmount;
        //private decimal _planTotalprice;
        //private decimal _addBalanceQuantity;
        //private decimal _addBalanceMoney;
        private decimal _tempAddBalanceQuantity;
        private decimal _tempAddBalanceMoney;

        private string _SJGCL;//审减工程量
        private string _SJDJ;//审减单价
        private string _SJJE;//审减金额
        private string _SHYJ;//审核意见

        ///// <summary>
        ///// 计划工程量
        ///// </summary>
        //public virtual decimal PlanWorkAmount
        //{
        //    get { return _planWorkAmount; }
        //    set { _planWorkAmount = value; }
        //}
        ///// <summary>
        ///// 计划金额
        ///// </summary>
        //public virtual decimal PlanTotalprice
        //{
        //    get { return _planTotalprice; }
        //    set { _planTotalprice = value; }
        //}
        ///// <summary>
        ///// 累计结算工程量
        ///// </summary>
        //public virtual decimal AddBalanceQuantity
        //{
        //    get { return _addBalanceQuantity; }
        //    set { _addBalanceQuantity = value; }
        //}
        ///// <summary>
        ///// 累计结算金额
        ///// </summary>
        //public virtual decimal AddBalanceMoney
        //{
        //    get { return _addBalanceMoney; }
        //    set { _addBalanceMoney = value; }
        //}
        /// <summary>
        /// 临时累计结算工程量
        /// </summary>
        public virtual decimal TempAddBalanceQuantity
        {
            get { return _tempAddBalanceQuantity; }
            set { _tempAddBalanceQuantity = value; }
        }
        /// <summary>
        /// 临时累计结算金额
        /// </summary>
        public virtual decimal TempAddBalanceMoney
        {
            get { return _tempAddBalanceMoney; }
            set { _tempAddBalanceMoney = value; }
        }
        /// <summary>
        /// 工长
        /// </summary>
        public virtual PersonInfo HandlePerson
        {
            get { return _handlePerson; }
            set { _handlePerson = value; }
        }
        /// <summary>
        /// 工长姓名
        /// </summary>
        public virtual string HandlePersonName
        {
            get { return _handlePersonName; }
            set { _handlePersonName = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }
        /// <summary>
        /// 用工说明
        /// </summary>
        public virtual string UseDescript
        {
            get { return _useDescript; }
            set { _useDescript = value; }
        }
        /// <summary>
        /// 结算依据
        /// </summary>
        public virtual string BalanceBase
        {
            get { return _balanceBase; }
            set { _balanceBase = value; }
        }
        /// <summary>
        /// 工长确认工程量
        /// </summary>
        public virtual decimal ConfirmQuantity
        {
            get { return _confirmQuantity; }
            set { _confirmQuantity = value; }
        }
        ///// <summary>
        ///// 核算工程量
        ///// </summary>
        //public virtual decimal AccountQuantity
        //{
        //    get { return _accountQuantity; }
        //    set { _accountQuantity = value; }
        //}
        ///// <summary>
        ///// 核算单价
        ///// </summary>
        //public virtual decimal AccountPrice
        //{
        //    get { return _accountPrice; }
        //    set { _accountPrice = value; }
        //}


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
        /// 结算任务层次码
        /// </summary>
        public virtual string BalanceTaskSyscode
        {
            get { return _balanceTaskSyscode; }
            set { _balanceTaskSyscode = value; }
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
        public virtual EndAccountAuditBill Master
        {
            get { return _master; }
            set { _master = value; }
        }

        ///// <summary>
        ///// 分包结算分科目明细
        ///// </summary>
        //public virtual ISet<SubContractBalanceSubjectDtl> Details
        //{
        //    get { return _Details; }
        //    set { _Details = value; }
        //}

        /// <summary>
        /// 审减工程量
        /// </summary>
        public virtual string SJGCL
        {
            get { return _SJGCL; }
            set { _SJGCL = value; }
        }
        /// <summary>
        /// 审减单价
        /// </summary>
        public virtual string SJDJ
        {
            get { return _SJDJ; }
            set { _SJDJ = value; }
        }
        /// <summary>
        /// 审减金额
        /// </summary>
        public virtual string SJJE
        {
            get { return _SJJE; }
            set { _SJJE = value; }
        }
        /// <summary>
        /// 审核意见
        /// </summary>
        public virtual string SHYJ
        {
            get { return _SHYJ; }
            set { _SHYJ = value; }
        }
    }
    ///// <summary>
    ///// 前驱但据类型
    ///// </summary>
    ///// 前驱单据类型：

    //public enum FrontBillType
    //{
    //    其他 = 0,
    //    工程任务核算 = 1,
    //    罚款单 = 2,
    //    扣款单 = 3,
    //    零星用工单 = 4,
    //    代工单 = 5,
    //    措施 = 6,
    //    税金 = 7,
    //    水电 = 8,
    //    建管 = 9,
    //    固定总价 = 10,
    //    计时派工 = 11,
    //    暂扣款单 = 12,
    //    分包签证 = 13

    //}
}