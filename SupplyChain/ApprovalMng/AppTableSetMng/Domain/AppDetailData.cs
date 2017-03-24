using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain
{
    /// <summary>
    ///审批数据(明细)
    /// </summary>
    [Serializable]
    public class AppDetailData
    {
        private string id;
        private string propertyName;
        private string propertyChineseName;
        private string propertyValue;
        private long appStatus;
        private string billId;
        private string billDtlId;
        private DateTime appDate;
        private int propertySerialNumber;

        private string _AppTableSet;
        /// <summary>
        /// 审批表单
        /// </summary>
        virtual public string AppTableSet
        {
            get { return _AppTableSet; }
            set { _AppTableSet = value; }
        }

        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        virtual public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }
        /// <summary>
        /// 属性中文名称
        /// </summary>
        virtual public string PropertyChineseName
        {
            get { return propertyChineseName; }
            set { propertyChineseName = value; }
        }
        /// <summary>
        /// 属性值
        /// </summary>
        virtual public string PropertyValue
        {
            get { return propertyValue; }
            set { propertyValue = value; }
        }
        /// <summary>
        /// 状态 1:未通过 2：通过
        /// </summary>
        virtual public long AppStatus
        {
            get { return appStatus; }
            set { appStatus = value; }
        }
        /// <summary>
        /// 单据Id
        /// </summary>
        virtual public string BillId
        {
            get { return billId; }
            set { billId = value; }
        }
        /// <summary>
        /// 单据明细ID
        /// </summary>
        virtual public string BillDtlId
        {
            get { return billDtlId; }
            set { billDtlId = value; }
        }
        /// <summary>
        /// 审批日期
        /// </summary>
        virtual public DateTime AppDate
        {
            get { return appDate; }
            set { appDate = value; }
        }
        /// <summary>
        /// 属性显示的顺序
        /// </summary>
        virtual public int PropertySerialNumber
        {
            get { return propertySerialNumber; }
            set { propertySerialNumber = value; }
        }
    }
}
