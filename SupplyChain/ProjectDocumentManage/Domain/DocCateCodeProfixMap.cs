using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain
{
    /// <summary>
    /// 文档分类编码前缀映射(知识库-项目管理IRP)
    /// </summary>
    [Serializable]
    public class DocCateCodeProfixMap
    {
        private string id;
        private long version;

        private string _KBDocCateCodeProfix;
        private string _MBP_IRPDocCateCodeProfix;

        /// <summary>
        /// 知识库文档分类编码前缀
        /// </summary>
        public virtual string KBDocCateCodeProfix
        {
            get { return _KBDocCateCodeProfix; }
            set { _KBDocCateCodeProfix = value; }
        }

        /// <summary>
        /// 项目管理IRP文档分类编码前缀
        /// </summary>
        public virtual string MBP_IRPDocCateCodeProfix
        {
            get { return _MBP_IRPDocCateCodeProfix; }
            set { _MBP_IRPDocCateCodeProfix = value; }
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
