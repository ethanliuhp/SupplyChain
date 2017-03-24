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
        /// �޸Ŀ�����ʵ���Ŀ�������
        /// </summary>
        /// <param name="staCat">���Id</param>
        /// <param name="mngStaId">����ʵ��Id</param>
        /// <param name="quantity">Ҫ�޸ĵ���(������������,�����������)</param>
        /// <returns>�޸ĺ�Ŀ�����ʵ��</returns>
        [TransManager]
        public StockQuantity UpdateStkMngStateUsableQuantity(string staCat, string mngStaId, decimal quantity)
        {
            //�ȸ���id���ҹ���ʵ��
            ObjectQuery oq = new ObjectQuery();
            oq.AddCriterion(Expression.Eq("TheStaCat.Id", staCat));
            oq.AddCriterion(Expression.Eq("TheMngState.Id", mngStaId));

            StockQuantity theManageState = new StockQuantity();
            IList lst = dao.ObjectQuery(typeof(StockQuantity), oq);
            if (lst == null || lst.Count==0)
            {
                throw new Exception("�޴˿��Ĺ���ʵ�����ݣ�");
            }
            theManageState = lst[0] as StockQuantity;
            //�����Ҫ�޸ĵ���С��0,��ȽϹ���ʵ����ǰ�Ŀ��������޸ĵ���,�ж��Ƿ񹻼�
            if (quantity < 0)
            {
                if (theManageState.UseableQuantity < Math.Abs(quantity))
                {
                    throw new Exception("�˹���ʵ����ǰ�Ŀ��ÿ������[" + theManageState.UseableQuantity.ToString() + "],Ҫ�޸ĵ���Ϊ[" + Convert.ToString(Math.Abs(quantity)) + "]");
                }
            }
            theManageState.UseableQuantity += quantity;
            dao.Update(theManageState);
            
            return theManageState;
        }
        /// <summary>
        /// ��ѯ����ʵ��
        /// </summary>
        /// <param name="oq"></param>
        /// <returns></returns>
        public IList GetStkMngState(ObjectQuery oq)
        {
            return FindStkMngState(oq);
        }
        /// <summary>
        /// ���ҹ���ʵ���Ŀ�����>0ʵ��
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
