using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialDamageApplyMng.Domain
{
    /// <summary>
    /// 料具报损申请
    /// </summary>
    [Serializable]
    public class MaterialDamageApplyMaster : BaseMaster
    {
        private string damageReason;
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;
        private string oldContractNum;

        /// <summary>
        /// 报损原因
        /// </summary>
        virtual public string DamageReason
        {
            get { return damageReason; }
            set { damageReason = value; }
        }
        /// <summary>
        /// 出租方/供应商
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }
        /// <summary>
        /// 出租方/供应商名称
        /// </summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
        /// <summary>
        /// 原始合同号
        /// </summary>
        virtual public string OldContractNum
        {
            get { return oldContractNum; }
            set { oldContractNum = value; }
        }
    }
}
