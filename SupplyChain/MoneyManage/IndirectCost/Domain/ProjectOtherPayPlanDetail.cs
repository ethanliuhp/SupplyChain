using System;
using VirtualMachine.Core.Attributes;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.MoneyManage.IndirectCost.Domain
{
    [Serializable]
    [Entity]
    public class ProjectOtherPayPlanDetail : BaseDetail
    {
        private string _costDetail;

        /// <summary>费用明细</summary>
        public virtual string CostDetail
        {
            get { return _costDetail; }
            set { _costDetail = value; }
        }

        private string _parentId;

        /// <summary>
        /// 父级id
        /// </summary>
        public virtual string ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        private decimal _quota;

        /// <summary>分配额</summary>
        public virtual decimal Quota
        {
            get { return _quota; }
            set { _quota = value; }
        }

        private decimal _planDeclarePayment;

        /// <summary>计划申报支出</summary>
        public virtual decimal PlanDeclarePayment
        {
            get { return _planDeclarePayment; }
            set { _planDeclarePayment = value; }
        }

        private int _orderNumber;

        /// <summary>序号</summary>
        public virtual int OrderNumber
        {
            get { return _orderNumber; }
            set { _orderNumber = value; }
        }

        private string _payScope;

        /// <summary>支出范围</summary>
        public virtual string PayScope
        {
            get { return _payScope; }
            set { _payScope = value; }
        }
    }
}
