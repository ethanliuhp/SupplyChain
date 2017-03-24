using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using Application.Business.Erp.Financial.InitialData.Domain;
using System.Collections;
using NHibernate.Criterion;
using Application.Business.Erp.Financial.BasicAccount.InitialSetting.Domain;

namespace Application.Business.Erp.Financial.InitialData.Service
{
    /// <summary>
    /// 科目级别Service
    /// </summary>
    public class AccountLevelImpl : IAccountLevelSrv
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        /// <summary>
        /// 存盘
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool SaveData(AccountLevel level)
        {
            return dao.SaveOrUpdate(level);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool DeleteData(AccountLevel level)
        {
            return dao.Delete(level);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        virtual public IList QueryData(ObjectQuery oq)
        {
            return dao.ObjectQuery(typeof(AccountLevel),oq);
        }

        /// <summary>
        /// 根据ID获取科目级别
        /// </summary>
        /// <param name="levId"></param>
        /// <returns></returns>
        virtual public AccountLevel GetAccLevelById(long levId)
        {
            AccountLevel accLev = Dao.Get(typeof(AccountLevel), levId) as AccountLevel;
            return accLev;
        }

        virtual public int GetUsedMaxAccLevel()
        {
            ObjectQuery query = new ObjectQuery();
            query.AddCriterion(Expression.Eq("State", 1));
            return (int)Dao.Max(typeof(AccountTitle), "Level", query);
        }
    }
}
