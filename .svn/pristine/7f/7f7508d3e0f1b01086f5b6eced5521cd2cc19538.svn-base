using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.StockManage.Stock.Domain;
using NHibernate.Exceptions;
using NHibernate.Criterion;

namespace Application.Business.Erp.SupplyChain.StockManage.Stock.Service
{
    public class StockQuantitySrv : Application.Business.Erp.SupplyChain.StockManage.Stock.Service.IStockQuantitySrv
    {
        private IDao dao;
        virtual public IDao Dao
        {
            get { return dao; }
            set { dao = value; }
        }

        virtual public IList GetStkMngStateByMaterial(string id)
        {
            return this.GetStkMngStateByMaterialId(id);
        }
        virtual public IList GetStkMngStateByMaterial(Material theMaterial)
        {
            return this.GetStkMngStateByMaterialId(theMaterial.Id);
        }

        private IList GetStkMngStateByMaterialId(string id)
        {
            ObjectQuery oq = new ObjectQuery();
            oq.AddFetchMode("TheMngState.Material", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheMngState.Supplier", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheStaCat", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheMngState.OManageState.Supplier", NHibernate.FetchMode.Eager);
            oq.AddCriterion(Expression.Eq("TheMngState.Material.Id", id));
            oq.AddCriterion(Expression.Gt("UseableQuantity", 0));
            IList listReturn = dao.ObjectQuery(typeof(StockQuantity), oq);
            foreach (StockQuantity var in listReturn)
            {
                if(var.TheMngState.OManageState!=null)
                    dao.InitializeObj(var.TheMngState.OManageState.Supplier);
            }
            return listReturn;
        }
        /// <summary>
        /// 修改库存管理实例的可用数量
        /// </summary>
        /// <param name="staCat">库存Id</param>
        /// <param name="mngStaId">管理实例Id</param>
        /// <param name="quantity">要修改的量(正数代表增加,负数代表减少)</param>
        /// <returns>修改后的库存管理实例</returns>
        [TransManager]
        public StockQuantity UpdateStkMngStateUsableQuantity(string staCat, string mngStaId, decimal quantity)
        {
            //先根据id查找管理实例
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheStaCat.Id", staCat));
            oq.AddCriterion(Expression.Eq("TheMngState.Id", mngStaId));

            StockQuantity theManageState = new StockQuantity();
            IList lst = dao.ObjectQuery(typeof(StockQuantity), oq);
            if (lst == null || lst.Count==0)
            {
                throw new Exception("无此库存的管理实例数据！");
            }
            theManageState = lst[0] as StockQuantity;
            //如果需要修改的量小于0,则比较管理实例当前的可用量与修改的量,判断是否够减
            if (quantity < 0)
            {
                if (theManageState.UseableQuantity < Math.Abs(quantity))
                {
                    throw new Exception("此管理实例当前的可用库存量是[" + theManageState.UseableQuantity.ToString() + "],要修改的量为[" + Convert.ToString(Math.Abs(quantity)) + "]");
                }
            }
            theManageState.UseableQuantity += quantity;
            dao.Update(theManageState);
            
            return theManageState;
        }
        /// <summary>
        /// 查询管理实例
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetStkMngState(ObjectQuery oq)
        {
            return FindStkMngState(oq);
        }
        /// <summary>
        /// 查找管理实例的可用量>0实例
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetStkMngStateUsableQuantity(ObjectQuery oq)
        {
            oq.AddCriterion(Expression.Gt("UseableQuantity", 0));
            return FindStkMngState(oq);
        }
        private IList FindStkMngState(ObjectQuery oq)
        {
            IList lstReturn = new ArrayList();
            oq.AddFetchMode("TheMngState.Material", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheStaCat", NHibernate.FetchMode.Eager);
            oq.AddFetchMode("TheMngState.Supplier", NHibernate.FetchMode.Eager);
            lstReturn = Dao.ObjectQuery(typeof(StockQuantity), oq);
            return lstReturn;
        }        

    }
}
