using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.InitStruct
{
    /// <summary>
    /// 科目查询条件
    /// </summary>
    [Serializable]
    public struct QueryAccSrt
    {
        /// <summary>
        /// 起始科目
        /// </summary>
        public string BeginCode;
        /// <summary>
        /// 截至科目
        /// </summary>
        public string EndCode;
        /// <summary>
        /// 起始级别
        /// </summary>
        public int BeginLevel;
        /// <summary>
        /// 截至级别
        /// </summary>
        public int EndLevel;
        /// <summary>
        /// 末级科目
        /// </summary>
        public bool isLeaf;
    }
}
