using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.EngineerManage.DocApprovalManage.Domain;
using Application.Business.Erp.SupplyChain.EngineerManage.DocApprovalManage.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using System.Collections;
using VirtualMachine.Core;
using CommonSearch.Basic.CommonClass;


namespace Application.Business.Erp.SupplyChain.Client.EngineerManage.DocumentsApprovalMng
{

    public class MDocApprovalMng
    {

        private IDocApprovalMngSrv docApprovalMngSrv;

        public IDocApprovalMngSrv DocApprovalMngSrv
        {
            get { return docApprovalMngSrv; }
            set { docApprovalMngSrv = value; }
        }

        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;

        public MDocApprovalMng()
        {
            if (DocApprovalMngSrv == null)
            {
                docApprovalMngSrv = StaticMethod.GetService("DocApprovalMngSrv") as IDocApprovalMngSrv;
            }
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
        }

        /// <summary>
        /// 保存工程文档审批信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DocApprovalMng saveImp(DocApprovalMng obj)
        {
            return docApprovalMngSrv.SaveImp(obj);
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

