using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using VirtualMachine.Core;
using System.Data;
using Application.Business.Erp.SupplyChain.EngineerManage.CollectionManage.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;
namespace Application.Business.Erp.SupplyChain.EngineerManage.CollectionManage.Service
{


    /// <summary>
    /// 收发函管理服务
    /// </summary>
    public interface ICollectionMngSrv : IBaseService
    {
        #region 收发函管理
        ColletionManage GetImpById(string id);
        ColletionManage GetImpByCode(string code);
        IList GetImp(ObjectQuery objectQuery);
        ColletionManage saveImp(ColletionManage obj);
        //DataSet ImplementationQuery(string condition);
        #endregion     
        DataSet CollectionQuery(string condition);
        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        bool Delete(IList list);
        /// <summary>
        /// 保存或修改收发邀请函
        /// </summary>
        /// <param name="item">收发邀请函</param>
        /// <returns></returns>
        [TransManager]
        ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument item);
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);
    }
}

