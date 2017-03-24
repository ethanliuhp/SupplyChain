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
    /// 账表查询公用服务
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
        /// 校验当前日期是否有效
        /// </summary>
        /// <param name="inDate">日期</param>
        /// <returns>bool</returns>
        virtual public bool VerifyDate(DateTime inDate)
        {
            bool isValid = false;
            Login nowInfo = CallContextUtil.LogicalGetData<Login>("LoginInformation");
            
            //校验日期是否小于初始日期
            int initYandM = nowInfo.TheComponentPeriod.InitialYAndM;
            int inYandM = inDate.Year * 100 + inDate.Month;
            if (inYandM >= initYandM)
            {
                isValid = true;
            }
            return isValid;
        }

        /// <summary>
        /// 校验当前日期是否有效
        /// </summary>
        /// <param name="inYear">当前年</param>
        /// <param name="inMonth">当前月</param>
        /// <returns>bool</returns>
        virtual public bool VerifyDate(int inYear, int inMonth)
        {
            bool isValid = false;
            Login nowInfo = CallContextUtil.LogicalGetData<Login>("LoginInformation");

            //校验日期是否小于初始日期
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
