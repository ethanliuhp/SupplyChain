using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Patterns.BusinessEssence.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.FinancialResource.Domain;
using System.Collections;
using VirtualMachine.Component.Util;
using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;

namespace Application.Business.Erp.SupplyChain.Base.Domain
{
    /// <summary>
    /// 业务单据主表公用属性
    /// </summary>
    [Serializable]
    [Entity]
    public abstract class BaseBillMaster : BusinessEntity
    {
        private PersonInfo createPerson;
        private PersonInfo auditPerson;
        private PersonInfo invalidatePerson;
        private PersonInfo handlePerson;
        private OperationOrgInfo operOrgInfo;
        private DateTime createDate = StringUtil.StrToDateTime("1900-01-01");
        private DateTime auditDate = StringUtil.StrToDateTime("1900-01-01");
        private DateTime invalidateDate = StringUtil.StrToDateTime("1900-01-01");
        private int createYear = 0;
        private int createMonth = 0;
        private int auditYear = 0;
        private int auditMonth = 0;
        private int invalidateYear = 0;
        private int invalidateMonth = 0;
        private DateTime realOperationDate = StringUtil.StrToDateTime("1900-01-01");
        private DateTime lastModifyDate = StringUtil.StrToDateTime("1900-01-01");
        private Currency currencyType;
        private decimal exchangeRate = 0;

        private string descript;

        private bool isSelect = false;
        private bool isFinished = false;
        private long _CheckState;
        private PersonInfo _JBR;
        private OperationOrg handleOrg;
        private string opgSysCode;

        private Iesi.Collections.Generic.ISet<BaseBillDetail> details = new Iesi.Collections.Generic.HashedSet<BaseBillDetail>();
        virtual public Iesi.Collections.Generic.ISet<BaseBillDetail> Details
        {
            get { return details; }
            set { details = value; }
        }

        /// <summary>
        /// 经办人
        /// </summary>
        virtual public PersonInfo JBR
        {
            get { return _JBR; }
            set { _JBR = value; }
        }


        /// <summary>
        /// 是否完成
        /// </summary>
        virtual public bool IsFinished
        {
            get { return isFinished; }
            set { isFinished = value; }
        }
        /// <summary>
        /// 是否选择,(不作Map)
        /// </summary>
        virtual public bool IsSelect
        {
            get { return isSelect; }
            set { isSelect = value; }
        }
        /// <summary>
        /// 制单人
        /// </summary>
        virtual public PersonInfo CreatePerson
        {
            get { return createPerson; }
            set { createPerson = value; }
        }

        /// <summary>
        /// 审核人
        /// </summary>
        public virtual PersonInfo AuditPerson
        {
            get { return auditPerson; }
            set { auditPerson = value; }
        }

        /// <summary>
        /// 单据失效人
        /// </summary>
        virtual public PersonInfo InvalidatePerson
        {
            get { return invalidatePerson; }
            set { invalidatePerson = value; }
        }

        /// <summary>
        /// 业务经手人（采购员，销售员）
        /// </summary>
        virtual public PersonInfo HandlePerson
        {
            get { return handlePerson; }
            set { handlePerson = value; }
        }

        /// <summary>
        /// 业务组织
        /// </summary>
        virtual public OperationOrgInfo OperOrgInfo
        {
            get { return operOrgInfo; }
            set { operOrgInfo = value; }
        }

        /// <summary>
        /// 制单日期
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// 审核日期
        /// </summary>
        public virtual DateTime AuditDate
        {
            get { return auditDate; }
            set { auditDate = value; }
        }

        /// <summary>
        /// 失效日期
        /// </summary>
        virtual public DateTime InvalidateDate
        {
            get { return invalidateDate; }
            set { invalidateDate = value; }
        }

        /// <summary>
        /// 制单年
        /// </summary>
        virtual public int CreateYear
        {
            get { return createYear; }
            set { createYear = value; }
        }

        /// <summary>
        /// 制单月
        /// </summary>
        virtual public int CreateMonth
        {
            get { return createMonth; }
            set { createMonth = value; }
        }

        /// <summary>
        /// 审核年
        /// </summary>       
        virtual public int AuditYear
        {
            get { return auditYear; }
            set { auditYear = value; }
        }

        /// <summary>
        /// 审核月
        /// </summary>
        virtual public int AuditMonth
        {
            get { return auditMonth; }
            set { auditMonth = value; }
        }

        /// <summary>
        /// 失效年
        /// </summary>
        virtual public int InvalidateYear
        {
            get { return invalidateYear; }
            set { invalidateYear = value; }
        }

        /// <summary>
        /// 失效月
        /// </summary>
        virtual public int InvalidateMonth
        {
            get { return invalidateMonth; }
            set { invalidateMonth = value; }
        }

        /// <summary>
        /// 实际业务日期
        /// </summary>
        virtual public DateTime RealOperationDate
        {
            get { return realOperationDate; }
            set { realOperationDate = value; }
        }
        /// <summary>
        /// 单据最后的修改日期（服务器时间）
        /// </summary>
        virtual public DateTime LastModifyDate
        {
            get { return lastModifyDate; }
            set { lastModifyDate = value; }
        }


        /// <summary>
        /// 币种
        /// </summary>
        virtual public Currency CurrencyType
        {
            get { return currencyType; }
            set { currencyType = value; }
        }

        /// <summary>
        /// 当时汇率
        /// </summary>
        virtual public decimal ExchangeRate
        {
            get { return exchangeRate; }
            set { exchangeRate = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 增加明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddDetail(BaseBillDetail detail)
        {
            detail.Master = this;
            Details.Add(detail);
        }
        /// <summary>
        ///  业务员归属业务组织
        /// </summary>
        virtual public OperationOrg HandleOrg
        {
            get { return handleOrg; }
            set { handleOrg = value; }
        }
        /// <summary>
        /// 业务组织树结构节点
        /// </summary>
        virtual public string OpgSysCode
        {
            get { return opgSysCode; }
            set { opgSysCode = value; }
        }
    }
}
