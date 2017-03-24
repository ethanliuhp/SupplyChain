using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.InitStruct
{
    /// <summary>
    /// 科目辅助核算查询条件
    /// </summary>
    [Serializable]
    public struct AccAssistInfoSrt
    {
        /// <summary>
        /// 多条件间且关系
        /// </summary>
        public bool AndRelation;
        /// <summary>
        /// 外币核算
        /// </summary>
        public bool ForeignAccount;
        /// <summary>
        /// 数量核算
        /// </summary>
        public bool QuantityAccount;
        /// <summary>
        /// 日记账
        /// </summary>
        public bool DailyAccBook;
        /// <summary>
        /// 银行账
        /// </summary>
        public bool BankAccBook;
        /// <summary>
        /// 部门核算
        /// </summary>
        public bool DepartmentAccount;
        /// <summary>
        /// 个人核算
        /// </summary>
        public bool PersonAccount;        
        /// <summary>
        /// 客户关系核算
        /// </summary>
        public bool ClientAccount;        
        /// <summary>
        /// 供应关系核算
        /// </summary>
        public bool SupplierAccount;
        /// <summary>
        /// 项目核算
        /// </summary>
        public bool ProjectAccount;
        /// <summary>
        /// 背书人管理
        /// </summary>
        public bool EndorsementManage;
        /// <summary>
        /// 预算管理
        /// </summary>
        public bool BudgetManage;
        /// <summary>
        /// 现金日记账
        /// </summary>
        public bool CashDailyAcc;
        /// <summary>
        /// 银行存款日记账
        /// </summary>
        public bool BankDailyAcc;
        /// <summary>
        /// 资金科目
        /// </summary>
        public bool CapitalAcc;
        /// <summary>
        /// 台账
        /// </summary>
        public bool DeskAcc;
    }
}
