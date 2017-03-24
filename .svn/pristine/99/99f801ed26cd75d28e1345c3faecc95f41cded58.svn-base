using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Iesi.Collections.Generic;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 月度核算单
    /// </summary>
    [Serializable]
    public class CostMonthAccountBill
    {
        private string _id;
        private long _version;
        private string _theProjectGUID;
        private string _theProjectName;
        private int kjn;
        private int kjy;
        private DateTime _createTime = DateTime.Now;
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
        private string _remark;
        ISet<CostMonthAccountDtl> _Details = new HashedSet<CostMonthAccountDtl>();

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
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
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

    }
}
