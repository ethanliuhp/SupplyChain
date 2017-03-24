using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.MaterialManage.MaterialBalanceMng.Domain
{
    /// <summary>
    /// 料具结算
    /// </summary>
    [Serializable]
    public class MaterialBalanceMaster : BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;
        private string oldContractNum;
        private int fiscalYear;
        private int fiscalMonth;
        private DateTime startDate;
        private DateTime endDate;
        private decimal sumMatMoney;
        private decimal sumMatQuantity;
        private decimal otherMoney;
        private GWBSTree usedPart;
        private string usedPartName;
        private string usedPartSysCode;

        private Iesi.Collections.Generic.ISet<MatBalOtherCostDetail> matBalOtherCostDetails = new Iesi.Collections.Generic.HashedSet<MatBalOtherCostDetail>();
        virtual public Iesi.Collections.Generic.ISet<MatBalOtherCostDetail> MatBalOtherCostDetails
        {
            get { return matBalOtherCostDetails; }
            set { matBalOtherCostDetails = value; }
        }

        /// <summary>
        /// 增加其他费用明细
        /// </summary>
        /// <param name="detail"></param>
        virtual public void AddMatBalOtherCostDetails(MatBalOtherCostDetail detail)
        {
            detail.Master = this;
            MatBalOtherCostDetails.Add(detail);
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
        /// <summary>
        /// 会计年
        /// </summary>
        virtual public int FiscalYear
        {
            get { return fiscalYear; }
            set { fiscalYear = value; }
        }
        /// <summary>
        /// 会计月
        /// </summary>
        virtual public int FiscalMonth
        {
            get { return fiscalMonth; }
            set { fiscalMonth = value; }
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        virtual public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
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
        /// 料具总费用
        /// </summary>
        virtual public decimal SumMatMoney
        {
            get { return sumMatMoney; }
            set { sumMatMoney = value; }
        }
        /// <summary>
        /// 料具总数量
        /// </summary>
        virtual public decimal SumMatQuantity
        {
            get { return sumMatQuantity; }
            set { sumMatQuantity = value; }
        }
        /// <summary>
        /// 料具其他费用
        /// </summary>
        virtual public decimal OtherMoney
        {
            get { return otherMoney; }
            set { otherMoney = value; }
        }
        /// <summary> 
        /// 使用部位
        /// </summary>
        virtual public GWBSTree UsedPart
        {
            get { return usedPart; }
            set { usedPart = value; }
        }
        /// <summary>
        /// 使用部位名称
        /// </summary>
        virtual public string UsedPartName
        {
            get { return usedPartName; }
            set { usedPartName = value; }
        }
        /// <summary>
        /// 使用部位层次码
        /// </summary>
        virtual public string UsedPartSysCode
        {
            get { return usedPartSysCode; }
            set { usedPartSysCode = value; }
        }
    }
}
