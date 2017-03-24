using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;
using Application.Resource.PersonAndOrganization.HumanResource.RelateClass;

namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Domain
{
    /// <summary>
    /// 库存关系锁
    /// </summary>
    [Serializable]
    [Entity]
    public class StockRelationLock
    {
        private string id;
        private StockRelation theStockRelation;
        private decimal quantity;
        private PersonInfo lockPerson;
        private DateTime lockDate;
        private int state;
        private string lockType;
        private string businessMxId;//业务单据ID
        private string businessCode;
        private string descript;

        /// <summary>
        /// 序号
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 库存关系
        /// </summary>
        virtual public StockRelation TheStockRelation
        {
            get { return theStockRelation; }
            set { theStockRelation = value; }
        }

        /// <summary>
        /// 锁定人
        /// </summary>
        virtual public PersonInfo LockPerson
        {
            get { return lockPerson; }
            set { lockPerson = value; }
        }

        /// <summary>
        /// 锁定日期
        /// </summary>
        public virtual DateTime LockDate
        {
            get { return lockDate; }
            set { lockDate = value; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        virtual public int State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 数量
        /// </summary>
        virtual public decimal Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        /// <summary>
        /// 锁定类型
        /// </summary>
        virtual public string LockType
        {
            get { return lockType; }
            set { lockType = value; }
        }

        /// <summary>
        /// 业务单据明细ID
        /// </summary>
        virtual public string BusinessMxId
        {
            get { return businessMxId; }
            set { businessMxId = value; }
        }

        /// <summary>
        /// 业务单据号
        /// </summary>
        virtual public string BusinessCode
        {
            get { return businessCode; }
            set { businessCode = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        virtual public string Descript
        {
            get { return descript; }
            set { descript = value; }
        }
    }
}
