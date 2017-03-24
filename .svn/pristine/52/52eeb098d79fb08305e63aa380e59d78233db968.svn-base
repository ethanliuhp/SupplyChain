using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.PenaltyDeductionManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement
{
    public class MPenaltyDeductionMng
    {
        private IPenaltyDeductionSrv penaltyDeductionSrv;
        public IPenaltyDeductionSrv PenaltyDeductionSrv
        {
            get { return penaltyDeductionSrv; }
            set { penaltyDeductionSrv = value; }
        }
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;

        public MPenaltyDeductionMng()
        {
            if (penaltyDeductionSrv == null)
            {
                penaltyDeductionSrv = StaticMethod.GetService("PenaltyDeductionSrv") as IPenaltyDeductionSrv;
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

        #region 罚扣款单
        /// <summary>
        /// 保存罚扣款单信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public PenaltyDeductionMaster SavePenaltyDeduction(PenaltyDeductionMaster obj)
        {
            return penaltyDeductionSrv.SavePenaltyDeduction(obj);
        }

        #endregion

        //#region 料具收料
        //public MaterialCollectionMaster SaveMaterialCollection(MaterialCollectionMaster obj, IList list)
        //{
        //    //return matMngSrv.SaveMaterialCollectionMaster(obj, list);
        //}
        //#endregion
    }
}
