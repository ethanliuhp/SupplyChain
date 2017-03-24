using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain
{
    /// <summary>
    /// 业主报量主表
    /// </summary>
    [Serializable]
    public class OwnerQuantityMaster : BaseMaster
    {
        private decimal submitSumQuantity;
        private string accountSign;
        private decimal collectionSumMoney;
        private decimal confirmSumMoney;
        private decimal sumPayforMoney;

        private string auditManage;
        private string projectRecovery;
        private string ownerBreach;
        private string quantityType;//报量类型
     
        /// <summary>
        /// 项目报量审核情况
        /// </summary>
        virtual public string AuditManage
        {
            get { return auditManage; }
            set { auditManage = value; }
        }
        /// <summary>
        /// 工程款回收情况
        /// </summary>
        virtual public string ProjectRecovery
        {
            get { return projectRecovery; }
            set { projectRecovery = value; }
        }
        /// <summary>
        /// 业主违约情况
        /// </summary>
        virtual public string OwnerBreach
        {
            get { return ownerBreach; }
            set { ownerBreach = value; }
        }

        /// <summary>
        /// 应付总金额
        /// </summary>
        virtual public decimal SumPayforMoney
        {
            get { return sumPayforMoney; }
            set { sumPayforMoney = value; }
        }
        /// <summary>
        /// 报送总金额
        /// </summary>
        virtual public decimal  SubmitSumQuantity
        {

            get { return submitSumQuantity; }
            set { submitSumQuantity = value; }
        }
        /// <summary>
        /// 收款总金额
        /// </summary>
        virtual public decimal CollectionSumMoney
        {
            get {return collectionSumMoney;}
            set { collectionSumMoney = value; }
        }
        /// <summary>
        /// 业务确认总金额
        /// </summary>
        virtual public decimal ConfirmSumMoney
        {
            get {return confirmSumMoney;}
            set { confirmSumMoney = value; }
        }
        /// <summary>
        /// 登帐标志
        /// </summary>
        virtual public string AccountSign
        {
            get { return accountSign; }
            set { accountSign = value; }
        }
        /// <summary>
        /// 报量类型
        /// </summary>
        virtual public string QuantityType
        {
            get { return quantityType; }
            set { quantityType = value; }
        }
    }
}
