using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.SupplyManage.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.SupplyManage.SupplyPlanManage.Domain
{
    /// <summary>
    /// 采购计划明细
    /// </summary>
    [Serializable]
    public class SupplyPlanDetail : BaseSupplyDetail
    {
        private decimal leftQuantity;
        private GWBSTree usedPart;
        private string usedPartName;
        private string sysCode;

        /// <summary>
        /// GWBS层次码
        /// </summary>
        virtual public string SysCode
        {
            get { return sysCode; }
            set { sysCode = value; }
        }
        private decimal quantityTemp;
        private string technologyParameter;
        virtual public string TechnologyParameter
        {
            get { return technologyParameter; }
            set { technologyParameter = value; }
        }
        virtual public decimal QuantityTemp
        {
            get { return quantityTemp; }
            set { quantityTemp = value; }
        }


        /// <summary>
        /// 剩余数量
        /// </summary>
        virtual public decimal LeftQuantity
        {
            get { return leftQuantity; }
            set { leftQuantity = value; }
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
    }
}
