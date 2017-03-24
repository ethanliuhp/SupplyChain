using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;
using System.ComponentModel;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.MaterialResource.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 月度工程任务明细核算
    /// </summary>
    [Serializable]
    public class CostMonthAccountDtl
    {
        private string _id;

        private GWBSTree _accountTaskNodeGUID;
        private string _accountTaskNodeName;
        private string _accountTaskNodeSyscode;
        private GWBSDetail _projectTaskDtlGUID;
        private string _projectTaskDtlName;
        private CostItem _theCostItem;
        private string _costItemName;
        private StandardUnit _quantityUnitGUID;
        private string _quantityUnitName;
        private StandardUnit _priceUnitGUID;
        private string _priceUnitName;
        private CostMonthAccountBill _theAccountBill;
        //量价信息(当前)
        private decimal currRealQuantity;
        private decimal currRealPrice;
        private decimal currRealTotalPrice;
        private decimal currIncomeQuantity;
        private decimal currIncomeTotalPrice;
        private decimal currResponsiQuantity;
        private decimal currResponsiTotalPrice;
        private decimal currEarnValue;
        private decimal currCompletedPercent;
        //量价信息(累计)
        private decimal sumRealQuantity;
        private decimal sumRealTotalPrice;
        private decimal sumIncomeQuantity;
        private decimal sumIncomeTotalPrice;
        private decimal sumResponsiQuantity;
        private decimal sumResponsiTotalPrice;
        private decimal sumEarnValue;
        private decimal sumCompletedPercent;

        private string _remark;

        ISet<CostMonthAccDtlConsume> _Details = new HashedSet<CostMonthAccDtlConsume>();


        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 核算工程任务节点
        /// </summary>
        public virtual GWBSTree AccountTaskNodeGUID
        {
            get { return _accountTaskNodeGUID; }
            set { _accountTaskNodeGUID = value; }
        }

        /// <summary>
        /// 核算工程任务节点名称
        /// </summary>
        public virtual string AccountTaskNodeName
        {
            get { return _accountTaskNodeName; }
            set { _accountTaskNodeName = value; }
        }

        /// <summary>
        /// 工程任务节点层次码
        /// </summary>
        public virtual string AccountTaskNodeSyscode
        {
            get { return _accountTaskNodeSyscode; }
            set { _accountTaskNodeSyscode = value; }
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
        /// 成本项
        /// </summary>
        public virtual CostItem TheCostItem
        {
            get { return _theCostItem; }
            set { _theCostItem = value; }
        }

        /// <summary>
        /// 成本项名称
        /// </summary>
        public virtual string CostItemName
        {
            get { return _costItemName; }
            set { _costItemName = value; }
        }

        /// <summary>
        /// 数量单位
        /// </summary>
        public virtual StandardUnit QuantityUnitGUID
        {
            get { return _quantityUnitGUID; }
            set { _quantityUnitGUID = value; }
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
        /// 价格单位
        /// </summary>
        public virtual StandardUnit PriceUnitGUID
        {
            get { return _priceUnitGUID; }
            set { _priceUnitGUID = value; }
        }

        /// <summary>
        /// 价格单位名称
        /// </summary>
        public virtual string PriceUnitName
        {
            get { return _priceUnitName; }
            set { _priceUnitName = value; }
        }

        /// <summary>
        /// 核算单
        /// </summary>
        public virtual CostMonthAccountBill TheAccountBill
        {
            get { return _theAccountBill; }
            set { _theAccountBill = value; }
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
        /// 工程任务明细分科目核算
        /// </summary>
        public virtual ISet<CostMonthAccDtlConsume> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        /// <summary>
        /// 当前实际工程量
        /// </summary>
        public virtual decimal CurrRealQuantity
        {
            get { return currRealQuantity; }
            set { currRealQuantity = value; }
        }

        /// <summary>
        /// 当期实际成本单价
        /// </summary>
        public virtual decimal CurrRealPrice
        {
            get { return currRealPrice; }
            set { currRealPrice = value; }
        }

        /// <summary>
        /// 当期实际成本合价
        /// </summary>
        public virtual decimal CurrRealTotalPrice
        {
            get { return currRealTotalPrice; }
            set { currRealTotalPrice = value; }
        }

        /// <summary>
        /// 当期合同收入实现量
        /// </summary>
        public virtual decimal CurrIncomeQuantity
        {
            get { return currIncomeQuantity; }
            set { currIncomeQuantity = value; }
        }

        /// <summary>
        /// 当期合同收入合价
        /// </summary>
        public virtual decimal CurrIncomeTotalPrice
        {
            get { return currIncomeTotalPrice; }
            set { currIncomeTotalPrice = value; }
        }

        /// <summary>
        /// 当期责任成本实现量
        /// </summary>
        public virtual decimal CurrResponsiQuantity
        {
            get { return currResponsiQuantity; }
            set { currResponsiQuantity = value; }
        }

        /// <summary>
        /// 当期责任成本合价
        /// </summary>
        public virtual decimal CurrResponsiTotalPrice
        {
            get { return currResponsiTotalPrice; }
            set { currResponsiTotalPrice = value; }
        }

        /// <summary>
        /// 当期挣值
        /// </summary>
        public virtual decimal CurrEarnValue
        {
            get { return currEarnValue; }
            set { currEarnValue = value; }
        }

        /// <summary>
        /// 当期核算形象进度
        /// </summary>
        public virtual decimal CurrCompletedPercent
        {
            get { return currCompletedPercent; }
            set { currCompletedPercent = value; }
        }

        /// <summary>
        /// 累积实际工程量
        /// </summary>
        public virtual decimal SumRealQuantity
        {
            get { return sumRealQuantity; }
            set { sumRealQuantity = value; }
        }

        /// <summary>
        /// 累积实际成本合价
        /// </summary>
        public virtual decimal SumRealTotalPrice
        {
            get { return sumRealTotalPrice; }
            set { sumRealTotalPrice = value; }
        }

        /// <summary>
        /// 累积合同收入实现量
        /// </summary>
        public virtual decimal SumIncomeQuantity
        {
            get { return sumIncomeQuantity; }
            set { sumIncomeQuantity = value; }
        }

        /// <summary>
        /// 累积合同收入合价
        /// </summary>
        public virtual decimal SumIncomeTotalPrice
        {
            get { return sumIncomeTotalPrice; }
            set { sumIncomeTotalPrice = value; }
        }

        /// <summary>
        /// 累积责任成本实现量
        /// </summary>
        public virtual decimal SumResponsiQuantity
        {
            get { return sumResponsiQuantity; }
            set { sumResponsiQuantity = value; }
        }

        /// <summary>
        /// 累积责任成本合价
        /// </summary>
        public virtual decimal SumResponsiTotalPrice
        {
            get { return sumResponsiTotalPrice; }
            set { sumResponsiTotalPrice = value; }
        }

        /// <summary>
        /// 累积挣值
        /// </summary>
        public virtual decimal SumEarnValue
        {
            get { return sumEarnValue; }
            set { sumEarnValue = value; }
        }

        /// <summary>
        /// 累积核算形象进度
        /// </summary>
        public virtual decimal SumCompletedPercent
        {
            get { return sumCompletedPercent; }
            set { sumCompletedPercent = value; }
        }
    }
}
