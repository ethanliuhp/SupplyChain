using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.Financial.BasicAccount.DailyOperation.ValueObject
{
    /// <summary>
    /// ��Ŀֵ����
    /// </summary>
    [Serializable]
    public struct AccTitleVob
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id;        
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public string AccCode;        
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public string AccName;
        /// <summary>
        /// ��Ŀһ����Ŀ����
        /// </summary>
        public string AccFirName;
        /// <summary>
        /// ��Ŀ��ϸ��Ŀ����
        /// </summary>
        public string AccDetName;
        /// <summary>
        /// ������������
        /// </summary>
        public int AccAssisType;
        /// <summary>
        /// ������
        /// </summary>
        public bool BankAccBook;
        /// <summary>
        /// ̨�����Id
        /// </summary>
        public string DeskAccId;
    }
}
