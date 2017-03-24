using System;
using Application.Business.Erp.Financial.InitialData.Domain;
using System.Collections;
using VirtualMachine.Core;

namespace Application.Business.Erp.Financial.InitialData.Service
{
    /// <summary>
    /// 科目级别Service
    /// </summary>
    public interface IAccountLevelSrv
    {
        /// <summary>
        /// 删除科目级别长度
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool DeleteData(AccountLevel level);
        
        /// <summary>
        /// 获取科目级别
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        IList QueryData(ObjectQuery oq);

        /// <summary>
        /// 科目级别存盘
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        bool SaveData(AccountLevel level);

        /// <summary>
        /// 根据ID获取科目级别
        /// </summary>
        /// <param name="levId"></param>
        /// <returns></returns>
        AccountLevel GetAccLevelById(long levId);
        int GetUsedMaxAccLevel();
    }
}
