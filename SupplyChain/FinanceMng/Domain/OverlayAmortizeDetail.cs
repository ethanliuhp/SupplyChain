using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;

namespace Application.Business.Erp.SupplyChain.FinanceMng.Domain
{
    /// <summary>
    /// 临建摊销单明细
    /// </summary>
    [Serializable]
    public class OverlayAmortizeDetail : BaseDetail
    {
        private Double periodAmortize;
        private Double totalAmortize;
        private Double overlayValue;
        private String overlayProject;
        private int amortizeTime;
        private Double orgValue;


        /// <summary>
        /// 本期摊销
        /// </summary>
        virtual public Double PeriodAmortize
        {
            get { return periodAmortize; }
            set { periodAmortize = value; }
        }
        /// <summary>
        /// 累计摊销
        /// </summary>
        virtual public Double TotalAmortize
        {
            get { return totalAmortize; }
            set { totalAmortize = value; }
        }
        /// <summary>
        /// 临建净值
        /// </summary>
        virtual public Double OverlayValue
        {
            get { return overlayValue; }
            set { overlayValue = value; }
        }
        /// <summary>
        /// 临建项目
        /// </summary>
        virtual public String OverlayProject
        {
            get { return overlayProject; }
            set { overlayProject = value; }
        }
        /// <summary>
        /// 临建期限
        /// </summary>
        virtual public int AmortizeTime
        {
            get { return amortizeTime; }
            set { amortizeTime = value; }
        }
        /// <summary>
        /// 原值
        /// </summary>
        virtual public Double OrgValue
        {
            get { return orgValue; }
            set { orgValue = value; }
        }


    }
}
