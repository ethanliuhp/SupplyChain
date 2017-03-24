using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.MaterialResource.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ProjectTaskAccountMng.Domain
{
    /// <summary>
    /// 工程任务明细核算汇总
    /// </summary>
    [Serializable]
    public class ProjectTaskDetailAccountSummary
    {
        private string _id;
        private long _version;
        private string _theProjectGUID;
        private string _theProjectName;
        private decimal _accountProjectAmount;
        private decimal _accountPrice;
        private decimal _accountTotalPrice;
        private StandardUnit _projectAmountUnitGUID;
        private string _projectAmountUnitName;
        private StandardUnit _priceUnitGUID;
        private string _priceUnitName;
        private CollectState _state;
        private string _remark;
        private string _accountNodeGUID;
        private string _accountNodeName;
        private string _accountNodeSysCode;
        private GWBSDetail _projectTaskDtlGUID;
        private string _projectTaskDtlName;
        private ProjectTaskAccountBill _theAccountBill;


        private decimal _currAccFigureProgress;
        private decimal _addupAccQuantity;
        private decimal _addupAccFigureProgress;
        /// <summary>
        /// 本次核算形象进度
        /// </summary>
        public virtual decimal CurrAccFigureProgress
        {
            get { return _currAccFigureProgress; }
            set { _currAccFigureProgress = value; }
        }
        /// <summary>
        /// 累计核算工程量
        /// </summary>
        public virtual decimal AddupAccQuantity
        {
            get { return _addupAccQuantity; }
            set { _addupAccQuantity = value; }
        }
        /// <summary>
        /// 累计核算形象进度
        /// </summary>
        public virtual decimal AddupAccFigureProgress
        {
            get { return _addupAccFigureProgress; }
            set { _addupAccFigureProgress = value; }
        }


        //移除的属性
        private decimal _theCompletePercent;
        private decimal _addupCompletePercent;
        /// <summary>
        /// 本次完成百分比
        /// </summary>
        public virtual decimal TheCompletePercent
        {
            get { return _theCompletePercent; }
            set { _theCompletePercent = value; }
        }
        /// <summary>
        /// 累计完成百分比
        /// </summary>
        public virtual decimal AddupCompletePercent
        {
            get { return _addupCompletePercent; }
            set { _addupCompletePercent = value; }
        }



        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        public virtual long Version
        {
            get { return _version; }
            set { _version = value; }
        }
        /// <summary>
        /// 项目GUID
        /// </summary>
        public virtual string TheProjectGUID
        {
            get { return _theProjectGUID; }
            set { _theProjectGUID = value; }
        }
        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string TheProjectName
        {
            get { return _theProjectName; }
            set { _theProjectName = value; }
        }
        /// <summary>
        /// 本次核算工程量
        /// </summary>
        public virtual decimal AccountProjectAmount
        {
            get { return _accountProjectAmount; }
            set { _accountProjectAmount = value; }
        }
        /// <summary>
        /// 核算单价
        /// </summary>
        public virtual decimal AccountPrice
        {
            get { return _accountPrice; }
            set { _accountPrice = value; }
        }
        /// <summary>
        /// 核算合价
        /// </summary>
        public virtual decimal AccountTotalPrice
        {
            get { return _accountTotalPrice; }
            set { _accountTotalPrice = value; }
        }
        /// <summary>
        /// 工程量计量单位GUID
        /// </summary>
        public virtual StandardUnit ProjectAmountUnitGUID
        {
            get { return _projectAmountUnitGUID; }
            set { _projectAmountUnitGUID = value; }
        }
        /// <summary>
        /// 工程量计量单位名称
        /// </summary>
        public virtual string ProjectAmountUnitName
        {
            get { return _projectAmountUnitName; }
            set { _projectAmountUnitName = value; }
        }
        /// <summary>
        /// 价格计量单位GUID
        /// </summary>
        public virtual StandardUnit PriceUnitGUID
        {
            get { return _priceUnitGUID; }
            set { _priceUnitGUID = value; }
        }
        /// <summary>
        /// 价格计量单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return _priceUnitName; }
            set { _priceUnitName = value; }
        }
        /// <summary>
        /// 汇集状态
        /// </summary>
        public virtual CollectState State
        {
            get { return _state; }
            set { _state = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        /// <summary>
        /// 核算节点GUID
        /// </summary>
        public virtual string AccountNodeGUID
        {
            get { return _accountNodeGUID; }
            set { _accountNodeGUID = value; }
        }
        /// <summary>
        /// 核算节点名称
        /// </summary>
        public virtual string AccountNodeName
        {
            get { return _accountNodeName; }
            set { _accountNodeName = value; }
        }
        /// <summary>
        /// 核算WBS节点层次码
        /// </summary>
        public virtual string AccountNodeSysCode
        {
            get { return _accountNodeSysCode; }
            set { _accountNodeSysCode = value; }
        }
        /// <summary>
        /// 工程任务明细
        /// </summary>
        public virtual GWBSDetail ProjectTaskDtlGUID
        {
            get { return _projectTaskDtlGUID; }
            set { _projectTaskDtlGUID = value; }
        }
        /// <summary>
        /// 工程任务明细名称
        /// </summary>
        public virtual string ProjectTaskDtlName
        {
            get { return _projectTaskDtlName; }
            set { _projectTaskDtlName = value; }
        }
        /// <summary>
        /// 核算单
        /// </summary>
        public virtual ProjectTaskAccountBill TheAccountBill
        {
            get { return _theAccountBill; }
            set { _theAccountBill = value; }
        }
    }
    /// <summary>
    /// 汇集状态
    /// </summary>
    public enum CollectState
    {
        /// <summary>
        /// 该核算结果未汇集到{工程任务明细}相关状态量上
        /// </summary>
        [Description("未汇集")]
        未汇集 = 1,
        /// <summary>
        /// 该核算结果未汇集到{工程任务明细}相关状态量上
        /// </summary>
        [Description("已汇集")]
        已汇集 = 2
    }
}
