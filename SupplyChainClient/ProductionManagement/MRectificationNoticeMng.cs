using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.RectificationNoticeMng.Domain;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement
{
    public class MRectificationNoticeMng
    {
        private IRectificationNoticeSrv rectificationNoticeSrv;

        public IRectificationNoticeSrv RectificationNoticeSrv
        {
            get { return rectificationNoticeSrv; }
            set { rectificationNoticeSrv = value; }
        }
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;
        public MRectificationNoticeMng()
        {
            if (rectificationNoticeSrv == null)
            {
                rectificationNoticeSrv = StaticMethod.GetService("RectificationNoticeSrv") as IRectificationNoticeSrv;
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
        #region 整改通知单

        /// <summary>
        /// 保存整改通知单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public RectificationNoticeMaster SaveRectificationNotice(RectificationNoticeMaster obj)
        {
            return rectificationNoticeSrv.SaveRectificationNotice(obj);
        }
        public RectificationNoticeDetail SaveRectificationNotice(RectificationNoticeDetail obj)
        {
            return rectificationNoticeSrv.SaveRectificationNotice(obj);
        }
        #endregion
    }
}
