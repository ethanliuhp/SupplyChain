using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.BasicData.Domain;
using System.Collections;
using NHibernate.Criterion;
namespace Application.Business.Erp.SupplyChain.BasicData.Service
{
    public class BasicDataService : Application.Business.Erp.SupplyChain.BasicData.Service.IBasicDataService
    {
        private IDao dao;
        public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public BasicDatas SaveBasicDatas(BasicDatas obj)
        {
            //Dao.Save(obj);
            Dao.SaveOrUpdate(obj);
            return obj; 
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="deletedDetail"></param>
        /// <returns></returns>
        [TransManager]
        public BasicDatas SaveBasicDatas(BasicDatas obj, IList deletedDetail)
        {
            Dao.SaveOrUpdate(obj);
            foreach (BasicDataDetail detail in deletedDetail)
            {
                Dao.Delete(detail);
            }
            return obj;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteBasicDatas(BasicDatas obj)
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IList ListAllBasicDatas()
        {
            ObjectQuery oq = new ObjectQuery();
            //oq.AddOrder(new Expression.Order("Id",true));
            return Dao.ObjectQuery(typeof(BasicDatas), oq);
        }

        /// <summary>
        /// 保存明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public BasicDataDetail SaveBasicDataDetail(BasicDataDetail obj)
        {
            Dao.SaveOrUpdate(obj);
            return obj;
        }

        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TransManager]
        public bool DeleteBasicDataDetail(BasicDataDetail obj)
        {
            return Dao.Delete(obj);
        }

        /// <summary>
        /// 根据主表查询明细
        /// </summary>
        /// <param name="bd"></param>
        /// <returns></returns>
        public IList GetDetailByMaster(BasicDatas bd)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BasicData.Id",bd.Id));
            oq.AddOrder(new Order("Id",true));
            return Dao.ObjectQuery(typeof(BasicDataDetail),oq);
        }

        /// <summary>
        /// 根据表名查询明细
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IList GetDetailByBasicTableName(string name)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("BasicData.Name", name));
            oq.AddOrder(new Order("Id", true));
            return Dao.ObjectQuery(typeof(BasicDataDetail), oq);
        }
    }
}
