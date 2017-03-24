using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.CostItemMng.Domain;

namespace Application.Business.Erp.SupplyChain.ConcreteManage.PouringNoteMng.Domain
{
    /// <summary>
    /// 浇筑记录明细
    /// </summary>
    [Serializable]
    public class PouringNoteDetail : BaseDetail
    {
        private CostAccountSubject subjectGUID;
        private string subjectName;
        private string subjectSysCode;
        private decimal planQuantity;
        /// <summary>
        /// 计划数量
        /// </summary>
        virtual public decimal PlanQuantity
        {
            get { return planQuantity; }
            set { planQuantity = value; }
        }
        /// <summary>
        /// 核算科目
        /// </summary>
        virtual public CostAccountSubject SubjectGUID
        {
            get { return subjectGUID; }
            set { subjectGUID = value; }
        }
        /// <summary>
        /// 核算科目名称
        /// </summary>
        virtual public string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }
        /// <summary>
        /// 核算科目层次码
        /// </summary>
        virtual public string SubjectSysCode
        {
            get { return subjectSysCode; }
            set { subjectSysCode = value; }
        }
        private bool isPump;
        /// <summary>
        /// 是否泵送
        /// </summary>
        virtual public bool IsPump
        {
            get { return isPump; }
            set { isPump = value; }
        }

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

        private DateTime pouringDate;
        /// <summary>
        /// 浇筑时间
        /// </summary>
        virtual public DateTime PouringDate
        {
            get { return pouringDate; }
            set { pouringDate = value; }
        }
        private string concreteCheckId;
        /// <summary>
        /// 商品砼对账单ID
        /// </summary>
        virtual public string ConcreteCheckId
        {
            get { return concreteCheckId; }
            set { concreteCheckId = value; }
        }
        private int concreteCheckState;
        /// <summary>
        /// 生成对账单状态  0：未生成对账 1：已生成对账单
        /// </summary>
        virtual public int ConcreteCheckState
        {
            get { return concreteCheckState; }
            set { concreteCheckState = value; }
        }
        private string reasonDes;
        private decimal importQty;
        private decimal importPrice;
        private decimal importMoney;
        private decimal exportQty;
        private decimal exportPrice;
        private decimal exportMoney;
        private decimal consumeQty;
        private decimal consumePrice;
        private decimal consumeMoney;
        private string projectId;


        /// <summary>
        /// 归属项目ID
        /// </summary>
        virtual public string ProjectId
        {
            get { return projectId; }
            set { projectId = value; }
        }
        /// <summary>
        /// 超计划原因说明
        /// </summary>
        virtual public string ReasonDes
        {
            get { return reasonDes; }
            set { reasonDes = value; }
        }
        /// <summary>
        /// 调入数量
        /// </summary>
        virtual public decimal ImportQty
        {
            get { return importQty; }
            set { importQty = value; }
        }
        /// <summary>
        /// 调入单价
        /// </summary>
        virtual public decimal ImportPrice
        {
            get { return importPrice; }
            set { importPrice = value; }
        }
        /// <summary>
        /// 调入金额
        /// </summary>
        virtual public decimal ImportMoney
        {
            get { return importMoney; }
            set { importMoney = value; }
        }
        /// <summary>
        /// 调出数量
        /// </summary>
        virtual public decimal ExportQty
        {
            get { return exportQty; }
            set { exportQty = value; }
        }
        /// <summary>
        /// 调出单价
        /// </summary>
        virtual public decimal ExportPrice
        {
            get { return exportPrice; }
            set { exportPrice = value; }
        }
        /// <summary>
        /// 调出金额
        /// </summary>
        virtual public decimal ExportMoney
        {
            get { return exportMoney; }
            set { exportMoney = value; }
        }
        /// <summary>
        /// 消耗数量
        /// </summary>
        virtual public decimal ConsumeQty
        {
            get { return consumeQty; }
            set { consumeQty = value; }
        }
        /// <summary>
        /// 消耗单价
        /// </summary>
        virtual public decimal ConsumePrice
        {
            get { return consumePrice; }
            set { consumePrice = value; }
        }
        /// <summary>
        /// 消耗金额
        /// </summary>
        virtual public decimal ConsumeMoney
        {
            get { return consumeMoney; }
            set { consumeMoney = value; }
        }
    }
}
