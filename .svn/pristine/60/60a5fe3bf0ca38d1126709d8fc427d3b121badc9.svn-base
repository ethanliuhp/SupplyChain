using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.CompleteSettlementBook.Service;
using Application.Business.Erp.SupplyChain.CompleteSettlementBook.Domain;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using System.Collections;
using VirtualMachine.Core;
using CommonSearch.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.CompleteSettlementBook
{
     public class MCompleteMng
    {
         private IComplete completeSrv;
         public IComplete CompleteSrv
         {
             get { return completeSrv; }
             set { completeSrv = value; }
         }
         private static IProObjectRelaDocumentSrv modelDoc;
         private static IPBSTreeSrv model;
        public MCompleteMng()
        {
            if (CompleteSrv == null)
            {
                CompleteSrv = StaticMethod.GetService("CompleteSrv") as IComplete;
            }
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
        }
        ///// <summary>
        ///// 保存竣工表信息
        ///// </summary>
        ///// <param name="obj"></param>
        ///// <returns></returns>
        public CompleteInfo SaveComplete(CompleteInfo obj)
        {
            return CompleteSrv.SaveComplete(obj);
        }
        /// <summary>
        /// 删除工程对象关联文档对象集合
        /// </summary>
        /// <param name="list">工程对象关联文档对象集合</param>
        /// <returns></returns>
        public bool DeleteProObjRelaDoc(IList list)
        {
            return modelDoc.DeleteProObjRelaDoc(list);
        }

        /// <summary>
        /// 对象查询
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="oq">查询条件</param>
        /// <returns></returns>
        public IList ObjectQuery(Type entityType, ObjectQuery oq)
        {
            return model.ObjectQuery(entityType, oq);
        }
    }
}
