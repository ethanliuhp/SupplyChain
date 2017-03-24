using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;
using System.ComponentModel;

namespace Application.Business.Erp.SupplyChain.CostManagement.SubContractBalanceMng.Domain
{
    /// <summary>
    ///分包资源耗用结算（分包结算分科目明细）
    /// </summary>
    [Serializable]
    [Entity]
    public class SubContractBalanceSubjectDtl
    {
        private string _id;
        private long _version;
        private string _costName;
        private StandardUnit _priceUnit;
        private string _priceUnitName;
        private decimal _balancePrice;
        private decimal _balanceTotalPrice;
        private decimal _balanceQuantity;
        private string _frontBillGUID;
        private StandardUnit _quantityUnit;
        private string _quantityUnitName;
        private CostAccountSubject _balanceSubjectGUID;
        private string _balanceSubjectName;
        private string _balanceSubjectSyscode;
        private string _balanceSubjectCode;
        private string _resourceTypeGUID;
        private string _resourceTypeName;
        private string _resourceTypeStuff;
        private string _resourceTypeSpec;
        private string _resourceSyscode;
        private string _remark;
        private SubContractBalanceDetail _theBalanceDetail;


        private MonthBalanceSuccessFlag _monthBlanceFlag = MonthBalanceSuccessFlag.未结算;

        private string diagramNumber;

        /// <summary>
        /// 图号
        /// </summary>
        public virtual string DiagramNumber
        {
            get { return diagramNumber; }
            set { diagramNumber = value; }
        }

        /// <summary>
        /// 月度结算成功标志
        /// </summary>
        public virtual MonthBalanceSuccessFlag MonthBalanceFlag
        {
            get { return _monthBlanceFlag; }
            set { _monthBlanceFlag = value; }
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
        /// 成本名称
        /// </summary>
        public virtual string CostName
        {
            get { return _costName; }
            set { _costName = value; }
        }
        /// <summary>
        /// 价格计量单位GUID
        /// </summary>
        public virtual StandardUnit PriceUnit
        {
            get { return _priceUnit; }
            set { _priceUnit = value; }
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
        public virtual decimal BalanceQuantity
        {
            get { return _balanceQuantity; }
            set { _balanceQuantity = value; }
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
        /// 数量单位GUID
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
        /// 结算科目
        /// </summary>
        public virtual CostAccountSubject BalanceSubjectGUID
        {
            get { return _balanceSubjectGUID; }
            set { _balanceSubjectGUID = value; }
        }
        /// <summary>
        /// 结算科目编码
        /// </summary>
        public virtual string BalanceSubjectCode
        {
            get { return _balanceSubjectCode; }
            set { _balanceSubjectCode = value; }
        }
        /// <summary>
        /// 结算科目名称
        /// </summary>
        public virtual string BalanceSubjectName
        {
            get { return _balanceSubjectName; }
            set { _balanceSubjectName = value; }
        }
        /// <summary>
        /// 结算科目层次码
        /// </summary>
        public virtual string BalanceSubjectSyscode
        {
            get { return _balanceSubjectSyscode; }
            set { _balanceSubjectSyscode = value; }
        }
        /// <summary>
        /// 资源类型GUID
        /// </summary>
        public virtual string ResourceTypeGUID
        {
            get { return _resourceTypeGUID; }
            set { _resourceTypeGUID = value; }
        }
        /// <summary>
        /// 资源类型名称
        /// </summary>
        public virtual string ResourceTypeName
        {
            get { return _resourceTypeName; }
            set { _resourceTypeName = value; }
        }
        /// <summary>
        /// 资源类型材质
        /// </summary>
        public virtual string ResourceTypeStuff
        {
            get { return _resourceTypeStuff; }
            set { _resourceTypeStuff = value; }
        }
        /// <summary>
        /// 资源类型规格
        /// </summary>
        public virtual string ResourceTypeSpec
        {
            get { return _resourceTypeSpec; }
            set { _resourceTypeSpec = value; }
        }
        /// <summary>
        /// 资源类型层次码
        /// </summary>
        public virtual string ResourceSyscode
        {
            get { return _resourceSyscode; }
            set { _resourceSyscode = value; }
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
        /// 结算明细
        /// </summary>
        public virtual SubContractBalanceDetail TheBalanceDetail
        {
            get { return _theBalanceDetail; }
            set { _theBalanceDetail = value; }
        }
    }
    /// <summary>
    /// 月度结算成功标志
    /// </summary>
    public enum MonthBalanceSuccessFlag
    {
        [Description("未结算")]
        未结算 = 0,
        [Description("成功")]
        成功 = 1,
        [Description("结算失败")]
        结算失败 = 2
    }
}
