using System;
using System.Collections.Generic;
using System.Text;
using VirtualMachine.Core;
using System.Collections;

namespace Application.Business.Erp.SupplyChain.Base.Service
{
    public class BaseBasicDataSrv : Application.Business.Erp.SupplyChain.Base.Service.IBaseBasicDataSrv
    {
        private IDao _Dao;
        virtual public IDao Dao
        {
            get { return _Dao; }
            set { _Dao = value; }
        }
        /// <summary>
        /// 保存新增数据
        /// </summary>
        /// <param name="obj">要保存的对象</param>
        /// <returns>保存后的对象</returns>
        [TransManager]
        virtual public Object Save(Object obj)
        {
            Dao.Save(obj);
            return obj;
        }
        /// <summary>
        /// 保存新增数据
        /// </summary>
        /// <param name="lst">要保存的对象IList</param>
        /// <returns>保存后的IList</returns>
        [TransManager]
        virtual public IList Save(IList lst)
        {
            Dao.SaveOrUpdate(lst);
            return lst;
        } 	

        /// <summary>
        /// 保存要修改的数据
        /// </summary>
        /// <param name="obj">要修改的对象</param>
        /// <returns>修改后的对象</returns>
        [TransManager]
        virtual public Object Update(Object obj)
        {
            Dao.Update(obj);
            return obj;
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="lst">要修改的对象IList</param>
        /// <returns>修改后的对象IList</returns>
        [TransManager]
        virtual public IList Update(IList lst)
        {
            Dao.Update(lst);
            return lst;
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="lst">要删除的对象IList</param>
        /// <returns>True 删除成功，False 删除失败</returns>
        [TransManager]
        virtual public bool Delete(IList lst)
        {
            Dao.Delete(lst);
            return true;
        } 	

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="obj">要删除的对象</param>
        /// <returns>True 删除成功，False 删除失败</returns>
        virtual public bool Delete(Object obj)
        {
            Dao.Delete(obj);
            return true;
        }
        /// <summary>
        /// 查询制定类型的所有的数据
        /// </summary>
        /// <param name="aType"></param>
        /// <returns></returns>
        virtual public IList GetObjects(Type aType)
        {
            return GetObjects(aType, new ObjectQuery());
        }
        /// <summary>
        /// 按条件查询制定类型的数据
        /// </summary>
        /// <param name="aType">类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns>查询结果IList</returns>
        virtual public IList GetObjects(Type aType, ObjectQuery oq)
        {
            IList lstReturn = new ArrayList();
            lstReturn = Dao.ObjectQuery(aType, oq);
            return lstReturn;
        }
 	
    }
}
