using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;

namespace Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain
{
    /// <summary>
    /// 明细审批属性定义
    /// </summary>
    [Serializable]
    public class AppDetailPropertySet
    {
        private string id;
        private string detailClassName;
        private string detailPropertyName;
        private string detailPropertyChineseName;
        private bool detailPropertyVisible;
        private bool detailPropertyReadOnly;
        private AppTableSet parentId;
        private int serialNumber;
        private string dbFieldName;
        private string dataType;

        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 明细类名称
        /// </summary>
        virtual public string DetailClassName
        {
            get { return detailClassName; }
            set { detailClassName = value; }
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        virtual public string DetailPropertyName
        {
            get { return detailPropertyName; }
            set { detailPropertyName = value; }
        }
        /// <summary>
        /// 属性中文名称
        /// </summary>
        virtual public string DetailPropertyChineseName
        {
            get { return detailPropertyChineseName; }
            set { detailPropertyChineseName = value; }
        }
        /// <summary>
        /// 是否可见
        /// </summary>
        virtual public bool DetailPropertyVisible
        {
            get { return detailPropertyVisible; }
            set { detailPropertyVisible = value; }
        }
        /// <summary>
        /// 是否可以修改
        /// </summary>
        virtual public bool DetailPropertyReadOnly
        {
            get { return detailPropertyReadOnly; }
            set { detailPropertyReadOnly = value; }
        }
        /// <summary>
        /// 父ID
        /// </summary>
        virtual public AppTableSet ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        /// <summary>
        /// 顺序号
        /// </summary>
        virtual public int SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }
        /// <summary>
        /// 数据库列名
        /// </summary>
        virtual public string DBFieldName
        {
            get { return dbFieldName; }
            set { dbFieldName = value; }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        virtual public string DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        private string tempBillId;
        private string tempValue;

        /// <summary>
        /// 临时单据ID，不存储
        /// </summary>
        virtual public string TempBillId
        {
            get { return tempBillId; }
            set { tempBillId = value; }
        }
        /// <summary>
        /// 临时数据值，不存储
        /// </summary>
        virtual public string TempValue
        {
            get { return tempValue; }
            set { tempValue = value; }
        }

    }
}
