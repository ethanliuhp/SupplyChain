using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core.Attributes;


namespace Application.Business.Erp.SupplyChain.Basic.Domain
{
    /// <summary>
    /// 基础数据类
    /// </summary>
    [Serializable]
    [Entity]
    public class BasicDataOptr
    {
        private string id;
        private string parentId;
        private string basicCode;
        private string basicName;
        private string descript;
        private int state;
        private string extendField1;


        /// <summary>
        /// 状态
        /// </summary>
        virtual public int State
        {
            get { return state; }
            set { state = value; }
        }
        /// <summary>
        /// ID
        /// </summary>
        public virtual string Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// 父ID
        /// </summary>
        public virtual string ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        /// <summary>
        /// 编码
        /// </summary>
        public virtual string BasicCode
        {
            get { return basicCode; }
            set { basicCode = value; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string BasicName
        {
            get { return basicName; }
            set { basicName = value; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Descript
        {
            get { return descript; }
            set { descript = value; }
        }

        /// <summary>
        /// 扩展字段1
        /// </summary>
        public virtual string ExtendField1
        {
            get { return extendField1; }
            set { extendField1 = value; }
        }
    }
}
