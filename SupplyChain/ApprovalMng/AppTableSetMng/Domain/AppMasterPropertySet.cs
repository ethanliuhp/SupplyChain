using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Approval.AppTableSetMng.Domain;

namespace Application.Business.Erp.SupplyChain.ApprovalMng.AppTableSetMng.Domain
{
    /// <summary>
    /// 主表审批属性定义
    /// </summary>
    [Serializable]
    public class AppMasterPropertySet
    {
        private string id;
        private string masterClassName;
        private string masterPropertyName;
        private string masterPpropertyChineseName;
        private bool masterPropertyVisible;
        private bool masterPropertyReadOnly;
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
        /// 主表类名称
        /// </summary>
        virtual public string MasterClassName
        {
            get { return masterClassName; }
            set { masterClassName = value; }
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        virtual public string MasterPropertyName
        {
            get { return masterPropertyName; }
            set { masterPropertyName = value; }
        }
        /// <summary>
        /// 属性中文名称
        /// </summary>
        virtual public string MasterPpropertyChineseName
        {
            get { return masterPpropertyChineseName; }
            set { masterPpropertyChineseName = value; }
        }
        /// <summary>
        /// 是否可见
        /// </summary>
        virtual public bool MasterPropertyVisible
        {
            get { return masterPropertyVisible; }
            set { masterPropertyVisible = value; }
        }
        /// <summary>
        /// 是否可以修改
        /// </summary>
        virtual public bool MasterPropertyReadOnly
        {
            get { return masterPropertyReadOnly; }
            set { masterPropertyReadOnly = value; }
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
