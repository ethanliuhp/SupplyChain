using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.ConcreteManage.PumpingPoundsMng.Domain
{
    /// <summary>
    /// 抽磅单主表
    /// </summary>
    [Serializable]
    public class PumpingPoundsMaster : BaseMaster
    {
        private decimal proportion;
        private decimal busNum;
        private decimal sumBusNum;
        private decimal sumDelta;
        private decimal sumWeight;
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;


        private GWBSTree usedPart;
        private string usedPartName;
        /// <summary>
        /// 浇筑部位
        /// </summary>
        virtual public GWBSTree UsedPart
        {
            get { return usedPart; }
            set { usedPart = value; }
        }
        /// <summary>
        /// 浇筑部位名称
        /// </summary>
        virtual public string UsedPartName
        {
            get { return usedPartName; }
            set { usedPartName = value; }
        }

        /// <summary>
        /// 抽磅比例
        /// </summary>
        virtual public decimal Proportion
        {
            get { return proportion; }
            set { proportion = value; }
        }
        /// <summary>
        /// 总车数
        /// </summary>
        virtual public decimal SumBusNum
        {
            get { return sumBusNum; }
            set { sumBusNum = value; }
        }
        /// <summary>
        /// 抽磅车数
        /// </summary>
        virtual public decimal BusNum
        {
            get { return busNum; }
            set { busNum = value; }
        }
        /// <summary>
        /// 总量差
        /// </summary>
        virtual public decimal SumDelta
        {
            get { return sumDelta; }
            set { sumDelta = value; }
        }
        /// <summary>
        /// 总重量
        /// </summary>
        virtual public decimal SumWeight
        {
            get { return sumWeight; }
            set { sumWeight = value; }
        }
        /// <summary>
        /// 供应单位
        /// </summary>
        virtual public SupplierRelationInfo TheSupplierRelationInfo
        {
            get { return theSupplierRelationInfo; }
            set { theSupplierRelationInfo = value; }
        }
        /// <summary>
        /// 供应单位名称
        /// </summary>
        virtual public string SupplierName
        {
            get { return supplierName; }
            set { supplierName = value; }
        }
    }
}
