using System;
using System.Collections;
using VirtualMachine.Core;
namespace Application.Business.Erp.SupplyChain.Base.Service
{
    public interface IBaseBasicDataSrv
    {
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        bool Delete(System.Collections.IList lst);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        bool Delete(object obj);
        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        System.Collections.IList Save(System.Collections.IList lst);
        /// <summary>
        /// 新增保存
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        object Save(object obj);
        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        object Update(object obj);
        /// <summary>
        /// 修改保存
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        System.Collections.IList Update(System.Collections.IList lst);
        /// <summary>
        /// 查询制定类型的所有的数据
        /// </summary>
        /// <param name="aType"></param>
        /// <returns></returns>
        IList GetObjects(Type aType);
        /// <summary>
        /// 按条件查询制定类型的数据
        /// </summary>
        /// <param name="aType">类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns>查询结果IList</returns>
        IList GetObjects(Type aType, ObjectQuery oq);
    }
}
