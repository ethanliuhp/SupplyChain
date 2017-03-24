using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using Application.Business.Erp.SupplyChain.CompleteSettlementBook.Domain;
using VirtualMachine.Core;
using System.Collections;
using System.Data;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;


namespace Application.Business.Erp.SupplyChain.CompleteSettlementBook.Service
{
    /// <summary>
    ///竣工结算服务
    /// </summary>
    public interface IComplete:IBaseService
    {

        #region 竣工表信息
        /// <summary>
        /// 通过ID查询竣工表信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CompleteInfo GetCompleteById(string id);
        /// <summary>
        /// 通过Code查询竣工表信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        CompleteInfo GetCompleteByCode(string code);
        /// <summary>
        /// 查询竣工表信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetComplete(ObjectQuery objectQuery);
        /// <summary>
        /// 竣工表查询
        /// </summary>
        CompleteInfo SaveComplete(CompleteInfo obj);
        #endregion
        DataSet CompleteRelationQuery(string condition);
        /// <summary>
        /// 删除对象集合
        /// </summary>
        /// <param name="list">对象集合</param>
        /// <returns></returns>
        [TransManager]
        bool Delete(IList list);
        /// <summary>
        /// 保存或修改工程对象关联文档
        /// </summary>
        /// <param name="item">工程对象关联文档</param>
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
