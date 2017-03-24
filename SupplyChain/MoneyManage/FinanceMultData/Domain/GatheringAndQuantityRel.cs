using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    //收款和报量关系表
    [Serializable]
    [Entity]
    public class GatheringAndQuantityRel
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual string Id { get; set; }
        /// <summary>
        /// 收款主表
        /// </summary>
        public virtual GatheringMaster GatheringID { get; set; }
        /// <summary>
        /// 报量主表
        /// </summary>
        public virtual OwnerQuantityDetail OwnerQuantityMxID { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public virtual decimal GatheringMoney { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Descript { get; set; }
    }
}
