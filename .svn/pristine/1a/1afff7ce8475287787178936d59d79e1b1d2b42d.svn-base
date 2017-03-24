using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Service;
using Application.Business.Erp.SupplyChain.ProductionManagement.ProfessionInspectionRecord.Domain;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement
{
    public class MProInsRecordMng
    {
        private IProfessionInspectionSrv professionInspectionSrv;

        public IProfessionInspectionSrv ProfessionInspectionSrv
        {
            get { return professionInspectionSrv; }
            set { professionInspectionSrv = value; }
        }
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;

        public MProInsRecordMng()
        {
            if (professionInspectionSrv == null)
            {
                professionInspectionSrv = StaticMethod.GetService("ProfessionInspectionSrv") as IProfessionInspectionSrv;
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

        #region 专业检查记录
        /// <summary>
        /// 保存专业检查记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ProfessionInspectionRecordMaster SaveProfessionInspectionMaster(ProfessionInspectionRecordMaster obj)
        {
            return professionInspectionSrv.SaveProfessionInspectionRecordPlan(obj);
        }

        #endregion
    }
}
