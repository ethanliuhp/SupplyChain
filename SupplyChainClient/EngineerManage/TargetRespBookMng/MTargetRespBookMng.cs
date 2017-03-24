using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.EngineerManage.TargetRespBookManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using CommonSearch.Basic.CommonClass;

namespace Application.Business.Erp.SupplyChain.EngineerManage.Client.TargetRespBookMng
{
    public class MTargetRespBookMng
    {
        private ITargetRespBookSrc targetRespBookSrc;

        public ITargetRespBookSrc TargetRespBookSrc
        {
            get { return targetRespBookSrc; }
            set { targetRespBookSrc = value; }
        }

        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;

        public MTargetRespBookMng()
        {
            if (TargetRespBookSrc == null)
            {
                TargetRespBookSrc = StaticMethod.GetService("TargetRespBookSrc") as ITargetRespBookSrc;
            }
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
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
