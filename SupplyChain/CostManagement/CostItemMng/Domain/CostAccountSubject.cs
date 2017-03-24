using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using System.ComponentModel;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain
{
    /// <summary>
    /// 成本核算科目
    /// </summary>
    [Entity]
    [Serializable]
    public class CostAccountSubject : CategoryNode
    {
        private DateTime _createTime = DateTime.Now;
        private DateTime _updatedDate = DateTime.Now;
        private string _summary;
        private string _ownerGUID;
        private string _ownerName;
        private string _ownerOrgSysCode;
        private string _accountingSubjectGUID;
        private string _accountingSubjectName;
        private string _useDesc;
        private CostAccountSubjectState _subjectState;
        private string _theProjectGUID;
        private string _theProjectName;
        private int _ifSubBalanceFlag;


        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public virtual DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; }
        }
        /// <summary>
        /// 摘要
        /// </summary>
        public virtual string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        /// <summary>
        /// 责任人GUID
        /// </summary>
        public virtual string OwnerGUID
        {
            get { return _ownerGUID; }
            set { _ownerGUID = value; }
        }

        /// <summary>
        /// 责任人名称
        /// </summary>
        public virtual string OwnerName
        {
            get { return _ownerName; }
            set { _ownerName = value; }
        }

        /// <summary>
        /// 责任人隶属层次码
        /// </summary>
        public virtual string OwnerOrgSysCode
        {
            get { return _ownerOrgSysCode; }
            set { _ownerOrgSysCode = value; }
        }

        /// <summary>
        /// 会计核算科目GUID
        /// </summary>
        public virtual string AccountingSubjectGUID
        {
            get { return _accountingSubjectGUID; }
            set { _accountingSubjectGUID = value; }
        }

        /// <summary>
        /// 会计核算科目名称
        /// </summary>
        public virtual string AccountingSubjectName
        {
            get { return _accountingSubjectName; }
            set { _accountingSubjectName = value; }
        }

        /// <summary>
        /// 使用说明
        /// </summary>
        public virtual string UseDesc
        {
            get { return _useDesc; }
            set { _useDesc = value; }
        }

        /// <summary>
        /// 成本核算科目状态
        /// </summary>
        public virtual CostAccountSubjectState SubjectState
        {
            get { return _subjectState; }
            set { _subjectState = value; }
        }

        /// <summary>
        /// 是否分包结算标志(1.必须纳入结算,2.不需纳入结算,3.由工长确定,4.由预算员确定,5.由合同确定)
        /// </summary>
        public virtual int IfSubBalanceFlag
        {
            get { return _ifSubBalanceFlag; }
            set { _ifSubBalanceFlag = value; }
        }

        /// <summary>
        /// 所属项目
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
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        public virtual CostAccountSubject Clone()
        {
            CostAccountSubject cat = new CostAccountSubject();
            cat.Name = this.Name;
            cat.Code = this.Code;
            cat.OrderNo = this.OrderNo;

            cat.Describe = this.Describe;
            cat.Summary = this.Summary;

            cat.TheProjectGUID = this.TheProjectGUID;
            cat.TheProjectName = this.TheProjectName;
            return cat;
        }
    }
    /// <summary>
    /// 成本核算科目状态
    /// </summary>
    public enum CostAccountSubjectState
    {
        [Description("制定")]
        制定 = 1,
        [Description("发布")]
        发布 = 2,
        [Description("冻结")]
        冻结 = 3,
        [Description("作废")]
        作废 = 4
    }
}
