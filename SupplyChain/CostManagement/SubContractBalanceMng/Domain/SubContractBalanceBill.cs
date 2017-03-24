using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;
using System.ComponentModel;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain
{
    /// <summary>
    /// 分包结算单
    /// </summary>
    [Serializable]
    [Entity]
    public class SubContractBalanceBill : BaseMaster
    {
        private SupplierRelationInfo _subContractUnitGUID;
        private string _subContractUnitName;
        private DateTime _beginTime;
        private DateTime _endTime;
        private SubContractBalanceBillState _state;
        private GWBSTree _balanceRange;
        private string _balanceTaskName;
        private string _balanceTaskSyscode;
        //private ISet<SubContractBalanceDetail> _listDetails = new HashedSet<SubContractBalanceDetail>();

        private MonthAccountFlag _monthAccFlag = MonthAccountFlag.未进行月度核算;
        private string _monthAccBill;
        private SubContractProject _theSubContractProject;
        private decimal _balanceMoney;
        private decimal _cumulativeMoney; 

        /// <summary>
        /// 月度核算标志
        /// </summary>
        public virtual MonthAccountFlag MonthAccFlag
        {
            get { return _monthAccFlag; }
            set { _monthAccFlag = value; }
        }
        /// <summary>
        /// 月度核算单GUID
        /// </summary>
        public virtual string MonthAccBill
        {
            get { return _monthAccBill; }
            set { _monthAccBill = value; }
        }
        /// <summary>
        /// 所属分包项目
        /// </summary>
        public virtual SubContractProject TheSubContractProject
        {
            get { return _theSubContractProject; }
            set { _theSubContractProject = value; }
        }
        /// <summary>
        /// 结算金额
        /// </summary>
        public virtual decimal BalanceMoney
        {
            get { return _balanceMoney; }
            set { _balanceMoney = value; }
        }
        /// <summary>
        /// 累计结算金额
        /// </summary>
        public virtual decimal CumulativeMoney
        {
            get { return _cumulativeMoney; }
            set { _cumulativeMoney = value; }
        }


        /// <summary>
        /// 分包单位（被结算的分包商）
        /// </summary>
        public virtual SupplierRelationInfo SubContractUnitGUID
        {
            get { return _subContractUnitGUID; }
            set { _subContractUnitGUID = value; }
        }
        /// <summary>
        /// 分包单位名称（被结算的分包商名称）
        /// </summary>
        public virtual string SubContractUnitName
        {
            get { return _subContractUnitName; }
            set { _subContractUnitName = value; }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        public virtual DateTime BeginTime
        {
            get { return _beginTime; }
            set { _beginTime = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public virtual DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
        /// <summary>
        /// 结算范围,结算的GWBS树节点
        /// </summary>
        public virtual GWBSTree BalanceRange
        {
            get { return _balanceRange; }
            set { _balanceRange = value; }
        }
        /// <summary>
        /// 结算的GWBS树节点名称
        /// </summary>
        public virtual string BalanceTaskName
        {
            get { return _balanceTaskName; }
            set { _balanceTaskName = value; }
        }
        /// <summary>
        /// 结算的GWBS树节点层次码
        /// </summary>
        public virtual string BalanceTaskSyscode
        {
            get { return _balanceTaskSyscode; }
            set { _balanceTaskSyscode = value; }
        }
        /// <summary>
        /// 结算明细
        /// </summary>
        //public virtual ISet<SubContractBalanceDetail> ListDetails
        //{
        //    get { return _listDetails; }
        //    set { _listDetails = value; }
        //}

        private string _createBatchNo;
        /// <summary>
        /// 生成批次号
        /// </summary>
        public virtual string CreateBatchNo
        {
            get { return _createBatchNo; }
            set { _createBatchNo = value; }
        }
    }
    /// <summary>
    /// 分包结算状态
    /// </summary>
    public enum SubContractBalanceBillState
    {
        /// <summary>
        /// 预算员编制单据
        /// </summary>
        [Description("制定")]
        制定 = 0,
        /// <summary>
        /// 预算员提交单据
        /// </summary>
        [Description("提交")]
        提交 = 1,
        /// <summary>
        /// 通过项目经理审批
        /// </summary>
        [Description("审批")]
        审批 = 2,
        /// <summary>
        /// 由分包商确认
        /// </summary>
        [Description("确认")]
        确认 = 3,
        /// <summary>
        /// 提交到财务执行
        /// </summary>
        [Description("执行")]
        执行 = 4,
        [Description("冻结")]
        冻结 = 5,
        [Description("作废")]
        作废 = 6
    }

    /// <summary>
    /// 月度核算标志
    /// </summary>
    public enum MonthAccountFlag
    {
        [Description("未进行月度核算")]
        未进行月度核算 = 0,
        [Description("已进行月度核算")]
        已进行月度核算 = 1
    }

}
