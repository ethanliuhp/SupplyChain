using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Application.Business.Erp.SupplyChain.ProductionManagement.Service;
using Application.Business.Erp.SupplyChain.Client.Basic.CommonClass;
using Application.Business.Erp.SupplyChain.ProductionManagement.LaborSporadicManagement.Domain;
using Application.Business.Erp.SupplyChain.Client.StockManage.StockInManage.StockInUI;
using CommonSearch.Basic.CommonClass;
using VirtualMachine.Component.Util;
using System.Collections;
using Application.Resource.MaterialResource.Domain;
using Application.Business.Erp.SupplyChain.WasteMaterialManage.Service;
using Application.Business.Erp.SupplyChain.ProjectDocumentManage.Service;

namespace Application.Business.Erp.SupplyChain.Client.ProductionManagement
{
    public enum EnumLaborType
    {
        /// <summary>
        /// 需求总计划查询
        /// </summary>
        laborSearch,
        /// <summary>
        /// 代工 单据
        /// </summary>
        代工,
        /// <summary>
        /// 派工 单据
        /// </summary>
        派工,
        ///// <summary>
        ///// 代工核算 单据
        ///// </summary>
        //代工核算,
        ///// <summary>
        ///// 派工核算 单据
        ///// </summary>
        // 派工核算
        分包签证,
        计时派工,
        /// <summary>
        /// 逐日派工单维护[计划值]
        /// </summary>
        逐日派工
    }

    public class MLaborSporadicMng
    {
        private ILaborSporadicSrv laborSporadicSrv;
        private static IProObjectRelaDocumentSrv modelDoc;
        public ILaborSporadicSrv LaborSporadicSrv
        {
            get { return laborSporadicSrv; }
            set { laborSporadicSrv = value; }
        }

        public MLaborSporadicMng()
        {
            if (laborSporadicSrv == null)
            {
                laborSporadicSrv = StaticMethod.GetService("LaborSporadicSrv") as ILaborSporadicSrv;
            }
            if (modelDoc == null)
                modelDoc = ConstMethod.GetService("ProObjectRelaDocumentSrv") as IProObjectRelaDocumentSrv;
        }

        #region 零星用工单
        /// <summary>
        /// 保存零星用工单
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public LaborSporadicMaster SaveLaborSporadicMaster(LaborSporadicMaster obj)
        {
            return laborSporadicSrv.SaveLaborSporadic(obj);
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

        #endregion
    }
}
