using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Domain;
using System.Data;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;

namespace Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Service
{
    /// <summary>
    /// 目标责任书服务
    /// </summary>
    public interface ITargetRespBookSrc : IBaseService
    {
        #region 目标责任书
        /// <summary>
        /// 通过ID查询目标责任书信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TargetRespBook GetTargetRespBookById(string id);
        /// <summary>
        /// 通过Code查询目标责任书信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        TargetRespBook GetWTargetRespBookByCode(string code);
        /// <summary>
        /// 查询目标责任书信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetTargetRespBook(ObjectQuery objectQuery);
        /// <summary>
        /// 保存目标责任书
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        TargetRespBook saveTargetRespBook(TargetRespBook obj);
        /// <summary>
        /// 查询目标节点信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetTargetProgressNode(ObjectQuery objectQuery);
        /// <summary>
        /// 查询风险抵押金缴纳记录信息
        /// </summary>
        /// <param name="objectQuery"></param>
        /// <returns></returns>
        IList GetIrpRiskDepositPayRecord(ObjectQuery objectQuery);
         /// <summary>
        /// 采购合同信息查询
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        IList GetPersonOnJob(ObjectQuery objectQuery);
        DataSet PersonInfoQuery(string condition);
        #endregion
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
        ProObjectRelaDocument SaveOrUpdate(ProObjectRelaDocument
item);
        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        IList ObjectQuery(Type entityType, ObjectQuery oq);
        DataSet GetProjectInfo(string condition);
    }
}
