using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 月度核算单
    /// </summary>
    [Serializable]
    public class CostMonthAccountBill : BaseMaster
    {
        private int kjn;
        private int kjy;
        private PersonInfo _accountPersonGUID;
        private string _accountPersonName;
        private string _accountPersonOrgSysCode;
        private string _theOrgName;
        private GWBSTree _accountRange;
        private string _accountTaskName;
        private string _accountTaskSysCode;
        private DateTime _beginTime;
        private DateTime _endTime;
        private decimal _exchangeRate;
        private string _accountOrgGUID;
        private string _accountOrgName;
        private string _remark;
        ISet<CostMonthAccountDtl> _Details = new HashedSet<CostMonthAccountDtl>();

        /// <summary>
        /// 会计年
        /// </summary>
        public virtual int Kjn
        {
            get { return kjn; }
            set { kjn = value; }
        }
        /// <summary>
        /// 会计月
        /// </summary>
        public virtual int Kjy
        {
            get { return kjy; }
            set { kjy = value; }
        }
        
        
        /// <summary>
        /// 核算人GUID
        /// </summary>
        public virtual PersonInfo AccountPersonGUID
        {
            get { return _accountPersonGUID; }
            set { _accountPersonGUID = value; }
        }
        
        /// <summary>
        /// 核算人名称
        /// </summary>
        public virtual string AccountPersonName
        {
            get { return _accountPersonName; }
            set { _accountPersonName = value; }
        }
        
        /// <summary>
        /// 核算人组织层次码
        /// </summary>
        public virtual string AccountPersonOrgSysCode
        {
            get { return _accountPersonOrgSysCode; }
            set { _accountPersonOrgSysCode = value; }
        }
        
        /// <summary>
        /// 组织名称
        /// </summary>
        public virtual string TheOrgName
        {
            get { return _theOrgName; }
            set { _theOrgName = value; }
        }
        
        /// <summary>
        /// 核算范围,核算的GWBS树节点,此处存GUID
        /// </summary>
        public virtual GWBSTree AccountRange
        {
            get { return _accountRange; }
            set { _accountRange = value; }
        }
        
        /// <summary>
        /// 核算的GWBS树节点名称
        /// </summary>
        public virtual string AccountTaskName
        {
            get { return _accountTaskName; }
            set { _accountTaskName = value; }
        }

        /// <summary>
        /// 核算的GWBS树节点层次码
        /// </summary>
        public virtual string AccountTaskSysCode
        {
            get { return _accountTaskSysCode; }
            set { _accountTaskSysCode = value; }
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
        /// 汇率
        /// </summary>
        public virtual decimal ExchangeRate
        {
            get { return _exchangeRate; }
            set { _exchangeRate = value; }
        }

        /// <summary>
        /// 核算组织GUID
        /// </summary>
        public virtual string AccountOrgGUID
        {
            get { return _accountOrgGUID; }
            set { _accountOrgGUID = value; }
        }

        /// <summary>
        /// 核算组织名称
        /// </summary>
        public virtual string AccountOrgName
        {
            get { return _accountOrgName; }
            set { _accountOrgName = value; }
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
        /// 月度工程任务明细核算
        /// </summary>
        public virtual ISet<CostMonthAccountDtl> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }

        private decimal _tempCalculateArea;
        ///<summary>累计建筑面积</summary>
        public virtual decimal TempCalculateArea
        {
            set { this._tempCalculateArea = value; }
            get { return this._tempCalculateArea; }
        }

        private decimal _tempProjectArea;
        ///<summary>项目建筑总面积</summary>
        public virtual decimal TempProjectArea
        {
            set { this._tempProjectArea = value; }
            get { return this._tempProjectArea; }
        }

        private decimal _tempVatRate;
        ///<summary>增值税率</summary>
        public virtual decimal TempVatRate
        {
            set { this._tempVatRate = value; }
            get { return this._tempVatRate; }
        }

        private decimal _tempMeasuresFeeRatio;
        ///<summary>措施费责任成本测定比值</summary>
        public virtual decimal TempMeasuresFeeRatio
        {
            set { this._tempMeasuresFeeRatio = value; }
            get { return this._tempMeasuresFeeRatio; }
        }

        private decimal _tempFeesRatio;
        ///<summary>规费责任成本测定比值</summary>
        public virtual decimal TempFeesRatio
        {
            set { this._tempFeesRatio = value; }
            get { return this._tempFeesRatio; }
        }

        private decimal _tempManagementFeeRatio;
        ///<summary>管理费责任成本测定比值</summary>
        public virtual decimal TempManagementFeeRatio
        {
            set { this._tempManagementFeeRatio = value; }
            get { return this._tempManagementFeeRatio; }
        }
       

    }
}
