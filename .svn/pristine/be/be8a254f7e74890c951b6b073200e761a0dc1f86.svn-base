using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.QWBS.Domain;

namespace Application.Business.Erp.SupplyChain.CostManagement.OwnerQuantityMng.Domain
{
    /// <summary>
    /// 业主报量明细
    /// </summary>
    [Serializable]
    public class OwnerQuantityDetail : BaseDetail
    {
        private decimal submitQuantity;
        private string state;
        private decimal collectionMoney;
        private decimal confirmMoney;
        private QWBSManage qWBS;
        private string qWBSName;
        private string qWBSCode;
        private string priceUnitName;
        private StandardUnit priceUnit;
        private decimal payforMoney;
        private decimal realProduct;//实际产值
        private decimal realpay;//实际应付款
        private decimal rightRate;//确权率
        private DateTime quantityDate;//报量日期
        private DateTime confirmDate;//审定日期
        private DateTime confirmStartDate;
        private DateTime confirmEndDate;
        private decimal gatheringRate;
        private decimal acctGatheringMoney;

        /// <summary>
        /// 实际产值
        /// </summary>
        virtual public decimal RealProduct
        {
            get { return realProduct; }
            set { realProduct = value; }
        }
        /// <summary>
        /// 实际应付款
        /// </summary>
        virtual public decimal Realpay
        {
            get { return realpay; }
            set { realpay = value; }
        }
        /// <summary>
        /// 确权率
        /// </summary>
        virtual public decimal RightRate
        {
            get { return rightRate; }
            set { rightRate = value; }
        }
        /// <summary>
        /// 应付金额
        /// </summary>
        virtual public decimal PayforMoney
        {
            get { return payforMoney; }
            set { payforMoney = value; }
        }
        /// <summary>
        /// 报送金额
        /// </summary>
        virtual public decimal SubmitQuantity
        {
            get { return submitQuantity; }
            set { submitQuantity = value; }
        }
        /// <summary>
        /// 收款金额
        /// </summary>
        virtual public decimal CollectionMoney
        {
            get { return collectionMoney; }
            set { collectionMoney = value; }
        }
        /// <summary>
        /// 业务确认金额
        /// </summary>
        virtual public decimal ConfirmMoney
        {
            get { return confirmMoney; }
            set { confirmMoney = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        virtual public string State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 价格单位
        /// </summary>
        virtual public StandardUnit PriceUnit
        {
            get { return priceUnit; }
            set { priceUnit = value; }
        }

        /// <summary>
        /// 价格单位名称
        /// </summary>
        virtual public string PriceUnitName
        {
            get { return priceUnitName; }
            set { priceUnitName = value; }
        }
        /// <summary>
        /// 清单WBS
        /// </summary>
        virtual public QWBSManage QWBS
        {
            get { return qWBS; }
            set { qWBS = value; }
        }
        /// <summary>
        /// 清单WBS名称
        /// </summary>
        virtual public string QWBSName
        {
            get { return qWBSName; }
            set { qWBSName = value; }
        }
        /// <summary>
        /// 清单WBS层次码
        /// </summary>
        virtual public string QWBSCode
        {
            get { return qWBSCode; }
            set { qWBSCode = value; }
        }
        /// <summary>
        /// 报量日期
        /// </summary>
        virtual public DateTime QuantityDate
        {
            get { return quantityDate; }
            set { quantityDate = value; }
        }
        /// <summary>
        /// 审定日期
        /// </summary>
        virtual public DateTime ConfirmDate
        {
            get { return confirmDate; }
            set { confirmDate = value; }
        }
        /// <summary>
        /// 审量确认开始日期
        /// </summary>
        virtual public DateTime ConfirmStartDate
        {
            get { return confirmStartDate; }
            set { confirmStartDate = value; }
        }
        /// <summary>
        /// 审量确认结束日期
        /// </summary>
        virtual public DateTime ConfirmEndDate
        {
            get { return confirmEndDate; }
            set { confirmEndDate = value; }
        }
        /// <summary>
        /// 合同对应收款比率
        /// </summary>
        virtual public decimal GatheringRate
        {
            get { return gatheringRate; }
            set { gatheringRate = value; }
        }
        /// <summary>
        /// 应收款金额
        /// </summary>
        virtual public decimal AcctGatheringMoney
        {
            get { return acctGatheringMoney; }
            set { acctGatheringMoney = value; }
        }
    }
}
