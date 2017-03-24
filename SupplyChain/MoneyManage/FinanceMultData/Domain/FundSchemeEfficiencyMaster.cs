using System;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    [Serializable]
    [Entity]
    public class FundSchemeEfficiencyMaster : Base.Domain.BaseMaster
    {
        private string _lastModifyBy;
        /// <summary>最后修改人</summary>
        public virtual string LastModifyBy
        {
            get { return _lastModifyBy; }
            set { _lastModifyBy = value; }
        }
    }
}
