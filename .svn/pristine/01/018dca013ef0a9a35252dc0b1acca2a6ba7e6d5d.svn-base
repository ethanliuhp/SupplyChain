using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.WBS.Domain;

namespace Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain
{
    /// <summary>
    /// 废旧物资处理明细
    /// </summary>
    [Serializable]
    public class WasteMatProcessDetail : BaseDetail
    {
        private string plateNumber;
        private decimal processPrice;
        private decimal netWeight;
        private decimal grossWeight;
        private decimal tareWeight;
        private string forwardDetailId;
        private string receiptCode;
        private decimal totalValue;
        private decimal quantityTemp;
        private DateTime processDate;

        
        private GWBSTree usedPart;
        private string usedPartName;
        private string usedPartSysCode;
        /// <summary>
        ///使用部位
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
        /// <summary>
        /// 车牌号
        /// </summary>
        virtual public decimal QuantityTemp
        {
            get { return quantityTemp; }
            set { quantityTemp = value; }
        }
        /// <summary>
        /// 车牌号
        /// </summary>
        virtual public string PlateNumber
        {
            get { return plateNumber; }
            set { plateNumber = value; }
        }
        /// <summary>
        /// 处理单价
        /// </summary>
        virtual public decimal ProcessPrice
        {
            get { return processPrice; }
            set { processPrice = value; }
        }
        /// <summary>
        /// 处理时间
        /// </summary>
        virtual public DateTime ProcessDate
        {
            get { return processDate; }
            set { processDate = value; }
        }
        /// <summary>
        /// 净重
        /// </summary>
        virtual public decimal NetWeight
        {
            get { return netWeight; }
            set { netWeight = value; }
        }
        /// <summary>
        /// 毛重
        /// </summary>
        virtual public decimal GrossWeight
        {
            get { return grossWeight; }
            set { grossWeight = value; }
        }
        /// <summary>
        /// 皮重
        /// </summary>
        virtual public decimal TareWeight
        {
            get { return tareWeight; }
            set { tareWeight = value; }
        }
        /// <summary>
        /// 前驱明细Id
        /// </summary>
        virtual public string ForwardDetailId
        {
            get { return forwardDetailId; }
            set { forwardDetailId = value; }
        }
        /// <summary>
        /// 收据单号
        /// </summary>
        virtual public string ReceiptCode
        {
            get { return receiptCode; }
            set { receiptCode = value; }
        }
        /// <summary>
        /// 总值
        /// </summary>
        virtual public decimal TotalValue
        {
            get { return totalValue; }
            set { totalValue = value; }
        }
    }
}
