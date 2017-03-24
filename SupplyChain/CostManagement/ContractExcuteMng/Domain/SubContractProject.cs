using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using System.ComponentModel;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Iesi.Collections.Generic;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain
{
    /// <summary>
    /// 分包项目
    /// </summary>
    [Serializable]
    public class SubContractProject : BaseMaster
    {
        //private string id;
        //private long version;
        //private string code;
        //private string _projectId;
        //private string _projectName;
        private SupplierRelationInfo _bearerOrg;
        private string _bearerOrgName;
        private PersonInfo _owner;
        private string _ownerName;
        private string _ownerOrgSysCode;
        //private DateTime _createTime = DateTime.Now;
        private SubContractType _contractType;
        private ContractGroup _theContractGroup;
        private string contractGroupCode;
        private decimal _contractInterimMoney = 0;
        private decimal contractSumMoney = 0;
        private decimal _addupBalanceMoney = 0;
        private decimal _addupWaitApproveBalMoney = 0;
        private decimal _allowExceedPercent = 0;
        private UtilitiesRememberMethod _UtilitiesRemMethod;
        private decimal _utilitiesRate = 0;
        private ManagementRememberMethod _managementRemMethod;
        private decimal _managementRate = 0;
        private ISet<SubContractChangeItem> _Details = new HashedSet<SubContractChangeItem>();
        private ISet<ProfessionalSubcontractPriceItem> _ProfessDetails = new HashedSet<ProfessionalSubcontractPriceItem>();
        private ISet<LaborSubContractPriceItem> _LaborDetails = new HashedSet<LaborSubContractPriceItem>();
        private string balanceStyle;
        private string subPackage;
        private decimal processPayRate;
        private decimal completePayRate;
        private decimal warrantyPayRate;

        /// <summary>
        /// 过程结算付款比例
        /// </summary>
        public virtual decimal ProcessPayRate
        {
            get { return processPayRate; }
            set { processPayRate = value; }
        }

        /// <summary>
        /// 完工结算付款比例
        /// </summary>
        public virtual decimal CompletePayRate
        {
            get { return completePayRate; }
            set { completePayRate = value; }
        }

        /// <summary>
        /// 质保期付款比例
        /// </summary>
        public virtual decimal WarrantyPayRate
        {
            get { return warrantyPayRate; }
            set { warrantyPayRate = value; }
        }

        //分包内容
        virtual public string SubPackage
        {
          get { return subPackage; }
          set { subPackage = value; }
        }

        //结算完成情况
        virtual public string BalanceStyle
        {
            get { return balanceStyle; }
            set { balanceStyle = value; }
        }
        //private DateTime lastModifyDate;
        //private PersonInfo createPerson;
        //private string createPersonName;
        //private DocumentState docState;

        private ManagementRememberMethod laborMoneyType;
        /// <summary>
        /// 分包劳务税金计取方式
        /// </summary>
        virtual public ManagementRememberMethod LaborMoneyType
        {
            get { return laborMoneyType; }
            set { laborMoneyType = value; }
        }
        private decimal laobrRace;
        /// <summary>
        /// 分包劳务税金费率
        /// </summary>
        virtual public decimal LaobrRace
        {
            get { return laobrRace; }
            set { laobrRace = value; }
        }

        /// <summary>
        /// 契约组编号
        /// </summary>
        virtual public string ContractGroupCode
        {
            get { return contractGroupCode; }
            set { contractGroupCode = value; }
        }
        ///// <summary>
        ///// 状态
        ///// </summary>
        //virtual public DocumentState DocState
        //{
        //    get { return docState; }
        //    set { docState = value; }
        //}

        ///// <summary>
        ///// 制单人
        ///// </summary>
        ///// <param name="detail"></param>
        //virtual public PersonInfo CreatePerson
        //{
        //    get { return createPerson; }
        //    set { createPerson = value; }
        //}
        ///// <summary>
        ///// 制单人名称
        ///// </summary>
        ///// <param name="detail"></param>
        //virtual public string CreatePersonName
        //{
        //    get { return createPersonName; }
        //    set { createPersonName = value; }
        //}
        ///// <summary>
        ///// 增加明细
        ///// </summary>
        ///// <param name="detail"></param>
        //virtual public DateTime LastModifyDate
        //{
        //    get { return lastModifyDate; }
        //    set { lastModifyDate = value; }
        //}

        /// <summary>
        /// 增加明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddDetail(SubContractChangeItem detail)
        {
            detail.TheProject = this;
            Details.Add(detail);
        }

        ///// <summary>
        ///// ID
        ///// </summary>
        //virtual public string Id
        //{
        //    get { return id; }
        //    set { id = value; }
        //}
        ///// <summary>
        ///// 版本
        ///// </summary>
        //virtual public long Version
        //{
        //    get { return version; }
        //    set { version = value; }
        //}
        ///// <summary>
        ///// 编号
        ///// </summary>
        //virtual public string Code
        //{
        //    get { return code; }
        //    set { code = value; }
        //}
        ///// <summary>
        ///// 所属项目ID
        ///// </summary>
        //public virtual string ProjectId
        //{
        //    get { return _projectId; }
        //    set { _projectId = value; }
        //}
        ///// <summary>
        ///// 所属项目名称
        ///// </summary>
        //public virtual string ProjectName
        //{
        //    get { return _projectName; }
        //    set { _projectName = value; }
        //}
        /// <summary>
        /// 承担组织
        /// </summary>
        public virtual  SupplierRelationInfo BearerOrg
        {
            get { return _bearerOrg; }
            set { _bearerOrg = value; }
        }
        /// <summary>
        ///承担组织名称 
        /// </summary>
        public virtual  string BearerOrgName
        {
            get { return _bearerOrgName; }
            set { _bearerOrgName = value; }
        }
        /// <summary>
        /// 责任人
        /// </summary>
        public virtual  PersonInfo Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        /// <summary>
        /// 责任人名称
        /// </summary>
        public virtual  string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }
        /// <summary>
        /// 责任人组织层次码
        /// </summary>
        public virtual  string OwnerOrgSysCode
        {
            get { return _ownerOrgSysCode; }
            set { _ownerOrgSysCode = value; }
        }
        ///// <summary>
        ///// 创建时间
        ///// </summary>
        //public virtual  DateTime CreateTime
        //{
        //    get { return _createTime; }
        //    set { _createTime = value; }
        //}
        /// <summary>
        /// 分包合同类型
        /// </summary>
        public virtual  SubContractType ContractType
        {
            get { return _contractType; }
            set { _contractType = value; }
        }
        /// <summary>
        /// 分包合同契约
        /// </summary>
        public virtual  ContractGroup TheContractGroup
        {
            get { return _theContractGroup; }
            set { _theContractGroup = value; }
        }
        /// <summary>
        /// 合同总金额
        /// </summary>
        public virtual decimal ContractSumMoney
        {
            get { return contractSumMoney; }
            set { contractSumMoney = value; }
        }
        /// <summary>
        /// 合同暂定金额
        /// </summary>
        public virtual  decimal ContractInterimMoney
        {
            get { return _contractInterimMoney; }
            set { _contractInterimMoney = value; }
        }
        /// <summary>
        /// 累计已结算金额
        /// </summary>
        public virtual  decimal AddupBalanceMoney
        {
            get { return _addupBalanceMoney; }
            set { _addupBalanceMoney = value; }
        }
        /// <summary>
        /// 累计待审批结算金额
        /// </summary>
        public virtual  decimal AddupWaitApproveBalMoney
        {
            get { return _addupWaitApproveBalMoney; }
            set { _addupWaitApproveBalMoney = value; }
        }
        /// <summary>
        /// 允许超结百分比
        /// </summary>
        public virtual  decimal AllowExceedPercent
        {
            get { return _allowExceedPercent; }
            set { _allowExceedPercent = value; }
        }
        /// <summary>
        /// 代缴水电费计取方式
        /// </summary>
        public virtual  UtilitiesRememberMethod UtilitiesRemMethod
        {
            get { return _UtilitiesRemMethod; }
            set { _UtilitiesRemMethod = value; }
        }
        /// <summary>
        /// 代缴水电费率
        /// </summary>
        public virtual  decimal UtilitiesRate
        {
            get { return _utilitiesRate; }
            set { _utilitiesRate = value; }
        }
        /// <summary>
        /// 建设管理费计取方式
        /// </summary>
        public virtual  ManagementRememberMethod ManagementRemMethod
        {
            get { return _managementRemMethod; }
            set { _managementRemMethod = value; }
        }
        /// <summary>
        /// 建设管理费率
        /// </summary>
        public virtual  decimal ManagementRate
        {
            get { return _managementRate; }
            set { _managementRate = value; }
        }


        /// <summary>
        /// 分包合同变更项集合
        /// </summary>
        public virtual  ISet<SubContractChangeItem> Details
        {
            get { return _Details; }
            set { _Details = value; }
        }
        /// <summary>
        /// 专业分包价格明细
        /// </summary>
        public virtual ISet<ProfessionalSubcontractPriceItem> ProfessDetails
        {
            get { return _ProfessDetails; }
            set { _ProfessDetails = value; }
        }
        /// <summary>
        /// 劳务分包价格明细
        /// </summary>
        public virtual ISet<LaborSubContractPriceItem> LaborDetails
        {
            get { return _LaborDetails; }
            set { _LaborDetails = value; }
        }
        /// <summary>
        /// 增加专业明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddProDetail(ProfessionalSubcontractPriceItem detail)
        {
            detail.TheProject = this;
            ProfessDetails.Add(detail);
        }
        /// <summary>
        /// 增加劳务明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddLaborDetail(LaborSubContractPriceItem detail)
        {
            detail.TheProject = this;
            LaborDetails.Add(detail);
        }
    }

    /// <summary>
    /// 分包合同类型
    /// </summary>
    public enum SubContractType
    {
        [Description("劳务分包")]
        劳务分包 = 0,
        [Description("专业分包")]
        专业分包 = 1,
        [Description("甲指分包")]
        甲指分包 = 2,
        [Description("固定总价")]
        固定总价 = 3,
        [Description("总承包")]
        总承包 = 4,
        [Description("计时工合同")]
        计时工合同 = 5
    }

    /// <summary>
    /// 代缴水电费计取方式
    /// </summary>
    public enum UtilitiesRememberMethod
    {
        [Description("不计取")]
        不计取 = 0,
        [Description("按费率计取")]
        按费率计取 = 1,
        [Description("据实计取")]
        据实计取 = 2
    }
    /// <summary>
    /// 建设管理费计取方式
    /// </summary>
    public enum ManagementRememberMethod
    {
        [Description("不计取")]
        不计取 = 0,
        [Description("按费率计取")]
        按费率计取 = 1,
        [Description("据实计取")]
        据实计取 = 2
    }
}
