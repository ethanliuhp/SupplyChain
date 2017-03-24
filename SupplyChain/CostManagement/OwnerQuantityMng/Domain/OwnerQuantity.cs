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
    /// 业主报量状态表
    /// </summary>
    [Serializable]
    public class OwnerQuantity
    {
        private string id;
        /// <summary>
        /// Id
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private decimal sumSubmitMoney;//报送总金额
        /// <summary>
        /// 报送总金额
        /// </summary>
        virtual public decimal SumSubmitMoney
        {
            get { return sumSubmitMoney; }
            set { sumSubmitMoney = value; }
        }
        private decimal sumContractMoney;//合同总金额
        /// <summary>
        /// 合同总金额
        /// </summary>
        virtual public decimal SumContractMoney
        {
            get { return sumContractMoney; }
            set { sumContractMoney = value; }
        }
        private string unitPriceName;//价格计量单位名称
        /// <summary>
        /// 价格计量单位名称
        /// </summary>
        virtual public string UnitPriceName
        {
            get { return unitPriceName; }
            set { unitPriceName = value; }
        }
        private StandardUnit unitPrice;//价格计量单位
        /// <summary>
        /// 价格计量单位
        /// </summary>
        virtual public StandardUnit UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }
        private decimal realCollectionMoney;//实际收款累计金额
        /// <summary>
        /// 实际收款累计金额
        /// </summary>
        virtual public decimal RealCollectionMoney
        {
            get { return realCollectionMoney; }
            set { realCollectionMoney = value; }
        }
        private string projectId;
        /// <summary>
        /// 归属项目
        /// </summary>
        virtual public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        private string projectName;
        /// <summary>
        /// 归属项目名称
        /// </summary>
        virtual public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
        private decimal sumConfirmMoney;//业主确认累计金额
        /// <summary>
        /// 业主确认累计金额
        /// </summary>
        virtual public decimal SumConfirmMoney
        {
            get { return sumConfirmMoney; }
            set { sumConfirmMoney = value; }
        }
        private DateTime lastUpdateDate;//最后更新时间
        /// <summary>
        /// 最后更新时间
        /// </summary>
        virtual public DateTime LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }
        private decimal sumPayforMoney;
        /// <summary>
        /// 应付累计金额
        /// </summary>
        virtual public decimal SumPayforMoney
        {
            get { return sumPayforMoney; }
            set { sumPayforMoney = value; }
        }
        private QWBSManage qWBSGUID;
        private string qWBSName;
        private string qWBSSysCode;

        /// <summary>
        /// 清单WBS
        /// </summary>
        virtual public QWBSManage QWBSGUID
        {
            get { return qWBSGUID; }
            set { qWBSGUID = value; }
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
        virtual public string QWBSSysCode
        {
            get { return qWBSSysCode; }
            set { qWBSSysCode = value; }
        }



    }
}
