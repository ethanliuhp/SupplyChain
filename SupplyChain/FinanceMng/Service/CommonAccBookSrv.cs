using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using VirtualMachine.Core.DataAccess;
using Application.Resource.CommonClass.Domain;
using VirtualMachine.Component.Util;

namespace Application.Business.Erp.Financial.GlobalInfo
{
    /// <summary>
    /// �˱��ѯ���÷���
    /// </summary>
    public class CommonAccBookSrv : ICommonAccBookSrv
    {
        private IDataAccess dbDao;
        virtual public IDataAccess DBDao
        {
            get { return dbDao; }
            set { dbDao = value; }
        }

        /// <summary>
        /// У�鵱ǰ�����Ƿ���Ч
        /// </summary>
        /// <param name="inDate">����</param>
        /// <returns>bool</returns>
        virtual public bool VerifyDate(DateTime inDate)
        {
            bool isValid = false;
            Login nowInfo = CallContextUtil.LogicalGetData<Login>("LoginInformation");
            
            //У�������Ƿ�С�ڳ�ʼ����
            int initYandM = nowInfo.TheComponentPeriod.InitialYAndM;
            int inYandM = inDate.Year * 100 + inDate.Month;
            if (inYandM >= initYandM)
            {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// У�鵱ǰ�����Ƿ���Ч
        /// </summary>
        /// <param name="inYear">��ǰ��</param>
        /// <param name="inMonth">��ǰ��</param>
        /// <returns>bool</returns>
        virtual public bool VerifyDate(int inYear, int inMonth)
        {
            bool isValid = false;
            Login nowInfo = CallContextUtil.LogicalGetData<Login>("LoginInformation");

            //У�������Ƿ�С�ڳ�ʼ����
            int initYandM = nowInfo.TheComponentPeriod.InitialYAndM;
            int inYandM = inYear * 100 + inMonth;
            if (inYandM >= initYandM)
            {
                isValid = true;
            }
            return isValid;
        }
        
    }
}
