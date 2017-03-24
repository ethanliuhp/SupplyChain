using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.CostMonthAccountMng.Domain
{
    /// <summary>
    /// 调差方案主表
    /// </summary>
    public class CostMonthReportChangeDetail : BaseDetail
    {
        private string _CostSubjectCode;
        private string _ResourceTypeGUID;
        private string _ResourceTypeName;
        private string _ResourceTypeSpec;
        private string _RationUnitName;
        private decimal _ChangeQty;
        private decimal _ChangeBudgetMoney;
        private string _ChangeRemark;
        /// <summary>核算科目编码 </summary>
        public virtual string CostSubjectCode
        {
            get { return _CostSubjectCode; }
            set { _CostSubjectCode = value; }
        }
        /// <summary>资源ID </summary>
        public virtual string  ResourceTypeGUID
        {
            get { return _ResourceTypeGUID; }
            set { _ResourceTypeGUID = value; }
        }

        /// <summary>资源名称 </summary>
        public virtual string ResourceTypeName
        {
            get { return _ResourceTypeName; }
            set { _ResourceTypeName = value; }
        }
        /// <summary>资源规格 </summary>
        public virtual string ResourceTypeSpec
        {
            get { return _ResourceTypeSpec; }
            set { _ResourceTypeSpec = value; }
        }
        /// <summary>资源单位</summary>
        public virtual string RationUnitName
        {
            get { return _RationUnitName; }
            set { _RationUnitName = value; }
        }
        /// <summary>调整数量 </summary>
        public virtual decimal ChangeQty
        {
            get { return _ChangeQty; }
            set { _ChangeQty = value; }
        }
        /// <summary>调整预估金额 </summary>
        public virtual decimal ChangeBudgetMoney
        {
            get { return _ChangeBudgetMoney; }
            set { _ChangeBudgetMoney = value; }
        }
        /// <summary>调差备注信息 </summary>
        public virtual string ChangeRemark
        {
            get { return _ChangeRemark; }
            set { _ChangeRemark = value; }
        }

        private ChangeSolutionType _changeType;
        /// <summary>调差方案 </summary>
        public virtual ChangeSolutionType ChangeType
        {
            get { return _changeType; }
            set { _changeType = value; }
        }
       

    }
    public enum ChangeSolutionType
    {
        ChangeDiff = 1,
        Estimate = 2
    }
}
