using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using Application.Business.Erp.Financial.BasicAccount.AssisAccount.Domain;
using System.Collections;
using NHibernate.Criterion;
using NHibernate;

namespace Application.Business.Erp.Financial.BasicAccount.AssisAccount.Service
{
    public class DeskAccontSrv : IDeskAccontSrv
    {
        private IDao dao;
        virtual public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        /// <summary>
        /// 获取台账类别信息
        /// </summary>
        /// <returns></returns>
        virtual public IList GetAllAccDeskSimple()
        {
            Disjunction disJun = new Disjunction();
            disJun.Add(Expression.Eq("State", 1));
            disJun.Add(Expression.Eq("Id", 0L));

            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(disJun);
            
            oq.AddOrder(Order.Asc("Id"));
            IList lsDesk = dao.ObjectQuery(typeof(DeskAccount), oq);
            return lsDesk; 
        }

        /// <summary>
        /// 显示台账类别
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        virtual public IList GetDetail(ObjectQuery oq)
        {
            //return dao.ObjectQuery(typeof(DeskAccDetails), oq);
            return null;
        }

        /// <summary>
        /// 显示所有已定义台账
        /// </summary>
        /// <returns></returns>
        virtual public IList GetAccDeskAll()
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("State", 1));
            oq.AddOrder(Order.Asc("Id"));
            oq.AddFetchMode("Details", FetchMode.Eager);
            IList lsDesk = dao.ObjectQuery(typeof(DeskAccount), oq);
            return lsDesk;             
        }

        /// <summary>
        /// 根据ID查询信息
        /// </summary>
        /// <param name="deskId"></param>
        /// <returns></returns>
        virtual public DeskAccount GetDesk(long deskId)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("Id", deskId));
            oq.AddOrder(Order.Asc("Id"));
            oq.AddFetchMode("Details", FetchMode.Eager);
            IList ls = dao.ObjectQuery(typeof(DeskAccount), oq);
            DeskAccount outDesk = null;
            if (ls.Count.Equals(1))
            {
                outDesk = ls[0] as DeskAccount;
            }
            return outDesk;
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="desk"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool SaveDesk(DeskAccount desk)
        {
            bool isok = false;
            try
            {
                dao.Save(desk);
            }
            catch
            {
                throw;
            }
            return isok;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="desk"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool UpdDesk(DeskAccount desk)
        {
            bool isok = false;
            try
            {
                isok = dao.Update(desk);
            }
            catch
            {
                throw;
            }
            return isok;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deskId"></param>
        /// <returns></returns>
        [TransManager]
        virtual public bool delDesk(long deskId)
        {
            try
            {
                DeskAccount deskAccId = null;
                deskAccId = dao.Get(typeof(DeskAccount), deskId) as DeskAccount;
                deskAccId.State = 0;
                dao.Update(deskAccId);
                return true;
            }
            catch
            {
                throw;
            }
        }
    }


}
