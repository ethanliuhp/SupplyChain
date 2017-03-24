using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.ClientManagement.RelateClass;
using Iesi.Collections.Generic;
using System.Collections;
using VirtualMachine.Core.Attributes;

namespace Application.Business.Erp.SupplyChain.MoneyManage.FinanceMultData.Domain
{
    //收款单主表
    [Serializable]
    [Entity]
    public class GatheringMaster : BaseMaster
    {
        private string theCustomerRelationInfo;
        private string theCustomerName;
        private string theSupplierRelationInfo;
        private string theSupplierName;
        private string accountTitleID;
        private string accountTitleName;
        private string accountTitleCode;
        private string accountTitleSyscode;
        private string ownerQuantityMxID;
        private int ifProjectMoney;
        private Iesi.Collections.Generic.ISet<GatheringInvoice> listInvoice = new Iesi.Collections.Generic.HashedSet<GatheringInvoice>();
        private Iesi.Collections.Generic.ISet<GatheringAndQuantityRel> listRel = new Iesi.Collections.Generic.HashedSet<GatheringAndQuantityRel>();

        /// <summary>
        /// 会计科目
        /// </summary>
        virtual public string AccountTitleID
        {
            get { return accountTitleID; }
            set { accountTitleID = value; }
        }

        /// <summary>
        /// 会计科目名称
        /// </summary>
        virtual public string AccountTitleName
        {
            get { return accountTitleName; }
            set { accountTitleName = value; }
        }

        /// <summary>
        /// 会计科目编码
        /// </summary>
        virtual public string AccountTitleCode
        {
            get { return accountTitleCode; }
            set { accountTitleCode = value; }
        }
        /// <summary>
        /// 会计科目层次码
        /// </summary>
        virtual public string AccountTitleSyscode
        {
            get { return accountTitleSyscode; }
            set { accountTitleSyscode = value; }
        }

        /// <summary>
        /// 客户
        /// </summary>
        virtual public string TheCustomerRelationInfo
        {
            get { return theCustomerRelationInfo; }
            set { theCustomerRelationInfo = value; }
        }

        /// <summary>
        /// 客户名称
        /// </summary>
        virtual public string TheCustomerName
        {
            get { return theCustomerName; }
            set { theCustomerName = value; }
        }
        /// <summary>
        /// 供应商
        /// </summary>
        public virtual string TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public virtual string TheSupplierName
        {
            get { return theSupplierName; }
            set { theSupplierName = value; }
        }

        /// <summary>
        /// 业主报量明细ID
        /// </summary>
        virtual public string OwnerQuantityMxID
        {
            get { return ownerQuantityMxID; }
            set { ownerQuantityMxID = value; }
        }
        /// <summary>
        /// 收款发票
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<GatheringInvoice> ListInvoice
        {
            get { return listInvoice; }
            set { listInvoice = value; }
        }
        /// <summary>
        /// 收款报量关系
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<GatheringAndQuantityRel> ListRel
        {
            get { return listRel; }
            set { listRel = value; }
        }
        /// <summary>
        /// 是否工程款
        /// </summary>
        virtual public int IfProjectMoney
        {
            get { return ifProjectMoney; }
            set { ifProjectMoney = value; }
        }
    }
}
