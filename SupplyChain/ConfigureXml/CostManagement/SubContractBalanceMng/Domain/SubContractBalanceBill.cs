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
        private ISet<SubContractBalanceDetail> _listDetails = new HashedSet<SubContractBalanceDetail>();


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
        /// 状态
        /// </summary>
        public virtual SubContractBalanceBillState State
        {
            get { return _state; }
            set { _state = value; }
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
        /// 结算明细
        /// </summary>
        public virtual ISet<SubContractBalanceDetail> ListDetails
        {
            get { return _listDetails; }
            set { _listDetails = value; }
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

}
