﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireOrder.Domain
{
    /// <summary>
    /// 料具租赁合同结算规则
    /// </summary>
    public enum EnumMaterialHireMngBalRule
    {
        算头不算尾,
        算尾不算头,
        两头都不算,
        两头都算
    }

    /// <summary>
    /// 料具租赁合同明细
    /// </summary>
    [Serializable]
    public class MatHireOrderDetail : BaseDetail
    {
        private int ruleState;
        /// <summary>
        /// 规则状态 0:未设置 1:已设置
        /// </summary>
        virtual public int RuleState
        {
            get { return ruleState; }
            set { ruleState = value; }
        }

        private Iesi.Collections.Generic.ISet<OrderDetailCostSetItem> basicDtlCostSets = new Iesi.Collections.Generic.HashedSet<OrderDetailCostSetItem>();
        virtual public Iesi.Collections.Generic.ISet<OrderDetailCostSetItem> BasicDtlCostSets
        {
            get { return basicDtlCostSets; }
            set { basicDtlCostSets = value; }
        }

        /// <summary>
        /// 增加费用明细设置
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddBasicDtlCostSet(OrderDetailCostSetItem detail)
        {
            detail.Master = this;
            BasicDtlCostSets.Add(detail);
        }
    }
}