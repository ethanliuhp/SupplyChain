using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Base.Service;
using System.Collections;
using VirtualMachine.Core;
using System.Data;
using Application.Business.Erp.SupplyChain.EngineerManage.DocApprovalManage.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Domain;

namespace Application.Business.Erp.SupplyChain.EngineerManage.DocApprovalManage.Service
{
        // <summary>
        /// 工程文档审批管理
        /// </summary>
    public interface IDocApprovalMngSrv : IBaseService
        {
            #region 工程文档审批管理
        DocApprovalMng GetImpById(string id);
        DocApprovalMng GetImpByCode(string code);
        IList GetImp(ObjectQuery objectQuery);
        DocApprovalMng SaveImp(DocApprovalMng obj);
        DataSet DocApprovalQuery(string condition);
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

    
        }
    }

