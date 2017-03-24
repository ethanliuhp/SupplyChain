using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialManage.Service;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatApplyMng.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.WasteMatProcessMng.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using System.Collections;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;

namespace Application.Business.Erp.SupplyChain.Client.WasteMaterialMng
{
    public class MWasteMaterialMng
    {
        private IWasteMatSrv wasteMatSrv;
        public IWasteMatSrv WasteMatSrv
        {
            get { return wasteMatSrv; }
            set { wasteMatSrv = value; }
        }
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;
        public MWasteMaterialMng()
        {
            if (wasteMatSrv == null)
                wasteMatSrv = StaticMethod.GetService("WasteMatSrv") as IWasteMatSrv;
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
            if (model == null)
                model = ConstMethod.GetService("PBSTreeSrv") as IPBSTreeSrv;
        }
        /// <summary>
        /// 保存废旧物资申请信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public WasteMatApplyMaster saveWasteMatApply(WasteMatApplyMaster obj)
        {
            return wasteMatSrv.saveWasteMatApply(obj);
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
