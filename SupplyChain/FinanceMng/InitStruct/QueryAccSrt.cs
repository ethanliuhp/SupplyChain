using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.InitStruct
{
    /// <summary>
    /// ��Ŀ��ѯ����
    /// </summary>
    [Serializable]
    public struct QueryAccSrt
    {
        /// <summary>
        /// ��ʼ��Ŀ
        /// </summary>
        public string BeginCode;
        /// <summary>
        /// ������Ŀ
        /// </summary>
        public string EndCode;
        /// <summary>
        /// ��ʼ����
        /// </summary>
        public int BeginLevel;
        /// <summary>
        /// ��������
        /// </summary>
        public int EndLevel;
        /// <summary>
        /// ĩ����Ŀ
        /// </summary>
        public bool isLeaf;
    }
}
