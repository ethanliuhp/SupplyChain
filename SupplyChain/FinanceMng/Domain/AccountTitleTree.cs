using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Patterns.CategoryTreePattern.Domain;
using VirtualMachine.Core.Attributes;
using System.ComponentModel;
 

namespace Application.Business.Erp.SupplyChain.FinanceMng.Domain
{
 
 /// <summary>
    /// 会计科目
    /// </summary>
    [Entity]
    [Serializable]
    public class AccountTitleTree : CategoryNode
    {
        private DateTime _createTime = DateTime.Now;
        private DateTime _updatedDate = DateTime.Now;
        private string _summary;
        private string _businessFlag;

        /// <summary>
        /// 业务标志
        /// </summary>
        public virtual string BusinessFlag
        {
            get { return _businessFlag; }
            set { _businessFlag = value; }
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
        /// 克隆对象
        /// </summary>
        /// <returns></returns>
        public virtual AccountTitleTree Clone()
        {
            AccountTitleTree cat = new AccountTitleTree();
            cat.Name = this.Name;
            cat.Code = this.Code;
            cat.OrderNo = this.OrderNo;
            cat.Describe = this.Describe;
            cat.Summary = this.Summary;

            return cat;
        }
    }
    /// <summary>
    /// 会计科目状态
    /// </summary>
    public enum AccountTitleTreeState
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
