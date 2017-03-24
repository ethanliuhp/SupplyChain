using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.SupplyManage.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Domain;
using Application.Business.Erp.SupplyChain.CostManagement.ContractExcuteMng.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;
using Application.Business.Erp.SupplyChain.CostManagement.PBS.Service;
using VirtualMachine.Core;

namespace Application.Business.Erp.SupplyChain.Client.CostManagement.ContractExcuteMng
{
    public class MContractExcuteMng
    {
        private IContractExcuteSrv contractExcuteSrv;

        public IContractExcuteSrv ContractExcuteSrv
        {
            get { return contractExcuteSrv; }
            set { contractExcuteSrv = value; }
        }
        private static IProObjectRelaDocumentSrv modelDoc;
        private static IPBSTreeSrv model;
        public MContractExcuteMng()
        {
            if (contractExcuteSrv == null)
            {
                contractExcuteSrv = StaticMethod.GetService("ContractExcuteSrv") as IContractExcuteSrv;
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
        #region 分包项目
        /// <summary>
        /// 保存分包项目
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public SubContractProject SaveContracExcuteMaster(SubContractProject obj)
        {
            return contractExcuteSrv.SaveContractExcute(obj);
        }
        #endregion
    }
}
