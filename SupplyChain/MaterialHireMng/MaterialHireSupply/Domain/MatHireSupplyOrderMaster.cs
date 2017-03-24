using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireSupply.BasicDomain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.MaterialHireMng.MaterialHireSupply.Domain
{
    /// <summary>
    /// 采购合同主表
    /// </summary>
    [Serializable]
    public class MatHireSupplyOrderMaster : MatHireBasicSupplyMaster
    {
        private string attachmentDocPath;
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;
        private decimal contractMoney;
        private string contractMatDes;
        private string telephone;
        private string contactPerson;
        private DateTime signDate;
        private string oldContratcNum;
        private string qualityRequirement;
        private Iesi.Collections.Generic.ISet<MatHireSupplyOrderPayment> paymentDetails = new Iesi.Collections.Generic.HashedSet<MatHireSupplyOrderPayment>();
        virtual public Iesi.Collections.Generic.ISet<MatHireSupplyOrderPayment> PaymentDetails
        {
            get { return paymentDetails; }
            set { paymentDetails = value; }
        }

        private Iesi.Collections.Generic.ISet<MatHireSupplyOrderProjectDetail> projectDetails = new Iesi.Collections.Generic.HashedSet<MatHireSupplyOrderProjectDetail>();
        /// <summary>
        /// 项目明细集合
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<MatHireSupplyOrderProjectDetail> ProjectDetails
        {
            get { return projectDetails; }
            set { projectDetails = value; }
        }

        private string specialType;
        private decimal pumpMoney;

        /// <summary>
        /// 泵送费
        /// </summary>
        virtual public decimal PumpMoney
        {
            get { return pumpMoney; }
            set { pumpMoney = value; }
        }

        /// <summary>
        /// 专业分类
        /// </summary>
        virtual public string SpecialType
        {
            get { return specialType; }
            set { specialType = value; }
        }

        private decimal rjSumMoney;
        /// <summary>
        /// 认价总金额
        /// </summary>
        virtual public decimal RJSumMoney
        {
            get { return rjSumMoney; }
            set { rjSumMoney = value; }
        }

        private string special;
        /// <summary>
        /// 专业 用于区分单据
        /// </summary>
        public virtual string Special
        {
            get { return special; }
            set { special = value; }
        }
        /// <summary>
        /// 增加付款方式
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddPaymentDetail(MatHireSupplyOrderPayment detail)
        {
            detail.Master = this;
            PaymentDetails.Add(detail);
        }

        /// <summary>
        /// 合同附件路径
        /// </summary>
        virtual public string AttachmentDocPath
        {
            get { return attachmentDocPath; }
            set { attachmentDocPath = value; }
        }
        /// <summary>
        /// 供应商
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        /// <summary>
        /// 合同金额
        /// </summary>
        virtual public decimal ContractMoney
        {
            get { return contractMoney; }
            set { contractMoney = value; }
        }
        /// <summary>
        /// 合同资源描述
        /// </summary>
        virtual public string ContractMatDes
        {
            get { return contractMatDes; }
            set { contractMatDes = value; }
        }
        /// <summary>
        /// 联系电话
        /// </summary>
        virtual public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }
        /// <summary>
        /// 联系人
        /// </summary>
        virtual public string ContactPerson
        {
            get { return contactPerson; }
            set { contactPerson = value; }
        }
        /// <summary>
        /// 签订日期
        /// </summary>
        virtual public DateTime SignDate
        {
            get { return signDate; }
            set { signDate = value; }
        }
        /// <summary>
        /// 原始合同号
        /// </summary>
        virtual public string OldContractNum
        {
            get { return oldContratcNum; }
            set { oldContratcNum = value; }
        }
        /// <summary>
        /// 质量要求
        /// </summary>
        virtual public string QualityRequirement
        {
            get { return qualityRequirement; }
            set { qualityRequirement = value; }
        }

        /// <summary>
        /// 增加项目信息明细
        /// </summary>
        /// <param name="detail"></param>
        public virtual void AddProjectDetail(MatHireSupplyOrderProjectDetail detail)
        {
            detail.Master = this;
            this.ProjectDetails.Add(detail);
        }
    }
}
