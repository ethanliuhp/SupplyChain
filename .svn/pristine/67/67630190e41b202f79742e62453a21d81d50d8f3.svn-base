using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.SupplyManage.SupplyOrderManage.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.SupplyMng
{
    public enum EnumSupplyType
    {
        /// <summary>
        /// 采购合同查询
        /// </summary>
        supplySearch,
        /// <summary>
        /// 土建 单据
        /// </summary>
        土建,
        /// <summary>
        /// 安装 单据
        /// </summary>
        安装,

    }
    public class MSupplyOrderMng
    {
        private ISupplyOrderSrv supplyOrderSrv;

        public ISupplyOrderSrv SupplyOrderSrv
        {
            get { return supplyOrderSrv; }
            set { supplyOrderSrv = value; }
        }
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;
        public MSupplyOrderMng()
        {
            if (supplyOrderSrv == null)
                supplyOrderSrv = StaticMethod.GetService("SupplyOrderSrv") as ISupplyOrderSrv;
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
        }

        #region 采购合同
        /// <summary>
        /// 保存采购合同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SupplyOrderMaster SaveSupplyOrderMaster(SupplyOrderMaster obj)
        {
            return supplyOrderSrv.SaveSupplyOrder(obj);
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
        #endregion

    }
}
