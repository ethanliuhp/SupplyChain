using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain
{
    /// <summary>
    /// 料具租赁合同主表
    /// </summary>
    [Serializable]
    public class MaterialRentalOrderMaster : BaseMaster
    {
        private string attachFilePath;
        private string originalContractNo;
        private string receiveLocation;
        private string receiveType;
        private DateTime receiveDate = new DateTime(1900, 1, 1);
        private SupplierRelationInfo theSupplierRelationInfo;


        private Iesi.Collections.Generic.ISet<BasicCostSet> basiCostSets = new Iesi.Collections.Generic.HashedSet<BasicCostSet>();
        virtual public Iesi.Collections.Generic.ISet<BasicCostSet> BasiCostSets
        {
            get { return basiCostSets; }
            set { basiCostSets = value; }
        }

        /// <summary>
        /// 增加费用设置表明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddBasicCostSet(BasicCostSet detail)
        {
            detail.Master = this;
            BasiCostSets.Add(detail);
        }
        private string balRule;

        /// <summary>
        /// 结算规则
        /// </summary>
        public virtual string BalRule
        {
            get { return balRule; }
            set { balRule = value; }
        }

        /// <summary>
        /// 出租方(供应商)
        /// </summary>
        public virtual SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }

        /// <summary>
        /// 交货日期
        /// </summary>
        public virtual DateTime ReceiveDate
        {
            get { return receiveDate; }
            set { receiveDate = value; }
        }

        /// <summary>
        /// 交货方式
        /// </summary>
        public virtual string ReceiveType
        {
            get { return receiveType; }
            set { receiveType = value; }
        }

        /// <summary>
        /// 交货地点
        /// </summary>
        public virtual string ReceiveLocation
        {
            get { return receiveLocation; }
            set { receiveLocation = value; }
        }

        /// <summary>
        /// 原始合同号
        /// </summary>
        public virtual string OriginalContractNo
        {
            get { return originalContractNo; }
            set { originalContractNo = value; }
        }

        /// <summary>
        /// 附件路径
        /// </summary>
        public virtual string AttachFilePath
        {
            get { return attachFilePath; }
            set { attachFilePath = value; }
        }
    }
}
