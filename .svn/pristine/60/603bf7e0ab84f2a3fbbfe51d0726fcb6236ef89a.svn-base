using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.ConcreteManage.PouringNoteMng.Domain
{
    /// <summary>
    /// 浇筑记录主表
    /// </summary>
    [Serializable]
    public class PouringNoteMaster : BaseMaster
    {
        private SupplierRelationInfo theSupplierRelationInfo;
        private string supplierName;
        private string ticketNum;
        private SupplierRelationInfo inportSupplier;
        private string inportSupplierName;
        private SupplierRelationInfo exportSupplier;
        private string exportSupplierName;
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
        /// 小票单号
        /// </summary>
        virtual public string TicketNum
        {
            get { return ticketNum; }
            set { ticketNum = value; }
        }
        /// <summary>
        /// 调入单位
        /// </summary>
        virtual public SupplierRelationInfo InportSupplier
        {
            get { return inportSupplier; }
            set { inportSupplier = value; }
        }
        /// <summary>
        /// 调入单位名称
        /// </summary>
        virtual public string InportSupplierName
        {
            get { return inportSupplierName; }
            set { inportSupplierName = value; }
        }
        /// <summary>
        /// 调出单位
        /// </summary>
        virtual public SupplierRelationInfo ExportSupplier
        {
            get { return exportSupplier; }
            set { exportSupplier = value; }
        }
        /// <summary>
        /// 调出单位名称
        /// </summary>
        virtual public string ExportSupplierName
        {
            get { return exportSupplierName; }
            set { exportSupplierName = value; }
        }
    }
}
