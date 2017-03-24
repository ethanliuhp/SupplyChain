using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using VirtualMachine.Component.Util;
using Application.Resource.PersonAndOrganization.OrganizationResource.RelateClass;
using Application.Resource.FinancialResource.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;
using Application.Resource.PersonAndOrganization.OrganizationResource.Domain;
using System.Collections;
using VirtualMachine.Patterns.BusinessEssence.Domain;

namespace Application.Business.Erp.SupplyChain.Base.Domain
{
    /// <summary>
    /// 业务单据主表公用属性
    /// </summary>
    [Serializable]
    [Entity]
    public class BaseMaster
    {
        private string auditRoles;
        private IList audits;
        private string classifyCode;
        private string code;
        private string id;
        private long version;
        private DocumentState docState;
        private PersonInfo createPerson;
        private PersonInfo auditPerson;
        private PersonInfo invalidatePerson;
        private PersonInfo handlePerson;     
        private int handOrgLevel;
        private OperationOrgInfo operOrgInfo;
        private DateTime createDate = StringUtil.StrToDateTime("1900-01-01");
        private DateTime auditDate = StringUtil.StrToDateTime("1900-01-01");
        private DateTime invalidateDate = StringUtil.StrToDateTime("1900-01-01");
        private DateTime submitDate = StringUtil.StrToDateTime("1900-01-01");
        private int createYear = 0;
        private int createMonth = 0;
        private int auditYear = 0;
        private int auditMonth = 0;
        private int invalidateYear = 0;
        private int invalidateMonth = 0;
        private DateTime realOperationDate = DateTime.Now;
        private DateTime lastModifyDate = DateTime.Now;
        private Currency currencyType;
        private decimal exchangeRate = 0;

        private string descript;

        private bool isSelect = false;
        private bool isFinished = false;
        private long _CheckState;
        private PersonInfo _JBR;
        private OperationOrg handleOrg;
        private string opgSysCode;

        private string projectId;
        private string projectName;
        private decimal sumQuantity;
        private decimal sumMoney;

        private string createPersonName;
        private string auditPersonName;
        private string handlePersonName;
        private string operOrgInfoName;

        private string forwardBillId;
        private string forwardBillCode;
        private string forwardBillType;

        private int printTimes;

        private string temp1;
        private string temp2;
        private string temp3;
        private string temp4;
        private string temp5;
        /// <summary>
        /// 临时储存
        /// </summary>
        virtual public string Temp1
        {
            get { return temp1; }
            set { temp1 = value; }
        }
        /// <summary>
        /// 临时储存
        /// </summary>
        virtual public string Temp2
        {
            get { return temp2; }
            set { temp2 = value; }
        }
        /// <summary>
        /// 临时储存
        /// </summary>
        virtual public string Temp3
        {
            get { return temp3; }
            set { temp3 = value; }
        }
        /// <summary>
        /// 临时储存
        /// </summary>
        virtual public string Temp4
        {
            get { return temp4; }
            set { temp4 = value; }
        }
        /// <summary>
        /// 临时储存
        /// </summary>
        virtual public string Temp5
        {
            get { return temp5; }
            set { temp5 = value; }
        }
        /// <summary>
        /// 业务组织名称
        /// </summary>
        public virtual string OperOrgInfoName
        {
            get { return operOrgInfoName; }
            set { operOrgInfoName = value; }
        }

        /// <summary>
        /// 经手人(责任人)名称
        /// </summary>
        public virtual string HandlePersonName
        {
            get { return handlePersonName; }
            set { handlePersonName = value; }
        }

        /// <summary>
        /// 审核人名称
        /// </summary>
        public virtual string AuditPersonName
        {
            get { return auditPersonName; }
            set { auditPersonName = value; }
        }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public virtual string CreatePersonName
        {
            get { return createPersonName; }
            set { createPersonName = value; }
        }

        /// <summary>
        /// 归属项目
        /// </summary>
        virtual public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        /// <summary>
        /// 归属项目名称
        /// </summary>
        virtual public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        private Iesi.Collections.Generic.ISet<BaseDetail> details = new Iesi.Collections.Generic.HashedSet<BaseDetail>();
        virtual public Iesi.Collections.Generic.ISet<BaseDetail> Details
        {
            get { return details; }
            set { details = value; }
        }

        /// <summary>
        /// 数量汇总
        /// </summary>
        virtual public decimal SumQuantity
        {
            get
            {
                decimal tmpQuantity = 0;
                //汇总
                foreach (BaseDetail var in Details)
                {
                    tmpQuantity += var.Quantity;
                }
                sumQuantity = tmpQuantity;
                return sumQuantity;
            }
            set { sumQuantity = value; }
        }

        /// <summary>
        /// 金额汇总
        /// </summary>
        virtual public decimal SumMoney
        {
            get
            {
                decimal temMoney = 0;
                //汇总
                foreach (BaseDetail var in Details)
                {
                    temMoney += var.Money;
                }
                sumMoney = temMoney;
                return sumMoney;
            }
            set
            {
                sumMoney = value;
            }
        }

        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// Code
        /// </summary>
        virtual public string Code
        {
            get { return code; }
            set { code = value; }
        }
        /// <summary>
        /// 审批角色
        /// </summary>
        virtual public string AuditRoles
        {
            get { return auditRoles; }
            set { auditRoles = value; }
        }
        /// <summary>
        /// 审批
        /// </summary>
        virtual public IList Audits
        {
            get { return audits; }
            set { audits = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
        /// <summary>
        /// 分类编码
        /// </summary>
        virtual public string ClassifyCode
        {
            get { return classifyCode; }
            set { classifyCode = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        virtual public DocumentState DocState
        {
            get { return docState; }
            set { docState = value; }
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
        /// 责任人所属组织层级(用于过滤兄弟部门数据权限)
        /// </summary>
        public virtual int HandOrgLevel
        {
            get { return handOrgLevel; }
            set { handOrgLevel = value; }
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

        /// <summary>
        /// 业务日期
        /// </summary>
        virtual public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        /// <summary>
        /// 提交日期
        /// </summary>
        virtual public DateTime SubmitDate
        {
            get { return submitDate; }
            set { submitDate = value; }
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
        /// 实际业务日期(制单时间)（服务器时间）
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
        virtual public void AddDetail(BaseDetail detail)
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
        /// 制单人组织层次码
        /// </summary>
        virtual public string OpgSysCode
        {
            get { return opgSysCode; }
            set { opgSysCode = value; }
        }

        /// <summary>
        /// 前驱单据ID
        /// </summary>
        virtual public string ForwardBillId
        {
            get { return forwardBillId; }
            set { forwardBillId = value; }
        }

        /// <summary>
        /// 前驱单据号
        /// </summary>
        virtual public string ForwardBillCode
        {
            get { return forwardBillCode; }
            set { forwardBillCode = value; }
        }

        /// <summary>
        /// 前驱单据类型
        /// </summary>
        virtual public string ForwardBillType
        {
            get { return forwardBillType; }
            set { forwardBillType = value; }
        }

        /// <summary>
        /// 打印次数
        /// </summary>
        virtual public int PrintTimes
        {
            get { return printTimes; }
            set { printTimes = value; }
        } 
    }
}
