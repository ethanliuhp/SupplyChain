using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Business.Erp.Financial.BasicAccount.InitialSetting.InitStruct
{
    /// <summary>
    /// ��Ŀ���������ѯ����
    /// </summary>
    [Serializable]
    public struct AccAssistInfoSrt
    {
        /// <summary>
        /// ���������ҹ�ϵ
        /// </summary>
        public bool AndRelation;
        /// <summary>
        /// ��Һ���
        /// </summary>
        public bool ForeignAccount;
        /// <summary>
        /// ��������
        /// </summary>
        public bool QuantityAccount;
        /// <summary>
        /// �ռ���
        /// </summary>
        public bool DailyAccBook;
        /// <summary>
        /// ������
        /// </summary>
        public bool BankAccBook;
        /// <summary>
        /// ���ź���
        /// </summary>
        public bool DepartmentAccount;
        /// <summary>
        /// ���˺���
        /// </summary>
        public bool PersonAccount;        
        /// <summary>
        /// �ͻ���ϵ����
        /// </summary>
        public bool ClientAccount;        
        /// <summary>
        /// ��Ӧ��ϵ����
        /// </summary>
        public bool SupplierAccount;
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        public bool ProjectAccount;
        /// <summary>
        /// �����˹���
        /// </summary>
        public bool EndorsementManage;
        /// <summary>
        /// Ԥ�����
        /// </summary>
        public bool BudgetManage;
        /// <summary>
        /// �ֽ��ռ���
        /// </summary>
        public bool CashDailyAcc;
        /// <summary>
        /// ���д���ռ���
        /// </summary>
        public bool BankDailyAcc;
        /// <summary>
        /// �ʽ��Ŀ
        /// </summary>
        public bool CapitalAcc;
        /// <summary>
        /// ̨��
        /// </summary>
        public bool DeskAcc;
    }
}
