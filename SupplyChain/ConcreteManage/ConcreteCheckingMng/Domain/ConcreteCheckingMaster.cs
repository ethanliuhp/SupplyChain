using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Resource.MaterialResource.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.SupplyChain.ConcreteManage.ConcreteCheckingMng.Domain
{
    /// <summary>
    /// 商品砼对账单
    /// </summary>

    [Serializable]
    public class ConcreteCheckingMaster : BaseMaster
    {
        private MaterialCategory materialCategory;
        private string matCategoryName;
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;
        private DateTime beginDate = StringUtil.StrToDateTime("1900-01-01");
        private DateTime endDate = StringUtil.StrToDateTime("1900-01-01");
        private decimal sumMoney;
        private decimal sumVolume;
        private decimal adjustmentMoney;
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
        /// 材料分类
        /// </summary>
        virtual public MaterialCategory MaterialCategory
        {
            get { return materialCategory; }
            set { materialCategory = value; }
        }
        /// <summary>
        /// 材料分类名称
        /// </summary>
        virtual public string MatCategoryName
        {
            get { return matCategoryName; }
            set { matCategoryName = value; }
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        virtual public DateTime BeginDate
        {
            get { return beginDate; }
            set { beginDate = value; }
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        virtual public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }
        /// <summary>
        /// 总金额
        /// </summary>
        virtual public decimal SumMoney
        {
            get { return sumMoney; }
            set { sumMoney = value; }
        }
        /// <summary>
        /// 总方量
        /// </summary>
        virtual public decimal SumVolume
        {
            get { return sumVolume; }
            set { sumVolume = value; }
        }
        /// <summary>
        /// 调整金额
        /// </summary>
        virtual public decimal AdjustmentMoney
        {
            get { return adjustmentMoney; }
            set { adjustmentMoney = value; }
        }
    }
}
;