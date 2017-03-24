using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Domain;
using Application.Resource.PersonAndOrganization.SupplierManagement.RelateClass;

namespace Application.Business.Erp.SupplyChain.StockManage.DetectionReceiptManage.Domain
{
    /// <summary>
    /// 检测回执单明细
    /// </summary>
    public class DetectionReceiptDetail : BaseDetail
    {
        private SupplierRelationInfo supplyUnit;
        private string supplyUnitName;
        private DateTime comeDate;
        private decimal detectionQuantity;
        private string manufacturer;
        private string appearanceTast;
        private string tastResult;

        /// <summary>
        /// 供应单位
        /// </summary>
        virtual public SupplierRelationInfo SupplyUnit
        {
            get { return supplyUnit; }
            set { supplyUnit = value; }
        }
        /// <summary>
        /// 供应单位名称
        /// </summary>
        virtual public string SupplyUnitName
        {
            get { return supplyUnitName; }
            set { supplyUnitName = value; }
        }
        ///<summary>
        ///进场时间
        ///</summary>
        virtual public DateTime ComeDate
        {
            get { return comeDate; }
            set { comeDate = value; }
        }
        ///<summary>
        ///检测数量
        ///</summary>
        virtual public decimal DetectionQuantity
        {
            get { return detectionQuantity; }
            set { detectionQuantity = value; }
        }
        ///<summary>
        ///厂家
        ///</summary>
        virtual public string Manufacturer
        {
            get { return manufacturer; }
            set { manufacturer = value; }
        }
        ///<summary>
        ///外观检测
        ///</summary>
        virtual public string AppearanceTast
        {
            get { return appearanceTast; }
            set { appearanceTast = value; }
        }
        ///<summary>
        ///检测结果
        ///</summary>
        virtual public string TastResult
        {
            get { return tastResult; }
            set { tastResult = value; }
        }

    }
}
