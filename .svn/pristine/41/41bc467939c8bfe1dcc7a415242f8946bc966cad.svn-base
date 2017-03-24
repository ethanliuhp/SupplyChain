using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.MaterialManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalOrder.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialCollectionMng.Domain;
using System.Collections;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalSettlementMng.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Domain;
using Application.Business.Erp.SupplyChain.SettlementManagement.MaterialSettleMng.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using VirtualMachine.Core;
using Application.Business.Erp.SupplyChain.MaterialManage.MaterialRentalContractMng.Domain;

namespace Application.Business.Erp.SupplyChain.Client.MaterialRentalMange
{
    public class MMatRentalMng
    {
        private IMatMngSrv matMngSrv;
        public IMatMngSrv MatMngSrv
        {
            get { return matMngSrv; }
            set { matMngSrv = value; }
        }

        private IMaterialSettleSrv materialSettleSrv;
        public IMaterialSettleSrv MaterialSettleSrv
        {
            get { return materialSettleSrv; }
            set { materialSettleSrv = value; }
        }
       
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;
        public MMatRentalMng()
        {
            if (matMngSrv == null)
            {
                matMngSrv = StaticMethod.GetService("MatMngSrv") as IMatMngSrv;
            }

            if (materialSettleSrv == null)
            {
                materialSettleSrv = StaticMethod.GetService("MaterialSettleSrv") as IMaterialSettleSrv;
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

        #region 料具租赁合同
        /// <summary>
        /// 保存料具租赁合同
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MaterialRentalOrderMaster SaveMaterialRentalOrder(MaterialRentalOrderMaster obj)
        {
            return matMngSrv.SaveMaterialRentalOrderMaster(obj);
        }

        #endregion

        #region 料具收料
        public MaterialCollectionMaster SaveMaterialCollection(MaterialCollectionMaster obj)
        {
            return matMngSrv.SaveMaterialCollectionMaster(obj);
        }
        #endregion

        #region 设备租赁结算单
        /// <summary>
        /// 保存设备租赁结算单信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MaterialRentalSettlementMaster SaveMaterialRentalSettlement(MaterialRentalSettlementMaster obj)
        {
            return matMngSrv.SaveMaterialRentalSettlement(obj);
        }
        #endregion

        #region 设备租赁合同单
        /// <summary>
        /// 保存设备租赁合同单信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public MaterialRentalContractMaster SaveMaterialRentalContract(MaterialRentalContractMaster obj)
        {
            return matMngSrv.SaveMaterialRentalContract(obj);
        }
        #endregion
    }
}
