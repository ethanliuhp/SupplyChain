using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain
{
    /// <summary>
    /// MBP对象类型关联文档分类缺省分类配置
    /// </summary>
    [Serializable]
    public class ObjTypeDefaultCateConfig
    {
        private string id;
        private long version;

        private string _objTypeName;
        private string _objTypeDesc;
        private string _objTypeAttributeName;
        private string _objTypeAttributeDesc;
        private string _objTypeAttributeValue;
        private string _docCateGUID;
        private string _docCateCode;
        private string _docCateName;

        /// <summary>
        /// 对象类型名称
        /// </summary>
        public virtual string ObjTypeName
        {
            get { return _objTypeName; }
            set { _objTypeName = value; }
        }
        /// <summary>
        /// 对象类型描述
        /// </summary>
        public virtual string ObjTypeDesc
        {
            get { return _objTypeDesc; }
            set { _objTypeDesc = value; }
        }
        /// <summary>
        /// 对象类型属性名称
        /// </summary>
        public virtual  string ObjTypeAttributeName
        {
            get { return _objTypeAttributeName; }
            set { _objTypeAttributeName = value; }
        }
        /// <summary>
        /// 对象类型属性说明
        /// </summary>
        public virtual string ObjTypeAttributeDesc
        {
            get { return _objTypeAttributeDesc; }
            set { _objTypeAttributeDesc = value; }
        }
        /// <summary>
        /// 对象类型属性值
        /// </summary>
        public virtual  string ObjTypeAttributeValue
        {
            get { return _objTypeAttributeValue; }
            set { _objTypeAttributeValue = value; }
        }
        /// <summary>
        /// 文档分类GUID
        /// </summary>
        public virtual  string DocCateGUID
        {
            get { return _docCateGUID; }
            set { _docCateGUID = value; }
        }
        /// <summary>
        /// 文档分类代码
        /// </summary>
        public virtual  string DocCateCode
        {
            get { return _docCateCode; }
            set { _docCateCode = value; }
        }
        /// <summary>
        /// 文档分类名称
        /// </summary>
        public virtual  string DocCateName
        {
            get { return _docCateName; }
            set { _docCateName = value; }
        }


        /// <summary>
        /// ID
        /// </summary>
        virtual public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 版本
        /// </summary>
        virtual public long Version
        {
            get { return version; }
            set { version = value; }
        }
    }
}
